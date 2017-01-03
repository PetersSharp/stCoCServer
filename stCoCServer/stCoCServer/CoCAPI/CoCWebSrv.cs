#if DEBUG
// #define DEBUG_PrintWebRequest
// #define DEBUG_PrintJson
// #define DEBUG_PrintDataTable
// #define DEBUG_PrintImageInfo
#endif

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using stNet.stWebServerUtil;
using stSqlite;
using System.Data;

namespace stCoCServer.CoCAPI
{
    public static partial class CoCWebSrv
    {
        private const string fmtClassName = "[WebSrv {0} request]: ";

        #region File WebRequest

        public static void FileWebRequest(string url, object ctx, object udata)
        {
            stCoCServerConfig.CoCServerConfigData.Configuration conf = udata as stCoCServerConfig.CoCServerConfigData.Configuration;
            HttpListenerContext context = ctx as HttpListenerContext;

            if (
                (udata == null) ||
                (conf.HttpSrv == null)
               )
            {
                context.Response.Abort();
                return;
            }

            byte[] msg = null;
            long   msgsize = 0;
            string modify = String.Empty;
            string filePath = conf.Opt.SYSROOTPath.value;

            if (url.Contains("?"))
            {
                string [] urlPart = url.Split('?');
                if ((urlPart.Length == 0) || (string.IsNullOrWhiteSpace(urlPart[0])))
                {
                    CoCWebSrv._ErrorHtmlDefault(
                        conf,
                        HttpStatusCode.BadRequest,
                        String.Empty,
                        context
                    );
                    return;
                }
                url = urlPart[0];
            }

            foreach(string part in url.Split('/'))
            {
                if (part.Equals(".."))
                {
                    CoCWebSrv._ErrorHtmlDefault(
                        conf,
                        HttpStatusCode.BadRequest,
                        String.Empty,
                        context
                    );
                    return;
                }
                filePath = Path.Combine(filePath, part);
            }
            if (!File.Exists(filePath))
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.NotFound,
                    HttpStatusCode.NotFound.ToString() + @" path: '<b>" + url + @"</b>'",
                    context
                );
                return;
            }
            try
            {
                msg = File.ReadAllBytes(filePath);
                switch (context.Request.HttpMethod)
                {
                    case "HEAD":
                        {
                            FileInfo fi = new FileInfo(filePath);
                            modify = fi.LastWriteTimeUtc.ToLongDateString();
                            break;
                        }
                    case "GET":
                        {
                            msg = File.ReadAllBytes(filePath);
                            msgsize = msg.Length;
                            modify = File.GetLastWriteTimeUtc(filePath).ToLongDateString();
                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException(Properties.Resources.httpMethodNotSupport);
                        }
                }
            }
            catch (Exception e)
            {
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName,""),
                        e.GetType().Name,
                        e.Message
                    )
                );
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.InternalServerError,
                    e.Message,
                    context
                );
                return;
            }
            try
            {
                context.Response.AddHeader(conf.HttpSrv.httpContentType, HttpUtil.GetMimeType(url));
                context.Response.AddHeader(conf.HttpSrv.httpContentDisposition,
                            string.Format(
                               Properties.Settings.Default.setHttpContentDisposition,
                               Path.GetFileName(filePath)
                            )
                );
                context.Response.AddHeader(conf.HttpSrv.httpLastModified, modify);
                context.Response.ContentLength64 = msgsize;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "File"),
                        e.GetType().Name,
                        e.Message
                    )
                );
