using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using stCore;
using System.IO;

namespace stCoCServer
{
    partial class CoCServerMain
    {
        private static stTimerWait reapeatIRCConnect = new stTimerWait();

        #region IRC Message contains

        private static bool _isHandleIrcMessage(string message)
        {
            if ((message[0] == '@') || (message[0] == '!'))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region IRC Set Channel

        private static void _IrcSetChannel(string path)
        {
            try
            {
                if (
                     (string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.IRCChannel.value)) ||
                     (string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.CLANTag.value)) ||
                     (string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.WEBRootUri.value))
                   )
                {
                    return;
                }
                File.WriteAllText(
                     path,
                     string.Format(
                        Properties.Resources.fmtIrcSetChannel,
                        CoCServerMain.Conf.Opt.IRCChannel.value,
                        CoCServerMain.Conf.Opt.CLANTag.value,
                        ((string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.DOKUWikiRootUrl.value)) ?
                            CoCServerMain.Conf.Opt.WEBRootUri.value :
                            CoCServerMain.Conf.Opt.DOKUWikiRootUrl.value
                        )
                     )
                );
            }
#if DEBUG
            catch (Exception e)
            {
                if (CoCServerMain.Conf.ILog != null)
                {
                    CoCServerMain.Conf.ILog.LogInfo("DEBUG SET IRC CHANNEL CMD: " + e.Message);
                }
#else
            catch (Exception)
            {
#endif
            }
        }

        #endregion

        #region All CALLBACK IRC Init

        private static void InitIrcCallBack()
        {
            #region USER MATHCH

            CoCServerMain.Conf.Irc.OnUserUpdate += (o, e) =>
            {
                if (e.UserList.Length > 0)
                {
                    foreach (string user in e.UserList)
                    {
                        if (user.Length == 0)
                        {
                            continue;
                        }
                        CoCServerMain.Conf.LogDump.Write(
                            string.Format(
                                Properties.Resources.ircUserList,
                                DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                                user
                            )
                        );
                    }
                }
            };
            CoCServerMain.Conf.Irc.OnUserJoined += (o, e) =>
            {
                CoCServerMain.Conf.LogDump.Write(
                    string.Format(
                        Properties.Resources.ircUserJoined,
                        DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                        e.User
                    )
                );
                CoCServerMain.Conf.Irc.SendMessage(
                    CoCServerMain.Conf.Opt.IRCChannel.value,
                    string.Format(
                        Properties.Resources.PrnRun,
                        DateTime.Now,
                        Properties.Resources.PrnRunIrcHelp
                    )
                );
            };
            CoCServerMain.Conf.Irc.OnUserLeft += (o, e) =>
            {
                CoCServerMain.Conf.LogDump.Write(
                    string.Format(
                        Properties.Resources.ircUserExit,
                        DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                        e.User
                    )
                );
            };
            CoCServerMain.Conf.Irc.OnUserKick += (o, e) =>
            {
                CoCServerMain.Conf.LogDump.Write(
                    string.Format(
                        Properties.Resources.ircUserKicked,
                        DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                        e.Channel,
                        e.User,
                        ((string.IsNullOrWhiteSpace(e.Message)) ? "" : " -> '" + e.Message + "'")
                    )
                );
            };
            CoCServerMain.Conf.Irc.OnUserNickChange += (o, e) =>
            {
                CoCServerMain.Conf.LogDump.Write(
                    string.Format(
                        Properties.Resources.ircUserChangeNick,
                        DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                        e.Old,
                        e.New
                    )
                );
            };

            #endregion

            #region CHANNEL/SERVER MESSAGE

            CoCServerMain.Conf.Irc.OnMessageServer += (o, e) =>
            {
                if (CoCServerMain.Conf.Opt.IRCServerMessage.bval)
                {
                    CoCServerMain.Conf.LogDump.Write(
                        string.Format(
                            Properties.Resources.ircServerMessage,
                            DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                            e.Result
                        )
                    );
                }
            };
            CoCServerMain.Conf.Irc.OnMessageChannel += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Message))
                {
                    return;
                }
                if (
                    (CoCServerMain._isHandleIrcMessage(e.Message)) &&
                    (CoCServerMain.Conf.IrcCmd.PluginParseCmd(false, e.Channel, e.From, e.Message))
                   ) { }
                else
                {
                    CoCServerMain.Conf.LogDump.Write(
                        string.Format(
                            Properties.Resources.ircChannelMessage,
                            DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                            e.From,
                            e.Message
                        )
                    );
                    CoCServerMain.Conf.IrcCmd.PluginParseContext(false, CoCServerMain.Conf.Opt.IRCChannel.value, e.From, e.Message);
                }
            };
            CoCServerMain.Conf.Irc.OnMessagePrivate += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Message))
                {
                    return;
                }
                if (
                    (CoCServerMain._isHandleIrcMessage(e.Message)) &&
                    (CoCServerMain.Conf.IrcCmd.PluginParseCmd(true, CoCServerMain.Conf.Opt.IRCChannel.value, e.From, e.Message))
                   ) { }
                else
                {
                    CoCServerMain.Conf.LogDump.Write(
                        string.Format(
                            Properties.Resources.ircPrivateMessage,
                            DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                            e.From,
                            e.Message
                        )
                    );
                    CoCServerMain.Conf.IrcCmd.PluginParseContext(true, CoCServerMain.Conf.Opt.IRCChannel.value, e.From, e.Message);
                }
            };
            CoCServerMain.Conf.Irc.OnMessageNotice += (o, e) =>
            {
                if (CoCServerMain.Conf.Opt.IRCNoticeMessage.bval)
                {
                    CoCServerMain.Conf.LogDump.Write(
                        string.Format(
                            Properties.Resources.ircNoticeMessage,
                            DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                            e.From,
                            e.Message
                        )
                    );
                }
            };
            #endregion

            #region CHANNEL/SERVER SYS Event

            CoCServerMain.Conf.Irc.OnConnect += (o, e) =>
            {
                Action<string> act = null;
                if (CoCServerMain.Conf.Opt.IRCSetNewChannel.bval)
                {
                    act = CoCServerMain._IrcSetChannel;
                }
                CoCServerMain.Conf.Irc.JoinChannel(CoCServerMain.Conf.Opt.IRCChannel.value);
                CoCServerMain.Conf.Irc.ExecCmdWatch(
                    Path.Combine(
                        CoCServerMain.Conf.Opt.SYSROOTPath.value,
                        "command.irc"
                    ),
                    act
                );
                CoCServerMain.Conf.ILog.LogInfo(
                    string.Format(
                        Properties.Resources.PrnIrcConnected,
                        CoCServerMain.Conf.Opt.IRCServer.value,
                        CoCServerMain.Conf.Opt.IRCChannel.value
                    )
                );
                if (Conf.Opt.IRCPluginContextUrlTitleEnable.bval)
                {
                    CoCServerMain.Conf.ILog.LogInfo(Properties.Resources.PrnIrcPluginContextUrlTitleStart);
                }
                CoCServerMain.Conf.IrcCmd.PluginLoopNotify();
            };
            CoCServerMain.Conf.Irc.OnExceptionThrown += (o, e) =>
            {
                CoCServerMain.Conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.PrnIrcException,
                        ((string.IsNullOrWhiteSpace(e.SocketError)) ? "" : "[" + e.SocketError + "]"),
                        ((e.Count > 0) ? "/" + e.Count.ToString() + " " : ""),
                        e.Exception.Message
                    )
                );
                if (e.Fatal)
                {
                    lock (reapeatIRCConnect)
                    {
                        reapeatIRCConnect.timer = new Timer((cbo) =>
                        {
                            CoCServerMain.Conf.ILog.LogInfo(
                                string.Format(
                                    Properties.Resources.fmtIRCReConnect,
                                    CoCServerMain.Conf.Opt.IRCServer.value,
                                    CoCServerMain.Conf.Opt.IRCPort.num,
                                    CoCServerMain.Conf.Opt.IRCChannel.value
                                )
                            );
                            try
                            {
                                CoCServerMain.Conf.Irc.Connect();
                            }
                            catch (Exception ex)
                            {
                                CoCServerMain.Conf.ILog.LogError(
                                    string.Format(
                                        Properties.Resources.fmtIRCError,
                                        CoCServerMain.Conf.Opt.IRCServer.value,
                                        ex.Message
                                    )
                                );
                            }
                            lock (cbo)
                            {
                                ((stTimerWait)cbo).Dispose();
                            }
                        }, reapeatIRCConnect, (5 * 60 * 1000), -1);
                    }
                }
            };

            #endregion
        }


        #endregion
    }
}
