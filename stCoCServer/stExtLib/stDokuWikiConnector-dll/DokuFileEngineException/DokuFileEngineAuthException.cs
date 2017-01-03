using System;
using stDokuWiki.WikiEngine;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineAuthException : Exception
    {
        public WikiFileMeta wfm = null;

        public WikiEngineAuthException() { }
        public WikiEngineAuthException(string message, WikiFileMeta wfm) : base(message) { this.wfm = wfm; }
        public WikiEngineAuthException(string message) : base(message) { this.wfm = null; }
        public WikiEngineAuthException(string message, Exception inner) : base(message, inner) { this.wfm = null; }
        protected WikiEngineAuthException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { this.wfm = null; }
    }
#pragma warning restore 1591
}