#else
            catch (Exception)
            {
#endif
                context.Response.Abort();
                return;
            }
            context.Response.Close();
        }
        
        #endregion

        #region Template WebRequest

        public static void TemplateWebRequest(string url, object ctx, object udata)
        {
            byte[] msg;
            string filePath, cacheid;
            stCoCServerConfig.CoCServerConfigData.Configuration conf = udata as stCoCServerConfig.CoCServerConfigData.Configuration;
            HttpListenerContext context = ctx as HttpListenerContext;

            if (
                (udata == null) ||
                (conf.HttpSrv == null)
               )
            {
                context.Response.Abort();
                return;
            }
            if (
                (conf.HtmlTemplate == null) ||
                (conf.LogDump == null)
               )
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.InternalServerError,
                    String.Empty,
                    context
                );
                return;
            }

            string [] urlPart = url.Split('/');

            if (
                (urlPart.Length < 5) ||
                (
                  (!urlPart[2].All(char.IsDigit)) ||
                  (!urlPart[3].All(char.IsDigit)) ||
                  (!urlPart[4].All(char.IsDigit))
                )
               )
            {
                DateTime cdate = DateTime.Now.AddDays(-1);
                urlPart = new string[] {
                    "",
                    "",
                    cdate.ToString("yyyy"),
                    cdate.ToString("MM"),
                    cdate.ToString("dd")
                };
            }
            try
            {
                cacheid = @"irc" + urlPart[4] + urlPart[3] + urlPart[2];

                if (
                    (!conf.Opt.WEBCacheEnable.bval) ||
                    (!stCore.stCache.GetCacheObject<byte[]>(cacheid, out msg))
                   )
                {
                    filePath = conf.LogDump.GetFilePath(urlPart[4], urlPart[3], urlPart[2]);

                    if (!File.Exists(filePath))
                    {
                        string title = string.Format(
                            Properties.Resources.httpLogNotFound,
                            urlPart[2],
                            urlPart[3],
                            urlPart[4]
                        );
                        msg = Encoding.UTF8.GetBytes(
                                conf.HtmlTemplate.Render(
                                    new
                                    {
                                        LANG = conf.Opt.SYSLANGConsole.value.ToLower(),
                                        TITLE = title,
                                        INSERTHTMLDATA = title,
                                        TOPMENU = string.Format(
                                            Properties.Resources.ircLogArchive,
                                            conf.Opt.IRCChannel.value
                                        ),
                                        DATE = DateTime.Now.ToString(),
                                        GENERATOR = conf.HttpSrv.wUserAgent,
                                        DATEY = urlPart[2],
                                        DATEM = urlPart[3],
                                        DATED = urlPart[4]
                                    },
                                    "IrcLogTemplate.html",
                                    filePath
                                )
                        );
                        context.Response.StatusCode = ((conf.Opt.WEBFrontEndEnable.bval) ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        msg = Encoding.UTF8.GetBytes(
                                conf.HtmlTemplate.Render(
                                    new
                                    {
                                        LANG = conf.Opt.SYSLANGConsole.value.ToLower(),
                                        TITLE = string.Format(
                                            Properties.Resources.httpDateLogTitle,
                                            urlPart[2],
                                            urlPart[3],
                                            urlPart[4]
                                        ),
                                        TOPMENU = string.Format(
                                            Properties.Resources.ircLogArchive,
                                            conf.Opt.IRCChannel.value
                                        ),
                                        DATE = DateTime.Now.ToString(),
                                        GENERATOR = conf.HttpSrv.wUserAgent,
                                        DATEY = urlPart[2],
                                        DATEM = urlPart[3],
                                        DATED = urlPart[4]
                                    },
                                    "IrcLogTemplate.html",
                                    filePath
                                )
                        );
                        if (conf.Opt.WEBCacheEnable.bval)
                        {
                            stCore.stCache.SetCacheObject<byte[]>(cacheid, msg, DateTime.Now.AddHours(6));
                        }
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
#if DEBUG_isCached
                    stConsole.WriteHeader("IRC LOG content is cached: " + cacheid + " : " + msg.Length.ToString());
#endif
                }
            }
            catch (Exception e)
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.InternalServerError,
                    e.Message,
                    context
                );
                return;
            }
            try
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "max-age=2629000, public");
                }
                else
                {
                    context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "no-cache");
                }
                context.Response.AddHeader(conf.HttpSrv.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeHtml));
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "Template"),
                        e.GetType().Name,
                        e.Message
                    )
                );
