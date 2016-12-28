using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCore
{
    public static class LogException
    {
        public static void Error(string err, stCore.IMessage iLog)
        {
            err = ((string.IsNullOrWhiteSpace(err)) ? "-nonrecoverable errors-" : err);
            if (iLog != null)
            {
                iLog.LogError(err);
                return;
            }
            else
            {
                throw new ArgumentException(err);
            }
        }
        public static void Error(Exception ex, stCore.IMessage iLog)
        {
            if (iLog != null)
            {
                iLog.LogError(ex.Message);
                return;
            }
            else
            {
                throw ex;
            }
        }
    }
}
