using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace stNet
{
    [XmlRoot("rss")]
    public class RSSFeedRoot
    {
        [XmlAttribute]
        public string version = "2.0";
        public RSSFeedChannel channel = null;
    }

    [XmlRoot("channel")]
    public class RSSFeedChannel
    {
        public string title;
        public string link;
        public string description;
        public string copyright;
        [XmlElement]
        public List<RSSFeedItem> items = new List<RSSFeedItem>();
    }

    public class RSSFeedItem
    {
        public string title { get; set; }
        public string link { get; set; }
        public string guid { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public DateTime pubdate { get; set; }
    }
}
