
#if DEBUG
// #define DEBUG_Headers
// #define DEBUG_CertificateSSL
#endif

using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using stCore;

namespace stNet
{
    public class stWebClient : WebClient
    {
        #region HTTP stWebClient class

        public enum MethodType : int
        {
            GET = 0,
            HEAD = 1,
            POST = 2
        };
        private readonly List<string> MethodName = new List<string>()
        {
            "GET",
            "HEAD",
            "POST"
        };
        public string wUserAgent { get; set; }
        public int wTimeoutConnect { get; set; }
        public int wTimeoutRW { get; set; }
        public int wHttpSecurityProtocolType { get; set; }
        public bool wHttpFakeCeretSSL { get; set; }
        public bool wHttpSupportHEAD { get; set; }
        public Dictionary<string, string> wHeaderList { get; set; }

        private string wAuthorization { get; set; }
        private string wMethodString { get; set; }
        private System.Net.CookieContainer wCookie = null;
        private System.Net.WebResponse wResponse = null;
        private bool wHttpExtMessage = false;

        private stCore.IMessage _ilog = null;
        public stCore.IMessage iLog
        {
            get { return _ilog; }
            set { if (value != null) { _ilog = value; wHttpExtMessage = true; } }
        }
        public stWebClient.MethodType wMethod
        {
            set { this.wMethodString = this.SelectMethod(value); }
        }
        private readonly string[] fmtLog = { "{0} {1}", "{0} ({1}) : {2}" };

        public WebResponse Response
        {
            get { return this.wResponse; }
        }

        public void AuthBearer(string key)
        {
            this.wAuthorization = ((string.IsNullOrWhiteSpace(key)) ?
                this.wAuthorization :
                "Bearer " + key
            );
        }
        public void AuthBasic(string usr, string pwd)
        {
            this.wAuthorization = (((string.IsNullOrWhiteSpace(usr)) || (string.IsNullOrWhiteSpace(pwd))) ?
                this.wAuthorization :
                "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(usr + ":" + pwd))
            );
        }
        public string SelectMethod(stWebClient.MethodType meth)
        {
            switch (meth)
            {
                default:
                case MethodType.GET:
                    {
                        break;
                    }
                case MethodType.HEAD:
                    {
                        if (this.wHttpSupportHEAD)
                        {
                            return this.MethodName[Convert.ToInt32(meth)];
                        }
                        break;
                    }
                case MethodType.POST:
                    {
                        return this.MethodName[Convert.ToInt32(meth)];
                    }
            }
            return String.Empty;
        }

        #region stWebClient INIT

        public stWebClient()
        {
            this._Init(0, 0, MethodType.GET);
        }

        public stWebClient(MethodType meth)
        {
            this._Init(0, 0, meth);
        }

        public stWebClient(
            int timeoutConnect,
            int timeoutRW
        )
        {
            this._Init(
                timeoutConnect,
                timeoutRW,
                MethodType.GET
            );
        }

        public stWebClient(
            int timeoutConnect,
            int timeoutRW,
            MethodType meth
        )
        {
            this._Init(
                timeoutConnect,
                timeoutRW,
                meth
            );
        }

        private void _Init(long timeoutConnect, long timeoutRW, MethodType meth)
        {
            this.wCookie = new CookieContainer();
            this.wMethodString = null;
            this.wUserAgent = null;
            this.wAuthorization = null;
            this.wHttpSecurityProtocolType = 0;
            this.wHttpFakeCeretSSL = false;
            this.wHttpSupportHEAD = false;
            this.wTimeoutConnect = ((timeoutConnect <= 0) ?
                 (int)global::stNet.Properties.Settings.Default.stConnectTimeout :
                 wTimeoutConnect
            );
            this.wTimeoutRW = ((timeoutRW <= 0) ?
                (int)global::stNet.Properties.Settings.Default.stRWTimeout :
                wTimeoutRW
            );
            this.Encoding = System.Text.Encoding.UTF8;
            this.Proxy = null;
            this.wMethodString = this.SelectMethod(meth);

            switch (meth)
            {
                default:
                case MethodType.GET:
                    {
                        break;
                    }
                case MethodType.HEAD:
                    {
                        if (this.wHttpSupportHEAD)
                        {
                            this.wMethodString = this.MethodName[Convert.ToInt32(meth)];
                        }
                        break;
                    }
                case MethodType.POST:
                    {
                        this.wMethodString = this.MethodName[Convert.ToInt32(meth)];
                        break;
                    }
            }
            this.stHttpCeretSSL();
        }

        #endregion

