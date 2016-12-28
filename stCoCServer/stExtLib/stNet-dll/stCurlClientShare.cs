using System;
using System.Collections.Generic;
using System.Text;
using stNet.CurlSharp;
using System.IO;

namespace stNet
{
    public class stCurlClientShare : IDisposable
    {
        private string urlAuth = null;
        private Dictionary<string, string> headerArgs = null;
        private StringBuilder sb = null;
        private BinaryWriter bw = null;

        #region Compatible variables

        public string RootPath {get; set;}
        public bool UTF8Out { get; set; }

        #endregion

        public stCurlClientShare()
        {
            this._Init(null, null);
        }
        public stCurlClientShare(string authb)
        {
            this._Init(authb, null);
        }
        public stCurlClientShare(Dictionary<string, string> urlargs)
        {
            this._Init(null, urlargs);
        }
        public stCurlClientShare(string authb, Dictionary<string, string> urlargs)
        {
            this._Init(authb, urlargs);
        }
        ~stCurlClientShare()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            try
            {
                if (this.headerArgs != null)
                {
                    this.headerArgs.Clear();
                }
                this._CleanInstance();
                Curl.GlobalCleanup();
            }
            catch (Exception)
            {
            }
            GC.SuppressFinalize(this);
        }
        private void _CleanInstance()
        {
            if (this.sb != null)
            {
                this.sb.Clear();
            }
            if (this.bw != null)
            {
                try
                {
                    this.bw.Close();
                    this.bw.Dispose();
                }
                catch (Exception) { }
                this.bw = null;
            }
        }

        public string Get(string urluri)
        {
            return this._CurlProcess(0, urluri, null, null);
        }
        public string Get(string urluri, string authb)
        {
            return this._CurlProcess(0, urluri, authb, null);
        }
        public string Get(string urluri, string authb, Dictionary<string, string> urlargs)
        {
            return this._CurlProcess(0, urluri, authb, String.Empty, urlargs);
        }
        public string Get(string urluri, string authb, string outpath, Dictionary<string, string> urlargs)
        {
            return this._CurlProcess(0, urluri, authb, outpath, urlargs);
        }
        public string GetJson(string urluri)
        {
            return this._CurlProcess(0, urluri, String.Empty, String.Empty, stCurlClientSet.jsonHeaderArgs);
        }
        public string GetJson(string urluri, string authb)
        {
            return this._CurlProcess(0, urluri, authb, String.Empty, stCurlClientSet.jsonHeaderArgs);
        }
        public string GetFile(string urluri, string outpath)
        {
            return this._CurlProcess(0, urluri, String.Empty, outpath, null);
        }
        public string GetFile(string urluri, string outpath, string authb)
        {
            return this._CurlProcess(0, urluri, authb, outpath, null);
        }
        public string GetFile(string urluri, string outpath, string authb, Dictionary<string, string> urlargs)
        {
            return this._CurlProcess(0, urluri, authb, outpath, urlargs);
        }
        public string GetResult()
        {
            if (this.sb != null)
            {
                return this.sb.ToString();
            }
            return String.Empty;
        }

        #region private method

        private void _Init(string authb, Dictionary<string, string> urlargs)
        {
            try
            {
                Curl.GlobalInit(CurlInitFlag.All);
                this._AddHeaderOptions(authb, urlargs);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void _AddHeaderOptions(string authb, Dictionary<string, string> urlargs)
        {
            if (urlargs != null)
            {
                if (this.headerArgs != null)
                {
                    this.headerArgs.Clear();
                }
                this.headerArgs = urlargs;
            }
            if (!string.IsNullOrWhiteSpace(authb))
            {
                this.urlAuth = authb;
            }
        }
        private CurlSlist _SetHeader(string authb, Dictionary<string, string> urlargs)
        {
            if (
                (string.IsNullOrWhiteSpace(authb)) &&
                (urlargs == null)
               )
            {
                return null;
            }
            
            CurlSlist sl = new CurlSlist();

            if ((urlargs != null) && (urlargs.Count > 0))
            {
                foreach (var row in urlargs)
                {
                    sl.Append(row.Key + ": " + row.Value);
                }
            }
            if (!string.IsNullOrWhiteSpace(authb))
            {
                sl.Append(
                    string.Format(
                        stCurlClientSet.urlBearerShare,
                        authb
                    )
                );
            }
            return sl;
        }
        private string _CurlProcess(int method, string urluri, string authb, string outpath, Dictionary<string, string> urlargs = null)
        {
            if (string.IsNullOrWhiteSpace(urluri))
            {
                throw new ArgumentNullException("Curl URL");
            }

            CurlWriteCallback cb;
            FileStream fs = null;
            bool getFile = ((string.IsNullOrWhiteSpace(outpath)) ? false : true);
            
            try
            {
                if (getFile)
                {
                    if (this.bw != null)
                    {
                        this.bw.Close();
                        this.bw.Dispose();
                        this.bw = null;
                    }
                    cb = this.WriteDataBinary;
                    fs = new FileStream(outpath, FileMode.Create);
                    this.bw = new BinaryWriter(fs);
                }
                else
                {
                    if (this.sb == null)
                    {
                        this.sb = new StringBuilder();
                    }
                    else
                    {
                        this.sb.Clear();
                    }
                    cb = this.WriteDataText;
                    this._AddHeaderOptions(authb, urlargs);
                }

                using (var easy = new CurlEasy())
                {
                    easy.Url = urluri;
                    easy.HttpHeader = ((getFile) ?
                        this._SetHeader(authb, urlargs) :
                        this._SetHeader(this.urlAuth, this.headerArgs)
                    );
                    easy.Encoding = stCurlClientSet.httpEncoding;
                    easy.UserAgent = string.Format(
                        global::stNet.Properties.Settings.Default.stHttpUADefault,
                        Environment.OSVersion.VersionString,
                        Environment.Version.ToString()
                    );
                    easy.Verbose = false;
                    easy.SetOpt(CurlOption.CookieList, "");
                    easy.SetOpt(CurlOption.SslVerifyPeer, false);
                    easy.SetOpt(CurlOption.SslVerifyhost, true);
                    easy.SetOpt(CurlOption.FollowLocation, true);
                    easy.SetOpt(CurlOption.MaxRedirs, 10);
                    easy.WriteFunction = cb;
                    easy.WriteData = null;
                    easy.Perform();
                }
                return ((getFile) ? String.Empty : this.sb.ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (fs != null)
                {
                    try
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                    catch (Exception) { }
                }
                this._CleanInstance();
            }
        }
        private Int32 WriteDataText(byte[] buf, Int32 size, Int32 mb, object voidref)
        {
            if (this.sb == null)
            {
                return 0;
            }
            try
            {
                this.sb.Append(Encoding.UTF8.GetString(buf));
            }
            catch (Exception)
            {
            }
            return (size * mb);
        }
        private Int32 WriteDataBinary(byte[] buf, Int32 size, Int32 mb, object voidref)
        {
            if (this.bw == null)
            {
                return 0;
            }
            try
            {
                this.bw.Write(buf);
                this.bw.Flush();
            }
            catch (Exception)
            {
            }
            return (size * mb);
        }

        #endregion
    }
}
