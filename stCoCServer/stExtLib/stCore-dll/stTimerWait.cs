using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace stCore
{
    public class stTimerWait
    {
        public Timer timer = null;
        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }
        }
    }
}
