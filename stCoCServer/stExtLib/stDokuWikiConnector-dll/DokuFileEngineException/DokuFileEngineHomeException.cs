using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineHomePageException : Exception 
    {
        public WikiEngineHomePageException() { }
        public WikiEngineHomePageException(string message) : base(message) { }
        public WikiEngineHomePageException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineHomePageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
