using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace stLog
{
    public static class UnhandledExcept
    {
        public static List<string> UList = new List<string>();

        public static void HandleThreadApp(object sender, ThreadExceptionEventArgs e)
        {
            string info = e.Exception.Message;
            if (string.IsNullOrWhiteSpace(info))
            {
                return;
            }
            stLog.UnhandledExcept.UList.Add("Unhandled Thread Exception: " + info);
        }
        public static void HandleCDomain(object sender, UnhandledExceptionEventArgs e)
        {
            string info = (e.ExceptionObject as Exception).Message;
            if (string.IsNullOrWhiteSpace(info))
            {
                return;
            }
            stLog.UnhandledExcept.UList.Add("Unhandled UI Exception: " + info);
        }
        public static void ErrorToLog(stCore.IMessage iMsg)
        {
            foreach (string toLog in (List<string>)stLog.UnhandledExcept.UList)
            {
                if (!string.IsNullOrWhiteSpace(toLog))
                {
                    iMsg.Log(toLog);
                }
            }
            stLog.UnhandledExcept.UList.Clear();
            stLog.UnhandledExcept.UList = null;
        }
        public static int ErrorCount()
        {
            return stLog.UnhandledExcept.UList.Count;
        }
    }
}
