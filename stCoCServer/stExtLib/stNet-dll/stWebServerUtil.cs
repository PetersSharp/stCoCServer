using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using stCore;
using System.Globalization;
using System.Net;

namespace stNet.stWebServerUtil
{
    public class HtmlTemplate
    {
        private string _templateDirectory = String.Empty;
        private const string _insDataTagDefault = "INSERTHTMLDATA";
        private Dictionary<string, string> _TemplateLocator = null;
        public string InsertFileNotFound = "";

        ///<summary>Html Template add value to HTML format, posible objects:</summary>
        public HtmlTemplate(string tmplDir)
        {
            if (string.IsNullOrWhiteSpace(tmplDir))
            {
                throw new ArgumentException(Properties.Resources.templateDirectoryEmpty);
            }
            if (!Directory.Exists(tmplDir))
            {
                throw new ArgumentException(
                    string.Format(
                        Properties.Resources.templateDirectoryNotFound,
                        tmplDir
                    )
                );
            }
            this._TemplateLocator = new Dictionary<string, string>();
            this._templateDirectory = tmplDir;
            this.InsertFileNotFound = Properties.Resources.templateInsertFileNotFound;
        }
        ~HtmlTemplate()
        {
            if (this._TemplateLocator != null) { this._TemplateLocator.Clear(); }
        }

