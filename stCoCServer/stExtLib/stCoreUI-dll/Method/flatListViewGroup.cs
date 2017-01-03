using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace stCoreUI
{
    public class FlatListViewGroup
    {
        private ListViewGroup lvg;

        public FlatListViewGroup(ListView lv, string[] val, HorizontalAlignment align)
            : base()
        {
            this.lvg = new ListViewGroup();
            this.lvg.Name = val[0];
            this.lvg.Header = val[1];
            this.lvg.HeaderAlignment = align;
            try
            {
#if NOSAFECALL
                lv.Groups.Add(this.lvg);
#else
                flatThreadSafe.Run(lv.FindForm(), (Action)(() => lv.Groups.Add(this.lvg)));
#endif
            }
            catch (Exception) { }
        }
        public ListViewGroup ListViewGroup()
        {
            return this.lvg;
        }
    }
}
