using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEnginePutException : Exception
    {
        public WikiEnginePutException() { }
        public WikiEnginePutException(string message) : base(message) { }
        public WikiEnginePutException(string message, Exception inner) : base(message, inner) { }
        protected WikiEnginePutException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}