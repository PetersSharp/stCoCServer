using System;
using stDokuWiki.WikiEngine;

namespace stCoCServer.CoCAPI
{
    [Serializable]
    public class WikiAuthException : Exception
    {
        public WikiFileMeta wfm = null;

        public WikiAuthException() { }
        public WikiAuthException(string message, WikiFileMeta wfm) : base(message) { this.wfm = wfm; }
        public WikiAuthException(string message) : base(message) { this.wfm = null; }
        public WikiAuthException(string message, Exception inner) : base(message, inner) { this.wfm = null; }
        protected WikiAuthException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { this.wfm = null; }
    }
}