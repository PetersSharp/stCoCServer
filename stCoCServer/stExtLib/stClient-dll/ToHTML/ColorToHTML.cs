using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace stClient
{
    public static class ColorToHTML
    {
        public static string ColorToHEX(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static string ColorToRGB(this Color c)
        {
            return "RGB(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString() + ")";
        }
    }
}
