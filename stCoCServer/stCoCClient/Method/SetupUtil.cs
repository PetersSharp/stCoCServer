using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stCoreUI;
using System.IO;
using System.Diagnostics;

namespace stCoCClient
{
    public partial class ClientForm
    {
        private const string _defaultExporFileName = "ClanNotifyExport";

        #region window Message/Error set

        private void _winMessageError(string msg)
        {
#if DEBUG
            if (!this.isMinimized)
            {
                string msgstack = "";
                StackFrame CallStack = null;
                for (int i = 1; i < 10; i++)
                {
                    CallStack = new StackFrame(i, true);
                    if ((CallStack != null) && (!string.IsNullOrWhiteSpace(CallStack.GetFileName())))
                    {
                        msgstack += string.Format(
                            "{0}[{1}:{2}]",
                            Environment.NewLine,
                            Path.GetFileName(CallStack.GetFileName()),
                            CallStack.GetFileLineNumber()
                        );
                    }
                    else
                    {
                        break;
                    }
                }
                MessageBox.Show(msg + msgstack);
            }
#endif
            if (this.isMinimized)
            {
                this.notifyIconMain_Balloon(msg, null, ToolTipIcon.Error, 10000);
            }
            else
            {
                this._winMessage(FlatAlertBox._Kind.Error, msg, 10000);
            }
        }
        private void _winMessageSuccess(string msg)
        {
            if (this.isMinimized)
            {
                this.notifyIconMain_Balloon(msg, null, ToolTipIcon.Warning, 5000);
            }
            else
            {
                this._winMessage(FlatAlertBox._Kind.Success, msg, 5000);
            }
        }
        private void _winMessageInfo(string msg)
        {
            if (this.isMinimized)
            {
                this.notifyIconMain_Balloon(msg, null, ToolTipIcon.Info, 5000);
            }
            else
            {
                this._winMessage(FlatAlertBox._Kind.Info, msg, 5000);
            }
        }
        private void _winMessage(FlatAlertBox._Kind type, string msg, int delay)
        {
            flatThreadSafe.Run(this, (Action)(() => this.flatAlertBoxMain.ShowControl(type, msg, delay)));
        }
        private void _winStatusBar(string msg)
        {
            if (this.isMinimized)
            {
                this.notifyIconMain_Balloon(msg, null, ToolTipIcon.None, 10000);
            }
            else
            {
                flatThreadSafe.Run(this, (Action)(() => this.flatSB.Text = msg));
            }
        }
        private void _winProgressBar(int cnt)
        {
            if ((cnt == 0) || (cnt >= 100))
            {
                flatThreadSafe.Run(this, (Action)(() => this.FPBNotify.Visible = false));
            }
            else if (!this.FPBMain.Visible)
            {
                flatThreadSafe.Run(this, (Action)(() => this.FPBNotify.Visible = true));
            }
            flatThreadSafe.Run(this, (Action)(() => this.FPBNotify.Value = cnt));
        }
        private void _SetErrorFlatTextBox(object ctrl, string error)
        {
            if (ctrl == null)
            {
                return;
            }
            flatThreadSafe.Run(this, (Action)(() => this.errorProviderSetup.SetIconPadding(((FlatTextBox)ctrl), 10)));
            flatThreadSafe.Run(this, (Action)(() => this.errorProviderSetup.SetError(((FlatTextBox)ctrl), error)));
        }

        #endregion

        #region Check method

