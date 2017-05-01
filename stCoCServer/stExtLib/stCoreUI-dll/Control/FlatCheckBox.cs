﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace stCoreUI
{
	[DefaultEvent("CheckedChanged")]
	public class FlatCheckBox : Control
	{
		private int W;
		private int H;
		private MouseState State = MouseState.None;
		private _Options O;
		private bool _Checked = false;

        public event EventHandler CheckedChanged;

		protected override void OnTextChanged(System.EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}

        [Category("Options")]
		public bool Checked
		{
			get { return _Checked; }
			set
			{
				_Checked = value;
				Invalidate();
		        if (CheckedChanged != null)
				{
                	CheckedChanged(this, null);
				}
			}
		}

		protected override void OnClick(System.EventArgs e)
		{
			_Checked = !_Checked;
			if (CheckedChanged != null)
			{
				CheckedChanged(this,null);
			}
			base.OnClick(e);
		}

		[Flags()]
		public enum _Options
		{
			Style1,
			Style2
		}

		[Category("Options")]
		public _Options Options
		{
			get { return O; }
			set { O = value; }
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Height = 22;
		}

		[Category("Colors")]
		public Color BaseColor
		{
			get { return _BaseColor; }
			set { _BaseColor = value; }
		}

		[Category("Colors")]
		public Color BorderColor
		{
			get { return _BorderColor; }
			set { _BorderColor = value; }
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			State = MouseState.Down;
			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			State = MouseState.Over;
			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			State = MouseState.Over;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			State = MouseState.None;
			Invalidate();
		}

		private Color _BaseColor = Color.FromArgb(45, 47, 49);
		private Color _TextColor = Color.FromArgb(243, 243, 243);
		private Color _BorderColor = Helpers.FlatColor;

		public FlatCheckBox()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;
			BackColor = Color.FromArgb(60, 70, 73);
			Cursor = Cursors.Hand;
			Font = new Font("Segoe UI", 10);
			Size = new Size(112, 22);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			this.UpdateColors();

			Bitmap B = new Bitmap(Width, Height);
			Graphics G = Graphics.FromImage(B);
			W = Width - 1;
			H = Height - 1;

			Rectangle Base = new Rectangle(0, 2, Height - 5, Height - 5);

			var _with11 = G;
			_with11.SmoothingMode = SmoothingMode.HighQuality;
			_with11.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			_with11.Clear(BackColor);
			switch (O)
			{
				case _Options.Style1:
					//-- Style 1
					//-- Base
					_with11.FillRectangle(new SolidBrush(_BaseColor), Base);

					switch (State)
					{
						case MouseState.Over:
							//-- Base
							_with11.DrawRectangle(new Pen(_BorderColor), Base);
							break;
						case MouseState.Down:
							//-- Base
							_with11.DrawRectangle(new Pen(_BorderColor), Base);
							break;
					}

					//-- If Checked
					if (Checked)
					{
						_with11.DrawString("ü", new Font("Wingdings", 18), new SolidBrush(_BorderColor), new Rectangle(5, 7, H - 9, H - 9), Helpers.CenterSF);
					}

					//-- If Enabled
					if (this.Enabled == false)
					{
						_with11.FillRectangle(new SolidBrush(Color.FromArgb(54, 58, 61)), Base);
						_with11.DrawString(Text, Font, new SolidBrush(Color.FromArgb(140, 142, 143)), new Rectangle(20, 2, W, H), Helpers.NearSF);
					}

					//-- Text
					_with11.DrawString(Text, Font, new SolidBrush(_TextColor), new Rectangle(20, 2, W, H), Helpers.NearSF);
					break;
				case _Options.Style2:
					//-- Style 2
					//-- Base
					_with11.FillRectangle(new SolidBrush(_BaseColor), Base);

					switch (State)
					{
						case MouseState.Over:
							//-- Base
							_with11.DrawRectangle(new Pen(_BorderColor), Base);
							_with11.FillRectangle(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
							break;
						case MouseState.Down:
							//-- Base
							_with11.DrawRectangle(new Pen(_BorderColor), Base);
							_with11.FillRectangle(new SolidBrush(Color.FromArgb(118, 213, 170)), Base);
							break;
					}

					//-- If Checked
					if (Checked)
					{
						_with11.DrawString("ü", new Font("Wingdings", 18), new SolidBrush(_BorderColor), new Rectangle(5, 7, H - 9, H - 9), Helpers.CenterSF);
					}

					//-- If Enabled
					if (this.Enabled == false)
					{
						_with11.FillRectangle(new SolidBrush(Color.FromArgb(54, 58, 61)), Base);
						_with11.DrawString(Text, Font, new SolidBrush(Color.FromArgb(48, 119, 91)), new Rectangle(20, 2, W, H), Helpers.NearSF);
					}

					//-- Text
					_with11.DrawString(Text, Font, new SolidBrush(_TextColor), new Rectangle(20, 2, W, H), Helpers.NearSF);
					break;
			}

			base.OnPaint(e);
			G.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(B, 0, 0);
			B.Dispose();
		}

		private void UpdateColors()
		{
			FlatColors colors = Helpers.GetColors(this);

			_BorderColor = colors.Flat;
		}
	}
}