#else
            catch (Exception)
            {
#endif
                context.Response.Abort();
                return;
            }
            context.Response.Close();
        }

        #endregion

        #region Json GET WebRequest

        public static void JsonWebRequest(string url, object ctx, object udata)
        {
            byte[] msg ;
            stCoCServerConfig.CoCServerConfigData.Configuration conf = udata as stCoCServerConfig.CoCServerConfigData.Configuration;
            HttpListenerContext context = ctx as HttpListenerContext;

            if (
                (udata == null) ||
                (conf.HttpSrv == null)
               )
            {
                context.Response.Abort();
                return;
            }
            if (
                (conf.Api == null) ||
                (!conf.Api.DBCheck())
               )
            {
                conf.HttpSrv.BadRequestJson(HttpStatusCode.InternalServerError.ToString(), context, (int)HttpStatusCode.InternalServerError);
                return;
            }

            string [] urlPart = url.Split('/');
            urlPart = urlPart.Skip(1).Concat(urlPart.Take(1)).ToArray();
            Array.Resize(ref urlPart, urlPart.Length - 1);

            if ((urlPart.Length < 3) || (urlPart.Length > 7))
            {
                conf.HttpSrv.BadRequestJson(HttpStatusCode.BadRequest.ToString(), context, (int)HttpStatusCode.BadRequest);
                return;
            }
            try
            {
                stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None;
                string query = string.Empty;

                query = conf.Api.GetQueryString(urlPart, ref cReq, conf.Opt.SQLDBFilterMemberTag.collection, conf.ILog.LogError);
                switch (cReq)
                {
                    case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None:
                        {
                            throw new ArgumentNullException();
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.ServerSetup:
                        {
                            msg = CoCClientSetup.GetJsonSetup(conf);
                            break;
                        }
                    default:
                        {
                            if (string.IsNullOrWhiteSpace(query))
                            {
                                throw new ArgumentNullException();
                            }
                            try
                            {
#if DEBUG_PrintWebRequest
                                stConsole.WriteHeader("JsonWebRequest -> URL: (" + url + ") Query: (" + query + ")");
#endif
                                msg = Encoding.UTF8.GetBytes(
                                    conf.Api.QueryData(query).ToJson(true, true)
                                );
#if DEBUG_PrintJson
                                stConsole.WriteHeader("JsonWebRequest -> JSON: " + Encoding.UTF8.GetString(msg));
#endif
                            }
                            catch (Exception)
                            {
                                throw new ArgumentException(HttpStatusCode.BadRequest.ToString());
                            }
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                conf.HttpSrv.BadRequestJson(e.Message, context, (int)HttpStatusCode.InternalServerError);
                return;
            }
            try
            {
                context.Response.AddHeader(conf.HttpSrv.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeJson));
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "Json"),
                        e.GetType().Name,
                        e.Message
                    )
                );
#else
            catch (Exception)
            {
#endif
                context.Response.Abort();
                return;
            }
            context.Response.Close();
        }
        #endregion

        #region SSE/RSS/Json notify WebRequest

        public static void SseWebRequest(string url, object ctx, object udata)
        {
            stCoCServerConfig.CoCServerConfigData.Configuration conf = udata as stCoCServerConfig.CoCServerConfigData.Configuration;
            HttpListenerContext context = ctx as HttpListenerContext;

            if (
                (udata == null) ||
                (conf.HttpSrv == null)
               )
            {
                context.Response.Abort();
                return;
            }

            string[] urlPart = url.Split('/');

            if (urlPart.Length < 3)
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.BadRequest,
                    Properties.Resources.fmtNotifyUrlError,
                    context
                );
                return;
            }

            stCoCAPI.CoCAPI.CoCNotifyHost client = new stCoCAPI.CoCAPI.CoCNotifyHost()
            {
                Response = context.Response,
                IpAddress = ((IPAddress)stNet.stWebServerUtil.HttpUtil.GetHttpClientIP(context.Request)).ToString(),
                Language = stNet.stWebServerUtil.HttpUtil.GetHttpPreferedLanguage(context.Request.UserLanguages)
            };

            switch (urlPart[2])
            {
                case "sse":
                    {
                        if (
                            (conf.Api == null) ||
                            (!conf.Api.DBCheck())
                           )
                        {
                            if (conf.Api.NotifySendSseStreamComplette(
                                client,
                                stCoCAPI.CoCAPI.CoCEnum.EventNotify.ServerError.ToString(),
                                HttpStatusCode.InternalServerError.ToString()
                            ))
                            { context.Response.Close(); } else { context.Response.Abort(); }
                            return;
                        }
                        if (!context.Request.KeepAlive)
                        {
                            if (conf.Api.NotifySendSseStreamComplette(
                                client,
                                stCoCAPI.CoCAPI.CoCEnum.EventNotify.ServerError.ToString(),
                                stCoCAPI.CoCAPI.CoCEnum.EventNotify.NoKeepAlive.ToString() + " : " + DateTime.Now.ToString()
                            ))
                            { context.Response.Close(); } else { context.Response.Abort(); }
                            return;
                        }
                        conf.Api.NotifySseAdd(client);
                        break;
                    }
                case "json":
                    {
                        if (
                            (conf.Api == null) ||
                            (!conf.Api.DBCheck())
                           )
                        {
                            conf.HttpSrv.BadRequestJson(stCoCAPI.CoCAPI.CoCEnum.EventNotify.ServerError.ToString(), context, (int)HttpStatusCode.InternalServerError);
                            return;
                        }
                        bool isfull = true;
                        if (
                            (urlPart.Length > 3) &&
                            (urlPart[3].Equals("list"))
                           )
                        {
                            isfull = false;
                        }
                        if (conf.Api.NotifySendJsonComplette(client, isfull))
                        { context.Response.Close(); } else { context.Response.Abort(); }
                        break;
                    }
                case "xml":
                case "rss":
                    {
                        if (
                            (conf.Api == null) ||
                            (!conf.Api.DBCheck())
                           )
                        {
                            conf.HttpSrv.BadRequestXml(stCoCAPI.CoCAPI.CoCEnum.EventNotify.ServerError.ToString(), context, (int)HttpStatusCode.InternalServerError);
                            return;
                        }
                        if (conf.Api.NotifySendRssComplette(client))
                        { context.Response.Close(); } else { context.Response.Abort(); }
                        break;
                    }
                default:
                    {
                        CoCWebSrv._ErrorHtmlDefault(
                            conf,
                            HttpStatusCode.InternalServerError,
                            Properties.Resources.fmtNotifyUrlError,
                            context
                        );
                        break;
                    }
            }
        }

        #endregion

        #region Informer WebRequest

        public static void InformerWebRequest(string url, object ctx, object udata)
        {
            stCoCServerConfig.CoCServerConfigData.Configuration conf = udata as stCoCServerConfig.CoCServerConfigData.Configuration;
            HttpListenerContext context = ctx as HttpListenerContext;
            stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq reqtype = stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.None;

            if (
                (udata == null) ||
                (conf.HttpSrv == null)
               )
            {
                context.Response.Abort();
                return;
            }
            if (
                (conf.Api == null) ||
                (!conf.Api.DBCheck())
               )
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.InternalServerError,
                    String.Empty,
                    context
                );
                return;
            }

            Int32 idx = 0, resize = 2;
            byte[] msg = null;
            string[] urlPart = url.Split('/');
            urlPart = urlPart.Skip(1).Concat(urlPart.Take(1)).ToArray();
            Array.Resize(ref urlPart, urlPart.Length - 1);
            string memberId = "",
                   langid = stNet.stWebServerUtil.HttpUtil.GetHttpPreferedLanguage(context.Request.UserLanguages);

            try
            {
                if (urlPart.Length < 3)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (!Int32.TryParse(urlPart[2], out idx))
                {
                    throw new ArgumentOutOfRangeException();
                }
                switch (urlPart[1])
                {
                    case "info":
                    case "clan":
                        {
                            reqtype = stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Clan;
                            if (urlPart.Length != 4)
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            break;
                        }
                    case "player":
                    case "member":
                        {
                            reqtype = stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Members;
                            if (urlPart.Length != 5)
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            if (urlPart[3].Equals("random"))
                            {
                                urlPart[1] = urlPart[3];
                                resize = 3;
                            }
                            else
                            {
                                if ((urlPart[3].Length > 12) || (urlPart[3].Length < 6))
                                {
                                    throw new ArgumentOutOfRangeException();
                                }
                                urlPart[2] = urlPart[3];
                            }
                            memberId = urlPart[3];
                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                }
                Array.Resize(ref urlPart, urlPart.Length - resize);
            }
            catch (Exception)
            {
                conf.HttpSrv.BadRequestRaw(conf.Api.InformerImageError(reqtype, langid), context, (int)HttpStatusCode.BadRequest, HttpUtil.MimeType.MimePng);
                return;
            }
            try
            {
                stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None;
                string query = string.Empty;
                try
                {
                    query = conf.Api.GetQueryString(urlPart, ref cReq, conf.Opt.SQLDBFilterMemberTag.collection, conf.ILog.LogError);
                    if (
                        (cReq == stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None) ||
                        (string.IsNullOrWhiteSpace(query))
                       )
                    {
                        throw new ArgumentNullException();
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException();
                }

#if DEBUG_PrintWebRequest
                stConsole.WriteHeader("InformerWebRequest -> URL: (" + url + ") Query: (" + query + ")");
#endif
                DataTable dt = conf.Api.QueryData(query);
                if ((dt == null) || (dt.Rows.Count == 0))
                {
                    throw new ArgumentNullException();
                }
#if DEBUG_PrintJson
                stConsole.WriteHeader("InformerWebRequest -> JSON: " + dt.ToJson(false, false));
#endif
#if DEBUG_PrintDataTable
                dt.DataTableToPrint();
#endif
                msg = conf.Api.InformerImageGet(dt.Rows[0], reqtype, idx, memberId, langid, conf.Opt.WEBCacheEnable.bval);

#if DEBUG_PrintImageInfo
                stConsole.WriteHeader("InformerWebRequest -> ImageInfo: " + msg.Length);
#endif
            }
            catch (Exception)
            {
                conf.HttpSrv.BadRequestRaw(
                    conf.Api.InformerImageError(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Auth, langid),
                    context,
                    (int)HttpStatusCode.InternalServerError,
                    HttpUtil.MimeType.MimePng
                );
                return;
            }
            try
            {
#if DEBUG_NoCache
                context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "no-cache");
                context.Response.AddHeader(conf.HttpSrv.httpAccelBuffering, "no");
#else
                context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "max-age=" + conf.Api.UpdateNextSeconds.ToString() + ", public");
                context.Response.AddHeader(conf.HttpSrv.httpAccelBuffering, "yes");
                context.Response.AddHeader(conf.HttpSrv.httpLastModified, conf.Api.UpdateLastTime.ToString("R"));
#endif
                context.Response.AddHeader(conf.HttpSrv.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimePng));
                context.Response.AddHeader(conf.HttpSrv.httpAccessControlAllowOrigin, "*");
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "Informer"),
                        e.GetType().Name,
                        e.Message
                    )
                );
