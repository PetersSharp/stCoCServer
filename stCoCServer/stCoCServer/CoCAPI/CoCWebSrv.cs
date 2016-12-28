using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using stNet.stWebServerUtil;
using stSqlite;
using stCoCAPI;
using stCore;
using System.Collections.Generic;
using System.Threading;

namespace stCoCServer.CoCAPI
{
    public static class CoCWebSrv
    {
        
        #region File WebRquest

        public static void FileWebRquest(string url, object ctx, object udata)
        {
            stCoCServerConfig.CoCServerConfigData.Configuration conf = udata as stCoCServerConfig.CoCServerConfigData.Configuration;
            HttpListenerContext context = ctx as HttpListenerContext;

            stConsole.WriteHeader("FileWebRquest: " + context.Request.HttpMethod);

            if (
                (udata == null) ||
                (conf.HttpSrv == null)
               )
            {
                context.Response.Abort();
                return;
            }

            byte[] msg = null;
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
            }
            catch (Exception e)
            {
                conf.ILog.LogError(e.Message);
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
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError("[File Web Rquest]: " + e.Message);
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

        #region Template WebRquest

        public static void TemplateWebRquest(string url, object ctx, object udata)
        {
            byte[] msg;
            string filePath;
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
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
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
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
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
                context.Response.AddHeader(conf.HttpSrv.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeHtml));
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError("[Template Web Rquest]: " + e.Message);
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

        #region Json GET WebRquest

        public static void JsonWebRquest(string url, object ctx, object udata)
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
                    throw new ArgumentException(HttpStatusCode.BadRequest.ToString());
                }

#if DEBUG_PrintWebRquest
                stConsole.WriteHeader("JsonWebRquest -> URL: (" + url + ") Query: (" + query + ")");
#endif
                msg = Encoding.UTF8.GetBytes(
                    conf.Api.QueryData(query).ToJson(true, true)
                );

#if DEBUG_PrintJson
                stConsole.WriteHeader("JsonWebRquest -> JSON: " + Encoding.UTF8.GetString(msg));
#endif
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
                conf.ILog.LogError("[Json Web Rquest]: " + e.Message);
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

        #region SSE/RSS/Json notify WebRquest

        public static void SseWebRquest(string url, object ctx, object udata)
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
                        if (conf.Api.NotifySendJsonComplette(client))
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
                HttpUtil.MimeType.MimeHtml,
                context,
                (int)code
            );
        }

        #endregion

    }
}
