using System;

namespace stCoCServer.CoCAPI
{
    [Serializable]
    public class WikiSearchException : Exception
    {
        public WikiSearchException() { }
        public WikiSearchException(string message) : base(message) { }
        public WikiSearchException(string message, Exception inner) : base(message, inner) { }
        protected WikiSearchException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
