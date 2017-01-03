using System;

namespace stCoCServer.CoCAPI
{
    [Serializable]
    public class WikiErrorPageException : Exception
    {
        public WikiErrorPageException() { }
        public WikiErrorPageException(string message) : base(message) { }
        public WikiErrorPageException(string message, Exception inner) : base(message, inner) { }
        protected WikiErrorPageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
