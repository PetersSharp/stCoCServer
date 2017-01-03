using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.IO;

using stCore;
using stNet.stWebServerUtil;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace stNet
{
    public class stWebResourceLocator
    {
        [DefaultValue("null"),
         Description("Bad request callback")]
        public Action<string, object, object> ReqCallBack = null;
        [DefaultValue("new List<stIpRange>()"),
         Description("ACL IP list, stIpRange")]
        public stIPFilter IPFilter = null;
        [DefaultValue("None"),
         Description("Web Handle Types")]
        public WebHandleTypes HandleTypes = WebHandleTypes.None;
    }

    public enum WebHandleTypes : int
    {
        None,
        FileWebRequest,
        TemplateWebRequest,
        JsonWebRequest,
        ServerSentEventWebRequest,
        InformerWebRequest
    }

    ///<summary>
    ///st WebServer class
    ///</summary>
    public class stWebServer : IDisposable
    {
        private HttpListener _listener = null;
        private Dictionary<string, stWebResourceLocator> _ResourceLocator = null;
        private string _httpUa = null;
        private string _SrvUri = null;
        private int _SrvPort = 0;
        private Action<string, object, object, object> _badReq = null; // (s, q, r, o) => { };
        private IMessage _iLog = new IMessage();
        private Dictionary<string, stWebContentWatcher> _contentwatcher = null;

        private class stWebContentWatcher
        {
            public bool Busy = false;
            public FileSystemWatcher Watch = null;
        }

        private bool _isStop = true;
        private bool _isConcat = false;
        private bool _isMinify = false;
        private bool _isBootstrapHtml = true;
        public  bool IsBootstrapHtml
        {
            get { return this._isBootstrapHtml; }
            set { this._isBootstrapHtml = value; }
        }

        private const string _httpLastModified = @"Last-Modified";
        private const string _httpContentType = @"Content-Type";
        private const string _httpContentDisposition = @"Content-Disposition";
        private const string _httpCacheControl = @"Cache-Control";
        private const string _httpAccelBuffering = @"X-Accel-Buffering";
        private const string _httpAccessControlAllowOrigin = @"Access-Control-Allow-Origin";

        private CultureInfo _ci = null;
        public string DefaultLang
        {
            get { return this._ci.Name; }
            set {
                this._ci = stNet.stWebServerUtil.HttpUtil.GetHttpClientLanguage(value, null);
            }
        }
        public string httpLastModified
        {
            get { return _httpLastModified; }
        }
        public string httpContentType
        {
            get { return _httpContentType; }
        }
        public string httpContentDisposition
        {
            get { return _httpContentDisposition; }
        }
        public string httpCacheControl
        {
            get { return _httpCacheControl; }
        }
        public string httpAccelBuffering
        {
            get { return _httpAccelBuffering; }
        }
        public string httpAccessControlAllowOrigin
        {
            get { return _httpAccessControlAllowOrigin; }
        }

        private bool _isDisposed = false;
        ///<summary>
        ///Get Disposed state of class
        ///</summary>
        public bool Disposed
        {
            get { return this._isDisposed; }
        }
        ///<summary>
        ///Set info prin/logging function
        ///</summary>
        public Action<object> wLogInfo
        {
            set { this._iLog.LogInfo = value; }
        }
        ///<summary>
        ///Set error prin/logging function
        ///</summary>
        public Action<object> wLogError
        {
            set { this._iLog.LogError = value; }
        }
        ///<summary>
        ///Set Bad Request CallBack function
        ///</summary>
        public Action<string, object, object, object> wBadReqCallBack
        {
            set { this._badReq = value; }
        }
        ///<summary>
        ///Get Bad Request function name
        ///</summary>
        public string wBadRequestName
        {
            get { return ((this._badReq == null) ? "none" : this._badReq.Method.Name); }
        }
        private bool _BadRequestDebugOut = false;
        ///<summary>
        ///Bad HTTP request debug HTML output
        ///</summary>
        public bool wBadRequestDebugOut
        {
            get { return this._BadRequestDebugOut; }
            set { this._BadRequestDebugOut = value; }
        }
        private bool _WatchChange = true;
        ///<summary>
        ///Watch Change concat list files in locations
        ///</summary>
        public bool wWatchChange
        {
            get { return this._WatchChange; }
            set { this._WatchChange = value; }
        }
        ///<summary>
        ///Set Uri Template: 
        ///     * | +.* | http://+.*:8182 | http://localhost:8182 | etc..
        ///</summary>
        public void wSetUriTemplate(string srvUri, int srvPort)
        {
            this._ServerUriTemplate(srvUri, srvPort);
        }
        private object _userData = null;
        ///<summary>
        ///Set User Data object
        ///</summary>
        public object wUserData
        {
            set { this._userData = value; }
        }
        public string wUserAgent
        {
            get { return ((string.IsNullOrWhiteSpace(this._httpUa)) ? "stWebServer" : this._httpUa); }
        }

        /// <summary>
        /// Enable concat source CSS/JS files
        /// </summary>
        public bool isConcat
        {
            get { return this._isConcat; }
            set { this._isConcat = value; }
        }
        /// <summary>
        /// Enable minified source CSS/JS files
        /// </summary>
        public bool isMinify
        {
            get { return this._isMinify; }
            set { this._isMinify = value; }
        }

        public stWebServer(string srvUri = null, int srvPort = 8989, IMessage ilog = null, Action<string, object, object, object> badReq = null)
        {
            if (!HttpListener.IsSupported)
            {
                throw new ArgumentNullException(Properties.Resources.httpListenerNoSupport);
            }
            if (string.IsNullOrWhiteSpace(srvUri))
            {
                throw new ArgumentNullException(Properties.Resources.httpListenerUrlEmpty);
            }
            if (ilog != null)
            {
                this._iLog = ilog;
            }
            if (badReq != null)
            {
                this._badReq = badReq;
            }
            if (this._httpUa == null)
            {
                this._httpUa = HttpUtil.GetHttpUA("Server");
            }
            this._contentwatcher = new Dictionary<string, stWebContentWatcher>();
            this._ServerUriTemplate(srvUri, srvPort);
            this._Init();
        }
        ~stWebServer()
        {
            this._Clear();
        }
        public void Dispose()
        {
            this._Clear();
        }
        private void _Init()
        {
            try
            {
                this._ResourceLocator = new Dictionary<string, stWebResourceLocator>();
                this._listener = new HttpListener();
            }
            catch (Exception e)
            {
                this._iLog.LogError(Properties.Resources.httpListenerInit + e.Message);
                return;
            }
        }
        private void _ServerUriTemplate(string srvUri, int srvPort)
        {
            if ( string.IsNullOrWhiteSpace(srvUri))
            {
                return;
            }
            if (srvUri.StartsWith(@"http"))
            {
                this._SrvUri = srvUri;
            }
            else if ((srvPort == 0) && (this._SrvPort > 0))
            {
                this._SrvUri = string.Format(
                    @"http://{0}:{1}",
                    srvUri,
                    this._SrvPort.ToString()
                );
            }
            else if (srvPort == 0)
            {
                this._SrvUri = string.Format(
                    @"http://{0}",
                    srvUri
                );
            }
            else
            {
                this._SrvUri = string.Format(
                    @"http://{0}:{1}",
                    srvUri,
                    srvPort.ToString()
                );
                this._SrvPort = srvPort;
            }
        }
        public bool Start(ref bool isRun)
        {
            try
            {
                this._Check();

                if (
                    (this._ResourceLocator.Count == 0) ||
                    (this._listener.Prefixes.Count == 0)
                   )
                {
                    throw new ArgumentOutOfRangeException(Properties.Resources.httpListenerEmptyResource);
                }
                this._isStop = false;
                System.Threading.ThreadPool.SetMaxThreads(50, 1000);
                System.Threading.ThreadPool.SetMinThreads(50, 50);
                this._listener.Start();
                if (isRun)
                {
                    ThreadPool.QueueUserWorkItem(s =>
                    {
                        while ((!this._isStop) && (this._listener.IsListening))
                        {
                            try
                            {
                                ThreadPool.QueueUserWorkItem(this._OnRequest, this._listener.GetContext());
                            }
                            catch (HttpListenerException e)
                            {
                                if (this._isStop)
                                {
                                    break;
                                }
                                this._iLog.LogError(Properties.Resources.httpListenerExceptionLoop + e.Message);
                            }
                        }
                    });
                }
                else
                {
                    this.Stop();
                    return false;
                }
            }
            catch (InvalidOperationException) { }
            catch (HttpListenerException e)
            {
                this._iLog.LogError(Properties.Resources.httpListenerExceptionStartHttp + e.Message);
                return false;
            }
            catch (Exception e)
            {
                this._iLog.LogError(Properties.Resources.httpListenerExceptionStart + e.Message);
                return false;
            }
            return true;
        }
        public void Stop()
        {
            try
            {
                this._Check();
                this._isStop = true;
                this._listener.Stop();
            }
            catch (Exception e)
            {
                this._iLog.LogError(Properties.Resources.httpListenerExceptionStop + e.Message);
                return;
            }
        }
        /// <summary>
        /// Add URL right path part (Prefixes)
        /// </summary>
        /// <param name="ht">enum <see cref="stNet.WebHandleTypes"/> Web Handle Types</param>
        /// <param name="httpRequestHandler">Function request job</param>
        /// <param name="ipFilter">ACL IP Filter ipFilter structure</param>
        /// <param name="path">URL path</param>
        /// <param name="dir">Full directory path for concat or minified files</param>
        public void AddHandler(Action<string, object, object> httpRequestHandler, string path)
        {
            this._AddHandler(stNet.WebHandleTypes.None, httpRequestHandler, null, path, null);
        }
        public void AddHandler(WebHandleTypes ht, Action<string, object, object> httpRequestHandler, string path, stIPFilter ipFilter)
        {
            this._AddHandler(ht, httpRequestHandler, ipFilter, path, null);
        }
        public void AddHandler(WebHandleTypes ht, Action<string, object, object> httpRequestHandler, string path = null, string dir = null)
        {
            this._AddHandler(ht, httpRequestHandler, null, path, dir);
        }
        public void AddHandler(WebHandleTypes ht, Action<string, object, object> httpRequestHandler, string path = null, stIPFilter ipFilter = null, string dir = null)
        {
            this._AddHandler(ht, httpRequestHandler, ipFilter, path, dir);
        }
        private void _AddHandler(WebHandleTypes ht, Action<string, object, object> httpRequestHandler, stIPFilter ipFilter, string path, string dir)
        {
            this._Check();

            if (httpRequestHandler == null)
            {
                throw new ArgumentNullException(Properties.Resources.httpCallbackNull);
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                path = httpRequestHandler.ToString();
            }

            path = ((!path.StartsWith("/")) ? "/" + path : path);
            path = ((!path.EndsWith("/")) ? path + "/" : path);

            stWebResourceLocator resLocator = new stWebResourceLocator()
            {
                IPFilter = ((ipFilter == null) ? new stIPFilter() : ipFilter),
                ReqCallBack = httpRequestHandler,
                HandleTypes = ht
            };

            if (!this._ResourceLocator.ContainsKey(path))
            {
                this._ResourceLocator.Add(path, resLocator);
            }
            else
            {
                this._ResourceLocator[path] = resLocator;
            }
            if (!this._listener.Prefixes.Contains(path))
            {
                try
                {
                    this._listener.Prefixes.Add(
                        string.Format(
                            @"{0}{1}",
                            this._SrvUri,
                            path
                        )
                    );
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.httpListenerExceptionAddResource,
                            e.Message
                        )
                    );
                }
            }
            if (
                ((this._isConcat) || (this._isMinify)) &&
                (!string.IsNullOrWhiteSpace(dir))
               )
            {
                path = path.Substring(1,(path.Length - 2));
                path = Path.Combine(dir, path);

                if (Directory.Exists(path))
                {
                    stBootStrap minify = null;

                    try
                    {
                        minify = new stBootStrap(this._isConcat, this._isMinify, this._iLog);
                        if (this._WatchChange)
                        {
                            minify.Minify(path, this._AddContentChangeWatch);
                        }
                        else
                        {
                            minify.Minify(path, null);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException(e.Message);
                    }
                    finally
                    {
                        if (minify != null)
                        {
                            minify.Dispose();
                        }
                    }
                }
            }
        }
        [Conditional("DEBUG")]
        public void PrintResorceLocation()
        {
            foreach (KeyValuePair<string, stWebResourceLocator> pair in this._ResourceLocator)
            {
                stWebResourceLocator rl = pair.Value as stWebResourceLocator;
                stConsole.WriteLine(
                    pair.Key +
                    "\n\t - IPFilter: " + (rl.IPFilter != null).ToString() +
                    "\n\t - CallBack: " + ((rl.ReqCallBack != null) ? rl.ReqCallBack.Method.Name : "*CallBack none") +
                    "\n\t - IpFilterType: " + ((rl.IPFilter != null) ? rl.IPFilter.IpFilterType.ToString() : "*IPFilter is null") +
                    "\n\t - GeoFilterType: " + ((rl.IPFilter != null) ? rl.IPFilter.GeoAsnFilterType.ToString() : "*IPFilter is null") +
                    "\n\t - List.IpRange: " + (rl.IPFilter.IpRange != null).ToString() + ", Count: " + ((rl.IPFilter != null) ? ((rl.IPFilter.IpRange != null) ? rl.IPFilter.IpRange.Count : 0) : 0) +
                    "\n\t - List.GeoDataASN: " + (rl.IPFilter.GeoDataASN != null).ToString() + ", Count: " + ((rl.IPFilter != null) ? ((rl.IPFilter.GeoDataASN != null) ? rl.IPFilter.GeoDataASN.Count : 0) : 0) +
                    "\n\t - List.GeoDataCounry: " + (rl.IPFilter.GeoDataCounry != null).ToString() + ", Count: " + ((rl.IPFilter != null) ? ((rl.IPFilter.GeoDataCounry != null) ? rl.IPFilter.GeoDataCounry.Count : 0) : 0)
                );
            }
        }
        private bool _Check()
        {
            if (this._listener == null)
            {
                throw new ArgumentNullException(Properties.Resources.httpListenerNull);
            }
            return true;
        }
        private void _Clear()
        {
            if (this._isDisposed)
            {
                return;
            }
            this._isDisposed = true;

            foreach (KeyValuePair<string, stWebContentWatcher> watch in this._contentwatcher)
            {
                ((stWebContentWatcher)watch.Value).Watch.Dispose();
            }
            this._contentwatcher.Clear();
            this._contentwatcher = null;

            if (this._ResourceLocator != null) { this._ResourceLocator.Clear(); }
            if (this._listener != null)
            {
                this._listener.Stop();
                this._listener.Close();
            }
            this._ResourceLocator = null;
            this._listener = null;
        }
        private void _OnRequest(object ctx)
        {
            bool isRequestFound = false;
            HttpListenerContext context = ctx as HttpListenerContext;

            context.Response.Headers.Add(@"Server", this._httpUa);
            context.Response.ProtocolVersion = new Version("1.1");

            try
            {
                int statusCode = 0;
                string respTxt = String.Empty;
                WebHandleTypes wht = WebHandleTypes.None;

                foreach (KeyValuePair<string, stWebResourceLocator> pair in this._ResourceLocator)
                {
                    if (context.Request.RawUrl.StartsWith(pair.Key))
                    {
                        stWebResourceLocator rl = pair.Value as stWebResourceLocator;
                        if (rl.IPFilter != null)
                        {
                            if (rl.IPFilter.IpFilterCheck(
                                stWebServerUtil.HttpUtil.GetHttpClientIP(context.Request))
                               )
                            {
                                wht = rl.HandleTypes;
                                respTxt = HttpStatusCode.Forbidden.ToString();
                                statusCode = (int)HttpStatusCode.Forbidden;
                                break;
                            }
                        }
                        rl.ReqCallBack(context.Request.RawUrl, context, this._userData);
                        statusCode = (int)HttpStatusCode.OK;
                        isRequestFound = true;
                        break;
                    }
                }
                if (!isRequestFound)
                {
                    respTxt = ((string.IsNullOrWhiteSpace(respTxt)) ? context.Request.RawUrl : respTxt);
                    context.Response.StatusCode = ((statusCode > 0) ? statusCode : (int)HttpStatusCode.BadRequest);

                    if (this._badReq != null)
                    {
                        this._badReq(
                            respTxt,
                            context.Request,
                            context.Response,
                            this._userData
                        );
                    }
                    else
                    {
                        switch (wht)
                        {
                            default:
                            case WebHandleTypes.None:
                            case WebHandleTypes.TemplateWebRequest:
                            case WebHandleTypes.FileWebRequest:
                                {
                                    this.BadRequestHtml(
                                        respTxt,
                                        context,
                                        statusCode
                                    );
                                    break;
                                }
                            case WebHandleTypes.JsonWebRequest:
                                {
                                    this.BadRequestJson(
                                        respTxt,
                                        context,
                                        statusCode
                                    );
                                    break;
                                }
                        }
                    }
                }
            }
            catch (Exception e) 
            {
                this._iLog.LogError(Properties.Resources.httpListenerExceptionOnReq + e.Message);
            }
        }
        public void BadRequestRaw(byte[] raw, HttpListenerContext context, int err, HttpUtil.MimeType mtype = HttpUtil.MimeType.MimePng)
        {
            context.Response.AddHeader(this.httpContentType, HttpUtil.GetMimeType("", mtype));
            this._BadRequest(
                raw,
                context,
                err
            );
        }
        public void BadRequestHtml(string reason, HttpListenerContext context, int err)
        {
            CultureInfo ci = stWebServerUtil.HttpUtil.GetHttpClientLanguage(context.Request.UserLanguages, this._ci);
            context.Response.AddHeader(this.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeHtml));
            this._BadRequest(
                Encoding.UTF8.GetBytes(
                    ((this._isBootstrapHtml) ?
                        string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("httpHtmlError", ci),
                            this.wUserAgent,
                            err,
                            reason,
                            DateTime.Now.ToString()
                        )
                        :
                        string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("httpHTMLBadRequest", ci),
                            err, reason, wUserAgent
                        )
                    )
                ),
                context,
                err
            );
        }
        public void BadRequestJson(string reason, HttpListenerContext context, int err)
        {
            CultureInfo ci = stWebServerUtil.HttpUtil.GetHttpClientLanguage(context.Request.UserLanguages, this._ci);
            context.Response.AddHeader(this.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeJson));
            this._BadRequest(
                Encoding.UTF8.GetBytes(
                    string.Format(
                        (string)Properties.Resources.ResourceManager.GetString("httpJsonResponse", ci),
                        err, reason, wUserAgent
                    )
                ),
                context,
                err
            );
        }
        public void BadRequestXml(string reason, HttpListenerContext context, int err)
        {
            CultureInfo ci = stWebServerUtil.HttpUtil.GetHttpClientLanguage(context.Request.UserLanguages, this._ci);
            context.Response.AddHeader(this.httpContentType, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeXml));
            this._BadRequest(
                Encoding.UTF8.GetBytes(
                    string.Format(
                        (string)Properties.Resources.ResourceManager.GetString("httpXmlError", ci),
                        err, reason, DateTime.Now
                    )
                ),
                context,
                err
            );
        }
        private void _BadRequest(string reason, object ctx, object udata)
        {
            this.BadRequestHtml(reason, (HttpListenerContext)ctx, 404);
        }
        private void _BadRequest(byte[] reason, HttpListenerContext context, int err)
        {
            byte[] msg;

            if ((this._BadRequestDebugOut) && (reason == null))
            {
                msg = Encoding.UTF8.GetBytes(stNet.stWebServerUtil.HttpUtil.ObjectDump((HttpListenerRequest)context.Request));
            }
            else if (reason == null)
            {
                CultureInfo ci = stWebServerUtil.HttpUtil.GetHttpClientLanguage(context.Request.UserLanguages, this._ci);
                msg = Encoding.UTF8.GetBytes(
                    string.Format(
                        (string)Properties.Resources.ResourceManager.GetString("httpHTMLBadRequest", ci),
                        err
                    )
                );
            }
            else
            {
                msg = reason;
            }
            try
            {
                context.Response.AddHeader(this.httpCacheControl, "no-cache");
                context.Response.AddHeader(this.httpAccelBuffering, "no");
                context.Response.StatusCode = err;
                context.Response.ContentLength64 = msg.Length;
                context.Response.OutputStream.Write(msg, 0, msg.Length);
                context.Response.OutputStream.Close();
            }
            catch (Exception)
            {
                context.Response.Abort();
            }
        }
        private void _AddContentChangeWatch(string path)
        {
            FileSystemWatcher watch = new FileSystemWatcher();
            watch.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.FileName;
            watch.Path = Path.GetDirectoryName(path);
            watch.Filter = Path.GetFileName(path);
            watch.Changed += new FileSystemEventHandler(this._OnContentChangeWatch);
            watch.EnableRaisingEvents = true;
            this._contentwatcher.Add(path, new stWebContentWatcher() { Watch = watch, Busy = false });
        }
        private void _OnContentChangeWatch(object source, FileSystemEventArgs ev)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Dictionary<string, stWebContentWatcher> dict = this._contentwatcher
                        .Where(o => (o.Key.Contains(ev.FullPath)) && (!o.Value.Busy))
                        .ToDictionary(o => o.Key, o => o.Value);

                    if (dict != null)
                    {
                        this._iLog.LogInfo(
                            string.Format(
                                Properties.Resources.httpMinifyChanged,
                                ev.ChangeType,
                                ev.Name
                            )
                        );
                        foreach (KeyValuePair<string, stWebContentWatcher> watch in dict)
                        {
                            watch.Value.Busy = true;
                            stBootStrap minify = new stBootStrap(this._isConcat, this._isMinify, this._iLog);
                            minify.Minify(Path.GetDirectoryName(watch.Key), null);
                            watch.Value.Busy = false;
                        }
                    }
                }
                catch (Exception e)
                {
                    this._iLog.LogError(e.Message);
                }
            });
        }

    }
}
