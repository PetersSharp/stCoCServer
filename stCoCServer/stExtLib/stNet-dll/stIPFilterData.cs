using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace stNet
{
    public class stIPFilter
    {
        public List<stIpRange> IpRange { get; set; }
        public List<int> GeoDataASN { get; set; }
        public List<int> GeoDataCounry { get; set; }
        public stACL.IpFilterType GeoAsnFilterType { get; set; }
        public stACL.IpFilterType GeoCountryFilterType { get; set; }
        public stACL.IpFilterType IpFilterType { get; set; }
        public Func<uint, List<int>, int, bool, bool> GeoAction { get; set; }

        public stIPFilter()
        {
            this.IpRange = new List<stIpRange>();
            this.GeoDataASN = new List<int>();
            this.GeoDataCounry = new List<int>();
            this.IpFilterType = stACL.IpFilterType.None;
            this.GeoAsnFilterType = stACL.IpFilterType.None;
            this.GeoCountryFilterType = stACL.IpFilterType.None;
            this.GeoAction = (x, y, z, v) => { return false; };
        }
    }
    public class stIpRange
    {
        public uint ipStart;
        public uint ipEnd;

        public stIpRange()
        {
            this.ipStart = 0;
            this.ipEnd = 0;
        }
        public IPAddress IpAddrStart
        {
            get { return this.ipStart.IpUIntToIPAddress(); }
        }
        public IPAddress IpAddrEnd
        {
            get { return this.ipEnd.IpUIntToIPAddress(); }
        }
        public AddressFamily ipFamily
        {
            get { return this.ipStart.IpUIntToIPAddress().AddressFamily; }
        }
    }
}
