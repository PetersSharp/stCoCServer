namespace stCoCClient
{
    partial class ClientForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.errorProviderSetup = new System.Windows.Forms.ErrorProvider(this.components);
            this.FTBCoCServer1 = new stCoreUI.FlatTextBox();
            this.FTBGameTag1 = new stCoreUI.FlatTextBox();
            this.FTBIrcNick1 = new stCoreUI.FlatTextBox();
            this.imageListNotify = new System.Windows.Forms.ImageList(this.components);
            this.bgNotifyWorker = new System.ComponentModel.BackgroundWorker();
            this.FBDNotifyExport = new System.Windows.Forms.FolderBrowserDialog();
            this.TimerRunNotify = new System.Windows.Forms.Timer(this.components);
            this.imageListChat = new System.Windows.Forms.ImageList(this.components);
            this.colorDialogInput = new System.Windows.Forms.ColorDialog();
            this.formSkinMain = new stCoreUI.FormSkin();
            this.PBCallInformer = new System.Windows.Forms.PictureBox();
            this.picBoxSetupMain = new System.Windows.Forms.PictureBox();
            this.flatAlertBoxMain = new stCoreUI.FlatAlertBox();
            this.flatSB = new stCoreUI.FlatStatusBar();
            this.flatContextMenuStripChatInput = new stCoreUI.FlatContextMenuStrip();
            this.TSMIInputPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputClear = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIInputSpell = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIInputSmilesNorm = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles1 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles2 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles3 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles4 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles5 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles6 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles7 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles8 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles9 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles10 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputSmilesBlink = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles11 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles12 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles13 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles14 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles15 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles16 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles17 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles18 = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMISmiles19 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIInputStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputStyleBold = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputStyleItalic = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputStyleUnderline = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputStyleStrike = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColor = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColorRed = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColorYellow = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColorGreen = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColorCyan = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColorBlue = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColorMagenta = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputColorCustom = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputImage = new System.Windows.Forms.ToolStripMenuItem();
            this.TSTBInputImage = new System.Windows.Forms.ToolStripTextBox();
            this.TSMIInputUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.TSTBInputUrl = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIInputSend = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIInputSendPriv = new System.Windows.Forms.ToolStripMenuItem();
            this.TSTBFindNick = new System.Windows.Forms.ToolStripTextBox();
            this.flatTabControlMain = new stCoreUI.FlatTabControl();
            this.tabPageSetupMain = new System.Windows.Forms.TabPage();
            this.bboxReload = new stCoreUI.FlatButton();
            this.bboxSave = new stCoreUI.FlatButton();
            this.flatGroupBox3 = new stCoreUI.FlatGroupBox();
            this.cboxWarClanEnd = new stCoreUI.FlatCheckBox();
            this.cboxClanChangeMembers = new stCoreUI.FlatCheckBox();
            this.cboxClanChangePoints = new stCoreUI.FlatCheckBox();
            this.cboxMemberChangeDonationReceive = new stCoreUI.FlatCheckBox();
            this.cboxMemberChangeDonationSend = new stCoreUI.FlatCheckBox();
            this.cboxMemberChangeTrophies = new stCoreUI.FlatCheckBox();
            this.cboxMemberChangeLeague = new stCoreUI.FlatCheckBox();
            this.cboxMemberChangeLevel = new stCoreUI.FlatCheckBox();
            this.cboxMemberChangeRole = new stCoreUI.FlatCheckBox();
            this.flatCheckBox4 = new stCoreUI.FlatCheckBox();
            this.cboxMemberChangeNik = new stCoreUI.FlatCheckBox();
            this.cboxMemberExit = new stCoreUI.FlatCheckBox();
            this.cboxMemberNew = new stCoreUI.FlatCheckBox();
            this.flatGroupBox2 = new stCoreUI.FlatGroupBox();
            this.FTBExportPath = new stCoreUI.FlatTextBox();
            this.pictureBox15 = new System.Windows.Forms.PictureBox();
            this.FSBExportPath = new stCoreUI.FlatStickyButton();
            this.flatLabel2 = new stCoreUI.FlatLabel();
            this.FSBRegistred = new stCoreUI.FlatStickyButton();
            this.FBWizard = new stCoreUI.FlatButton();
            this.flatNumericInformerId1 = new stCoreUI.FlatNumeric();
            this.flatLabel12 = new stCoreUI.FlatLabel();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.FTBGameTag2 = new stCoreUI.FlatTextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.FTBIrcPass2 = new stCoreUI.FlatTextBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.FTBIrcNick2 = new stCoreUI.FlatTextBox();
            this.FTBCoCServer2ro = new stCoreUI.FlatTextBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.flatGroupBox1 = new stCoreUI.FlatGroupBox();
            this.pictureBox14 = new System.Windows.Forms.PictureBox();
            this.FTBWikiHome = new stCoreUI.FlatTextBox();
            this.flatLabel10 = new stCoreUI.FlatLabel();
            this.flatToggle4 = new stCoreUI.FlatToggle();
            this.flatLabel9 = new stCoreUI.FlatLabel();
            this.flatToggle3 = new stCoreUI.FlatToggle();
            this.flatLabel8 = new stCoreUI.FlatLabel();
            this.flatToggle2 = new stCoreUI.FlatToggle();
            this.flatLabel7 = new stCoreUI.FlatLabel();
            this.flatToggle1 = new stCoreUI.FlatToggle();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.FTBIRCChannel = new stCoreUI.FlatTextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.FTBIRCPort = new stCoreUI.FlatTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.FTBIRCServer = new stCoreUI.FlatTextBox();
            this.tabPageSetupUser = new System.Windows.Forms.TabPage();
            this.flatGroupBox5 = new stCoreUI.FlatGroupBox();
            this.FLVSetup = new System.Windows.Forms.ListView();
            this.flatGroupBox4 = new stCoreUI.FlatGroupBox();
            this.FCBNikAuto = new stCoreUI.FlatCheckBox();
            this.flatButton4 = new stCoreUI.FlatButton();
            this.flatButton2 = new stCoreUI.FlatButton();
            this.flatButton3 = new stCoreUI.FlatButton();
            this.pictureBox13 = new System.Windows.Forms.PictureBox();
            this.flatNumericInformerId2 = new stCoreUI.FlatNumeric();
            this.flatLabel1 = new stCoreUI.FlatLabel();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.pictureBox11 = new System.Windows.Forms.PictureBox();
            this.FTBIrcPass1 = new stCoreUI.FlatTextBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.tabPageSetupReg = new System.Windows.Forms.TabPage();
            this.tabPageClanNews = new System.Windows.Forms.TabPage();
            this.tabPageClanStat = new System.Windows.Forms.TabPage();
            this.flatContextMenuStripNotify = new stCoreUI.FlatContextMenuStrip();
            this.TSMINotifyMultiselect = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINotifyGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINotifyScroll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMINotifyExport = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINotifyExportTXT = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINotifyExportCSV = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINotifyExportHTML = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMINotifyCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMINotifySelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.TSTBNotifyFind = new System.Windows.Forms.ToolStripTextBox();
            this.TSCBGroup = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMINotifyRenew = new System.Windows.Forms.ToolStripMenuItem();
            this.FLVNotify = new System.Windows.Forms.ListView();
            this.PBNotifyRenew = new System.Windows.Forms.PictureBox();
            this.FPBNotify = new stCoreUI.FlatProgressBar();
            this.tabPageClanChat = new System.Windows.Forms.TabPage();
            this.PBChatSend = new System.Windows.Forms.PictureBox();
            this.FLVChatUser = new System.Windows.Forms.ListView();
            this.PBChatMsgType = new System.Windows.Forms.PictureBox();
            this.WBChat = new System.Windows.Forms.WebBrowser();
            this.flatContextMenuStripChatBrowser = new stCoreUI.FlatContextMenuStrip();
            this.TSMIChatClear = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIChatExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIChatArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.FBChatConnect = new stCoreUI.FlatButton();
            this.FTBChatInput = new stCoreUI.FlatTextBox();
            this.flatClose1 = new stCoreUI.FlatClose();
            this.flatMinimize = new stCoreUI.FlatMini();
            this.flatContextMenuStripChatWeb = new stCoreUI.FlatContextMenuStrip();
            this.TSMIInsertNick = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIReplyNick = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMICopyNick = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIPrivateMsg = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIKickNick = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIReloadUserList = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.flatContextMenuStripTray = new stCoreUI.FlatContextMenuStrip();
            this.TSMIOpenApp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIOpenClanStat = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIOpenClanChat = new System.Windows.Forms.ToolStripMenuItem();
            this.TSMIOpenSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIOpenInformer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.TSMIAppExit = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderSetup)).BeginInit();
            this.formSkinMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCallInformer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSetupMain)).BeginInit();
            this.flatContextMenuStripChatInput.SuspendLayout();
            this.flatTabControlMain.SuspendLayout();
            this.tabPageSetupMain.SuspendLayout();
            this.flatGroupBox3.SuspendLayout();
            this.flatGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.flatGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPageSetupUser.SuspendLayout();
            this.flatGroupBox5.SuspendLayout();
            this.flatGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            this.tabPageClanStat.SuspendLayout();
            this.flatContextMenuStripNotify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBNotifyRenew)).BeginInit();
            this.tabPageClanChat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBChatSend)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBChatMsgType)).BeginInit();
            this.flatContextMenuStripChatBrowser.SuspendLayout();
            this.flatContextMenuStripChatWeb.SuspendLayout();
            this.flatContextMenuStripTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProviderSetup
            // 
            this.errorProviderSetup.ContainerControl = this;
            // 
            // FTBCoCServer1
            // 
            this.FTBCoCServer1.BackColor = System.Drawing.Color.DimGray;
            this.FTBCoCServer1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "USRCoCServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProviderSetup.SetError(this.FTBCoCServer1, resources.GetString("FTBCoCServer1.Error"));
            this.FTBCoCServer1.FocusOnHover = true;
            resources.ApplyResources(this.FTBCoCServer1, "FTBCoCServer1");
            this.FTBCoCServer1.MaxLength = 32767;
            this.FTBCoCServer1.Multiline = false;
            this.FTBCoCServer1.Name = "FTBCoCServer1";
            this.FTBCoCServer1.ReadOnly = false;
            this.FTBCoCServer1.SelectedText = "";
            this.FTBCoCServer1.SelectionLength = 0;
            this.FTBCoCServer1.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBCoCServer1.SpellMark")));
            this.FTBCoCServer1.Text = global::stCoCClient.Properties.Settings.Default.USRCoCServer;
            this.FTBCoCServer1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBCoCServer1.TextCaption = "CoC Server: http(s):// IP/DNS";
            this.FTBCoCServer1.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBCoCServer1.UseSystemPasswordChar = false;
            this.FTBCoCServer1.TextChanged += new System.EventHandler(this.FTBCoCServer_TextChanged);
            this.FTBCoCServer1.Enter += new System.EventHandler(this.tboxCaption_Enter);
            this.FTBCoCServer1.Leave += new System.EventHandler(this.FTBCoCServer_Leave);
            // 
            // FTBGameTag1
            // 
            this.FTBGameTag1.BackColor = System.Drawing.Color.DimGray;
            this.FTBGameTag1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "USRCoCTag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProviderSetup.SetError(this.FTBGameTag1, resources.GetString("FTBGameTag1.Error"));
            this.FTBGameTag1.FocusOnHover = true;
            resources.ApplyResources(this.FTBGameTag1, "FTBGameTag1");
            this.FTBGameTag1.MaxLength = 32767;
            this.FTBGameTag1.Multiline = false;
            this.FTBGameTag1.Name = "FTBGameTag1";
            this.FTBGameTag1.ReadOnly = false;
            this.FTBGameTag1.SelectedText = "";
            this.FTBGameTag1.SelectionLength = 0;
            this.FTBGameTag1.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBGameTag1.SpellMark")));
            this.FTBGameTag1.Text = global::stCoCClient.Properties.Settings.Default.USRCoCTag;
            this.FTBGameTag1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBGameTag1.TextCaption = "CoC game user tag, no # add to this";
            this.FTBGameTag1.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBGameTag1.UseSystemPasswordChar = false;
            this.FTBGameTag1.TextChanged += new System.EventHandler(this.FTBGameTag_TextChanged);
            this.FTBGameTag1.Enter += new System.EventHandler(this.tboxCaption_Enter);
            this.FTBGameTag1.Leave += new System.EventHandler(this.CheckGameTag_Click);
            // 
            // FTBIrcNick1
            // 
            this.FTBIrcNick1.BackColor = System.Drawing.Color.DimGray;
            this.FTBIrcNick1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "IRCNik", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.errorProviderSetup.SetError(this.FTBIrcNick1, resources.GetString("FTBIrcNick1.Error"));
            this.FTBIrcNick1.FocusOnHover = true;
            resources.ApplyResources(this.FTBIrcNick1, "FTBIrcNick1");
            this.FTBIrcNick1.MaxLength = 32767;
            this.FTBIrcNick1.Multiline = false;
            this.FTBIrcNick1.Name = "FTBIrcNick1";
            this.FTBIrcNick1.ReadOnly = false;
            this.FTBIrcNick1.SelectedText = "";
            this.FTBIrcNick1.SelectionLength = 0;
            this.FTBIrcNick1.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBIrcNick1.SpellMark")));
            this.FTBIrcNick1.Text = global::stCoCClient.Properties.Settings.Default.IRCNik;
            this.FTBIrcNick1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBIrcNick1.TextCaption = "IRC user nik name";
            this.FTBIrcNick1.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBIrcNick1.UseSystemPasswordChar = false;
            this.FTBIrcNick1.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // imageListNotify
            // 
            this.imageListNotify.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListNotify.ImageStream")));
            this.imageListNotify.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListNotify.Images.SetKeyName(0, "MemberChangeRole");
            this.imageListNotify.Images.SetKeyName(1, "ClanChangeLevel");
            this.imageListNotify.Images.SetKeyName(2, "ClanChangeMembers");
            this.imageListNotify.Images.SetKeyName(3, "ClanChangeName");
            this.imageListNotify.Images.SetKeyName(4, "ClanChangePoints");
            this.imageListNotify.Images.SetKeyName(5, "ClanChangeTrophies");
            this.imageListNotify.Images.SetKeyName(6, "ClanChangeType");
            this.imageListNotify.Images.SetKeyName(7, "ClanChangeWarFrequency");
            this.imageListNotify.Images.SetKeyName(8, "ClanChangeWarPublic");
            this.imageListNotify.Images.SetKeyName(9, "ClanChangeWarSeries");
            this.imageListNotify.Images.SetKeyName(10, "ClanChangeWarWin");
            this.imageListNotify.Images.SetKeyName(11, "MemberChangeNik");
            this.imageListNotify.Images.SetKeyName(12, "MemberChangeTrophies");
            this.imageListNotify.Images.SetKeyName(13, "MemberExit");
            this.imageListNotify.Images.SetKeyName(14, "MemberNew");
            this.imageListNotify.Images.SetKeyName(15, "WarClanEnd");
            this.imageListNotify.Images.SetKeyName(16, "MemberChangeDonationReceive");
            this.imageListNotify.Images.SetKeyName(17, "MemberChangeDonationSend");
            this.imageListNotify.Images.SetKeyName(18, "MemberChangeLeague");
            this.imageListNotify.Images.SetKeyName(19, "MemberChangeLevel");
            // 
            // bgNotifyWorker
            // 
            this.bgNotifyWorker.WorkerReportsProgress = true;
            this.bgNotifyWorker.WorkerSupportsCancellation = true;
            this.bgNotifyWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgNotifyWorker_DoWork);
            this.bgNotifyWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgNotifyWorker_ProgressChanged);
            this.bgNotifyWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgNotifyWorker_RunWorkerCompleted);
            // 
            // FBDNotifyExport
            // 
            resources.ApplyResources(this.FBDNotifyExport, "FBDNotifyExport");
            this.FBDNotifyExport.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.FBDNotifyExport.SelectedPath = global::stCoCClient.Properties.Settings.Default.ExportPath;
            // 
            // TimerRunNotify
            // 
            this.TimerRunNotify.Interval = global::stCoCClient.Properties.Settings.Default.USRTimerNotifyInterval;
            this.TimerRunNotify.Tick += new System.EventHandler(this.TimerRunNotify_Tick);
            // 
            // imageListChat
            // 
            this.imageListChat.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListChat.ImageStream")));
            this.imageListChat.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListChat.Images.SetKeyName(0, "29000022");
            this.imageListChat.Images.SetKeyName(1, "29000000");
            this.imageListChat.Images.SetKeyName(2, "29000001");
            this.imageListChat.Images.SetKeyName(3, "29000002");
            this.imageListChat.Images.SetKeyName(4, "29000003");
            this.imageListChat.Images.SetKeyName(5, "29000004");
            this.imageListChat.Images.SetKeyName(6, "29000005");
            this.imageListChat.Images.SetKeyName(7, "29000006");
            this.imageListChat.Images.SetKeyName(8, "29000007");
            this.imageListChat.Images.SetKeyName(9, "29000008");
            this.imageListChat.Images.SetKeyName(10, "29000009");
            this.imageListChat.Images.SetKeyName(11, "29000010");
            this.imageListChat.Images.SetKeyName(12, "29000011");
            this.imageListChat.Images.SetKeyName(13, "29000012");
            this.imageListChat.Images.SetKeyName(14, "29000013");
            this.imageListChat.Images.SetKeyName(15, "29000014");
            this.imageListChat.Images.SetKeyName(16, "29000015");
            this.imageListChat.Images.SetKeyName(17, "29000016");
            this.imageListChat.Images.SetKeyName(18, "29000017");
            this.imageListChat.Images.SetKeyName(19, "29000018");
            this.imageListChat.Images.SetKeyName(20, "29000019");
            this.imageListChat.Images.SetKeyName(21, "29000020");
            this.imageListChat.Images.SetKeyName(22, "29000021");
            this.imageListChat.Images.SetKeyName(23, "00000001");
            // 
            // colorDialogInput
            // 
            this.colorDialogInput.AnyColor = true;
            this.colorDialogInput.Color = System.Drawing.Color.White;
            // 
            // formSkinMain
            // 
            this.formSkinMain.BackColor = System.Drawing.Color.White;
            this.formSkinMain.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.formSkinMain.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(58)))), ((int)(((byte)(60)))));
            this.formSkinMain.Controls.Add(this.PBCallInformer);
            this.formSkinMain.Controls.Add(this.picBoxSetupMain);
            this.formSkinMain.Controls.Add(this.flatAlertBoxMain);
            this.formSkinMain.Controls.Add(this.flatSB);
            this.formSkinMain.Controls.Add(this.flatTabControlMain);
            this.formSkinMain.Controls.Add(this.flatClose1);
            this.formSkinMain.Controls.Add(this.flatMinimize);
            this.formSkinMain.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.formSkinMain, "formSkinMain");
            this.formSkinMain.DoubleBufferAlways = false;
            this.formSkinMain.FlatColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.formSkinMain.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.formSkinMain.HeaderMaximize = false;
            this.formSkinMain.Name = "formSkinMain";
            this.formSkinMain.WinShadow = false;
            // 
            // PBCallInformer
            // 
            this.PBCallInformer.BackColor = System.Drawing.Color.Transparent;
            this.PBCallInformer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PBCallInformer.Image = global::stCoCClient.Properties.Resources.ic_error_outline_white_18dp;
            resources.ApplyResources(this.PBCallInformer, "PBCallInformer");
            this.PBCallInformer.Name = "PBCallInformer";
            this.PBCallInformer.TabStop = false;
            this.PBCallInformer.Click += new System.EventHandler(this.PBCallInformer_Click);
            // 
            // picBoxSetupMain
            // 
            this.picBoxSetupMain.BackColor = System.Drawing.Color.Transparent;
            this.picBoxSetupMain.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.picBoxSetupMain, "picBoxSetupMain");
            this.picBoxSetupMain.Name = "picBoxSetupMain";
            this.picBoxSetupMain.TabStop = false;
            this.picBoxSetupMain.Click += new System.EventHandler(this.picBoxSetupMain_Click);
            // 
            // flatAlertBoxMain
            // 
            this.flatAlertBoxMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.flatAlertBoxMain.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.flatAlertBoxMain, "flatAlertBoxMain");
            this.flatAlertBoxMain.kind = stCoreUI.FlatAlertBox._Kind.Success;
            this.flatAlertBoxMain.Name = "flatAlertBoxMain";
            // 
            // flatSB
            // 
            this.flatSB.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatSB.ContextMenuStrip = this.flatContextMenuStripChatInput;
            resources.ApplyResources(this.flatSB, "flatSB");
            this.flatSB.ForeColor = System.Drawing.Color.White;
            this.flatSB.Name = "flatSB";
            this.flatSB.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatSB.ShowTimeDate = false;
            this.flatSB.TextColor = System.Drawing.Color.White;
            // 
            // flatContextMenuStripChatInput
            // 
            resources.ApplyResources(this.flatContextMenuStripChatInput, "flatContextMenuStripChatInput");
            this.flatContextMenuStripChatInput.ForeColor = System.Drawing.Color.White;
            this.flatContextMenuStripChatInput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIInputPaste,
            this.TSMIInputClear,
            this.TSMIInputCopy,
            this.TSMIInputSelect,
            this.toolStripSeparator4,
            this.TSMIInputSpell,
            this.toolStripSeparator5,
            this.TSMIInputSmilesNorm,
            this.TSMIInputSmilesBlink,
            this.toolStripSeparator7,
            this.TSMIInputStyle,
            this.TSMIInputColor,
            this.TSMIInputImage,
            this.TSMIInputUrl,
            this.toolStripSeparator8,
            this.TSMIInputSend,
            this.TSMIInputSendPriv});
            this.flatContextMenuStripChatInput.Name = "flatContextMenuStripMainSetup";
            this.flatContextMenuStripChatInput.Opening += new System.ComponentModel.CancelEventHandler(this.flatContextMenuStripChatInput_Opening);
            // 
            // TSMIInputPaste
            // 
            this.TSMIInputPaste.Image = global::stCoCClient.Properties.Resources.paste;
            this.TSMIInputPaste.Name = "TSMIInputPaste";
            resources.ApplyResources(this.TSMIInputPaste, "TSMIInputPaste");
            this.TSMIInputPaste.Click += new System.EventHandler(this.TSMIInputPaste_Click);
            // 
            // TSMIInputClear
            // 
            this.TSMIInputClear.Image = global::stCoCClient.Properties.Resources.cut;
            this.TSMIInputClear.Name = "TSMIInputClear";
            resources.ApplyResources(this.TSMIInputClear, "TSMIInputClear");
            this.TSMIInputClear.Click += new System.EventHandler(this.TSMIInputClear_Click);
            // 
            // TSMIInputCopy
            // 
            this.TSMIInputCopy.Image = global::stCoCClient.Properties.Resources.copy;
            this.TSMIInputCopy.Name = "TSMIInputCopy";
            resources.ApplyResources(this.TSMIInputCopy, "TSMIInputCopy");
            this.TSMIInputCopy.Click += new System.EventHandler(this.TSMIInputCopy_Click);
            // 
            // TSMIInputSelect
            // 
            this.TSMIInputSelect.Image = global::stCoCClient.Properties.Resources.select;
            this.TSMIInputSelect.Name = "TSMIInputSelect";
            resources.ApplyResources(this.TSMIInputSelect, "TSMIInputSelect");
            this.TSMIInputSelect.Click += new System.EventHandler(this.TSMIInputSelect_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // TSMIInputSpell
            // 
            this.TSMIInputSpell.Image = global::stCoCClient.Properties.Resources.ic_spellcheck_white_18dp;
            this.TSMIInputSpell.Name = "TSMIInputSpell";
            resources.ApplyResources(this.TSMIInputSpell, "TSMIInputSpell");
            this.TSMIInputSpell.Click += new System.EventHandler(this.TSMIInputSpell_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            // 
            // TSMIInputSmilesNorm
            // 
            this.TSMIInputSmilesNorm.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMISmiles1,
            this.TSMISmiles2,
            this.TSMISmiles3,
            this.TSMISmiles4,
            this.TSMISmiles5,
            this.TSMISmiles6,
            this.TSMISmiles7,
            this.TSMISmiles8,
            this.TSMISmiles9,
            this.TSMISmiles10});
            this.TSMIInputSmilesNorm.Image = global::stCoCClient.Properties.Resources.emoji_hopeless;
            this.TSMIInputSmilesNorm.Name = "TSMIInputSmilesNorm";
            resources.ApplyResources(this.TSMIInputSmilesNorm, "TSMIInputSmilesNorm");
            // 
            // TSMISmiles1
            // 
            this.TSMISmiles1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles1.Image = global::stCoCClient.Properties.Resources.emoji_happy;
            this.TSMISmiles1.Name = "TSMISmiles1";
            resources.ApplyResources(this.TSMISmiles1, "TSMISmiles1");
            this.TSMISmiles1.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles2
            // 
            this.TSMISmiles2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles2.Image = global::stCoCClient.Properties.Resources.emoji_hopeless;
            this.TSMISmiles2.Name = "TSMISmiles2";
            resources.ApplyResources(this.TSMISmiles2, "TSMISmiles2");
            this.TSMISmiles2.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles3
            // 
            this.TSMISmiles3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles3.Image = global::stCoCClient.Properties.Resources.emoji_weirdout;
            this.TSMISmiles3.Name = "TSMISmiles3";
            resources.ApplyResources(this.TSMISmiles3, "TSMISmiles3");
            this.TSMISmiles3.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles4
            // 
            this.TSMISmiles4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles4.Image = global::stCoCClient.Properties.Resources.emoji_anger;
            this.TSMISmiles4.Name = "TSMISmiles4";
            resources.ApplyResources(this.TSMISmiles4, "TSMISmiles4");
            this.TSMISmiles4.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles5
            // 
            this.TSMISmiles5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles5.Image = global::stCoCClient.Properties.Resources.emoji_sadistic;
            this.TSMISmiles5.Name = "TSMISmiles5";
            resources.ApplyResources(this.TSMISmiles5, "TSMISmiles5");
            this.TSMISmiles5.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles6
            // 
            this.TSMISmiles6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles6.Image = global::stCoCClient.Properties.Resources.emoji_glad;
            this.TSMISmiles6.Name = "TSMISmiles6";
            resources.ApplyResources(this.TSMISmiles6, "TSMISmiles6");
            this.TSMISmiles6.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles7
            // 
            this.TSMISmiles7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles7.Image = global::stCoCClient.Properties.Resources.emoji_blushing;
            this.TSMISmiles7.Name = "TSMISmiles7";
            resources.ApplyResources(this.TSMISmiles7, "TSMISmiles7");
            this.TSMISmiles7.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles8
            // 
            this.TSMISmiles8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles8.Image = global::stCoCClient.Properties.Resources.emoji_nervous;
            this.TSMISmiles8.Name = "TSMISmiles8";
            resources.ApplyResources(this.TSMISmiles8, "TSMISmiles8");
            this.TSMISmiles8.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles9
            // 
            this.TSMISmiles9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles9.Image = global::stCoCClient.Properties.Resources.emoji_amazed;
            this.TSMISmiles9.Name = "TSMISmiles9";
            resources.ApplyResources(this.TSMISmiles9, "TSMISmiles9");
            this.TSMISmiles9.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles10
            // 
            this.TSMISmiles10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles10.Image = global::stCoCClient.Properties.Resources.emoji_blank;
            this.TSMISmiles10.Name = "TSMISmiles10";
            resources.ApplyResources(this.TSMISmiles10, "TSMISmiles10");
            this.TSMISmiles10.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMIInputSmilesBlink
            // 
            this.TSMIInputSmilesBlink.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMISmiles11,
            this.TSMISmiles12,
            this.TSMISmiles13,
            this.TSMISmiles14,
            this.TSMISmiles15,
            this.TSMISmiles16,
            this.TSMISmiles17,
            this.TSMISmiles18,
            this.TSMISmiles19});
            this.TSMIInputSmilesBlink.Image = global::stCoCClient.Properties.Resources.emoji_helpless;
            this.TSMIInputSmilesBlink.Name = "TSMIInputSmilesBlink";
            resources.ApplyResources(this.TSMIInputSmilesBlink, "TSMIInputSmilesBlink");
            // 
            // TSMISmiles11
            // 
            this.TSMISmiles11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles11.Image = global::stCoCClient.Properties.Resources.emoji_wink;
            this.TSMISmiles11.Name = "TSMISmiles11";
            resources.ApplyResources(this.TSMISmiles11, "TSMISmiles11");
            this.TSMISmiles11.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles12
            // 
            this.TSMISmiles12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles12.Image = global::stCoCClient.Properties.Resources.emoji_helpless;
            this.TSMISmiles12.Name = "TSMISmiles12";
            resources.ApplyResources(this.TSMISmiles12, "TSMISmiles12");
            this.TSMISmiles12.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles13
            // 
            this.TSMISmiles13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles13.Image = global::stCoCClient.Properties.Resources.emoji_unbelievables;
            this.TSMISmiles13.Name = "TSMISmiles13";
            resources.ApplyResources(this.TSMISmiles13, "TSMISmiles13");
            this.TSMISmiles13.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles14
            // 
            this.TSMISmiles14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles14.Image = global::stCoCClient.Properties.Resources.emoji_rage;
            this.TSMISmiles14.Name = "TSMISmiles14";
            resources.ApplyResources(this.TSMISmiles14, "TSMISmiles14");
            this.TSMISmiles14.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles15
            // 
            this.TSMISmiles15.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles15.Image = global::stCoCClient.Properties.Resources.emoji_consoling;
            this.TSMISmiles15.Name = "TSMISmiles15";
            resources.ApplyResources(this.TSMISmiles15, "TSMISmiles15");
            this.TSMISmiles15.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles16
            // 
            this.TSMISmiles16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles16.Image = global::stCoCClient.Properties.Resources.emoji_helpful;
            this.TSMISmiles16.Name = "TSMISmiles16";
            resources.ApplyResources(this.TSMISmiles16, "TSMISmiles16");
            this.TSMISmiles16.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles17
            // 
            this.TSMISmiles17.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles17.Image = global::stCoCClient.Properties.Resources.emoji_cute;
            this.TSMISmiles17.Name = "TSMISmiles17";
            resources.ApplyResources(this.TSMISmiles17, "TSMISmiles17");
            this.TSMISmiles17.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles18
            // 
            this.TSMISmiles18.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles18.Image = global::stCoCClient.Properties.Resources.emoji_sad;
            this.TSMISmiles18.Name = "TSMISmiles18";
            resources.ApplyResources(this.TSMISmiles18, "TSMISmiles18");
            this.TSMISmiles18.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // TSMISmiles19
            // 
            this.TSMISmiles19.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMISmiles19.Image = global::stCoCClient.Properties.Resources.emoji_whining;
            this.TSMISmiles19.Name = "TSMISmiles19";
            resources.ApplyResources(this.TSMISmiles19, "TSMISmiles19");
            this.TSMISmiles19.Click += new System.EventHandler(this.TSMISmiles_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            // 
            // TSMIInputStyle
            // 
            this.TSMIInputStyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIInputStyleBold,
            this.TSMIInputStyleItalic,
            this.TSMIInputStyleUnderline,
            this.TSMIInputStyleStrike});
            this.TSMIInputStyle.Image = global::stCoCClient.Properties.Resources.style;
            this.TSMIInputStyle.Name = "TSMIInputStyle";
            resources.ApplyResources(this.TSMIInputStyle, "TSMIInputStyle");
            // 
            // TSMIInputStyleBold
            // 
            this.TSMIInputStyleBold.ForeColor = System.Drawing.Color.White;
            this.TSMIInputStyleBold.Image = global::stCoCClient.Properties.Resources.bold;
            this.TSMIInputStyleBold.Name = "TSMIInputStyleBold";
            resources.ApplyResources(this.TSMIInputStyleBold, "TSMIInputStyleBold");
            this.TSMIInputStyleBold.Tag = "b";
            this.TSMIInputStyleBold.Click += new System.EventHandler(this.TSMIInputStyle_Click);
            // 
            // TSMIInputStyleItalic
            // 
            this.TSMIInputStyleItalic.ForeColor = System.Drawing.Color.White;
            this.TSMIInputStyleItalic.Image = global::stCoCClient.Properties.Resources.italic;
            this.TSMIInputStyleItalic.Name = "TSMIInputStyleItalic";
            resources.ApplyResources(this.TSMIInputStyleItalic, "TSMIInputStyleItalic");
            this.TSMIInputStyleItalic.Tag = "i";
            this.TSMIInputStyleItalic.Click += new System.EventHandler(this.TSMIInputStyle_Click);
            // 
            // TSMIInputStyleUnderline
            // 
            this.TSMIInputStyleUnderline.ForeColor = System.Drawing.Color.White;
            this.TSMIInputStyleUnderline.Image = global::stCoCClient.Properties.Resources.underline;
            this.TSMIInputStyleUnderline.Name = "TSMIInputStyleUnderline";
            resources.ApplyResources(this.TSMIInputStyleUnderline, "TSMIInputStyleUnderline");
            this.TSMIInputStyleUnderline.Tag = "u";
            this.TSMIInputStyleUnderline.Click += new System.EventHandler(this.TSMIInputStyle_Click);
            // 
            // TSMIInputStyleStrike
            // 
            this.TSMIInputStyleStrike.ForeColor = System.Drawing.Color.White;
            this.TSMIInputStyleStrike.Image = global::stCoCClient.Properties.Resources.strike;
            this.TSMIInputStyleStrike.Name = "TSMIInputStyleStrike";
            resources.ApplyResources(this.TSMIInputStyleStrike, "TSMIInputStyleStrike");
            this.TSMIInputStyleStrike.Tag = "s";
            this.TSMIInputStyleStrike.Click += new System.EventHandler(this.TSMIInputStyle_Click);
            // 
            // TSMIInputColor
            // 
            this.TSMIInputColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIInputColorRed,
            this.TSMIInputColorYellow,
            this.TSMIInputColorGreen,
            this.TSMIInputColorCyan,
            this.TSMIInputColorBlue,
            this.TSMIInputColorMagenta,
            this.TSMIInputColorCustom});
            this.TSMIInputColor.Image = global::stCoCClient.Properties.Resources.ic_invert_colors_white_18dp;
            this.TSMIInputColor.Name = "TSMIInputColor";
            resources.ApplyResources(this.TSMIInputColor, "TSMIInputColor");
            // 
            // TSMIInputColorRed
            // 
            this.TSMIInputColorRed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TSMIInputColorRed, "TSMIInputColorRed");
            this.TSMIInputColorRed.Name = "TSMIInputColorRed";
            this.TSMIInputColorRed.Tag = "red";
            this.TSMIInputColorRed.Click += new System.EventHandler(this.TSMIInputColor_Click);
            // 
            // TSMIInputColorYellow
            // 
            this.TSMIInputColorYellow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TSMIInputColorYellow, "TSMIInputColorYellow");
            this.TSMIInputColorYellow.Name = "TSMIInputColorYellow";
            this.TSMIInputColorYellow.Tag = "yellow";
            this.TSMIInputColorYellow.Click += new System.EventHandler(this.TSMIInputColor_Click);
            // 
            // TSMIInputColorGreen
            // 
            this.TSMIInputColorGreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TSMIInputColorGreen, "TSMIInputColorGreen");
            this.TSMIInputColorGreen.Name = "TSMIInputColorGreen";
            this.TSMIInputColorGreen.Tag = "green";
            this.TSMIInputColorGreen.Click += new System.EventHandler(this.TSMIInputColor_Click);
            // 
            // TSMIInputColorCyan
            // 
            this.TSMIInputColorCyan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TSMIInputColorCyan, "TSMIInputColorCyan");
            this.TSMIInputColorCyan.Name = "TSMIInputColorCyan";
            this.TSMIInputColorCyan.Tag = "cyan";
            this.TSMIInputColorCyan.Click += new System.EventHandler(this.TSMIInputColor_Click);
            // 
            // TSMIInputColorBlue
            // 
            this.TSMIInputColorBlue.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TSMIInputColorBlue, "TSMIInputColorBlue");
            this.TSMIInputColorBlue.Name = "TSMIInputColorBlue";
            this.TSMIInputColorBlue.Tag = "deepskyblue";
            this.TSMIInputColorBlue.Click += new System.EventHandler(this.TSMIInputColor_Click);
            // 
            // TSMIInputColorMagenta
            // 
            this.TSMIInputColorMagenta.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.TSMIInputColorMagenta, "TSMIInputColorMagenta");
            this.TSMIInputColorMagenta.Name = "TSMIInputColorMagenta";
            this.TSMIInputColorMagenta.Tag = "magenta";
            this.TSMIInputColorMagenta.Click += new System.EventHandler(this.TSMIInputColor_Click);
            // 
            // TSMIInputColorCustom
            // 
            this.TSMIInputColorCustom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSMIInputColorCustom.Image = global::stCoCClient.Properties.Resources.color_custom;
            this.TSMIInputColorCustom.Name = "TSMIInputColorCustom";
            resources.ApplyResources(this.TSMIInputColorCustom, "TSMIInputColorCustom");
            this.TSMIInputColorCustom.Click += new System.EventHandler(this.TSMIInputColorCustom_Click);
            // 
            // TSMIInputImage
            // 
            this.TSMIInputImage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSTBInputImage});
            this.TSMIInputImage.Image = global::stCoCClient.Properties.Resources.image;
            this.TSMIInputImage.Name = "TSMIInputImage";
            resources.ApplyResources(this.TSMIInputImage, "TSMIInputImage");
            // 
            // TSTBInputImage
            // 
            this.TSTBInputImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.TSTBInputImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TSTBInputImage.ForeColor = System.Drawing.Color.White;
            this.TSTBInputImage.Name = "TSTBInputImage";
            resources.ApplyResources(this.TSTBInputImage, "TSTBInputImage");
            this.TSTBInputImage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TSTBInputImage_KeyUp);
            // 
            // TSMIInputUrl
            // 
            this.TSMIInputUrl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSTBInputUrl});
            this.TSMIInputUrl.Image = global::stCoCClient.Properties.Resources.attach;
            this.TSMIInputUrl.Name = "TSMIInputUrl";
            resources.ApplyResources(this.TSMIInputUrl, "TSMIInputUrl");
            // 
            // TSTBInputUrl
            // 
            this.TSTBInputUrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.TSTBInputUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TSTBInputUrl.ForeColor = System.Drawing.Color.White;
            this.TSTBInputUrl.Name = "TSTBInputUrl";
            resources.ApplyResources(this.TSTBInputUrl, "TSTBInputUrl");
            this.TSTBInputUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TSTBInputUrl_KeyUp);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // TSMIInputSend
            // 
            this.TSMIInputSend.Image = global::stCoCClient.Properties.Resources.ic_compare_arrows_white_18dp;
            this.TSMIInputSend.Name = "TSMIInputSend";
            resources.ApplyResources(this.TSMIInputSend, "TSMIInputSend");
            this.TSMIInputSend.Click += new System.EventHandler(this.TSMIInputSend_Click);
            // 
            // TSMIInputSendPriv
            // 
            this.TSMIInputSendPriv.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSTBFindNick});
            this.TSMIInputSendPriv.Image = global::stCoCClient.Properties.Resources.ic_lock_outline_white_18dp;
            this.TSMIInputSendPriv.Name = "TSMIInputSendPriv";
            resources.ApplyResources(this.TSMIInputSendPriv, "TSMIInputSendPriv");
            // 
            // TSTBFindNick
            // 
            this.TSTBFindNick.AcceptsReturn = true;
            this.TSTBFindNick.AcceptsTab = true;
            this.TSTBFindNick.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TSTBFindNick.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.TSTBFindNick.AutoToolTip = true;
            this.TSTBFindNick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.TSTBFindNick.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TSTBFindNick.ForeColor = System.Drawing.Color.White;
            this.TSTBFindNick.Name = "TSTBFindNick";
            resources.ApplyResources(this.TSTBFindNick, "TSTBFindNick");
            // 
            // flatTabControlMain
            // 
            this.flatTabControlMain.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatTabControlMain.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatTabControlMain.Controls.Add(this.tabPageSetupMain);
            this.flatTabControlMain.Controls.Add(this.tabPageSetupUser);
            this.flatTabControlMain.Controls.Add(this.tabPageSetupReg);
            this.flatTabControlMain.Controls.Add(this.tabPageClanNews);
            this.flatTabControlMain.Controls.Add(this.tabPageClanStat);
            this.flatTabControlMain.Controls.Add(this.tabPageClanChat);
            resources.ApplyResources(this.flatTabControlMain, "flatTabControlMain");
            this.flatTabControlMain.Name = "flatTabControlMain";
            this.flatTabControlMain.SelectedIndex = 0;
            this.flatTabControlMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.flatTabControlMain.Selected += new System.Windows.Forms.TabControlEventHandler(this.flatTabControlMain_Selected);
            // 
            // tabPageSetupMain
            // 
            this.tabPageSetupMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.tabPageSetupMain.Controls.Add(this.bboxReload);
            this.tabPageSetupMain.Controls.Add(this.bboxSave);
            this.tabPageSetupMain.Controls.Add(this.flatGroupBox3);
            this.tabPageSetupMain.Controls.Add(this.flatGroupBox2);
            this.tabPageSetupMain.Controls.Add(this.flatGroupBox1);
            this.tabPageSetupMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            resources.ApplyResources(this.tabPageSetupMain, "tabPageSetupMain");
            this.tabPageSetupMain.Name = "tabPageSetupMain";
            // 
            // bboxReload
            // 
            this.bboxReload.BackColor = System.Drawing.Color.Transparent;
            this.bboxReload.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.bboxReload.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.bboxReload, "bboxReload");
            this.bboxReload.Name = "bboxReload";
            this.bboxReload.Rounded = false;
            this.bboxReload.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.bboxReload.Click += new System.EventHandler(this.bboxReload_Click);
            // 
            // bboxSave
            // 
            this.bboxSave.BackColor = System.Drawing.Color.Transparent;
            this.bboxSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.bboxSave.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.bboxSave, "bboxSave");
            this.bboxSave.Name = "bboxSave";
            this.bboxSave.Rounded = false;
            this.bboxSave.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.bboxSave.Click += new System.EventHandler(this.bboxSave_Click);
            // 
            // flatGroupBox3
            // 
            this.flatGroupBox3.BackColor = System.Drawing.Color.Transparent;
            this.flatGroupBox3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(59)))), ((int)(((byte)(61)))));
            this.flatGroupBox3.Controls.Add(this.cboxWarClanEnd);
            this.flatGroupBox3.Controls.Add(this.cboxClanChangeMembers);
            this.flatGroupBox3.Controls.Add(this.cboxClanChangePoints);
            this.flatGroupBox3.Controls.Add(this.cboxMemberChangeDonationReceive);
            this.flatGroupBox3.Controls.Add(this.cboxMemberChangeDonationSend);
            this.flatGroupBox3.Controls.Add(this.cboxMemberChangeTrophies);
            this.flatGroupBox3.Controls.Add(this.cboxMemberChangeLeague);
            this.flatGroupBox3.Controls.Add(this.cboxMemberChangeLevel);
            this.flatGroupBox3.Controls.Add(this.cboxMemberChangeRole);
            this.flatGroupBox3.Controls.Add(this.flatCheckBox4);
            this.flatGroupBox3.Controls.Add(this.cboxMemberChangeNik);
            this.flatGroupBox3.Controls.Add(this.cboxMemberExit);
            this.flatGroupBox3.Controls.Add(this.cboxMemberNew);
            resources.ApplyResources(this.flatGroupBox3, "flatGroupBox3");
            this.flatGroupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.flatGroupBox3.Name = "flatGroupBox3";
            this.flatGroupBox3.ShowText = true;
            // 
            // cboxWarClanEnd
            // 
            this.cboxWarClanEnd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxWarClanEnd.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxWarClanEnd.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxWarClanEnd.Checked = global::stCoCClient.Properties.Settings.Default.WarClanEnd;
            this.cboxWarClanEnd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxWarClanEnd.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "WarClanEnd", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxWarClanEnd, "cboxWarClanEnd");
            this.cboxWarClanEnd.Name = "cboxWarClanEnd";
            this.cboxWarClanEnd.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxClanChangeMembers
            // 
            this.cboxClanChangeMembers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxClanChangeMembers.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxClanChangeMembers.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxClanChangeMembers.Checked = global::stCoCClient.Properties.Settings.Default.ClanChangeMembers;
            this.cboxClanChangeMembers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxClanChangeMembers.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "ClanChangeMembers", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxClanChangeMembers, "cboxClanChangeMembers");
            this.cboxClanChangeMembers.Name = "cboxClanChangeMembers";
            this.cboxClanChangeMembers.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxClanChangePoints
            // 
            this.cboxClanChangePoints.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxClanChangePoints.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxClanChangePoints.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxClanChangePoints.Checked = global::stCoCClient.Properties.Settings.Default.ClanChangePoints;
            this.cboxClanChangePoints.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxClanChangePoints.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "ClanChangePoints", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxClanChangePoints, "cboxClanChangePoints");
            this.cboxClanChangePoints.Name = "cboxClanChangePoints";
            this.cboxClanChangePoints.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberChangeDonationReceive
            // 
            this.cboxMemberChangeDonationReceive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberChangeDonationReceive.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberChangeDonationReceive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberChangeDonationReceive.Checked = global::stCoCClient.Properties.Settings.Default.MemberChangeDonationReceive;
            this.cboxMemberChangeDonationReceive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberChangeDonationReceive.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberChangeDonationReceive", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberChangeDonationReceive, "cboxMemberChangeDonationReceive");
            this.cboxMemberChangeDonationReceive.Name = "cboxMemberChangeDonationReceive";
            this.cboxMemberChangeDonationReceive.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberChangeDonationSend
            // 
            this.cboxMemberChangeDonationSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberChangeDonationSend.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberChangeDonationSend.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberChangeDonationSend.Checked = global::stCoCClient.Properties.Settings.Default.MemberChangeDonationSend;
            this.cboxMemberChangeDonationSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberChangeDonationSend.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberChangeDonationSend", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberChangeDonationSend, "cboxMemberChangeDonationSend");
            this.cboxMemberChangeDonationSend.Name = "cboxMemberChangeDonationSend";
            this.cboxMemberChangeDonationSend.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberChangeTrophies
            // 
            this.cboxMemberChangeTrophies.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberChangeTrophies.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberChangeTrophies.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberChangeTrophies.Checked = global::stCoCClient.Properties.Settings.Default.MemberChangeTrophies;
            this.cboxMemberChangeTrophies.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberChangeTrophies.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberChangeTrophies", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberChangeTrophies, "cboxMemberChangeTrophies");
            this.cboxMemberChangeTrophies.Name = "cboxMemberChangeTrophies";
            this.cboxMemberChangeTrophies.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberChangeLeague
            // 
            this.cboxMemberChangeLeague.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberChangeLeague.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberChangeLeague.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberChangeLeague.Checked = global::stCoCClient.Properties.Settings.Default.MemberChangeLeague;
            this.cboxMemberChangeLeague.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberChangeLeague.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberChangeLeague", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberChangeLeague, "cboxMemberChangeLeague");
            this.cboxMemberChangeLeague.Name = "cboxMemberChangeLeague";
            this.cboxMemberChangeLeague.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberChangeLevel
            // 
            this.cboxMemberChangeLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberChangeLevel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberChangeLevel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberChangeLevel.Checked = global::stCoCClient.Properties.Settings.Default.MemberChangeLevel;
            this.cboxMemberChangeLevel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberChangeLevel.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberChangeLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberChangeLevel, "cboxMemberChangeLevel");
            this.cboxMemberChangeLevel.Name = "cboxMemberChangeLevel";
            this.cboxMemberChangeLevel.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberChangeRole
            // 
            this.cboxMemberChangeRole.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberChangeRole.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberChangeRole.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberChangeRole.Checked = global::stCoCClient.Properties.Settings.Default.MemberChangeRole;
            this.cboxMemberChangeRole.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberChangeRole.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberChangeRole", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberChangeRole, "cboxMemberChangeRole");
            this.cboxMemberChangeRole.Name = "cboxMemberChangeRole";
            this.cboxMemberChangeRole.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // flatCheckBox4
            // 
            this.flatCheckBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.flatCheckBox4.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatCheckBox4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatCheckBox4.Checked = false;
            this.flatCheckBox4.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.flatCheckBox4, "flatCheckBox4");
            this.flatCheckBox4.Name = "flatCheckBox4";
            this.flatCheckBox4.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberChangeNik
            // 
            this.cboxMemberChangeNik.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberChangeNik.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberChangeNik.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberChangeNik.Checked = global::stCoCClient.Properties.Settings.Default.MemberChangeNik;
            this.cboxMemberChangeNik.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberChangeNik.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberChangeNik", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberChangeNik, "cboxMemberChangeNik");
            this.cboxMemberChangeNik.Name = "cboxMemberChangeNik";
            this.cboxMemberChangeNik.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberExit
            // 
            this.cboxMemberExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberExit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberExit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberExit.Checked = global::stCoCClient.Properties.Settings.Default.MemberExit;
            this.cboxMemberExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberExit.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberExit", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberExit, "cboxMemberExit");
            this.cboxMemberExit.Name = "cboxMemberExit";
            this.cboxMemberExit.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // cboxMemberNew
            // 
            this.cboxMemberNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.cboxMemberNew.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.cboxMemberNew.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.cboxMemberNew.Checked = global::stCoCClient.Properties.Settings.Default.MemberNew;
            this.cboxMemberNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboxMemberNew.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "MemberNew", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.cboxMemberNew, "cboxMemberNew");
            this.cboxMemberNew.Name = "cboxMemberNew";
            this.cboxMemberNew.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // flatGroupBox2
            // 
            this.flatGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.flatGroupBox2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(59)))), ((int)(((byte)(61)))));
            this.flatGroupBox2.Controls.Add(this.FTBExportPath);
            this.flatGroupBox2.Controls.Add(this.pictureBox15);
            this.flatGroupBox2.Controls.Add(this.FSBExportPath);
            this.flatGroupBox2.Controls.Add(this.flatLabel2);
            this.flatGroupBox2.Controls.Add(this.FSBRegistred);
            this.flatGroupBox2.Controls.Add(this.FBWizard);
            this.flatGroupBox2.Controls.Add(this.flatNumericInformerId1);
            this.flatGroupBox2.Controls.Add(this.flatLabel12);
            this.flatGroupBox2.Controls.Add(this.pictureBox8);
            this.flatGroupBox2.Controls.Add(this.pictureBox7);
            this.flatGroupBox2.Controls.Add(this.FTBGameTag2);
            this.flatGroupBox2.Controls.Add(this.pictureBox4);
            this.flatGroupBox2.Controls.Add(this.FTBIrcPass2);
            this.flatGroupBox2.Controls.Add(this.pictureBox5);
            this.flatGroupBox2.Controls.Add(this.FTBIrcNick2);
            this.flatGroupBox2.Controls.Add(this.FTBCoCServer2ro);
            this.flatGroupBox2.Controls.Add(this.pictureBox6);
            resources.ApplyResources(this.flatGroupBox2, "flatGroupBox2");
            this.flatGroupBox2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.flatGroupBox2.Name = "flatGroupBox2";
            this.flatGroupBox2.ShowText = true;
            // 
            // FTBExportPath
            // 
            this.FTBExportPath.BackColor = System.Drawing.Color.DimGray;
            this.FTBExportPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "ExportPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.FTBExportPath, "FTBExportPath");
            this.FTBExportPath.FocusOnHover = true;
            this.FTBExportPath.MaxLength = 32767;
            this.FTBExportPath.Multiline = false;
            this.FTBExportPath.Name = "FTBExportPath";
            this.FTBExportPath.ReadOnly = true;
            this.FTBExportPath.SelectedText = "";
            this.FTBExportPath.SelectionLength = 0;
            this.FTBExportPath.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBExportPath.SpellMark")));
            this.FTBExportPath.Text = global::stCoCClient.Properties.Settings.Default.ExportPath;
            this.FTBExportPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBExportPath.TextCaption = "CoC Server: http(s):// IP/DNS";
            this.FTBExportPath.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBExportPath.UseSystemPasswordChar = false;
            // 
            // pictureBox15
            // 
            this.pictureBox15.Image = global::stCoCClient.Properties.Resources.ic_create_new_folder_white_18dp;
            resources.ApplyResources(this.pictureBox15, "pictureBox15");
            this.pictureBox15.Name = "pictureBox15";
            this.pictureBox15.TabStop = false;
            // 
            // FSBExportPath
            // 
            this.FSBExportPath.BackColor = System.Drawing.Color.Transparent;
            this.FSBExportPath.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.FSBExportPath.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.FSBExportPath, "FSBExportPath");
            this.FSBExportPath.Name = "FSBExportPath";
            this.FSBExportPath.Rounded = false;
            this.FSBExportPath.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.FSBExportPath.Click += new System.EventHandler(this.FSBExportPath_Click);
            // 
            // flatLabel2
            // 
            resources.ApplyResources(this.flatLabel2, "flatLabel2");
            this.flatLabel2.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel2.ForeColor = System.Drawing.Color.Silver;
            this.flatLabel2.Name = "flatLabel2";
            // 
            // FSBRegistred
            // 
            this.FSBRegistred.BackColor = System.Drawing.Color.Transparent;
            this.FSBRegistred.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.FSBRegistred.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.FSBRegistred, "FSBRegistred");
            this.FSBRegistred.Name = "FSBRegistred";
            this.FSBRegistred.Rounded = false;
            this.FSBRegistred.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // FBWizard
            // 
            this.FBWizard.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.FBWizard, "FBWizard");
            this.FBWizard.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.FBWizard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FBWizard.Name = "FBWizard";
            this.FBWizard.Rounded = false;
            this.FBWizard.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.FBWizard.Click += new System.EventHandler(this.CheckSetup_Click);
            // 
            // flatNumericInformerId1
            // 
            this.flatNumericInformerId1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.flatNumericInformerId1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatNumericInformerId1.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatNumericInformerId1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::stCoCClient.Properties.Settings.Default, "USRInformerId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.flatNumericInformerId1, "flatNumericInformerId1");
            this.flatNumericInformerId1.ForeColor = System.Drawing.Color.DarkGray;
            this.flatNumericInformerId1.Maximum = ((long)(15));
            this.flatNumericInformerId1.Minimum = ((long)(0));
            this.flatNumericInformerId1.Name = "flatNumericInformerId1";
            this.flatNumericInformerId1.Value = global::stCoCClient.Properties.Settings.Default.USRInformerId;
            // 
            // flatLabel12
            // 
            resources.ApplyResources(this.flatLabel12, "flatLabel12");
            this.flatLabel12.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel12.ForeColor = System.Drawing.Color.Silver;
            this.flatLabel12.Name = "flatLabel12";
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pictureBox8, "pictureBox8");
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox7
            // 
            resources.ApplyResources(this.pictureBox7, "pictureBox7");
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.TabStop = false;
            // 
            // FTBGameTag2
            // 
            this.FTBGameTag2.BackColor = System.Drawing.Color.DimGray;
            this.FTBGameTag2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "USRCoCTag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.FTBGameTag2, "FTBGameTag2");
            this.FTBGameTag2.FocusOnHover = true;
            this.FTBGameTag2.MaxLength = 32767;
            this.FTBGameTag2.Multiline = false;
            this.FTBGameTag2.Name = "FTBGameTag2";
            this.FTBGameTag2.ReadOnly = true;
            this.FTBGameTag2.SelectedText = "";
            this.FTBGameTag2.SelectionLength = 0;
            this.FTBGameTag2.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBGameTag2.SpellMark")));
            this.FTBGameTag2.Text = global::stCoCClient.Properties.Settings.Default.USRCoCTag;
            this.FTBGameTag2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBGameTag2.TextCaption = "CoC game user tag, no # add to this";
            this.FTBGameTag2.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBGameTag2.UseSystemPasswordChar = false;
            this.FTBGameTag2.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // pictureBox4
            // 
            resources.ApplyResources(this.pictureBox4, "pictureBox4");
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.TabStop = false;
            // 
            // FTBIrcPass2
            // 
            this.FTBIrcPass2.BackColor = System.Drawing.Color.DimGray;
            this.FTBIrcPass2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "IRCPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.FTBIrcPass2, "FTBIrcPass2");
            this.FTBIrcPass2.FocusOnHover = true;
            this.FTBIrcPass2.MaxLength = 32767;
            this.FTBIrcPass2.Multiline = false;
            this.FTBIrcPass2.Name = "FTBIrcPass2";
            this.FTBIrcPass2.ReadOnly = true;
            this.FTBIrcPass2.SelectedText = "";
            this.FTBIrcPass2.SelectionLength = 0;
            this.FTBIrcPass2.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBIrcPass2.SpellMark")));
            this.FTBIrcPass2.Text = global::stCoCClient.Properties.Settings.Default.IRCPassword;
            this.FTBIrcPass2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBIrcPass2.TextCaption = "IRC user password";
            this.FTBIrcPass2.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBIrcPass2.UseSystemPasswordChar = true;
            this.FTBIrcPass2.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // pictureBox5
            // 
            resources.ApplyResources(this.pictureBox5, "pictureBox5");
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.TabStop = false;
            // 
            // FTBIrcNick2
            // 
            this.FTBIrcNick2.BackColor = System.Drawing.Color.DimGray;
            this.FTBIrcNick2.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "IRCNik", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.FTBIrcNick2, "FTBIrcNick2");
            this.FTBIrcNick2.FocusOnHover = true;
            this.FTBIrcNick2.MaxLength = 32767;
            this.FTBIrcNick2.Multiline = false;
            this.FTBIrcNick2.Name = "FTBIrcNick2";
            this.FTBIrcNick2.ReadOnly = true;
            this.FTBIrcNick2.SelectedText = "";
            this.FTBIrcNick2.SelectionLength = 0;
            this.FTBIrcNick2.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBIrcNick2.SpellMark")));
            this.FTBIrcNick2.Text = global::stCoCClient.Properties.Settings.Default.IRCNik;
            this.FTBIrcNick2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBIrcNick2.TextCaption = "IRC user nik name";
            this.FTBIrcNick2.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBIrcNick2.UseSystemPasswordChar = false;
            this.FTBIrcNick2.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // FTBCoCServer2ro
            // 
            this.FTBCoCServer2ro.BackColor = System.Drawing.Color.DimGray;
            this.FTBCoCServer2ro.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "USRCoCServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.FTBCoCServer2ro, "FTBCoCServer2ro");
            this.FTBCoCServer2ro.FocusOnHover = true;
            this.FTBCoCServer2ro.MaxLength = 32767;
            this.FTBCoCServer2ro.Multiline = false;
            this.FTBCoCServer2ro.Name = "FTBCoCServer2ro";
            this.FTBCoCServer2ro.ReadOnly = true;
            this.FTBCoCServer2ro.SelectedText = "";
            this.FTBCoCServer2ro.SelectionLength = 0;
            this.FTBCoCServer2ro.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBCoCServer2ro.SpellMark")));
            this.FTBCoCServer2ro.Text = global::stCoCClient.Properties.Settings.Default.USRCoCServer;
            this.FTBCoCServer2ro.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBCoCServer2ro.TextCaption = "CoC Server: http(s):// IP/DNS";
            this.FTBCoCServer2ro.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBCoCServer2ro.UseSystemPasswordChar = false;
            this.FTBCoCServer2ro.TextChanged += new System.EventHandler(this.FTBCoCServer_TextChanged);
            this.FTBCoCServer2ro.Enter += new System.EventHandler(this.tboxCaption_Enter);
            this.FTBCoCServer2ro.Leave += new System.EventHandler(this.FTBCoCServer_Leave);
            // 
            // pictureBox6
            // 
            resources.ApplyResources(this.pictureBox6, "pictureBox6");
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.TabStop = false;
            // 
            // flatGroupBox1
            // 
            this.flatGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.flatGroupBox1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(59)))), ((int)(((byte)(61)))));
            this.flatGroupBox1.Controls.Add(this.pictureBox14);
            this.flatGroupBox1.Controls.Add(this.FTBWikiHome);
            this.flatGroupBox1.Controls.Add(this.flatLabel10);
            this.flatGroupBox1.Controls.Add(this.flatToggle4);
            this.flatGroupBox1.Controls.Add(this.flatLabel9);
            this.flatGroupBox1.Controls.Add(this.flatToggle3);
            this.flatGroupBox1.Controls.Add(this.flatLabel8);
            this.flatGroupBox1.Controls.Add(this.flatToggle2);
            this.flatGroupBox1.Controls.Add(this.flatLabel7);
            this.flatGroupBox1.Controls.Add(this.flatToggle1);
            this.flatGroupBox1.Controls.Add(this.pictureBox3);
            this.flatGroupBox1.Controls.Add(this.FTBIRCChannel);
            this.flatGroupBox1.Controls.Add(this.pictureBox2);
            this.flatGroupBox1.Controls.Add(this.FTBIRCPort);
            this.flatGroupBox1.Controls.Add(this.pictureBox1);
            this.flatGroupBox1.Controls.Add(this.FTBIRCServer);
            resources.ApplyResources(this.flatGroupBox1, "flatGroupBox1");
            this.flatGroupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.flatGroupBox1.Name = "flatGroupBox1";
            this.flatGroupBox1.ShowText = true;
            // 
            // pictureBox14
            // 
            resources.ApplyResources(this.pictureBox14, "pictureBox14");
            this.pictureBox14.Name = "pictureBox14";
            this.pictureBox14.TabStop = false;
            // 
            // FTBWikiHome
            // 
            this.FTBWikiHome.BackColor = System.Drawing.Color.DimGray;
            this.FTBWikiHome.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "USRWikiHome", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FTBWikiHome.FocusOnHover = true;
            resources.ApplyResources(this.FTBWikiHome, "FTBWikiHome");
            this.FTBWikiHome.MaxLength = 32767;
            this.FTBWikiHome.Multiline = false;
            this.FTBWikiHome.Name = "FTBWikiHome";
            this.FTBWikiHome.ReadOnly = false;
            this.FTBWikiHome.SelectedText = "";
            this.FTBWikiHome.SelectionLength = 0;
            this.FTBWikiHome.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBWikiHome.SpellMark")));
            this.FTBWikiHome.Text = global::stCoCClient.Properties.Settings.Default.USRWikiHome;
            this.FTBWikiHome.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBWikiHome.TextCaption = "Clan Wiki home page";
            this.FTBWikiHome.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBWikiHome.UseSystemPasswordChar = false;
            this.FTBWikiHome.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // flatLabel10
            // 
            resources.ApplyResources(this.flatLabel10, "flatLabel10");
            this.flatLabel10.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel10.ForeColor = System.Drawing.Color.Silver;
            this.flatLabel10.Name = "flatLabel10";
            // 
            // flatToggle4
            // 
            this.flatToggle4.BackColor = System.Drawing.Color.Transparent;
            this.flatToggle4.Checked = global::stCoCClient.Properties.Settings.Default.IRCKickRespawn;
            this.flatToggle4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatToggle4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "IRCKickRespawn", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.flatToggle4, "flatToggle4");
            this.flatToggle4.Name = "flatToggle4";
            this.flatToggle4.Options = stCoreUI.FlatToggle._Options.Style3;
            // 
            // flatLabel9
            // 
            resources.ApplyResources(this.flatLabel9, "flatLabel9");
            this.flatLabel9.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel9.ForeColor = System.Drawing.Color.Silver;
            this.flatLabel9.Name = "flatLabel9";
            // 
            // flatToggle3
            // 
            this.flatToggle3.BackColor = System.Drawing.Color.Transparent;
            this.flatToggle3.Checked = global::stCoCClient.Properties.Settings.Default.IRCPrivateMessage;
            this.flatToggle3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatToggle3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "IRCPrivateMessage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.flatToggle3, "flatToggle3");
            this.flatToggle3.Name = "flatToggle3";
            this.flatToggle3.Options = stCoreUI.FlatToggle._Options.Style3;
            // 
            // flatLabel8
            // 
            resources.ApplyResources(this.flatLabel8, "flatLabel8");
            this.flatLabel8.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel8.ForeColor = System.Drawing.Color.Silver;
            this.flatLabel8.Name = "flatLabel8";
            // 
            // flatToggle2
            // 
            this.flatToggle2.BackColor = System.Drawing.Color.Transparent;
            this.flatToggle2.Checked = global::stCoCClient.Properties.Settings.Default.IRCNoticeMessage;
            this.flatToggle2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatToggle2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "IRCNoticeMessage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.flatToggle2, "flatToggle2");
            this.flatToggle2.Name = "flatToggle2";
            this.flatToggle2.Options = stCoreUI.FlatToggle._Options.Style3;
            // 
            // flatLabel7
            // 
            resources.ApplyResources(this.flatLabel7, "flatLabel7");
            this.flatLabel7.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel7.ForeColor = System.Drawing.Color.Silver;
            this.flatLabel7.Name = "flatLabel7";
            // 
            // flatToggle1
            // 
            this.flatToggle1.BackColor = System.Drawing.Color.Transparent;
            this.flatToggle1.Checked = global::stCoCClient.Properties.Settings.Default.IRCServerMessage;
            this.flatToggle1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flatToggle1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::stCoCClient.Properties.Settings.Default, "IRCServerMessage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.flatToggle1, "flatToggle1");
            this.flatToggle1.Name = "flatToggle1";
            this.flatToggle1.Options = stCoreUI.FlatToggle._Options.Style3;
            // 
            // pictureBox3
            // 
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // FTBIRCChannel
            // 
            this.FTBIRCChannel.BackColor = System.Drawing.Color.DimGray;
            this.FTBIRCChannel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "IRCChannel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FTBIRCChannel.FocusOnHover = true;
            resources.ApplyResources(this.FTBIRCChannel, "FTBIRCChannel");
            this.FTBIRCChannel.MaxLength = 32767;
            this.FTBIRCChannel.Multiline = false;
            this.FTBIRCChannel.Name = "FTBIRCChannel";
            this.FTBIRCChannel.ReadOnly = false;
            this.FTBIRCChannel.SelectedText = "";
            this.FTBIRCChannel.SelectionLength = 0;
            this.FTBIRCChannel.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBIRCChannel.SpellMark")));
            this.FTBIRCChannel.Text = global::stCoCClient.Properties.Settings.Default.IRCChannel;
            this.FTBIRCChannel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBIRCChannel.TextCaption = "IRC channel";
            this.FTBIRCChannel.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBIRCChannel.UseSystemPasswordChar = false;
            this.FTBIRCChannel.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::stCoCClient.Properties.Resources.ic_cast_connected_white_18dp;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // FTBIRCPort
            // 
            this.FTBIRCPort.BackColor = System.Drawing.Color.DimGray;
            this.FTBIRCPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "IRCPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FTBIRCPort.FocusOnHover = true;
            resources.ApplyResources(this.FTBIRCPort, "FTBIRCPort");
            this.FTBIRCPort.MaxLength = 32767;
            this.FTBIRCPort.Multiline = false;
            this.FTBIRCPort.Name = "FTBIRCPort";
            this.FTBIRCPort.ReadOnly = false;
            this.FTBIRCPort.SelectedText = "";
            this.FTBIRCPort.SelectionLength = 0;
            this.FTBIRCPort.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBIRCPort.SpellMark")));
            this.FTBIRCPort.Text = global::stCoCClient.Properties.Settings.Default.IRCPort;
            this.FTBIRCPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBIRCPort.TextCaption = "IRC server port";
            this.FTBIRCPort.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBIRCPort.UseSystemPasswordChar = false;
            this.FTBIRCPort.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::stCoCClient.Properties.Resources.ic_cast_white_18dp;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // FTBIRCServer
            // 
            this.FTBIRCServer.BackColor = System.Drawing.Color.DimGray;
            this.FTBIRCServer.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "IRCServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FTBIRCServer.FocusOnHover = true;
            resources.ApplyResources(this.FTBIRCServer, "FTBIRCServer");
            this.FTBIRCServer.MaxLength = 32767;
            this.FTBIRCServer.Multiline = false;
            this.FTBIRCServer.Name = "FTBIRCServer";
            this.FTBIRCServer.ReadOnly = false;
            this.FTBIRCServer.SelectedText = "";
            this.FTBIRCServer.SelectionLength = 0;
            this.FTBIRCServer.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBIRCServer.SpellMark")));
            this.FTBIRCServer.Text = global::stCoCClient.Properties.Settings.Default.IRCServer;
            this.FTBIRCServer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBIRCServer.TextCaption = "IRC server IP/DNS";
            this.FTBIRCServer.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBIRCServer.UseSystemPasswordChar = false;
            this.FTBIRCServer.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // tabPageSetupUser
            // 
            this.tabPageSetupUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.tabPageSetupUser.Controls.Add(this.flatGroupBox5);
            this.tabPageSetupUser.Controls.Add(this.flatGroupBox4);
            resources.ApplyResources(this.tabPageSetupUser, "tabPageSetupUser");
            this.tabPageSetupUser.Name = "tabPageSetupUser";
            // 
            // flatGroupBox5
            // 
            this.flatGroupBox5.BackColor = System.Drawing.Color.Transparent;
            this.flatGroupBox5.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(59)))), ((int)(((byte)(61)))));
            this.flatGroupBox5.Controls.Add(this.FLVSetup);
            resources.ApplyResources(this.flatGroupBox5, "flatGroupBox5");
            this.flatGroupBox5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.flatGroupBox5.Name = "flatGroupBox5";
            this.flatGroupBox5.ShowText = true;
            // 
            // FLVSetup
            // 
            this.FLVSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(59)))), ((int)(((byte)(61)))));
            this.FLVSetup.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.FLVSetup, "FLVSetup");
            this.FLVSetup.ForeColor = System.Drawing.Color.Silver;
            this.FLVSetup.FullRowSelect = true;
            this.FLVSetup.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.FLVSetup.MultiSelect = false;
            this.FLVSetup.Name = "FLVSetup";
            this.FLVSetup.Scrollable = false;
            this.FLVSetup.UseCompatibleStateImageBehavior = false;
            this.FLVSetup.View = System.Windows.Forms.View.Details;
            // 
            // flatGroupBox4
            // 
            this.flatGroupBox4.BackColor = System.Drawing.Color.Transparent;
            this.flatGroupBox4.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(59)))), ((int)(((byte)(61)))));
            this.flatGroupBox4.Controls.Add(this.FCBNikAuto);
            this.flatGroupBox4.Controls.Add(this.flatButton4);
            this.flatGroupBox4.Controls.Add(this.flatButton2);
            this.flatGroupBox4.Controls.Add(this.flatButton3);
            this.flatGroupBox4.Controls.Add(this.pictureBox13);
            this.flatGroupBox4.Controls.Add(this.FTBCoCServer1);
            this.flatGroupBox4.Controls.Add(this.flatNumericInformerId2);
            this.flatGroupBox4.Controls.Add(this.flatLabel1);
            this.flatGroupBox4.Controls.Add(this.pictureBox9);
            this.flatGroupBox4.Controls.Add(this.pictureBox10);
            this.flatGroupBox4.Controls.Add(this.FTBGameTag1);
            this.flatGroupBox4.Controls.Add(this.pictureBox11);
            this.flatGroupBox4.Controls.Add(this.FTBIrcPass1);
            this.flatGroupBox4.Controls.Add(this.pictureBox12);
            this.flatGroupBox4.Controls.Add(this.FTBIrcNick1);
            resources.ApplyResources(this.flatGroupBox4, "flatGroupBox4");
            this.flatGroupBox4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.flatGroupBox4.Name = "flatGroupBox4";
            this.flatGroupBox4.ShowText = true;
            // 
            // FCBNikAuto
            // 
            this.FCBNikAuto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.FCBNikAuto.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.FCBNikAuto.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.FCBNikAuto.Checked = true;
            this.FCBNikAuto.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.FCBNikAuto, "FCBNikAuto");
            this.FCBNikAuto.Name = "FCBNikAuto";
            this.FCBNikAuto.Options = stCoreUI.FlatCheckBox._Options.Style1;
            // 
            // flatButton4
            // 
            this.flatButton4.BackColor = System.Drawing.Color.Transparent;
            this.flatButton4.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.flatButton4, "flatButton4");
            this.flatButton4.Name = "flatButton4";
            this.flatButton4.Rounded = false;
            this.flatButton4.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.flatButton4.Click += new System.EventHandler(this.CheckCoCServer_Click);
            // 
            // flatButton2
            // 
            this.flatButton2.BackColor = System.Drawing.Color.Transparent;
            this.flatButton2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.flatButton2, "flatButton2");
            this.flatButton2.Name = "flatButton2";
            this.flatButton2.Rounded = false;
            this.flatButton2.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.flatButton2.Click += new System.EventHandler(this.CancelAndExit_Click);
            // 
            // flatButton3
            // 
            this.flatButton3.BackColor = System.Drawing.Color.Transparent;
            this.flatButton3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.flatButton3, "flatButton3");
            this.flatButton3.Name = "flatButton3";
            this.flatButton3.Rounded = false;
            this.flatButton3.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.flatButton3.Click += new System.EventHandler(this.CheckAndSave_Click);
            // 
            // pictureBox13
            // 
            resources.ApplyResources(this.pictureBox13, "pictureBox13");
            this.pictureBox13.Name = "pictureBox13";
            this.pictureBox13.TabStop = false;
            // 
            // flatNumericInformerId2
            // 
            this.flatNumericInformerId2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.flatNumericInformerId2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatNumericInformerId2.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.flatNumericInformerId2.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::stCoCClient.Properties.Settings.Default, "USRInformerId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.flatNumericInformerId2, "flatNumericInformerId2");
            this.flatNumericInformerId2.ForeColor = System.Drawing.Color.White;
            this.flatNumericInformerId2.Maximum = ((long)(15));
            this.flatNumericInformerId2.Minimum = ((long)(0));
            this.flatNumericInformerId2.Name = "flatNumericInformerId2";
            this.flatNumericInformerId2.Value = global::stCoCClient.Properties.Settings.Default.USRInformerId;
            this.flatNumericInformerId2.ValueChanged += new stCoreUI.ValueChangedEventHandler(this.flatNumericInformerId2_ValueChanged);
            this.flatNumericInformerId2.Leave += new System.EventHandler(this.flatNumericInformerId2_Leave);
            // 
            // flatLabel1
            // 
            resources.ApplyResources(this.flatLabel1, "flatLabel1");
            this.flatLabel1.BackColor = System.Drawing.Color.Transparent;
            this.flatLabel1.ForeColor = System.Drawing.Color.Silver;
            this.flatLabel1.Name = "flatLabel1";
            // 
            // pictureBox9
            // 
            resources.ApplyResources(this.pictureBox9, "pictureBox9");
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.TabStop = false;
            // 
            // pictureBox10
            // 
            resources.ApplyResources(this.pictureBox10, "pictureBox10");
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.TabStop = false;
            // 
            // pictureBox11
            // 
            resources.ApplyResources(this.pictureBox11, "pictureBox11");
            this.pictureBox11.Name = "pictureBox11";
            this.pictureBox11.TabStop = false;
            // 
            // FTBIrcPass1
            // 
            this.FTBIrcPass1.BackColor = System.Drawing.Color.DimGray;
            this.FTBIrcPass1.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::stCoCClient.Properties.Settings.Default, "IRCPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FTBIrcPass1.FocusOnHover = true;
            resources.ApplyResources(this.FTBIrcPass1, "FTBIrcPass1");
            this.FTBIrcPass1.MaxLength = 32767;
            this.FTBIrcPass1.Multiline = false;
            this.FTBIrcPass1.Name = "FTBIrcPass1";
            this.FTBIrcPass1.ReadOnly = false;
            this.FTBIrcPass1.SelectedText = "";
            this.FTBIrcPass1.SelectionLength = 0;
            this.FTBIrcPass1.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBIrcPass1.SpellMark")));
            this.FTBIrcPass1.Text = global::stCoCClient.Properties.Settings.Default.IRCPassword;
            this.FTBIrcPass1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBIrcPass1.TextCaption = "IRC user password";
            this.FTBIrcPass1.TextColor = System.Drawing.Color.WhiteSmoke;
            this.FTBIrcPass1.UseSystemPasswordChar = true;
            this.FTBIrcPass1.Enter += new System.EventHandler(this.tboxCaption_Enter);
            // 
            // pictureBox12
            // 
            resources.ApplyResources(this.pictureBox12, "pictureBox12");
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.TabStop = false;
            // 
            // tabPageSetupReg
            // 
            this.tabPageSetupReg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            resources.ApplyResources(this.tabPageSetupReg, "tabPageSetupReg");
            this.tabPageSetupReg.Name = "tabPageSetupReg";
            // 
            // tabPageClanNews
            // 
            this.tabPageClanNews.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            resources.ApplyResources(this.tabPageClanNews, "tabPageClanNews");
            this.tabPageClanNews.Name = "tabPageClanNews";
            // 
            // tabPageClanStat
            // 
            this.tabPageClanStat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.tabPageClanStat.ContextMenuStrip = this.flatContextMenuStripNotify;
            this.tabPageClanStat.Controls.Add(this.FLVNotify);
            this.tabPageClanStat.Controls.Add(this.PBNotifyRenew);
            this.tabPageClanStat.Controls.Add(this.FPBNotify);
            resources.ApplyResources(this.tabPageClanStat, "tabPageClanStat");
            this.tabPageClanStat.Name = "tabPageClanStat";
            // 
            // flatContextMenuStripNotify
            // 
            resources.ApplyResources(this.flatContextMenuStripNotify, "flatContextMenuStripNotify");
            this.flatContextMenuStripNotify.ForeColor = System.Drawing.Color.White;
            this.flatContextMenuStripNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMINotifyMultiselect,
            this.TSMINotifyGroup,
            this.TSMINotifyScroll,
            this.toolStripSeparator1,
            this.TSMINotifyExport,
            this.toolStripSeparator2,
            this.TSMINotifyCopy,
            this.TSMINotifySelectAll,
            this.TSTBNotifyFind,
            this.TSCBGroup,
            this.toolStripSeparator3,
            this.TSMINotifyRenew});
            this.flatContextMenuStripNotify.Name = "flatContextMenuStripNotify";
            this.flatContextMenuStripNotify.ShowCheckMargin = true;
            this.flatContextMenuStripNotify.ShowImageMargin = false;
            this.flatContextMenuStripNotify.Opening += new System.ComponentModel.CancelEventHandler(this.flatContextMenuStripNotify_Opening);
            // 
            // TSMINotifyMultiselect
            // 
            this.TSMINotifyMultiselect.Checked = global::stCoCClient.Properties.Settings.Default.TSMIMultiselect;
            this.TSMINotifyMultiselect.CheckOnClick = true;
            this.TSMINotifyMultiselect.Name = "TSMINotifyMultiselect";
            resources.ApplyResources(this.TSMINotifyMultiselect, "TSMINotifyMultiselect");
            this.TSMINotifyMultiselect.Click += new System.EventHandler(this.TSMINotifyMultiselect_Click);
            // 
            // TSMINotifyGroup
            // 
            this.TSMINotifyGroup.Checked = global::stCoCClient.Properties.Settings.Default.TSMINotifyGroup;
            this.TSMINotifyGroup.CheckOnClick = true;
            this.TSMINotifyGroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TSMINotifyGroup.Name = "TSMINotifyGroup";
            resources.ApplyResources(this.TSMINotifyGroup, "TSMINotifyGroup");
            this.TSMINotifyGroup.Click += new System.EventHandler(this.TSMINotifyGroup_Click);
            // 
            // TSMINotifyScroll
            // 
            this.TSMINotifyScroll.Checked = global::stCoCClient.Properties.Settings.Default.TSMIScroll;
            this.TSMINotifyScroll.CheckOnClick = true;
            this.TSMINotifyScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TSMINotifyScroll.Name = "TSMINotifyScroll";
            resources.ApplyResources(this.TSMINotifyScroll, "TSMINotifyScroll");
            this.TSMINotifyScroll.Click += new System.EventHandler(this.TSMINotifyScroll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // TSMINotifyExport
            // 
            this.TSMINotifyExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMINotifyExportTXT,
            this.TSMINotifyExportCSV,
            this.TSMINotifyExportHTML});
            this.TSMINotifyExport.Name = "TSMINotifyExport";
            resources.ApplyResources(this.TSMINotifyExport, "TSMINotifyExport");
            // 
            // TSMINotifyExportTXT
            // 
            this.TSMINotifyExportTXT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSMINotifyExportTXT.ForeColor = System.Drawing.Color.White;
            this.TSMINotifyExportTXT.Name = "TSMINotifyExportTXT";
            resources.ApplyResources(this.TSMINotifyExportTXT, "TSMINotifyExportTXT");
            this.TSMINotifyExportTXT.Click += new System.EventHandler(this.TSMINotifyExportTXT_Click);
            // 
            // TSMINotifyExportCSV
            // 
            this.TSMINotifyExportCSV.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSMINotifyExportCSV.ForeColor = System.Drawing.Color.White;
            this.TSMINotifyExportCSV.Name = "TSMINotifyExportCSV";
            resources.ApplyResources(this.TSMINotifyExportCSV, "TSMINotifyExportCSV");
            this.TSMINotifyExportCSV.Click += new System.EventHandler(this.TSMINotifyExportCSV_Click);
            // 
            // TSMINotifyExportHTML
            // 
            this.TSMINotifyExportHTML.ForeColor = System.Drawing.Color.White;
            this.TSMINotifyExportHTML.Name = "TSMINotifyExportHTML";
            resources.ApplyResources(this.TSMINotifyExportHTML, "TSMINotifyExportHTML");
            this.TSMINotifyExportHTML.Click += new System.EventHandler(this.TSMINotifyExportHTML_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // TSMINotifyCopy
            // 
            this.TSMINotifyCopy.Name = "TSMINotifyCopy";
            resources.ApplyResources(this.TSMINotifyCopy, "TSMINotifyCopy");
            this.TSMINotifyCopy.Click += new System.EventHandler(this.TSMINotifyCopy_Click);
            // 
            // TSMINotifySelectAll
            // 
            this.TSMINotifySelectAll.Name = "TSMINotifySelectAll";
            resources.ApplyResources(this.TSMINotifySelectAll, "TSMINotifySelectAll");
            this.TSMINotifySelectAll.Click += new System.EventHandler(this.TSMINotifySelectAll_Click);
            // 
            // TSTBNotifyFind
            // 
            this.TSTBNotifyFind.AcceptsReturn = true;
            this.TSTBNotifyFind.AcceptsTab = true;
            this.TSTBNotifyFind.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TSTBNotifyFind.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.TSTBNotifyFind.AutoToolTip = true;
            this.TSTBNotifyFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.TSTBNotifyFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TSTBNotifyFind.ForeColor = System.Drawing.Color.White;
            this.TSTBNotifyFind.Name = "TSTBNotifyFind";
            resources.ApplyResources(this.TSTBNotifyFind, "TSTBNotifyFind");
            this.TSTBNotifyFind.DoubleClick += new System.EventHandler(this.TSTBNotifyFind_Click);
            // 
            // TSCBGroup
            // 
            this.TSCBGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TSCBGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.TSCBGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            resources.ApplyResources(this.TSCBGroup, "TSCBGroup");
            this.TSCBGroup.ForeColor = System.Drawing.Color.White;
            this.TSCBGroup.Name = "TSCBGroup";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // TSMINotifyRenew
            // 
            this.TSMINotifyRenew.Name = "TSMINotifyRenew";
            resources.ApplyResources(this.TSMINotifyRenew, "TSMINotifyRenew");
            this.TSMINotifyRenew.Click += new System.EventHandler(this.TSMINotifyRenew_Click);
            // 
            // FLVNotify
            // 
            this.FLVNotify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.FLVNotify.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FLVNotify.ContextMenuStrip = this.flatContextMenuStripNotify;
            resources.ApplyResources(this.FLVNotify, "FLVNotify");
            this.FLVNotify.ForeColor = System.Drawing.Color.Gainsboro;
            this.FLVNotify.FullRowSelect = true;
            this.FLVNotify.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.FLVNotify.MultiSelect = false;
            this.FLVNotify.Name = "FLVNotify";
            this.FLVNotify.SmallImageList = this.imageListNotify;
            this.FLVNotify.UseCompatibleStateImageBehavior = false;
            this.FLVNotify.View = System.Windows.Forms.View.Details;
            this.FLVNotify.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FLVNotify_KeyUp);
            // 
            // PBNotifyRenew
            // 
            this.PBNotifyRenew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.PBNotifyRenew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PBNotifyRenew.Image = global::stCoCClient.Properties.Resources.ic_refresh_white_18dp;
            resources.ApplyResources(this.PBNotifyRenew, "PBNotifyRenew");
            this.PBNotifyRenew.Name = "PBNotifyRenew";
            this.PBNotifyRenew.TabStop = false;
            this.PBNotifyRenew.Click += new System.EventHandler(this.PBNotifyRenew_Click);
            // 
            // FPBNotify
            // 
            this.FPBNotify.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.FPBNotify.DarkerProgress = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(148)))), ((int)(((byte)(92)))));
            resources.ApplyResources(this.FPBNotify, "FPBNotify");
            this.FPBNotify.Maximum = 100;
            this.FPBNotify.Name = "FPBNotify";
            this.FPBNotify.Pattern = true;
            this.FPBNotify.PercentSign = true;
            this.FPBNotify.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.FPBNotify.ShowBalloon = true;
            this.FPBNotify.Value = 0;
            // 
            // tabPageClanChat
            // 
            this.tabPageClanChat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.tabPageClanChat.Controls.Add(this.PBChatSend);
            this.tabPageClanChat.Controls.Add(this.FLVChatUser);
            this.tabPageClanChat.Controls.Add(this.PBChatMsgType);
            this.tabPageClanChat.Controls.Add(this.WBChat);
            this.tabPageClanChat.Controls.Add(this.FBChatConnect);
            this.tabPageClanChat.Controls.Add(this.FTBChatInput);
            resources.ApplyResources(this.tabPageClanChat, "tabPageClanChat");
            this.tabPageClanChat.Name = "tabPageClanChat";
            // 
            // PBChatSend
            // 
            this.PBChatSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.PBChatSend.Image = global::stCoCClient.Properties.Resources.ic_chat_white_18dp;
            resources.ApplyResources(this.PBChatSend, "PBChatSend");
            this.PBChatSend.Name = "PBChatSend";
            this.PBChatSend.TabStop = false;
            this.PBChatSend.Click += new System.EventHandler(this.PBChatSend_Click);
            // 
            // FLVChatUser
            // 
            this.FLVChatUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.FLVChatUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.FLVChatUser.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FLVChatUser.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(163)))), ((int)(((byte)(241)))), ((int)(((byte)(73)))));
            this.FLVChatUser.FullRowSelect = true;
            this.FLVChatUser.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            resources.ApplyResources(this.FLVChatUser, "FLVChatUser");
            this.FLVChatUser.MultiSelect = false;
            this.FLVChatUser.Name = "FLVChatUser";
            this.FLVChatUser.ShowGroups = false;
            this.FLVChatUser.SmallImageList = this.imageListChat;
            this.FLVChatUser.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.FLVChatUser.UseCompatibleStateImageBehavior = false;
            this.FLVChatUser.View = System.Windows.Forms.View.Details;
            this.FLVChatUser.SelectedIndexChanged += new System.EventHandler(this.FLVChatUser_SelectedIndexChanged);
            this.FLVChatUser.Click += new System.EventHandler(this.FLVChatUser_Click);
            // 
            // PBChatMsgType
            // 
            this.PBChatMsgType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PBChatMsgType.Image = global::stCoCClient.Properties.Resources.ic_lock_open_white_18dp;
            resources.ApplyResources(this.PBChatMsgType, "PBChatMsgType");
            this.PBChatMsgType.Name = "PBChatMsgType";
            this.PBChatMsgType.TabStop = false;
            this.PBChatMsgType.Click += new System.EventHandler(this.PBChatMsgType_Click);
            // 
            // WBChat
            // 
            this.WBChat.ContextMenuStrip = this.flatContextMenuStripChatBrowser;
            this.WBChat.IsWebBrowserContextMenuEnabled = false;
            resources.ApplyResources(this.WBChat, "WBChat");
            this.WBChat.MinimumSize = new System.Drawing.Size(20, 20);
            this.WBChat.Name = "WBChat";
            this.WBChat.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.WBChat.WebBrowserShortcutsEnabled = false;
            this.WBChat.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.WBChat_Navigating);
            // 
            // flatContextMenuStripChatBrowser
            // 
            resources.ApplyResources(this.flatContextMenuStripChatBrowser, "flatContextMenuStripChatBrowser");
            this.flatContextMenuStripChatBrowser.ForeColor = System.Drawing.Color.White;
            this.flatContextMenuStripChatBrowser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIChatClear,
            this.TSMIChatExport,
            this.toolStripSeparator6,
            this.TSMIChatArchive});
            this.flatContextMenuStripChatBrowser.Name = "flatContextMenuStripChatBrowser";
            this.flatContextMenuStripChatBrowser.ShowImageMargin = false;
            // 
            // TSMIChatClear
            // 
            this.TSMIChatClear.Image = global::stCoCClient.Properties.Resources.ic_delete_forever_white_18dp;
            this.TSMIChatClear.Name = "TSMIChatClear";
            resources.ApplyResources(this.TSMIChatClear, "TSMIChatClear");
            this.TSMIChatClear.Click += new System.EventHandler(this.TSMIChatClear_Click);
            // 
            // TSMIChatExport
            // 
            this.TSMIChatExport.Image = global::stCoCClient.Properties.Resources.ic_create_new_folder_white_18dp;
            this.TSMIChatExport.Name = "TSMIChatExport";
            resources.ApplyResources(this.TSMIChatExport, "TSMIChatExport");
            this.TSMIChatExport.Click += new System.EventHandler(this.TSMIChatExport_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            // 
            // TSMIChatArchive
            // 
            this.TSMIChatArchive.Image = global::stCoCClient.Properties.Resources.ic_cast_connected_white_18dp;
            this.TSMIChatArchive.Name = "TSMIChatArchive";
            resources.ApplyResources(this.TSMIChatArchive, "TSMIChatArchive");
            this.TSMIChatArchive.Click += new System.EventHandler(this.TSMIChatArchive_Click);
            // 
            // FBChatConnect
            // 
            this.FBChatConnect.BackColor = System.Drawing.Color.Transparent;
            this.FBChatConnect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(168)))), ((int)(((byte)(109)))));
            this.FBChatConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.FBChatConnect, "FBChatConnect");
            this.FBChatConnect.Name = "FBChatConnect";
            this.FBChatConnect.Rounded = false;
            this.FBChatConnect.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.FBChatConnect.Click += new System.EventHandler(this.FBChatConnect_Click);
            // 
            // FTBChatInput
            // 
            this.FTBChatInput.BackColor = System.Drawing.Color.Gray;
            this.FTBChatInput.ContextMenuStrip = this.flatContextMenuStripChatInput;
            this.FTBChatInput.FocusOnHover = true;
            resources.ApplyResources(this.FTBChatInput, "FTBChatInput");
            this.FTBChatInput.MaxLength = 32767;
            this.FTBChatInput.Multiline = false;
            this.FTBChatInput.Name = "FTBChatInput";
            this.FTBChatInput.ReadOnly = false;
            this.FTBChatInput.SelectedText = "";
            this.FTBChatInput.SelectionLength = 0;
            this.FTBChatInput.SpellMark = ((System.Collections.Generic.List<stCore.TxtPosition>)(resources.GetObject("FTBChatInput.SpellMark")));
            this.FTBChatInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FTBChatInput.TextCaption = null;
            this.FTBChatInput.TextColor = System.Drawing.Color.White;
            this.FTBChatInput.UseSystemPasswordChar = false;
            this.FTBChatInput.TextChanged += new System.EventHandler(this.FTBChatInput_TextChanged);
            this.FTBChatInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FTBChatInput_KeyDown);
            // 
            // flatClose1
            // 
            resources.ApplyResources(this.flatClose1, "flatClose1");
            this.flatClose1.BackColor = System.Drawing.Color.White;
            this.flatClose1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.flatClose1.Name = "flatClose1";
            this.flatClose1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            // 
            // flatMinimize
            // 
            resources.ApplyResources(this.flatMinimize, "flatMinimize");
            this.flatMinimize.BackColor = System.Drawing.Color.White;
            this.flatMinimize.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(47)))), ((int)(((byte)(49)))));
            this.flatMinimize.Name = "flatMinimize";
            this.flatMinimize.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.flatMinimize.Click += new System.EventHandler(this.flatMinimize_Click);
            // 
            // flatContextMenuStripChatWeb
            // 
            resources.ApplyResources(this.flatContextMenuStripChatWeb, "flatContextMenuStripChatWeb");
            this.flatContextMenuStripChatWeb.ForeColor = System.Drawing.Color.White;
            this.flatContextMenuStripChatWeb.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIInsertNick,
            this.TSMIReplyNick,
            this.TSMICopyNick,
            this.TSMIPrivateMsg,
            this.TSMIKickNick,
            this.toolStripSeparator9,
            this.TSMIReloadUserList});
            this.flatContextMenuStripChatWeb.Name = "flatContextMenuStripChatRichText";
            this.flatContextMenuStripChatWeb.ShowImageMargin = false;
            // 
            // TSMIInsertNick
            // 
            this.TSMIInsertNick.Image = global::stCoCClient.Properties.Resources.insert;
            this.TSMIInsertNick.Name = "TSMIInsertNick";
            resources.ApplyResources(this.TSMIInsertNick, "TSMIInsertNick");
            this.TSMIInsertNick.Click += new System.EventHandler(this.TSMIInsertNick_Click);
            // 
            // TSMIReplyNick
            // 
            this.TSMIReplyNick.Image = global::stCoCClient.Properties.Resources.reply;
            this.TSMIReplyNick.Name = "TSMIReplyNick";
            resources.ApplyResources(this.TSMIReplyNick, "TSMIReplyNick");
            this.TSMIReplyNick.Click += new System.EventHandler(this.TSMIReplyNick_Click);
            // 
            // TSMICopyNick
            // 
            this.TSMICopyNick.Image = global::stCoCClient.Properties.Resources.copy;
            this.TSMICopyNick.Name = "TSMICopyNick";
            resources.ApplyResources(this.TSMICopyNick, "TSMICopyNick");
            this.TSMICopyNick.Click += new System.EventHandler(this.TSMICopyNick_Click);
            // 
            // TSMIPrivateMsg
            // 
            this.TSMIPrivateMsg.Image = global::stCoCClient.Properties.Resources.ic_lock_outline_white_18dp;
            this.TSMIPrivateMsg.Name = "TSMIPrivateMsg";
            resources.ApplyResources(this.TSMIPrivateMsg, "TSMIPrivateMsg");
            this.TSMIPrivateMsg.Click += new System.EventHandler(this.TSMIPrivateMsg_Click);
            // 
            // TSMIKickNick
            // 
            this.TSMIKickNick.Image = global::stCoCClient.Properties.Resources.ic_delete_forever_white_18dp;
            this.TSMIKickNick.Name = "TSMIKickNick";
            resources.ApplyResources(this.TSMIKickNick, "TSMIKickNick");
            this.TSMIKickNick.Click += new System.EventHandler(this.TSMIKickNick_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            // 
            // TSMIReloadUserList
            // 
            this.TSMIReloadUserList.Image = global::stCoCClient.Properties.Resources.ic_refresh_white_18dp;
            this.TSMIReloadUserList.Name = "TSMIReloadUserList";
            resources.ApplyResources(this.TSMIReloadUserList, "TSMIReloadUserList");
            this.TSMIReloadUserList.Click += new System.EventHandler(this.TSMIReloadUserList_Click);
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            resources.ApplyResources(this.notifyIconMain, "notifyIconMain");
            this.notifyIconMain.ContextMenuStrip = this.flatContextMenuStripTray;
            this.notifyIconMain.DoubleClick += new System.EventHandler(this.notifyIconMain_DoubleClick);
            this.notifyIconMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseClick);
            // 
            // flatContextMenuStripTray
            // 
            resources.ApplyResources(this.flatContextMenuStripTray, "flatContextMenuStripTray");
            this.flatContextMenuStripTray.ForeColor = System.Drawing.Color.White;
            this.flatContextMenuStripTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIOpenApp,
            this.toolStripSeparator10,
            this.TSMIOpenClanStat,
            this.TSMIOpenClanChat,
            this.TSMIOpenSetup,
            this.toolStripSeparator11,
            this.TSMIOpenInformer,
            this.toolStripSeparator12,
            this.TSMIAppExit});
            this.flatContextMenuStripTray.Name = "flatContextMenuStripTray";
            this.flatContextMenuStripTray.ShowImageMargin = false;
            this.flatContextMenuStripTray.Opening += new System.ComponentModel.CancelEventHandler(this.flatContextMenuStripTray_Opening);
            // 
            // TSMIOpenApp
            // 
            this.TSMIOpenApp.Image = global::stCoCClient.Properties.Resources.ic_touch_app_white_18dp;
            this.TSMIOpenApp.Name = "TSMIOpenApp";
            resources.ApplyResources(this.TSMIOpenApp, "TSMIOpenApp");
            this.TSMIOpenApp.Click += new System.EventHandler(this.TSMIOpenApp_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // TSMIOpenClanStat
            // 
            this.TSMIOpenClanStat.Image = global::stCoCClient.Properties.Resources.ic_import_contacts_white_18dp;
            this.TSMIOpenClanStat.Name = "TSMIOpenClanStat";
            resources.ApplyResources(this.TSMIOpenClanStat, "TSMIOpenClanStat");
            this.TSMIOpenClanStat.Click += new System.EventHandler(this.TSMIOpenClanStat_Click);
            // 
            // TSMIOpenClanChat
            // 
            this.TSMIOpenClanChat.Image = global::stCoCClient.Properties.Resources.ic_chat_white_18dp;
            this.TSMIOpenClanChat.Name = "TSMIOpenClanChat";
            resources.ApplyResources(this.TSMIOpenClanChat, "TSMIOpenClanChat");
            this.TSMIOpenClanChat.Click += new System.EventHandler(this.TSMIOpenClanChat_Click);
            // 
            // TSMIOpenSetup
            // 
            this.TSMIOpenSetup.Image = global::stCoCClient.Properties.Resources.ic_settings_applications_white_18dp;
            this.TSMIOpenSetup.Name = "TSMIOpenSetup";
            resources.ApplyResources(this.TSMIOpenSetup, "TSMIOpenSetup");
            this.TSMIOpenSetup.Click += new System.EventHandler(this.TSMIOpenSetup_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // TSMIOpenInformer
            // 
            this.TSMIOpenInformer.Image = global::stCoCClient.Properties.Resources.ic_error_outline_white_18dp;
            this.TSMIOpenInformer.Name = "TSMIOpenInformer";
            resources.ApplyResources(this.TSMIOpenInformer, "TSMIOpenInformer");
            this.TSMIOpenInformer.Click += new System.EventHandler(this.TSMIOpenInformer_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // TSMIAppExit
            // 
            this.TSMIAppExit.Image = global::stCoCClient.Properties.Resources.ic_power_settings_new_white_18dp;
            this.TSMIAppExit.Name = "TSMIAppExit";
            resources.ApplyResources(this.TSMIAppExit, "TSMIAppExit");
            this.TSMIAppExit.Click += new System.EventHandler(this.TSMIAppExit_Click);
            // 
            // ClientForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.formSkinMain);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClientForm";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.MinimumSizeChanged += new System.EventHandler(this.ClientForm_MinimumSizeChanged);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderSetup)).EndInit();
            this.formSkinMain.ResumeLayout(false);
            this.formSkinMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCallInformer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSetupMain)).EndInit();
            this.flatContextMenuStripChatInput.ResumeLayout(false);
            this.flatTabControlMain.ResumeLayout(false);
            this.tabPageSetupMain.ResumeLayout(false);
            this.flatGroupBox3.ResumeLayout(false);
            this.flatGroupBox2.ResumeLayout(false);
            this.flatGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox15)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.flatGroupBox1.ResumeLayout(false);
            this.flatGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPageSetupUser.ResumeLayout(false);
            this.flatGroupBox5.ResumeLayout(false);
            this.flatGroupBox4.ResumeLayout(false);
            this.flatGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            this.tabPageClanStat.ResumeLayout(false);
            this.flatContextMenuStripNotify.ResumeLayout(false);
            this.flatContextMenuStripNotify.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBNotifyRenew)).EndInit();
            this.tabPageClanChat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PBChatSend)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBChatMsgType)).EndInit();
            this.flatContextMenuStripChatBrowser.ResumeLayout(false);
            this.flatContextMenuStripChatWeb.ResumeLayout(false);
            this.flatContextMenuStripTray.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private stCoreUI.FormSkin formSkinMain;
        private stCoreUI.FlatClose flatClose1;
        private stCoreUI.FlatMini flatMinimize;
        private stCoreUI.FlatTabControl flatTabControlMain;
        private System.Windows.Forms.TabPage tabPageClanStat;
        private System.Windows.Forms.TabPage tabPageClanChat;
        private stCoreUI.FlatStatusBar flatSB;
        private stCoreUI.FlatAlertBox flatAlertBoxMain;
        private stCoreUI.FlatTextBox FTBChatInput;
        private stCoreUI.FlatButton FBChatConnect;
        private stCoreUI.FlatContextMenuStrip flatContextMenuStripChatWeb;
        private System.Windows.Forms.ToolStripMenuItem TSMIPrivateMsg;
        private System.Windows.Forms.ToolStripMenuItem TSMIInsertNick;
        private System.Windows.Forms.ToolStripMenuItem TSMIKickNick;
        private stCoreUI.FlatContextMenuStrip flatContextMenuStripChatInput;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputCopy;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputPaste;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputSelect;
        private System.Windows.Forms.TabPage tabPageSetupMain;
        private System.Windows.Forms.TabPage tabPageClanNews;
        private stCoreUI.FlatGroupBox flatGroupBox1;
        private stCoreUI.FlatTextBox FTBIRCServer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private stCoreUI.FlatTextBox FTBIRCPort;
        private stCoreUI.FlatGroupBox flatGroupBox2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private stCoreUI.FlatTextBox FTBIrcPass2;
        private System.Windows.Forms.PictureBox pictureBox5;
        private stCoreUI.FlatTextBox FTBIrcNick2;
        private System.Windows.Forms.PictureBox pictureBox6;
        private stCoreUI.FlatTextBox FTBCoCServer2ro;
        private System.Windows.Forms.PictureBox pictureBox3;
        private stCoreUI.FlatTextBox FTBIRCChannel;
        private stCoreUI.FlatToggle flatToggle1;
        private stCoreUI.FlatLabel flatLabel7;
        private stCoreUI.FlatLabel flatLabel8;
        private stCoreUI.FlatToggle flatToggle2;
        private stCoreUI.FlatLabel flatLabel10;
        private stCoreUI.FlatToggle flatToggle4;
        private stCoreUI.FlatLabel flatLabel9;
        private stCoreUI.FlatToggle flatToggle3;
        private System.Windows.Forms.PictureBox pictureBox7;
        private stCoreUI.FlatTextBox FTBGameTag2;
        private stCoreUI.FlatLabel flatLabel12;
        private System.Windows.Forms.PictureBox pictureBox8;
        //private stCoreUI.FlatNumeric flatNumericInformerId;
        private stCoreUI.FlatGroupBox flatGroupBox3;
        private stCoreUI.FlatCheckBox cboxMemberNew;
        private stCoreUI.FlatCheckBox flatCheckBox4;
        private stCoreUI.FlatCheckBox cboxMemberChangeNik;
        private stCoreUI.FlatCheckBox cboxMemberExit;
        private stCoreUI.FlatCheckBox cboxClanChangeMembers;
        private stCoreUI.FlatCheckBox cboxClanChangePoints;
        private stCoreUI.FlatCheckBox cboxMemberChangeDonationReceive;
        private stCoreUI.FlatCheckBox cboxMemberChangeDonationSend;
        private stCoreUI.FlatCheckBox cboxMemberChangeTrophies;
        private stCoreUI.FlatCheckBox cboxMemberChangeLeague;
        private stCoreUI.FlatCheckBox cboxMemberChangeLevel;
        private stCoreUI.FlatCheckBox cboxMemberChangeRole;
        private stCoreUI.FlatCheckBox cboxWarClanEnd;
        private stCoreUI.FlatButton bboxSave;
        private stCoreUI.FlatButton bboxReload;
        private stCoreUI.FlatNumeric flatNumericInformerId1;
        private stCoreUI.FlatButton FBWizard;
        private System.Windows.Forms.PictureBox picBoxSetupMain;
        private System.Windows.Forms.TabPage tabPageSetupUser;
        private stCoreUI.FlatGroupBox flatGroupBox4;
        private stCoreUI.FlatNumeric flatNumericInformerId2;
        private stCoreUI.FlatLabel flatLabel1;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.PictureBox pictureBox10;
        private stCoreUI.FlatTextBox FTBGameTag1;
        private System.Windows.Forms.PictureBox pictureBox11;
        private stCoreUI.FlatTextBox FTBIrcPass1;
        private System.Windows.Forms.PictureBox pictureBox12;
        private stCoreUI.FlatTextBox FTBIrcNick1;
        private System.Windows.Forms.PictureBox pictureBox13;
        private stCoreUI.FlatTextBox FTBCoCServer1;
        private System.Windows.Forms.ErrorProvider errorProviderSetup;
        private stCoreUI.FlatButton flatButton2;
        private stCoreUI.FlatButton flatButton3;
        private stCoreUI.FlatGroupBox flatGroupBox5;
        private stCoreUI.FlatButton flatButton4;
        private System.Windows.Forms.ListView FLVSetup;
        private System.Windows.Forms.PictureBox pictureBox14;
        private stCoreUI.FlatTextBox FTBWikiHome;
        private stCoreUI.FlatCheckBox FCBNikAuto;
        private stCoreUI.FlatStickyButton FSBRegistred;
        private stCoreUI.FlatLabel flatLabel2;
        private System.Windows.Forms.TabPage tabPageSetupReg;
        private System.ComponentModel.BackgroundWorker bgNotifyWorker;
        private stCoreUI.FlatProgressBar FPBNotify;
        private System.Windows.Forms.PictureBox PBNotifyRenew;
        private System.Windows.Forms.ListView FLVNotify;
        private stCoreUI.FlatContextMenuStrip flatContextMenuStripNotify;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyMultiselect;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyExport;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyExportCSV;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyExportTXT;
        private System.Windows.Forms.ImageList imageListNotify;
        private System.Windows.Forms.FolderBrowserDialog FBDNotifyExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyRenew;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyScroll;
        private System.Windows.Forms.Timer TimerRunNotify;
        private System.Windows.Forms.ToolStripTextBox TSTBNotifyFind;
        private System.Windows.Forms.ToolStripComboBox TSCBGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifySelectAll;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyGroup;
        private System.Windows.Forms.ToolStripMenuItem TSMINotifyExportHTML;
        private stCoreUI.FlatStickyButton FSBExportPath;
        private stCoreUI.FlatTextBox FTBExportPath;
        private System.Windows.Forms.PictureBox pictureBox15;
        private System.Windows.Forms.WebBrowser WBChat;
        private System.Windows.Forms.ToolStripMenuItem TSMICopyNick;
        private System.Windows.Forms.ToolStripMenuItem TSMIReplyNick;
        private System.Windows.Forms.PictureBox PBChatMsgType;
        private stCoreUI.FlatContextMenuStrip flatContextMenuStripChatBrowser;
        private System.Windows.Forms.ToolStripMenuItem TSMIChatClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputSmilesNorm;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles1;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles2;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles3;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputClear;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles4;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles5;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles6;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles7;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles8;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles9;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles10;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputSmilesBlink;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles11;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles12;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles13;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles14;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles15;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles16;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles17;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles18;
        private System.Windows.Forms.ToolStripMenuItem TSMISmiles19;
        private System.Windows.Forms.ListView FLVChatUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputSend;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputSendPriv;
        private System.Windows.Forms.ToolStripTextBox TSTBFindNick;
        private System.Windows.Forms.PictureBox PBChatSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem TSMIChatArchive;
        private System.Windows.Forms.ImageList imageListChat;
        private System.Windows.Forms.ToolStripMenuItem TSMIChatExport;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputStyle;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputImage;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputUrl;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputStyleBold;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputStyleItalic;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputStyleUnderline;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputStyleStrike;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColorRed;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColorYellow;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColorGreen;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColorCyan;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColorBlue;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColorMagenta;
        private System.Windows.Forms.ToolStripTextBox TSTBInputImage;
        private System.Windows.Forms.ToolStripTextBox TSTBInputUrl;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputSpell;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem TSMIInputColorCustom;
        private System.Windows.Forms.ColorDialog colorDialogInput;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem TSMIReloadUserList;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private stCoreUI.FlatContextMenuStrip flatContextMenuStripTray;
        private System.Windows.Forms.ToolStripMenuItem TSMIOpenApp;
        private System.Windows.Forms.ToolStripMenuItem TSMIOpenClanStat;
        private System.Windows.Forms.ToolStripMenuItem TSMIOpenClanChat;
        private System.Windows.Forms.ToolStripMenuItem TSMIOpenSetup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem TSMIAppExit;
        private System.Windows.Forms.PictureBox PBCallInformer;
        private System.Windows.Forms.ToolStripMenuItem TSMIOpenInformer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;

    }
}

