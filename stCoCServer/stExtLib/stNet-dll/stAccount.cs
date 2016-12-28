using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stNet
{
    public class Account
    {
        public static string RandomNik(string template)
        {
            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                template = string.Concat(
                    template,
                    r.Next(10).ToString()
                );
            }
            return template;
        }
    }
}
