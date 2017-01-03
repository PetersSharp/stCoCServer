using System;
using System.Data;
using System.Windows.Forms;
using stCore;
using stCoreUI;
using stClient;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Drawing;

namespace stCoCClient
{
    public partial class ClientForm
    {
        private DataTable _dtsetup = null;

        #region event change CoCServer (FTBCoCServerW/FTBCoCServerS)

        private void flatTabControlMain_Selected(object sender, TabControlEventArgs e)
        {
            this.errorProviderSetup.Clear();
        }
        private void FTBCoCServer_TextChanged(object sender, EventArgs e)
        {
            this.errorProviderSetup.Clear();
            this._isCoCServChanged = true;
        }
        private void FTBGameTag_TextChanged(object sender, EventArgs e)
        {
            this.errorProviderSetup.Clear();
            this._isGameTagChanged = true;
        }

        private void FTBCoCServer_Leave(object sender, EventArgs e)
        {
            if (!this._isCoCServChanged)
            {
                return;
            }

            string srvuri;

            if (
                ((srvuri = this._CheckTextBox(sender)) == null) ||
                (!this._CheckURL(srvuri, sender))
               )
            {
                this._winMessageError(
                    string.Format(
                        Properties.Resources.fmtCoCServer,
                        "URL",
                        Properties.Resources.txtCheckFailed
                    )
                );
            }
            this._isCoCServChanged = false;
        }

        #endregion

        #region event change informer id

        private void flatNumericInformerId2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (
                ((FlatNumeric)sender == this.flatNumericInformerId2) &&
                (Properties.Settings.Default.USRInformerId != this.flatNumericInformerId2.Value)
               )
            {
                string urlpart;

                if ((this._dtsetup != null) && (this._dtsetup.Rows.Count != 0))
                {
                    urlpart = Convert.ToString(this._dtsetup.Rows[0]["URLInformer"], CultureInfo.InvariantCulture);
                }
                else
                {
                    urlpart = "informer";
                }

                this._IMGInformerControl.ShowId(
                    this.FTBCoCServer1.Text,
                    this.FTBGameTag1.Text,
                    urlpart,
                    this.flatNumericInformerId2.Value,
                    new Point(15, 8)
                );
            }
        }

        private void flatNumericInformerId2_Leave(object sender, EventArgs e)
        {
            this._IMGInformerControl.Hide();
        }

        #endregion

