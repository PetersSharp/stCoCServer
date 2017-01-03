using System;

namespace stDokuWiki.WikiEngine.Exceptions
{
#pragma warning disable 1591
    [Serializable]
    public class WikiEngineAtticIncompletteException : Exception
    {
        public WikiEngineAtticIncompletteException() { }
        public WikiEngineAtticIncompletteException(string message) : base(message) { }
        public WikiEngineAtticIncompletteException(string message, Exception inner) : base(message, inner) { }
        protected WikiEngineAtticIncompletteException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
#pragma warning restore 1591
}