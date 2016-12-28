
using System;
using stCore;
using stNet;
using stNet.Syslog;
using System.Collections.Generic;

namespace stCoCServerConfig.CoCServerConfigData
{
    public class Configuration : IDisposable
    {
        public Configuration()
        {
        }
        ~Configuration()
        {
            this.Dispose();
        }

        public bool Disposed = false;

        private IrcClient _irc = null;
        public IrcClient Irc
        {
            get { return this._irc; }
            set { this._irc = value; }
        }
        private stCoCAPI.CoCAPI _cocapi = null;
        public stCoCAPI.CoCAPI Api
        {
            get { return this._cocapi; }
            set { this._cocapi = value; }
        }
        private stCore.IOFile _logfile = null;
        public stCore.IOFile LogDump
        {
            get { return this._logfile; }
            set { this._logfile = value; }
        }
        //private IrcCommand dynamic _irccmd = null;
        //public IrcCommand dynamic IrcCmd
        private dynamic _irccmd = null;
        public dynamic IrcCmd
        {
            get { return this._irccmd; }
            set { this._irccmd = value; }
        }
        private stNet.stWebServer _web = null;
        public stNet.stWebServer HttpSrv
        {
            get { return this._web; }
            set { this._web = value; }
        }
        private stNet.stWebServerUtil.HtmlTemplate _tmpl = null;
        public stNet.stWebServerUtil.HtmlTemplate HtmlTemplate
        {
            get { return this._tmpl; }
            set { this._tmpl = value; }
        }
        private stGeo.GeoFilter _gf = null;
        public stGeo.GeoFilter Geo
        {
            get { return this._gf; }
            set { this._gf = value; }
        }
        private stSysLogNG _syslog = null;
        public stSysLogNG SysLog
        {
            get { return this._syslog; }
            set { this._syslog = value; }
        }
        private IMessage _iLog = null;
        public IMessage ILog
        {
            get { return this._iLog; }
            set { this._iLog = value; }
        }
        public CoCServerConfigData.Option Opt;
        public DateTime StatTime;

        public void Dispose()
        {
            this.Disposed = true;

            if (this.Irc != null)
            {
                this.Irc.Disconnect(true);
                this.Irc.Dispose();
                this.Irc = null;
            }
            if (this.HttpSrv != null)
            {
                this.HttpSrv.Stop();
                this.HttpSrv.Dispose();
                this.HttpSrv = null;
            }
            if (this.Api != null)
            {
                this.Api.Stop();
                this.Api.Dispose();
                this.Api = null;
            }
            if (this.Geo != null)
            {
                this.Geo.Dispose();
                this.Geo = null;
            }
            if (this.LogDump != null)
            {
                this.LogDump.Close();
                this.LogDump = null;
            }
            if (this.SysLog != null)
            {
                this.SysLog.Close();
                this.SysLog = null;
            }
        }
    }

    public class Option
    {
        public Option()
        {
        }
        public object this[string pName]
        {
            get {
                var Obj = this.GetType().GetProperty(pName);
                if (Obj != null)
                {
                    return Obj.GetValue(this, null);
                }
                return null;
            }
            set {
                var Obj = this.GetType().GetProperty(pName);
                if (Obj != null)
                {
                    Obj.SetValue(this, value, null);
                }
            }
        }
        public OptionItem IRCPort { get; set; }
        public OptionItem IRCServer { get; set; }
        public OptionItem IRCPassword { get; set; }
        public OptionItem IRCAdminPassword { get; set; }
        public OptionItem IRCChannel { get; set; }
        public OptionItem IRCNik { get; set; }
        public OptionItem IRCFloodTimeOut { get; set; }
        public OptionItem IRCSOutDirName { get; set; }
        public OptionItem IRCSOutFileName { get; set; }
        public OptionItem IRCLogTimeFormat { get; set; }
        public OptionItem IRCServerMessage { get; set; }
        public OptionItem IRCNoticeMessage { get; set; }
        public OptionItem IRCKickRespawn { get; set; }

        public OptionItem IRCPluginLanguage { get; set; }
        public OptionItem IRCPluginSayEnable { get; set; }
        public OptionItem IRCPluginClanEnable { get; set; }
        public OptionItem IRCPluginHelpEnable { get; set; }
        public OptionItem IRCPluginModeEnable { get; set; }
        public OptionItem IRCPluginTimeEnable { get; set; }
        public OptionItem IRCPluginTopicEnable { get; set; }
        public OptionItem IRCPluginUpTimeEnable { get; set; }
        public OptionItem IRCPluginVersionEnable { get; set; }
        public OptionItem IRCPluginUrlShortEnable { get; set; }
        public OptionItem IRCPluginNotifySetupEnable { get; set; }
        public OptionItem IRCPluginLangSetupEnable { get; set; }
        public OptionItem IRCPluginContextUrlTitleEnable { get; set; }
        public OptionItem IRCPluginLoopClanNotifyEnable { get; set; }
        public OptionItem IRCPluginLoopClanNotifyPeriod { get; set; }

        public OptionItem SYSAppName { get; set; }
        public OptionItem SYSROOTPath { get; set; }
        public OptionItem SYSCONFPath { get; set; }
        public OptionItem SYSGEOPath { get; set; }
        public OptionItem SYSIRCLOGPath { get; set; }
        public OptionItem SYSTMPLPath { get; set; }
        public OptionItem SYSLANGConsole { get; set; }

        public OptionItem CLANTag { get; set; }
        public OptionItem CLANAPIKey { get; set; }

        public OptionItem SQLDBPath { get; set; }
        public OptionItem SQLDBUri { get; set; }
        public OptionItem SQLDBUpdateTime { get; set; }
        public OptionItem SQLDBFilterMemberTag { get; set; }

        public OptionItem WEBRootUri { get; set; }
        public OptionItem WEBRootPort { get; set; }
        public OptionItem WEBLANGDefault { get; set; }

        public OptionItem LOGRemoteServerAddress { get; set; }
        public OptionItem LOGRemoteServerPort { get; set; }
        public OptionItem LOGRemoteServerEnable { get; set; }
        public OptionItem LOGDuplicateEntry { get; set; }

        public OptionItem PrnQuiet { get; set; }
        public OptionItem IsRun { get; set; }

        public List<OptionItem> IPFLocation { get; set; }
        public List<OptionItem> IPFLocationEnable { get; set; }
        public List<OptionItem> IPFType { get; set; }
        public List<OptionItem> IPFIsIpBlackList { get; set; }
        public List<OptionItem> IPFIsGeoAsnBlackList { get; set; }
        public List<OptionItem> IPFIsGeoCountryBlackList { get; set; }
        public List<OptionItem> IPFIpList { get; set; }
        public List<OptionItem> IPFGeoListASN { get; set; }
        public List<OptionItem> IPFGeoListCountry { get; set; }
    }
    public class OptionItem
    {
        public OptionItem()
        {
        }
        public OptionItem(bool b, string t1, string t2)
        {
            this.bval = b;
            this.tag1 = t1;
            this.tag1 = t2;
        }
        public OptionItem(Int32 n, string t1, string t2)
        {
            this.num = n;
            this.tag1 = t1;
            this.tag1 = t2;
        }
        public OptionItem(string v, string t1, string t2)
        {
            this.value = v;
            this.tag1 = t1;
            this.tag1 = t2;
        }
        public bool bval = false;
        public Int32 num = 0;
        public string value = null;
        public string tag1 = null;
        public string tag2 = null;
        public System.Collections.Specialized.StringCollection collection = null;
    }
}
