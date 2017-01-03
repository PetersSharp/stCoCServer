using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalNameSpaceErrorException : Exception
    {
        public WikiEngineInternalNameSpaceErrorException() { }
        public WikiEngineInternalNameSpaceErrorException(string message) : base(message) { }
        public WikiEngineInternalNameSpaceErrorException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalNameSpaceErrorException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
