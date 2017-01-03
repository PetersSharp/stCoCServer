using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Net;
using System.Windows.Forms;
using stCoreUI;
using stClient;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Drawing;

namespace stCoCClient
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public partial class ClientForm : Form
    {
        private string selectedNick = String.Empty;

        private void WBChat_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                if (
                    (!(e.Url.ToString().Contains("about:blank"))) &&
                    (!(e.Url.ToString().Contains("javascript:void(0)")))
                   )
                {
                    System.Diagnostics.Process.Start(e.Url.ToString());
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
                e.Cancel = true;
            }
        }
        public void WBChat_GetChatMenu(object obj)
        {
            this.selectedNick = obj as string;
            if (!string.IsNullOrWhiteSpace(this.selectedNick))
            {
                if (this.selectedNick.Contains("-&gt;"))
                {
                    string [] parts = this.selectedNick.Split(new char [] { ';' });
                    if (parts.Length > 1)
                    {
                        this.selectedNick = parts[1].Trim();
                    }
                }
                if (Properties.Settings.Default.IRCNik.Equals(this.selectedNick))
                {
                    this.TSMIReplyNick.Enabled = false;
                    this.TSMIPrivateMsg.Enabled = false;
                    this.TSMIKickNick.Enabled = false;
                }
                else
                {
                    this.TSMIReplyNick.Enabled = true;
                    this.TSMIPrivateMsg.Enabled = true;
                    this.TSMIKickNick.Enabled = true;
                }
                this.TSMIReplyNick.Text = string.Format(Properties.Resources.txtMenuNickReply, this.selectedNick);
                this.TSMIPrivateMsg.Text = string.Format(Properties.Resources.txtMenuNickPrivate, this.selectedNick);
                this.TSMIKickNick.Text = string.Format(Properties.Resources.txtMenuNickKick, this.selectedNick);

                this.TSMIInsertNick.Text = string.Format(Properties.Resources.txtMenuNickInsert, this.selectedNick);
                this.flatContextMenuStripChatWeb.Show(new Point(Cursor.Position.X, Cursor.Position.Y));
            }
        }
    }
}
