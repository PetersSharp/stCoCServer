using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stCore;

#if !STCLIENTBUILD
using stCoCAPI;
using stSqlite;
#endif

#if STCLIENTBUILD
namespace stClient

#else

namespace stCoCServer.CoCAPI
#endif
{
    public class ServerSetup : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("IRCServer", "STRING")]
        public string IRCServer { get; set; }

        [TablePropertyMapAttribute("IRCPort", typeof(System.Int64))]
        public Int64 IRCPort { get; set; }

        [TablePropertyMapAttribute("IRCChannel", "STRING")]
        public string IRCChannel { get; set; }

        [TablePropertyMapAttribute("IRCLanguage", "STRING")]
        public string IRCLanguage { get; set; }

        [TablePropertyMapAttribute("NotifyUpdateTime", typeof(System.Int64))]
        public Int64 NotifyUpdateTime { get; set; }

        [TablePropertyMapAttribute("URLClan", "STRING")]
        public string URLClan { get; set; }

        [TablePropertyMapAttribute("URLNotify", "STRING")]
        public string URLNotify { get; set; }

        [TablePropertyMapAttribute("URLInformer", "STRING")]
        public string URLInformer { get; set; }

        [TablePropertyMapAttribute("URLIrcLog", "STRING")]
        public string URLIrcLog { get; set; }

        [TablePropertyMapAttribute("URLWiki", "STRING")]
        public string URLWiki { get; set; }

        [TablePropertyMapAttribute("ServerVersion", "STRING")]
        public string ServerVersion { get; set; }

        [TablePropertyMapAttribute("ServerMagic", "STRING")]
        public string ServerMagic { get; set; }
    }
}
