#if DEBUG
// #define DEBUG_CHECKCURL
#endif

using System;
using System.Collections.Generic;
using System.Data;
using stCore;
using stSqlite;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        public static class CoCRequest
        {
            private const string cocBaseUrl = @"https://api.clashofclans.com/v1";
            private static Dictionary<stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq, string> cocTemplateUrl = new Dictionary<stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq, string>()
            {
                {stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.None,      @"{0}{1}" },
                {stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Clan,      @"{0}/clans/{1}" },
                {stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Warlog,    @"{0}/clans/{1}/warlog" },
                {stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Members,   @"{0}/clans/{1}/members" },
                {stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Leagues,     @"{0}/leagues{1}" },
                {stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Locations, @"{0}/locations{1}" },
                {stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.ClanRank,  @"{0}/locations/{1}/rankings/clans" }
            };

            private static string MakeCoCUrl(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string clanTag)
            {
                bool[] opUrl;
                switch (type)
                {
                    case stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.None: { return String.Empty; }
                    case stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Clan:
                    case stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Members:
                    case stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Warlog: { opUrl = new bool[] { true, true }; break; }
                    case stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.ClanRank: { opUrl = new bool[] { true, false }; break; }
                    case stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Leagues:
                    case stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Locations: { opUrl = new bool[] { false, false }; break; }
                    default: { opUrl = new bool[] { false, true }; break; }
                }
                return string.Format(
                    cocTemplateUrl[type],
                    cocBaseUrl,
                    ((opUrl[0]) ? ((opUrl[1]) ? CoCDataUtils.TagToUrl(clanTag) : clanTag) : "")
                );
            }
            private static bool _CheckArgs(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string key, string clanTag, stCore.IMessage iLog = null)
            {
                if (type == stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.None)
                {
                    stCore.LogException.Error(Properties.Resources.CoCRequestIdEmpty, iLog);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(clanTag))
                {
                    stCore.LogException.Error(Properties.Resources.CoCClanIdEmpty, iLog);
                    return false;
                }
                if (string.IsNullOrWhiteSpace(key))
                {
                    stCore.LogException.Error(Properties.Resources.CoCKeyEmpty, iLog);
                    return false;
                }
                return true;
            }
            private static bool _CheckReturnString(string jsonOut, stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, stCore.IMessage iLog = null)
            {
                if (string.IsNullOrWhiteSpace(jsonOut))
                {
                    string serverError = string.Format(
                        Properties.Resources.CoCGetUrlEmpty,
                        type.ToString()
                    );
                    stCore.LogException.Error(serverError, iLog);
                    return false;
                }
#if DEBUG_CHECKCURL
                stConsole.WriteHeader("Json Out (CheckReturnString): " + jsonOut);
#endif
                if (jsonOut.Contains("\"reason\":"))
                {
                    string serverError = String.Empty;
                    DataTable dt1 = jsonOut.JsonToDataTable();
#if DEBUG_CHECKCURL
                    dt1.DataTableToPrint();
#endif
                    if ((dt1 != null) && (dt1.Rows.Count > 0))
                    {
                        serverError = string.Format(
                            Properties.Resources.CoCGetUrlServerError,
                            ((dt1.Rows[0].Table.Columns.Contains("reason")) ?
                                stConsole.SplitCapitalizeString((string)dt1.Rows[0]["reason"]) :
                                Properties.Resources.CurlReturnReason
                            ),
                            ((dt1.Rows[0].Table.Columns.Contains("message")) ?
                                dt1.Rows[0]["message"] :
                                Properties.Resources.CurlReturnReason
                            )
                        );
                    }
                    else
                    {
                        serverError = string.Format(
                            Properties.Resources.CoCGetUrlEmpty,
                            type.ToString()
                        );
                    }
                    throw new CoCDBExceptionReason(serverError);
                }
                return true;
            }
            /// <summary>
            /// Initialize cURL instance
            /// </summary>
            /// <param name="key">Bearer auth key</param>
            /// <param name="iLog">log instance, <see cref="stCore.IMessage"/></param>
            /// <returns></returns>
            public static dynamic InitCUrlObj(string key, stCore.IMessage iLog = null)
            {
                return CoCRequest._InitCUrlObj(null, key, null, iLog);
            }
            /// <summary>
            /// Initialize cURL instance
            /// </summary>
            /// <param name="curlexe">name and path of curl binary, is null - auto detect</param>
            /// <param name="key">Bearer auth key</param>
            /// <param name="iLog">log instance, <see cref="stCore.IMessage"/></param>
            /// <returns></returns>
            public static dynamic InitCUrlObj(string curlexe, string key, stCore.IMessage iLog = null)
            {
                return CoCRequest._InitCUrlObj(curlexe, key, null, iLog);
            }
            /// <summary>
            /// Initialize cURL instance
            /// </summary>
            /// <param name="curlexe">name and path of curl binary, is null - auto detect</param>
            /// <param name="key">Bearer auth key</param>
            /// <param name="iLog">log instance, <see cref="stCore.IMessage"/></param>
            /// <returns></returns>
            public static dynamic InitCUrlObj(string curlexe, string key, string rootpath, stCore.IMessage iLog = null)
            {
                return CoCRequest._InitCUrlObj(curlexe, key, rootpath, iLog);
            }
            private static dynamic _InitCUrlObj(string curlexe, string key, string rootpath, stCore.IMessage iLog)
            {
                dynamic ccl = null;

                try
                {
                    // curl is use on native Win32/64, libcurl use in Mono Linux/OSX
                    // for use libcurl for WIN32, needed compile static libcurl library with OpenSSL,
                    // and remove flag LIBCURLSELECTOR and/or LIBCURLBINARY
                    //
                    // Curl help: Legacy Windows and SSL
                    // WinSSL (specifically SChannel from Windows SSPI), is the native SSL library
                    // in Windows. However, WinSSL in Windows <= XP is unable to connect to servers
                    // that no longer support the legacy handshakes and algorithms used by those
                    // versions. If you will be using curl in one of those earlier versions of
                    // Windows you should choose another SSL backend such as OpenSSL.
#if LIBCURLSELECTOR
                    //HACK: Compile stCoCAPI is use libcurl selector (curl/libcurl), Mono compatible
                    if (stRuntime.isRunTime())
                    {
                        ccl = new stNet.stCurlClientShare(key, stNet.stCurlClientSet.jsonHeaderArgs);
                    }
                    else
                    {
                        ccl = new stNet.stCurlClientBinary(curlexe);
                        ccl.RootPath = rootpath;
                        ccl.UTF8Out = true;
                    }
#elif LIBCURLBINARY
                //HACK: Compile stCoCAPI is use curl binary
                ccl = new stNet.stCurlClientBiary(curlexe);
#else
                //HACK: Compile stCoCAPI is use libcurl shared library, Mono compatible
                ccl = new stNet.stCurlClientShare(key, stNet.stCurlClientSet.jsonHeaderArgs);
#endif
                    if (ccl == null)
                    {
                        throw new ArgumentNullException(Properties.Resources.CurlObjEmpty);
                    }
                    return ccl;
                }
                catch (Exception e)
                {
                    stCore.LogException.Error(e, iLog);
                    return null;
                }
            }
            public static string GetCoCUrlMulti(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string key, string clanTag, dynamic curlobj, stCore.IMessage iLog = null)
            {
                if (curlobj == null)
                {
                    stCore.LogException.Error(Properties.Resources.CurlObjEmpty, iLog);
                    return String.Empty;
                }
                if (!CoCRequest._CheckArgs(type, key, clanTag, iLog))
                {
                    return String.Empty;
                }
                string reqUrl = CoCRequest.MakeCoCUrl(type, clanTag);
                if (string.IsNullOrWhiteSpace(reqUrl))
                {
                    stCore.LogException.Error(Properties.Resources.CoCUrlEmpty, iLog);
                    return String.Empty;
                }
#if DEBUG_CHECKCURL
                stConsole.WriteHeader("Get URL: " + reqUrl);
#endif
                try
                {
                    string jsonOut = curlobj.GetJson(reqUrl, ((stRuntime.isRunTime()) ? null : key));
                    if (!CoCRequest._CheckReturnString(jsonOut, type, iLog))
                    {
                        return String.Empty;
                    }
                    return jsonOut;
                }
                catch (Exception e)
                {
                    stCore.LogException.Error(e, iLog);
                    return String.Empty;
                }
            }
            public static string GetCoCUrlSingle(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string key, string clanTag, string curlexe, string rootpath, stCore.IMessage iLog = null)
            {
                if (!CoCRequest._CheckArgs(type, key, clanTag, iLog))
                {
                    return String.Empty;
                }
                string reqUrl = CoCRequest.MakeCoCUrl(type, clanTag);
                if (string.IsNullOrWhiteSpace(reqUrl))
                {
                    stCore.LogException.Error(Properties.Resources.CoCUrlEmpty, iLog);
                    return String.Empty;
                }
#if DEBUG_CHECKCURL
                stConsole.WriteHeader("Get URL: " + reqUrl);
#endif

                dynamic ccl = null;

                try
                {
                    ccl = CoCRequest._InitCUrlObj(curlexe, key, rootpath, iLog);

                    string jsonOut = ccl.GetJson(reqUrl, ((stRuntime.isRunTime()) ? null : key));
                    if (!CoCRequest._CheckReturnString(jsonOut, type, iLog))
                    {
                        return String.Empty;
                    }
                    return jsonOut;
                }
                catch (Exception e)
                {
                    stCore.LogException.Error(e, iLog);
                    return String.Empty;
                }
                finally
                {
                    if (ccl != null)
                    {
                        ccl.Dispose();
                    }
                }
            }
        }
    }
}
