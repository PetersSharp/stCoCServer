using System;
using System.Runtime.Serialization;

namespace stCore
{
    [Serializable()]
    public class IExceptionInfo : Exception
    {
        public IExceptionInfo()
            : base() { }

        public IExceptionInfo(string msg)
            : base(msg) { }

        public IExceptionInfo(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public IExceptionInfo(string message, Exception iException)
            : base(message, iException) { }

        public IExceptionInfo(string format, Exception iException, params object[] args)
            : base(string.Format(format, args), iException) { }

        protected IExceptionInfo(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    [Serializable()]
    public class IExceptionAlert : Exception
    {
        public IExceptionAlert()
            : base() { }

        public IExceptionAlert(string msg)
            : base(msg) { }

        public IExceptionAlert(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public IExceptionAlert(string message, Exception iException)
            : base(message, iException) { }

        public IExceptionAlert(string format, Exception iException, params object[] args)
            : base(string.Format(format, args), iException) { }

        protected IExceptionAlert(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
