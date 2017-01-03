using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stCore;
using stCoreUI;
using stClient;
using System.Globalization;
using System.ComponentModel;

namespace stCoCClient
{
    public partial class ClientForm : Form
    {
        private readonly object _lockBallon = new Object();

        private void notifyIconMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.PBCallInformer_Click(sender, e as EventArgs);
            }
        }

        private void notifyIconMain_DoubleClick(object sender, EventArgs e)
        {
            this._ShowApp();
        }

        private void notifyIconMain_Balloon(string body, string title = null, ToolTipIcon ti = ToolTipIcon.None, int tm = 0)
        {
            if (this.isMinimized)
            {
                lock (this._lockBallon)
                {
                    flatThreadSafe.Run(this, (Action)(() => this.notifyIconMain.BalloonTipIcon = ti));
                    flatThreadSafe.Run(this, (Action)(() => this.notifyIconMain.BalloonTipTitle = ((string.IsNullOrEmpty(title)) ? "" : title)));
                    flatThreadSafe.Run(this, (Action)(() => this.notifyIconMain.BalloonTipText = ((string.IsNullOrEmpty(body)) ? "" : body)));
                    flatThreadSafe.Run(this, (Action)(() => this.notifyIconMain.ShowBalloonTip((tm > 0) ? tm : 1000)));
                }
            }
        }

        #region Tray Context strp menu

        private void flatContextMenuStripTray_Opening(object sender, CancelEventArgs e)
        {
            if (this._CheckSetupIRCChat())
            {
                this.TSMIOpenClanChat.Visible = true;
            }
            else
            {
                this.TSMIOpenClanChat.Visible = false;
            }
            if (this._CheckSetupSSENotify())
            {
                this.TSMIOpenClanStat.Visible = true;
            }
            else
            {
                this.TSMIOpenClanStat.Visible = false;
            }
        }

        private void TSMIAppExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TSMIOpenApp_Click(object sender, EventArgs e)
        {
            this._ShowApp();
        }

        private void TSMIOpenSetup_Click(object sender, EventArgs e)
        {
            this._TabSelector(TabCtrlName.tabPageSetupMain, true);
            this._ShowApp();
        }

        private void TSMIOpenClanChat_Click(object sender, EventArgs e)
        {
            if (this._CheckSetupIRCChat())
            {
                this._TabSelect(TabCtrlName.tabPageClanChat);
            }
            this._ShowApp();
        }

        private void TSMIOpenClanStat_Click(object sender, EventArgs e)
        {
            if (this._CheckSetupSSENotify())
            {
                this._TabSelect(TabCtrlName.tabPageClanStat);
            }
            this._ShowApp();
        }

        private void TSMIOpenInformer_Click(object sender, EventArgs e)
        {
            this.PBCallInformer_Click(sender, e);
        }

        #endregion

        private void _ShowApp()
        {
            this.notifyIconMain.Visible = false;
            this.ShowInTaskbar = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Focus();
        }

    }
}
