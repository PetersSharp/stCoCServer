using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineErrorPageException : Exception
    {
        public WikiEngineErrorPageException() { }
        public WikiEngineErrorPageException(string message) : base(message) { }
        public WikiEngineErrorPageException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineErrorPageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
