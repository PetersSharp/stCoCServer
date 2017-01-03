
#if DEBUG
// #define DEBUG_XMLResponse
// #define DEBUG_prnXMLResponse
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using stDokuWiki.Util;

namespace stDokuWiki.Connector
{
    /// <summary>
    /// Rpc-Xml DokuWiki connector class
    /// </summary>
    /// <code>
    /// using stDokuWiki;
    /// using stDokuWiki.Data;
    /// using stDokuWiki.Connector;
    /// </code>
    public partial class RpcXml
    {
        #region Variables

        private const string className = "XML-RPC: ";
        private const string urlRPCPart = "lib/exe/xmlrpc.php";
        private bool _isAuth = false;
        private string _url = String.Empty,
                       _login = String.Empty,
                       _passwd = String.Empty,
                       _namespace = String.Empty;
        private CookieContainer _cookie = null;

        /// <summary>
        /// (String) URL DokuWiki server
        /// </summary>
        public string Url
        {
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this._url = value + ((value[(value.Length - 1)] == '/') ? "" : "/") + urlRPCPart;
                    this._isAuth = false;
                    this._CheckAuth();
                }
                else
                {
                    throw new RpcXmlException(
                        string.Format(
                            Properties.ResourceCodeError.rpcErrorIntFormat,
                            className,
                            "Url"
                        ),
                        5050
                    );
                }
            }
        }
        /// <summary>
        /// (String) Login to DokuWiki server
        /// </summary>
        public string Login
        {
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this._login = value;
                    this._isAuth = false;
                }
                else
                {
                    throw new RpcXmlException(
                        string.Format(
                            Properties.ResourceCodeError.rpcErrorIntFormat,
                            className,
                            "Login"
                        ),
                        5051
                    );
                }
            }
        }
        /// <summary>
        /// (String) Password to DokuWiki server
        /// </summary>
        public string Passwd
        {
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this._passwd = value;
                    this._isAuth = false;
                }
                else
                {
                    throw new RpcXmlException(
                        string.Format(
                            Properties.ResourceCodeError.rpcErrorIntFormat,
                            className,
                            "Password"
                        ),
                        5052
                    );
                }
            }
        }
        /// <summary>
        /// (String) Default DokuWiki Namespace
        /// <example>
        /// "wiki:"
        /// </example>
        /// </summary>
        public string NameSpaceDefault
        {
            get { return this._namespace; }
            set { this._namespace = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// (class) Rpc-Xml Constructor
        /// </summary>
        /// <param name="url">DokuWiki base Url</param>
        /// <param name="login">you DokuWiki login credentials</param>
        /// <param name="passwd">you DokuWiki password credentials</param>
        public RpcXml(string url, string login, string passwd)
        {
            this.Login = login;
            this.Passwd = passwd;
            this.Url = url;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Execute DokuWiki command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public T Get<T>(XmlRpcRequest type, int n)
        {
            string methodName = type.ToString();
            return this.Get<T>(
                this._RpcXmlRequestToString<Data.RpcXmlRequestInt>(
                    this._CreateRequestFromInt(type, methodName, n),
                    methodName
                )
#if DEBUG_XMLResponse
                , methodName
#endif
            );
        }
        /// <summary>
        /// Execute DokuWiki command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public T Get<T>(XmlRpcRequest type, string val, int n)
        {
            string methodName = type.ToString();
            return this.Get<T>(
                this._RpcXmlRequestToString<Data.RpcXmlRequestComposite>(
                    this._CreateRequestFromComposite(type, methodName, val, n),
                    methodName
                )
#if DEBUG_XMLResponse
                , methodName
#endif
            );
        }
        /// <summary>
        /// Execute DokuWiki command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public T Get<T>(XmlRpcRequest type, params string[] val)
        {
            string methodName = type.ToString();
            return this.Get<T>(
                this._RpcXmlRequestToString<Data.RpcXmlRequestString>(
                    this._CreateRequestFromString(type, methodName, val),
                    methodName
                )
#if DEBUG_XMLResponse
                , methodName
#endif
            );
        }
        /// <summary>
        /// Execute DokuWiki command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
#if DEBUG_XMLResponse
        public T Get<T>(string xmlstr, string methodName = "none")
#else
        public T Get<T>(string xmlstr)
#endif
        {
            try
            {
#if DEBUG_XMLResponse
#if DEBUG_prnXMLResponse
                Console.WriteLine("-----[" + methodName + "]-----");
                Console.WriteLine("--------------------------[Request]---------------------------------");
                Console.WriteLine(xmlstr);
#endif
                File.WriteAllText("request-" + methodName, xmlstr);
#endif
                string xmlout = this._WebGet(Encoding.UTF8.GetBytes(xmlstr));
#if DEBUG_XMLResponse
#if DEBUG_prnXMLResponse
                Console.WriteLine("-------------------------[Response]--------------------------------");
                Console.WriteLine(xmlout);
#endif
                File.WriteAllText("response-" + methodName, xmlout);
#endif
                try
                {
                    Data.XMLMethodErrorResponse xerror = this._RpcXmlStringToObject<Data.XMLMethodErrorResponse>(xmlout);
                    try
                    {
                        if (xerror.Fault.Value.Struct.Member.Count == 2)
                        {
                            throw new RpcXmlException(
                                className + xerror.Fault.Value.Struct.Member[1].Value.String,
                                Int32.Parse(xerror.Fault.Value.Struct.Member[0].Value.Int)
                            );
                        }
                        else
                        {
                            throw new RpcXmlException(
                                className + Properties.ResourceCodeError.rpcErrorInt5094,
                                5094
                            );
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                catch (RpcXmlException e)
                {
                    throw e;
                }
                catch (Exception)
                {
                    return (T)this._RpcXmlStringToObject<T>(xmlout);
                }
            }
            catch (RpcXmlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5007
                );
            }
        }

        #endregion

        #region Private Method

        private void _CheckAuth()
        {
            if (this._isAuth)
            {
                return;
            }
            bool isAuthEmpty = true;
            try
            {
                if (
                    (string.IsNullOrWhiteSpace(_url)) ||
                    (string.IsNullOrWhiteSpace(_login)) ||
                    (string.IsNullOrWhiteSpace(_passwd))
                   )
                {
                    this._isAuth = isAuthEmpty = false;
                    throw new ArgumentException();
                }
                if (!(this._isAuth = this.DokuAuth()))
                {
                    throw new ArgumentException();
                }
            }
            catch (RpcXmlException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new RpcXmlException(
                    string.Format(
                        Properties.ResourceCodeError.rpcErrorIntFormat,
                        className,
                        ((isAuthEmpty) ? "Auth result" : "Login, Password or Url")
                    ),
                    5024
                );
            }
        }

        private Data.RpcXmlRequestString _CreateRequestFromString(XmlRpcRequest type, string methodName, params string[] val)
        {
            try
            {
                Data.RpcXmlRequestString xmlreq = new Data.RpcXmlRequestString();
                xmlreq.Params = new Data.RpcXmlStringParams();
                xmlreq.MethodName = methodName.Replace("_", ".");

                if (val.Length > 0)
                {
                    List<Data.RpcXmlStringValue> eles = new List<Data.RpcXmlStringValue>();

                    foreach (var p in val)
                    {
                        Data.RpcXmlStringValue ele = new Data.RpcXmlStringValue();
                        ele.Value = new Data.RpcXmlStringString();
                        ele.Value.String = p;
                        eles.Add(ele);
                    }
                    xmlreq.Params.Param = eles.ToArray();
                }
                return xmlreq;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5091
                );
            }
        }
        private Data.RpcXmlRequestInt _CreateRequestFromInt(XmlRpcRequest type, string methodName, int n)
        {
            try
            {
                Data.RpcXmlRequestInt xmlreq = new Data.RpcXmlRequestInt();
                xmlreq.Params = new Data.RpcXmlIntParams();
                xmlreq.Params.Param = new Data.RpcXmlIntParam();
                xmlreq.Params.Param.Value = new Data.RpcXmlIntValue();
                xmlreq.Params.Param.Value.Int = n.ToString();
                xmlreq.MethodName = methodName.Replace("_", ".");
                return xmlreq;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5092
                );
            }
        }
        private Data.RpcXmlRequestComposite _CreateRequestFromComposite(XmlRpcRequest type, string methodName, string val, int n)
        {
            try
            {
                Data.RpcXmlRequestComposite xmlreq = new Data.RpcXmlRequestComposite();
                xmlreq.Params = new Data.RpcXmlCompositeParams();
                xmlreq.Params.Param = new List<Data.RpcXmlCompositeParam>();
                xmlreq.MethodName = methodName.Replace("_", ".");

                Data.RpcXmlCompositeParam p = new Data.RpcXmlCompositeParam();
                p.Value = new Data.RpcXmlCompositeValue();
                p.Value.Int = n.ToString();
                p.Value.String = val;
                xmlreq.Params.Param.Add(p);
                return xmlreq;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5093
                );
            }
        }

        private T _RpcXmlStringToObject<T>(string xmlstr)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (TextReader sr = new StringReader(xmlstr))
                {
                    return (T)serializer.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5006
                );
            }
        }

        private string _RpcXmlRequestToString<T>(T xmlreq, string methodName)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings()
                    {
                        Encoding = new UTF8Encoding(false),
                        Indent = true,
                        NewLineOnAttributes = true,
                    })
                    )
                    {
                        serializer.Serialize(xw, xmlreq);
                        return sw.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5005
                );
            }
        }

        private string _WebGet(byte[] data)
        {
            if ((data == null) || (data.Length == 0))
            {
                throw new RpcXmlException(
                    className + Properties.ResourceCodeError.rpcErrorInt5000,
                    5000
                );
            }

            HttpWebResponse res = null;

            try
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this._url);
                    req.Method = "POST";
                    req.ContentLength = data.Length;
                    req.CookieContainer = this._cookie;
                    using (Stream stream = req.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    res = (HttpWebResponse)req.GetResponse();
                }
                catch (WebException e)
                {
                    res = (HttpWebResponse)e.Response;
                    if (res == null)
                    {
                        throw new RpcXmlException(
                            e.Message,
                            5001
                        );
                    }
                }
                catch (Exception e)
                {
                    throw new RpcXmlException(
                        className + e.Message,
                        5002
                    );
                }
                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Moved:
                    case HttpStatusCode.Redirect:
                        {
                            break;
                        }
                    default:
                        {
                            string error;
                            switch ((int)res.StatusCode)
                            {
                                case 401:
                                case 403:
                                case 100:
                                case 110:
                                case 111:
                                case 112:
                                case 113:
                                case 114:
                                case 120:
                                case 121:
                                case 130:
                                case 131:
                                case 132:
                                case 133:
                                case 134:
                                case 210:
                                case 211:
                                case 212:
                                case 215:
                                case 220:
                                case 221:
                                case 230:
                                case 231:
                                case 232:
                                case 233:
                                case 300:
                                case 310:
                                case 311:
                                case 320:
                                case 321:
                                    {
                                        try
                                        {
                                            error = Properties.ResourceCodeError.ResourceManager.GetString("rpcError" + (int)res.StatusCode);
                                        }
                                        catch (Exception)
                                        {
                                            error = res.StatusCode.ToString();
                                        }
                                        break;
                                    }
                                case -32600:
                                case -32601:
                                case -32602:
                                case -32603:
                                case -32604:
                                case -32605:
                                case -32700:
                                case -32800:
                                case -99999:
                                    {
                                        try
                                        {
                                            error = Properties.ResourceCodeError.ResourceManager.GetString("rpcErrorExt" + (int)res.StatusCode); // TODO convert -/+
                                        }
                                        catch (Exception)
                                        {
                                            error = res.StatusCode.ToString();
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        error = res.StatusCode.ToString();
                                        break;
                                    }

                            }
                            throw new RpcXmlException(
                                error,
                                (int)res.StatusCode
                            );
                        }
                }
                using (Stream st = res.GetResponseStream())
                {
                    using (StreamReader rd = new StreamReader(st))
                    {
                        return rd.ReadToEnd();
                    }
                }
            }
            catch (RpcXmlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5004
                );
            }
        }

        #endregion
    }
}
