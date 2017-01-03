using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalStructErrorException : Exception
    {
        public WikiEngineInternalStructErrorException() { }
        public WikiEngineInternalStructErrorException(string message) : base(message) { }
        public WikiEngineInternalStructErrorException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalStructErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}