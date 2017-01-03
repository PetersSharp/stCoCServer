using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel;

using stDokuWiki.WikiEngine;

namespace stDokuWiki.WikiSyntax
{
    /// <summary>
    /// Convert DokuWiki format and syntax
    /// </summary>
    /// <example>
    /// using stDokuWiki.WikiSyntax;
    ///   string htmldoc = "&lt;html&gt;&lt;head&gt;&lt;/head&gt;&lt;body&gt;...&lt;/body&gt;&lt;/html&gt;";
    ///   string htmltxt = "&lt;h3&gt;...&lt;/h3&gt;&lt;br/&gt;&lt;i&gt;...&lt;/i&gt;&lt;br/&gt;";
    ///   string mdtxt   = "== test header ==\n **bold** text and //italic// and __underline__\n";
    ///   DokuWikiFormat dwf = new DokuWikiFormat();
    ///   dwf.OnProcessError += (o, e) =&gt;
    ///   {
    ///      Console.WriteLine(e.ex.GetType().Name + ": " + e.ex.Message);
    ///   };
    ///   Console.WriteLine(
    ///      dwf.HtmlDocToDokuWiki(htmldoc)
    ///   );
    ///   Console.WriteLine(
    ///      dwf.HtmlTextToDokuWiki(htmltxt)
    ///   );
    ///   Console.WriteLine(
    ///      dwf.DokuWikiToHtml(mdtxt)
    ///   );
    /// </example>
    public class DokuWikiFormat
    {
        #region Variables

        #region Constant

        private const string xslNameSpace = "stDokuWiki.";
        private const string xslToMd = "DokuWikiSyntaxToMd.xsl";
        private const string xslToHtml = "DokuWikiSyntaxToHtml.xsl";
        private const string xhtmlDocHtmlTemplate = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n{0}\n</html>";
        private const string xhtmlTextHtmlTemplate = "<head>\n<meta charset=\"UTF-8\"/>\n</head>\n<body>\n{0}\n</body>";
        private const string xhtmlTextMdTemplate = "<text>\n{0}\n</text>";
        
        #endregion

        private XslCompiledTransform xslTransformToMd = null,
                                     xslTransformToHtml = null;
        private XmlReader xsltReaderToMd = null,
                          xsltReaderToHtml = null;
        private List<string> listPattern = new List<string>()
        {
            "<!DOCTYPE([^>]*)>",
            "</?html([^>]*)>"
        };

        /// <summary>
        /// Enable convert DokuWiki markdown format to Html Document
        /// (readonly) set in Constructor <see cref="WikiSyntax.DokuWikiFormat(bool,bool)"/>
        /// </summary>
        public bool IsToHtml { get; private set; }
        /// <summary>
        /// Enable convert Html Document to DokuWiki markdown format
        /// (readonly) set in Constructor <see cref="WikiSyntax.DokuWikiFormat(bool,bool)"/>
        /// </summary>
        public bool IsToMd { get; private set; }

