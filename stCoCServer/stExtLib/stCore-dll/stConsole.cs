using System;
using SysConsole = global::System.Console;
using SysColor = global::System.ConsoleColor;
using System.Text.RegularExpressions;

namespace stCore
{
    public static class stConsole
    {
        private static object consLock = new Object();
        private const string asciiline = "-----------------------------------------------------------";

        #region Console ASCII progress bar

        /// <summary>
        /// An ASCII progress bar
        /// </summary>
        /// <param name="top">Position cols with screen</param>
        public static void ProgressTxt(int progress, int total)
        {
            stConsole._ProgressTxt(-1, progress, total, null, false);
        }
        public static void ProgressTxt(int progress, int total, string text)
        {
            stConsole._ProgressTxt(-1, progress, total, text, false);
        }
        public static void ProgressTxt(int progress, int total, string text, bool newLine)
        {
            stConsole._ProgressTxt(-1, progress, total, text, newLine);
        }
        public static void ProgressTxt(int top, int progress, int total, string text = null, bool newLine = false)
        {
            stConsole._ProgressTxt(top, progress, total, text, newLine);
        }
        private static void _ProgressTxt(int top, int progress, int total, string text = null, bool newLine = false)
        {
            if (newLine)
            {
                lock (consLock)
                {
                    if (top >= 0)
                    {
                        Console.SetCursorPosition(0, top);
                    }
                    SysConsole.Write(Environment.NewLine);
                }
                return;
            }
            int offset = ((string.IsNullOrWhiteSpace(text)) ? 0 : text.Length);

            lock (consLock)
            {
                int curstop = SysConsole.CursorTop;
                SysConsole.CursorVisible = false;

                if (top >= 0)
                {
                    Console.SetCursorPosition(0, top);
                }
                else
                {
                    SysConsole.CursorLeft = 0;
                }
                if (offset > 0)
                {
                    SysConsole.Write(text);
                }
                SysConsole.Write("[");
                SysConsole.CursorLeft = (32 + offset);
                SysConsole.Write("]");
                SysConsole.CursorLeft = (1 + offset);
                float onechunk = 30.0f / total;

                int poscursor = (1 + offset);
                int posinfo   = (35 + offset);
                for (int i = 0; i < onechunk * progress; i++)
                {
                    SysConsole.BackgroundColor = SysColor.Gray;
                    SysConsole.CursorLeft = poscursor++;
                    SysConsole.Write(" ");
                }
                for (int i = poscursor; i <= (31 + offset); i++)
                {
                    SysConsole.BackgroundColor = SysColor.Black;
                    SysConsole.CursorLeft = poscursor++;
                    SysConsole.Write(" ");
                }
                string strinfo = progress.ToString() + "/" + total.ToString();
                if ((posinfo + strinfo.Length) > SysConsole.WindowWidth)
                {
                    strinfo = strinfo.Substring(0, (SysConsole.WindowWidth - posinfo - 2)) + "..";
                }
                SysConsole.CursorLeft = posinfo;
                SysConsole.BackgroundColor = SysColor.Black;
                SysConsole.Write(strinfo);
                if (top >= 0)
                {
                    Console.SetCursorPosition(0, curstop);
                }
                SysConsole.CursorVisible = true;
            }
        }

        #endregion

        #region Split Capitalize String

