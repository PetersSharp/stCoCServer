using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using stCore;
using stClient;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace stCoCClient
{
    public partial class ClientForm
    {
        private static object webwLock = null;
        private stClient.IrcClient Irc = null;
        private const string _sysNoticeTag = @"IRC System";

        #region CALLBACK

        private void _InitIrcCallBack()
        {

            #region USER MATHCH

            this.Irc.OnUserUpdate += (o, e) =>
            {
                if (e.UserList.Length > 0)
                {
                    foreach (string user in e.UserList)
                    {
                        if (user.Length == 0)
                        {
                            continue;
                        }
                        this.ChatUserListAdd(user);
                    }
                    this.ChatUserListSortColor();
                    this.SetChatUserListAutocomplete();
                }
            };
            this.Irc.OnUserJoined += (o, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.User))
                {
                    this.ChatUserListAdd(e.User);
                    if (!Properties.Settings.Default.IRCNoticeMessage)
                    {
                        lock (webwLock)
                        {
                            this.WBChat.Document.InvokeScript("MsgPrn",
                                new object[] {
                                    "msgnotice",
                                    e.User,
                                    e.Channel + " + " + e.User
                                }
                            );
                        }
                    }
                }
            };
            this.Irc.OnUserLeft += (o, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.User))
                {
                    this.ChatUserListDel(e.User);
                    if (!Properties.Settings.Default.IRCNoticeMessage)
                    {
                        lock (webwLock)
                        {
                            this.WBChat.Document.InvokeScript("MsgPrn",
                                new object[] {
                                    "msgnotice",
                                    e.User,
                                    e.Channel + " - " + e.User
                                }
                            );
                        }
                    }
                }
            };
            this.Irc.OnUserKick += (o, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.User))
                {
                    this.ChatUserListDel(e.User);
                    if (!Properties.Settings.Default.IRCNoticeMessage)
                    {
                        lock (webwLock)
                        {
                            this.WBChat.Document.InvokeScript("MsgPrn",
                                new object[] {
                                    "msgnotice",
                                    e.User,
                                    e.Channel + " KICK " + e.User + " : " + e.Message
                                }
                            );
                        }
                    }
                }
            };
            this.Irc.OnUserNickChange += (o, e) =>
            {
                string Unew = ((string.IsNullOrWhiteSpace(e.New)) ? String.Empty : e.New),
                       Uold = ((string.IsNullOrWhiteSpace(e.Old)) ? String.Empty : e.Old);

                this.ChatUserListAdd(Unew);
                this.ChatUserListDel(Uold);

                if (!Properties.Settings.Default.IRCNoticeMessage)
                {
                    lock (webwLock)
                    {
                        this.WBChat.Document.InvokeScript("MsgPrn",
                            new object[] {
                                    "msgnotice",
                                    Unew,
                                    "NICK Change: " + Uold + " -&gt; " + Unew
                                }
                        );
                    }
                }
            };

            #endregion

            #region CHANNEL/SERVER MESSAGE

            this.Irc.OnMessageServer += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Result))
                {
                    return;
                }
                if (Properties.Settings.Default.IRCServerMessage)
                {
                    lock (webwLock)
                    {
                        this.WBChat.Document.InvokeScript("MsgPrn",
                            new object[] {
                                "msgserver",
                                "",
                                e.Result.TrimEnd(new char [] { '\n', '\r', ' '})
                            }
                        );
                    }
                }
            };
            this.Irc.OnMessageChannel += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Message))
                {
                    return;
                }
                lock (webwLock)
                {
                    this.WBChat.Document.InvokeScript("MsgPrn",
                        new object[] {
                            "msguser",
                            e.From,
                            this._IRCChatToHTML(e.Message)
                        }
                    );
                }
                if (this.isMinimized)
                {
                    this.notifyIconMain_Balloon(
                        e.Message,
                        string.Format(
                            Properties.Resources.fmtIRCBallonTitle,
                            e.From,
                            ""
                        ),
                        ToolTipIcon.None,
                        3000
                    );
                }
            };
            this.Irc.OnMessagePrivate += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Message))
                {
                    return;
                }
                if (Properties.Settings.Default.IRCPrivateMessage)
                {
                    lock (webwLock)
                    {
                        this.WBChat.Document.InvokeScript("MsgPrn",
                            new object[] {
                                "msgpriv",
                                e.From,
                                this._IRCChatToHTML(e.Message)
                            }
                        );
                    }
                    if (this.isMinimized)
                    {
                        this.notifyIconMain_Balloon(
                            e.Message, 
                            string.Format(
                                Properties.Resources.fmtIRCBallonTitle,
                                Properties.Resources.txtIRCForYouMessage,
                                e.From
                            ),
                            ToolTipIcon.None,
                            3000
                        );
                    }
                }
            };
            this.Irc.OnMessageNotice += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Message))
                {
                    return;
                }
                if (Properties.Settings.Default.IRCNoticeMessage)
                {
                    lock (webwLock)
                    {
                        this.WBChat.Document.InvokeScript("MsgPrn",
                            new object[] {
                                "msgnotice",
                                e.From,
                                e.Message
                            }
                        );
                    }
                }
            };
            #endregion

            #region CHANNEL/SERVER

            this.Irc.OnConnect += (o, e) =>
            {
                this.Irc.JoinChannel(Properties.Settings.Default.IRCChannel);
                lock (webwLock)
                {
                    this.WBChat.Document.InvokeScript("MsgPrn",
                        new object[] {
                            "msgnotice",
                            Properties.Settings.Default.IRCServer,
                            Properties.Settings.Default.IRCChannel
                        }
                    );
                }
                this.FBChatConnect.Text = Properties.Resources.txtIRCDisConnectButton;
                this.isIrcJoin = true;
            };
            this.Irc.OnExceptionThrown += (o, e) =>
            {
                if ((e.Exception == null) || (string.IsNullOrWhiteSpace(e.Exception.Message)))
                {
                    return;
                }
                lock (webwLock)
                {
                    this.WBChat.Document.InvokeScript("MsgPrn",
                        new object[] {
                            "msgnotice",
                            _sysNoticeTag,
                            ((string.IsNullOrWhiteSpace(e.SocketError)) ? "" : "[" + e.SocketError + "]" +
                            ((e.Count > 0) ? "/" + e.Count.ToString() + " " : "") + e.Exception.Message)
                        }
                    );
                }
                if (e.Fatal)
                {
                    this.isIrcJoin = false;
                    this.FBChatConnectSetDisconnected();
                }
            };

            #endregion

            #endregion
        }
    }
}

