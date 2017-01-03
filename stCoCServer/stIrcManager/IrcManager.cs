using System;
using System.Reflection;
using System.Threading;
using System.IO;
using stCore;
using System.Text;
using System.Globalization;
using System.Linq;

namespace stIrcManager
{
    class IrcManager
    {
        private static string rootAppPath = String.Empty;

        static void Main(string[] args)
        {
            stCore.IniConfig.IniFile _iniFile = null;
            IrcManager.rootAppPath = stCore.IOBaseAssembly.BaseDataDir();
            Thread.CurrentThread.Name = stCore.IOBaseAssembly.BaseName(Assembly.GetExecutingAssembly());

            string usrAction = String.Empty,
                   usrEmail = String.Empty,
                   usrHomeUrl = String.Empty,
                   usrChannel = String.Empty,
                   usrClanId = String.Empty,
                   usrClanName = String.Empty,
                   usrName = String.Empty,
                   usrTopic = String.Empty,
                   usrBotName = String.Empty,
                   usrIPAllow = String.Empty;
            bool   boolConfig = false,
                   boolBatch = false,
                   boolHelp = false;

            args.Process(
               () => { },
               new CommandLine.Switch("web", val => usrHomeUrl = string.Join("", val), "-w"),
               new CommandLine.Switch("email", val => usrEmail = string.Join("", val), "-e"),
               new CommandLine.Switch("channel", val => usrChannel = string.Join("", val), "-c"),
               new CommandLine.Switch("clanid", val => usrClanId = string.Join("", val), "-i"),
               new CommandLine.Switch("claname", val => usrClanName = string.Join("", val), "-n"),
               new CommandLine.Switch("topic", val => usrTopic = string.Join("", val), "-t"),
               new CommandLine.Switch("usrname", val => usrName = string.Join("", val), "-u"),
               new CommandLine.Switch("botname", val => usrBotName = string.Join("", val), "-k"),
               new CommandLine.Switch("ipallow", val => usrIPAllow = string.Join("", val), "-l"),
               new CommandLine.Switch("action", val => usrAction = string.Join("", val), "-a"),
               new CommandLine.Switch("batch", val => boolBatch = true, "-b"),
               new CommandLine.Switch("config", val => boolConfig = true, "-s"),
               new CommandLine.Switch("help", val => boolHelp = true, "-h")
            );
            try
            {
                if (boolHelp)
                {
                    IrcManager.PrnHead();
                    IrcManager.PrnHelp();
                    IrcManager.PrnExample();
                    return;
                }

                string iniPath = Path.Combine(
                        IrcManager.rootAppPath,
                        "stCoCServer.ini"
                );
                if (!File.Exists(iniPath))
                {
                    throw new FileNotFoundException(
                        string.Format(
                            Properties.Resources.fmtIniNotFound,
                            iniPath
                        )
                    );
                }
                _iniFile = new stCore.IniConfig.IniFile(iniPath);
                if (_iniFile == null)
                {
                    throw new FileLoadException(
                        string.Format(
                            Properties.Resources.fmtIniErrorLoad,
                            iniPath
                        )
                    );
                }
                string lng = _iniFile.Section("SYS").Get("SYSLANGConsole");
                if (!string.IsNullOrWhiteSpace(lng))
                {
                    try
                    {
                        CultureInfo ci = null;
                        if ((ci = CultureInfo.GetCultureInfo(lng)) != null)
                        {
                            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                usrHomeUrl = ((string.IsNullOrWhiteSpace(usrHomeUrl)) ?
                    _iniFile.Section("DOK").Get("DOKUWikiRootUrl") :
                    usrHomeUrl
                );
                usrEmail = ((string.IsNullOrWhiteSpace(usrEmail)) ?
                    _iniFile.Section("SYS").Get("SYSRegEmail") :
                    usrEmail
                );
                usrChannel = ((string.IsNullOrWhiteSpace(usrChannel)) ?
                    _iniFile.Section("IRC").Get("IRCChannel") :
                    usrChannel
                );
                usrBotName = ((string.IsNullOrWhiteSpace(usrBotName)) ?
                    _iniFile.Section("IRC").Get("IRCNik") :
                    usrBotName
                );
                usrClanId = ((string.IsNullOrWhiteSpace(usrClanId)) ?
                    _iniFile.Section("CLA").Get("CLANTag") :
                    usrClanId
                );
                usrClanName = ((string.IsNullOrWhiteSpace(usrClanName)) ?
                    _iniFile.Section("CLA").Get("CLANName") :
                    usrClanName
                );

                if (!string.IsNullOrWhiteSpace(usrChannel))
                {
                    if (!usrChannel.StartsWith("#"))
                    {
                        usrChannel = "#" + usrChannel;
                    }
                }
                if (!string.IsNullOrWhiteSpace(usrClanId))
                {
                    if (!usrClanId.StartsWith("#"))
                    {
                        usrClanId = "#" + usrClanId;
                    }
                }
                if (boolConfig)
                {
                    IrcManager.PrnHead();
                    stConsole.WriteHeader(
                        "\tHome Web\t- " + usrHomeUrl + Environment.NewLine +
                        "\tREG Email\t- " + usrEmail + Environment.NewLine +
                        "\tIRC Channel\t- " + usrChannel + Environment.NewLine +
                        "\tClan ID\t- " + usrClanId + Environment.NewLine +
                        "\tClan Name\t- " + usrClanName + Environment.NewLine +
                        "\tBot Name\t- " + usrBotName + Environment.NewLine
                    );
                    return;
                }

                string ircOut = String.Empty;

                switch (usrAction)
                {
                    case "admin":
                    case "ban":
                    case "kick":
                        {
                            if (string.IsNullOrWhiteSpace(usrName))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotNickName
                                );
                            }
                            if (string.IsNullOrWhiteSpace(usrChannel))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotIrcChannel
                                );
                            }
                            break;
                        }
                    case "topic":
                        {
                            if (string.IsNullOrWhiteSpace(usrChannel))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotIrcChannel
                                );
                            }
                            if (string.IsNullOrWhiteSpace(usrTopic))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotIrcTopic
                                );
                            }
                            break;
                        }
                    case "channel":
                        {
                            if (string.IsNullOrWhiteSpace(usrHomeUrl))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotHomeUrl
                                );
                            }
                            if (string.IsNullOrWhiteSpace(usrChannel))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotIrcChannel
                                );
                            }
                            if (string.IsNullOrWhiteSpace(usrClanId))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotClanId
                                );
                            }
                            if (string.IsNullOrWhiteSpace(usrClanName))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotClanName
                                );
                            }
                            break;
                        }
                    case "regchan":
                        {
                            if (string.IsNullOrWhiteSpace(usrChannel))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotIrcChannel
                                );
                            }
                            if (string.IsNullOrWhiteSpace(usrClanName))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotClanName
                                );
                            }
                            if (string.IsNullOrWhiteSpace(usrBotName))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotBotName
                                );
                            }
                            break;
                        }
                    case "regbot":
                        {
                            if (string.IsNullOrWhiteSpace(usrEmail))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotRegEmail
                                );
                            }
                            if (!IrcManager.IsValidEmail(usrEmail))
                            {
                                throw new FileLoadException(
                                    Properties.Resources.txtOptionNotValidEmail
                                );
                            }
                            break;
                        }
                    default:
                        {
                            throw new FileLoadException(
                                string.Format(
                                    Properties.Resources.fmtOptionNotAction,
                                    usrAction
                                )
                            );
                        }
                }
                switch (usrAction)
                {
                    case "ban":
                    case "admin":
                    case "regchan":
                        {
                            if (string.IsNullOrWhiteSpace(usrIPAllow))
                            {
                                usrIPAllow = "*";
                            }
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
                switch (usrAction)
                {
                    case "admin":
                        {
                            ircOut = string.Format(
                                Properties.Resources.fmtIRCAdmin,
                                usrChannel,
                                usrName,
                                usrIPAllow
                            );
                            break;
                        }
                    case "ban":
                        {
                            ircOut = string.Format(
                                Properties.Resources.fmtIRCBan,
                                usrChannel,
                                usrName,
                                usrIPAllow
                            );
                            break;
                        }
                    case "kick":
                        {
                            ircOut = string.Format(
                                Properties.Resources.fmtIRCKick,
                                usrChannel,
                                usrName
                            );
                            break;
                        }
                    case "topic":
                        {
                            ircOut = string.Format(
                                Properties.Resources.fmtIRCTopic,
                                usrChannel,
                                usrTopic
                            );
                            break;
                        }
                    case "channel":
                        {
                            ircOut = string.Format(
                                Properties.Resources.fmtIRCRegChannel,
                                usrChannel,
                                usrClanName
                            );
                            ircOut += Environment.NewLine + string.Format(
                                Properties.Resources.fmtIRCSet4Channel,
                                usrChannel,
                                "DESC",
                                "Clash of Clans",
                                usrClanName + " (" + usrClanId + ")"
                            );
                            ircOut += Environment.NewLine + string.Format(
                                Properties.Resources.fmtIRCSet3Channel,
                                usrChannel,
                                "URL",
                                usrHomeUrl
                            );
                            ircOut += Environment.NewLine + string.Format(
                                Properties.Resources.fmtIRCSet3Channel,
                                usrChannel,
                                "ENTRYMSG",
                                "\"Clan " + usrClanName + ": " + usrHomeUrl + "\""
                            );
                            break;
                        }
                    case "regchan":
                        {
                            ircOut = string.Format(
                                Properties.Resources.fmtIRCRegChannel,
                                usrChannel,
                                usrClanName
                            );
                            ircOut += Environment.NewLine + string.Format(
                                Properties.Resources.fmtIRCAdmin,
                                usrChannel,
                                usrBotName,
                                usrIPAllow
                            );
                            break;
                        }
                    case "regbot":
                        {
                            string usrPassword1 = String.Empty, usrPassword2 = String.Empty;

                            if (boolBatch)
                            {
                                usrPassword1 = IrcManager.GetRandomPassword();
                            }
                            else
                            {
                                stConsole.Write(
                                    string.Format(
                                        Properties.Resources.fmtIRCPassword, 1)
                                );
                                usrPassword1 = Console.ReadLine();
                                if (string.IsNullOrWhiteSpace(usrPassword1))
                                {
                                    throw new FileLoadException(
                                        Properties.Resources.txtOptionPasswordEmpty
                                    );
                                }
                                stConsole.Write(
                                    string.Format(
                                        Properties.Resources.fmtIRCPassword, 2)
                                );
                                usrPassword2 = Console.ReadLine();

                                if (!usrPassword1.Equals(usrPassword2))
                                {
                                    throw new FileLoadException(
                                        Properties.Resources.txtPasswordNotEquals
                                    );
                                }
                            }

                            ircOut = string.Format(
                                Properties.Resources.fmtIRCRegBot,
                                usrPassword1,
                                usrEmail
                            );
                            stConsole.WriteHeader(
                                string.Format(
                                    Properties.Resources.fmtPasswordAddOrChange,
                                    usrPassword1,
                                    Environment.NewLine
                                )
                            );
                            break;
                        }
                    default:
                        {
                            throw new FileLoadException(
                                string.Format(
                                    Properties.Resources.fmtOptionNotAction,
                                    usrAction
                                )
                            );
                        }
                }
#if DEBUG
                stConsole.WriteHeader(ircOut);
#endif
                File.WriteAllText(
                    Path.Combine(
                        IrcManager.rootAppPath,
                        "command.irc"
                    ),
                    ircOut,
                    Encoding.UTF8
                );
            }
            catch (Exception e)
            {
                IrcManager.PrnHead();
                IrcManager.PrnHelp();
                IrcManager.PrnError(e.Message);
            }
#if DEBUG
            Console.ReadLine();
#endif
        }

        #region Check Valid Email

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return ((addr.Address == email) ? true : false);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Generate password

        private static string GetRandomPassword()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 14)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }


        #endregion

        #region HELP PRINT

        public static void PrnHead()
        {
            string AppDesc = IOBaseAssembly.BaseDescription(Assembly.GetExecutingAssembly());
            stApp.AppInformation.PrnBanner(
                new string[] {
                        string.Format(
                            "{0}: {1}",
                            Thread.CurrentThread.Name,
                            ((string.IsNullOrWhiteSpace(AppDesc)) ? "" : AppDesc)
                        ),
                        Properties.Resources.banRun
                    }
            );
        }
        public static void PrnHelp()
        {
            stConsole.WriteHeader(
                "\t-a\taction\t: " + Environment.NewLine +
                    "\t\t\tregbot\t- " + Properties.Resources.txtHelpActionRegbot + Environment.NewLine +
                    "\t\t\tregchan\t- " + Properties.Resources.txtHelpActionRegchan + Environment.NewLine +
                    "\t\t\tchannel\t- " + Properties.Resources.txtHelpActionChannel + Environment.NewLine +
                    "\t\t\ttopic\t- " + Properties.Resources.txtHelpActionTopic + Environment.NewLine +
                    "\t\t\tkick\t- " + Properties.Resources.txtHelpActionKick + Environment.NewLine +
                    "\t\t\tban\t- " + Properties.Resources.txtHelpActionBan + Environment.NewLine +
                    "\t\t\tadmin\t- " + Properties.Resources.txtHelpActionAdmin + Environment.NewLine +
                "\t-b\tbatch\t- " + Properties.Resources.txtHelpBatch + Environment.NewLine +
                "\t-s\tconfig\t- " + Properties.Resources.txtHelpShowConfig + Environment.NewLine +
                "\t-u\tusrname\t- " + Properties.Resources.txtHelpUsrname + Environment.NewLine +
                "\t-k\tbotname\t- " + Properties.Resources.txtHelpBotname + Environment.NewLine +
                "\t-l\tipallow\t- " + Properties.Resources.txtHelpIpAllow + Environment.NewLine +
                "\t-i\tclanid\t- " + Properties.Resources.txtHelpClanid + Environment.NewLine +
                "\t-n\tclaname\t- " + Properties.Resources.txtHelpClaname + Environment.NewLine +
                "\t-w\tweb\t- " + Properties.Resources.txtHelpWeb + Environment.NewLine +
                "\t-e\temail\t- " + Properties.Resources.txtHelpEmail + Environment.NewLine +
                "\t-c\tchannel\t- " + Properties.Resources.txtHelpChannel + Environment.NewLine +
                "\t-t\ttopic\t- " + Properties.Resources.txtHelpTopic + Environment.NewLine
            );
        }
        public static void PrnExample()
        {
            stConsole.WriteHeader(
                "\tExample:" + Environment.NewLine +
                "   stIrcManager.exe action=regchan claname=\"ClanName\" ipallow=\"1.2.3.4\"" + Environment.NewLine +
                "   stIrcManager.exe action=admin ipallow=\"1.2.3.4\" usrname=\"userName1\"" + Environment.NewLine +
                "   stIrcManager.exe action=topic topic=\"this is long description..\"" + Environment.NewLine +
                "   stIrcManager.exe action=ban usrname=\"userName1\" ipallow=\"1.2.3.4\"" + Environment.NewLine +
                "   stIrcManager.exe action=kick usrname=\"userName1\"" + Environment.NewLine
            );
        }

        #endregion

        #region CONSOLE PRINT

        public static void PrnInfo(string msg)
        {
            stConsole.MessageInfo(Properties.Resources.PrnOk, msg, true);
        }
        public static void PrnError(string msg)
        {
            stConsole.MessageError(Properties.Resources.PrnError, msg, true);
        }

        #endregion

    }
}