        private void stHttpCeretSSL()
        {
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(this.ValidateCertificateSSL);
            switch (this.wHttpSecurityProtocolType)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                        break;
                    }
                case 2:
                    {
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                        break;
                    }
            }
        }

        public bool ValidateCertificateSSL(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors err)
        {
#if DEBUG_CertificateSSL
            stConsole.WriteHeader(
                "SSL Validate Certificate: " + Environment.NewLine +
                "- SSL Policy Errors: " + err.ToString() + Environment.NewLine +
                "- X509 Certificate: " + cert.ToString()
            );
#endif
            if (this.wHttpFakeCeretSSL)
            {
                return true;
            }
            if (err == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }
            if (this.wHttpExtMessage)
            {
                this.iLog.ToLogAndLine(
                    System.Reflection.MethodBase.GetCurrentMethod(),
                    string.Format(
                        fmtLog[1],
                        global::stNet.Properties.Resources.httpError,
                        cert.Subject,
                        err.ToString()
                    )
                );
            }
            return false;
        }

        protected override WebRequest GetWebRequest(Uri url)
        {
            WebRequest req = base.GetWebRequest(url);

            if (
                (req != null) &&
                (req.GetType() == typeof(HttpWebRequest))
               )
            {
                req.Timeout = this.wTimeoutConnect;
                ((HttpWebRequest)req).CookieContainer = this.wCookie;
                ((HttpWebRequest)req).ReadWriteTimeout = this.wTimeoutRW;
                ((HttpWebRequest)req).UserAgent = ((string.IsNullOrWhiteSpace(this.wUserAgent)) ?
                    string.Format(
                        global::stNet.Properties.Settings.Default.stHttpUADefault,
                        Environment.OSVersion.VersionString,
                        Environment.Version.ToString()
                    ) :
                    this.wUserAgent
                );
                ((HttpWebRequest)req).AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                ((HttpWebRequest)req).Headers[HttpRequestHeader.AcceptEncoding] = "gzip,deflate";
                if (!string.IsNullOrWhiteSpace(this.wMethodString))
                {
                    ((HttpWebRequest)req).Method = this.wMethodString;
                }
                if (!string.IsNullOrWhiteSpace(this.wAuthorization))
                {
                    ((HttpWebRequest)req).Headers[HttpRequestHeader.Authorization] = this.wAuthorization;
                }

#if DEVEL
                if (this.wHeaderList != null)
                {
                    foreach (var lst in this.wHeaderList)
                    {
                        // TODO: bug is name key collection..
                        //Console.WriteLine(lst.Key + "   :   " + lst.Value + "***");
                        ((HttpWebRequest)req).Headers.Remove(lst.Key);
                        ((HttpWebRequest)req).Headers.Add(lst.Key as string, lst.Value as string);
                    }
                }
#endif

#if DEBUG_Headers
                foreach (string lst in ((HttpWebRequest)req).Headers)
                {
                    Console.WriteLine(lst + "   :   " + ((HttpWebRequest)req).Headers[lst]);
                }
#endif
            }
            return req;
        }
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            try
            {
                return this._GetWebResponse(request, null);
            }
            catch (Exception e)
            {
#if DEBUG
                stConsole.WriteHeader(e.ToString());
#endif
                throw e;
            }

        }
        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            try
            {
                return this._GetWebResponse(request, result);
            }
            catch (Exception e)
            {
#if DEBUG
                stConsole.WriteHeader(e.ToString());
#endif
                throw e;
            }
        }
        private WebResponse _GetWebResponse(WebRequest request, IAsyncResult result)
        {
            try
            {
                if (result == null)
                {
                    this.wResponse = base.GetWebResponse(request);
                }
                else
                {
                    this.wResponse = base.GetWebResponse(request, result);
                }
                return this.wResponse;
            }
            catch (WebException e)
            {
#if DEBUG
                stConsole.WriteHeader(e.ToString());
#endif
                string info = string.Format(
                    fmtLog[0],
                    global::stNet.Properties.Resources.httpError,
                    e.Message
                );
                if (this.wHttpExtMessage)
                {
                    this.iLog.ToLogAndLine(
                        System.Reflection.MethodBase.GetCurrentMethod(),
                        info
                    );
                    this.iLog.BoxError(info, Properties.Resources.httpBoxButton, 5);
                }
                stCore.LogException.Error(info, this._ilog);
                return null;
            }
            catch (Exception e)
            {
#if DEBUG
                stConsole.WriteHeader(e.ToString());
#endif
                string info = string.Format(
                    fmtLog[0],
                    global::stNet.Properties.Resources.httpLocalError,
                    e.Message
                );

                if (this.wHttpExtMessage)
                {
                    this.iLog.ToLogAndLine(
                        System.Reflection.MethodBase.GetCurrentMethod(),
                        info
                    );
                    this.iLog.BoxError(info, Properties.Resources.httpBoxButton, 5);
                }
                stCore.LogException.Error(info, this._ilog);
                return null;
            }
        }

        #endregion
    }
}
