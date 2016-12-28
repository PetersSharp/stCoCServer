using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace stUI
{
    [DefaultProperty("Threshold"),
    DefaultEvent("ThresholdExceeded"),
    HelpKeywordAttribute(typeof(System.Windows.Forms.Button)),
    ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design"),
    ToolboxBitmap(typeof(System.Windows.Forms.Button))]
    public partial class stButton : System.Windows.Forms.Button
    {
        //
        // Hidden override
        //
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }

        //
        // public
        //
        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(string), "#006496"),
        Description("HTML Background Button color"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public string HTMLBackColor
        {
            get
            {
                return stUIUtil.ColorToHtml(this.BackColor);
            }
            set
            {
                this.BackColor = stUIUtil.ColorFromHtml(value);
            }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(string), "#F1F1F1"),
        Description("HTML Text Button color"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public string HTMLForeColor
        {
            get
            {
                return stUIUtil.ColorToHtml(this.ForeColor);
            }
            set
            {
                this.ForeColor = stUIUtil.ColorFromHtml(value);
            }
        }

        public stButton()
            : base()
        {
            this.stButtonInit(null, 0, new Size(0, 0), System.Windows.Forms.DockStyle.None);
        }
        public stButton(string name)
            : base()
        {
            this.stButtonInit(name, 0, new Size(0, 0), System.Windows.Forms.DockStyle.None);
        }
        public stButton(string name, int w)
            : base()
        {
            this.stButtonInit(name, w, new Size(0, 0), System.Windows.Forms.DockStyle.None);
        }
        public stButton(string name, System.Windows.Forms.DockStyle dock)
            : base()
        {
            this.stButtonInit(name, 0, new Size(0, 0), dock);
        }
        public stButton(string name, int w, System.Windows.Forms.DockStyle dock)
            : base()
        {
            this.stButtonInit(name, w, new Size(0, 0), dock);
        }
        public stButton(string name, int h, int w, System.Windows.Forms.DockStyle dock)
            : base()
        {
            this.stButtonInit(name, w, new Size(h, w), dock);
        }
        public stButton(string name, Size sz, System.Windows.Forms.DockStyle dock)
            : base()
        {
            this.stButtonInit(name, 0, sz, dock);
        }
        private void stButtonInit(string name, int w, Size sz, System.Windows.Forms.DockStyle dock)
        {
            this.InitAttrDefaults();

            this.BackColor = stTheme.Bg();
            this.ForeColor = stTheme.Txt();

            if (string.IsNullOrWhiteSpace(name))
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                this.Name = string.Format("stButton{0}", rnd);
            }
            else
            {
                this.Name = name;
            }

            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.UseVisualStyleBackColor = false;
            this.AutoEllipsis = true;
            this.Dock = dock;

            if (sz.IsEmpty)
            {
                if (w > 0)
                {
                    this.Width = w;
                    this.AutoSize = false;
                }
                else
                {
                    this.AutoSize = true;
                }
            }
            else
            {
                this.Size = sz;
            }
        }
    }
}
