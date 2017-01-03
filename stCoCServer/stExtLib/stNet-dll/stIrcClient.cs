using System;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using stCore;


namespace stNet
{
    /// <summary>
    /// IRC Client class written at:
    ///     http://tech.reboot.pro
    ///     (https://github.com/cshivers/IrcClient-csharp)
    /// </summary>
    public class IrcClient : IDisposable
    {
        #region VARIABLES

        private const string clasName = @"IRC -> {0}";

        private IMessage _ilog = null;

        // default server
        private string _server = @"irc.freenode.net";

        // default port
        private int _port = 6667;

        private string _ServerPass = "";

        // default nick
        private string _nick = "Test";

        // default alternate nick
        private string _altNick = "";

        private string _outBanner = "";

        private string _ircchannel = "";

        //default IRC kick respawn mode
        private bool _kickrespawn = true;

        //default consoleOutput mode
        private bool _consoleOutput = false;

        // private TcpClient used to talk to the server
        private TcpClient _irc = null;

        // private network stream used to read/write from/to
        private NetworkStream _stream = null;

        // global variable used to read input from the client
        private string _inputLine;

        // stream reader to read from the network stream
        private StreamReader _reader = null;

        // stream writer to write to the stream
        private StreamWriter _writer = null;

        // AsyncOperation used to handle cross-thread wonderness
        private AsyncOperation op;

        private bool _isRun = false;
        private CancellationTokenSource _canceler = null;

        #endregion

        #region Constructors

        /// <summary>
        /// IrcClient used to connect to an IRC Server (default port: 6667)
        /// </summary>
        /// <param name="Server">IRC Server</param>
        public IrcClient(string Server) : this(Server, 6667) { }

