using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace stCoreUI
{
    /*
     * No good redraw group box..
     * 
    public class FlatListView : ListView
    {
        public FlatListView() : base()
        {
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(49,59,61);
            this.HeaderStyle = ColumnHeaderStyle.None;
            this.BorderStyle = BorderStyle.None;
            this.ForeColor = Color.Silver;
            this.Font = new Font("Segoe UI", 8);
            this.View = View.Details;
            this.UseCompatibleStateImageBehavior = true;
            this.FullRowSelect = true;
            this.MultiSelect = false;
            this.Scrollable = true;
            //this.OwnerDraw = true;
            //this.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(_DrawColumnHeader);
            //this.DrawItem += new DrawListViewItemEventHandler(_DrawItem);
            //this.DrawSubItem += new DrawListViewSubItemEventHandler(_DrawSubItem);
            
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            //e.Graphics.DrawString("(c) FlatListView", new Font(FontFamily.GenericSerif, 10, FontStyle.Bold), Brushes.White, new PointF(0, e.ClipRectangle.X));
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }
        
        private static void _DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            MessageBox.Show("AAA: " + e.Header.Text);
            e.Graphics.FillRectangle(new SolidBrush(Color.LawnGreen), e.Bounds);
            e.Graphics.DrawString(e.Header.Text, e.Font, new SolidBrush(Color.White), e.Bounds);
            e.Graphics.DrawString(e.Header.Text, new Font(FontFamily.GenericSerif, 10, FontStyle.Bold), Brushes.Black, new PointF(0, 300));
        }
        private void _DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            const int TEXT_OFFSET = 20;    // I don't know why the text is located at 1px to the right. Maybe it's only for me.

            ListView listView = (ListView)sender;

            // Check if e.Item is selected and the ListView has a focus.
            //if (!listView.Focused && e.Item.Selected)
            if (e.Item.Selected)
            {
                Rectangle rowBounds = e.SubItem.Bounds;
                Rectangle labelBounds = e.Item.GetBounds(ItemBoundsPortion.Label);
                int leftMargin = labelBounds.Left - TEXT_OFFSET;
                Rectangle bounds = new Rectangle(rowBounds.Left + leftMargin, rowBounds.Top, e.ColumnIndex == 0 ? labelBounds.Width : (rowBounds.Width - leftMargin - TEXT_OFFSET), rowBounds.Height);
                TextFormatFlags align;
                switch (listView.Columns[e.ColumnIndex].TextAlign)
                {
                    case HorizontalAlignment.Right:
                        {
                            align = TextFormatFlags.Right;
                            break;
                        }
                    case HorizontalAlignment.Center:
                        {
                            align = TextFormatFlags.HorizontalCenter;
                            break;
                        }
                    default:
                        {
                            align = TextFormatFlags.Left;
                            break;
                        }
                }
                TextRenderer.DrawText(e.Graphics, e.SubItem.Text, listView.Font, bounds, Color.Red,
                    align | TextFormatFlags.SingleLine | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
            }
            else
            {
                //e.DrawDefault = true;
                e.Graphics.FillRectangle(new SolidBrush(Color.Azure), e.Bounds);
                e.Graphics.DrawString(e.Header.Text, listView.Font, new SolidBrush(Color.BlueViolet), e.Bounds);
            }
        }
        private void _DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            ListView listView = (ListView)sender;

            // Check if e.Item is selected and the ListView has a focus.
            //if (!listView.Focused && e.Item.Selected)
            if (e.Item.Selected)
            {
                Rectangle rowBounds = e.Bounds;
                int leftMargin = e.Item.GetBounds(ItemBoundsPortion.Label).Left;
                Rectangle bounds = new Rectangle(leftMargin, rowBounds.Top, rowBounds.Width - leftMargin, rowBounds.Height);
                e.Graphics.FillRectangle(Brushes.GreenYellow, bounds);
            }
            else
            {
                //e.DrawDefault = true;
                Rectangle rowBounds = e.Bounds;
                int leftMargin = e.Item.GetBounds(ItemBoundsPortion.Label).Left;
                Rectangle bounds = new Rectangle(leftMargin, rowBounds.Top, rowBounds.Width, rowBounds.Height);
                e.Graphics.FillRectangle(Brushes.Green, bounds);
            }
        }
    }
    */
}