        private void FSBExportPath_Click(object sender, EventArgs e)
        {
            if (this.FBDNotifyExport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this._RenewExportFolderPath(this.FBDNotifyExport.SelectedPath);
                    this._winMessageSuccess(
                        string.Format(
                            Properties.Resources.fmtExportPath,
                            this.FBDNotifyExport.SelectedPath
                        )
                    );
                }
                catch (Exception ex)
                {
                    this._winMessageError(ex.Message);
                }
            }
        }

        private void picBoxSetupMain_Click(object sender, EventArgs e)
        {
            this._ctab = this.flatTabControlMain.SelectedTab;
            this._TabSelector(TabCtrlName.tabPageSetupMain, true);
        }

        private void CheckSetup_Click(object sender, EventArgs e)
        {
            this._TabSelector(TabCtrlName.tabPageSetupUser, true);
        }

        private void CheckCoCServer_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._dtsetup != null)
                {
                    this._dtsetup.Clear();
                    this._dtsetup = null;
                }

                this.FLVSetup.Clear();
                this.errorProviderSetup.Clear();

                string jout, url;
                this.FTBCoCServer1.Text = url = this._URLNormalize(this.FTBCoCServer1.Text);

                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "URL",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }

                url = string.Format(
                    Properties.Settings.Default.URLServerSetup,
                    url,
                    new Random(DateTime.Now.Millisecond).Next(0,Int32.MaxValue)
                );

                jout = WebGet.GetJsonString(url, this._iLog);
                if (string.IsNullOrWhiteSpace(jout))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "Json " + Properties.Resources.txtRequest,
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }

                this._dtsetup = jout.JsonToDataTable<stClient.ServerSetup>();
                if (this._dtsetup.Rows.Count < 1)
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "DataTable",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                if (
                    (!this._CheckDataRow(this._dtsetup.Rows[0], 10, "ServerVersion", "ServerMagic")) ||
                    (!((string)this._dtsetup.Rows[0]["ServerVersion"]).ToMD5().Equals((string)this._dtsetup.Rows[0]["ServerMagic"]))
                   )
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            Properties.Resources.txtSetupData,
                            Properties.Resources.txtIncorrect
                        )
                    );
                }
                this.FLVSetup.Columns.Add("", 150, HorizontalAlignment.Left);
                this.FLVSetup.Columns.Add("", (this.FLVSetup.Width - 150), HorizontalAlignment.Left);
                for (int i = 0; i < this._dtsetup.Columns.Count; i++)
                {
                    new FlatListViewItem(
                        this.FLVSetup,
                        new string[]
                        {
                            (string)this._dtsetup.Columns[i].ColumnName,
                            (string)this._dtsetup.Rows[0][this._dtsetup.Columns[i].ColumnName].ToString()
                        }
                    );
                }
                this._winStatusBar(
                    string.Format(
                        Properties.Resources.fmtServerFound,
                        (string)this._dtsetup.Rows[0]["ServerVersion"]
                    )
                );
                this._CheckTimerNotify();
            }
            catch (Exception ex)
            {
                this._SetErrorFlatTextBox(
                    this.FTBCoCServer1,
                    string.Format(
                        Properties.Resources.fmtCoCServer,
                        Properties.Resources.txtIncorrectServerAddr,
                        ""
                    )
                );
                this._winMessageError(ex.Message);
                this._CheckTimerNotify();
            }
        }
        private void CheckGameTag_Click(object sender, EventArgs e)
        {
            if (!this._isGameTagChanged)
            {
                return;
            }
            
            this._isGameTagChanged = false;
            
            try
            {
                string jout, url, tag = this.FTBGameTag1.Text;
                this.FTBCoCServer1.Text = url = this._URLNormalize(this.FTBCoCServer1.Text);

                if (string.IsNullOrWhiteSpace(url))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "URL",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                if (string.IsNullOrWhiteSpace(tag))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "tag/server " + Properties.Resources.txtRequest,
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                if (tag.StartsWith("#"))
                {
                    this.FTBGameTag1.Text = tag = tag.Substring(1, (tag.Length - 1));
                }

                url = string.Format(
                    Properties.Settings.Default.URLUserSetup,
                    url,
                    tag
                );

                jout = WebGet.GetJsonString(url, this._iLog);
                if (string.IsNullOrWhiteSpace(jout))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "Json " + Properties.Resources.txtRequest,
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                DataTable dt = jout.JsonToDataTable<stClient.ClanMemberClient>();
                if (dt.Rows.Count < 1)
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "DataTable",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                if (
                    (!this._CheckDataRow(dt.Rows[0], 18, "tag", "nik")) ||  // TODO: later, count is 19
                    (!tag.Equals((string)dt.Rows[0]["tag"]))
                   )
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "client " + Properties.Resources.txtSetupData,
                            Properties.Resources.txtIncorrect
                        )
                    );

                }
                new FlatListViewItem(
                    this.FLVSetup,
                    new string[]
                        {
                            "IRCNick",
                            (string)(string)dt.Rows[0]["nik"].ToString()
                        }
                );
                new FlatListViewItem(
                    this.FLVSetup,
                    new string[]
                        {
                            "UserGameTag",
                            (string)(string)dt.Rows[0]["tag"].ToString()
                        }
                );
                if (this.FCBNikAuto.Checked)
                {
                    this.FTBIrcNick1.Text =
                        Regex.Replace((string)dt.Rows[0]["nik"].ToString(), @"[^\u0020-\u007E]", string.Empty);
                }
            }
            catch (Exception ex)
            {
                this._SetErrorFlatTextBox(
                    this.FTBGameTag1,
                    string.Format(
                        Properties.Resources.fmtCoCServer,
                        Properties.Resources.txtIncorrectUserIdTag,
                        ""
                    )
                );
                this._winMessageError(ex.Message);
            }
        }

        private void CancelAndExit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(this.FTBCoCServer1.Text))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtError,
                            "URL",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                Properties.Settings.Default.Reload();
                this._IMGInformerControl.Hide();
                this._TabSelector(TabCtrlName.tabPageSetupMain, true);
            }
            catch (Exception ex)
            {
                this._SetErrorFlatTextBox(
                    this.FTBCoCServer1,
                    string.Format(
                        Properties.Resources.fmtCoCServer,
                        Properties.Resources.txtIncomplete,
                        "URL"
                    )
                );
                this._winMessageError(ex.Message);
            }
        }
        
        private void CheckAndSave_Click(object sender, EventArgs e)
        {
            FlatTextBox ftb = null;
            try
            {
                this.CheckCoCServer_Click(sender, e);
                this.CheckGameTag_Click(sender, e);

                if (this._dtsetup == null)
                {
                    ftb = this.FTBCoCServer1;
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtText,
                            Properties.Resources.txtSetupData,
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                if (string.IsNullOrWhiteSpace(this.FTBIrcNick1.Text))
                {
                    ftb = this.FTBIrcNick1;
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtText,
                            "IRC Nick",
                            Properties.Resources.txtIsEmpty
                        )
                    );
                }
                if (this.Irc != null)
                {
                    this.Irc.Nick = this._IRCGetNickName(this.FTBIrcNick1.Text);
                }
                if ((string.IsNullOrWhiteSpace(this.FTBGameTag1.Text)) || (this.FTBGameTag1.Text.Length < 5))
                {
                    ftb = this.FTBGameTag1;
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtText,
                            Properties.Resources.txtCoCGameTag,
                            Properties.Resources.txtIncorrect
                        )
                    );
                }

                Properties.Settings.Default.IRCServer = Convert.ToString(this._dtsetup.Rows[0]["IRCServer"], CultureInfo.InvariantCulture);
                Properties.Settings.Default.IRCPort = Convert.ToString(this._dtsetup.Rows[0]["IRCPort"], CultureInfo.InvariantCulture);
                Properties.Settings.Default.IRCChannel = Convert.ToString(this._dtsetup.Rows[0]["IRCChannel"], CultureInfo.InvariantCulture);
                Properties.Settings.Default.IRCLanguage = Convert.ToString(this._dtsetup.Rows[0]["IRCLanguage"], CultureInfo.InvariantCulture);
                Properties.Settings.Default.USRServerVersion = Convert.ToString(this._dtsetup.Rows[0]["ServerVersion"], CultureInfo.InvariantCulture);
                Properties.Settings.Default.USRNotifyUpdateTime = this._dtsetup.Rows[0].Field<long>("NotifyUpdateTime");

                if (this._chatHistory != null)
                {
                    this._chatHistory.Max = Properties.Settings.Default.IRCChatHistory;
                }
                if (!this._CheckTimerNotify())
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.fmtSettingsError,
                            "Notify Timer",
                            Properties.Resources.txtNotCorrect
                        )
                    );
                }
                if (Properties.Settings.Default.USRUrl.Count > 0)
                {
                    Properties.Settings.Default.USRUrl.Clear();
                }
                try
                {
                    Properties.Settings.Default.USRUrl.Add(Convert.ToString(this._dtsetup.Rows[0]["URLClan"], CultureInfo.InvariantCulture));
                    Properties.Settings.Default.USRUrl.Add(Convert.ToString(this._dtsetup.Rows[0]["URLNotify"], CultureInfo.InvariantCulture));
                    Properties.Settings.Default.USRUrl.Add(Convert.ToString(this._dtsetup.Rows[0]["URLInformer"], CultureInfo.InvariantCulture));
                    Properties.Settings.Default.USRUrl.Add(Convert.ToString(this._dtsetup.Rows[0]["URLIrcLog"], CultureInfo.InvariantCulture));
                }
                catch (Exception ex)
                {
                    this._winMessageError(
                        string.Format(
                            Properties.Resources.fmtSetupSerCliMismatch,
                            ex.Message
                        )
                    );
                }
                
                Properties.Settings.Default.USRInformerId = this.flatNumericInformerId2.Value;
                Properties.Settings.Default.Save();
                this._dtsetup.Clear();
                this._dtsetup = null;
                this._IMGInformerControl.Hide();
                this._TabSelector(TabCtrlName.tabPageSetupUser, false);
            }
            catch (Exception ex)
            {
                this._SetErrorFlatTextBox(
                    ftb,
                    string.Format(
                        Properties.Resources.fmtSettingsError,
                        ex.Message
                    )
                );
                this._winMessageError(ex.Message);
            }
        }
        
        private void bboxSave_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.USRInformerId = this.flatNumericInformerId1.Value;
                Properties.Settings.Default.Save();
                this._winMessageSuccess(
                    string.Format(
                        Properties.Resources.fmtSettingsOk,
                        Properties.Resources.txtSave
                    )
                );
                this._TabSelector(TabCtrlName.tabPageSetupMain, false);
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
        }
        
        private void bboxReload_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.Reload();
                this._mainform.Invalidate();
                this._winMessageSuccess(
                    string.Format(
                        Properties.Resources.fmtSettingsOk,
                        Properties.Resources.txtReloading
                    )
                );
                this._TabSelector(TabCtrlName.tabPageSetupMain, false);
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
        }
        private void tboxCaption_Enter(object sender, EventArgs e)
        {
            try
            {
                this._winMessageInfo(((FlatTextBox)sender).TextCaption);
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
        }
    }
}
