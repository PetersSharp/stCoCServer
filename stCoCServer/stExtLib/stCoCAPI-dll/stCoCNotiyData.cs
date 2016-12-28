using stSqlite;
using System.Net;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        public class CoCNotifyField
        {
            /// <summary>
            /// <see cref="stCoCAPI.CoCEnum.CoCFmtReq"/>
            /// </summary>
            public CoCEnum.CoCFmtReq TableId;
            /// <summary>
            /// <see cref="stCoCAPI.CoCEnum.EventNotify"/>
            /// </summary>
            public CoCEnum.EventNotify EventId;
            public string NameField = null;
        }
        public class CoCNotifyHost
        {
            public HttpListenerResponse Response;
            public string IpAddress = null;
            public string Language = null;
        }
        public class CoCNotifyEntry : TablePropertyMapMethod
        {
            [TablePropertyMapAttribute("id", typeof(System.String), null)]
            public string id { get; set; }

            [TablePropertyMapAttribute("name", typeof(System.String), null)]
            public string name { get; set; }

            [TablePropertyMapAttribute("tag", typeof(System.String), null)]
            public string tag { get; set; }

            [TablePropertyMapAttribute("vold", typeof(System.String), null)]
            public string vold { get; set; }

            [TablePropertyMapAttribute("vnew", typeof(System.String), null)]
            public string vnew { get; set; }

            [TablePropertyMapAttribute("vres", typeof(System.String), null)]
            public string vres { get; set; }

            [TablePropertyMapAttribute("vs", typeof(System.String), null)]
            public string vs { get; set; }

            [TablePropertyMapAttribute("vcalc", typeof(System.String), null)]
            public string vcalc { get; set; }
        }
        public class CoCNotifyEventSetup : TablePropertyMapMethod
        {
            [TablePropertyMapAttribute("name", typeof(System.String), null)]
            public string name { get; set; }

            [TablePropertyMapAttribute("desc", typeof(System.String), null)]
            public string desc { get; set; }

            [TablePropertyMapAttribute("check", typeof(System.Boolean), null)]
            public bool check { get; set; }
        }
    }
}