        /// <summary>
        /// External action to convert Html -&gt; Md
        /// </summary>
        public Func<string, string> ActionToMd { get; set; }
        /// <summary>
        /// External action to convert Md -&gt; Html
        /// </summary>
        public Func<string, string> ActionToHtml { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Event DokuWikiFormat Error, return <see cref="WikiEngine.WikiSyntaxErrorEventArgs"/>WikiEngine.WikiSyntaxErrorEventArgs
        /// </summary>
        public event EventHandler<WikiSyntaxErrorEventArgs> OnProcessError = delegate { };

        private AsyncOperation op;

        private void Fire_ProcessError(WikiSyntaxErrorEventArgs o)
        {
            op.Post(x => OnProcessError(this, (WikiSyntaxErrorEventArgs)x), o);
        }

        #endregion

        /// <summary>
        /// Init DokuWikiFormat constructor
        /// </summary>
        /// <param name="isToMd">Bolean - enable/disable convert Html to DokuWiki format</param>
        /// <param name="isToHtml">Bolean - enable/disable convert DokuWiki to Html format</param>
        public DokuWikiFormat(bool isToMd = true, bool isToHtml = true)
        {
            this.IsToMd = isToMd;
            this.IsToHtml = isToHtml;
            this.ActionToMd = this.ActionToHtml = null;
            this.op = AsyncOperationManager.CreateOperation(null);
            DokuXmlUrlResolver xmlUrlResolver = new DokuXmlUrlResolver();
            try
            {
                if (this.IsToMd)
                {
                    xsltReaderToMd = XmlReader.Create(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            string.Concat(xslNameSpace, xslToMd)
                        )
                    );
                    xslTransformToMd = new XslCompiledTransform(true);
                    xslTransformToMd.Load(xsltReaderToMd, new XsltSettings(false, true), xmlUrlResolver);
                }
                if (this.IsToHtml)
                {
                    xsltReaderToHtml = XmlReader.Create(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream(
                            string.Concat(xslNameSpace, xslToHtml)
                        )
                    );
                    xslTransformToHtml = new XslCompiledTransform(true);
                    xslTransformToHtml.Load(xsltReaderToHtml, new XsltSettings(false, true), xmlUrlResolver);
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiSyntaxErrorEventArgs(e));
            }
        }
        /// <summary>
        /// Html Document to DokuWiki markdown format
        /// </summary>
        /// <param name="html">Html dokument source</param>
        /// <returns>String markdown DokuWiki</returns>
        public string HtmlDocToDokuWiki(string html)
        {
            if (this.ActionToMd != null)
            {
                return this._DokuWikiExternal(html, this.ActionToMd);
            }
            listPattern.ForEach(o => 
            {
                html = Regex.Replace(html, o, String.Empty);
            });
            return _HtmlToDokuWiki(
                string.Format(
                    DokuWikiFormat.xhtmlDocHtmlTemplate,
                    html
                )
            );
        }
        /// <summary>
        /// Html text to DokuWiki markdown format
        /// </summary>
        /// <param name="html">Html text source</param>
        /// <returns>String markdown DokuWiki</returns>
        public string HtmlTextToDokuWiki(string html)
        {
            if (this.ActionToMd != null)
            {
                return this._DokuWikiExternal(html, this.ActionToMd);
            }
            return _HtmlToDokuWiki(
                string.Format(
                    DokuWikiFormat.xhtmlDocHtmlTemplate,
                    string.Format(
                        DokuWikiFormat.xhtmlTextHtmlTemplate,
                        html
                    )
                )
            );
        }
        /// <summary>
        /// XHtml to DokuWiki markdown format
        /// </summary>
        /// <param name="xhtml">XHtml complette source</param>
        /// <returns>String markdown DokuWiki</returns>
        public string XHtmlToDokuWiki(string xhtml)
        {
            return _HtmlToDokuWiki(xhtml);
        }

        /// <summary>
        /// DokuWiki markdown text to Html format
        /// </summary>
        /// <param name="mdtext">DokuWiki markdown text source</param>
        /// <returns>String Html</returns>
        public string DokuWikiToHtml(string mdtext)
        {
            if (this.ActionToHtml != null)
            {
                return this._DokuWikiExternal(mdtext, this.ActionToHtml);
            }
            return _DokuWikiToHtml(
                string.Format(
                    DokuWikiFormat.xhtmlTextMdTemplate,
                    mdtext
                )
            );
        }

        private string _HtmlToDokuWiki(string xhtml)
        {
            if (!this.IsToMd)
            {
                Exception e = new Exception(
                    Properties.ResourceWikiEngine.txtDisableHtmlToDokuWiki
                );
                this.Fire_ProcessError(new WikiSyntaxErrorEventArgs(e));
                return String.Empty;
            }
            try
            {
                using (StringWriter sWriter = new StringWriter())
                using (StringReader sReader = new StringReader(xhtml))
                using (XmlReader xReader = XmlReader.Create(sReader))
                {
                    this.xslTransformToMd.Transform(xReader, null, sWriter);
                    return sWriter.ToString();
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiSyntaxErrorEventArgs(e));
                return String.Empty;
            }
        }
        private string _DokuWikiToHtml(string xhtml)
        {
            if (!this.IsToHtml)
            {
                Exception e = new Exception(
                    Properties.ResourceWikiEngine.txtDisableDokuWikiToHtml
                );
                this.Fire_ProcessError(new WikiSyntaxErrorEventArgs(e));
                return String.Empty;
            }
            try
            {
                using (StringWriter sWriter = new StringWriter())
                using (StringReader sReader = new StringReader(xhtml))
                using (XmlReader xReader = XmlReader.Create(sReader))
                {
                    this.xslTransformToHtml.Transform(xReader, null, sWriter);
                    return sWriter.ToString();
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiSyntaxErrorEventArgs(e));
                return String.Empty;
            }
        }
        private string _DokuWikiExternal(string src, Func<string,string> act)
        {
            try
            {
                return act(src);
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiSyntaxErrorEventArgs(e));
                return String.Empty;
            }
        }
        private class DokuXmlUrlResolver : XmlUrlResolver
        {
            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                switch (absoluteUri.Scheme)
                {
                    case "file":
                        {
                            return Assembly.GetExecutingAssembly().GetManifestResourceStream(
                                string.Concat(xslNameSpace, absoluteUri.OriginalString)
                            );
                        }
                    default:
                        {
                            return (Stream)base.GetEntity(absoluteUri, role, ofObjectToReturn);
                        }
                }
            }
        }
    }
}
