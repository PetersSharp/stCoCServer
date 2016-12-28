using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCore
{
    public static class stRuntime
    {
        public static bool isRunTime()
        {
            return (bool)(Type.GetType("Mono.Runtime") != null);
        }
    }
}
