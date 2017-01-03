using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace stClient
{
    public static class Emoticons
    {
        private static Dictionary<char, string> emojinorm = new Dictionary<char, string>()
        {
            {')', "happy"},
            {'(', "hopeless"},
            {'O', "weirdout"},
            {'E', "anger"},
            {'|', "sadistic"},
            {'D', "glad"},
            {'*', "blushing"},
            {'?', "nervous"},
            {'[', "amazed"},
            {'.', "blank"}
        };
        private static Dictionary<char, string> emojiblink = new Dictionary<char, string>()
        {
            {')', "wink"},
            {'(', "helpless"},
            {'O', "unbelievables"},
            {'E', "rage"},
            {'|', "consoling"},
            {'D', "helpful"},
            {'*', "cute"},
            {'?', "sad"},
            {'[', "whining"}
        };
        public static string EmoticonsToHtml(this string val, string fmtHtml)
        {
            try
            {
                string nvs = string.Empty;
                int lastpos = -1;
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < val.Length; i++)
                {
                    Dictionary<char, string> emojidic = null;

                    if ((val.Length - 2) == i)
                    {
                        break;
                    }
                    if (val[(i + 1)] != '-')
                    {
                        continue;
                    }
                    switch (val[i])
                    {
                        case ':':
                            {
                                emojidic = emojinorm;
                                break;
                            }
                        case ';':
                            {
                                emojidic = emojiblink;
                                break;
                            }
                        default:
                            {
                                continue;
                            }
                    }
                    lastpos = ((lastpos == -1) ? 0 : lastpos);
                    sb.Append(
                        val.Substring(lastpos, (i - lastpos))
                    );
                    lastpos = (i + 3);
                    if (emojidic.ContainsKey(val[(i + 2)]))
                    {
                        sb.AppendFormat(
                            fmtHtml,
                            emojidic[val[(i + 2)]]
                        );
                    }
                }
                if ((lastpos != -1) && (sb.Length > 0))
                {
                    if (lastpos < val.Length)
                    {
                        sb.Append(
                            val.Substring(lastpos, (val.Length - lastpos))
                        );
                    }
                    return sb.ToString();
                }
                return val;
            }
            catch (Exception)
            {
                return ((string.IsNullOrWhiteSpace(val)) ? "" : val);
            }
        }
    }
}
