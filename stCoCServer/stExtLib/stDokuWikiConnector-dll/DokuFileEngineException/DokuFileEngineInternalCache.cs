using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalCacheException : Exception
    {
        public WikiEngineInternalCacheException() { }
        public WikiEngineInternalCacheException(string message) : base(message) { }
        public WikiEngineInternalCacheException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalCacheException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}

