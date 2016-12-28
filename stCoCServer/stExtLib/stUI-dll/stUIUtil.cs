using System.Drawing;

namespace stUI
{
    public class stUIUtil
    {
        public static string ColorToHtml(Color clr)
        {

            if (clr != Color.Empty)
            {
                return System.Drawing.ColorTranslator.ToHtml(clr);
            }
            return "";
        }
        public static Color ColorFromHtml(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return System.Drawing.ColorTranslator.FromHtml(str);
            }
            return Color.Empty;
        }

    }
}
