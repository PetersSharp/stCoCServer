using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace stDokuWiki
{
    /// <summary>
    /// RPC-XML/AuthManager error Exception
    /// </summary>
    /// <code>
    /// using stDokuWiki;
    /// </code>
    [Serializable]
    public class RpcXmlException : Exception
    {
        /// <summary>
        /// Exception internal error code
        /// </summary>
        public int errcode = 0;

        /// <summary>
        /// Base RpcXmlException
        /// </summary>
        /// <param name="message">Exception error message</param>
        /// <param name="ecode">Exception error code</param>
        public RpcXmlException(string message, int ecode) : base(message) { errcode = ecode;  }
        /// <summary>
        /// Custom Exception RpcXmlException
        /// </summary>
        /// <param name="message">Exception error message</param>
        /// <param name="ecode">Exception error code</param>
        /// <param name="inner">Exception inner Exception</param>
        public RpcXmlException(string message, int ecode, Exception inner) : base(message, inner) { errcode = ecode; }
        /// <summary>
        /// Serialization method RpcXmlException
        /// </summary>
        /// <param name="info">Exception error SerializationInfo</param>
        /// <param name="context">Exception error StreamingContext</param>
        /// <param name="ecode">Exception error code</param>
        protected RpcXmlException(SerializationInfo info, StreamingContext context, int ecode) : base(info, context) { errcode = ecode; }
    }
}