        public string Render(object objv, string templateFile, string insertFilePath = null, string insertTemplateTag = null)
        {
            string templateThis = String.Empty;
            string insertThis   = String.Empty;

            try
            {
                if (objv == null)
                {
                    throw new ArgumentNullException(Properties.Resources.templateAppendObjEmpty);
                }
                if (string.IsNullOrWhiteSpace(templateFile))
                {
                    throw new ArgumentNullException(Properties.Resources.templateFileNameEmpty);
                }

                templateFile = ((!templateFile.EndsWith(".html")) ?
                    ((!templateFile.Contains(".")) ? templateFile + ".html" : templateFile) :
                    templateFile
                );

                foreach (KeyValuePair<string, string> pair in this._TemplateLocator)
                {
                    if (templateFile.Equals(pair.Key))
                    {
                        templateThis = pair.Value;
                        break;
                    }
                }
                if (string.IsNullOrWhiteSpace(templateThis))
                {
                    string templatePath = Path.Combine(this._templateDirectory, templateFile);
                    if (!File.Exists(templatePath))
                    {
                        throw new ArgumentException(
                            string.Format(
                                Properties.Resources.templateFileNotFound,
                                templatePath
                            )
                        );
                    }
                    else
                    {
                        try
                        {
                            using (var reader = new StreamReader(templatePath))
                            {
                                templateThis = reader.ReadToEnd();
                                this._TemplateLocator.Add(templateFile, templateThis);
                                reader.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            throw new ArgumentException(
                                string.Format(
                                    Properties.Resources.templateFileBusy,
                                    templatePath,
                                    e.Message
                                )
                            );
                        }
                    }
                }
                if ((!string.IsNullOrWhiteSpace(insertFilePath)) && (!File.Exists(insertFilePath)))
                {
                    insertThis = string.Format(
                        this.InsertFileNotFound,
                        DateTime.Now.ToString(),
                        "(Not found)", ""
                    );
                }
                else
                {
                    try
                    {
                        using (var reader = new StreamReader(insertFilePath))
                        {
                            insertThis = reader.ReadToEnd();
                            reader.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        insertThis = string.Format(
                            this.InsertFileNotFound,
                            DateTime.Now.ToString(),
                            e.Message, ""
                        );
                    }
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
            return this._Render(objv, templateThis, insertThis, insertTemplateTag);
        }

        private string _Render(object objv, string template, string insert, string tag)
        {
            string output = template;
            foreach (var p in objv.GetType().GetProperties())
            {
                output = output.Replace("{{" + p.Name + "}}", (p.GetValue(objv, null) as string) ?? string.Empty);
            }
            if (!string.IsNullOrWhiteSpace(insert))
            {
                output = output.Replace(
                    "{{" + 
                    ((string.IsNullOrWhiteSpace(tag)) ? _insDataTagDefault : tag)
                    + "}}",
                    insert
                );
            }
            return output;
        }
    }

    public class HttpUtil
    {
        public enum MimeType : int
        {
            MimePng = 0,
            MimeGif,
            MimeJpg,
            MimeSvg,
            MimeIco,
            MimeText,
            MimeHtml,
            MimeXml,
            MimeCss,
            MimeSse,
            MimeJs,
            MimeJson,
            MimeFontWoff,
            MimeFontTtf,
            MimeFontEot,
            MimeStream,
            MimeAddUTF8,
            MimeNone
        }
        private static readonly string[] MimeString = 
            {
                @"image/png",
                @"image/gif",
                @"image/jpeg",
                @"image/svg+xml",
                @"image/x-icon",
                @"text/plain",
                @"text/html",
                @"text/xml",
                @"text/css",
                @"text/event-stream",
                @"application/javascript",
                @"application/json",
                @"application/font-woff",
                @"application/font-sfnt",
                @"application/vnd.ms-fontobject",
                @"application/octet-stream",
                @"; charset=utf-8"
            };

        ///<summary>
        ///Reurn given URL Mime type to Add HTTP response Header "Content-Type"
        ///</summary>
        public static string GetMimeType(string url, HttpUtil.MimeType etype = HttpUtil.MimeType.MimeNone)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                if (etype == HttpUtil.MimeType.MimeNone)
                {
                    return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeStream];
                }
                url = "";
            }
            if ((etype == HttpUtil.MimeType.MimePng) || ((url.Length > 3) && (url.EndsWith(".png"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimePng];
            }
            else if ((etype == HttpUtil.MimeType.MimeGif) || ((url.Length > 3) && (url.EndsWith(".gif"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeGif];
            }
            else if ((etype == HttpUtil.MimeType.MimeJpg) || ((url.Length > 3) && ((url.EndsWith(".jpg")) || (url.EndsWith(".jpeg")))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeJpg];
            }
            else if ((etype == HttpUtil.MimeType.MimeSvg) || ((url.Length > 3) && (url.EndsWith(".svg"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeSvg];
            }
            else if ((etype == HttpUtil.MimeType.MimeIco) || ((url.Length > 3) && (url.EndsWith(".ico"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeIco];
            }
            else if ((etype == HttpUtil.MimeType.MimeText) || ((url.Length > 3) && ((url.EndsWith(".txt")) || (url.EndsWith(".text")) || (url.EndsWith(".md")))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeText] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if ((etype == HttpUtil.MimeType.MimeHtml) || ((url.Length > 3) && ((url.EndsWith(".htm")) || (url.EndsWith(".html")))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeHtml] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if ((etype == HttpUtil.MimeType.MimeXml) || ((url.Length > 3) && ((url.EndsWith(".xml")) || (url.EndsWith(".rss")))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeXml] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if ((etype == HttpUtil.MimeType.MimeCss) || ((url.Length > 3) && (url.EndsWith(".css"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeCss] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if (etype == HttpUtil.MimeType.MimeSse)
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeSse];
            }
            else if ((etype == HttpUtil.MimeType.MimeJs) || ((url.Length > 3) && (url.EndsWith(".js"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeJs] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if ((etype == HttpUtil.MimeType.MimeJson) || ((url.Length > 3) && (url.EndsWith(".json"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeJson] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if ((etype == HttpUtil.MimeType.MimeFontWoff) || ((url.Length > 3) && ((url.EndsWith(".woff")) || (url.EndsWith(".woff2"))) ))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeJson] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if ((etype == HttpUtil.MimeType.MimeFontTtf) || ((url.Length > 3) && (url.EndsWith(".ttf"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeJson] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else if ((etype == HttpUtil.MimeType.MimeFontEot) || ((url.Length > 3) && (url.EndsWith(".eot"))))
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeJson] + HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeAddUTF8];
            }
            else
            {
                return HttpUtil.MimeString[(int)HttpUtil.MimeType.MimeStream];
            }
        }

        ///<summary>Reurn given HTTP User Agent string (Client/Server)</summary>
        public static string GetHttpUA(string restype = null)
        {
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return string.Format(
                    Properties.Resources.httpUA,
                    ((string.IsNullOrWhiteSpace(restype)) ? "CS" : restype),
                    ver.ProductMajorPart,
                    ver.ProductMinorPart,
                    Environment.OSVersion.VersionString,
                    Environment.Version.ToString(),
                    Environment.OSVersion.Platform.ToString()
            );
        }

        ///<summary>Reurn given HTTP prefered language string (Server)</summary>
        public static string GetHttpPreferedLanguage(string [] langs = null)
        {
            if ((langs == null) || (langs.Length == 0))
            {
                return String.Empty;
            }
            return langs.First();
        }

        ///<summary>Reurn given HTTP Request.UserLanguages or Defalt language CultureInfo (Server)</summary>
        public static CultureInfo GetHttpClientLanguage(string[] langs, CultureInfo ciDefault)
        {
            string lang = HttpUtil.GetHttpPreferedLanguage(langs);
            return HttpUtil.GetHttpClientLanguage(lang, ciDefault);
        }

        ///<summary>Reurn client IP from HTTP Request (Server)</summary>
        public static IPAddress GetHttpClientIP(HttpListenerRequest req)
        {
            string clip = req.Headers["X-Real-IP"];
            if (!string.IsNullOrWhiteSpace(clip))
            {
                return IPAddress.Parse(clip);
            }
            return req.RemoteEndPoint.Address;
        }

        ///<summary>Reurn given string or Defalt language CultureInfo (Server)</summary>
        public static CultureInfo GetHttpClientLanguage(string lang, CultureInfo ciDefault)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(lang))
                {
                    try
                    {
                        return CultureInfo.GetCultureInfo(lang);
                    }
                    catch (Exception)
                    {
                        throw new ArgumentNullException();
                    }
                }
            }
            catch (Exception) { }
            return ((ciDefault == null) ? CultureInfo.GetCultureInfo("en") : ciDefault);
        }

        ///<summary>Print object dump in HTML format, posible objects:
        ///            HttpListenerContext,
        ///            HttpListenerRequest,
        ///            HttpListenerResponse
        ///</summary>
        public static string ObjectDump(object o, StringBuilder sb = null, bool html = true)
        {
            bool closeSb = false;

            if (sb == null)
            {
                sb = new StringBuilder();
                closeSb = true;
            }
            if (html) { sb.Append("<ul>"); }
            if (o is string || o is int || o is long || o is double)
            {
                if (html) { sb.Append("<li>"); }
                sb.Append(o.ToString());
                if (html) { sb.Append("</li>"); }
            }
            else
            {
                Type t = o.GetType();
                foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    sb.Append("<li><b>" + p.Name + ":</b> ");
                    object val = null;

                    try
                    {
                        val = p.GetValue(o, null);
                    }
                    catch { }

                    if (val is string || val is int || val is long || val is double)
                    {
                        sb.Append(val);
                    }
                    else
                    {
                        if (val != null)
                        {
                            Array arr = val as Array;
                            if (arr == null)
                            {
                                NameValueCollection nv = val as NameValueCollection;
                                if (nv == null)
                                {
                                    IEnumerable ie = val as IEnumerable;
                                    if (ie == null)
                                    {
                                        sb.Append(val.ToString());
                                    }
                                    else
                                    {
                                        foreach (object oo in ie) { HttpUtil.ObjectDump(oo, sb); }
                                    }
                                }
                                else
                                {
                                    sb.Append("<ul>");
                                    foreach (string key in nv.AllKeys)
                                    {
                                        sb.AppendFormat("<li>{0} = ", key);
                                        HttpUtil.ObjectDump(nv[key], sb, false);
                                        sb.Append("</li>");
                                    }
                                    sb.Append("</ul>");
                                }
                            }
                            else
                            {
                                foreach (object oo in arr) { HttpUtil.ObjectDump(oo, sb); }
                            }
                        }
                        else
                        {
                            sb.Append("<i>null</i>");
                        }
                        sb.Append("</li>");
                    } // endif
                } //end foreach
                if (html) { sb.Append("</ul>"); }
            }
            if (closeSb)
            {
                string rstr = sb.ToString();
                sb.Clear();
                return rstr;
            }
            return sb.ToString();
        }
    }
    public class stBootStrap : IDisposable
    {
        private const string listFile = @"concat.list.*";
        private const string outFile = @"concat";
        private IMessage _iLog = new IMessage();

        private bool _isConcat = true;
        private bool _isMinify = true;

        /// <summary>
        /// Enable concat body in one file
        /// </summary>
        public bool isConcat
        {
            get { return this._isConcat; }
            set { this._isConcat = value; }
        }
        /// <summary>
        /// Enable minified source files
        /// </summary>
        public bool isMinify
        {
            get { return this._isMinify; }
            set { this._isMinify = value; }
        }

        /// <summary>
        /// Constructor - does all CSS/JS minify the processing in directory,
        ///     search and read concat.list.(*),
        ///     create concat.min.(extension) in same directory
        /// </summary>
        /// <param name="isconcat">Enable concat source</param>
        /// <param name="isminify">Enable minify source</param>
        /// <param name="logError">log error print function (Action)</param>
        /// <param name="logInfo">log information print function (Action)</param>
        public stBootStrap(bool isconcat, bool isminify, IMessage ilog = null)
        {
            this._Init(isconcat, isminify, ilog);
        }
        /// <summary>
        /// Constructor - see same full method
        /// </summary>
        public stBootStrap(IMessage ilog = null)
        {
            this._Init(true, true, ilog);
        }
        private void _Init(bool isconcat, bool isminify, IMessage ilog)
        {
            if (ilog != null)
            {
                this._iLog = ilog;
            }
            this._isConcat = isconcat;
            this._isMinify = isminify;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Scan direcory and minified JS/CSS source
        /// </summary>
        /// <param name="fpath">Full directory path to search minify JS/CSS source</param>
        public void Minify(string fpath)
        {
            if ((!this._isConcat) && (!this._isMinify))
            {
                this._iLog.LogError(
                    string.Format(
                        Properties.Resources.httpUtilMinifyError,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Properties.Resources.httpMinifyConcatDisabled, "return"
                    )
                );
                return;
            }
            foreach (string lstfn in Directory.GetFiles(fpath, listFile, System.IO.SearchOption.AllDirectories))
            {
                string fnr = String.Empty;
                string fnw = String.Empty;
                StreamReader sr = null;
                StreamWriter sw = null;

                try
                {
                    if ((sr = File.OpenText(lstfn)) == null)
                    {
                        this._iLog.LogError(
                            string.Format(
                                Properties.Resources.httpUtilMinifyError,
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                Properties.Resources.httpMinifyStreamReadNull, ""
                            )
                        );
                        continue;
                    }
                    fnw = Path.Combine(
                        Path.GetDirectoryName(lstfn),
                        ((this._isConcat) ? 
                            ((this._isMinify) ? 
                                outFile + ".min" + Path.GetExtension(lstfn) :
                                outFile + Path.GetExtension(lstfn)
                            ) :
                            ((this._isMinify) ? 
                                Path.GetFileNameWithoutExtension(lstfn) + ".min" + Path.GetExtension(lstfn) :
                                Path.GetFileNameWithoutExtension(lstfn) + ".orig" + Path.GetExtension(lstfn)
                            )
                        )
                    );

                    if ((sw = new StreamWriter(fnw, false, Encoding.UTF8)) == null)
                    {
                        this._iLog.LogError(
                            string.Format(
                                Properties.Resources.httpUtilMinifyError,
                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                Properties.Resources.httpMinifyStreamOutNull, fnw
                            )
                        );
                        continue;
                    }
                    while ((fnr = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(fnr))
                        {
                            string srcText = String.Empty;
                            string pathFrom = Path.Combine(Path.GetDirectoryName(lstfn), fnr.Trim());
                            if (File.Exists(pathFrom))
                            {
                                if ((this._isMinify) && (!fnr.Contains(".min.")) && (fnr.EndsWith(".js")))
                                {
                                    this._iLog.LogInfo(
                                        string.Format(
                                            Properties.Resources.httpMinifyJShead,
                                            Path.GetFileName(pathFrom)
                                        )
                                    );
                                    srcText = this._Minify<string>(pathFrom, this._doProcessJS);
                                }
                                else if ((this._isMinify) && (!fnr.Contains(".min.")) && (fnr.EndsWith(".css")))
                                {
                                    this._iLog.LogInfo(
                                        string.Format(
                                            Properties.Resources.httpMinifyCSShead,
                                            Path.GetFileName(pathFrom)
                                        )
                                    );
                                    srcText = this._Minify<string>(pathFrom, this._doProcessCSS);
                                }
                                else if ((!this._isConcat) && (fnr.Contains(".min.")))
                                {
                                    continue;
                                }
                                else
                                {
                                    this._iLog.LogInfo(
                                        string.Format(
                                            Properties.Resources.httpMinifyOriginhead,
                                            Path.GetFileName(pathFrom)
                                        )
                                    );
                                    try
                                    {
                                        srcText = System.IO.File.ReadAllText(pathFrom, Encoding.UTF8);
                                    }
                                    catch (Exception e)
                                    {
                                        this._iLog.LogError(
                                            string.Format(
                                                Properties.Resources.httpUtilMinifyError,
                                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                Properties.Resources.httpMinifyStreamOrigin,
                                                e.Message
                                            )
                                        );
                                        continue;
                                    }
                                }
                                if (!string.IsNullOrWhiteSpace(srcText))
                                {
                                    try
                                    {
                                        sw.Write(srcText);
                                    }
                                    catch (Exception e)
                                    {
                                        this._iLog.LogError(
                                            string.Format(
                                                Properties.Resources.httpUtilMinifyError,
                                                System.Reflection.MethodBase.GetCurrentMethod().Name,
                                                Properties.Resources.httpMinifyStreamOut,
                                                e.Message
                                            )
                                        );
                                    }
                                }
                            }
                        }
                        fnr = String.Empty;
                    }
                }
                catch (Exception e)
                {
                    this._iLog.LogError(
                        string.Format(
                            Properties.Resources.httpUtilMinifyError,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            Properties.Resources.httpMinifyStreamRead,
                            e.Message
                        )
                    );
                    continue;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();
                        sr.Dispose();
                    }
                    sr = null;
                    if (sw != null)
                    {
                        sw.Close();
                        sw.Dispose();
                    }
                    sw = null;
                }
            }
        }
        /// <summary>
        /// Minified CSS routine - CSS minify this processing
        /// </summary>
        /// <param name="fpath">Full path to minify CSS source</param>
        public string MinifyCSS(string fpath)
        {
            return this._Minify<string>(fpath, this._doProcessCSS);
        }
        /// <summary>
        /// Minified JS routine - JS minify this processing
        /// </summary>
        /// <param name="fpath">Full path to minify JS source</param>
        public string MinifyJS(string fpath)
        {
            return this._Minify<string>(fpath, this._doProcessJS);
        }
        private TResult _Minify<TResult>(string fpath, Func<BinaryReader, TResult> act)
        {
            if ((string.IsNullOrWhiteSpace(fpath)) || (!File.Exists(fpath)))
            {
                throw new ArgumentException(
                    string.Format(
                        Properties.Resources.httpUtilMinifyFileNotFound,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        fpath
                    )
                );
            }
            try
            {
                return act(
                    new BinaryReader(new FileStream(fpath, FileMode.Open),Encoding.UTF8)
                );
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        /// <summary>
        /// Main CSS minify process
        /// </summary>
        private string _doProcessCSS(BinaryReader rd = null)
        {
            int lastChar = 1;                   // current byte read
            int thisChar = -1;                  // previous byte read
            int nextChar = -1;                  // byte read in peek()
            bool endProcess = false;            // loop control
            bool ignore = false;                // if false then add byte to final output
            bool inComment = false;             // true when current bytes are part of a comment
            bool isDoubleSlashComment = false;  // '//' comment
            string MinifiedData = "";

            if (rd == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                        Properties.Resources.httpUtilMinifyError,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Properties.Resources.httpMinifyStreamBinary, "CSS"
                    )
                );
            }
            while (!endProcess)
            {
                if ((endProcess = (rd.PeekChar() == -1)))
                {
                    break;
                }
                ignore = false;
                thisChar = rd.ReadByte();

                if (thisChar == '\t')
                {
                    thisChar = ' ';
                }
                else if (thisChar == '\t')
                {
                    thisChar = '\n';
                }
                else if (thisChar == '\r')
                {
                    thisChar = '\n';
                }
                if (thisChar == '\n')
                {
                    ignore = true;
                }
                if (thisChar == ' ')
                {
                    if ((lastChar == ' ') || (isDelimiter(lastChar, false)))
                    {
                        ignore = true;
                    }
                    else
                    {
                        if (!(endProcess = (rd.PeekChar() == -1)))
                        {
                            nextChar = rd.PeekChar();
                            if (isDelimiter(nextChar, false))
                            {
                                ignore = true;
                            }
                        }
                    }
                }
                if (thisChar == '/')
                {
                    nextChar = rd.PeekChar();
                    if (nextChar == '/' || nextChar == '*')
                    {
                        ignore = true;
                        inComment = true;
                        if (nextChar == '/')
                        {
                            isDoubleSlashComment = true;
                        }
                        else
                        {
                            isDoubleSlashComment = false;
                        }
                    }
                    if (nextChar == '/')
                    {
                        int x = 0;
                        x = x + 1;
                    }
                }
                if (inComment)
                {
                    while (true)
                    {
                        thisChar = rd.ReadByte();
                        if (thisChar == '*')
                        {
                            nextChar = rd.PeekChar();
                            if (nextChar == '/')
                            {
                                thisChar = rd.ReadByte();
                                inComment = false;
                                break;
                            }
                        }
                        if (isDoubleSlashComment && thisChar == '\n')
                        {
                            inComment = false;
                            break;
                        }
                    }
                    ignore = true;
                }
                if (!ignore)
                {
                    MinifiedData += (char)thisChar;
                }
                lastChar = thisChar;
            }
            if (rd != null)
            {
                rd.Close();
            }
            return MinifiedData.Trim();
        }
        /// <summary>
        /// Main JS minify process
        /// </summary>
        private string _doProcessJS(BinaryReader rd = null)
        {
            int lastChar = 1;                   // current byte read
            int thisChar = -1;                  // previous byte read
            int nextChar = -1;                  // byte read in peek()
            bool endProcess = false;            // loop control
            bool ignore = false;                // if false then add byte to final output
            bool inComment = false;             // true when current bytes are part of a comment
            bool isDoubleSlashComment = false;  // '//' comment
            string MinifiedData = "";

            if (rd == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                        Properties.Resources.httpUtilMinifyError,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Properties.Resources.httpMinifyStreamBinary, "JS"
                    )
                );
            }
            while (!endProcess)
            {
                if ((endProcess = (rd.PeekChar() == -1)))
                {
                    break;
                }

                ignore = false;
                thisChar = rd.ReadByte();

                if (thisChar == '\t')
                {
                    thisChar = ' ';
                }
                else if (thisChar == '\t')
                {
                    thisChar = '\n';
                }
                else if (thisChar == '\r')
                {
                    thisChar = '\n';
                }
                if (thisChar == '\n')
                {
                    ignore = true;
                }
                if (thisChar == ' ')
                {
                    if ((lastChar == ' ') || (isDelimiter(lastChar, true)))
                    {
                        ignore = true;
                    }
                    else
                    {
                        endProcess = (rd.PeekChar() == -1);
                        if (!endProcess)
                        {
                            nextChar = rd.PeekChar();
                            if (isDelimiter(nextChar, true))
                            {
                                ignore = true;
                            }
                        }
                    }
                }
                if (thisChar == '/')
                {
                    nextChar = rd.PeekChar();
                    if (nextChar == '/' || nextChar == '*')
                    {
                        ignore = true;
                        inComment = true;
                        if (nextChar == '/')
                        {
                            isDoubleSlashComment = true;
                        }
                        else
                        {
                            isDoubleSlashComment = false;
                        }
                    }
                }
                if (inComment)
                {
                    while (true)
                    {
                        thisChar = rd.ReadByte();
                        if (thisChar == '*')
                        {
                            nextChar = rd.PeekChar();
                            if (nextChar == '/')
                            {
                                thisChar = rd.ReadByte();
                                inComment = false;
                                break;
                            }
                        }
                        if ((isDoubleSlashComment) && (thisChar == '\n'))
                        {
                            inComment = false;
                            break;
                        }
                    }
                    ignore = true;
                }
                if (!ignore)
                {
                    MinifiedData += (char)thisChar;
                }
                lastChar = thisChar;
            }
            if (rd != null)
            {
                rd.Close();
            }
            return MinifiedData.Trim();
        }
        /// <summary>
        /// Check if a byte is a delimiter 
        /// </summary>
        /// <param name="c">byte to check</param>
        /// <returns>retval - 1 if yes. else 0</returns>
        private bool isDelimiter(int c, bool jsource)
        {
            if (c == '(' || c == ',' || c == '=' || c == ':' ||
                c == '[' || c == '!' || c == '&' || c == '|' ||
                c == '?' || c == '+' || c == '~' || c == ';' ||
                c == '*' || c == '/' || c == '{' || c == '\n'
               )
            {
                return true;
            }
            if ((jsource) && (c == '-'))
            {
                return true;
            }
            return false;
        }
    }

}
