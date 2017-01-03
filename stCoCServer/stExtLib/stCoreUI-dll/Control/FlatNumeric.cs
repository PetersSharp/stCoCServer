using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace stCoreUI
{
    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);

    [DefaultProperty("Value"), 
    DefaultEvent("ValueChanged"),
    DefaultBindingProperty("Value")]
	public class FlatNumeric : Control
	{
		private int W;
		private int H;
		private MouseState _mouseState = MouseState.None;
		private int x;
		private int y;
		private long _Value;
		private long _Min;
		private long _Max;
		private bool Bool;

        private ValueChangedEventHandler _valueChanged = null;

        [Category("Action")]
        public event ValueChangedEventHandler ValueChanged
        {
            add
            {
                _valueChanged += value;
            }
            remove
            {
                _valueChanged -= value;
            }
        }
        protected virtual void OnValueChanged(ValueChangedEventArgs e)
        {
            ValueChangedEventHandler handler = _valueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [Category("Options")]
        [BindableAttribute(true)]
        public long Value
	    {
		    get { return _Value; }
		    set
		    {
                if ((value <= _Max) & (value >= _Min))
	            {
	                _Value = value;
                    OnValueChanged(new ValueChangedEventArgs(_Value));
	            }
			    Invalidate();
		    }
	    }

        [Category("Options")]
		public long Maximum
		{
			get { return _Max; }
			set
			{
				if (value > _Min)
					_Max = value;
				if (Value > _Max)
					Value = _Max;
				Invalidate();
			}
		}

        [Category("Options")]
		public long Minimum
		{
			get { return _Min; }
			set
			{
				if (value < _Max)
					_Min = value;
				if (Value < _Min)
					Value = Minimum;
				Invalidate();
			}
		}

        [Category("Mouse")]
        public MouseState MouseState
        {
            get { return this._mouseState; }
        }

        protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			x = e.Location.X;
			y = e.Location.Y;
			Invalidate();
			if (e.X < Width - 23)
				Cursor = Cursors.IBeam;
			else
				Cursor = Cursors.Hand;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (x > Width - 21 && x < Width - 3)
			{
				if (y < 15)
				{
					if ((Value + 1) <= _Max)
						Value += 1;
				}
				else
				{
					if ((Value - 1) >= _Min)
						Value -= 1;
				}
			}
			else
			{
				Bool = !Bool;
				Focus();
			}
			Invalidate();
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			try
			{
				if (Bool)
					Value = Convert.ToInt64(Value.ToString() + e.KeyChar.ToString());
				if (Value > _Max)
					Value = _Max;
				Invalidate();
			}
			catch
			{
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode == Keys.Back)
			{
				Value = 0;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Height = 30;
		}

		[Category("Colors")]
		public Color BaseColor
		{
			get { return _BaseColor; }
			set { _BaseColor = value; }
		}

		[Category("Colors")]
		public Color ButtonColor
		{
			get { return _ButtonColor; }
			set { _ButtonColor = value; }
		}

		private Color _BaseColor = Color.FromArgb(45, 47, 49);
		private Color _ButtonColor = Helpers.FlatColor;

		public FlatNumeric()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
			DoubleBuffered = true;
			Font = new Font("Segoe UI", 10);
			BackColor = Color.FromArgb(60, 70, 73);
			ForeColor = Color.White;
			_Min = 0;
			_Max = 9999999;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.UpdateColors();

			Bitmap B = new Bitmap(Width, Height);
			Graphics G = Graphics.FromImage(B);
			W = Width;
			H = Height;

			Rectangle Base = new Rectangle(0, 0, W, H);

			var _with18 = G;
			_with18.SmoothingMode = SmoothingMode.HighQuality;
			_with18.PixelOffsetMode = PixelOffsetMode.HighQuality;
			_with18.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			_with18.Clear(BackColor);

			//-- Base
			_with18.FillRectangle(new SolidBrush(_BaseColor), Base);
			_with18.FillRectangle(new SolidBrush(_ButtonColor), new Rectangle(Width - 24, 0, 24, H));

			//-- Add
			_with18.DrawString("+", new Font("Segoe UI", 12), Brushes.White, new Point(Width - 12, 8), Helpers.CenterSF);
			//-- Subtract
			_with18.DrawString("-", new Font("Segoe UI", 10, FontStyle.Bold), Brushes.White, new Point(Width - 12, 22), Helpers.CenterSF);

			//-- Text
			_with18.DrawString(Value.ToString(), Font, Brushes.White, new Rectangle(5, 1, W, H), new StringFormat { LineAlignment = StringAlignment.Center });

			base.OnPaint(e);
			G.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(B, 0, 0);
			B.Dispose();
		}

		private void UpdateColors()
		{
			FlatColors colors = Helpers.GetColors(this);

			_ButtonColor = colors.Flat;
		}
	}
}