        public static string SplitCapitalizeString(string msg)
        {
            return Regex.Replace(msg, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
        }

        #endregion

        #region Get Tab to String

        public static string GetTabString(int start, int end)
        {
            string msg = "";
            for (int i = 0; i < (start + end); i++)
            {
                msg += "\t";
            }
            return msg;
        }

        #endregion

        #region Console Write method

        /// <summary>
        /// Write dbug string array to console
        /// </summary>
        public static void WriteHeader(string [] msgs)
        {
            lock (consLock)
            {
                SysConsole.WriteLine(asciiline);
                foreach (string msg in msgs)
                {
                    SysConsole.WriteLine(msg);
                }
                SysConsole.WriteLine(asciiline);
            }
        }
        public static void WriteHeader(string msg)
        {
            lock (consLock)
            {
                SysConsole.WriteLine(asciiline);
                SysConsole.WriteLine(msg);
                SysConsole.WriteLine(asciiline);
            }
        }
        /// <summary>
        /// Write ASCII colorized text to console
        /// </summary>
        public static void Write(string msg)
        {
            lock (consLock)
            {
                SysConsole.Write(msg);
            }
        }
        public static void Write(string msg, SysColor clr)
        {
            lock (consLock)
            {
                stConsole.SetBgColorFromEnum(clr);
                SysConsole.Write(msg);
                stConsole.ResetColor();
            }
        }
        public static void Write(string msg, string clrname)
        {
            lock (consLock)
            {
                stConsole.SetBgColorFromString(clrname);
                SysConsole.Write(msg);
                stConsole.ResetColor();
            }
        }
        public static void Write(string[] msgs)
        {
            lock (consLock)
            {
                stConsole._Write(msgs);
            }
        }
        public static void Write(string[] msgs, string clrname)
        {
            lock (consLock)
            {
                stConsole.SetBgColorFromString(clrname);
                stConsole._Write(msgs);
                stConsole.ResetColor();
            }
        }
        public static void Write(string[] msgs, SysColor clr)
        {
            lock (consLock)
            {
                stConsole.SetBgColorFromEnum(clr);
                stConsole._Write(msgs);
                stConsole.ResetColor();
            }
        }
        public static void Write(string fmt, params object[] args)
        {
            lock (consLock)
            {
                SysConsole.Write(fmt, args);
            }
        }
        public static void Write(string clrname, string fmt, params object[] args)
        {
            lock (consLock)
            {
                stConsole.SetBgColorFromString(clrname);
                SysConsole.Write(fmt, args);
                stConsole.ResetColor();
            }
        }
        public static void Write(SysColor clr, string fmt, params object[] args)
        {
            lock (consLock)
            {
                stConsole.SetBgColorFromEnum(clr);
                SysConsole.Write(fmt, args);
                stConsole.ResetColor();
            }
        }
        public static void WriteLine(string msg)
        {
            stConsole.Write(new string[] { msg, Environment.NewLine });
        }
        public static void WriteLine(string msg, SysColor clr)
        {
            stConsole.Write(new string[] { msg, Environment.NewLine }, clr);
        }
        public static void WriteLine(string msg, string clrname)
        {
            stConsole.Write(new string[] { msg, Environment.NewLine }, clrname);
        }
        public static void WriteLine(string fmt, params object[] args)
        {
            stConsole.Write(fmt, args);
        }
        public static void WriteLine(SysColor clr, string fmt, params object[] args)
        {
            stConsole.Write(clr, fmt, args);
        }
        public static void WriteLine(string clrname, string fmt, params object[] args)
        {
            stConsole.Write(clrname, fmt, args);
        }
        private static void _Write(string[] msgs)
        {
            foreach (string msg in msgs)
            {
                SysConsole.Write(msg);
            }
        }
        public static void NewLine()
        {
            lock (consLock)
            {
                SysConsole.Write(Environment.NewLine);
            }
        }

        #endregion

        #region Set console Color

        public static void SetBgColorFromString(string clrname)
        {
            SysConsole.ForegroundColor = (SysColor)Enum.Parse(typeof(SysColor), clrname);
        }
        public static void SetBgColorFromEnum(SysColor clr)
        {
            SysConsole.ForegroundColor = clr;
        }
        public static void ResetColor()
        {
            SysConsole.ResetColor();
        }

        #endregion

        #region Cursor Position

        public static int GetCursor()
        {
            lock (consLock)
            {
                return SysConsole.CursorTop;
            }
        }
        public static int GetCursorAlign(int offset)
        {
            lock (consLock)
            {
                if ((SysConsole.WindowHeight - 1) <= Console.CursorTop)
                {
                    return (SysConsole.CursorTop - offset);
                }
                return SysConsole.CursorTop;
            }
        }
        public static void RestoreNextLine(int top)
        {
            lock (consLock)
            {
                SysConsole.SetCursorPosition(0, top);
            }
        }
        public static int [] GetCursorPosition()
        {
            return new int[] {
                SysConsole.CursorTop,
                SysConsole.CursorLeft,
            };
        }
        public static int[] WriteGetPosition(string msg)
        {
            lock (consLock)
            {
                SysConsole.Write(msg);
                return stConsole.GetCursorPosition();
            }
        }
        public static void WriteToPosition(string msg, int [] cursor)
        {
            if ((cursor == null) || (cursor.Length == 0) || (string.IsNullOrWhiteSpace(msg)))
            {
                return;
            }
            lock (consLock)
            {
                SysConsole.SetCursorPosition(cursor[1], cursor[0]);
                SysConsole.Write(msg);
            }
        }

        

        #endregion

        #region Print ASCII message

        /// <summary>
        /// Print ASCII message Info / Error
        /// </summary>
        public static void MessageInfo(string head, string body, bool isPrint = true)
        {
            if (!isPrint) { return; }
            stConsole._MessageConsole(head, "\t\t" + body, SysColor.DarkGreen);
        }
        public static void MessageError(string head, string body, bool isPrint = true)
        {
            if (!isPrint) { return; }
            stConsole._MessageConsole(head, "\t" + body, SysColor.DarkRed);
        }
        private static void _MessageConsole(string head, string body, SysColor clr)
        {
            lock (consLock)
            {
                SysConsole.Write(" " + DateTime.Now.ToString("HH:mm:ss") + " [");
                SysConsole.ForegroundColor = clr;
                SysConsole.Write(head);
                SysConsole.ResetColor();
                SysConsole.Write("]" + body + Environment.NewLine);
            }
        }

        #endregion

    }
}
