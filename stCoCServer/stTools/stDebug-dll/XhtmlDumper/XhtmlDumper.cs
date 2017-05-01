﻿/*
 * Original code from Lukas Bühler: https://github.com/lukebuehler/XhtmlDumper
 * LINQPad like XHTML dumping method.
 * 
 * Dumps the contents of .NET objects in Xhtml format.
 * XhtmlDumper is intended to provide similar features as the LINQPads Dump() method.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using stDebug.Properties;
using System.Reflection;

namespace stDebug.Dumper
{
    public class XhtmlDumper : IDisposable
    {
        private readonly XhtmlTextWriter writer;
        private readonly IXhtmlRenderer[] renderers;

        public XhtmlDumper(TextWriter writer)
        {
            this.writer = new XhtmlTextWriter(writer); ;
            this.renderers = new IXhtmlRenderer[] { new ObjectXhtmlRenderer(), new BasicXhtmlRenderer() };
            InitHeader();
        }

        public XhtmlDumper(TextWriter writer, params IXhtmlRenderer[] renderers)
        {
            this.writer = new XhtmlTextWriter(writer);
            this.renderers = renderers;
            InitHeader();
        }

        /// <summary>
        /// Object Dumper
        /// </summary>
        /// <code>
        /// stDebug.Dumper.XhtmlDumper.Dump<TtypeObject>(obj);
        /// </code>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="parent"></param>
        public static void Dump<T>(T obj, int parent = 1)
        {
            string objName = obj.GetType().Name,
                   htmlName = Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        string.Concat("dump_", objName, ".html")
                   );
            using (var s = new StreamWriter(htmlName, false))
            {
                using (var dumper = new stDebug.Dumper.XhtmlDumper(s))
                {
                    dumper.WriteObject(obj, objName, ((parent < 1) ? 1 : parent));
                }
            }
            try
            {
                Console.WriteLine("*** Create report: \"" + htmlName + "\"");
                System.Diagnostics.Process.Start("chrome.exe", "\"" + htmlName + "\"");
            }
            catch (Exception e) { Console.WriteLine("*** Create report: " + e.GetType().Name + " -> " + e.Message); }
        }

        private void InitHeader()
        {
            writer.WriteLineNoTabs("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
            writer.AddAttribute("xmlns", "http://www.w3.org/1999/xhtml");
            writer.AddAttribute("xml:lang", "en");
            writer.RenderBeginTag(HtmlTextWriterTag.Html);
            writer.RenderBeginTag(HtmlTextWriterTag.Head);

            writer.RenderBeginTag(HtmlTextWriterTag.Title);
            writer.Write("XhtmlDumper");
            writer.RenderEndTag();


            writer.AddAttribute("http-equiv", "content-type");
            writer.AddAttribute(HtmlTextWriterAttribute.Content, "text/html;charset=utf-8");
            writer.RenderBeginTag(HtmlTextWriterTag.Meta);
            writer.RenderEndTag();
            writer.WriteLine();

            writer.AddAttribute(HtmlTextWriterAttribute.Name, "generator");
            writer.AddAttribute(HtmlTextWriterAttribute.Content, "XhtmlDumper");
            writer.RenderBeginTag(HtmlTextWriterTag.Meta);
            writer.RenderEndTag();
            writer.WriteLine();

            writer.AddAttribute(HtmlTextWriterAttribute.Name, "description");
            writer.AddAttribute(HtmlTextWriterAttribute.Content, "Generated on: " + DateTime.Now);
            writer.RenderBeginTag(HtmlTextWriterTag.Meta);
            writer.RenderEndTag();
            writer.WriteLine();

            writer.AddAttribute("type", "text/css");
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.WriteLineNoTabs(Resources.StyleSheet);
            writer.RenderEndTag(); // style

            writer.RenderEndTag(); // Head
            writer.WriteLine();

            writer.RenderBeginTag(HtmlTextWriterTag.Body);
        }


        public void WriteObject(object o, string description, int depth)
        {
            //try to loop through all renderes to see if one will render the object,
            // otherwise fallback on textwriter
            if(!renderers.Any(xhtmlRenderer => xhtmlRenderer.Render(o, description, depth, writer)))
                writer.Write(o);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                writer.RenderEndTag(); // body
                writer.RenderEndTag(); // html
                foreach (var xhtmlRenderer in renderers)
                {
                    var disposableXhtmlRenderer = xhtmlRenderer as IDisposable;
                    if (disposableXhtmlRenderer != null)
                        disposableXhtmlRenderer.Dispose();
                }
            }
        }

    }
}
