﻿using System.IO;
using System.Web.UI;

namespace stDebug.Dumper
{
    //todo: should be called GeneralXhtmlRenderer
    public sealed class BasicXhtmlRenderer : IXhtmlRenderer
    {
        public bool Render(object o, string description, int depth, XhtmlTextWriter writer)
        {
            writer.WriteLine(o);
            return true;
        }
    }
}
