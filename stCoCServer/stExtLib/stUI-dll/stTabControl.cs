using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace stUI
{
    [DefaultProperty("Threshold"),
    DefaultEvent("ThresholdExceeded"),
    HelpKeywordAttribute(typeof(System.Windows.Forms.TabControl)),
    ToolboxItem("System.Windows.Forms.Design.AutoSizeToolboxItem,System.Design"),
    ToolboxBitmap(typeof(System.Windows.Forms.TabControl))]
    public partial class stTabControl : System.Windows.Forms.TabControl, IDisposable
    {
        const int TCM_ADJUSTRECT = 0x1328;
        private Color clrTabSeleted { get; set; }
        private Color clrTabUnSeleted { get; set; }
        private Color clrTabText { get; set; }

        //
        // Hidden override
        //
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DefaultValue(typeof(System.Windows.Forms.TabSizeMode), "Fixed"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new System.Windows.Forms.TabSizeMode SizeMode
        {
            get { return base.SizeMode; }
            set { base.SizeMode = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DefaultValue(typeof(Boolean), "false"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Multiline
        {
            get { return base.Multiline; }
            set { base.Multiline = value; }
        }
        [Browsable(false),
        EditorBrowsable(EditorBrowsableState.Never),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Size ItemSize
        {
            get { return base.ItemSize; }
            set { base.ItemSize = value; }
        }

        //
        // public
        //
        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(Boolean), "false"),
        Description("Hide/Show all Tab"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool TabHide { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(Boolean), "false"),
        Description("Wrap text in all Tab"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool TabTitleWrap { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(Boolean), "true"),
        Description("Auto size all Tab"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool TabAutoSize { get; set; }

        [Browsable(true),
        Bindable(true),
        Category("Appearance"),
        DefaultValue(typeof(Size), "100, 25"),
        Description("Fixed size all Tab"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public Size TabFixedSize
        {
            get
            {
                return base.ItemSize;
            }
            set
            {
                base.ItemSize = value;
            }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(int), "0"),
        Description("Total number of Tab"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public int TabNumber { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(Boolean), "false"),
        Description("ToolTip active all Tab"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public bool TabBalonTitle { get; set; }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(string), "#00A3E0"),
        Description("HTML active Tab color"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public string TabColorSeleted
        {
            get
            {
                return stUIUtil.ColorToHtml(this.clrTabSeleted);
            }
            set
            {
                this.clrTabSeleted = stUIUtil.ColorFromHtml(value);
            }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(string), "#006496"),
        Description("HTML passive Tab color"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public string TabColorUnSeleted
        {
            get
            {
                return stUIUtil.ColorToHtml(this.clrTabUnSeleted);
            }
            set
            {
                this.clrTabUnSeleted = stUIUtil.ColorFromHtml(value);
            }
        }

        [Browsable(true),
        Category("Appearance"),
        DefaultValue(typeof(string), "#FFFFFF"),
        Description("HTML text Tab color"),
        EditorBrowsable(EditorBrowsableState.Always)]
        public string TabColorText
        {
            get
            {
                return stUIUtil.ColorToHtml(this.clrTabText);
            }
            set
            {
                this.clrTabText = stUIUtil.ColorFromHtml(value);
            }
        }

        public stTabControl()
            : base()
        {
            this.InitAttrDefaults();

            this.clrTabText = stTheme.TabTxtWhite();
            this.clrTabSeleted = stTheme.TabActive();
            this.clrTabUnSeleted = stTheme.TabPassive();

            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true
            );

            /* 
             * Manual debug auto size mode:
             * this.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
             * this.TabAutoSize = true;
             */

            this.SetAutoItemSize();
            this.Invalidate();
        }
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (
                (this.TabHide) &&
                (m.Msg == TCM_ADJUSTRECT && !DesignMode)
               )
            {
                m.Result = (IntPtr)1;
            }
            else
            {
                base.WndProc(ref m);
            }
        }
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            this.SetAutoItemSize();
        }
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            this.SetAutoItemSize();
        }
        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlAdded(e);
            this.SetAutoItemSize();
        }
        protected override void OnControlRemoved(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            this.SetAutoItemSize();
        }
        protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs p)
        {
            base.OnPaintBackground(p);
            if (!this.DesignMode)
            {
                p.Graphics.Clear(Color.White);
            }
            if (this.TabCount <= 0)
            {
                return;
            }

            int cnt = 0;

            foreach (System.Windows.Forms.TabPage tab in this.TabPages)
            {
                Brush ctrlBrush;
                Rectangle tabRect = this.GetTabRect(this.TabPages.IndexOf(tab));

                if (this.SelectedIndex == cnt)
                {
                    ctrlBrush = new SolidBrush(this.clrTabSeleted);
                }
                else
                {
                    ctrlBrush = new SolidBrush(this.clrTabUnSeleted);
                }
                p.Graphics.FillRectangle(ctrlBrush, tabRect);
                ctrlBrush.Dispose();

                if (!string.IsNullOrWhiteSpace(tab.Text))
                {
                    if (this.TabBalonTitle)
                    {
                        tab.ToolTipText = tab.Text.Trim();
                    }
                    
                    p.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    Rectangle rotRect = new Rectangle(0, 0, tabRect.Height, tabRect.Width);

                    switch (this.Alignment)
                    {
                        case System.Windows.Forms.TabAlignment.Right:
                        case System.Windows.Forms.TabAlignment.Left:
                            {
                                DrawRotatedText(p.Graphics, -90, tab.Text.Trim(), tabRect.Left, tabRect.Bottom, rotRect);
                                break;
                            }
                        case System.Windows.Forms.TabAlignment.Top:
                        case System.Windows.Forms.TabAlignment.Bottom:
                            {
                                DrawRotatedText(p.Graphics, 0, tab.Text.Trim(), tabRect.Left, tabRect.Bottom, rotRect);
                                break;
                            }
                        default:
                            {
                                return;
                            }
                    }
                }
                else if (tab.ImageIndex >= 0)
                {
                    Image img = this.ImageList.Images[tab.ImageIndex];
                    float x = (tabRect.X + tabRect.Width) - img.Width - 3;
                    float y = ((tabRect.Height - img.Height) / 2.0f) + tabRect.Y;
                    p.Graphics.DrawImage(img, x, y);
                    img.Dispose();
                }
                cnt++;
            }
        }
        private void DrawRotatedText(Graphics g, float angle, string str, int x, int y, Rectangle rect)
        {
            GraphicsState state = g.Save();
            Brush brush = new SolidBrush(this.clrTabText);
            SizeF szTxt = g.MeasureString(str, this.Font);
            StringFormat sfmt = new StringFormat(
                StringFormatFlags.MeasureTrailingSpaces |
                StringFormatFlags.LineLimit
            );
            if (!this.TabTitleWrap)
            {
                sfmt.FormatFlags |= StringFormatFlags.NoWrap;
            }
            sfmt.Alignment = StringAlignment.Near;
            sfmt.LineAlignment = StringAlignment.Center;
            
            float yy = (float)((y - szTxt.Height) / 1.0);

            g.ResetTransform();

            if (angle == 0)
            {
                g.DrawString(str, this.Font, brush, x + 4, yy, sfmt);
            }
            else
            {
                g.RotateTransform(angle);
                g.TranslateTransform(x, yy, System.Drawing.Drawing2D.MatrixOrder.Append);
                g.DrawString(str, this.Font, brush, rect, sfmt);
                g.Restore(state);
            }
            g.Restore(state);
            brush.Dispose();
        }
        private void SetAutoItemSize()
        {
            if (this.TabCount <= 0)
            {
                this.ItemSize = new Size(0, 0);
                return;
            }
            if (this.TabAutoSize)
            {
                int  cnt = ((this.TabNumber < this.TabCount) ? this.TabCount : this.TabNumber);
                int  off = ((cnt > 0) ? (cnt * 3) : 3);
                Size csz = this.SizeFromClientSize(this.Size);

                switch (this.Alignment)
                {
                    case System.Windows.Forms.TabAlignment.Right:
                    case System.Windows.Forms.TabAlignment.Left:
                        {
                            this.ItemSize = new Size(
                                ((csz.Height / cnt) - off),
                                ((this.ImageList == null) ? 25 :
                                ((this.ImageList.ImageSize.Width > 25) ? this.ImageList.ImageSize.Width : 25))
                            );
                            break;
                        }
                    case System.Windows.Forms.TabAlignment.Top:
                    case System.Windows.Forms.TabAlignment.Bottom:
                        {
                            this.ItemSize = new Size(
                                ((csz.Width / cnt) - off),
                                ((this.ImageList == null) ? 25 :
                                ((this.ImageList.ImageSize.Height > 25) ? this.ImageList.ImageSize.Height : 25))
                            );
                            break;
                        }
                    default:
                        {
                            return;
                        }
                }
                this.Invalidate();
            }
        }
    }
}
