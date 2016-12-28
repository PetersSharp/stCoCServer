using System;
using System.Drawing;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace stUI
{
    [DefaultProperty("Threshold"),
    DefaultEvent("ThresholdExceeded"),
    HelpKeywordAttribute(typeof(System.Windows.Forms.UserControl)),
    ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design"),
    ToolboxBitmap(typeof(System.Windows.Forms.MessageBox))]
    public partial class stMessageBox : System.Windows.Forms.UserControl
    {
        private const int HeightDefault = 100;
        private System.Windows.Forms.Timer tmClose { get; set; }

        private System.Windows.Forms.Form _pForm { get; set; }
        private System.Windows.Forms.Form pForm
        {
            get { lock (this) { return this._pForm; } }
            set { lock (this) { this._pForm = value; } }
        }

        private static stMessageBox _mBox { get; set; }
        private stMessageBox mBox
        {
            get { lock (this) { return stMessageBox._mBox; } }
            set { lock (this) { stMessageBox._mBox = value; } }
        }
        public enum Style : int
        {
            Default = 0,
            Error,
            Info,
            Allow,
            Deny,
            None
        }

        public stMessageBox()
        {
            this._stMessageBoxClean();
            this.mBox = this;
            this.pForm = null;
            this.tmClose = null;
            this.InitializeComponent();
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this._stMessageBoxGetForm(null) == null)
            {
                return;
            }
            this.stMessageBoxFormSet(this.pForm);
        }
        private void stMessageBox_Load(object sender, EventArgs e)
        {
            stMessageBox stBox = sender as stMessageBox;
            this.Dock = System.Windows.Forms.DockStyle.None;
            this.ForeColor = this.MboxText.ForeColor = stUI.stTheme.Txt();
            this.MboxButton.ForeColor = stTheme.White();
        }
        private void MboxButton_Click(object sender, EventArgs e)
        {
            this._stMessageBoxClean();
        }
        private System.Windows.Forms.Form _stMessageBoxGetForm(System.Windows.Forms.Control ctrl)
        {
            this.pForm = null;

            if (ctrl == null)
            {
                if (this.Parent != null)
                {
                    ctrl = this.Parent;
                }
            }
            if (ctrl is System.Windows.Forms.Form)
            {
                this.pForm = (System.Windows.Forms.Form)ctrl as System.Windows.Forms.Form;
            }
            else if (ctrl != null)
            {
                this.pForm = (System.Windows.Forms.Form)ctrl.FindForm();
            }
            this.pForm = ((this.pForm is System.Windows.Forms.Form) ? this.pForm : null);
            return this.pForm;
        }

        private void _stMessageBoxClean()
        {
            this._stMessageBoxTimerClose();

            if (
                (this.mBox != null) &&
                (!this.mBox.IsDisposed) &&
                (!this.mBox.Disposing)
               )
            {
                this.mBox.Dispose();
            }
            if (this.pForm != null)
            {
                this.pForm.Refresh();
                this.pForm = null;
            }
            this.mBox = null;
        }

        ///<summary>
        /// stMessageBox.Close() close opened stMessageBox control.
        ///</summary>
        public static void Close()
        {
            if (stMessageBox._mBox != null)
            {
                stMessageBox._mBox._stMessageBoxClean();
            }
        }

        private void stMessageBoxAutoClose(int sec)
        {
            this.tmClose = new System.Windows.Forms.Timer();
            this.tmClose.Interval = (sec * 1000);
            this.tmClose.Tick += (s, e) =>
            {
                stMessageBox.Close();
            };
            this.tmClose.Start();
        }

        private void _stMessageBoxTimerClose()
        {
            if (this.tmClose != null)
            {
                this.tmClose.Stop();
                this.tmClose.Dispose();
                this.tmClose = null;
            }
        }

        private static void _stMessageBoxInit(System.Windows.Forms.Control ctrl, object ostyle, string msg, string btn, int sec)
        {
            System.Windows.Forms.Form frm;

            if (ctrl == null)
            {
                return;
            }
            if ((stMessageBox._mBox = new stMessageBox()) == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                        global::stUI.Properties.Resources.UIExceptionHead,
                        global::stUI.Properties.Resources.MboxIsNull
                    )
                );
            }
            stMessageBox.Style style = ((ostyle is stMessageBox.Style) ?
                (stMessageBox.Style)ostyle : stMessageBox.Style.Error);

            if ((frm = (System.Windows.Forms.Form)stMessageBox._mBox._Dialog(ctrl, style, msg, btn, sec)) == null)
            {
                return;
            }
            frm.Controls.Add(stMessageBox._mBox);
            stMessageBox._mBox.BringToFront();
            stMessageBox._mBox.Update();
        }

        ///<summary>
        /// stMessageBox.Dialog(...) open & show stMessageBox control.
        ///     stMessageBox.Dialog(System.Windows.Forms.Control ParentFormControl, string Message)
        ///     stMessageBox.Dialog(System.Windows.Forms.Control ParentFormControl, string Message, int CloseSeconds)
        ///     stMessageBox.Dialog(System.Windows.Forms.Control ParentFormControl, string Message, string ButtonTitle)
        ///     stMessageBox.Dialog(System.Windows.Forms.Control ParentFormControl, string Message, string ButtonTitle, int CloseSeconds)
        ///     stMessageBox.Dialog(System.Windows.Forms.Control ParentFormControl, stMessageBox.Style ControlStyle, string Message, string ButtonTitle, int CloseSeconds)
        ///</summary>

        public static void Dialog(System.Windows.Forms.Control ctrl, string msg)
        {
            stMessageBox._stMessageBoxInit(ctrl, (object)stMessageBox.Style.None, msg, null, 0);
        }
        public static void Dialog(System.Windows.Forms.Control ctrl, string msg, int sec)
        {
            stMessageBox._stMessageBoxInit(ctrl, (object)stMessageBox.Style.None, msg, null, sec);
        }
        public static void Dialog(System.Windows.Forms.Control ctrl, string msg, string btn)
        {
            stMessageBox._stMessageBoxInit(ctrl, (object)stMessageBox.Style.None, msg, btn, 0);
        }
        public static void Dialog(System.Windows.Forms.Control ctrl, string msg, string btn, int sec)
        {
            stMessageBox._stMessageBoxInit(ctrl, (object)stMessageBox.Style.None, msg, btn, sec);
        }
        public static void Dialog(System.Windows.Forms.Control ctrl, object ostyle, string msg, string btn)
        {
            stMessageBox._stMessageBoxInit(ctrl, ostyle, msg, btn, 0);
        }
        public static void Dialog(System.Windows.Forms.Control ctrl, object ostyle, string msg, string btn, int sec)
        {
            stMessageBox._stMessageBoxInit(ctrl, ostyle, msg, btn, sec);
        }

        private System.Windows.Forms.Form _Dialog(System.Windows.Forms.Control ctrl, stMessageBox.Style style, string msg, string btn, int sec)
        {
            if (
                (ctrl == null) ||
                (this._stMessageBoxGetForm(ctrl) == null)
               )
            {
                throw new ArgumentNullException(
                    string.Format(
                        global::stUI.Properties.Resources.UIExceptionHead,
                        global::stUI.Properties.Resources.UIExceptionParentIsNull
                    )
                );
            }
            this.MboxButton.Text = ((string.IsNullOrEmpty(btn)) ?
                this._ButtonText(style) :
                btn
            );
            this.MboxText.Text = ((string.IsNullOrEmpty(msg)) ?
                global::stUI.Properties.Resources.MboxDefaultMsg
                : msg
            );
            this._Style(style);
            if (sec > 0)
            {
                this.stMessageBoxAutoClose(sec);
            }
            return this.pForm;
        }

        private void stMessageBoxFormSet(System.Windows.Forms.Form frm)
        {
            this.Width = frm.Width;
            this.Height = (((frm.Height / 4) >= HeightDefault) ? (frm.Height / 4) : HeightDefault);
            this.Location = new Point(0, ((frm.Height - this.Height) / 2));
            this.MboxText.MaximumSize = new Size(
                (((this.Width - (this.Width - this.MboxButton.Location.X)) + this.MboxButton.Width) - this.MboxText.Location.X),
                ((this.Height - (this.Height - this.MboxButton.Location.Y)) - this.MboxText.Location.Y)
            );
        }

        private void _Style(stMessageBox.Style style)
        {
            switch (style)
            {
                case Style.Error:
                    {
                        this.BackColor = this.MboxButton.BackColor = Color.Firebrick;
                        this.MboxPicture.Image = global::stUI.Properties.Resources.MboxError;
                        break;
                    }
                case Style.Info:
                    {
                        this.BackColor = this.MboxButton.BackColor = Color.OliveDrab;
                        this.MboxPicture.Image = global::stUI.Properties.Resources.MboxInfo;
                        break;
                    }
                case Style.Allow:
                    {
                        this.BackColor = this.MboxButton.BackColor = Color.Crimson;
                        this.MboxPicture.Image = global::stUI.Properties.Resources.MboxAllow;
                        break;
                    }
                case Style.Deny:
                    {
                        this.BackColor = this.MboxButton.BackColor = Color.Goldenrod;
                        this.MboxPicture.Image = global::stUI.Properties.Resources.MboxDeny;
                        break;
                    }
                default:
                    {
                        this.BackColor = this.MboxButton.BackColor = Color.Firebrick;
                        this.MboxPicture.Image = global::stUI.Properties.Resources.MboxDefault;
                        break;
                    }
            }
        }
        private string _ButtonText(stMessageBox.Style style)
        {
            switch (style)
            {
                case Style.Error:
                    {
                        return global::stUI.Properties.Resources.MboxErrorStr;
                    }
                case Style.Info:
                    {
                        return global::stUI.Properties.Resources.MboxInfoStr;
                    }
                case Style.Allow:
                    {
                        return global::stUI.Properties.Resources.MboxAllowStr;
                    }
                case Style.Deny:
                    {
                        return global::stUI.Properties.Resources.MboxDenyStr;
                    }
                default:
                    {
                        return global::stUI.Properties.Resources.MboxDefaultStr;
                    }
            }
        }
    }
}
