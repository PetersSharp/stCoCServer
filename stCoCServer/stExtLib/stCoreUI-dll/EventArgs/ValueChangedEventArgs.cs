using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCoreUI
{
    public class ValueChangedEventArgs : EventArgs
    {
        public long Value { get; set; }

        public ValueChangedEventArgs(long newValue)
        {
            Value = newValue;
        }
    }
}
