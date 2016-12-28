using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace stCore
{
    public class IMessage
    {
        public IMessage()
        {
        }
        ~IMessage()
        {
        }

        public Action<int, int, int, string, bool> ProgressBar = (x, y, z, v, w) => { }; 
        public Action<string> Line = (x) => { };
        public Action<string> LogInfo = (x) => { };
        public Action<string> LogError = (x) => { };
        public Action<string> LogNetSyslog = (x) => { };
        public Action<object, string, string, int> Box = (x, y, z, i) => { };
        public Action<string, string, int> BoxError
        {
            get { return (y, z, i) => this.Box(null, y, z, i); }
            protected set { }
        }
        public Action BoxClose = () => { };
        public void AddStrings(string[] args)
        {
            this._AddStrings(null, args);
        }
        public void AddStrings(MethodBase mb, string[] args)
        {
            this._AddStrings(mb, args);
        }
        private void _AddStrings(MethodBase mb, string[] args)
        {
            if (args == null)
            {
                return;
            }
            string toLog = "";
            for (int i = 0; i < args.Length; i++)
            {
                toLog = toLog + args[i];
            }
            if (mb != null)
            {
                this.LogError(this.GetMethod(mb, toLog));
            }
            else
            {
                this.LogError(toLog);
            }
        }
        public void LogLocation(MethodBase mb, string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return;
            }
            this.LogError(this.GetMethod(mb, src));
        }
        public void ToLogAndLine(MethodBase mb, string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return;
            }
            this.Line(src);
            this.LogError(this.GetMethod(mb, src));
        }
        private string GetMethod(MethodBase mb, string src)
        {
            return string.Format("[ {0}.{1} ]: {2}",
                ((mb != null) ? mb.DeclaringType.Name : "_"),
                ((mb != null) ? mb.Name : "_"),
                ((string.IsNullOrWhiteSpace(src)) ? "empty message" : src)
            );
        }

    }
}
