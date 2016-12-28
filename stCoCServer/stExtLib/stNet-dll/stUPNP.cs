
// #define STDEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Xml;
using stCore;

namespace stNet
{
    public class UPNPHostList : IEnumerable<stNet.UPNPHostList>
    {
        public int Id            { set; get; }
        public string Name       { set; get; }
        public string Desc       { set; get; }
        public string Copy       { set; get; }
        public string Version    { set; get; }
        public string urlRoot    { set; get; }
        public string urlBase    { set; get; }
        public string urlControl { set; get; }
        public string urlEvent   { set; get; }

        public IEnumerator<stNet.UPNPHostList> GetEnumerator()
        {
            return this.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

#if STDEBUG

    class UPNPTestData
    {
        public static string xmlTest =
"<?xml version=\"1.0\"?>" +
"<root xmlns=\"urn:schemas-upnp-org:device-1-0\">" +
"    <specVersion>" +
"        <major>1</major>" +
"        <minor>0</minor>" +
"    </specVersion>" +
"    <URLBase>base URL for all relative URLs</URLBase>" +
"    <device>" +
"        <deviceType>urn:schemas-upnp-org:device:Basic:1</deviceType>" +
"        <friendlyName>short user-friendly title</friendlyName>" +
"        <manufacturer>manufacturer name</manufacturer>" +
"        <manufacturerURL>URL to manufacturer site</manufacturerURL>" +
"        <modelDescription>long user-friendly title</modelDescription>" +
"        <modelName>model name</modelName>" +
"        <modelNumber>model number</modelNumber>" +
"        <modelURL>URL to model site</modelURL>" +
"        <serialNumber>manufacturer's serial number</serialNumber>" +
"        <UDN>uuid:UUID</UDN>" +
"        <UPC>Universal Product Code</UPC>" +
"        <iconList>" +
"            <icon>" +
"                <mimetype>image/format</mimetype>" +
"                <width>horizontal pixels</width>" +
"                <height>vertical pixels</height>" +
"                <depth>color depth</depth>" +
"                <url>URL to icon</url>" +
"            </icon>" +
"        </iconList>" +
"        <presentationURL>URL for presentation</presentationURL>" +
"    </device>" +
"</root>"; 
    }

#endif

    public class UPNP
    {
        private IPEndPoint MEPoint = null,
                           LEPoint = null;
        private Socket     sock = null;
        private byte[]     qSearch = null;
        private bool       disposed = false;
        private long       timeOut = 30000000;
        private string     strUUID = null;

        public Action<int> ProgressChanged = (x) => {};
        private stCore.IMessage IMsg;

        public long TimeOut {
            get { return this.timeOut; }
            set { this.timeOut = (value * 3000000); }
        }
        public string UUID
        {
            get { return this.strUUID; }
            set {
                    this.strUUID = value;
                    this.qSearch = this.ToUUID("urn:" + value + ":device:Basic:1");
                }
        }
        private byte[] ToUUID(string src)
        {
            return Encoding.ASCII.GetBytes(
                string.Format(
                    reqSearch,
                    src,
                    (this.timeOut / 10000000)
                )
            );
        }

        private const int ScanBuffSize = 64000;
        private const string strdev = "upnp:rootdevice";
        private const string urnSheme = "urn:schemas-upnp-org:device-1-0";
        private const string tnsUdn = "//tns:device/tns:UDN/text()";
        private const string tnsDeviceType = "//tns:device/tns:deviceType/text()";
        private const string reqSearch = "M-SEARCH * HTTP/1.1\r\n" +
            "HOST: 239.255.255.250:1900\r\n" +
            "MAN:\"ssdp:discover\"\r\n" +
            "ST:{0}\r\n" +
            "MX:{1}\r\n\r\n";

        private readonly List<string> tnsName = new List<string>()
        {
            "//tns:device/tns:friendlyName/text()",
            "//tns:device/tns:modelName/text()",
            "//tns:device/tns:manufacturer/text()"
        };

        private readonly List<string> tnsDesc = new List<string>()
        {
            "//tns:device/tns:modelDescription/text()"
        };

        private readonly List<string> tnsVersion = new List<string>()
        {
            "//tns:device/tns:modelNumber/text()",
            "//tns:device/tns:serialNumber/text()",
            "//tns:device/tns:UPC/text()",
            "//tns:device/tns:modelURL/text()"
        };

        private readonly List<string> tnsCopy = new List<string>()
        {
            "//tns:device/tns:manufacturer/text()",
            "//tns:device/tns:manufacturerURL/text()"
        };

        private readonly List<string> tnsURL = new List<string>()
        {
            "//tns:URLBase/text()",
            "//tns:device/tns:presentationURL/text()"
        };
       
        private readonly List<string> FieldName = new List<string>()
        {
            "location:",
            "usn:",
            "uuid:"
        };
        
        public enum Type : int
        {
            Broadcast,
            Multicast
        };

        public UPNP(stCore.IMessage iMsg)
        {
            this.IMsg = ((iMsg == null) ? (new stCore.IMessage()) : iMsg);
            this._InitUPNP(Type.Broadcast);
        }

        public UPNP(Type type, stCore.IMessage imsg)
        {
            this.IMsg = imsg;
            this._InitUPNP(type);
        }

        private UPNP _InitUPNP(Type type)
        {
            try
            {
                if ((this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)) == null)
                {
                    return null;
                }
                if ((this.LEPoint = new IPEndPoint(this.GetLocalAdress(), 0)) == null)
                {
                    return null;
                }
                this.sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                this.sock.Bind(LEPoint);
                this._SocketInit(type);
                if (this.MEPoint == null)
                {
                    return null;
                }
                this.TimeOut = 5;
                this.strUUID = null;
                this.qSearch = this.ToUUID(@"upnp:rootdevice");
                return this;
            }
            catch (Exception ex)
            {
                this.IMsg.ToLogAndLine(
                    System.Reflection.MethodBase.GetCurrentMethod(),
                    string.Format(
                        Properties.Resources.upnpError,
                        ex.Message
                    )
                );
                this.Dispose(true);
                return null;
            }
        }

        private void _SocketInit(Type type)
        {
            switch (type)
            {
                case Type.Broadcast:
                    {
                        this.MEPoint = new IPEndPoint(IPAddress.Broadcast, 1900);
                        this.sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                        break;
                    }
                case Type.Multicast:
                    {
                        this.MEPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);
                        this.sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(MEPoint.Address, IPAddress.Any));
                        this.sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 4);
                        this.sock.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);
                        break;
                    }
            }

        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.sock != null)
                    {
                        this.sock.Close();
                        this.sock = null;
                    }
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Close()
        {
            this.Dispose();
        }

        private IPAddress GetLocalAdress()
        {
            NetworkInterface[] netIfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface network in netIfaces)
            {
                IPInterfaceProperties properties = network.GetIPProperties();
                if (properties.GatewayAddresses.Count == 0)
                {
                    continue;
                }
                foreach (IPAddressInformation address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                    {
                        continue;
                    }

                    if (IPAddress.IsLoopback(address.Address))
                    {
                        continue;
                    }
                    return address.Address;
                }
            }
            return default(IPAddress);
        }

        private string GetHeadValue(string src, int idx)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return null;
            }
            string resp = src.Substring(src.IndexOf(this.FieldName[idx]) + this.FieldName[idx].Length);
            if (string.IsNullOrWhiteSpace(resp))
            {
                return null;
            }
            return resp.Substring(0, resp.IndexOf("\r")).Trim();
        }

        private string ConcatUrl(string src, string app)
        {
            int n = src.IndexOf("://");
            n = src.IndexOf('/', n + 3);
            return src.Substring(0, n) + app;
        }

        private string GetNodeValue(XmlDocument desc, XmlNamespaceManager nsMgr, List<string> search)
        {
            XmlNode node = null;
            foreach (string tns in (List<string>)search)
            {

                if (
                    ((node = desc.SelectSingleNode(tns, nsMgr)) != null) &&
                    (!string.IsNullOrWhiteSpace(node.Value))
                   )
                {
                    return node.Value;
                }
            }
            GC.SuppressFinalize(node);
            return null;
        }

        public IList<string> Discover()
        {
            IList<string> SSDPDevices = new List<string>();

            if (
                (this.sock == null) ||
                (this.qSearch == null) ||
                (this.MEPoint == null)
               )
            {
                return SSDPDevices;
            }

            long tCur, tMax;
            tCur = tMax = (long)DateTime.Now.Ticks;
            tMax += this.TimeOut;
            byte[] rBuffer = new byte[ScanBuffSize];
            int rBytes = 0,
                cnt = 0,
                num = 0;
            bool isNoUUID = string.IsNullOrWhiteSpace(this.strUUID);

            for (int i = 0; i < 3; i++)
            {
                this.sock.SendTo(this.qSearch, this.qSearch.Length, SocketFlags.None, this.MEPoint);
            }

            while (tCur <= tMax)
            {
                this.ProgressChanged(cnt++);

                while (this.sock.Available > 0)
                {
                    this.ProgressChanged(cnt++);

                    rBytes = this.sock.Receive(rBuffer, 0, ScanBuffSize, SocketFlags.None);
                    if (rBytes > 0)
                    {
                        string resp = null;
                        string head = Encoding.ASCII.GetString(rBuffer, 0, rBytes).ToLower();
                        if (string.IsNullOrWhiteSpace(head))
                        {
                            continue;
                        }
                        if (head.Contains(strdev))
                        {
                            switch (isNoUUID)
                            {
                                case false:
                                    {
                                        if (head.Contains(this.strUUID))
                                        {
                                            resp = this.GetHeadValue(head, 0);
                                        }
                                        break;
                                    }
                                case true:
                                    {
                                        resp = this.GetHeadValue(head, 0);
                                        break;
                                    }
                            }
                            if (!string.IsNullOrWhiteSpace(resp))
                            {
                                bool isDup = false;
                                foreach (string host in (List<string>)SSDPDevices)
                                {
                                    if (resp.Equals(host))
                                    {
                                        isDup = true;
                                        break;
                                    }
                                }
                                if (isDup)
                                {
                                    continue;
                                }
                                num++;
                                SSDPDevices.Add(resp);
                            }
                        }
                    }
                }
                Thread.Sleep(100);
                Thread.Yield();
                tCur = (long)DateTime.Now.Ticks;
            }
            this.ProgressChanged(0);
            this.IMsg.ToLogAndLine(
                System.Reflection.MethodBase.GetCurrentMethod(),
                string.Format(
                    Properties.Resources.upnpFindServer,
                    num
                )
            );
            return SSDPDevices;
        }

        public List<stNet.UPNPHostList> DiscoverBasicdevice()
        {
            List<stNet.UPNPHostList> dict = new List<stNet.UPNPHostList>();
            IList<string> hosts = this.Discover();
            if (hosts.Count == 0)
            {
                return null;
            }
            int cnt = 0;

            foreach (string host in (List<string>)hosts)
            {
                stNet.UPNPHostList resp = this.deviceBasicRequest(host);
                if (
                    (resp != null) &&
                    (!string.IsNullOrWhiteSpace(resp.urlBase))
                   )
                {
                    resp.Id = cnt++;
                    dict.Add(resp);
                }
            }
            GC.SuppressFinalize(hosts);
            return dict;
        }

        public stNet.UPNPHostList deviceBasicRequest(string uri)
        {
            stNet.UPNPHostList strarr = new stNet.UPNPHostList()
            {
                urlBase = null,
                urlControl = null,
                urlEvent = null,
                Desc = null
            };

            try
            {
                XmlNode node;
                XmlDocument desc = new XmlDocument();

                #if STDEBUG
                desc.LoadXml(UPNPTestData.xmlTest);
                #else
                desc.Load(WebRequest.Create(uri).GetResponse().GetResponseStream());
                #endif
                
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(desc.NameTable);
                nsMgr.AddNamespace("tns", urnSheme);
                if (
                    ((node = desc.SelectSingleNode(tnsDeviceType, nsMgr)) == null) ||
                    (!node.Value.Contains("device:Basic:1"))
                   )
                {
                    GC.SuppressFinalize(desc);
                    return strarr;
                }
                if (!string.IsNullOrWhiteSpace(this.strUUID))
                {
                    if (
                        ((node = desc.SelectSingleNode(tnsUdn, nsMgr)) != null)  &&
                        (!node.Value.Contains(this.strUUID))
                       )
                    {
                        GC.SuppressFinalize(desc);
                        return strarr;
                    }
                }
                /* SpeedTest URLRoot */
                strarr.urlRoot = uri;
                /* SpeedTest URLBase */
                strarr.urlBase = GetNodeValue(desc, nsMgr, this.tnsURL);
                /* Server Name */
                strarr.Name = GetNodeValue(desc, nsMgr, this.tnsName);
                /* Server Description */
                strarr.Desc = GetNodeValue(desc, nsMgr, this.tnsDesc);
                /* Server Version */
                strarr.Version = GetNodeValue(desc, nsMgr, this.tnsVersion);
                /* Server Copyright */
                strarr.Copy = GetNodeValue(desc, nsMgr, this.tnsCopy);

                GC.SuppressFinalize(desc);
                return strarr;
            }
            catch (Exception e)
            {
                this.IMsg.ToLogAndLine(
                    System.Reflection.MethodBase.GetCurrentMethod(),
                    string.Format(
                        Properties.Resources.upnpError,
                        e.Message
                    )
                );
                return strarr;
            }
        }

        public stNet.UPNPHostList deviceInternetGatewayRequest(string uri)
        {
            stNet.UPNPHostList strarr = new stNet.UPNPHostList()
            {
                Id = 0,
                Name = null,
                Desc = null,
                Copy = null,
                Version = null,
                urlRoot = null,
                urlBase = null,
                urlControl = null,
                urlEvent = null
            };

            try
            {
                XmlNode node;
                XmlDocument desc = new XmlDocument();
                List<string> tnsList;
                string tnsService = "//tns:service[tns:serviceType=\"urn:schemas-upnp-org:service:WANIPConnection:1\"]/tns:{0}/text()";
                string outValue;

                desc.Load(WebRequest.Create(uri).GetResponse().GetResponseStream());
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(desc.NameTable);
                nsMgr.AddNamespace("tns", urnSheme);
                if (
                    ((node = desc.SelectSingleNode(tnsDeviceType, nsMgr)) == null) ||
                    (!node.Value.Contains("InternetGatewayDevice"))
                   )
                {
                    GC.SuppressFinalize(desc);
                    return strarr;
                }

                /* SpeedTest URLRoot */
                strarr.urlRoot = uri;
                /* SpeedTest URLBase */
                strarr.urlBase = GetNodeValue(desc, nsMgr, this.tnsURL);
                /* Server Name */
                strarr.Name = GetNodeValue(desc, nsMgr, this.tnsName);
                /* Server Description */
                strarr.Desc = GetNodeValue(desc, nsMgr, this.tnsDesc);
                /* Server Version */
                strarr.Version = GetNodeValue(desc, nsMgr, this.tnsVersion);
                /* Server Copyright */
                strarr.Copy = GetNodeValue(desc, nsMgr, this.tnsCopy);
                /* Server Control URL */
                tnsList = new List<string>() {
                    string.Format(
                        tnsService,
                        "controlURL"
                    )
                };
                if (string.IsNullOrWhiteSpace(outValue = GetNodeValue(desc, nsMgr, tnsList)))
                {
                    tnsList.Clear();
                    GC.SuppressFinalize(desc);
                    return strarr;
                }
                strarr.urlControl = this.ConcatUrl(uri, outValue);
                /* Server Event URL */
                tnsList.Clear();
                tnsList = new List<string>() {
                    string.Format(
                        tnsService,
                        "eventSubURL"
                    )
                };
                if (string.IsNullOrWhiteSpace(outValue = GetNodeValue(desc, nsMgr, tnsList)))
                {
                    tnsList.Clear();
                    GC.SuppressFinalize(desc);
                    return strarr;
                }
                strarr.urlEvent = this.ConcatUrl(uri, outValue);
                tnsList.Clear();
                GC.SuppressFinalize(desc);
                return strarr;
            }
            catch (Exception ex)
            {
                this.IMsg.ToLogAndLine(
                    System.Reflection.MethodBase.GetCurrentMethod(),
                    string.Format(
                        Properties.Resources.upnpError,
                        ex.Message
                    )
                );
                return strarr;
            }
        }

    }
}
