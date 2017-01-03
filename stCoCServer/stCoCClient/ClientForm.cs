using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using stCore;
using stCoreUI;
using stClient;

namespace stCoCClient
{
    public partial class ClientForm : Form
    {
        public  InformerForm _informerform = null;
        private ClientForm _mainform = null;
        private Control.IMGInformerControl _IMGInformerControl = null;
        private Control.TBSpellControl _TBSControl = null;
        private Dictionary<TabCtrlName, TabPage> _alltab;
        private TabPage _ctab = null;
        private bool _isCoCServChanged = false;
        private bool _isGameTagChanged = false;
        public IMessage _iLog = null;

        enum TabCtrlName : int
        {
            tabNone = -1,
            tabPageSetupMain = 0,
            tabPageSetupUser,
            tabPageSetupReg,
            tabPageClanNews,
            tabPageClanStat,
            tabPageClanChat,
            tabPageHelp
        }

        public bool isMinimized
        {
            get { return this.notifyIconMain.Visible; }
        }

        public ClientForm()
        {
            this._mainform = this;
            this._alltab = new Dictionary<TabCtrlName, TabPage>();
            this._iLog = new IMessage()
            {
                LogOk = this._winMessageSuccess,
                LogInfo = this._winMessageInfo,
                LogError = this._winMessageError,
                Line = this._winStatusBar
            };

            webwLock = new Object();
            _chatHistory = new ListCircular<string>(Properties.Settings.Default.IRCChatHistory); 

            InitializeComponent();

            _TBSControl = new Control.TBSpellControl(this.FTBChatInput, this._iLog);
            this.tabPageClanChat.Controls.Add(_TBSControl);
            _IMGInformerControl = new Control.IMGInformerControl(this);
            this.flatGroupBox5.Controls.Add(_IMGInformerControl);

            this.TSTBFindNick.KeyUp += new KeyEventHandler(TSTBFindNick_KeyUp);
            this.TSTBNotifyFind.KeyUp += new KeyEventHandler(TSTBNotifyFind_KeyUp);
            this.TSCBGroup.KeyUp += new KeyEventHandler(TSCBGroup_KeyUp);
            this.TSCBGroup.SelectedIndexChanged += new EventHandler(TSCBGroup_SelectedIndexChanged);

            /// Not work in designer, auto remove it..
            this.flatContextMenuStripChatBrowser.ShowCheckMargin = false;
            this.flatContextMenuStripChatBrowser.ShowImageMargin = true;

            this.flatContextMenuStripChatInput.ShowCheckMargin = false;
            this.flatContextMenuStripChatInput.ShowImageMargin = true;

            this.flatContextMenuStripChatWeb.ShowCheckMargin = false;
            this.flatContextMenuStripChatWeb.ShowImageMargin = true;

            this.flatContextMenuStripTray.ShowCheckMargin = false;
            this.flatContextMenuStripTray.ShowImageMargin = true;

            this.notifyIconMain.Visible = false;

            this.ImageInformer = (Image)Properties.Resources.InformerLoading;
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            foreach (TabPage tab in this.flatTabControlMain.TabPages)
            {
                TabCtrlName tcn;

                try
                {
                    tcn = (TabCtrlName)Enum.Parse(typeof(TabCtrlName), tab.Name);
                    this._alltab.Add(tcn, tab);
                }
                catch (Exception ex)
                {
                    this._winMessageError(ex.Message);
                    continue;
                }

                if (
                    (tcn == TabCtrlName.tabPageSetupMain) ||
                    (tcn == TabCtrlName.tabPageSetupUser) ||
                    (tcn == TabCtrlName.tabPageSetupReg) ||
                    (tcn == TabCtrlName.tabPageHelp)
                   )
                {
                    tab.Parent = null;
                }
            }
            if (!this._CheckSetupCoCServer())
            {
                this._TabSelector(TabCtrlName.tabPageSetupUser, true);
                this.errorProviderSetup.Clear();
            }
            else
            {
                if (this._CheckTimerNotify())
                {
                    this.RunNotify();
                }
            }
            this.SetUserListColumns();
            this._WBInit();
            this._IRCInit();
        }

        private void flatMinimize_Click(object sender, EventArgs e)
        {
            this.ClientForm_MinimumSizeChanged(sender, e);
        }

        private void ClientForm_MinimumSizeChanged(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.Text = this.formSkinMain.Text + " - " + DateTime.Now.ToShortTimeString();
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.notifyIconMain.Visible = false;
        }

        private void PBCallInformer_Click(object sender, EventArgs e)
        {
            if (
                (this._informerform == null) &&
                (this._CheckSetupInformer())
               )
            {
                this._informerform = new InformerForm(this);
                this._informerform.Show();
            }
        }

    }
}
