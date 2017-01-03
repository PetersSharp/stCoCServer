using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalSearchManyResultException : Exception
    {
        public WikiEngineInternalSearchManyResultException() { }
        public WikiEngineInternalSearchManyResultException(string message) : base(message) { }
        public WikiEngineInternalSearchManyResultException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalSearchManyResultException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}