#else
            catch (Exception)
            {
#endif
                context.Response.Abort();
                return;
            }
            context.Response.Close();
        }

        #endregion

        #region Wiki WebRequest

        public static void WikiWebRequest(string url, object ctx, object udata)
        {
            byte[] msg;
            stCoCServerConfig.CoCServerConfigData.Configuration conf = udata as stCoCServerConfig.CoCServerConfigData.Configuration;
            HttpListenerContext context = ctx as HttpListenerContext;

            if (
                (udata == null) ||
                (conf.HttpSrv == null)
               )
            {
                context.Response.Abort();
                return;
            }
            if (conf.HtmlTemplate == null)
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.InternalServerError,
                    String.Empty,
                    context
                );
                return;
            }
            try
            {
                if ((msg = CoCWebSrv.WebWikiRouteTree(url, conf)) == null)
                {
                    throw new ArgumentException("Create Wiki page error");
                }
            }
            catch (Exception e)
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.InternalServerError,
                    e.Message,
                    context
                );
                return;
            }
            try
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "max-age=2629000, public");
                }
                else
                {
                    context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "no-cache");
                }
                context.Response.AddHeader(conf.HttpSrv.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeHtml));
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "Wiki"),
                        e.GetType().Name,
                        e.Message
                    )
                );
