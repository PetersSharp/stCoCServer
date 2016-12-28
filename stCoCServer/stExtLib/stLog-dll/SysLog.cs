using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;
using System.Drawing;
// using System.Runtime.InteropServices;

namespace stLog
{
    [DefaultProperty("Threshold"),
    DefaultEvent("ThresholdExceeded"),
    HelpKeywordAttribute(typeof(System.ComponentModel.Component)),
    ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design"),
    ToolboxBitmap(typeof(EventLog))]
    /* GuidAttribute("0dc40e28-232a-4060-a104-184771eae99e")] */
    public partial class SysLog : Component
    {
        private EventLog evLog = null;
        private bool isInit = false;
        protected int _nLine = 0;
        private int nLine
        {
            get { lock (this) { return this._nLine; } }
            set { lock (this) { this._nLine = value; } }
        }

        [Browsable(false),
        DefaultValue(typeof(stCore.IMessage), "new stCore.IMessage();"),
        Description("Action: Logging, message Box, info Line"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public stCore.IMessage IMsg { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(string), "myApp"),
        Description("Log app name"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public string appName { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(string), "ErrorLog"),
        Description("Log marker name"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public string evName { get; set; }

        public SysLog()
            : base()
        {
            this._SysLog(null, null, null);
        }
        public SysLog(string name)
            : base()
        {
            this._SysLog(
                name,
                null,
                null
            );
        }
        public SysLog(stCore.IMessage iMsg)
            : base()
        {
            this._SysLog(
                null,
                null,
                iMsg
            );
        }
        public SysLog(string name, stCore.IMessage iMsg)
            : base()
        {
            this._SysLog(
                name,
                null,
                iMsg
            );
        }
        public SysLog(string name, string evname, stCore.IMessage iMsg)
            : base()
        {
            this._SysLog(
                name,
                evname,
                iMsg
            );
        }
        private void _SysLog(string name, string evname, stCore.IMessage iMsg)
        {
            this.InitAttrDefaults();

            this.appName = ((string.IsNullOrWhiteSpace(name)) ? this.appName : name);
            this.evName  = ((string.IsNullOrWhiteSpace(evname)) ? this.evName : evname);
            this.IMsg    = ((iMsg == null) ? (new stCore.IMessage()) : iMsg);
            this.Start();
        }
        ~SysLog()
        {
            // this.Clear();
        }

        private bool CheckThisLog()
        {
            if (
                (!this.isInit) ||
                (this.evLog == null)
               )
            {
                if (
                    (!EventLog.SourceExists(this.appName)) ||
                    (!EventLog.Exists(this.evName))
                   )
                {
                    this.IMsg.Line(global::stLog.Properties.Resources.stLogDisabled);
                }
                return false;
            }
            return true;
        }

        public void Add(string msg)
        {
            if (!CheckThisLog())
            {
                return;
            }
            try
            {
                this.evLog.WriteEntry(msg, EventLogEntryType.Warning);
            }
            catch (Exception e)
            {
                switch (stLog.SysLog.GetErrorCode(e))
                {
                    case 1502:
                        {
                            this.Clear();
                            this.IMsg.Line(global::stLog.Properties.Resources.stLogClear);
                            this.IMsg.BoxError(
                                e.Message,
                                global::stLog.Properties.Resources.stLogClear,
                                0
                            );
                            break;
                        }
                    case -1:
                        {
                            this.IMsg.Line(global::stLog.Properties.Resources.stErrorCodeUnknown);
                            break;
                        }
                }
            }
        }
        public int Read(Action<DateTime, string, string> act)
        {
            if (
                (this.evLog != null) &&
                (this.evLog.Entries.Count == 0)
               )
            {
                this.IMsg.Line(global::stLog.Properties.Resources.stLogEmpty);
                return 0;
            }
            this.nLine = 0;
            return this._ReadPart(act);
        }
        public int ReadPart(Action<DateTime, string, string> act)
        {
            return this._ReadPart(act);
        }
        public int ReadPart(Action<DateTime, string, string> act, int setNline)
        {
            this.nLine = setNline;
            return this._ReadPart(act);
        }
        private int _ReadPart(Action<DateTime, string, string> act)
        {
            if (
                (!CheckThisLog()) ||
                (act == null) ||
                (this.evLog.Entries.Count == 0)
               )
            {
                return this.nLine;
            }
            foreach (EventLogEntry entry in this.evLog.Entries)
            {
                if (entry.Index > this.nLine)
                {
                    act(entry.TimeGenerated, entry.EntryType.ToString(), entry.Message);
                    this.nLine = entry.Index;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            return this.nLine;
        }
        public void Clear()
        {
            if (!CheckThisLog())
            {
                return;
            }
            this.evLog.Clear();
            this.nLine = 0;
        }
        public void Start()
        {
            if (string.IsNullOrWhiteSpace(this.appName))
            {
                throw new ArgumentNullException(global::stLog.Properties.Resources.stAppNameIsNull);
            }
            if (string.IsNullOrWhiteSpace(this.evName))
            {
                throw new ArgumentNullException(global::stLog.Properties.Resources.stAppEventIsNull);
            }
            if (!EventLog.SourceExists(this.appName))
            {
                EventLog.CreateEventSource(this.appName, this.evName);
            }
            if (this.evLog != null)
            {
                this.evLog.Dispose();
            }
            this.evLog = new EventLog();
            this.evLog.Source = this.appName;
            this.isInit = true;
        }
        public void Stop()
        {
            if (!this.isInit)
            {
                return;
            }
            if (EventLog.SourceExists(this.appName))
            {
                EventLog.DeleteEventSource(this.appName);

                if (EventLog.Exists(this.evName))
                {
                    EventLog.Delete(this.evName);
                }
            }
            this.isInit = false;
            this.nLine = 0;
        }
        public void Close()
        {
            if (this.isInit)
            {
                this.Stop();
            }
            if (this.evLog != null)
            {
                this.evLog.Dispose();
            }
            this.evLog = null;
            this.isInit = false;
            this.nLine = 0;
        }
        public static string GetMethod(MethodBase mb)
        {
            return string.Format("[ {0}.{1} ]: ",
                mb.DeclaringType.Name,
                mb.Name
            );
        }
        public static int GetErrorCode(Exception ex)
        {
            Win32Exception w32ex = ex as Win32Exception;
            if (w32ex == null)
            {
                w32ex = ex.InnerException as Win32Exception;
            }
            if (w32ex != null)
            {
                return w32ex.NativeErrorCode;
            }
            return -1;
        }

    }
}
