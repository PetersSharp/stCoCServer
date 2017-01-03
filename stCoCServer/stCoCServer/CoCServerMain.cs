#if DEBUG
// #define DEBUG_StackTrace
// #define DEBUG_ExtendedError
// #define DEBUG_HTTPResorceLocation
#endif

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using stCore;
using stNet;
using stGeo;
using stCoCServer.plugins;
using stCoCServer.CoCAPI;
using stCoCServerConfig.CoCServerConfiguration;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Resources;
using System.Diagnostics;

namespace stCoCServer
{
    partial class CoCServerMain
    {
        private static object _historyLogLock = new Object();
        private static string _historyLogString = String.Empty;

        private static CancellationTokenSource cwtoken = null;
        private static stCoCServerConfig.CoCServerConfigData.Configuration _Conf = null;
        public static stCoCServerConfig.CoCServerConfigData.Configuration Conf
        {
            get
            {
                if (CoCServerMain._Conf == null)
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.fmtConfErrorException,
                            "CoCServerMain->Config"
                        )
                    );
                }
                return CoCServerMain._Conf;
            }
        }
        private static stCoCServerConfig.CoCServerConfigData.Option _Opt = null;
        public static stCoCServerConfig.CoCServerConfigData.Option Opt
        {
            get
            {
                if (CoCServerMain._Opt == null)
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.fmtConfErrorException,
                            "CoCServerMain->Options"
                        )
                    );
                }
                return CoCServerMain._Opt;
            }
        }

        private static bool CheckOpt
        {
            get
            {
                return ((CoCServerMain._Opt == null) ? false : true);
            }
        }
        private static bool CheckConf
        {
            get
            {
                return ((CoCServerMain._Conf == null) ? false : true);
            }
        }
        private static bool CheckSyslog
        {
            get
            {
                return ((!CoCServerMain.CheckConf) ? false : ((CoCServerMain._Conf.SysLog == null) ? false : true));
            }
        }

        public static void Main(string[] args)
        {
            CoCServerMain.MainStart(args);
        }

        public static void MainStart(string[] args)
        {
            const string className = "[Main]: ";
            IMessage iLog = new IMessage() {
                LogInfo = CoCServerMain.PrnInfo,
                LogError = CoCServerMain.PrnError,
                LogNetSyslog = CoCServerMain.PrnNetLog,
                ProgressBar = stConsole.ProgressTxt
            };
            Func<uint, List<int>, int, bool, bool> geoCheckFun = null;
            Func<string, int> geoCountryFun = null;
            
            ///
            Thread.CurrentThread.Name = stCore.IOBaseAssembly.BaseName(Assembly.GetExecutingAssembly());
            CoCServerMain.PrnInfo(
                string.Format(
                    Properties.Resources.PrnRun,
                    stApp.AppInformation.GetAppVersion(),
                    DateTime.Now
                )
            );
            ///

            try
            {
                if ((CoCServerMain._Opt = stCoCServerConfig.CoCServerConfiguration.BuildConfig.InitOption(args, Assembly.GetExecutingAssembly(), iLog)) == null)
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.fmtConfErrorException,
                            "CoCServerMain.Opt"
                        )
                    );
                }
                if (string.IsNullOrWhiteSpace(CoCServerMain.Opt.CLANTag.value))
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.fmtConfMissException,
                            "CLAN Tag"
                        )
                    );
                }
                if (string.IsNullOrWhiteSpace(CoCServerMain.Opt.CLANAPIKey.value))
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.fmtConfMissException,
                            "CLAN API Key"
                        )
                    );
                }
                if ((CoCServerMain._Conf = stCoCServerConfig.CoCServerConfiguration.BuildConfig.InitConfiguration(CoCServerMain.Opt, iLog)) == null)
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.fmtConfErrorException,
                            "CoCServerMain.Conf"
                        )
                    );
                }
                if (!string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.SYSLANGConsole.value))
                {
                    try
                    {
                        CultureInfo ci = null;
                        if ((ci = CultureInfo.GetCultureInfo(CoCServerMain.Conf.Opt.SYSLANGConsole.value)) != null)
                        {
                            System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                            System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                        }
                    }
                    catch (Exception e)
                    {
                        CoCServerMain.PrnError(
                            string.Format(
                                Properties.Resources.prnLangCultureError,
                                CoCServerMain.Conf.Opt.SYSLANGConsole.value,
                                e.Message
                            )
                        );
                    }
                }
                CoCServerMain.PrnInfo(
                    string.Format(
                        Properties.Resources.prnLangCulture,
                        CultureInfo.CurrentCulture.DisplayName
                    )
                );

                if (CoCServerMain.Conf.Opt.LOGRemoteServerEnable.bval)
                {
                    try
                    {
                        CoCServerMain.Conf.SysLog = new stNet.Syslog.stSysLogNG(
                            new stNet.Syslog.SysLogSenderUdp(
                                new IPEndPoint(
                                    IPAddress.Parse(CoCServerMain.Conf.Opt.LOGRemoteServerAddress.value),
                                    CoCServerMain.Conf.Opt.LOGRemoteServerPort.num
                                )
                            )
                        );
                        CoCServerMain.Conf.SysLog.isUTF8Bom = true;
                        CoCServerMain.PrnInfo(Properties.Resources.serviceSysLogStarted);
                    }
                    catch (Exception e)
                    {
                        CoCServerMain.Conf.Opt.LOGRemoteServerEnable.bval = false;
                        CoCServerMain.PrnError(
                            string.Format(
                                Properties.Resources.serviceSysLogStartFailed,
                                e.Message
                            )
                        );
                    }
                }
                else
                {
                    CoCServerMain.PrnInfo(Properties.Resources.SysLogDisabled);
                }

                /// IP Filter

                bool isFilterEnable = BuildConfig.isFilterAll(ref CoCServerMain.Conf.Opt);
                if (isFilterEnable)
                {
                    try
                    {
                        CoCServerMain.Conf.Geo = new GeoFilter(iLog, true);
                        CoCServerMain.Conf.Geo.InitBase(
                            CoCServerMain.Conf.Opt.SYSGEOPath.value,
                            false,
                            stConsole.GetCursorAlign(2)
                        );
                        geoCheckFun = CoCServerMain.Conf.Geo.InGeoRange;
                        geoCountryFun = stGeo.MaxMindUtil.GetCountryId;
                        CoCServerMain.PrnInfo(Properties.Resources.serviceGeoStarted);
                    }
                    catch (Exception e)
                    {
                        if (CoCServerMain.Conf.Geo != null)
                        {
                            CoCServerMain.Conf.Geo.Dispose();
                        }
                        CoCServerMain.Conf.Geo = null;
                        CoCServerMain.PrnError(
                            string.Format(
                                Properties.Resources.fmtMainError,
                                className,
                                e.GetType().Name,
                                e.Message
                            )
                        );

                    }
                }
                else
                {
                    CoCServerMain.PrnInfo(Properties.Resources.GeoFilterDisabled);
                }

                /// CoC API

                CoCServerMain.Conf.Api = new stCoCAPI.CoCAPI(
                    CoCServerMain.Conf.Opt.SQLDBPath.value,
                    CoCServerMain.Conf.Opt.SQLDBUri.value,
                    CoCServerMain.Conf.Opt.CLANAPIKey.value,
                    CoCServerMain.Conf.Opt.CLANTag.value,
                    CoCServerMain.Conf.Opt.SYSROOTPath.value,
                    iLog
                );
                CoCServerMain.Conf.Api.FilterMemberTag = CoCServerMain.Conf.Opt.SQLDBFilterMemberTag.collection;
                CoCServerMain.Conf.Api.DefaultLang = CoCServerMain.Conf.Opt.WEBLANGDefault.value;
                CoCServerMain.Conf.Api.InformerStaticEnable = CoCServerMain.Conf.Opt.CLANInformerStaticEnable.bval;
                CoCServerMain.Conf.Api.AssetsPath = CoCServerMain.Conf.Opt.IPFLocation[0].value;

                /// Integrate DokuWiki API Auth method

                if (CoCServerMain.Conf.Opt.DOKUWikiAuthEnable.bval)
                {
                    CoCServerMain.Conf.Api.DokuWikiAuthInit(
                        Opt.DOKUWikiRootPath.value,
                        Opt.DOKUWikiDefaultGroup.value
                    );
                    CoCServerMain.PrnInfo(Properties.Resources.serviceWikiAuthStarted);
                }

                /// CoC Api start
                 
                CoCServerMain.Conf.Api.Start(CoCServerMain.Conf.Opt.SQLDBUpdateTime.num);
                CoCServerMain.PrnInfo(Properties.Resources.serviceCoCApiStarted);

                /// Json Client setup

                stCoCServer.CoCAPI.CoCClientSetup.SaveJsonSetup(CoCServerMain.Conf);

                /// Web server

                if (
                    (!string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.WEBRootUri.value)) &&
                    ((int)CoCServerMain.Conf.Opt.WEBRootPort.num > 0)
                   )
                {
                    /// Web server Template
                    CoCServerMain.Conf.HtmlTemplate = new stNet.stWebServerUtil.HtmlTemplate(
                        Path.Combine(
                            CoCServerMain.Conf.Opt.SYSROOTPath.value,
                            CoCServerMain.Conf.Opt.SYSTMPLPath.value
                        )
                    );
                    CoCServerMain.Conf.HtmlTemplate.InsertFileNotFound = Properties.Resources.httpLogNotFound;
                    CoCServerMain.PrnInfo(Properties.Resources.serviceTemplateStarted);

                    /// Web server Wiki Engine
                    if (
                        (CoCServerMain.Conf.Opt.IPFLocationEnable.Count > 6) &&
                        (CoCServerMain.Conf.Opt.IPFLocationEnable[6].bval)
                       )
                    {
                        try
                        {
                            CoCServerMain.Conf.WikiEngine = new stDokuWiki.WikiEngine.WikiFile(
                                ((string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.DOKUWikiRootPath.value)) ?
                                    Path.Combine(
                                        CoCServerMain.Conf.Opt.SYSROOTPath.value,
                                        stDokuWiki.WikiEngine.WikiFile.wikiLocalPath
                                    ) :
                                    CoCServerMain.Conf.Opt.DOKUWikiRootPath.value
                                )
                            );
                            CoCServerMain.Conf.WikiEngine.OnProcessError += (o, e) =>
                            {
                                CoCServerMain.PrnError(
                                    string.Format(
                                        Properties.Resources.fmtMainError,
                                        className,
                                        e.ex.GetType().Name,
                                        e.ex.Message
                                    )
                                );
                            };
                            // external DokuWiki auth disaled
                            // CoCServerMain.Conf.Opt.DOKUWikiAuthEnable.bval = false;
                            // TODO: normalize this
                            CoCServerMain.PrnInfo(Properties.Resources.serviceWikiStarted);
                        }
                        catch (Exception e)
                        {
                            CoCServerMain.PrnError(
                                string.Format(
                                    Properties.Resources.fmtMainError,
                                    className,
                                    e.GetType().Name,
                                    e.Message
                                )
                            );
                            CoCServerMain.Conf.WikiEngine = null;
                            CoCServerMain.Conf.Opt.IPFLocationEnable[6].bval = false;
                        }
                    }
                    else
                    {
                        CoCServerMain.Conf.WikiEngine = null;
                    }

                    /// Web server Engine
                    CoCServerMain.Conf.HttpSrv = new stWebServer(
                        CoCServerMain.Conf.Opt.WEBRootUri.value,
                        CoCServerMain.Conf.Opt.WEBRootPort.num,
                        iLog
                    );
                    CoCServerMain.Conf.HttpSrv.wUserData = Conf;
                    CoCServerMain.Conf.HttpSrv.isConcat = true;
                    CoCServerMain.Conf.HttpSrv.isMinify = true;
                    CoCServerMain.Conf.HttpSrv.isFrontEnd = CoCServerMain.Conf.Opt.WEBFrontEndEnable.bval;
                    CoCServerMain.Conf.HttpSrv.DefaultLang = CoCServerMain.Conf.Opt.WEBLANGDefault.value;
                    CoCServerMain.Conf.HttpSrv.wBadRequestDebugOut = CoCServerMain.Conf.Opt.WEBRequestDebugEnable.bval;

                    /// Web server Locations/IPFilter
                    for (int i = 0; i < BuildConfig.numFilters; i++)
                    {
                        stIPFilter ipFilter = null;

                        if (!CoCServerMain.Conf.Opt.IPFLocationEnable[i].bval)
                        {
                            continue;
                        }
                        if (
                            (isFilterEnable) &&
                            (BuildConfig.isFilter(ref CoCServerMain.Conf.Opt, i))
                           )
                        {
                            ipFilter = stACL.IpFilterCreate(
                                CoCServerMain.Conf.Opt.IPFIpList[i].collection,
                                CoCServerMain.Conf.Opt.IPFGeoListASN[i].collection,
                                CoCServerMain.Conf.Opt.IPFGeoListCountry[i].collection,

                                CoCServerMain.Conf.Opt.IPFIsIpBlackList[i].bval,
                                CoCServerMain.Conf.Opt.IPFIsGeoAsnBlackList[i].bval,
                                CoCServerMain.Conf.Opt.IPFIsGeoCountryBlackList[i].bval,
                                geoCheckFun,
                                geoCountryFun
                            );
                        }
                        Conf.HttpSrv.AddHandler(
                            CoCWebSrvHandleSettings.HttpHandleType(CoCServerMain.Conf.Opt.IPFType[i].value),
                            CoCWebSrvHandleSettings.HttpHandleAction(CoCServerMain.Conf.Opt.IPFType[i].value),
                            CoCServerMain.Conf.Opt.IPFLocation[i].value,
                            ipFilter,
                            ((CoCWebSrvHandleSettings.HttpHandleBool(CoCServerMain.Conf.Opt.IPFType[i].value)) ? CoCServerMain.Conf.Opt.SYSROOTPath.value : null)
                        );
                    }
