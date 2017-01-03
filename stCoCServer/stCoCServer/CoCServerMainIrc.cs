using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using stCore;

namespace stCoCServer
{
    partial class CoCServerMain
    {
        #region IRC Message contains

        private static bool _isHandleMessage(string message)
        {
            if ((message[0] == '@') || (message[0] == '!'))
            {
                return true;
            }
            return false;
        }

        #endregion

        private static void InitIrcCallBack()
        {
            #region CALLBACK

            #region USER MATHCH

            CoCServerMain.Conf.Irc.OnUserJoined += (o, e) =>
            {
                CoCServerMain.Conf.LogDump.Write(
                    string.Format(
                        Properties.Resources.ircUserJoined,
                        DateTime.Now.ToString(CoCServerMain.Conf.Opt.IRCLogTimeFormat.value),
                        e.User
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
                    (CoCServerMain._isHandleMessage(e.Message)) &&
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
                    (CoCServerMain._isHandleMessage(e.Message)) &&
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

            #region CHANNEL/SERVER
            CoCServerMain.Conf.Irc.OnUserJoined += (o, e) =>
            {
                CoCServerMain.Conf.Irc.SendMessage(
                    CoCServerMain.Conf.Opt.IRCChannel.value,
                    string.Format(
                        Properties.Resources.PrnRun,
                        DateTime.Now,
                        Properties.Resources.PrnRunIrcHelp
                    )
                );
            };
            CoCServerMain.Conf.Irc.OnConnect += (o, e) =>
            {
                CoCServerMain.Conf.Irc.JoinChannel(CoCServerMain.Conf.Opt.IRCChannel.value);
                CoCServerMain.Conf.ILog.LogInfo(
                    string.Format(
                        Properties.Resources.PrnConnected,
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
                stCore.stConsole.WriteHeader("IRC OnExceptionThrown");
                CoCServerMain.PrnError(e.Exception.Message);
            };

            #endregion

            #endregion
        }
    }
}
