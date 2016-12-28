using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stGeo
{
    [Serializable]
    public class stGeoRange
    {
        public uint ipStart = 0;
        public uint ipEnd = 0;
        public int  ipASNumber = 0;
        public int  ipCountry = 0;
    }

    [Serializable]
    public class stGeoCountry
    {
        public int Id = 0;
        public string Tag = String.Empty;
        public string Name = String.Empty;
    }
}
