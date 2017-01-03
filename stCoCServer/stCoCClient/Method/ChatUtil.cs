using System;
using System.Windows.Forms;
using stClient;
using System.Globalization;

namespace stCoCClient
{
    public partial class ClientForm : Form
    {
        public void _IRCInit()
        {
            try
            {
                if (!this._CheckSetupIRCChat())
                {
                    throw new ArgumentException(Properties.Resources.txtIRCSetupErrorComplette);
                }

                int port = 0;

                if (!Int32.TryParse(Properties.Settings.Default.IRCPort, out port))
                {
                    throw new ArgumentException(Properties.Resources.txtIRCSetupErrorPort);
                }

                if (this.Irc != null)
                {
                    this.Irc.Disconnect();
                    this.Irc.Dispose();
                    this.Irc = null;
                }

                this.Irc = new IrcClient(
                    Properties.Settings.Default.IRCServer,
                    port
                );
                this.Irc.Nick = this._IRCGetNickName(Properties.Settings.Default.IRCNik);
                this.Irc.ConsoleOutput = false;
                this.Irc.KickRespawn = Properties.Settings.Default.IRCKickRespawn;
                this.Irc.ServerPass =
                    ((!string.IsNullOrWhiteSpace(Properties.Settings.Default.IRCPassword)) ?
                        Properties.Settings.Default.IRCPassword : ""
                    );
                this.Irc.iLog = this._iLog;
                this._InitIrcCallBack();
            }
            catch (Exception ex)
            {
                this._winMessageError(ex.Message);
            }
        }
        public void _WBInit()
        {
            this.isWebBrowser = false;
            ((HtmlDocument)this.WBChat.Document).Write(String.Empty);
            this.WBChat.DocumentText = Properties.Resources.webChatTemplate;
            this.WBChat.ObjectForScripting = this;
            while (this.WBChat.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            this.isWebBrowser = true;
            lock (webwLock)
            {
                this.WBChat.Document.InvokeScript("MsgPrn",
                    new object[]
                    {
                        "msghistory",
                        this._IRCArchiveUrl(),
                        Properties.Resources.txtChatIrcArchive
                    }
                );
            }
        }
        public string _IRCGetNickName(string val)
        {
            if (val.Length < 8)
            {
                for (int i = val.Length; i <= 8; i++)
                {
                    val += "_";
                }
            }
            return val;
        }
        public string _IRCChatToHTML(string val)
        {
            if (Properties.Settings.Default.IRCChatEmojiEnabled)
            {
                val = val.EmoticonsToHtml(Properties.Resources.fmtEmojiHtml);
            }
            if (Properties.Settings.Default.IRCChatBBCodeEnabled)
            {
                val = val.BBCodeToHtml();
            }
            return val;
        }
        private string _IRCArchiveUrl()
        {
            try
            {
                return string.Format(
                    Properties.Settings.Default.URLIrcArchive,
                    Properties.Settings.Default.USRCoCServer,
                    ((Properties.Settings.Default.USRUrl.Count >= 4) ? Properties.Settings.Default.USRUrl[3] : "irclog"),
                    DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
