using System;
using System.Net;
using System.Net.Sockets;

namespace stNet
{
    public static class stConvertIP
    {
        public static string IpUIntToString(this uint ip)
        {
            try
            {
                byte[] bytes = BitConverter.GetBytes(ip);
                Array.Reverse(bytes);
                return new IPAddress(bytes).ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " " + e.Source); 
            }
        }
        public static uint IpAddressToUInt(this IPAddress ipa)
        {
            try
            {
                byte[] ipb = (ipa).GetAddressBytes();
                Array.Reverse(ipb);
                return BitConverter.ToUInt32(ipb, 0);
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " " + e.Source);
            }
        }
        public static uint IpStringToUInt(this string ips)
        {
            try
            {
                byte[] ipb = IPAddress.Parse(ips).GetAddressBytes();
                Array.Reverse(ipb);
                return BitConverter.ToUInt32(ipb, 0);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " " + e.Source);
            }
        }
        public static IPAddress IpUIntToIPAddress(this uint ip)
        {
            try
            {
                byte[] ipb = BitConverter.GetBytes(ip);
                Array.Reverse(ipb);
                return new IPAddress(ipb);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " " + e.Source);
            }
        }
    }
}
