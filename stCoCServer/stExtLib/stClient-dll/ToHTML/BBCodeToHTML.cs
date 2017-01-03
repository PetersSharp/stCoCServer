using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace stClient
{
    public static class BBCode
    {
        public static string BBCodeToHtml(this string val)
        {
            Regex exp;

            exp = new Regex(@"\[b\](.+?)\[/b\]");
            val = exp.Replace(val, "<strong>$1</strong>");

            exp = new Regex(@"\[i\](.+?)\[/i\]");
            val = exp.Replace(val, "<em>$1</em>");

            exp = new Regex(@"\[u\](.+?)\[/u\]");
            val = exp.Replace(val, "<u>$1</u>");

            exp = new Regex(@"\[s\](.+?)\[/s\]");
            val = exp.Replace(val, "<strike>$1</strike>");

            exp = new Regex(@"\[url\=([^\]]+)\]([^\]]+)\[/url\]");
            val = exp.Replace(val, "<a href=\"$1\">$2</a>");
            exp = new Regex(@"\[url\]([^\]]+)\[/url\]");
            val = exp.Replace(val, "<a href=\"$1\">$1</a>");

            exp = new Regex(@"\[quote\=([^\]]+)\]([^\]]+)\[/quote\]");
            val = exp.Replace(val, "<blockquote><p>$1</p><p>$2</p></blockquote>");
            exp = new Regex(@"\[quote\]([^\]]+)\[/quote\]");
            val = exp.Replace(val, "<blockquote><p>$1</p></blockquote>");

            exp = new Regex(@"\[img\]([^\]]+)\[/img\]");
            val = exp.Replace(val, "<a href=\"$1\"><img src=\"$1\" width=\"60\"></a>");
            exp = new Regex(@"\[img\=([^\]]+)\]([^\]]+)\[/img\]");
            val = exp.Replace(val, "<a href=\"$1\"><img src=\"$2\" width=\"60\"></a>");

            exp = new Regex(@"\[color\=([^\]]+)\]([^\]]+)\[/color\]");
            val = exp.Replace(val, "<span style=\"color: $1\">$2</span>");
            exp = new Regex(@"\[colour\=([^\]]+)\]([^\]]+)\[/colour\]");
            val = exp.Replace(val, "<span style=\"color: $1\">$2</span>");
            exp = new Regex(@"\[style color\=([^\]]+)\]([^\]]+)\[/style\]");
            val = exp.Replace(val, "<span style=\"color: $1\">$2</span>");

            exp = new Regex(@"\[size\=([^\]]+)\]([^\]]+)\[/size\]");
            val = exp.Replace(val, "<span>$2</span>");
            val = val.Replace("\r\n", "");

            exp = new Regex(@"\[code\]([^\]]+)\[/code\]");
            val = exp.Replace(val, "<code style=\"white-space:pre;\">$1</code>");

            return val;
        }
    }
}
