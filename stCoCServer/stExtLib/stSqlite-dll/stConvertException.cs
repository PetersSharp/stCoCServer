using System;

#if STCLIENTBUILD
namespace stClient
#else
namespace stSqlite
#endif
{
    [Serializable]
    public class ConvertExtensionException : Exception
    {
        public enum ConvertErrorType : int
        {
            ConvertErrorUndefined,
            ConvertErrorBadInheritClass,
            ConvertErrorToDataTable
        };
        public ConvertExtensionException() { }
        public ConvertExtensionException(ConvertErrorType ct, string msg1 = null, string msg2 = null) : base(_setMessage(ct, msg1, msg2)) { }
        public ConvertExtensionException(Exception inner, ConvertErrorType ct, string msg1 = null, string msg2 = null) : base(_setMessage(ct, msg1, msg2), inner) { }
        protected ConvertExtensionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
        private static string _setMessage(ConvertErrorType ct, string msg1, string msg2)
        {
            switch (ct)
            {
                case ConvertErrorType.ConvertErrorBadInheritClass:
                    {
                        return string.Format(
                            Properties.Resources.ConvertErrorBadInheritClass,
                            ((string.IsNullOrWhiteSpace(msg1)) ? "-" : msg1)
                        );
                    }
                case ConvertErrorType.ConvertErrorToDataTable:
                    {
                        return string.Format(
                            Properties.Resources.ConvertErrorToDataTable,
                            ((string.IsNullOrWhiteSpace(msg1)) ? "" : msg1),
                            ((string.IsNullOrWhiteSpace(msg2)) ? "" : msg2)
                        );
                    }
                default:
                    {
                        return ((!string.IsNullOrWhiteSpace(msg1)) ? msg1 :
                            ((!string.IsNullOrWhiteSpace(msg2)) ? msg2 : "error in exception..")
                        );
                    }
            }
        }
    }
}
