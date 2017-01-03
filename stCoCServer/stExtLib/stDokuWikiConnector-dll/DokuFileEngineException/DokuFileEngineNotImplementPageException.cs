using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineNotImplementPageException : Exception
    {
        public WikiEngineNotImplementPageException() { }
        public WikiEngineNotImplementPageException(string message) : base(message) { }
        public WikiEngineNotImplementPageException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineNotImplementPageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}