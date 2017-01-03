using System;

namespace stCoCServer.CoCAPI
{
    [Serializable]
    public class WikiNotImplementPageException : Exception
    {
        public WikiNotImplementPageException() { }
        public WikiNotImplementPageException(string message) : base(message) { }
        public WikiNotImplementPageException(string message, Exception inner) : base(message, inner) { }
        protected WikiNotImplementPageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}