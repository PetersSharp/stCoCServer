using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using stCoreUI;
using stClient;
using System.Globalization;
using System.Drawing;

namespace stCoCClient
{
    public partial class ClientForm : Form
    {
        public Image ImageInformer = null;
        private int _oldNotifyItems = 0;
        private DataTable _dtnotify = null,
                          _dtclan = null;

        #region control events

        private void TimerRunNotify_Tick(object sender, EventArgs e)
        {
            if (sender == this.TimerRunNotify)
            {
                this.RunNotify();
            }
        }
        private void TSMINotifyRenew_Click(object sender, EventArgs e)
        {
            this.PBNotifyRenew_Click(sender, e);
        }
        private void PBNotifyRenew_Click(object sender, EventArgs e)
        {
            if (this._dtnotify != null)
            {
                this._dtnotify.Clear();
                this.FLVNotify.Clear();
                this.SetNotifyColumns();
                this.TSCBGroup.Items.Clear();
            }
            this.RunNotify();
        }
        private void FLVNotify_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender != (object)this.FLVNotify)
            {
                return;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                NotifyCopySelectedToClipboard();
            }
        }
        private void TSMINotifyMultiselect_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            this._MultiselectSetNotify(item.Checked);
        }
        private void TSMINotifyScroll_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Scrollable = item.Checked));
        }
        private void TSMINotifyCopy_Click(object sender, EventArgs e)
        {
            this.NotifyCopySelectedToClipboard();
        }
        private void TSMINotifyExportCSV_Click(object sender, EventArgs e)
        {
            if (this.FBDNotifyExport.ShowDialog() == DialogResult.OK)
            {
                if ((this._dtnotify != null) && (this._dtnotify.Rows.Count > 0))
                {
                    try
                    {
                        this._RenewExportFolderPath(this.FBDNotifyExport.SelectedPath);
                        this._ExportCSV(this._dtnotify, this.FBDNotifyExport.SelectedPath);
                    }
                    catch (Exception ex)
                    {
                        this._winMessageError(ex.Message);
                    }
                }
                else
                {
                    this._winStatusBar(Properties.Resources.txtNotifyExportEmpty);
                }
            }
        }
        private void TSMINotifyExportTXT_Click(object sender, EventArgs e)
        {
            if (this.FBDNotifyExport.ShowDialog() == DialogResult.OK)
            {
                if (this.FLVNotify.Items.Count > 0)
                {
                    try
                    {
                        this._RenewExportFolderPath(this.FBDNotifyExport.SelectedPath);
                        this._ExportTXT(this.FLVNotify, this.FBDNotifyExport.SelectedPath);
                    }
                    catch (Exception ex)
                    {
                        this._winMessageError(ex.Message);
                    }
                }
                else
                {
                    this._winStatusBar(Properties.Resources.txtNotifyExportEmpty);
                }
            }
        }
        private void TSMINotifyExportHTML_Click(object sender, EventArgs e)
        {
            if (this.FBDNotifyExport.ShowDialog() == DialogResult.OK)
            {
                if (this.FLVNotify.Items.Count > 0)
                {
                    try
                    {
                        this._RenewExportFolderPath(this.FBDNotifyExport.SelectedPath);
                        this._ExportHTML(this.FLVNotify, this.FBDNotifyExport.SelectedPath);
                    }
                    catch (Exception ex)
                    {
                        this._winMessageError(ex.Message);
                    }
                }
                else
                {
                    this._winStatusBar(Properties.Resources.txtNotifyExportEmpty);
                }
            }
        }

        private void flatContextMenuStripNotify_Opening(object sender, CancelEventArgs e)
        {
            this.TSTBNotifyFind.Text = this.TSCBGroup.Text = "";
        }

        private void TSCBGroup_KeyUp(object sender, KeyEventArgs e)
        {
            if (
                (sender == (object)this.TSCBGroup) &&
                (e.KeyCode == Keys.Return)
               )
            {
                this.NotifyCheckFindItem(((ToolStripComboBox)sender).Text,
                    ((Action<ListView, string>)((a, b) => this.NotifyGroupFind(a, b)))
                );
            }
        }
        private void TSCBGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == (object)this.TSCBGroup)
            {
                this.NotifyCheckFindItem(((ToolStripComboBox)sender).Text,
                    ((Action<ListView, string>)((a, b) => this.NotifyGroupFind(a, b)))
                );
            }
        }
        private void TSTBNotifyFind_KeyUp(object sender, KeyEventArgs e)
        {
            if (
                (sender == (object)this.TSTBNotifyFind) &&
                (e.KeyCode == Keys.Return) &&
                (this.FLVNotify.Items.Count > 0)
               )
            {
                this.NotifyCheckFindItem(((ToolStripTextBox)sender).Text,
                    ((Action<ListView, string>)((a, b) => this.NotifyItemFind(a, b)))
                );
            }
        }
        private void TSTBNotifyFind_Click(object sender, EventArgs e)
        {
            if (
                (sender == (object)this.TSTBNotifyFind) &&
                (this.FLVNotify.Items.Count > 0)
               )
            {
                this.NotifyCheckFindItem(((ToolStripTextBox)sender).Text,
                    ((Action<ListView, string>)((a, b) => this.NotifyItemFind(a, b)))
                );
            }
        }
        private void TSMINotifySelectAll_Click(object sender, EventArgs e)
        {
            if (
                (sender == (object)this.TSMINotifySelectAll) &&
                (this.FLVNotify.Items.Count > 0)
               )
            {
                this.NotifyItemsSelect(this.FLVNotify);
            }
        }
        private void TSMINotifyGroup_Click(object sender, EventArgs e)
        {
            if (sender == (object)this.TSMINotifyGroup)
            {
                if (this.FLVNotify.ShowGroups)
                {
                    this.FLVNotify.Columns[1].Width = 100;
                    this.FLVNotify.Columns[2].Width = (this.FLVNotify.Width - 300);
                    this.FLVNotify.Columns[3].Width = 150;
                }
                else
                {
                    this.FLVNotify.Columns[1].Width = 0;
                    this.FLVNotify.Columns[2].Width = (this.FLVNotify.Width - 250);
                    this.FLVNotify.Columns[3].Width = 200;
                }
                this.FLVNotify.ShowGroups = !this.FLVNotify.ShowGroups;
            }
        }

        #endregion

        #region Notify services

        private void NotifyContextMenuClose()
        {
            flatThreadSafe.Run(this, (Action)(() => this.flatContextMenuStripNotify.Close()));
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Focus()));
        }

        private void NotifyItemsSelect(ListView lv)
        {
            this._MultiselectSetNotify(true);

            foreach (ListViewItem item in lv.Items)
            {
                item.Selected = true;
            }
            this.NotifyContextMenuClose();
        }

        private void NotifyCheckFindItem(string txt, Action<ListView, string> act)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return;
            }
            flatThreadSafe.Run(this, (Action)(() => act(this.FLVNotify, txt)));
        }

        private void NotifyItemFind(ListView lv, string search)
        {
            this._MultiselectSetNotify(true);

            foreach (ListViewItem item in lv.Items)
            {
                item.Selected = false;

                if (item.Text.Contains(search))
                {
                    item.Selected = true;
                }
                else
                {
                    foreach (ListViewItem.ListViewSubItem sitem in item.SubItems)
                    {
                        if (sitem.Text.Contains(search))
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
            this.NotifyContextMenuClose();
        }

        private void NotifyGroupFind(ListView lv, string grpname)
        {
            foreach (ListViewGroup grp in lv.Groups)
            {
                if (grp.Name.Equals(grpname))
                {
                    if (grp.Items.Count > 0)
                    {
                        grp.Items[0].EnsureVisible();
                        this.NotifyContextMenuClose();
                    }
                }
            }
        }

        private void AutoCompleteNotifySelect(ToolStripTextBox tstb)
        {
            if (Properties.Settings.Default.USRUrl.Count == 0)
            {
                return;
            }
            try
            {
                string jout, url = string.Format(
                    Properties.Settings.Default.URLMemberSetup,
                    Properties.Settings.Default.USRCoCServer,
                    Properties.Settings.Default.USRUrl[0],
                    new Random(DateTime.Now.Millisecond).Next(0, Int32.MaxValue)
                );

                jout = WebGet.GetJsonString(url, this._iLog);
                if (string.IsNullOrWhiteSpace(jout))
                {
                    this.bgNotifyWorker.ReportProgress(0);
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "Json " + Properties.Resources.txtRequest,
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }

                if (this._dtclan != null)
                {
                    flatThreadSafe.Run(this, (Action)(() => this._dtclan.Clear()));
                }

                this._dtclan = jout.JsonToDataTable<stClient.ClanMemberClient>();
                if (this._dtclan.Rows.Count < 1)
                {
                    this.bgNotifyWorker.ReportProgress(0);
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "DataTable",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }

                List<string> autolst = new List<string>();

                foreach (DataRow dr in this._dtclan.Rows)
                {
                    string nik;
                    try
                    {
                        nik = Convert.ToString(dr["nik"], CultureInfo.InvariantCulture);
                        if (string.IsNullOrWhiteSpace(nik))
                        {
                            throw new ArgumentNullException();
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    autolst.Add(nik);
                }
                flatThreadSafe.Run(this, (Action)(() => tstb.AutoCompleteCustomSource.Clear()));
                flatThreadSafe.Run(this, (Action)(() => tstb.AutoCompleteCustomSource.AddRange(autolst.ToArray())));
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
        }

        private void NotifyCopySelectedToClipboard()
        {
            StringBuilder sb = new StringBuilder();
            foreach (ListViewItem item in this.FLVNotify.SelectedItems)
            {
                sb.Append(item.SubItems[1].Text);
                sb.Append(_txt_copysep);
                sb.Append(item.SubItems[2].Text);
                sb.Append(_txt_copysep);
                sb.Append(item.SubItems[3].Text);
                sb.Append(_txt_copysep);
                sb.AppendLine(item.SubItems[4].Text);
            }
            string txt = sb.ToString();
            if (!string.IsNullOrWhiteSpace(txt))
            {
                Clipboard.SetText(txt);
                this._winStatusBar(Properties.Resources.txtNotifyCopy);
            }
            else
            {
                this._winStatusBar(Properties.Resources.txtNotifyNoCopy);
            }
        }

        #endregion

        #region Notify set

        private void SetNotifyColumns()
        {
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Clear()));
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Columns.Add("*", 25, HorizontalAlignment.Center)));
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Columns.Add("date", 0, HorizontalAlignment.Left)));
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Columns.Add("event", (this.FLVNotify.Width - 250), HorizontalAlignment.Left)));
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Columns.Add("details", 200, HorizontalAlignment.Left)));
            flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Columns.Add("tag", 0, HorizontalAlignment.Right)));
        }

        #endregion

        #region Notify bg worker

        private void RunNotify()
        {
            if (this._dtnotify == null)
            {
                this._dtnotify = stClient.ClientConvertExtension.MapToDataTable<stClient.ClanNotifyClient>();
                this.SetNotifyColumns();
            }
            if ((!this.bgNotifyWorker.IsBusy) && (_CheckSetupCoCServer()))
            {
                this.bgNotifyWorker.RunWorkerAsync();
                this._winStatusBar(
                    string.Format(
                        Properties.Resources.fmtNotifyStart,
                        DateTime.Now.ToShortTimeString()
                    )
                );
            }
            else if ((this.bgNotifyWorker.IsBusy) && (_CheckSetupCoCServer()))
            {
                if (!this.bgNotifyWorker.CancellationPending)
                {
                    this.bgNotifyWorker.CancelAsync();
                }
                while (this.bgNotifyWorker.IsBusy)
                {
                    Application.DoEvents();
                }
                this.bgNotifyWorker.RunWorkerAsync();
                this._winStatusBar(
                    string.Format(
                        Properties.Resources.fmtNotifyReStart,
                        DateTime.Now.ToShortTimeString()
                    )
                );
            }
            else if ((!this.bgNotifyWorker.IsBusy) && (!_CheckSetupCoCServer()))
            {
                this._winStatusBar(Properties.Resources.txtNotifyIncomplette);
            }
            else if ((this.bgNotifyWorker.IsBusy) && (!_CheckSetupCoCServer()) && (!this.bgNotifyWorker.CancellationPending))
            {
                this.bgNotifyWorker.CancelAsync();
                this._winStatusBar(Properties.Resources.txtNotifyChangeSettings);
            }
        }
        private void bgNotifyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (this.bgNotifyWorker.CancellationPending)
                {
                    e.Cancel = true;
                    this.bgNotifyWorker.ReportProgress(0);
                    return;
                }
                if (
                    (Properties.Settings.Default.USRUrl.Count < 3) ||
                    (string.IsNullOrWhiteSpace(Properties.Settings.Default.USRUrl[1]))
                   )
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "URL " + Properties.Resources.txtSetupData,
                            Properties.Resources.txtIncomplete
                        )
                    );
                }

                string jout, url = string.Format(
                    Properties.Settings.Default.URLNotifySetup,
                    Properties.Settings.Default.USRCoCServer,
                    Properties.Settings.Default.USRUrl[1],
                    new Random(DateTime.Now.Millisecond).Next(0, Int32.MaxValue)
                );

                DateTime dtnow;
                CoCEnum.EventNotify type;
                bool isChangeMembers = false;
                int crw, cnp, cnt = 10;
                this._oldNotifyItems = crw = this.FLVNotify.Items.Count;

                this.bgNotifyWorker.ReportProgress(5);

                jout = WebGet.GetJsonString(url, this._iLog);
                if (string.IsNullOrWhiteSpace(jout))
                {
                    this.bgNotifyWorker.ReportProgress(0);
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "Json " + Properties.Resources.txtRequest,
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }

                this.bgNotifyWorker.ReportProgress(cnt);

                DataTable dt = jout.JsonToDataTable<stClient.ClanNotifyClient>();
                if (dt.Rows.Count < 1)
                {
                    this.bgNotifyWorker.ReportProgress(0);
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "DataTable",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                cnp = (int)(90 / dt.Rows.Count);
                cnp = ((cnp == 0) ? 1 : cnp);
                dtnow = DateTime.Now;
                string grpDate = dtnow.ToString("HH:mm dd-MM", CultureInfo.InvariantCulture);

                flatThreadSafe.Run(this, (Action)(() => this.TSCBGroup.Items.Add(grpDate)));

                ListViewGroup grp = new FlatListViewGroup(
                    this.FLVNotify,
                    new string[]
                    {
                        grpDate,
                        string.Format(
                            Properties.Resources.fmtListViewGroup,
                            grpDate,
                            dt.Rows.Count
                        )
                    },
                    HorizontalAlignment.Left
                ).ListViewGroup();

                foreach (DataRow dr in dt.Rows)
                {
                    string ids, fmt1, fmt2;
                    DataRow drn = this._dtnotify.NewRow();

                    drn.ItemArray = dr.ItemArray.Clone() as object[];
                    drn["dtin"] = dtnow;
                    this._dtnotify.Rows.Add(drn);

                    this.bgNotifyWorker.ReportProgress(cnt += cnp);

                    try
                    {
                        ids = Convert.ToString(drn["id"], CultureInfo.InvariantCulture);
                        if (
                            (string.IsNullOrWhiteSpace(ids)) ||
                            (!Enum.TryParse<CoCEnum.EventNotify>(ids, out type)) || // Check a valid event id
                            (!(bool)Properties.Settings.Default[ids])
                           )
                        {
                            throw new ArgumentException();
                        }
                        fmt1 = stClient.Properties.ResourceNotify.ResourceManager.GetString("fmt1" + ids);
                        fmt2 = stClient.Properties.ResourceNotify.ResourceManager.GetString("fmt2" + ids);
                        if (
                            (string.IsNullOrWhiteSpace(fmt1)) ||
                            (string.IsNullOrWhiteSpace(fmt2))
                           )
                        {
                            throw new ArgumentException();
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    switch (type)
                    {
                        case CoCEnum.EventNotify.MemberNew:
                        case CoCEnum.EventNotify.MemberExit:
                        case CoCEnum.EventNotify.ClanChangeMembers:
                            {
                                isChangeMembers = true;
                                break;
                            }
                    }
                    new FlatListViewItem(
                        this.FLVNotify,
                        grp,
                        new string[]
                        {
                            "",
                            grpDate,
                            string.Format(
                                fmt1,
                                (string)drn["name"] as string,
                                (string)drn["vold"] as string,
                                (string)drn["vnew"] as string,
                                (string)drn["vres"] as string,
                                (string)drn["vs"] as string,
                                (string)drn["vcalc"] as string,
                                (string)drn["tag"] as string
                            ),
                            string.Format(
                                fmt2,
                                (string)drn["name"] as string,
                                (string)drn["vold"] as string,
                                (string)drn["vnew"] as string,
                                (string)drn["vres"] as string,
                                (string)drn["vs"] as string,
                                (string)drn["vcalc"] as string,
                                (string)drn["tag"] as string
                            ),
                            (string)drn["tag"] as string
                        },
                        (string)drn["id"] as string,
                        crw++
                    );
                }

                if (
                    (this.TSTBNotifyFind.AutoCompleteCustomSource.Count == 0) ||
                    (isChangeMembers)
                   )
                {
                    this.AutoCompleteNotifySelect(this.TSTBNotifyFind);
                }

                this.bgNotifyWorker.ReportProgress(90);

                if (crw > this._oldNotifyItems)
                {
                    flatThreadSafe.Run(this, (Action)(() => this.FLVNotify.Items[this._oldNotifyItems].Selected = true));
                }

                // Image Informer

                if (this._CheckSetupInformer())
                {
                    url = string.Format(
                        Properties.Settings.Default.URLServerInformer,
                        Properties.Settings.Default.USRCoCServer,
                        Properties.Settings.Default.USRUrl[2],
                        Properties.Settings.Default.USRInformerId,
                        Properties.Settings.Default.USRCoCTag,
                        new Random(DateTime.Now.Millisecond).Next(0, Int32.MaxValue)
                    );
                    if (this.ImageInformer != null)
                    {
                        this.ImageInformer.Dispose();
                        this.ImageInformer = null;
                    }
                    try
                    {
                        this.ImageInformer = (Image)WebGet.GetImage(url, this._iLog);
                    }
                    catch (Exception ex)
                    {
                        this.ImageInformer = (Image)Properties.Resources.InformerLoadingError;
                        throw ex;
                    }
                    finally
                    {
                        this.bgNotifyWorker.ReportProgress(100);
                    }
                }
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
            finally
            {
                this.bgNotifyWorker.ReportProgress(0);
            }
        }
        private void bgNotifyWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this._winProgressBar(e.ProgressPercentage);
        }
        private void bgNotifyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string status;

            if (e.Cancelled)
            {
                status = Properties.Resources.txtCancelled;
            }
            else if (e.Error != null)
            {
                status = Properties.Resources.txtProcessAbort;
            }
            else
            {
                status = Properties.Resources.txtCompleted;
            }
            status = string.Format(
                        Properties.Resources.fmtProcessUpdate,
                        (this.FLVNotify.Items.Count - this._oldNotifyItems),
                        status,
                        DateTime.Now.ToShortTimeString()
            );
            this._LVAutoScroll(this.FLVNotify);
            this._winProgressBar(0);
            this._winStatusBar(status);
        }

        #endregion
    }
}
