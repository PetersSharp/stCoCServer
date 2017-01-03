using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stCore;
using System.Text.RegularExpressions;

#if !STCLIENTBUILD
using stSqlite;
#endif

#if STCLIENTBUILD
namespace stClient

#else

namespace stCoCAPI.Client
#endif
{
    public class ClanMemberClient : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("status", typeof(System.Int32))]
        public int status { get; set; }

        [TablePropertyMapAttribute("year", typeof(System.Int32))]
        public int year { get; set; }

        [TablePropertyMapAttribute("season", typeof(System.Int32))]
        public int season { get; set; }

        [TablePropertyMapAttribute("tag", typeof(System.String))]
        public string tag { get; set; }

        [TablePropertyMapAttribute("nik", typeof(System.String))]
        public string nik { get; set; }

        [TablePropertyMapAttribute("role", typeof(System.String))]
        public string role { get; set; }

        [TablePropertyMapAttribute("level", typeof(System.Int32))]
        public int level { get; set; }

        [TablePropertyMapAttribute("trophies", typeof(System.Int32))]
        public int trophies { get; set; }

        [TablePropertyMapAttribute("rank", typeof(System.Int32))]
        public int rank { get; set; }

        [TablePropertyMapAttribute("prank", typeof(System.Int32))]
        public int prank { get; set; }

        [TablePropertyMapAttribute("send", typeof(System.Int32))]
        public int send { get; set; }

        [TablePropertyMapAttribute("receive", typeof(System.Int32))]
        public int receive { get; set; }

        [TablePropertyMapAttribute("ratio", typeof(System.Double))]
        public double ratio { get; set; }

        [TablePropertyMapAttribute("note", typeof(System.String))]
        public string note { get; set; }

        [TablePropertyMapAttribute("dtin", "DATETIME")]
        public string dtin { get; set; }

        [TablePropertyMapAttribute("dtout", "DATETIME")]
        public string dtout { get; set; }

        [TablePropertyMapAttribute("leagueid", typeof(System.Int32))]
        public int leagueid { get; set; }

        [TablePropertyMapAttribute("league", typeof(System.String))]
        public string league { get; set; }

        [TablePropertyMapAttribute("ico", typeof(System.String))]
        public string ico { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable"/>
        /// </summary>
        public static string NormalizeJsonTable(string jString)
        {
            string[] TblPattern = new string[] {
                @"{.*""data"":",
                @"}]}"
            };
            string[] TblReplacement = new string[] {
                "",
                @"}]"
            };
            return ClientTableUtil.MapNormalizeJsonTable(jString, TblPattern, TblReplacement);
        }
    }

    public class ClanMemberAuthClient : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("status", typeof(System.Int32))]
        public int status { get; set; }

        [TablePropertyMapAttribute("year", typeof(System.Int32))]
        public int year { get; set; }

        [TablePropertyMapAttribute("season", typeof(System.Int32))]
        public int season { get; set; }

        [TablePropertyMapAttribute("tag", typeof(System.String))]
        public string tag { get; set; }

        [TablePropertyMapAttribute("nik", typeof(System.String))]
        public string nik { get; set; }

        [TablePropertyMapAttribute("alias", typeof(System.String))]
        public string alias { get; set; }

        [TablePropertyMapAttribute("leagueid", typeof(System.Int32))]
        public int leagueid { get; set; }

        [TablePropertyMapAttribute("passwd", typeof(System.String))]
        public string passwd { get; set; }
    }

    /*
     *      {
     *      "id":"MemberChangeDonationReceive",
     *      "name":"NIKNAME",
     *      "tag":"298JR880C",
     *      "vold":"3901",
     *      "vnew":"3925",
     *      "vres":"", // result (War)
     *      "vs":"", // members number (War)
     *      "vcalc":"+24"
     *      }]}
     */

    public class ClanNotifyClient : TablePropertyMapMethod
    {
        [TablePropertyMapAttribute("id", typeof(System.String))]
        public string id { get; set; }

        [TablePropertyMapAttribute("name", typeof(System.String))]
        public string name { get; set; }

        [TablePropertyMapAttribute("tag", typeof(System.String))]
        public string tag { get; set; }

        [TablePropertyMapAttribute("vold", typeof(System.String))]
        public string vold { get; set; }

        [TablePropertyMapAttribute("vnew", typeof(System.String))]
        public string vnew { get; set; }

        [TablePropertyMapAttribute("vres", typeof(System.String))]
        public string vres { get; set; }

        [TablePropertyMapAttribute("vs", typeof(System.String))]
        public string vs { get; set; }

        [TablePropertyMapAttribute("vcalc", typeof(System.String))]
        public string vcalc { get; set; }

        [TablePropertyMapAttribute("dtin", typeof(System.DateTime))]
        public DateTime dtin { get; set; }

        /// <summary>
        /// <see cref="stCoCAPI.CoCAPI.CoCDataUtils.MapNormalizeJsonTable"/>
        /// </summary>
        public static string NormalizeJsonTable(string jString)
        {
            string[] TblPattern = new string[] {
                @"{.*""data"":",
                @"}]}"
            };
            string[] TblReplacement = new string[] {
                "",
                @"}]"
            };
            return ClientTableUtil.MapNormalizeJsonTable(jString, TblPattern, TblReplacement);
        }
    }

    public class ClientTableUtil
    {
        public static string MapNormalizeJsonTable(string jString, string[] TblPattern, string[] TblReplacement)
        {
            for (int i = 0; i < TblPattern.Length; i++)
            {
                jString = Regex.Replace(jString, TblPattern[i], TblReplacement[i]);
            }
            return jString;
        }
    }
}