#if DEBUG_HTTPResorceLocation
                    Conf.HttpSrv.PrintResorceLocation();
#endif
                    Conf.HttpSrv.Start(ref CoCServerMain.Conf.Opt.IsRun.bval);
                    CoCServerMain.PrnInfo(Properties.Resources.serviceHttpStarted);
                }
                else
                {
                    CoCServerMain.PrnInfo(Properties.Resources.httpServerDisabled);
                }

                /// IRC bot

                if (
                    (!string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.IRCChannel.value)) &&
                    (!string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.IRCServer.value)) &&
                    ((int)CoCServerMain.Conf.Opt.IRCPort.num > 0)
                   )
                {
                    Conf.LogDump = new stCore.IOFile(
                        CoCServerMain.Conf.Opt.IRCSOutDirName.value,
                        CoCServerMain.Conf.Opt.IRCSOutFileName.value,
                        CoCServerMain.Conf.Opt.SYSIRCLOGPath.value,
                        CoCServerMain.PrnInfo
                    );
                    CoCServerMain.PrnInfo(Properties.Resources.serviceIrcLogStarted);

                    Conf.Irc = new IrcClient(CoCServerMain.Conf.Opt.IRCServer.value, (int)CoCServerMain.Conf.Opt.IRCPort.num);
                    Conf.Irc.Nick = Conf.Opt.IRCNik.value;
                    Conf.Irc.ConsoleOutput = false;
                    Conf.Irc.KickRespawn = Conf.Opt.IRCKickRespawn.bval;
                    Conf.Irc.ServerPass = ((!string.IsNullOrWhiteSpace(CoCServerMain.Conf.Opt.IRCPassword.value)) ? CoCServerMain.Conf.Opt.IRCPassword.value : "");
                    Conf.Irc.iLog = iLog;
                    Conf.Irc.Connect();
                    Conf.IrcCmd = new IrcCommand(CoCServerMain.Conf);
                    CoCServerMain.InitIrcCallBack();
                    CoCServerMain.PrnInfo(Properties.Resources.serviceIrcBotStarted);
                    Conf.IrcCmd.PluginsPrint();
                }
                else
                {
                    CoCServerMain.PrnInfo(Properties.Resources.ircBootDisabled);
                }

                #region Cancel Key Press / SYSTEM

                CoCServerMain.cwtoken = new CancellationTokenSource();
                Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
                {
                    e.Cancel = true;
                    CoCServerMain.Conf.Opt.IsRun.bval = false;
                    if ((CoCServerMain.cwtoken != null) && (!CoCServerMain.cwtoken.IsCancellationRequested))
                    {
                        CoCServerMain.cwtoken.Cancel();
                    }
                };
                
                #endregion

            }
            catch (Exception e)
            {
                CoCServerMain.PrnError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        className,
                        e.GetType().Name,
                        e.Message
                    )
                );

