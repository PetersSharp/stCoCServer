﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace stCoreUI
{
	public class FlatColorPalette : Control
	{
		private int W;
		private int H;
		private const string _caption = "Color Palette";

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Width = 180;
			Height = 80;
		}

		[Category("Colors")]
		public Color Red
		{
			get { return _Red; }
			set { _Red = value; }
		}

		[Category("Colors")]
		public Color Cyan
		{
			get { return _Cyan; }
			set { _Cyan = value; }
		}

		[Category("Colors")]
		public Color Blue
		{
			get { return _Blue; }
			set { _Blue = value; }
		}

		[Category("Colors")]
		public Color LimeGreen
		{
			get { return _LimeGreen; }
			set { _LimeGreen = value; }
		}

		[Category("Colors")]
		public Color Orange
		{
			get { return _Orange; }
			set { _Orange = value; }
		}

		[Category("Colors")]
		public Color Purple
		{
			get { return _Purple; }
			set { _Purple = value; }
		}

		[Category("Colors")]
		public Color Black
		{
			get { return _Black; }
			set { _Black = value; }
		}

		[Category("Colors")]
		public Color Gray
		{
			get { return _Gray; }
			set { _Gray = value; }
		}

		[Category("Colors")]
		public Color White
		{
			get { return _White; }
			set { _White = value; }
		}

		private Color _Red = Color.FromArgb(220, 85, 96);
		private Color _Cyan = Color.FromArgb(10, 154, 157);
		private Color _Blue = Color.FromArgb(0, 128, 255);
		private Color _LimeGreen = Color.FromArgb(35, 168, 109);
		private Color _Orange = Color.FromArgb(253, 181, 63);
		private Color _Purple = Color.FromArgb(155, 88, 181);
		private Color _Black = Color.FromArgb(45, 47, 49);
		private Color _Gray = Color.FromArgb(63, 70, 73);
		private Color _White = Color.FromArgb(243, 243, 243);

		public FlatColorPalette()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;
			BackColor = Color.FromArgb(60, 70, 73);
			Size = new Size(160, 80);
			Font = new Font("Segoe UI", 12);
			Text = ((string.IsNullOrWhiteSpace(Text)) ? _caption : Text);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Bitmap B = new Bitmap(Width, Height);
			Graphics G = Graphics.FromImage(B);
			W = Width - 1;
			H = Height - 1;

			var _with6 = G;
			_with6.SmoothingMode = SmoothingMode.HighQuality;
			_with6.PixelOffsetMode = PixelOffsetMode.HighQuality;
			_with6.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			_with6.Clear(BackColor);

			//-- Colors 
			_with6.FillRectangle(new SolidBrush(_Red), new Rectangle(0, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_Cyan), new Rectangle(20, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_Blue), new Rectangle(40, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_LimeGreen), new Rectangle(60, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_Orange), new Rectangle(80, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_Purple), new Rectangle(100, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_Black), new Rectangle(120, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_Gray), new Rectangle(140, 0, 20, 40));
			_with6.FillRectangle(new SolidBrush(_White), new Rectangle(160, 0, 20, 40));

			//-- Text
			_with6.DrawString(Text, Font, new SolidBrush(_White), new Rectangle(0, 22, W, H), Helpers.CenterSF);

			base.OnPaint(e);
			G.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(B, 0, 0);
			B.Dispose();
		}
	}
}
