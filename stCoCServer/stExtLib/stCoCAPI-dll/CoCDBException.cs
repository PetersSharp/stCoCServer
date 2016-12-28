using System;

namespace stCoCAPI
{
    [Serializable]
    public class CoCDBException : Exception
    {
        public stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq enumId = stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtNone;

        public CoCDBException() { }
        public CoCDBException(stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq errorId, string message = null)
            : base(message)
        {
            this.enumId = errorId;
        }
        public CoCDBException(string message) : base(message) { }
        public CoCDBException(stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq errorId, string message, Exception inner)
            : base(message, inner)
        {
            this.enumId = errorId;
        }
        protected CoCDBException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class CoCDBExceptionReason : Exception
    {
        public CoCDBExceptionReason() { }
        public CoCDBExceptionReason(string message) : base(message) { }
        public CoCDBExceptionReason(string message, Exception inner) : base(message, inner) { }
        protected CoCDBExceptionReason(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
