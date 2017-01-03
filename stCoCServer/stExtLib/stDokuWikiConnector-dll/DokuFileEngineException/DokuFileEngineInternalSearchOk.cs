using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalSearchOkException : Exception
    {
        public WikiEngineInternalSearchOkException() { }
        public WikiEngineInternalSearchOkException(string message) : base(message) { }
        public WikiEngineInternalSearchOkException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalSearchOkException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
