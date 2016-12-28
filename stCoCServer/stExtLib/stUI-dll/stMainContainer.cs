using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace stUI
{
    [DefaultProperty("Threshold"),
    DefaultEvent("ThresholdExceeded"),
    HelpKeywordAttribute(typeof(System.Windows.Forms.SplitContainer)),
    ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design"),
    ToolboxBitmap(typeof(System.Windows.Forms.SplitContainer))]
    public partial class stMainContainer : System.Windows.Forms.SplitContainer
    {
        //
        // Hidden override
        //
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Color ForeColor
        {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override System.Drawing.Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsSplitterFixed
        {
            get { return base.IsSplitterFixed; }
            set { base.IsSplitterFixed = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new System.Windows.Forms.FixedPanel FixedPanel
        {
            get { return base.FixedPanel; }
            set { base.FixedPanel = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new System.Windows.Forms.Orientation Orientation
        {
            get { return base.Orientation; }
            set { base.Orientation = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set { base.BorderStyle = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int SplitterWidth
        {
            get { return base.SplitterWidth; }
            set { base.SplitterWidth = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int SplitterIncrement
        {
            get { return base.SplitterIncrement; }
            set { base.SplitterIncrement = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int SplitterDistance
        {
            get { return base.SplitterDistance; }
            set { base.SplitterDistance = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Panel1MinSize
        {
            get { return base.Panel1MinSize; }
            set { base.Panel1MinSize = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Panel2MinSize
        {
            get { return base.Panel2MinSize; }
            set { base.Panel2MinSize = value; }
        }
        
        //
        // local
        //
        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.Timer), "null"),
        Description("ToglePanel animation timer")]
        private System.Windows.Forms.PictureBox pbPanelTogle { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.ToolStripContainer), "null"),
        Description("toolStripContainer StausBar instance")]
        private System.Windows.Forms.ToolStripContainer toolStripStausBar { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.Timer), "null"),
        Description("ToglePanel animation timer")]
        private System.Windows.Forms.Timer tmColapse { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.Panel), "null"),
        Description("ref right Panel body to add Controls")]
        private System.Windows.Forms.Panel PanelBody { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.Windows.Forms.Panel), "null"),
        Description("ref added Controls to right Panel body")]
        private System.Windows.Forms.UserControl ControlBody { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.EventHandler), "null"),
        Description("ToglePanel animation timer EventHandler")]
        private System.EventHandler evColapse { get; set; }

        [Browsable(false),
        DefaultValue(typeof(System.EventHandler), "null"),
        Description("ToglePanel click EventHandler")]
        private System.EventHandler evTogleClick { get; set; }

        [Browsable(false),
        DefaultValue(typeof(int), "0"),
        Description("ToglePanel animation internal counter")]
        private int aniPanel { get; set; }

        [Browsable(false),
        DefaultValue(typeof(int), "0"),
        Description("ToglePanel size step")]
        private int sizePanel { get; set; }

        //
        // public
        //
        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(int), "70"),
        Description("Max Panel1 size"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int Panel1DefaultSize
        {
            get { return this.sizePanel;  }
            set { this.sizePanel = ((value <= 0) ? 70 : value); }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(int), "20"),
        Description("Panel1 animation msec per tick. Default 20 msec"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int Panel1Ani  { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(Boolean), "false"),
        Description("Hide/Show  Panel1 change disabled"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool Panel1TogleDisabled { get; set; }

        [Browsable(false),
        DefaultValue(typeof(Boolean), "false"),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Description("its run background worker, disable change Panel2 UserControl")]
        public bool ControlChangeDisabled { get; set; }

        public stMainContainer()
            : base()
        {
            this.InitAttrDefaults();

            this.aniPanel = 0;
            this.tmColapse = null;
            this.evColapse = null;
            this.DoubleBuffered = true;

            this.BackColor = stTheme.Transparent();
            this.ForeColor = stTheme.Black();
            this.Panel1.BackColor = stTheme.BgPanel1();
            this.Panel1.ForeColor = stTheme.TxtPanel1();
            this.Panel2.BackColor = stTheme.BgPanel2();
            this.Panel2.ForeColor = stTheme.TxtPanel2();

            this.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.IsSplitterFixed = true;

            this.Panel1Ani = 20;
            this.Panel1DefaultSize = 70;
            this.SplitterWidth = 1;
            this.SplitterIncrement = 1;

            this.Panel1Collapsed = false;
            this.Panel2Collapsed = false;
            this.Panel1MinSize = this.Panel1DefaultSize;
            this.Panel2MinSize = (this.Width - this.Panel1DefaultSize - 1);
            this.SplitterDistance = this.Panel1DefaultSize;

            this.pbPanelTogle = new System.Windows.Forms.PictureBox();
            this.pbPanelTogle.BackColor = stTheme.Transparent();
            this.pbPanelTogle.Cursor = System.Windows.Forms.Cursors.PanWest;
            this.pbPanelTogle.Image = global::stUI.Properties.Resources.PanelTogle;
            this.pbPanelTogle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbPanelTogle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbPanelTogle.Size = new System.Drawing.Size(5,15);
            this.pbPanelTogle.Name = "PbPanelTogle";
            this.pbPanelTogle.TabStop = false;
            this.pbPanelTogle.Dock = System.Windows.Forms.DockStyle.Left;
            this.evTogleClick = new System.EventHandler(this.pbPanelTogle_Click);
            this.pbPanelTogle.Click += this.evTogleClick;
            this.Panel2.Controls.Add(pbPanelTogle);

            this.PanelBody = new System.Windows.Forms.Panel();
            this.PanelBody.Name = "PanelBody";
            this.PanelBody.BackColor = stTheme.Transparent();
            this.PanelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel2.Controls.Add(this.PanelBody);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.panelTimerFree();

            if (
                (this.pbPanelTogle != null) &&
                (!this.pbPanelTogle.Disposing)
               )
            {
                if (this.evTogleClick != null)
                {
                    this.pbPanelTogle.Click -= this.evTogleClick;
                }
                this.pbPanelTogle.Dispose();
                GC.SuppressFinalize(this.pbPanelTogle);
                this.pbPanelTogle = null;
            }
            if (
                (this.PanelBody != null) &&
                (!this.PanelBody.Disposing)
               )
            {
                this.PanelBody.Dispose();
                GC.SuppressFinalize(this.PanelBody);
                this.PanelBody = null;
            }
        }

        private void _ToglePanel(object sender, EventArgs e)
        {
            if (this.tmColapse != null)
            {
                return;
            }
            
            this.tmColapse = new System.Windows.Forms.Timer();
            this.tmColapse.Interval = this.Panel1Ani;

            switch (this.Panel1Collapsed)
            {
                case true:
                    {
                        this.aniPanel = 0;
                        this.evColapse = new System.EventHandler(this.PanelTickUp);
                        this.tmColapse.Tick += this.evColapse;
                        break;
                    }
                case false:
                    {
                        this.aniPanel = this.Panel1DefaultSize;
                        this.evColapse = new System.EventHandler(this.PanelTickDown);
                        this.tmColapse.Tick += this.evColapse;
                        break;
                    }
            }
            this.Panel1Collapsed = false;
            this.tmColapse.Start();
        }

        private void PanelTickUp(object sender, EventArgs e)
        {
            if (this.aniPanel >= this.sizePanel)
            {
                this.pbPanelTogle.Cursor = System.Windows.Forms.Cursors.PanWest;
                this.panelTimerFree();
                return;
            }
            this.aniPanel += 10;
            this.SplitterDistance = this.Panel1MinSize =
                ((this.aniPanel > this.sizePanel) ? this.sizePanel : this.aniPanel);
            this.Panel2MinSize = (this.Width - this.Panel1MinSize);
            System.Windows.Forms.Application.DoEvents();
        }

        private void PanelTickDown(object sender, EventArgs e)
        {
            if (this.aniPanel <= 0)
            {
                this.Panel1Collapsed = true;
                this.pbPanelTogle.Cursor = System.Windows.Forms.Cursors.PanEast;
                this.panelTimerFree();
                return;
            }
            this.aniPanel -= 10;
            this.SplitterDistance = this.Panel1MinSize = 
                ((this.aniPanel < 0) ? 0 : this.aniPanel);
            this.Panel2MinSize = (this.Width - this.Panel1MinSize - 1);
            System.Windows.Forms.Application.DoEvents();
        }

        private void pbPanelTogle_Click(object sender, EventArgs e)
        {
            if (!this.Panel1TogleDisabled)
            {
                this._ToglePanel(sender, e);
            }
        }

        private void panelTimerFree()
        {
            if (this.tmColapse == null)
            {
                return;
            }
            this.tmColapse.Stop();

            if (this.evColapse != null)
            {
                this.tmColapse.Tick -= this.evColapse;
            }
            this.tmColapse.Dispose();
            this.tmColapse = null;
            this.evColapse = null;
        }

        public void ToglePanel()
        {
            if (!this.Panel1TogleDisabled)
            {
                this._ToglePanel(null, null);
            }
        }

        public void TogleDisabled()
        {
            if (this.pbPanelTogle == null)
            {
                return;
            }
            this.Panel1TogleDisabled = true;
            this.pbPanelTogle.Visible = false;
            this.pbPanelTogle.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbPanelTogle.Invalidate();
        }
        public void TogleEnabled()
        {
            if (this.pbPanelTogle == null)
            {
                return;
            }
            this.Panel1TogleDisabled = false;
            this.pbPanelTogle.Visible = true;
            this.pbPanelTogle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbPanelTogle.Invalidate();
        }

        public void TogleShow()
        {
            if (this.Panel1Collapsed)
            {
                this._ToglePanel(this, null);
            }
        }

        public void TogleHide()
        {
            if (!this.Panel1Collapsed)
            {
                this._ToglePanel(this, null);
            }
        }

        private void _CleanNewControl(System.Windows.Forms.Control ctrl)
        {
            if (
                (ctrl != null) &&
                (!ctrl.Disposing)
               )
            {
                ctrl.Dispose();
                ctrl = null;
            }
        }

        private bool _CheckControl(System.Windows.Forms.Control ctrl)
        {
            if (
                (this.PanelBody == null) ||
                (ctrl == null)
               )
            {
                return false;
            }
            return true;
        }

        public void AddBody(System.Windows.Forms.UserControl ctrl)
        {
            this._AddBody(ctrl);
            this.TogleHide();
        }

        public void AddBodyByName(System.Windows.Forms.UserControl ctrl, string name)
        {
            if (!this._CheckControl((System.Windows.Forms.Control)ctrl))
            {
                return;
            }
            if (this.CheckControlByName(name))
            {
                this._CleanNewControl(ctrl);
                return;
            }
            this._AddBody(ctrl);
            this.TogleHide();
        }

        public void AddBodyByType(System.Windows.Forms.UserControl ctrl, Type type)
        {
            if (!this._CheckControl((System.Windows.Forms.Control)ctrl))
            {
                return;
            }
            if (this.CheckControlByType(type))
            {
                this._CleanNewControl(ctrl);
                return;
            }
            this._AddBody(ctrl);
            this.TogleHide();
        }

        private bool _AddBody(System.Windows.Forms.UserControl ctrl)
        {
            if (this.ControlChangeDisabled)
            {
                throw new ArgumentException(
                    string.Format(
                        global::stUI.Properties.Resources.UIExceptionHead,
                        global::stUI.Properties.Resources.UIExceptionBusy
                    )
                );
            }
            if (!this._CheckControl((System.Windows.Forms.Control)ctrl))
            {
                this._CleanNewControl(ctrl);
                return false;
            }
            this._ClearBody();
            this.ControlBody = (System.Windows.Forms.UserControl)ctrl;
            this.ControlBody.Padding = new System.Windows.Forms.Padding(10, 5, 0, 0);
            this.PanelBody.Controls.Add((System.Windows.Forms.UserControl)this.ControlBody);
            this.PanelBody.Invalidate();
            return true;
        }

        private bool _ClearBody()
        {
            if (this.ControlChangeDisabled)
            {
                throw new ArgumentException(
                    string.Format(
                        global::stUI.Properties.Resources.UIExceptionHead,
                        global::stUI.Properties.Resources.UIExceptionBusy
                    )
                );
            }
            if (this.PanelBody == null)
            {
                return false;
            }
            this._CleanNewControl(this.ControlBody);
            this.ControlBody = null;
            this.PanelBody.Controls.Clear();
            this.PanelBody.Invalidate();
            return true;
        }

        public void ClearBody()
        {
            if (!this._ClearBody())
            {
                return;
            }
            if (this.Panel1Collapsed)
            {
                this._ToglePanel(this, null);
            }
        }

        public void ClearBodyByName(string name)
        {
            if (this.CheckControlByName(name))
            {
                 this.ClearBody();
            }
        }

        public bool CheckControlByName(string name)
        {
            if (
                (this.PanelBody != null) &&
                (this.ControlBody != null) &&
                (!this.ControlBody.Disposing) &&
                (!string.IsNullOrWhiteSpace(name)) &&
                (this.ControlBody.GetType().ToString().Equals(name))
               )
            {
                return true;
            }
            return false;
        }

        public bool CheckControlByType(Type type)
        {
            if (
                (type == null) ||
                (this.ControlBody == null) ||
                (this.ControlBody.Disposing)
               )
            {
                return false;
            }
            return this.ControlBody.GetType().Equals(type);
        }

        public System.Windows.Forms.UserControl ReturnControlRef()
        {
            if (
                (this.ControlBody == null) ||
                (this.ControlBody.Disposing)
               )
            {
                throw new ArgumentNullException();
            }
            return (System.Windows.Forms.UserControl)this.ControlBody;
        }

        public Type ReturnControlType()
        {
            if (
                (this.ControlBody == null) ||
                (this.ControlBody.Disposing)
               )
            {
                throw new ArgumentNullException();
            }
            return (Type)this.ControlBody.GetType();
        }

    }
}
