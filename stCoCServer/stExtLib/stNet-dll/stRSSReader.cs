using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Net;

namespace stNet
{
    public static partial class stRSS
    {
        public class RSSReader : IEnumerable<RSSFeedItem>
        {
            private IEnumerable<RSSFeedItem> _Feeds { get; set; }

            public RSSReader(string url)
            {
                XDocument feed = XDocument.Load(url, LoadOptions.PreserveWhitespace);
                using (StringWriter writer = new UTF8StringWriter())
		{
	           feed.Save(writer, SaveOptions.OmitDuplicateNamespaces);
		}
                this._Feeds = from item in feed.Element("rss").Element("channel").Elements("item")
                              select new RSSFeedItem
                              {
                                  title = WebUtility.HtmlDecode(item.Element("title").Value),
                                  link = WebUtility.HtmlDecode(item.Element("link").Value),
                                  category = WebUtility.HtmlDecode(item.Element("category").Value),
                                  description = WebUtility.HtmlDecode(item.Element("description").Value),
                                  pubdate = DateTime.Parse(item.Element("pubDate").Value)
                              };
            }

            private class UTF8StringWriter : StringWriter
            {
                public override Encoding Encoding { get { return Encoding.UTF8; } }
            }
            public IEnumerator<RSSFeedItem> GetEnumerator()
            {
                return this._Feeds.GetEnumerator();
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
