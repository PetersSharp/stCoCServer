using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCore
{
    [Serializable]
    public class TxtPosition
    {
        public Int32 start, end, length;

        public TxtPosition(Int32 s, Int32 l)
        {
            start = s;
            end = 0;
            length = l;
        }
        public TxtPosition(Int32 s, Int32 e, Int32 l)
        {
            start = s;
            end = e;
            length = l;
        }
    }
}
