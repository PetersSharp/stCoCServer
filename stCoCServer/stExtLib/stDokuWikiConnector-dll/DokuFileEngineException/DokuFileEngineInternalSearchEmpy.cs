using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalSearchEmptyException : Exception
    {
        public WikiEngineInternalSearchEmptyException() { }
        public WikiEngineInternalSearchEmptyException(string message) : base(message) { }
        public WikiEngineInternalSearchEmptyException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalSearchEmptyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
