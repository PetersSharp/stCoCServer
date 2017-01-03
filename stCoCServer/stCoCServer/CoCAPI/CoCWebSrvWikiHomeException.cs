using System;

namespace stCoCServer.CoCAPI
{
    [Serializable]
    public class WikiHomePageException : Exception 
    {
        public WikiHomePageException() { }
        public WikiHomePageException(string message) : base(message) { }
        public WikiHomePageException(string message, Exception inner) : base(message, inner) { }
        protected WikiHomePageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