#if DEBUG_ExtendedError
                stConsole.WriteHeader(e.ToString());
#endif
                if (CoCServerMain.CheckConf)
                {
                    CoCServerMain.Conf.Dispose();
                }
#if DEBUG
                Console.ReadLine();
#endif
                return;
            }
            CoCServerMain.PrnInfo(
                string.Format(
                    Properties.Resources.PrnJob,
                    CoCServerMain.Conf.Opt.SYSAppName.value,
                    DateTime.Now
                )
            );
            while (CoCServerMain.Conf.Opt.IsRun.bval)
            {
                try
                {
                    CoCServerMain.cwtoken.Token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                try
                {
                    CoCServerMain.cwtoken.Token.WaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
            if (CoCServerMain.cwtoken != null)
            {
                CoCServerMain.cwtoken.Dispose();
            }
            CoCServerMain.Conf.Dispose();
            CoCServerMain.PrnInfo(Properties.Resources.PrnExit);
        }

        #region CONSOLE MSG / SYSLOG MSG

        private static bool PrnLogFilter(string msg)
        {
            if ((CoCServerMain.CheckOpt) && (!CoCServerMain.Opt.LOGDuplicateEntry.bval))
            {
                lock (_historyLogLock)
                {
                    if (string.IsNullOrWhiteSpace(CoCServerMain._historyLogString))
                    {
                        return true;
                    }
                    if (msg.Equals(CoCServerMain._historyLogString))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private static void LogToSyslog(string msg, stNet.Syslog.Level lvl, stNet.Syslog.Facility fc)
        {
            if (CoCServerMain.CheckSyslog)
            {
                try
                {
                    Task t1 = Task.Factory.StartNew(() =>
                    {
                        CoCServerMain.Conf.SysLog.SendMessage(lvl, fc, CoCServerMain.Opt.SYSAppName.value, msg);
                    });
                }
                catch (Exception e)
                {
                    stConsole.MessageError(
                        Properties.Resources.PrnError,
                        string.Format(
                            Properties.Resources.serviceSysLogSendFailed,
                            e.Message
                        ),
                        ((!CoCServerMain.CheckOpt) ? true :
                            ((!CoCServerMain.Opt.PrnQuiet.bval) ? true : false))
                    );
                }
            }
        }
        public static void PrnInfo(string msg)
        {
            if ((CoCServerMain.CheckOpt) && (!CoCServerMain.PrnLogFilter(msg)))
            {
                return;
            }
            stConsole.MessageInfo(
                Properties.Resources.PrnOK, msg,
                ((!CoCServerMain.CheckOpt) ? true : 
                    ((!CoCServerMain.Opt.PrnQuiet.bval) ? true : false))
            );
            if ((CoCServerMain.CheckOpt) && (CoCServerMain.Opt.LOGRemoteServerEnable.bval))
            {
                CoCServerMain.LogToSyslog(msg, stNet.Syslog.Level.Information, stNet.Syslog.Facility.Local5);
            }
        }
        public static void PrnNetLog(string msg)
        {
            if ((CoCServerMain.CheckOpt) && (CoCServerMain.Opt.LOGRemoteServerEnable.bval))
            {
                CoCServerMain.LogToSyslog(msg, stNet.Syslog.Level.Debug, stNet.Syslog.Facility.Local5);
            }
        }
        public static void PrnError(string msg)
        {
            if ((CoCServerMain.CheckOpt) && (!CoCServerMain.PrnLogFilter(msg)))
            {
                return;
            }
            if (CoCServerMain.Opt.LOGDebug.bval)
            {
                if (stRuntime.isRunTime())
                {
                    stConsole.WriteHeader(Environment.StackTrace.ToString());
                }
                else
                {
#if DEBUG_StackTrace
                    stConsole.WriteHeader(Environment.StackTrace.ToString());
#endif
                    StackFrame CallStack = null;
                    for (int i = 1; i < 10; i++)
                    {
                        CallStack = new StackFrame(i, true);
                        if ((CallStack != null) && (!string.IsNullOrWhiteSpace(CallStack.GetFileName())))
                        {
                            msg += string.Format(
                                "{0}{1}[{2}:{3}]",
                                Environment.NewLine,
                                stConsole.GetTabString(2, i),
                                Path.GetFileName(CallStack.GetFileName()),
                                CallStack.GetFileLineNumber()
                            );
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            stConsole.MessageError(
                Properties.Resources.PrnError, msg,
                ((!CoCServerMain.CheckOpt) ? true :
                    ((!CoCServerMain.Opt.PrnQuiet.bval) ? true : false))
            );
            if ((CoCServerMain.CheckOpt) && (CoCServerMain.Opt.LOGRemoteServerEnable.bval))
            {
                CoCServerMain.LogToSyslog(msg, stNet.Syslog.Level.Error, stNet.Syslog.Facility.Local5);
            }
        }

        #endregion

    }
}