        /// <summary>
        /// IrcClient used to connect to an IRC Server
        /// </summary>
        /// <param name="Server">IRC Server</param>
        /// <param name="Port">IRC Port (6667 if you are unsure)</param>
        public IrcClient(string Server, int Port)
        {
            op = AsyncOperationManager.CreateOperation(null);
            this._server = Server;
            this._port = Port;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the Server address used
        /// </summary>
        public string Server
        {
            get { return _server; }
        }
        /// <summary>
        /// Returns the Port used
        /// </summary>
        public int Port
        {
            get { return _port; }
        }
        /// <summary>
        /// Returns the password used to auth to the server
        /// </summary>
        public string ServerPass
        {
            get { return _ServerPass; }
            set { _ServerPass = value; }
        }
        /// <summary>
        /// Returns the current nick being used.
        /// </summary>
        public string Nick
        {
            get { return _nick; }
            set { _nick = value; }
        }
        /// <summary>
        /// Returns the alternate nick being used
        /// </summary>
        public string AltNick
        {
            get { return _altNick; }
            set { _altNick = value; }
        }
        /// <summary>
        /// Channel exit append message
        /// </summary>
        public string OutBanner
        {
            get { return _outBanner; }
            set { _outBanner = value; }
        }
        /// <summary>
        /// Output RAW IRC data to console
        /// </summary>
        public bool ConsoleOutput
        {
            get { return _consoleOutput; }
            set { _consoleOutput = value; }
        }
        /// <summary>
        /// IRC KICK respawn mode
        /// </summary>
        public bool KickRespawn
        {
            get { return _kickrespawn; }
            set { _kickrespawn = value; }
        }
        /// <summary>
        /// Logging/printing instance <see cref="stCore.IMessage"/> class
        /// Depended stCore.dll
        /// </summary>
        public IMessage iLog
        {
            get { return this._ilog; }
            set { if (value != null) { this._ilog = value; } }
        }
        /// <summary>
        /// Returns true if the client is connected.
        /// </summary>
        public bool Connected
        {
            get
            {
                if (_irc != null)
                    if (_irc.Connected)
                        return true;
                return false;
            }
        }
        #endregion

        #region Events

        public event EventHandler<UpdateUsersEventArgs> OnUserUpdate = delegate { };
        public event EventHandler<UserJoinedEventArgs> OnUserJoined = delegate { };
        public event EventHandler<UserLeftEventArgs> OnUserLeft = delegate { };
        public event EventHandler<UserKickEventArgs> OnUserKick = delegate { };
        public event EventHandler<UserNickChangedEventArgs> OnUserNickChange = delegate { };

        public event EventHandler<ChannelMessageEventArgs> OnMessageChannel = delegate { };
        public event EventHandler<NoticeMessageEventArgs> OnMessageNotice = delegate { };
        public event EventHandler<PrivateMessageEventArgs> OnMessagePrivate = delegate { };
        public event EventHandler<StringEventArgs> OnMessageServer = delegate { };

        public event EventHandler<StringEventArgs> OnNickTaken = delegate { };

        public event EventHandler OnConnect = delegate { };

        public event EventHandler<ExceptionEventArgs> OnExceptionThrown = delegate { };

        public event EventHandler<ModeSetEventArgs> OnChannelModeSet = delegate { };

        private void Fire_UpdateUsers(UpdateUsersEventArgs o)
        {
            op.Post(x => OnUserUpdate(this, (UpdateUsersEventArgs)x), o);
        }
        private void Fire_UserJoined(UserJoinedEventArgs o)
        {
            op.Post(x => OnUserJoined(this, (UserJoinedEventArgs)x), o);
        }
        private void Fire_UserLeft(UserLeftEventArgs o)
        {
            op.Post(x => OnUserLeft(this, (UserLeftEventArgs)x), o);
        }
        private void Fire_UserKick(UserKickEventArgs o)
        {
            op.Post(x => OnUserKick(this, (UserKickEventArgs)x), o);
        }
        private void Fire_NickChanged(UserNickChangedEventArgs o)
        {
            op.Post(x => OnUserNickChange(this, (UserNickChangedEventArgs)x), o);
        }
        private void Fire_ChannelMessage(ChannelMessageEventArgs o)
        {
            op.Post(x => OnMessageChannel(this, (ChannelMessageEventArgs)x), o);
        }
        private void Fire_NoticeMessage(NoticeMessageEventArgs o)
        {
            op.Post(x => OnMessageNotice(this, (NoticeMessageEventArgs)x), o);
        }
        private void Fire_PrivateMessage(PrivateMessageEventArgs o)
        {
            op.Post(x => OnMessagePrivate(this, (PrivateMessageEventArgs)x), o);
        }
        private void Fire_ServerMesssage(string s)
        {
            op.Post(x => OnMessageServer(this, (StringEventArgs)x), new StringEventArgs(s));
        }
        private void Fire_NickTaken(string s)
        {
            op.Post(x => OnNickTaken(this, (StringEventArgs)x), new StringEventArgs(s));
        }
        private void Fire_Connected()
        {
            op.Post((x) => OnConnect(this, null), null);
        }
        private void Fire_ExceptionThrown(Exception ex)
        {
            op.Post(x => OnExceptionThrown(this, (ExceptionEventArgs)x), new ExceptionEventArgs(ex));
        }
        private void Fire_ChannelModeSet(ModeSetEventArgs o)
        {
            op.Post(x => OnChannelModeSet(this, (ModeSetEventArgs)x), o);
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Connect to the IRC server
        /// </summary>
        public void Connect()
        {
            Thread t = new Thread(DoConnect) { IsBackground = true };
            t.Start();
        }
        private void DoConnect()
        {
            if (this._isRun)
            {
                Fire_ExceptionThrown(
                    new ArgumentException(
                        string.Format(
                            Properties.Resources.IRCAlreadyRunException,
                            this._server,
                            this._port
                        )
                    )
                );
                return;
            }
            if (this._canceler == null)
            {
                this._canceler = new CancellationTokenSource();
            }

            this._isRun = true;

            while (this._isRun)
            {
                try
                {
                    this.DoConnection();
                }
                catch (OperationCanceledException e)
                {
                    if (this.iLog != null)
                    {
                        this.iLog.LogError(
                            string.Format(
                                IrcClient.clasName,
                                e.Message
                            )
                        );
                    }
                    break;
                }
                catch (IOException e)
                {
                    if (this.iLog != null)
                    {
                        this.iLog.LogError(
                            string.Format(
                                IrcClient.clasName,
                                e.Message
                            )
                        );
                    }
                    System.Threading.Thread.Sleep(5000);
                    if (this.iLog != null)
                    {
                        this.iLog.LogInfo(
                            string.Format(
                                IrcClient.clasName,
                                "reConnect: " + this._server + ":" + this._port
                            )
                        );
                    }
                    continue;
                }
                catch (Exception e)
                {
#if DEBUG_DoConnect
                    stConsole.WriteHeader("* IRC DoConnect Exception: " + e.Message);
                    stConsole.WriteHeader("* IRC DoConnect Exception: " + e.ToString());
#endif
                    Fire_ExceptionThrown(e);
                    return;
                }
                finally
                {
                    this.Disconnect();
                }
            }

        }

        private void DoConnection()
        {
            try
            {
                this._irc = new TcpClient(this._server, this._port);
                this._stream = this._irc.GetStream();
                this._reader = new StreamReader(this._stream);
                this._writer = new StreamWriter(this._stream);

                if (!string.IsNullOrEmpty(this._ServerPass))
                {
                    Send("PASS " + this._ServerPass);
                }
                Send("NICK " + this._nick);
                Send("USER " + this._nick + " 0 * :" + _nick);

                Listen();
            }
            catch (Exception e)
            {
                Fire_ExceptionThrown(e);
            }
        }

        /// <summary>
        /// Disconnect from the IRC server
        /// </summary>
        public void Disconnect(bool isRun = false)
        {
            if (this._irc != null)
            {
                if (this._irc.Connected)
                {
                    Send(
                        string.Format(
                            Properties.Resources.IRCDisconnectedMessage,
                            this._outBanner
                        )
                    );
                }
                if ((isRun) && (this._canceler != null))
                {
                    this._isRun = false;
                    this._canceler.Cancel();
                }
                this._irc.Close();
                this._irc = null;
                if (this.iLog != null)
                {
                    this.iLog.LogInfo(
                        string.Format(
                            IrcClient.clasName,
                            "Disconnect: " + this._server
                        )
                    );
                }
            }
        }
        /// <summary>
        /// Sends the JOIN command to the server
        /// </summary>
        /// <param name="channel">Channel to join</param>
        public void JoinChannel(string channel)
        {
            if (string.IsNullOrWhiteSpace(channel))
            {
                return;
            }
            if (!channel.Equals(this._ircchannel))
            {
                this._ircchannel = channel;
            }
            if ((this._irc != null) && (this._irc.Connected))
            {
                Send("JOIN " + channel);
            }
        }
        /// <summary>
        /// Sends the PART command for a given channel
        /// </summary>
        /// <param name="channel">Channel to leave</param>
        public void PartChannel(string channel)
        {
            Send("PART " + channel);
        }
        /// <summary>
        /// Send a notice to a user
        /// </summary>
        /// <param name="toNick">User to send the notice to</param>
        /// <param name="message">The message to send</param>
        public void SendNotice(string toNick, string message)
        {
            Send("NOTICE " + toNick + " :" + message);
        }

        /// <summary>
        /// Send a message to the channel
        /// </summary>
        /// <param name="channel">Channel to send message</param>
        /// <param name="message">Message to send</param>
        public void SendMessage(string channel, string message)
        {
            Send("PRIVMSG " + channel + " :" + message);
        }
        /// <summary>
        /// Send a new Topic to the channel
        /// </summary>
        /// <param name="channel">Channel to send message</param>
        /// <param name="message">Topic string to send</param>
        public void SendTopic(string channel, string message)
        {
            Send("TOPIC " + channel + " :" + message);
        }
        /// <summary>
        /// Set mode to user of the channel
        /// </summary>
        /// <param name="channel">Channel to send message</param>
        /// <param name="message">Topic string to send</param>
        public void SendMode(string channel, string message)
        {
            Send("MODE " + channel + " :" + message);
        }
        /// <summary>
        /// Send RAW IRC commands
        /// </summary>
        /// <param name="message"></param>
        public void SendRaw(string message)
        {
            Send(message);
        }

        public void Dispose()
        {
            if (this._stream != null) { this._stream.Dispose(); }
            if (this._writer != null) { this._writer.Dispose(); }
            if (this._reader != null) { this._reader.Dispose(); }
            if (this._canceler != null)
            {
                this._canceler.Dispose();
                this._canceler = null;
            }
        }
        #endregion

        #region PrivateMethods

        /// <summary>
        /// Listens for messages from the server
        /// </summary>
        private void Listen()
        {
            try
            {
                while ((this._inputLine = this._reader.ReadLine()) != null)
                {
                    try
                    {
                        if (this._consoleOutput)
                        {
                            stConsole.WriteLine(this._inputLine);
                        }
                        this.ParseData(this._inputLine);
                        this._canceler.Token.ThrowIfCancellationRequested();
                    }
                    catch (Exception e)
                    {
#if DEBUG_Listen
                    stConsole.WriteHeader("* IRC Listen Exception: " + e.Message);
                    stConsole.WriteHeader("* IRC Listen Exception: " + e.ToString());
#endif
                        Fire_ExceptionThrown(e);
                        break;
                    }
                }
            }
            catch (IOException)
            {
                return;
            }
            catch (Exception e)
            {
                if (this.iLog != null)
                {
                    this.iLog.LogInfo(
                        string.Format(
                            IrcClient.clasName,
                            "Exception: " + e.Message
                        )
                    );
                }
                return;
            }
        }
        /// <summary>
        /// Parses data sent from the server
        /// </summary>
        /// <param name="data">message from the server</param>
        private void ParseData(string data)
        {
            // split the data into parts
            string[] ircData = data.Split(' ');

            var ircCommand = ircData[1];

            // if the message starts with PING we must PONG back
            if (data.Length > 4)
            {
                if (data.Substring(0, 4) == "PING")
                {
                    Send("PONG " + ircData[1]);
                    return;
                }
            }

            // re-act according to the IRC Commands
            switch (ircCommand)
            {
                case "001": // server welcome message, after this we can join
                    {
                        Send("MODE " + _nick + " +B");
                        Fire_Connected();    //TODO: this might not work
                        break;
                    }
                case "353": // member list
                    {
                        if (ircData.Length >= 5)
                        {
                            var channel = ircData[4];
                            var userList = JoinArray(ircData, 5).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            Fire_UpdateUsers(new UpdateUsersEventArgs(channel, userList));
                        }
                        break;
                    }
                case "433":
                    {
                        if (ircData.Length >= 4)
                        {
                            var takenNick = ircData[3];

                            // notify user
                            Fire_NickTaken(takenNick);

                            // try alt nick if it's the first time 
                            if (takenNick == _altNick)
                            {
                                var rand = new Random();
                                var randomNick = "Guest" + rand.Next(0, 9) + rand.Next(0, 9) + rand.Next(0, 9);
                                Send("NICK " + randomNick);
                                Send("USER " + randomNick + " 0 * :" + randomNick);
                                _nick = randomNick;
                            }
                            else
                            {
                                Send("NICK " + _altNick);
                                Send("USER " + _altNick + " 0 * :" + _altNick);
                                _nick = _altNick;
                            }
                        }
                        break;
                    }
                case "JOIN": // someone joined
                    {
                        if (ircData.Length >= 3)
                        {
                            var channel = ircData[2];
                            var user = ircData[0].Substring(1, ircData[0].IndexOf("!", StringComparison.Ordinal) - 1);
                            Fire_UserJoined(new UserJoinedEventArgs(channel, user));
                        }
                        break;
                    }
                case "MODE": // MODE was set
                    {
                        /*
                        Console.WriteLine("0: " + ircData[0]);
                        Console.WriteLine("1: " + ircData[1]);
                        Console.WriteLine("2: " + ircData[2]);
                        Console.WriteLine("3: " + ircData[3]);
                        Console.WriteLine("4: " + ircData[3]);
                         */
                        if (ircData.Length >= 5)
                        {
                            string from, channel = ircData[2];
                            if (
                                (!string.IsNullOrWhiteSpace(channel)) && 
                                (!channel.Equals(this.Nick))
                               )
                            {
                                if (ircData[0].Contains("!"))
                                {
                                    from = ircData[0].Substring(1, ircData[0].IndexOf("!", StringComparison.Ordinal) - 1);
                                }
                                else
                                {
                                    from = ircData[0].Substring(1);
                                }
                                Fire_ChannelModeSet(new ModeSetEventArgs(channel, from, ircData[4], ircData[3]));
                            }
                        }

                        // TODO: event for userMode's
                        break;
                    }
                case "KICK": // some kicked command
                    {
                        if (ircData.Length >= 3)
                        {
                            var channel = ircData[2];
                            var user = ircData[0].Substring(1, ircData[0].IndexOf("!", StringComparison.Ordinal) - 1);
                            string message = ((ircData.Length >= 6) ? JoinArray(ircData, 5) : String.Empty);

                            Fire_UserKick(
                                new UserKickEventArgs(
                                    channel,
                                    user,
                                    message
                                )
                            );
                            if ((!this._kickrespawn) || (!this._nick.Equals(ircData[3])))
                            {
                                break;
                            }
                            if (this.iLog != null)
                            {
                                this.iLog.LogInfo(
                                    string.Format(
                                        IrcClient.clasName,
                                        "KICKED: " + channel + " from user '" + user + "'" +
                                        ((string.IsNullOrWhiteSpace(message)) ? "" : " -> '" + JoinArray(ircData, 5) + "'")
                                    )
                                );
                            }
                            stTimerWait tw = new stTimerWait();
                            lock (tw)
                            {
                                tw.timer = new Timer((cbo) =>
                                {
                                    this.JoinChannel(this._ircchannel);
                                    this.SendMessage(
                                        channel,
                                        string.Format(
                                            Properties.Resources.IRCKickMessage,
                                            user
                                        )
                                    );
                                    lock (cbo)
                                    {
                                        ((stTimerWait)cbo).Dispose();
                                    }
                                }, tw, 1000, -1);
                            }
                        }
                        break;
                    }
                case "NICK": // someone changed their nick
                    {
                        if (ircData.Length >= 3)
                        {
                            var oldNick = ircData[0].Substring(1, ircData[0].IndexOf("!", StringComparison.Ordinal) - 1);
                            var newNick = JoinArray(ircData, 3);
                            Fire_NickChanged(new UserNickChangedEventArgs(oldNick, newNick));
                        }
                        break;
                    }
                case "NOTICE": // someone sent a notice
                    {
                        if (ircData.Length >= 3)
                        {
                            var from = ircData[0];
                            var message = JoinArray(ircData, 3);
                            if (from.Contains("!"))
                            {
                                from = from.Substring(1, ircData[0].IndexOf('!') - 1);
                                Fire_NoticeMessage(new NoticeMessageEventArgs(from, message));
                            }
                            else
                            {
                                Fire_NoticeMessage(new NoticeMessageEventArgs(_server, message));
                            }
                        }
                        break;
                    }
                case "PRIVMSG": // message was sent to the channel or as private
                    {
                        if (ircData.Length >= 3)
                        {
                            var from = ircData[0].Substring(1, ircData[0].IndexOf('!') - 1);
                            var to = ircData[2];
                            var message = JoinArray(ircData, 3);

                            // if it's a private message
                            if (String.Equals(to, _nick, StringComparison.CurrentCultureIgnoreCase))
                            {
                                Fire_PrivateMessage(new PrivateMessageEventArgs(from, message));
                            }
                            else
                            {
                                Fire_ChannelMessage(new ChannelMessageEventArgs(to, from, message));
                            }
                        }
                        break;
                    }
                case "PART":
                case "QUIT":// someone left
                    {
                        if (ircData.Length >= 3)
                        {
                            var channel = ircData[2];
                            var user = ircData[0].Substring(1, data.IndexOf("!") - 1);
                            Fire_UserLeft(new UserLeftEventArgs(channel, user));
                            Send("NAMES " + ircData[2]);
                        }
                        break;
                    }
                default:
                    {
                        // still using this while debugging
                        if (ircData.Length > 3)
                        {
                            Fire_ServerMesssage(JoinArray(ircData, 3));
                        }
                        break;
                    }
            }

        }
        /// <summary>
        /// Strips the message of unnecessary characters
        /// </summary>
        /// <param name="message">Message to strip</param>
        /// <returns>Stripped message</returns>
        private static string StripMessage(string message)
        {
            // remove IRC Color Codes
            foreach (Match m in new Regex((char)3 + @"(?:\d{1,2}(?:,\d{1,2})?)?").Matches(message))
            {
                message = message.Replace(m.Value, "");
            }
            // if there is nothing to strip
            if (message == "")
            {
                return "";
            }
            else if (message.Substring(0, 1) == ":" && message.Length > 2)
            {
                return message.Substring(1, message.Length - 1);
            }
            else
            {
                return message;
            }
        }
        /// <summary>
        /// Joins the array into a string after a specific index
        /// </summary>
        /// <param name="strArray">Array of strings</param>
        /// <param name="startIndex">Starting index</param>
        /// <returns>String</returns>
        private static string JoinArray(string[] strArray, int startIndex)
        {
            return StripMessage(String.Join(" ", strArray, startIndex, strArray.Length - startIndex));
        }
        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message">Message to send</param>
        private void Send(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                try
                {
                    this._writer.WriteLine(message);
                    this._writer.Flush();
                }
                catch (Exception e)
                {
                    if (this.iLog != null)
                    {
                        this.iLog.LogError(
                            string.Format(
                                IrcClient.clasName,
                                e.Message
                            )
                        );
                    }
                }
            }
        }
        #endregion
    }
}
