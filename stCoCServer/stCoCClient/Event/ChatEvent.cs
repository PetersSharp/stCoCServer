using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using stCore;
using stCoreUI;
using stClient;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace stCoCClient
{
    public partial class ClientForm : Form
    {
        private const string defaultLeague = "29000000";
        private const string defaultIrcAdmin = "00000001";

        private readonly object _lockChatUserList = new Object();

        private bool isWebBrowser = false;
        private bool isPrivateMsg = false;
        private bool isChangeMsg = false;
        private bool isIrcJoin = false;
        private ListCircular<string> _chatHistory = null;

        #region Button web panel

        #region Connect UI

        private void FBChatConnectSetDisconnected()
        {
            this.FBChatConnect.BackColor = Color.FromArgb(35, 168, 109);
            this.FBChatConnect.Text = Properties.Resources.txtIRCConnectButton;
            this.isIrcJoin = false;
            lock (webwLock)
            {
                this.WBChat.Document.InvokeScript("MsgPrn",
                    new object[] {
                                    "msgnotice",
                                    Properties.Settings.Default.IRCChannel,
                                    string.Format(
                                        Properties.Resources.fmtChatDisconnected,
                                        Properties.Settings.Default.IRCServer
                                    )
                                }
                );
            }
        }
        private void FBChatConnectSetConnected()
        {
            this.FBChatConnect.BackColor = Color.FromArgb(234, 59, 4);
            this.FBChatConnect.Text = Properties.Resources.txtIRCDisConnectButton;
        }

        #endregion

        private void FBChatConnect_Click(object sender, EventArgs e)
        {
            if (this.Irc != null)
            {
                if (this.Irc.Connected)
                {
                    this.isIrcJoin = false;
                    this.Irc.Disconnect(true);
                    this.FLVChatUser.Items.Clear();
                    this.FBChatConnectSetDisconnected();
                }
                else
                {
                    this.TSMIChatClear_Click(sender, e);
                    this.Irc.Connect();
                    this.FBChatConnectSetConnected();
                }
            }
            else
            {
                this._winMessageError(Properties.Resources.txtIRCSetupErrorComplette);
                this.FBChatConnect.Text = Properties.Resources.txtIRCConnectButton;
                this.FBChatConnect.Enabled = false;
                this.isIrcJoin = false;
            }
        }
        private void PBChatSend_Click(object sender, EventArgs e)
        {
            if (this.Irc == null)
            {
                this._winMessageError(Properties.Resources.txtIRCSetupErrorComplette);
                return;
            }
            else if ((this.Irc != null) && (!this.Irc.Connected))
            {
                this._winMessageError(Properties.Resources.txtIRCNoConnected);
                return;
            }
            if (!this.isIrcJoin)
            {
                this._winStatusBar(Properties.Resources.txtIRCConnectWait);
                return;
            }

            string msg = this.FTBChatInput.Text;

            try
            {
                if ((this.isWebBrowser) && (!string.IsNullOrWhiteSpace(msg)))
                {
                    msg = Regex.Replace(msg, Properties.Settings.Default.ChatFilterHtml, String.Empty).Trim();

                    if (this._chatHistory.CheckRange(msg, 2))
                    {
                        this._winMessageError(Properties.Resources.txtChatSpamMsg);
                        return;
                    }

                    this._chatHistory.Add(msg);

                    if ((this.isPrivateMsg) && (!string.IsNullOrWhiteSpace(this.selectedNick)))
                    {
                        lock (webwLock)
                        {
                            this.WBChat.Document.InvokeScript("MsgPrn",
                                new object[] {
                                    "msgmy",
                                    string.Format(
                                        Properties.Resources.txtChatPrivateHead,
                                        this.selectedNick
                                    ),
                                    this._IRCChatToHTML(msg)
                                }
                            );
                        }
                        this.Irc.SendMessage(this.selectedNick, msg);
                    }
                    else
                    {
                        lock (webwLock)
                        {
                            this.WBChat.Document.InvokeScript("MsgPrn",
                                new object[] {
                                    "msgmy",
                                    Properties.Settings.Default.IRCNik,
                                    this._IRCChatToHTML(msg)
                                }
                            );
                        }
                        this.Irc.SendMessage(Properties.Settings.Default.IRCChannel, msg);
                    }
                    if (this.isPrivateMsg)
                    {
                        this.PBChatMsgType_Click(sender, e);
                    }
                    this.FTBChatInput.Text = "";
                    this.isChangeMsg = false;
                    this.selectedNick = String.Empty;
                }
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
        }
        private void FTBChatInput_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.FTBChatInput.Text))
            {
                if (this.isChangeMsg)
                {
                    this.PBChatSend.BackColor = Color.FromArgb(45, 47, 49);
                    this.PBChatSend.Cursor = Cursors.Default;
                    this.isChangeMsg = false;
                }
            }
            else
            {
                if (!this.isChangeMsg)
                {
                    this.PBChatSend.BackColor = Color.FromArgb(35, 168, 109);
                    this.PBChatSend.Cursor = Cursors.Hand;
                    this.isChangeMsg = true;
                }
            }
        }
        private void FTBChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == (object)this.FTBChatInput)
            {
                bool isContexMenu = false;

                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        {
                            this.FTBChatInput.Text = "";
                            break;
                        }
                    case Keys.Return:
                        {
                            this.PBChatSend_Click(sender, null);
                            break;
                        }
                    case Keys.Apps:
                        {
                            isContexMenu = true;
                            break;
                        }
                    case Keys.Oemtilde:
                    case Keys.M:
                        {
                            if (e.Control)
                            {
                                isContexMenu = true;
                            }
                            break;
                        }
                    case Keys.Up:
                        {
                            if ((this._chatHistory != null) && (this._chatHistory.Count > 0))
                            {
                                this.FTBChatInput.Text = (string)this._chatHistory.Next();
                            }
                            break;
                        }
                    case Keys.Down:
                        {
                            if ((this._chatHistory != null) && (this._chatHistory.Count > 0))
                            {
                                this.FTBChatInput.Text = (string)this._chatHistory.Prev();
                            }
                            break;
                        }
                }
                if (isContexMenu)
                {
                    this.flatContextMenuStripChatInput.Show(
                        new Point(Cursor.Position.X, Cursor.Position.Y)
                    );
                }
            }
        }
        private void PBChatMsgType_Click(object sender, EventArgs e)
        {
            if (this.isPrivateMsg)
            {
                this.PBChatMsgType.Image.Dispose();
                this.PBChatMsgType.Image = global::stCoCClient.Properties.Resources.ic_lock_open_white_18dp;
                this.PBChatMsgType.BackColor = Color.FromArgb(60, 70, 73);
                this.isPrivateMsg = false;
                if (!string.IsNullOrWhiteSpace(this.selectedNick))
                {
                    this._winMessageInfo(
                        string.Format(
                            Properties.Resources.txtChatCancelPrivateMsg,
                            this.selectedNick
                        )
                    );
                }
                this.selectedNick = String.Empty;
            }
        }

        #endregion

        #region web IRC User context menu

        private void TSMIInsertNick_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.selectedNick))
            {
                this.FTBChatInput.Text += string.Format(Properties.Resources.txtChatInsert, this.selectedNick);
                this.FTBChatInput.Focus();
                this.selectedNick = String.Empty;
            }
        }
        private void TSMIReplyNick_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.selectedNick))
            {
                this.FTBChatInput.Text = string.Format(Properties.Resources.txtChatReply, this.selectedNick);
                this.FTBChatInput.Focus();
                this.selectedNick = String.Empty;
            }
        }
        private void TSMICopyNick_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.selectedNick))
            {
                Clipboard.SetText(this.selectedNick);
                this.selectedNick = String.Empty;
            }
        }
        private void TSMIPrivateMsg_Click(object sender, EventArgs e)
        {
            if (!this.isPrivateMsg)
            {
                if (Properties.Settings.Default.IRCNik.Equals(this.selectedNick))
                {
                    this._winMessageError(Properties.Resources.txtChatErrorNickEquals);
                    return;
                }
                this._winStatusBar(
                    string.Format(
                        Properties.Resources.fmtChatPrivateNick,
                        this.selectedNick
                    )
                );
                this.PBChatMsgType.Image.Dispose();
                this.PBChatMsgType.Image = global::stCoCClient.Properties.Resources.ic_lock_outline_white_18dp;
                this.PBChatMsgType.BackColor = Color.FromArgb(35, 168, 109);
                this.isPrivateMsg = true;
            }
        }
        private void TSMIKickNick_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.selectedNick))
            {
                this._winMessageError(Properties.Resources.txtChatErrorNickEmpty);
                return;
            }
            if (Properties.Settings.Default.IRCNik.Equals(this.selectedNick))
            {
                this._winMessageError(Properties.Resources.txtChatErrorNickKick);
                return;
            }
            this.Irc.SendKick(Properties.Settings.Default.IRCChannel, this.selectedNick);
            this.selectedNick = String.Empty;
        }
        private void TSMIReloadUserList_Click(object sender, EventArgs e)
        {
            if ((this.Irc != null) && (this.Irc.Connected) && (this.isIrcJoin))
            {
                this.ChatUserListClear();
                this.Irc.GetUserListChannel();
            }
        }

        #endregion

        #region web Panel context menu

        private void TSMIChatClear_Click(object sender, EventArgs e)
        {
            if (this.isWebBrowser)
            {
                this._WBInit();
            }
            this.selectedNick = String.Empty;
        }

        private void TSMIChatExport_Click(object sender, EventArgs e)
        {
            if (this.FBDNotifyExport.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this._RenewExportFolderPath(this.FBDNotifyExport.SelectedPath);
                    this._ExportBrowser(this.WBChat, this.FBDNotifyExport.SelectedPath);
                }
                catch (Exception ex)
                {
                    this._winMessageError(ex.Message);
                }
            }
        }

        private void TSMIChatArchive_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(this._IRCArchiveUrl());
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
        }

        #endregion

        #region Input Control context menu

        private void flatContextMenuStripChatInput_Opening(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.FTBChatInput.Text))
            {
                this.TSMIInputCopy.Visible = false;
                this.TSMIInputClear.Visible = false;
                this.TSMIInputSelect.Visible = false;

                this.toolStripSeparator5.Visible = false;
                this.TSMIInputSpell.Visible = false;

                this.TSMIInputStyle.Visible = false;
                this.TSMIInputColor.Visible = false;

                this.TSMIInputSend.Visible = false;
                this.TSMIInputSendPriv.Visible = false;
            }
            else
            {
                this.TSMIInputCopy.Visible = true;
                this.TSMIInputClear.Visible = true;
                this.TSMIInputSelect.Visible = true;

                this.toolStripSeparator5.Visible = true;
                this.TSMIInputSpell.Visible = true;

                this.TSMIInputStyle.Visible = true;
                this.TSMIInputColor.Visible = true;

                this.TSMIInputSend.Visible = true;
                this.TSMIInputSendPriv.Visible = true;

                if (this.isIrcJoin)
                {
                    this.TSMIInputSend.Enabled = true;
                    this.TSMIInputSendPriv.Enabled = true;
                }
                else
                {
                    this.TSMIInputSend.Enabled = false;
                    this.TSMIInputSendPriv.Enabled = false;
                }

                if (this.FTBChatInput.SelectionLength > 0)
                {
                    this.TSMIInputStyle.Enabled = true;
                    this.TSMIInputStyleBold.Enabled = true;
                    this.TSMIInputStyleItalic.Enabled = true;
                    this.TSMIInputStyleStrike.Enabled = true;
                    this.TSMIInputStyleUnderline.Enabled = true;
                    
                    this.TSMIInputColor.Enabled = true;
                    this.TSMIInputColorRed.Enabled = true;
                    this.TSMIInputColorCyan.Enabled = true;
                    this.TSMIInputColorBlue.Enabled = true;
                    this.TSMIInputColorGreen.Enabled = true;
                    this.TSMIInputColorYellow.Enabled = true;
                    this.TSMIInputColorMagenta.Enabled = true;
                    this.TSMIInputColorCustom.Enabled = true;
                }
                else
                {
                    this.TSMIInputStyle.Enabled = false;
                    this.TSMIInputStyleBold.Enabled = false;
                    this.TSMIInputStyleItalic.Enabled = false;
                    this.TSMIInputStyleStrike.Enabled = false;
                    this.TSMIInputStyleUnderline.Enabled = false;

                    this.TSMIInputColor.Enabled = false;
                    this.TSMIInputColorRed.Enabled = false;
                    this.TSMIInputColorCyan.Enabled = false;
                    this.TSMIInputColorBlue.Enabled = false;
                    this.TSMIInputColorGreen.Enabled = false;
                    this.TSMIInputColorYellow.Enabled = false;
                    this.TSMIInputColorMagenta.Enabled = false;
                    this.TSMIInputColorCustom.Enabled = false;
                }
            }
        }
        private void TSMIInputPaste_Click(object sender, EventArgs e)
        {
            this.FTBChatInput.Paste();
        }
        private void TSMIInputCopy_Click(object sender, EventArgs e)
        {
            this.FTBChatInput.Copy();
        }
        private void TSMIInputSelect_Click(object sender, EventArgs e)
        {
            this.FTBChatInput.SelectAll();
        }
        private void TSMIInputClear_Click(object sender, EventArgs e)
        {
            this.FTBChatInput.Text = "";
        }
        private void TSMIInputSend_Click(object sender, EventArgs e)
        {
            this.PBChatSend_Click(sender, e);
        }
        private void TSTBFindNick_KeyUp(object sender, KeyEventArgs e)
        {
            if (
                (sender == (object)this.TSTBFindNick) &&
                (e.KeyCode == Keys.Return) &&
                (this.FLVChatUser.Items.Count > 0)
               )
            {
                this.selectedNick = ((ToolStripTextBox)sender).Text;
                if (string.IsNullOrWhiteSpace(this.selectedNick))
                {
                    this._winMessageError(Properties.Resources.txtChatSelectNickEmpty);
                    return;
                }
                this.isPrivateMsg = true;
                this.PBChatSend_Click(sender, null);
                this.flatContextMenuStripChatInput.Close();
            }
        }

        #region Translate/Spelling in context menu

        private void TSMIInputSpell_Click(object sender, EventArgs e)
        {
            if (_TBSControl.Visible)
            {
                _TBSControl.Visible = false;
            }
            else
            {
                _TBSControl.Location = new Point(0, 331);
                _TBSControl.Text = this.FTBChatInput.Text.Trim();
                _TBSControl.Visible = true;
                _TBSControl.BringToFront();
            }
        }

        #endregion

        #region Smiles in context menu

        private void TSMISmiles_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = ((ToolStripMenuItem)sender);
            this.FTBChatInput.Text += tsmi.Text;
        }

        #endregion

        #region Style/Color in context menu

        private void TSMIInputStyle_Click(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(ToolStripMenuItem))
            {
                return;
            }
            if (this.FTBChatInput.SelectionLength > 0)
            {
                this.FTBChatInput.SelectionReplace(
                    string.Format(
                        Properties.Resources.fmtChatStyleText,
                        ((ToolStripMenuItem)sender).Tag as string,
                        this.FTBChatInput.SelectedText
                    )
                );
            }
        }

        private void TSMIInputColor_Click(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(ToolStripMenuItem))
            {
                return;
            }
            if (this.FTBChatInput.SelectionLength > 0)
            {
                this.FTBChatInput.SelectionReplace(
                    string.Format(
                        Properties.Resources.fmtChatStyleColor,
                        ((ToolStripMenuItem)sender).Tag as string,
                        this.FTBChatInput.SelectedText
                    )
                );
            }
        }

        private void TSMIInputColorCustom_Click(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(ToolStripMenuItem))
            {
                return;
            }
            if (this.FTBChatInput.SelectionLength > 0)
            {
                if (this.colorDialogInput.ShowDialog() == DialogResult.OK)
                {
                    this.FTBChatInput.SelectionReplace(
                        string.Format(
                            Properties.Resources.fmtChatStyleColor,
                            this.colorDialogInput.Color.ColorToHEX(),
                            this.FTBChatInput.SelectedText
                        )
                    );
                }
            }
        }

        #endregion

        #region Image/Url in context menu

        private void TSTBInputImage_KeyUp(object sender, KeyEventArgs e)
        {
            if (
                (sender == (object)this.TSTBInputImage) &&
                (e.KeyCode == Keys.Return)
               )
            {
                string url = this.TSTBInputImage.Text.Trim();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    if (url.CheckURL())
                    {
                        this.FTBChatInput.Text += string.Format(
                            Properties.Resources.fmtChatStyleImage,
                            ((this.FTBChatInput.Text.Length > 0) ? " " : ""),
                            url
                        );
                        this.TSTBInputImage.Text = "";
                        this.flatContextMenuStripChatInput.Close();
                    }
                    else
                    {
                        this._winMessageError(
                            string.Format(
                                Properties.Resources.fmtChatBadUrl,
                               url
                            )
                        );
                    }
                }
            }
        }
        private void TSTBInputUrl_KeyUp(object sender, KeyEventArgs e)
        {
            if (
                (sender == (object)this.TSTBInputUrl) &&
                (e.KeyCode == Keys.Return)
               )
            {
                string url = this.TSTBInputUrl.Text.Trim();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    if (url.CheckURL())
                    {
                        this.FTBChatInput.Text += string.Format(
                            Properties.Resources.fmtChatStyleUrl,
                            ((this.FTBChatInput.Text.Length > 0) ? " " : ""),
                            url,
                            url.HeaderURL()
                        );
                        this.TSTBInputUrl.Text = "";
                        this.flatContextMenuStripChatInput.Close();
                    }
                    else
                    {
                        this._winMessageError(
                            string.Format(
                                Properties.Resources.fmtChatBadUrl,
                               url
                            )
                        );
                    }
                }
            }
        }

        #endregion

        #endregion

        #region IRC UserList set and event

        #region IRC Autocomplete User nick to private message

        private void SetChatUserListAutocomplete()
        {
            if (this.FLVChatUser.Items.Count == 0)
            {
                return;
            }

            List<string> autolst = new List<string>();

            foreach (ListViewItem li in this.FLVChatUser.Items)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(li.Text))
                    {
                        throw new ArgumentNullException();
                    }
                    autolst.Add(li.Text);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            if (autolst.Count == 0)
            {
                return;
            }
            flatThreadSafe.Run(this, (Action)(() => this.TSTBFindNick.AutoCompleteCustomSource.Clear()));
            flatThreadSafe.Run(this, (Action)(() => this.TSTBFindNick.AutoCompleteCustomSource.AddRange(autolst.ToArray())));
        }

        #endregion

        private void SetUserListColumns()
        {
            flatThreadSafe.Run(this, (Action)(() => this.FLVChatUser.Clear()));
            flatThreadSafe.Run(this, (Action)(() => this.FLVChatUser.Columns.Add("nick", (this.FLVChatUser.Width - 25), HorizontalAlignment.Left)));
            flatThreadSafe.Run(this, (Action)(() => this.FLVChatUser.Columns.Add("tag", 0, HorizontalAlignment.Right)));
        }

        private void ChatUserListAdd(string user)
        {
            bool isUpdate = false;
            string tag = String.Empty, img = String.Empty, nick = user;

            lock (this._lockChatUserList)
            {
                if (nick.Length == 0)
                {
                    return;
                }
                if (nick[0] == '@')
                {
                    nick = nick.Substring(1, (nick.Length - 1)).Trim();
                    img = defaultIrcAdmin;
                }
                else
                {
                    img = defaultLeague;
                }

                if (this.FLVChatUser.Items.Count == 0)
                {
                    isUpdate = true;
                }
                else
                {
                    bool isFound = false;
                    foreach (ListViewItem item in this.FLVChatUser.Items)
                    {
                        if (item.Text.Equals(nick))
                        {
                            isFound = true;
                        }
                    }
                    isUpdate = !isFound;
                }
                if (isUpdate)
                {
                    if ((this._dtclan != null) && (this._dtclan.Rows.Count > 0) && (!img.Equals(defaultIrcAdmin)))
                    {
                        try
                        {
                            string clearnick = nick;

                            if (clearnick.EndsWith("_"))
                            {
                                int i = (clearnick.Length - 1);
                                for (; i >= 0; i--)
                                {
                                    if (clearnick[i] != '_')
                                    {
                                        break;
                                    }
                                }
                                clearnick = clearnick.Substring(0, (i + 1)).Trim();
                            }
                            DataRow dr = this._dtclan.Rows.Cast<DataRow>().FirstOrDefault(o => o.Field<string>("nik").Equals(clearnick));
                            if (dr != null)
                            {
                                tag = Convert.ToString(dr["tag"], CultureInfo.InvariantCulture);
                                img = Convert.ToString(dr["leagueid"], CultureInfo.InvariantCulture);
                            }
                        }
                        catch (Exception) { }
                    }
                    new FlatListViewItem(
                        this.FLVChatUser,
                        new string[]
                        {
                                nick,
                                tag
                        },
                        img
                    );
                    this.ChatUserListSortColor();
                    this.SetChatUserListAutocomplete();
                }
            }
        }

        private void ChatUserListClear()
        {
            lock (this._lockChatUserList)
            {
                if (this.FLVChatUser.Items.Count == 0)
                {
                    return;
                }

                foreach (ListViewItem li in this.FLVChatUser.Items)
                {
                    this.FLVChatUser.Items.Remove(li);
                }
            }
        }

        private void ChatUserListDel(string nick)
        {
            lock (this._lockChatUserList)
            {
                if (this.FLVChatUser.Items.Count == 0)
                {
                    return;
                }

                ListViewItem[] li = this.FLVChatUser.Items.Find(nick, false);

                if (li.Length > 0)
                {
                    this.FLVChatUser.Items.RemoveByKey(nick);
                    this.ChatUserListSortColor();
                    this.SetChatUserListAutocomplete();
                }
            }
        }

        private void ChatUserListSortColor()
        {
            int i = 0;

            foreach (ListViewItem item in this.FLVChatUser.Items)
            {
                if ((i % 2) == 1)
                {
                    item.BackColor = Color.FromArgb(60, 70, 73);
                }
                else
                {
                    item.BackColor = Color.FromArgb(45, 47, 49);
                }
                item.UseItemStyleForSubItems = true;
                i++;
            }
        }

        private string [] ChatUserListSelectedIndex()
        {
            if (this.FLVChatUser.SelectedItems.Count == 0)
            {
                this._winStatusBar(Properties.Settings.Default.USRServerVersion);
                return default(string[]);
            }
            else if (this.FLVChatUser.SelectedItems.Count > 1)
            {
                this._winMessageInfo(Properties.Resources.txtChatSelectNickMany);
                return default(string[]);
            }
            if (string.IsNullOrWhiteSpace(this.FLVChatUser.SelectedItems[0].Text))
            {
                this._winMessageError(Properties.Resources.txtChatSelectNickEmpty);
                return default(string []);
            }
            
            return new string [] {
                this.FLVChatUser.SelectedItems[0].Text,
                this.FLVChatUser.SelectedItems[0].ImageKey
            };
        }

        private void FLVChatUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string [] ircuser = this.ChatUserListSelectedIndex();
            if (
                (ircuser == default(string[])) ||
                (ircuser[0].Length == 0) ||
                (ircuser[1].Length == 0)
               )
            {
                return;
            }
            this._winStatusBar(
                string.Format(
                    Properties.Resources.fmtChatSelectedNick,
                    ircuser[0]
                )
            );
        }

        private void FLVChatUser_Click(object sender, EventArgs e)
        {
            string [] ircuser = this.ChatUserListSelectedIndex();
            if (
                (ircuser == default(string[])) ||
                (ircuser[0].Length == 0) ||
                (ircuser[1].Length == 0)
               )
            {
                return;
            }
            this.WBChat_GetChatMenu((object)ircuser[0]);
        }

        #endregion


    }
}