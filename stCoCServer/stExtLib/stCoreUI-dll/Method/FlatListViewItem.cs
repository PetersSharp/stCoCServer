using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace stCoreUI
{
    public class FlatListViewItem : ListViewItem
    {
        private Color _clr = Color.FromArgb(60, 70, 73);

        public FlatListViewItem(ListView lv, string[] val)
            : base()
        {
            this.AddShaded(lv, null, val, String.Empty, _clr, lv.Items.Count);
        }
        public FlatListViewItem(ListView lv, ListViewGroup gv, string[] val)
            : base()
        {
            this.AddShaded(lv, gv, val, String.Empty, _clr, lv.Items.Count);
        }
        public FlatListViewItem(ListView lv, string[] val, string imgid)
            : base()
        {
            this.AddShaded(lv, null, val, imgid, _clr, lv.Items.Count);
        }
        public FlatListViewItem(ListView lv, string[] val, int p)
            : base()
        {
            this.AddShaded(lv, null, val, String.Empty, _clr, p);
        }
        public FlatListViewItem(ListView lv, string[] val, string imgid, int p)
            : base()
        {
            this.AddShaded(lv, null, val, imgid, _clr, p);
        }
        public FlatListViewItem(ListView lv, ListViewGroup gv, string[] val, string imgid, int p)
            : base()
        {
            this.AddShaded(lv, gv, val, imgid, _clr, p);
        }

        public FlatListViewItem(ListView lv, string[] val, Color clr)
            : base()
        {
            this.AddShaded(lv, null, val, String.Empty, clr, lv.Items.Count);
        }
        public FlatListViewItem(ListView lv, string[] val, Color clr, int p)
            : base()
        {
            this.AddShaded(lv, null, val, String.Empty, clr, p);
        }
        public FlatListViewItem(ListView lv, string[] val, string imgid, Color clr, int p)
            : base()
        {
            this.AddShaded(lv, null, val, imgid, clr, p);
        }
        public FlatListViewItem(ListView lv, ListViewGroup gv, string[] val, string imgid, Color clr, int p)
            : base()
        {
            this.AddShaded(lv, gv, val, imgid, clr, p);
        }

        public void AddShaded(ListView lv, ListViewGroup gv, string[] val, string imgid, Color clr, int p)
        {
            if (val.Length == 0)
            {
                return;
            }

            this.Text = val[0];

            for (int i = 1; i < val.Length; i++)
            {
                this.SubItems.Add(val[i]);
            }
            if ((p % 2) == 1)
            {
                this.BackColor = clr;
                this.UseItemStyleForSubItems = true;
            }
            if (!string.IsNullOrWhiteSpace(imgid))
            {
                this.ImageKey = imgid;
            }
            if (gv != null)
            {
                this.Group = gv;
            }
            try
            {
#if NOSAFECALL
                lv.Items.Add(this);
#else
                flatThreadSafe.Run(lv.FindForm(), (Action)(() => lv.Items.Add(this)));
#endif
            }
            catch (Exception) { }
        }
    }
}
