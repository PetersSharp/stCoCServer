using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;

namespace stNet
{
    public static class stACL
    {
        private const uint localHost = 2130706433;
        public enum IpFilterType : int
        {
            None = 0,
            GeoASN,
            GeoCountry,
            IPRange,
            BlackList,
            WhiteList
        };

        #region private method

        private static void _IpFilterAddIpRange(this List<stIpRange> IpListRange, string aip)
        {
            string [] ipRange = new string[2] { "", "" };

            if (string.IsNullOrWhiteSpace(aip))
            {
                throw new ArgumentNullException(Properties.Resources.aclExceptionAddress);
            }
            if (aip.Contains("-"))
            {
                ipRange = aip.Split('-');
            }
            else if (aip.Contains("/"))
            {
                uint mask, startIp, endIp;
                ipRange = aip.Split('/');

                if (ipRange.Length >= 2)
                {
                    if (ipRange[1].Length > 2)
                    {
                        mask = ipRange[1].IpStringToUInt();
                        startIp = (ipRange[0].IpStringToUInt() & mask);
                        endIp = (startIp | (mask ^ 0xffffffff));
                    }
                    else
                    {
                        int cidr = Convert.ToInt32(ipRange[1]);
                        mask = ~(0xFFFFFFFF >> cidr);
                        startIp = (ipRange[0].IpStringToUInt() & mask);
                        endIp = (startIp | (mask ^ 0xffffffff));
                    }
                    if (endIp <= 0)
                    {
                        throw new ArgumentOutOfRangeException(Properties.Resources.aclExceptioConvertIpEnd);
                    }
                    ipRange[0] = startIp.IpUIntToString();
                    ipRange[1] = endIp.IpUIntToString();
#if DEBUGIPRANGE
                    Console.Write("start ip: " + startIp.IpUIntToString() + "\r\n");
                    Console.Write("end ip:   " + endIp.IpUIntToString() + "\r\n");
                    Console.Write("mask:     " + mask.IpUIntToString() + "\r\n");
#endif
                }
            }
            else
            {
                ipRange[0] = aip;
                ipRange[1] = aip;
            }
            if (ipRange.Length < 2)
            {
                throw new ArgumentOutOfRangeException(Properties.Resources.aclExceptioSourceFormat);
            }
            stACL._IpFilterAddIpRange(IpListRange, ipRange[0], ipRange[1]);
        }
        private static void _IpFilterAddIpRange(this List<stIpRange> IpListRange, string bip, string eip)
        {
            try
            {
                if (IpListRange == null)
                {
                    throw new ArgumentNullException(Properties.Resources.aclExceptionList);
                }
                if ((string.IsNullOrWhiteSpace(bip)) || (string.IsNullOrWhiteSpace(eip)))
                {
                    throw new ArgumentNullException(Properties.Resources.aclExceptionAddress);
                }
                IpListRange.Add(
                    new stIpRange()
                    {
                        ipStart = bip.IpStringToUInt(),
                        ipEnd   = eip.IpStringToUInt()
                    }
                );
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " " + e.Source);
            }
        }
        private static stIPFilter _IpFilterCreate(
            StringCollection strIPList,
            StringCollection strASNList,
            StringCollection strCountryList,
            bool isIpBlackList,
            bool isGeoAsnBlackList,
            bool isGeoCountryBlackList,
            Func<uint, List<int>, int, bool, bool> GeoAction,
            Func<string, int> GeoCountryFunc
            )
        {
            stIPFilter IpFilter = new stIPFilter();

            try
            {
                if ((strIPList != null) && (strIPList.Count > 0))
                {
                    foreach (string rip in strIPList)
                    {
                        if (string.IsNullOrWhiteSpace(rip))
                        {
                            continue;
                        }
                        stACL._IpFilterAddIpRange(IpFilter.IpRange, rip);
                    }
                    IpFilter.IpFilterType = ((isIpBlackList) ? stACL.IpFilterType.BlackList : stACL.IpFilterType.WhiteList);
                }
                if ((strASNList != null) && (strASNList.Count > 0))
                {
                    foreach (string num in strASNList)
                    {
                        string asn;
                        if (num.StartsWith("ASN"))
                        {
                            asn = num.Substring(3, (num.Length - 3));
                        }
                        else if (num.StartsWith("AS"))
                        {
                            asn = num.Substring(2, (num.Length - 2));
                        }
                        else
                        {
                            asn = num;
                        }
                        int n = 0;
                        if (Int32.TryParse(asn.Trim(), out n))
                        {
                            IpFilter.GeoDataASN.Add(n);
                        }
                    }
                    IpFilter.GeoAsnFilterType = ((isGeoAsnBlackList) ? stACL.IpFilterType.BlackList : stACL.IpFilterType.WhiteList);
                }
                if ((strCountryList != null) && (strCountryList.Count > 0))
                {
                    foreach (string num in strCountryList)
                    {
                        int n = 0;
                        string country = num.Trim();
                        if (Int32.TryParse(country, out n))
                        {
                            IpFilter.GeoDataCounry.Add(n);
                        }
                        else
                        {
                            if ((GeoCountryFunc != null) && (country.Length == 2))
                            {
                                if ((n = GeoCountryFunc(country)) != -1)
                                {
                                    IpFilter.GeoDataCounry.Add(n);
                                }
                            }
                        }
                    }
                    IpFilter.GeoCountryFilterType = ((isGeoCountryBlackList) ? stACL.IpFilterType.BlackList : stACL.IpFilterType.WhiteList);
                }
                if (GeoAction != null)
                {
                    IpFilter.GeoAction = GeoAction;
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
            return IpFilter;
        }

        #endregion

        #region IpFilterCreate

        public static stIPFilter IpFilterCreate(
            StringCollection strCollection,
            bool isBlackList = true
            )
        {
            return stACL.IpFilterCreate(
                strCollection,
                IpFilterType.IPRange,
                isBlackList,
                null
            );
        }
        /// <summary>
        /// Create IpFilter
        /// </summary>
        /// <param name="srcList">Source IpRange/ASN/Country based StringCollection</param>
        /// <param name="listType"></param>
        /// <param name="isBlackList"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static stIPFilter IpFilterCreate(
            StringCollection strCollection,
            stACL.IpFilterType listType,
            bool isBlackList = true,
            Func<uint, List<int>, int, bool, bool> act = null,
            Func<string, int> func = null
            )
        {
            return stACL._IpFilterCreate(
                ((listType == IpFilterType.IPRange) ? strCollection : null),
                ((listType == IpFilterType.GeoASN) ? strCollection : null),
                ((listType == IpFilterType.GeoCountry) ? strCollection : null),
                ((listType == IpFilterType.IPRange) ? isBlackList : true),
                ((listType == IpFilterType.GeoASN) ? isBlackList : true),
                ((listType == IpFilterType.GeoCountry) ? isBlackList : true),
                act,
                func
            );
        }
        /// <summary>
        /// Create IpFilter
        /// </summary>
        /// <param name="geoCollection">IP based List string</param>
        /// <param name="IpBlackList"></param>
        /// <returns></returns>
        public static stIPFilter IpFilterCreate(
            List<string> ipList,
            bool IpBlackList = true
            )
        {
            return stACL.IpFilterCreate(
                ipList,
                IpFilterType.IPRange,
                IpBlackList,
                null,
                null
            );
        }
        /// <summary>
        /// Create IpFilter
        /// </summary>
        /// <param name="srcList">Source IpRange/ASN/Country based List string</param>
        /// <param name="listType"></param>
        /// <param name="isBlackList"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static stIPFilter IpFilterCreate(
            List<string> srcList,
            stACL.IpFilterType listType,
            bool isBlackList = true,
            Func<uint, List<int>, int, bool, bool> act = null,
            Func<string, int> func = null
            )
        {
            StringCollection geoCollection = null;

            if ((srcList != null) && (srcList.Count > 0))
            {
                geoCollection = new StringCollection();
                geoCollection.AddRange(srcList.ToArray());
            }
            return stACL._IpFilterCreate(
                ((listType == IpFilterType.IPRange) ? geoCollection : null),
                ((listType == IpFilterType.GeoASN) ? geoCollection : null),
                ((listType == IpFilterType.GeoCountry) ? geoCollection : null),
                ((listType == IpFilterType.IPRange) ? isBlackList : true),
                ((listType == IpFilterType.GeoASN) ? isBlackList : true),
                ((listType == IpFilterType.GeoCountry) ? isBlackList : true),
                act,
                func
            );
        }
        /// <summary>
        /// Create IpFilter
        /// </summary>
        /// <param name="srcList">Source ASN/Country based List int</param>
        /// <param name="listType"></param>
        /// <param name="isBlackList"></param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static stIPFilter IpFilterCreate(
            List<int> srcList,
            stACL.IpFilterType listType,
            bool isBlackList = true,
            Func<uint, List<int>, int, bool, bool> act = null,
            Func<string, int> func = null
            )
        {
            StringCollection geoCollection = null;

            if (
                ((srcList != null) && (srcList.Count > 0)) &&
                ((listType == IpFilterType.GeoASN) || (listType == IpFilterType.GeoCountry))
               )
            {
                geoCollection = new StringCollection();
                foreach (int num in srcList)
                {
                    geoCollection.Add(num.ToString());
                }
            }
            return stACL._IpFilterCreate(
                null,
                ((listType == IpFilterType.GeoASN) ? geoCollection : null),
                ((listType == IpFilterType.GeoCountry) ? geoCollection : null),
                true,
                ((listType == IpFilterType.GeoASN) ? isBlackList : true),
                ((listType == IpFilterType.GeoCountry) ? isBlackList : true),
                act,
                func
            );
        }
        /// <summary>
        /// Create IpFilter
        /// Full based StringCollection
        /// </summary>
        /// <param name="strIPList">IP Range source</param>
        /// <param name="strASNList">Geo ASN source</param>
        /// <param name="strCountryList">Geo Country source</param>
        /// <param name="isIpBlackList">Boolean, Ip source is BlackList</param>
        /// <param name="isGeoBlackList">Boolean, Geo source is BlackList</param>
        /// <param name="act"></param>
        /// <returns></returns>
        public static stIPFilter IpFilterCreate(
            StringCollection strIPList,
            StringCollection strASNList,
            StringCollection strCountryList,
            bool isIpBlackList = true,
            bool isGeoAsnBlackList = true,
            bool isGeoCountryBlackList = true,
            Func<uint, List<int>, int, bool, bool> act = null,
            Func<string, int> func = null
            )
        {
            return stACL._IpFilterCreate(strIPList, strASNList, strCountryList, isIpBlackList, isGeoAsnBlackList, isGeoCountryBlackList, act, func);
        }

        #endregion

        #region IpFilterIpRange

        /// <summary>
        /// Check IP in list Ip Range (stIpRange)
        /// </summary>
        /// <param name="IpListRange">structure List stIpRange</param>
        /// <param name="ips">IP string</param>
        /// <param name="isBlackList">bool is BlackList true, White list false</param>
        /// <returns></returns>
        public static bool IpFilterIpRange(this List<stIpRange> IpListRange, string ips, bool isBlackList = true)
        {
            if (string.IsNullOrWhiteSpace(ips))
            {
                return false;
            }

            uint ipa = 0;
            
            try
            {
                ipa = ips.IpStringToUInt();
                if (ipa == 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return stACL._IpFilterIpRange(IpListRange, ipa, isBlackList);
        }
        public static bool IpFilterIpRange(this List<stIpRange> IpListRange, uint ipa, bool isBlackList = true)
        {
            if (ipa == 0)
            {
                return false;
            }
            return stACL._IpFilterIpRange(IpListRange, ipa, isBlackList);
        }
        public static bool IpFilterIpRange(this List<stIpRange> IpListRange, uint ipa, stACL.IpFilterType ipListType = stACL.IpFilterType.BlackList)
        {
            if (ipa == 0)
            {
                return false;
            }
            return stACL._IpFilterIpRange(IpListRange, ipa, ((ipListType == stACL.IpFilterType.WhiteList) ? false : true));
        }
        private static bool _IpFilterIpRange(List<stIpRange> IpListRange, uint ipa, bool isBlackList)
        {
            if ((IpListRange == null) || (IpListRange.Count == 0))
            {
                return false;
            }

            foreach (stIpRange acl in (List<stIpRange>)IpListRange)
            {
                if ((ipa >= acl.ipStart) && ((ipa <= acl.ipEnd)))
                {
                    return ((isBlackList) ? true : false);
                }
            }
            return ((isBlackList) ? false : true);
        }

        #endregion

        #region IpFilterCheck

        /// <summary>
        /// Check IP in stIPFilter structure set
        /// </summary>
        /// <param name="ipData">stIPFilter structure set</param>
        /// <param name="ipa">IP <see cref="System.Net.IPAddress"/></param>
        /// <returns>Boolean is found reverse true/false for Black or White list</returns>
        public static bool IpFilterCheck(this stIPFilter ipData, IPAddress ipa)
        {
            if ((ipa == null) || (ipData == null))
            {
                return false;
            }
            
            uint ipu = 0;

            try
            {
                ipu = ipa.IpAddressToUInt();
                if (ipu == 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return stACL.IpFilterCheck(ipData, ipu);
        }
        /// <summary>
        /// Check IP in stIPFilter structure set
        /// </summary>
        /// <param name="ipData">stIPFilter structure set</param>
        /// <param name="ips">IP string</param>
        /// <returns>Boolean is found reverse true/false for Black or White list</returns>
        public static bool IpFilterCheck(this stIPFilter ipData, string ips)
        {
            if ((string.IsNullOrWhiteSpace(ips)) || (ipData == null))
            {
                return false;
            }

            uint ipu = 0;

            try
            {
                ipu = ips.IpStringToUInt();
                if (ipu == 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return stACL.IpFilterCheck(ipData, ipu);
        }
        /// <summary>
        /// Check IP in stIPFilter structure set
        /// </summary>
        /// <param name="ipData">stIPFilter structure set</param>
        /// <param name="ipu">IP unsigned int, produced IpStringToUInt()</param>
        /// <returns>Boolean is found reverse true/false for Black or White list</returns>
        public static bool IpFilterCheck(this stIPFilter ipData, uint ipu = 0)
        {
            if (ipu == 0)
            {
                return true;
            }
            if ((ipu == localHost) || (ipData == null))
            {
                return false;
            }

            bool ret = false;
            bool cond = ((ipData.IpFilterType == IpFilterType.WhiteList) ? false : true);

            if (ipData.IpRange.Count > 0)
            {
                ret = ipData.IpRange.IpFilterIpRange(
                    ipu,
                    cond
                );
            }
            if (
                (((ret) && (!cond)) || ((!ret) && (cond))) &&
                (ipData.GeoDataASN.Count > 0)
               )
            {
                cond = ((ipData.GeoAsnFilterType == IpFilterType.WhiteList) ? false : true);
                /// prototype stGeo.GeoFilter.InGeoRange(
                ///     uint ip address,
                ///     List<int> list numer asn or contry,
                ///     int type (int)enum stACL.IpFilterType asn or contry,
                ///     bool isBlackList)
                ret = ipData.GeoAction(
                    ipu,
                    ipData.GeoDataASN,
                    (int)stACL.IpFilterType.GeoASN,
                    cond
                );
            }
            if (
                (((ret) && (!cond)) || ((!ret) && (cond))) &&
                (ipData.GeoDataCounry.Count > 0)
               )
            {
                cond = ((ipData.GeoCountryFilterType == IpFilterType.WhiteList) ? false : true);
                ret = ipData.GeoAction(
                    ipu,
                    ipData.GeoDataCounry,
                    (int)stACL.IpFilterType.GeoCountry,
                    cond
                );
            }
            return ret;
        }

        #endregion
    }
}
