using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalSearchErrorException : Exception
    {
        public WikiEngineInternalSearchErrorException() { }
        public WikiEngineInternalSearchErrorException(string message) : base(message) { }
        public WikiEngineInternalSearchErrorException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalSearchErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
