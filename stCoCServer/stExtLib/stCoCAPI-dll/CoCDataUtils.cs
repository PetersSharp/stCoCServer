using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Globalization;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        public static class CoCDataUtils
        {
            private static readonly string tagHex = "%23";

            /// <summary>
            /// Strip # sumbol in tag string and add HEX value
            /// </summary>
            /// <param name="src">tag string</param>
            /// <returns>convert tag string</returns>
            public static string TagToUrl(string src)
            {
                return ((src[0] == '#') ?
                    CoCDataUtils.tagHex + src.Substring(1) :
                    CoCDataUtils.tagHex + src
                );
            }
            /// <summary>
            /// Calculate Donation Ratio
            /// </summary>
            /// <param name="send"></param>
            /// <param name="receive"></param>
            /// <returns>double precession to 2</returns>
            public static double DonationsRatio(Int32 send, Int32 receive)
            {
                if ((send == 0) && (receive == 0))
                {
                    return (double)0.0;
                }
                receive = ((receive == 0) ? 1 : receive);
                return (double)Math.Round((double)((double)send / receive), 2);
            }
            /// <summary>
            /// Normalize war destruction percentage
            /// </summary>
            /// <param name="dd">double in</param>
            /// <returns>double precession to 2</returns>
            public static double DestructionWar(double dd)
            {
                if ((dd < Double.MinValue) || (dd > Double.MaxValue))
                {
                    return (double)0.0;
                }
                return (double)Math.Round(dd, 2);
            }
            /// <summary>
            /// Strip # sumbol in tag string
            /// </summary>
            /// <param name="src">tag string</param>
            /// <returns>convert tag string</returns>
            public static string MapFilterTag(string src)
            {
                return ((src[0] == '#') ? src.Substring(1) : src);
            }
            /// <summary>
            /// Strip URL to file name
            /// </summary>
            /// <param name="src">image url string</param>
            /// <returns>convert file name string</returns>
            public static string MapFilterImageIco(string src)
            {
                return Path.GetFileNameWithoutExtension(src);
            }
            /// <summary>
            /// Convert stupid CoC format to DataTime
            /// </summary>
            /// <param name="src">string</param>
            /// <returns>convert string</returns>
            public static string MapFilterDateTime(string src)
            {
                DateTime dt = DateTime.MinValue;
                DateTime.TryParseExact(src, "yyyyMMdd'T'HHmmss.FFF'Z'", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                return ((dt.Equals(DateTime.MinValue)) ? DateTime.Now.ToString() : dt.ToString());
            }
            public static DateTime FieldConvertDateTime(string src)
            {
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(src, out dt);
                return ((dt.Equals(DateTime.MinValue)) ? DateTime.Now : dt);
            }
            /// <summary>
            /// Pre-proccess Json string, regexp rule replace method,
            /// generate 'Flat' Json object
            /// </summary>
            /// <param name="src">tag string</param>
            /// <returns>convert tag string</returns>
            public static string MapNormalizeJsonTable(string jString, string[] TblPattern, string[] TblReplacement)
            {
                for (int i = 0; i < TblPattern.Length; i++)
                {
                    jString = Regex.Replace(jString, TblPattern[i], TblReplacement[i]);
                }
                return jString;
            }
        }
    }
}
