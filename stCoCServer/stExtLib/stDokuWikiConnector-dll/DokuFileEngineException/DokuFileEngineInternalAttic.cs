using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalCacheExceptionAtticException : Exception
    {
        public WikiEngineInternalCacheExceptionAtticException() { }
        public WikiEngineInternalCacheExceptionAtticException(string message) : base(message) { }
        public WikiEngineInternalCacheExceptionAtticException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalCacheExceptionAtticException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
