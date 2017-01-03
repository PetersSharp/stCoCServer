using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalFileTypeException : Exception
    {
        public WikiEngineInternalFileTypeException() { }
        public WikiEngineInternalFileTypeException(string message) : base(message) { }
        public WikiEngineInternalFileTypeException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalFileTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
