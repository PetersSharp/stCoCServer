using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stNet
{
    public static class IrcFormatText
    {
        //
        // color from http://www.mirc.co.uk/colors.html
        //
        public enum Color : int
        {
            White = 0,
            Black = 1,
            DarkBlue = 2,
            DarkGreen = 3,
            Red = 4,
            DarkRed = 5,
            DarkViolet = 6,
            Orange = 7,
            Yellow = 8,
            LightGreen = 9,
            Cyan = 10,
            LightCyan = 11,
            Blue = 12,
            Violet = 13,
            DarkGray = 14,
            LightGray = 15,
            Reset = 99
        };

        public enum Style : int
        {
            Bold = 0,
            Color = 1,
            Italic = 2,
            StrikeThrough = 3,
            Underline = 4,
            Underline2 = 5,
            Reverse = 6,
            Reset = 7
        };

        private static readonly string [] StyleChar = new string[] {
            "\x02", "\x03", "\x09", "\u0013", "\u0015", "\u001F", "\u0016", "\u000F"
        };

        private static string GetColorCode2Digits(IrcFormatText.Color color) { string s = GetColorCodeInt(color).ToString(); while (s.Length < 2) { s = "0" + s; } return s; }
        private static int GetColorCodeInt(IrcFormatText.Color color) { return (int)color; }
        private static int GetStyleCodeInt(IrcFormatText.Style style) { return (int)style; }
        private static string GetColorCodeName(IrcFormatText.Color color) { return color.ToString(); }
        private static string GetColorCodeName(int color) { return ((IrcFormatText.Color)color).ToString(); }

        public static string ColorText(this string text, IrcFormatText.Color fg, IrcFormatText.Color bg = IrcFormatText.Color.Reset)
        {
            return
                IrcFormatText.StyleChar[IrcFormatText.GetStyleCodeInt(IrcFormatText.Style.Color)]
                + GetColorCode2Digits(fg)
                + ((bg != IrcFormatText.Color.Reset)
                   ? "," + GetColorCode2Digits(bg)
                    : ""
                  )
                + text.TrimEnd()
                + IrcFormatText.StyleChar[IrcFormatText.GetStyleCodeInt(IrcFormatText.Style.Color)]
             ;
        }
        public static string StyleText(this string text, IrcFormatText.Style style = IrcFormatText.Style.Reset)
        {
            if (style == IrcFormatText.Style.Reset)
            {
                return text;
            }
            return
                IrcFormatText.StyleChar[IrcFormatText.GetStyleCodeInt(style)]
                + text
                + IrcFormatText.StyleChar[IrcFormatText.GetStyleCodeInt(style)]
            ;
        }
    }
}
