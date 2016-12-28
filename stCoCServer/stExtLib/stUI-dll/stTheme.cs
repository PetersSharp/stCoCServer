using System.Drawing;

namespace stUI
{
    public class stTheme
    {
        public static Color Bg()
        {
            return stUIUtil.ColorFromHtml("#006496");
        }
        public static Color Txt()
        {
            return stUIUtil.ColorFromHtml("#F1F1F1");
        }

        public static Color White()
        {
            return stUIUtil.ColorFromHtml("#FFFFFF");
        }
        public static Color Black()
        {
            return stUIUtil.ColorFromHtml("#000000");
        }
        public static Color Transparent()
        {
            return System.Drawing.Color.Transparent;
        }

        public static Color TabTxtWhite()
        {
            return stTheme.White();
        }
        public static Color TabTxtBlack()
        {
            return stTheme.Black();
        }
        public static Color TabActive()
        {
            return stUIUtil.ColorFromHtml("#00A3E0");
        }
        public static Color TabPassive()
        {
            return stUIUtil.ColorFromHtml("#006496");
        }

        public static Color BgPanel1()
        {
            return stTheme.Bg();
        }
        public static Color TxtPanel1()
        {
            return stTheme.Txt();
        }
        public static Color BgPanel2()
        {
            return stTheme.Transparent();
        }
        public static Color TxtPanel2()
        {
            return stTheme.Black();
        }

    }
}
