using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace stNet.Syslog
{
    /// <summary>
    /// the level of severity
    /// </summary>
    public enum Level
    {
        Emergency = 0,
        Alert = 1,
        Critical = 2,
        Error = 3,
        Warning = 4,
        Notice = 5,
        Information = 6,
        Debug = 7,
    }

    /// <summary>
    /// facility from where a message comes from
    /// </summary>
    public enum Facility
    {
        Kernel = 0,
        User = 1,
        Mail = 2,
        Daemon = 3,
        Auth = 4,
        Syslog = 5,
        Lpr = 6,
        News = 7,
        UUCP = 8,
        Clock = 9,
        Auth2 = 10,
        FTP = 11,
        NTP = 12,
        LogAudit = 13,
        LogAlert = 14,
        Clock2 = 15,
        Local0 = 16,
        Local1 = 17,
        Local2 = 18,
        Local3 = 19,
        Local4 = 20,
        Local5 = 21,
        Local6 = 22,
        Local7 = 23
    }

    public abstract class SysLogSender
    {
        internal abstract void SendMessage(byte[] data);
        internal abstract void Close();
    }

    public class SysLogSenderUdp : SysLogSender
    {
        private UdpClient client = null;
        public IPEndPoint SyslogServer { get; set; }

        /// <summary>
        /// Creates a sender for syslog class
        /// </summary>
        /// <param name="local">the local endpoint from where the client must send the datagrams</param>
        /// <param name="syslogServer">the server to wich it must send its datagrams</param>
        public SysLogSenderUdp(IPEndPoint local, IPEndPoint syslogServer)
        {
            this.client = new UdpClient(local);
            this.SyslogServer = syslogServer;
        }

        /// <summary>
        /// Creates a sender for the syslog class
        /// </summary>
        /// <param name="syslogServer"></param>
        public SysLogSenderUdp(IPEndPoint syslogServer)
        {
            this.client = new UdpClient();
            this.SyslogServer = syslogServer;
        }
        ~SysLogSenderUdp()
        {
            this.Close();
        }

        internal override void Close()
        {
            if (this.client != null)
            {
                this.client.Close();
            }
        }

        /// <summary>
        /// Sends a message.
        /// </summary>
        /// <remarks>This method should only be called from the syslog class</remarks>
        /// <param name="data">array with data</param>
        internal override void SendMessage(byte[] data)
        {
            client.Send(data, data.Length, SyslogServer);
        }
    }

    public class stSysLogNG
    {
        private SysLogSender _transport;
        private const string _NILVALUE = "-";
        private object _LockMsg = new Object();

        /// <summary>
        /// Application name that must be send with the messages
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// The process id that must be send with the messages. if this is not set, null or empty it wil use the PID.
        /// </summary>
        public string ProcID { get; set; }
        /// <summary>
        /// the protocol version
        /// </summary>
        public const int VERSION = 1;

        /// <summary>
        /// insert UTF8 Bom header
        /// </summary>
        public bool isUTF8Bom = true;

        /// <summary>
        /// Creates a Syslog client
        /// </summary>
        /// <param name="transport">The transport protocol that should be used.</param>
        public stSysLogNG(SysLogSender transport)
        {
            this._transport = transport;
        }
        ~stSysLogNG()
        {
            this.Close();
        }

        public void Close()
        {
            if (this._transport != null)
            {
                lock (this._LockMsg)
                {
                    this._transport.Close();
                }
            }
        }

        /// <summary>
        /// constructs a message with a set of parameters
        /// </summary>
        /// <param name="level"></param>
        /// <param name="facility"></param>
        /// <param name="messageID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private byte[] ConstructMessage(Level level, Facility facility, string messageID, string message = "")
        {
            int prival = ((int)facility) * 8 + ((int)level);
            string pri = string.Format("<{0}>", prival);
            string timestamp =
                new DateTimeOffset(DateTime.Now, TimeZoneInfo.Local.GetUtcOffset(DateTime.Now)).ToString("yyyy-MM-ddTHH:mm:ss.ffffffzzz");
            string hostname = Dns.GetHostEntry(Environment.UserDomainName).HostName;
            string appName = string.IsNullOrWhiteSpace(this.AppName) ? _NILVALUE : this.AppName;
            string procId = string.IsNullOrWhiteSpace(this.ProcID) ? Process.GetCurrentProcess().Id.ToString() : this.ProcID;
            string msgId = string.IsNullOrWhiteSpace(messageID) ? _NILVALUE : messageID;

            string header = string.Format("{0}{1} {2} {3} {4} {5} {6}", pri, VERSION, timestamp, hostname, appName, procId, msgId);

            List<byte> syslogMsg = new List<byte>();
            syslogMsg.AddRange(System.Text.Encoding.ASCII.GetBytes(header));
            syslogMsg.AddRange(System.Text.Encoding.ASCII.GetBytes(" - "));
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (isUTF8Bom)
                {
                    syslogMsg.AddRange(new byte[] { 0x20, 0xEF, 0xBB, 0xBF });
                }
                syslogMsg.AddRange(System.Text.Encoding.UTF8.GetBytes(message));
            }

            return syslogMsg.ToArray();
        }

        /// <summary>
        /// Sends a message to a syslog server
        /// </summary>
        /// <param name="level">The level of the message</param>
        /// <param name="facility">The facility from where it is send</param>
        /// <param name="messageId">The message id that must be send with the message</param>
        /// <param name="message">The message. This is optional as the protocol describes it.</param>
        public void SendMessage(Level level, Facility facility, string messageId, string message = "")
        {
            lock (this._LockMsg)
            {
                this._transport.SendMessage(ConstructMessage(level, facility, messageId, message));
            }
        }
    }
}
