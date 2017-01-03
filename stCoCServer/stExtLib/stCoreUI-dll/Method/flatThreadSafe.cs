using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace stCoreUI
{
    public class flatThreadSafe
    {
        public static void Run(Form frm, Action act)
        {
            if (frm == null)
            {
                return;
            }
            if (frm.InvokeRequired)
            {
                frm.BeginInvoke(act);
            }
            else
            {
                act();
            }
        }
    }
}
