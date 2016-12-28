using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCoCServer.plugins
{
    public class IrcPluginEntry
    {
        public Action<bool, string, string, string[], string> act = null;
        public bool enable = false;
    }
    public partial class IrcCommand
    {
        private Dictionary<string, IrcPluginEntry> _plugins =
            new Dictionary<string, IrcPluginEntry>();

        /// <summary>
        /// Action in Dictionary:
        ///     <code>(bool isPrivate, string channelIRC, string nikName, string [] cmdArray, string cmdString)</code>
        /// </summary>
        private void PluginsInit()
        {
            try
            {
                this._plugins.Add("?", new IrcPluginEntry() { act = this.PluginHelp, enable = this.Conf.Opt.IRCPluginHelpEnable.bval });
                this._plugins.Add("help", new IrcPluginEntry() { act = this.PluginHelp, enable = this.Conf.Opt.IRCPluginHelpEnable.bval });
                this._plugins.Add("say", new IrcPluginEntry() { act = this.PluginSay, enable = this.Conf.Opt.IRCPluginSayEnable.bval });
                this._plugins.Add("time", new IrcPluginEntry() { act = this.PluginTime, enable = this.Conf.Opt.IRCPluginTimeEnable.bval });
                this._plugins.Add("uptime", new IrcPluginEntry() { act = this.PluginUpTime, enable = this.Conf.Opt.IRCPluginUpTimeEnable.bval });
                this._plugins.Add("version", new IrcPluginEntry() { act = this.PluginVersion, enable = this.Conf.Opt.IRCPluginVersionEnable.bval });
                this._plugins.Add("urlshort", new IrcPluginEntry() { act = this.PluginUrlShort, enable = this.Conf.Opt.IRCPluginUrlShortEnable.bval });
                this._plugins.Add("ushort", new IrcPluginEntry() { act = this.PluginUrlShort, enable = this.Conf.Opt.IRCPluginUrlShortEnable.bval });
                this._plugins.Add("short", new IrcPluginEntry() { act = this.PluginUrlShort, enable = this.Conf.Opt.IRCPluginUrlShortEnable.bval });
                this._plugins.Add("topic", new IrcPluginEntry() { act = this.PluginTopic, enable = this.Conf.Opt.IRCPluginTopicEnable.bval });
                this._plugins.Add("mode", new IrcPluginEntry() { act = this.PluginMode, enable = this.Conf.Opt.IRCPluginModeEnable.bval });
                this._plugins.Add("notify", new IrcPluginEntry() { act = this.PluginNotifySetup, enable = this.Conf.Opt.IRCPluginNotifySetupEnable.bval });
                this._plugins.Add("lang", new IrcPluginEntry() { act = this.PluginLangSetup, enable = this.Conf.Opt.IRCPluginLangSetupEnable.bval });
                this._plugins.Add("clan", new IrcPluginEntry() { act = this.PluginClan, enable = this.Conf.Opt.IRCPluginClanEnable.bval });
            }
            catch (Exception) { }
        }
        public void PluginsPrint()
        {
            Dictionary<string, bool> pluginsFiltred = new Dictionary<string, bool>();

            foreach (KeyValuePair<string, IrcPluginEntry> dic in this._plugins)
            {
                IrcPluginEntry pe = (IrcPluginEntry)dic.Value;
                if (!pluginsFiltred.ContainsKey(pe.act.Method.Name))
                {
                    try
                    {
                        pluginsFiltred.Add(
                            pe.act.Method.Name,
                            pe.enable
                        );
                    }
                    catch (Exception) { }
                }
            }
            foreach (KeyValuePair<string, bool> dic in pluginsFiltred)
            {
                this.Conf.ILog.LogInfo(
                    string.Format(
                        "IRC {0}\t- {1}",
                        dic.Key,
                        ((dic.Value) ?
                            Properties.Resources.prnYes :
                            Properties.Resources.prnNo
                        )
                    )
                );
            }
            pluginsFiltred.Clear();
        }
    }
}