        private string _CheckTextBox(Object ctrl)
        {
            string uritxt = ((FlatTextBox)ctrl).Text;
            if (string.IsNullOrWhiteSpace(uritxt))
            {
                this._SetErrorFlatTextBox(
                    ctrl,
                    string.Format(
                        Properties.Resources.fmtCheck,
                        "value",
                        Properties.Resources.txtIsEmpty,
                        ""
                    )
                );
                return null;
            }
            return uritxt;
        }
        private bool _CheckURL(string src, Object ctrl)
        {
            Uri outUri;
            if (
                (Uri.TryCreate(src, UriKind.Absolute, out outUri)) &&
                ((outUri.Scheme == Uri.UriSchemeHttp) || (outUri.Scheme == Uri.UriSchemeHttps))
               )
            {
                this.errorProviderSetup.Clear();
                return true;
            }
            this._SetErrorFlatTextBox(
                ctrl,
                string.Format(
                    Properties.Resources.fmtCheck,
                    "URL",
                    Properties.Resources.txtNotCorrect,
                    ""
                )
            );
            return false;
        }
        private bool _CheckSetupCoCServer()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.USRCoCServer))
            {
                this.errorProviderSetup.Clear();
                return false;
            }
            return true;
        }
        private bool _CheckSetupInformer()
        {
            if (
                (!this._CheckSetupCoCServer()) ||
                (Properties.Settings.Default.USRUrl.Count < 4) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.USRUrl[2])) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.IRCNik)) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.USRCoCTag))
               )
            {
                return false;
            }
            return true;
        }
        private bool _CheckSetupIRCChat()
        {
            if (
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.IRCServer)) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.IRCPort)) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.IRCChannel)) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.IRCNik))
               )
            {
                this.errorProviderSetup.Clear();
                return false;
            }
            return true;
        }
        private bool _CheckSetupSSENotify()
        {
            if (
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.USRCoCServer)) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.USRCoCTag)) ||
                (Properties.Settings.Default.USRUrl.Count < 2) ||
                (string.IsNullOrWhiteSpace(Properties.Settings.Default.USRUrl[1]))
               )
            {
                this.errorProviderSetup.Clear();
                return false;
            }
            return true;
        }
        private bool _CheckDataRow(DataRow dr, int cnt, string nm, string val)
        {
            return ((
                      (dr.Table.Columns.Count < cnt) ||
                      (dr.IsNull(nm)) ||
                      (dr.IsNull(val)) ||
                      (string.IsNullOrWhiteSpace((string)dr[nm])) ||
                      (string.IsNullOrWhiteSpace((string)dr[val]))
                  ) ? false : true);
        }
        private bool _CheckTimerNotify()
        {
            if (Properties.Settings.Default.USRNotifyUpdateTime > 0)
            {
                int tmi = (int)(((Properties.Settings.Default.USRNotifyUpdateTime + 3) * 60) * 1000);
                if (this.TimerRunNotify.Interval != tmi)
                {
                    this.TimerRunNotify.Stop();
                    this.TimerRunNotify.Interval = Properties.Settings.Default.USRTimerNotifyInterval = tmi;
                }
            }
            else if (this.TimerRunNotify.Interval == 1)
            {
                this.TimerRunNotify.Stop();
                return false;
            }
            this.TimerRunNotify.Start();
            return true;
        }
        #endregion

        #region Normalize/Convert method

        private string _URLNormalize(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return String.Empty;
            }
            return string.Format(
                "{0}{1}",
                url,
                ((url[(url.Length - 1)] == '/') ? "" : "/")
            );
        }
        public string _ExportFilePath(string path, string ext, string fname = null)
        {
            fname = ((string.IsNullOrWhiteSpace(fname)) ? _defaultExporFileName : fname);
            return Path.Combine(
                path,
                fname + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + "." + ext
            );
        }
        public void _RenewExportFolderPath(string path)
        {
            if (!path.Equals(Properties.Settings.Default.ExportPath))
            {
                Properties.Settings.Default.ExportPath = path;
            }
        }

        #endregion

        #region ListView method

        private void _LVAutoScroll(ListView lv)
        {
            int cnt = (lv.Items.Count - 1);
            if (cnt > 0)
            {
                flatThreadSafe.Run(this, (Action)(() => lv.Select()));
                flatThreadSafe.Run(this, (Action)(() => lv.EnsureVisible(cnt)));
                flatThreadSafe.Run(this, (Action)(() => lv.Items[cnt].Focused = true));

                /*
                 * Another way
                 * flatThreadSafe.Run(this, (Action)(() => lv.Items[cnt].EnsureVisible()));
                 *
                 */
            }
        }

        private void _MultiselectSetNotify(bool state)
        {
            Properties.Settings.Default.TSMIMultiselect = state;
            flatThreadSafe.Run(this, (Action)(() => this.TSMINotifyMultiselect.Checked = this.FLVNotify.MultiSelect = true));
        }

        #endregion

        #region Tab control select manager

        private void _TabSelector(TabCtrlName tabId, bool show)
        {
            TabPage tp  = null;
            bool ircok = this._CheckSetupIRCChat(),
                 statok = this._CheckSetupSSENotify();

            foreach (var tabEntry in this._alltab)
            {
                if (tabEntry.Key == tabId)
                {
                    tp = (TabPage)tabEntry.Value;
                }
                if (show)
                {
                    ((TabPage)tabEntry.Value).Parent = null;
                }
                else
                {
                    if (
                          (tabEntry.Key == TabCtrlName.tabPageSetupMain) ||
                          (tabEntry.Key == TabCtrlName.tabPageSetupUser) ||
                          (tabEntry.Key == TabCtrlName.tabPageSetupReg) ||
                          (tabEntry.Key == TabCtrlName.tabPageHelp) ||
                          ((!ircok) && (tabEntry.Key == TabCtrlName.tabPageClanChat)) ||
                          ((!statok) && (tabEntry.Key == TabCtrlName.tabPageClanStat))
                        )
                    {
                        ((TabPage)tabEntry.Value).Parent = null;
                    }
                    else
                    {
                        ((TabPage)tabEntry.Value).Parent = this.flatTabControlMain;
                    }
                }
            }
            if (tp != null)
            {
                if (show)
                {
                    tp.Parent = this.flatTabControlMain;
                    this.flatTabControlMain.SelectedTab = tp;
                    tp.Show();
                }
                else if (tp.Parent != null)
                {
                    tp.Parent = null;
                }
            }
            if ((!show) && (this._ctab != null))
            {
                this.flatTabControlMain.SelectedTab = this._ctab;
                this._ctab.Show();
                this._ctab = null;
            }
        }
        private void _TabSelect(TabCtrlName tabId)
        {
            foreach (var tabEntry in this._alltab)
            {
                if (tabEntry.Key == tabId)
                {
                    TabPage tp = (TabPage)tabEntry.Value;
                    this.flatTabControlMain.SelectedTab = tp;
                    tp.Show();
                }
            }
        }

        #endregion
    }
}
