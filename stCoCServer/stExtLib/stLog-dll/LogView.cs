using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace stLog
{
    public partial class LogView
    {
        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(Boolean), "false"),
        Description("Show log full date"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool isFullDate { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(Boolean), "true"),
        Description("Show log colored"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool isColorize { get; set; }

        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static readonly string[] linePart =
        {
            "\t[ ",
            " ]\t",
            "      "
        };

        [Browsable(false),
        DefaultValue(typeof(stLog.SysLog), "null"),
        Description("stLog.SysLog class handle"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private stLog.SysLog sysLog { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.RichTextBox), "null"),
        Description("Forms.RichTextBox control handle"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private System.Windows.Forms.RichTextBox rBox { get; set; }

        [Browsable(false),
        DefaultValue(typeof(AutoCompleteStringCollection), "null"),
        Description("AutoCompleteStringCollection ref"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private AutoCompleteStringCollection autoCmpl { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.Timer), "null"),
        Description("Forms.Timer control handle"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.Timer timerLog { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.EventHandler), "null"),
        Description("Forms.Timer tick function handle"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.EventHandler timerHandle { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.Control), "null"),
        Description("Forms.Control handle, update Log statistic to Control.Text"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.Windows.Forms.Control ctrlInfo { get; set; }

        #region INIT LOGVIEW

        public LogView(stLog.SysLog syslog, System.Windows.Forms.RichTextBox rbox)
        {
            this._LogView(syslog, rbox, null, null, null);
        }
        public LogView(stLog.SysLog syslog, System.Windows.Forms.RichTextBox rbox, System.Windows.Forms.Timer timerlog)
        {
            this._LogView(syslog, rbox, timerlog, null, null);
        }
        public LogView(stLog.SysLog syslog, System.Windows.Forms.RichTextBox rbox, AutoCompleteStringCollection autocmpl, object ctrlinfo)
        {
            this._LogView(syslog, rbox, null, autocmpl, ctrlinfo);
        }
        public LogView(stLog.SysLog syslog, System.Windows.Forms.RichTextBox rbox, System.Windows.Forms.Timer timerlog, AutoCompleteStringCollection autocmpl)
        {
            this._LogView(syslog, rbox, timerlog, autocmpl, null);
        }
        public LogView(stLog.SysLog syslog, System.Windows.Forms.RichTextBox rbox, System.Windows.Forms.Timer timerlog, AutoCompleteStringCollection autocmpl, object ctrlinfo)
        {
            this._LogView(syslog, rbox, timerlog, autocmpl, ctrlinfo);
        }
        ~LogView()
        {
            if (this.timerLog != null)
            {
                if (this.timerLog.Enabled)
                {
                    this.timerLog.Stop();
                    this.timerLog.Enabled = false;
                }
                if (this.timerHandle != null)
                {
                    this.timerLog.Tick -= this.timerHandle;
                }
                this.timerLog.Dispose();
                this.sysLog.IMsg.Line(global::stLog.Properties.Resources.stLogDisabled);
            }
        }

        private void _LogView(stLog.SysLog syslog, System.Windows.Forms.RichTextBox rbox, System.Windows.Forms.Timer timerlog, AutoCompleteStringCollection autocmpl, object ctrlinfo)
        {
            this.InitAttrDefaults();

            if (syslog == null)
            {
                throw new ArgumentNullException(global::stLog.Properties.Resources.stSysLogIsNull);
            }
            if (rbox == null)
            {
                throw new ArgumentNullException(global::stLog.Properties.Resources.stRichTextBoxIsNull);
            }
            this.rBox = rbox;
            this.sysLog = syslog;
            this.ctrlInfo = ((ctrlinfo != null) ? (ctrlinfo as Control) : ctrlInfo);
            this.autoCmpl = ((autocmpl != null) ? autocmpl : autoCmpl);
            this.timerHandle = new System.EventHandler(this.AutoUpdateTick);

            if (timerlog == null)
            {
                this.timerLog = new System.Windows.Forms.Timer();
                this.timerLog.Enabled = false;
                this.timerLog.Interval = 1000;
                this.timerLog.Tick += this.timerHandle;
            }
            else
            {
                this.timerLog = timerlog;
                this.timerLog.Tick += this.timerHandle;
            }
        }

        private void _CheckLogView()
        {
            if (this.sysLog == null)
            {
                throw new ArgumentNullException(global::stLog.Properties.Resources.stSysLogIsNull);
            }
            if (this.rBox == null)
            {
                throw new ArgumentNullException(global::stLog.Properties.Resources.stRichTextBoxIsNull);
            }
        }

        #endregion

        #region LOG RICH TEXT BOX METHOD

        public void LogFind(string txts, Color clr)
        {
            try
            {
                this._CheckLogView();
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(e.Message);
            }
            if (
                (string.IsNullOrWhiteSpace(txts)) ||
                (txts.Trim().Length <= 0)
               )
            {
                throw new ArgumentNullException(global::stLog.Properties.Resources.stSearchStrIsNull);
            }

            int lIndex = 0,
                cIndex = 0,
                sIndex = 0,
                tIndex = this.rBox.TextLength;

            txts = txts.Trim();

            if (
                (string.IsNullOrWhiteSpace(txts)) ||
                (txts.Length <= 0) ||
                (tIndex <= 0) ||
                (tIndex < txts.Length)
               )
            {
                return;
            }
            tIndex = (tIndex - txts.Length);
            lIndex = this.rBox.Text.LastIndexOf(txts, StringComparison.OrdinalIgnoreCase);

            if (this.autoCmpl != null)
            {
                this.autoCmpl.Add(txts);
            }

            while (cIndex <= lIndex)
            {
                this.rBox.Find(txts, cIndex, tIndex, RichTextBoxFinds.None);
                this.rBox.SelectionBackColor = ((clr == Color.Empty) ? Color.Yellow : clr);
                cIndex = this.rBox.Text.IndexOf(txts, cIndex);
                if (
                    (cIndex < 0) ||
                    (sIndex == cIndex)
                   )
                {
                    break;
                }
                sIndex = cIndex;
                cIndex += 1;
            }
        }

        public void PrintAllLogLine()
        {
            this.sysLog.ReadPart(this.PrintLogLine, 0);
        }

        public void PrintLogLine(DateTime dt, string lvs, string msg)
        {
            try
            {
                this._CheckLogView();
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(e.Message);
            }
            string dts = ((this.isFullDate) ? dt.ToString() : dt.ToShortTimeString());
            if (this.rBox.MaxLength < (this.rBox.TextLength + lvs.Length + msg.Length + dts.Length + 8))
            {
                this.sysLog.IMsg.Line(global::stLog.Properties.Resources.stLogFull);
                this.rBox.Clear();
            }
            if (this.isColorize)
            {
                this.PrintLineColorize(dts, lvs, msg);
            }
            else
            {
                this.PrintLineDefault(dts, lvs, msg);
            }
            this.LogScroll();
        }

        private void PrintLineDefault(string dts, string lvs, string msg)
        {
            this.rBox.AppendText(dts + stLog.LogView.linePart[0] + lvs + stLog.LogView.linePart[1] + msg + Environment.NewLine);
        }

        private void PrintLineColorize(string dts, string lvs, string msg)
        {
            this.PrintStringColor(dts, Color.DarkGray);
            this.PrintStringColor(stLog.LogView.linePart[0], Color.Black);
            this.PrintStringColor(lvs, Color.Red);
            this.PrintStringColor(stLog.LogView.linePart[1] + msg + Environment.NewLine, Color.Black);
        }

        private void PrintStringColor(string src, Color clr)
        {
            int len = this.rBox.TextLength;
            this.rBox.AppendText(src);
            this.rBox.Select(
                len,
                (src.Length + 1)
            );
            this.rBox.SelectionColor = clr;
        }

        private void LogScroll()
        {
            this.rBox.SelectionStart = this.rBox.MaxLength;
            this.rBox.ScrollToCaret();
            this.rBox.Refresh();
        }

        #endregion

        #region LOG AUTO UPDATE METHOD

        public void AutoUpdateTick(object o, EventArgs ea)
        {
            try
            {
                this._CheckLogView();
            }
            catch (Exception e)
            {
                if (this.timerLog.Enabled)
                {
                    this.timerLog.Stop();
                    this.timerLog.Enabled = false;
                }
                throw new ArgumentNullException(e.Message);
            }
            int cnt = this.sysLog.ReadPart(this.PrintLogLine);
            if (this.ctrlInfo != null)
            {
                this.ctrlInfo.Text =
                    string.Format(
                        global::stLog.Properties.Resources.stLogInfo,
                        cnt
                    ) + stLog.LogView.linePart[2];
            }
        }

        public bool AutoUpdateStop()
        {
            if (this.timerLog != null)
            {
                if (this.timerLog.Enabled)
                {
                    this.timerLog.Stop();
                    this.timerLog.Enabled = false;
                    this.sysLog.IMsg.Line(global::stLog.Properties.Resources.stLogDisabled);
                }
                return true;
            }
            return false;
        }

        public bool AutoUpdateStart()
        {
            try
            {
                this._CheckLogView();
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(e.Message);
            }
            if (this.timerLog != null)
            {
                if (!this.timerLog.Enabled)
                {
                    this.timerLog.Enabled = true;
                    this.sysLog.IMsg.Line(global::stLog.Properties.Resources.stLogEnabled);
                    this.timerLog.Start();
                }
                return true;
            }
            return false;
        }

        public bool AutoUpdateCheck()
        {
            if (
                (this.timerLog != null) &&
                (this.timerLog.Enabled)
               )
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
