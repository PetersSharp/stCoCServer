using System.IO;
using System.Web.UI;

namespace stDebug.Dumper
{
    public interface IXhtmlRenderer
    {
        bool Render(object o, string description, int depth, XhtmlTextWriter writer);
    }
}
