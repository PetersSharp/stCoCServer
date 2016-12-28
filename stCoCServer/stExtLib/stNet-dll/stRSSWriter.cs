using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace stNet
{
    public static partial class stRSS
    {
        public class RSSWriter
        {
            private RSSFeedRoot _rss = null;

            public RSSWriter(RSSFeedChannel rsschannel)
            {
                this._rss = new RSSFeedRoot();
                this._rss.channel = ((rsschannel == null) ? new RSSFeedChannel() : rsschannel);
            }
            ~RSSWriter()
            {
                this.Clear();
            }
            public void Add(RSSFeedItem rssitem)
            {
                if (rssitem != null)
                {
                    this._rss.channel.items.Add(rssitem);
                }
            }
            public void Clear()
            {
                if (this._rss != null)
                {
                    this._rss.channel.items.Clear();
                }
            }
            public bool WriteStream(Stream stm, stCore.IMessage iLog = null)
            {
                if (this._rss == null)
                {
                    return false;
                }
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(RSSFeedRoot));
                    serializer.Serialize(stm, this._rss);
                    return true;
                }
                catch (Exception e)
                {
                    if (iLog != null)
                    {
                        iLog.LogError(e.Message);
                    }
                    return false;
                }
            }
            public string WriteString(stCore.IMessage iLog = null)
            {
                if (this._rss == null)
                {
                    return String.Empty;
                }
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(RSSFeedRoot));
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
                            serializer.Serialize(xw, this._rss);
                            return sw.ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    if (iLog != null)
                    {
                        iLog.LogError(e.Message);
                    }
                }
                return String.Empty;
            }
        }
    }
}
