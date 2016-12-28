using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using stNet;
using stNet.stWebServerUtil;
using stCoCServerConfig.CoCServerConfigData;
using System.Threading.Tasks;
using System.Globalization;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        private const string thisClass = "[IrcCMD]: ";
        private stCoCServerConfig.CoCServerConfigData.Configuration Conf = null;
        private string _httpUa = null;
        private CultureInfo _ci = null;

        ///<summary>Process command DB Clan request/response init</summary>
        ///<param name="conf">Configuration IRCBotData Class</param>
        public IrcCommand(stCoCServerConfig.CoCServerConfigData.Configuration conf)
        {
            if (conf == null)
            {
                throw new ArgumentException(thisClass + Properties.Resources.cnfEmpty);
            }
            this.Conf = conf;
            if (this._httpUa == null)
            {
                this._httpUa = HttpUtil.GetHttpUA(Properties.Settings.Default.setCoCHttpClient);
            }
            this._ci = stNet.stWebServerUtil.HttpUtil.GetHttpClientLanguage(this.Conf.Opt.IRCPluginLanguage.value, null);

            this.PluginsInit();
        }
        ///<summary>Process command DB Clan destruct part</summary>
        ~IrcCommand()
        {
            this._plugins.Clear();
        }

        ///<summary>Process parse context request/response</summary>
        ///<param name="isPrivate">bool IRC channel is private</param>
        ///<param name="channel">string IRC channel name</param>
        ///<param name="nik">string IRC nik name</param>
        ///<param name="cmds">string query string</param>
        ///<returns>bool, is contains key - true</returns>
        public void PluginParseContext(bool isPrivate, string channel, string nik, string cmds)
        {
            if (Conf.Opt.IRCPluginContextUrlTitleEnable.bval)
            {
                this.PluginContextUrlTitle(isPrivate, channel, nik, null, cmds);
            }
        }

        ///<summary>Process loop Notify, <see cref="stCoCAPI.CoCAPI"/> CoCAPI.NotifyEventGetData()</summary>
        ///<returns></returns>
        public void PluginLoopNotify()
        {
            if (Conf.Opt.IRCPluginLoopClanNotifyEnable.bval)
            {
                this.IRCPluginLoopClanNotify(false, Conf.Opt.IRCChannel.value);
            }
        }

        ///<summary>
        /// Process parse command request/response
        ///</summary>
        public bool PluginParseCmd(bool isPrivate, string channel, string nik, string cmds)
        {
            if ((this.Conf.Irc == null) || (!this.Conf.Irc.Connected))
            {
                this.Conf.ILog.LogError(Properties.Resources.cmdNotConnect);
                return false;
            }

            string[] cmd = cmds.Split(new Char[] { ' ' });

            if (cmd.Length == 0)
            {
                return false;
            }

            bool toPrivate = ((cmd[0][0] == '!') ? true : ((isPrivate) ? true : false));

            try
            {
                IrcPluginEntry entry =
                    this._plugins.FirstOrDefault(w => w.Key.Equals(cmd[0].Substring(1, (cmd[0].Length - 1)))).Value;

                if ((entry == null) || (entry.act == null))
                {
                    throw new ArgumentException();
                }
                if (entry.enable)
                {
                    entry.act(toPrivate, channel, nik, cmd, cmds);
                }
                else
                {
                    this._SendNotice(nik, Properties.Resources.cmdPluginDisabled);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void _SendNotice(string nik, string msg)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.Conf.Irc.SendNotice(nik, msg.ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Red));
                }
                catch (Exception e)
                {
                    if (this.Conf.ILog != null)
                    {
                        this.Conf.ILog.LogError(thisClass + e.Message);
                    }
                }
            });
        }
        private void _Send(bool isPrivate, string channel, string nik, string msg)
        {
            Task.Factory.StartNew(() =>
            {
                this._SendFromTask(isPrivate, channel, nik, msg);
            });
        }
        private void _SendFromTask(bool isPrivate, string channel, string nik, string msg)
        {
            try
            {
                switch (isPrivate)
                {
                    case true:  { this.Conf.Irc.SendNotice(nik, msg); break; }
                    case false: { this.Conf.Irc.SendMessage(channel, msg); break; }
                }
            }
            catch (Exception e)
            {
                if (this.Conf.ILog != null)
                {
                    this.Conf.ILog.LogError(thisClass + e.Message);
                }
            }
        }
        private void _SetUserMode(string channel, string msg)
        {
            this.Conf.Irc.SendMode(channel, msg);
        }
        private void _SendTopic(string channel, string msg)
        {
            this.Conf.Irc.SendTopic(channel, msg);
        }
        private bool _AuthorizeCmd(bool isPrivate, string channel, string nik, string [] cmd)
        {
            bool isAuth = true;
            if (!isPrivate)
            {
                this._Send(
                    true, channel, nik,
                    (Properties.Resources.cmdUserPrivateOnly).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Red)
                );
                isAuth = false;
            }
            if ((isAuth) && (cmd.Length < 3))
            {
                this._Send(
                    true, channel, nik,
                    (Properties.Resources.cmdMissingArgument).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Red)
                );
                isAuth = false;
            }
            if ((isAuth) && (!string.IsNullOrWhiteSpace(Conf.Opt.IRCAdminPassword.value)) && (!cmd[1].Equals(Conf.Opt.IRCAdminPassword.value)))
            {
                this._Send(
                    true, channel, nik,
                    (Properties.Resources.cmdUserNotAllowed).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Red)
                );
                isAuth = false;
            }
            if ((isAuth) && (string.IsNullOrWhiteSpace(cmd[2])))
            {
                this._Send(
                    true, channel, nik,
                    (Properties.Resources.cmdEmptyBody).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Red)
                );
                isAuth = false;
            }
            if (!isAuth)
            {
                this._Send(
                    true, channel, nik,
                    (Properties.Resources.fmtSetCommandHelp).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkGreen)
                );
            }
            return isAuth;
        }
    }
}
