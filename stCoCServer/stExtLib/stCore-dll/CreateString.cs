using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCore
{
    public static class CreateString
    {
        public static string Build(this string[] astr, string sep = null, Action<string> log = null)
        {
            StringBuilder sb = null;
            try
            {
                sb = new StringBuilder();
                foreach (string str in astr)
                {
                    if (sep != null)
                    {
                        sb.Append(sep);
                    }
                    sb.Append(str);
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                if (log != null)
                {
                    log(e.Message);
                }
                return "";
            }
            finally
            {
                if (sb != null) { sb.Clear(); }
            }
        }
    }
}
