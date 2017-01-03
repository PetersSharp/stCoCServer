using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineSearchException : Exception
    {
        public WikiEngineSearchException() { }
        public WikiEngineSearchException(string message) : base(message) { }
        public WikiEngineSearchException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineSearchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591

}
