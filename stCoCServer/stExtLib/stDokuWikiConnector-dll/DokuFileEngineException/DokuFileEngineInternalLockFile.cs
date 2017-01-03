using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineInternalLockFile : Exception
    {
        public WikiEngineInternalLockFile() { }
        public WikiEngineInternalLockFile(string message) : base(message) { }
        public WikiEngineInternalLockFile(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineInternalLockFile(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}
