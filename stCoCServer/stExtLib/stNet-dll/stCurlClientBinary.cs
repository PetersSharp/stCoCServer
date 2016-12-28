using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using stCore;

namespace stNet
{
    /// <summary>
    ///  Class to use native executable curl/curl.exe
    ///  SSL/TLS is broken in Net 4.0, error handshake
    ///  worked cUrl + OpenSSL binary, link: http://winampplugins.co.uk/curl/
    ///  TODO: include openssl + libcurl static link and write http request wrapper..
    /// </summary>
    public class stCurlClientBinary : IDisposable
    {
        private string _rootPath = String.Empty;
        private string _curlPath = String.Empty;
        private string _urlUri = String.Empty;

        public string Url
        {
            get { return this._urlUri; }
            set { this._urlUri = value; }
        }
        public string CurlPath
        {
            get { return this._curlPath; }
            set { this._curlPath = value; }
        }
        public string RootPath
        {
            get { return this._rootPath; }
            set { this._rootPath = Path.Combine(value, @"tmp"); }
        }
        public bool CheckCurlPath { get; set; }
        public bool UTF8Out { get; set; }

        public stCurlClientBinary()
        {
            this._curlPath = this.exeDefaultRunTime();
            this.UTF8Out = false;
        }
        public stCurlClientBinary(string exepath)
        {
            this._curlPath = ((string.IsNullOrWhiteSpace(exepath)) ? this.exeDefaultRunTime() : exepath);
            this.UTF8Out = false;
        }
        public stCurlClientBinary(string exepath, string urluri)
        {
            this._curlPath = ((string.IsNullOrWhiteSpace(exepath)) ? this.exeDefaultRunTime() : exepath);
            this._urlUri = ((string.IsNullOrWhiteSpace(urluri)) ? this._urlUri : urluri);
            this.UTF8Out = false;
        }
        ~stCurlClientBinary()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
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

        #region private method

        private string exeDefaultRunTime()
        {
            return ((stRuntime.isRunTime()) ? stCurlClientSet.exeDefaultBinary[0] : stCurlClientSet.exeDefaultBinary[1]);
        }
        private bool _CheckCurlBin()
        {
            if (this.CheckCurlPath)
            {
                return true;
            }
            if (File.Exists(this._curlPath))
            {
                return true;
            }
            return false;
        }
        private void _CheckUTF8Out()
        {
            if (string.IsNullOrWhiteSpace(this._rootPath))
            {
                throw new ArgumentNullException(Properties.Resources.curlProcessError9);
            }
            if (!Directory.Exists(this._rootPath))
            {
                try
                {
                    Directory.CreateDirectory(this._rootPath);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        private ProcessStartInfo _ArgumentsParser(string urluri, string authb, string outpath, Dictionary<string, string> urlargs, int method)
        {
            string uri = ((string.IsNullOrWhiteSpace(urluri)) ? this._urlUri : urluri);
            if ((string.IsNullOrWhiteSpace(uri)) || (string.IsNullOrWhiteSpace(this._curlPath)))
            {
                return null;
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            StringBuilder sb = new StringBuilder();

            if ((urlargs != null) && (urlargs.Count > 0))
            {
                foreach (var row in urlargs)
                {
                    sb.Append(" -H \"" + row.Key + ": " + row.Value + "\"");
                }
            }
            if (!string.IsNullOrWhiteSpace(outpath))
            {
                sb.AppendFormat(stCurlClientSet.outPathBinary, outpath);
            }
            if (!string.IsNullOrWhiteSpace(authb))
            {
                sb.AppendFormat(stCurlClientSet.urlBearerBinary, authb);
            }
            sb.Append(stCurlClientSet.urlMethodBinary[method] + uri);

            startInfo.Arguments = sb.ToString();
            startInfo.FileName = this._curlPath;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;

            return startInfo;
        }
        private string _CurlProcess(int method, string urluri, string authb, string outpath, Dictionary<string, string> urlargs = null)
        {
            string outfile = String.Empty;

            if (!this._CheckCurlBin())
            {
                throw new ArgumentException(Properties.Resources.curlProcessError1);
            }
            if (this.UTF8Out)
            {
                if (string.IsNullOrWhiteSpace(outpath))
                {
                    this._CheckUTF8Out();
                    outpath = outfile = Path.Combine(
                        this._rootPath,
                        "curl-get-" + DateTime.Now.Ticks.ToString() + ".tmp"
                    );
                }
            }
            Task<string> t1 = null;
            ProcessStartInfo startInfo = this._ArgumentsParser(urluri, authb, outpath, urlargs, method);
            if (startInfo == null)
            {
                throw new ArgumentException(Properties.Resources.curlProcessError2);
            }
            try
            {
                t1 = (Task<string>)this._CurlTask(startInfo);
                t1.Wait();
                string result = t1.Result;
                if ((t1.IsFaulted) || (t1.IsCanceled))
                {
                    throw new ArgumentNullException(Properties.Resources.curlProcessError3);
                }
                if (result == null)
                {
                    throw new ArgumentNullException(Properties.Resources.curlProcessError4);
                }
                if (!string.IsNullOrWhiteSpace(outfile))
                {
                    try
                    {
                        result = System.IO.File.ReadAllText(outfile, Encoding.UTF8);
                        File.Delete(outfile);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    string.Format(
                        Properties.Resources.curlProcessError5,
                        e.Message
                    )
                );
            }
            finally
            {
                t1.Dispose();
            }
        }
        private Task<string> _CurlTask(ProcessStartInfo startInfo)
        {
            return (Task<string>)Task<string>.Factory.StartNew(() =>
            {
                try
                {
                    var process = Process.Start(startInfo);
                    var reader = process.StandardOutput;
                    string result = null;

                    if (reader == null)
                    {
                        throw new ArgumentException(Properties.Resources.curlProcessError6);
                    }
                    result = reader.ReadToEnd();
                    if (!process.WaitForExit(200))
                    {
                        throw new ArgumentException(Properties.Resources.curlProcessError7);
                    }
                    return result;
                }
                catch (Exception e)
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.curlProcessError8,
                            e.Message
                        )
                    );
                }
            });
        }

        #endregion

    }
}
