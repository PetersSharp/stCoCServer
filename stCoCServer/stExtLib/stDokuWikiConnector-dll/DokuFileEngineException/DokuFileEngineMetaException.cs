using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineMetaException : Exception
    {
        public WikiEngineMetaException() { }
        public WikiEngineMetaException(string message) : base(message) { }
        public WikiEngineMetaException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineMetaException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}