#else
            catch (Exception)
            {
#endif
                context.Response.Abort();
                return;
            }

            context.Response.Close();
            return;

            /*
            ///////
            //////// Go this!
            //////
            string[] urlPart = url.Split('/');

            if (
                (urlPart.Length < 5) ||
                (
                  (!urlPart[2].All(char.IsDigit)) ||
                  (!urlPart[3].All(char.IsDigit)) ||
                  (!urlPart[4].All(char.IsDigit))
                )
               )
            {
                DateTime cdate = DateTime.Now.AddDays(-1);
                urlPart = new string[] {
                    "",
                    "",
                    cdate.ToString("yyyy"),
                    cdate.ToString("MM"),
                    cdate.ToString("dd")
                };
            }
            try
            {
                cacheid = @"irc" + urlPart[4] + urlPart[3] + urlPart[2];

                if (
                    (!conf.Opt.WEBCacheEnable.bval) ||
                    (!stCore.stCache.GetCacheObject<byte[]>(cacheid, out msg))
                   )
                {
                    filePath = conf.LogDump.GetFilePath(urlPart[4], urlPart[3], urlPart[2]);

                    if (!File.Exists(filePath))
                    {
                        string title = string.Format(
                            Properties.Resources.httpLogNotFound,
                            urlPart[2],
                            urlPart[3],
                            urlPart[4]
                        );
                        msg = Encoding.UTF8.GetBytes(
                                conf.HtmlTemplate.Render(
                                    new
                                    {
                                        LANG = conf.Opt.SYSLANGConsole.value.ToLower(),
                                        TITLE = title,
                                        INSERTHTMLDATA = title,
                                        TOPMENU = string.Format(
                                            Properties.Resources.ircLogArchive,
                                            conf.Opt.IRCChannel.value
                                        ),
                                        DATE = DateTime.Now.ToString(),
                                        GENERATOR = conf.HttpSrv.wUserAgent,
                                        DATEY = urlPart[2],
                                        DATEM = urlPart[3],
                                        DATED = urlPart[4]
                                    },
                                    "IrcLogTemplate.html",
                                    filePath
                                )
                        );
                        context.Response.StatusCode = ((conf.Opt.WEBFrontEndEnable.bval) ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound);
                    }
                    else
                    {
                        msg = Encoding.UTF8.GetBytes(
                                conf.HtmlTemplate.Render(
                                    new
                                    {
                                        LANG = conf.Opt.SYSLANGConsole.value.ToLower(),
                                        TITLE = string.Format(
                                            Properties.Resources.httpDateLogTitle,
                                            urlPart[2],
                                            urlPart[3],
                                            urlPart[4]
                                        ),
                                        TOPMENU = string.Format(
                                            Properties.Resources.ircLogArchive,
                                            conf.Opt.IRCChannel.value
                                        ),
                                        DATE = DateTime.Now.ToString(),
                                        GENERATOR = conf.HttpSrv.wUserAgent,
                                        DATEY = urlPart[2],
                                        DATEM = urlPart[3],
                                        DATED = urlPart[4]
                                    },
                                    "IrcLogTemplate.html",
                                    filePath
                                )
                        );
                        if (conf.Opt.WEBCacheEnable.bval)
                        {
                            stCore.stCache.SetCacheObject<byte[]>(cacheid, msg, DateTime.Now.AddHours(6));
                        }
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
#if DEBUG_isCached
                    stConsole.WriteHeader("IRC LOG content is cached: " + cacheid + " : " + msg.Length.ToString());
#endif
                }
            }
            catch (Exception e)
            {
                CoCWebSrv._ErrorHtmlDefault(
                    conf,
                    HttpStatusCode.InternalServerError,
                    e.Message,
                    context
                );
                return;
            }
            try
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                {
                    context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "max-age=2629000, public");
                }
                else
                {
                    context.Response.AddHeader(conf.HttpSrv.httpCacheControl, "no-cache");
                }
                context.Response.AddHeader(conf.HttpSrv.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeHtml));
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError("[Template Web Request]: " + e.GetType().Name + " -> " + e.Message);
#else
            catch (Exception)
            {
#endif
                context.Response.Abort();
                return;
            }
            context.Response.Close();
             */
        }

        #endregion

        #region Error user format response

        private static void _ErrorHtmlDefault(
            stCoCServerConfig.CoCServerConfigData.Configuration conf,
            HttpStatusCode code,
            string reason,
            HttpListenerContext context
            )
        {
            conf.HttpSrv.BadRequestRaw(
                Encoding.UTF8.GetBytes(
                    string.Format(
                        Properties.Resources.httpHtmlError,
                        conf.HttpSrv.wUserAgent,
                        (int)code,
                        code.ToString(),
                        ((string.IsNullOrWhiteSpace(reason)) ? "" : reason + @".<br/>"),
                        DateTime.Now.ToString(),
                        @"/assets/html/ClanInfo.html"
                    )
                ),
                context,
                (int)code,
                HttpUtil.MimeType.MimeHtml
            );
        }

        #endregion

    }
}
