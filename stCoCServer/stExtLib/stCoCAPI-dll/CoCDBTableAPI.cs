using stCore;
using stSqlite;
using System;

namespace stCoCAPI
{
    public class ClanMemberAuth : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("tag", typeof(System.String), true, false, true, "MapFilterTag")]
        public string tag { get; set; }

        [TablePropertyMapAttribute("passwd", "STRING")]
        public string passwd { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag"/>
        /// </summary>
        public static string MapFilterTag(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag(src);
        }
    }
    public class ClanMember : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("idx", "INTEGER", true, true)]
        public int idx { get; set; }

        [TablePropertyMapAttribute("status", "INTEGER")]
        public int status { get; set; }

        [TablePropertyMapAttribute("syear", "INTEGER")]
        public int year { get; set; }

        [TablePropertyMapAttribute("smonth", "INTEGER")]
        public int season { get; set; }

        [TablePropertyMapAttribute("tag", typeof(System.String), "MapFilterTag")]
        public string tag { get; set; }

        [TablePropertyMapAttribute("name")]
        public string nik { get; set; }

        [TablePropertyMapAttribute("role")]
        public string role { get; set; }

        [TablePropertyMapAttribute("expLevel", typeof(System.Int32))]
        public int level { get; set; }

        [TablePropertyMapAttribute("leagueid", typeof(System.Int32))]
        public string league { get; set; }

        [TablePropertyMapAttribute("trophies", typeof(System.Int32))]
        public int trophies { get; set; }

        [TablePropertyMapAttribute("clanRank", typeof(System.Int32))]
        public int rank { get; set; }

        [TablePropertyMapAttribute("previousClanRank", typeof(System.Int32))]
        public int prank { get; set; }

        [TablePropertyMapAttribute("donations", typeof(System.Int32))]
        public int send { get; set; }

        [TablePropertyMapAttribute("donationsReceived", typeof(System.Int32))]
        public int receive { get; set; }

        [TablePropertyMapAttribute("offset", typeof(System.Double))]
        public double ratio { get; set; }

        [TablePropertyMapAttribute("note", "STRING")]
        public string note { get; set; }

        [TablePropertyMapAttribute("warstatus", "STRING")]
        public string warstatus { get; set; }

        [TablePropertyMapAttribute("dtin", "DATETIME")]
        public string dtin { get; set; }

        [TablePropertyMapAttribute("dtout", "DATETIME")]
        public string dtout { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable"/>
        /// </summary>
        public static string NormalizeJsonTable(string jString)
        {
            string[] TblPattern = new string[] {
                @"{""items"":",
                @",""paging"":{""cursors"":{}}}",
                @"""league"":{""id"":(\d+),""name"":""([^""]*)"",""iconUrls"":{",
                @"png""}}"
            };
            string[] TblReplacement = new string[] {
                "",
                "",
                @"""leagueid"":$1,""leaguename"":""$2"",",
                @"png"""
            };
            return stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable(jString, TblPattern, TblReplacement);
        }
        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag"/>
        /// </summary>
        public static string MapFilterTag(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag(src);
        }
    }
    public class ClanInfo : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("tag", typeof(System.String), true, false, true, "MapFilterTag")]
        public string tag { get; set; }

        [TablePropertyMapAttribute("name", typeof(System.String))]
        public string name { get; set; }

        [TablePropertyMapAttribute("type", typeof(System.String))]
        public string type { get; set; }

        [TablePropertyMapAttribute("description", typeof(System.String))]
        public string desc { get; set; }

        [TablePropertyMapAttribute("locid", "INTEGER")]
        public int locid { get; set; }

        [TablePropertyMapAttribute("locname", typeof(System.String))]
        public string locname { get; set; }

        [TablePropertyMapAttribute("locctry", typeof(System.String))]
        public string locctry { get; set; }

        [TablePropertyMapAttribute("clanLevel")]
        public int level { get; set; }

        [TablePropertyMapAttribute("clanPoints")]
        public int points { get; set; }

        [TablePropertyMapAttribute("requiredTrophies")]
        public int trophies { get; set; }

        [TablePropertyMapAttribute("warFrequency")]
        public string warfreq { get; set; }

        [TablePropertyMapAttribute("warWinStreak")]
        public int warstr { get; set; }

        [TablePropertyMapAttribute("warWins")]
        public int warwin { get; set; }

        [TablePropertyMapAttribute("isWarLogPublic")]
        public bool warpub { get; set; }

        [TablePropertyMapAttribute("members")]
        public int members { get; set; }

        [TablePropertyMapAttribute("small", typeof(System.String), "MapFilterImageIco")]
        public string ico { get; set; }

        [TablePropertyMapAttribute("dtup", "DATETIME")]
        public string dtup { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterImageIco"/>
        /// </summary>
        public static string MapFilterImageIco(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterImageIco(src);
        }
        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable"/>
        /// </summary>
        public static string NormalizeJsonTable(string jString)
        {
            string[] TblPattern = new string[] {
                @",""memberList"":.+",
                @"""badgeUrls"":{",
                @".png""}",
                @"""location"":{""id"":(\d+),""name"":""([^""]*)"",""isCountry"":.+,""countryCode"":""(\w+)""}"
            };
            string[] TblReplacement = new string[] {
                @"}",
                @"",
                @".png""",
                @"""locid"":$1,""locname"":""$2"",""locctry"":""$3"""
            };
            return stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable(jString, TblPattern, TblReplacement);
        }
        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag"/>
        /// </summary>
        public static string MapFilterTag(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag(src);
        }
    }
    public class WarLog : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("endTime", typeof(System.String), "DATETIME", true, false, true, "MapFilterDateTime")]
        public string dtend { get; set; }

        [TablePropertyMapAttribute("result", typeof(System.String))]
        public string result { get; set; }

        [TablePropertyMapAttribute("teamSize", typeof(System.Int32))]
        public int members { get; set; }

        [TablePropertyMapAttribute("ctag", typeof(System.String), "MapFilterTag")]
        public string ctag { get; set; }

        [TablePropertyMapAttribute("clanLevel", typeof(System.Int32))]
        public int clevel { get; set; }

        [TablePropertyMapAttribute("attacks", typeof(System.Int32))]
        public int cattacks { get; set; }

        [TablePropertyMapAttribute("stars", typeof(System.Int32))]
        public int cstars { get; set; }

        [TablePropertyMapAttribute("expEarned", typeof(System.Int32))]
        public int cexp { get; set; }

        [TablePropertyMapAttribute("destructionPercentage", typeof(System.Double))]
        public double cdestruct { get; set; }

        [TablePropertyMapAttribute("optag", typeof(System.String), "MapFilterTag")]
        public string optag { get; set; }

        [TablePropertyMapAttribute("opname", typeof(System.String))]
        public string opname { get; set; }

        [TablePropertyMapAttribute("opico", typeof(System.String), "MapFilterImageIco")]
        public string ico { get; set; }

        [TablePropertyMapAttribute("oplevel", typeof(System.Int32))]
        public int oplevel { get; set; }

        [TablePropertyMapAttribute("opattacks", typeof(System.Int32))]
        public int opattacks { get; set; }

        [TablePropertyMapAttribute("opdestruct", typeof(System.Double))]
        public double opdestruct { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterDateTime"/>
        /// </summary>
        public static string MapFilterDateTime(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterDateTime(src);
        }
        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterImageIco"/>
        /// </summary>
        public static string MapFilterImageIco(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterImageIco(src);
        }
        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable"/>
        /// </summary>
        public static string NormalizeJsonTable(string jString)
        {
            string[] TblPattern = new string[] {
                @"{""items"":",
                @",""paging"":{""cursors"":{}}}",
                @"""clan"":{""tag"":""([A-Z0-9#]+)"",.+?}",
                @"},""opponent"":{""tag"":""([A-Z0-9#]+)"",""name"":""([^""]*)"",""badgeUrls"":{""small"":""(.+?)"",""large"":"".+?"",""medium"":"".+?""},""clanLevel"":(\d+),""stars"":(\d+),""destructionPercentage"":([0-9.]+)}}"
            };
            string[] TblReplacement = new string[] {
                @"",
                @"",
                @"""ctag"":""$1""",
                @",""optag"":""$1"",""opname"":""$2"",""opico"":""$3"",""oplevel"":$4,""opattacks"":$5,""opdestruct"":$6}"
            };
            return stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable(jString, TblPattern, TblReplacement);
        }
        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag"/>
        /// </summary>
        public static string MapFilterTag(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterTag(src);
        }
    }
    public class AllLeague : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("id", typeof(System.Int32), true, false, true)]
        public int id { get; set; }

        [TablePropertyMapAttribute("name", typeof(System.String))]
        public string name { get; set; }

        [TablePropertyMapAttribute("small", typeof(System.String), "MapFilterImageIco")]
        public string ico { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapFilterImageIco"/>
        /// </summary>
        public static string MapFilterImageIco(string src)
        {
            return stCoCAPI.CoCAPI.CoCDataUtils.MapFilterImageIco(src);
        }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable"/>
        /// </summary>
        public static string NormalizeJsonTable(string jString)
        {
            string[] TblPattern = new string[] {
                @"{""items"":",
                @",""paging"":{""cursors"":{}}}",
                @"""iconUrls"":{",
                @".png""}"
            };
            string[] TblReplacement = new string[] {
                "",
                "",
                "",
                @".png"""
            };
            return stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable(jString, TblPattern, TblReplacement);
        }
    }
    public class AllLocations : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("id", typeof(System.Int32), true, false, true)]
        public int id { get; set; }

        [TablePropertyMapAttribute("name", typeof(System.String))]
        public string name { get; set; }

        [TablePropertyMapAttribute("isCountry", typeof(System.Boolean))]
        public bool isreal { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable"/>
        /// </summary>
        public static string NormalizeJsonTable(string jString)
        {
            string[] TblPattern = new string[] {
                @"{""items"":",
                @",""paging"":{""cursors"":{}}}"
            };
            string[] TblReplacement = new string[] {
                "",
                ""
            };
            return stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable(jString, TblPattern, TblReplacement);
        }
    }
    /// <summary>
    /// Create SQL view public request
    /// </summary>
    public static class SQLViewRequest
    {
        const string ViewClanInfo =
            "CREATE VIEW clan AS SELECT " +
            "tag, " +
            "ClanInfo.name AS name, " +
            "type, " +
            "desc, " +
            "level, " +
            "points, " +
            "trophies, " +
            "warfreq, " +
            "warstr, " +
            "warwin, " +
            "warpub, " +
            "members, " +
            "dtup, " +
            "locctry, " +
            "AllLocations.name AS locname, " +
            "AllLocations.isreal AS locreal, " +
            "COCBADGEICO(ClanInfo.ico) as ico, " +
            "COCFLAGICO(ClanInfo.locctry,AllLocations.isreal) as flag " +
            "FROM ClanInfo " +
            "INNER JOIN AllLocations ON AllLocations.id = ClanInfo.locid;";
        const string ViewClanMember =
            "CREATE VIEW clanmembers AS SELECT " +
            "status, " +
            "year, " +
            "season, " +
            "tag, " +
            "nik, " +
            "role, " +
            "level, " +
            "trophies, " +
            "rank, " +
            "prank, " +
            "send, " +
            "receive, " +
            "ratio, " +
            "note, " +
            "dtin, " +
            "dtout, " +
            "AllLeague.name AS league, " +
            "COCLEAGUEICO(ico) as ico " +
            "FROM ClanMember " +
            "INNER JOIN AllLeague ON AllLeague.id = ClanMember.league;";
        const string ViewWarLog =
            "CREATE VIEW clanwar AS SELECT " +
            "dtend, " +
            "result, " +
            "WarLog.members AS members, " +
            "ctag, " +
            "ClanInfo.name AS cname, " +
            "ClanInfo.members AS cmembers, " +
            "COCBADGEICO(ClanInfo.ico) as cico, " +
            "clevel, " +
            "cattacks, " +
            "cstars, " +
            "cdestruct, " +
            "cexp, " +
            "optag, " +
            "opname, " +
            "oplevel, " +
            "opattacks, " +
            "opdestruct, " +
            "COCBADGEICO(WarLog.ico) as opico " +
            "FROM WarLog " +
            "INNER JOIN ClanInfo ON ClanInfo.tag = WarLog.ctag;";
        const string ViewClanMemberAuth =
            "CREATE VIEW clanmembersauth AS SELECT " +
            "ClanMember.season AS season, " +
            "ClanMember.nik AS nik, " +
            "ClanMember.tag AS tag, " +
            "ClanMemberAuth.passwd AS passwd " +
            "FROM ClanMember " +
            "INNER JOIN ClanMemberAuth ON ClanMemberAuth.tag = ClanMember.tag;";

        public static readonly string[] AllView = new string[] {
            SQLViewRequest.ViewWarLog,
            SQLViewRequest.ViewClanInfo,
            SQLViewRequest.ViewClanMember,
            SQLViewRequest.ViewClanMemberAuth
        };

    }
}
