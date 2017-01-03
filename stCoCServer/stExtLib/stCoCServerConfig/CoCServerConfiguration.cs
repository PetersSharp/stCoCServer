using System;
using System.Collections.Generic;
using System.Configuration;
using stCore;
using stCoCServerConfig.CoCServerConfigData;
using System.IO;

namespace stCoCServerConfig.CoCServerConfiguration
{
    public static class BuildConfig
    {
        public static int numFilters = 7;
        private const string tagFilters = @"IPF";
        private const string nameFilters = @"IpFilter";
        private const string ircNikDefault = @"clanNik";

        private static bool isFilterItem(CoCServerConfigData.OptionItem OIpt)
        {
            return (
                ((OIpt.collection != null) && (OIpt.collection.Count > 0)) ?
                true : false
            );
        }
        public static bool isFilter(ref CoCServerConfigData.Option Opt, int idx)
        {
            return (
                ((BuildConfig.isFilterItem(Opt.IPFIpList[idx])) ||
                 (BuildConfig.isFilterItem(Opt.IPFGeoListCountry[idx])) ||
                 (BuildConfig.isFilterItem(Opt.IPFGeoListASN[idx]))) ? true : false
            );
        }
        public static bool isFilterAll(ref CoCServerConfigData.Option Opt)
        {
            for (int i = 0; i < BuildConfig.numFilters; i++)
            {
                if (BuildConfig.isFilter(ref Opt, i))
                {
                    return true;
                }
            }
            return false;
        }
        public static CoCServerConfigData.Configuration InitConfiguration(CoCServerConfigData.Option Opt, IMessage iLog)
        {
            CoCServerConfigData.Configuration Conf = new CoCServerConfigData.Configuration()
            {
                StatTime = DateTime.Now,
                ILog = iLog,
                SysLog = null,
                LogDump = null,
                Irc = null,
                Api = null,
                IrcCmd = null,
                Geo = null,
                Opt = Opt
            };
            return Conf;
        }
        public static CoCServerConfigData.Option InitOption(string[] args, System.Reflection.Assembly asm, IMessage iLog, bool overwrite = true)
        {
            if (iLog == null)
            {
                throw new ArgumentNullException("iLog");
            }
            CoCServerConfigData.Option Opt = new CoCServerConfigData.Option()
            {
                IRCPort = new CoCServerConfigData.OptionItem(String.Empty, "port", "-p"),
                IRCServer = new CoCServerConfigData.OptionItem(String.Empty, "server", "-s"),
                IRCPassword = new CoCServerConfigData.OptionItem(String.Empty, "password", "-pwd"),
                IRCAdminPassword = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                IRCChannel = new CoCServerConfigData.OptionItem(String.Empty, "channel", "-c"),
                IRCNik = new CoCServerConfigData.OptionItem(String.Empty, "nik", "-n"),
                IRCSOutDirName = new CoCServerConfigData.OptionItem(String.Empty, "dirname", "-d"),
                IRCSOutFileName = new CoCServerConfigData.OptionItem(String.Empty, "filename", "-f"),
                IRCFloodTimeOut = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                IRCLogTimeFormat = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                IRCServerMessage = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                IRCNoticeMessage = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                IRCKickRespawn = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCSetNewChannel = new CoCServerConfigData.OptionItem(false, String.Empty, String.Empty),
                IRCLanguage = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                IRCPluginSayEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginClanEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginHelpEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginModeEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginTimeEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginTopicEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginUpTimeEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginVersionEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginUrlShortEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginLangSetupEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginNotifySetupEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginContextUrlTitleEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginLoopClanNotifyEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                IRCPluginLoopClanNotifyPeriod = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                SYSROOTPath = new CoCServerConfigData.OptionItem(String.Empty, "root", "-r"),
                SYSCONFPath = new CoCServerConfigData.OptionItem(String.Empty, "config", "-i"),
                SYSGEOPath = new CoCServerConfigData.OptionItem(String.Empty, "geo", "-g"),
                SYSIRCLOGPath = new CoCServerConfigData.OptionItem(String.Empty, "irclog", "-l"),
                SYSTMPLPath = new CoCServerConfigData.OptionItem(String.Empty, "template", "-m"),
                CLANTag = new CoCServerConfigData.OptionItem(String.Empty, "tag", "-t"),
                CLANAPIKey = new CoCServerConfigData.OptionItem(String.Empty, "key", "-k"),
                CLANInformerStaticEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                SQLDBPath = new CoCServerConfigData.OptionItem(String.Empty, "dbname", "-b"),
                SQLDBUri = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                SQLDBUpdateTime = new CoCServerConfigData.OptionItem(String.Empty, "dbupdate", "-u"),
                SQLDBFilterMemberTag = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                SYSLANGConsole = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                SYSAppName = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                WEBRootUri = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                WEBRootPort = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                WEBLANGDefault = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                WEBCacheEnable = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                WEBFrontEndEnable = new CoCServerConfigData.OptionItem(false, String.Empty, String.Empty),
                WEBRequestDebugEnable = new CoCServerConfigData.OptionItem(false, String.Empty, String.Empty),
                DOKUWikiAuthEnable = new CoCServerConfigData.OptionItem(false, String.Empty, String.Empty),
                DOKUWikiRootUrl = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                DOKUWikiRootPath = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                DOKUWikiQuestLogin = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                DOKUWikiQuestPassword = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                DOKUWikiDefaultGroup = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                LOGRemoteServerEnable = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                LOGRemoteServerPort = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                LOGRemoteServerAddress = new CoCServerConfigData.OptionItem(String.Empty, String.Empty, String.Empty),
                LOGDuplicateEntry = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty),
                LOGDebug = new CoCServerConfigData.OptionItem(false, String.Empty, String.Empty),
                PrnQuiet = new CoCServerConfigData.OptionItem(false, "quiet", "-q"),
                IsRun = new CoCServerConfigData.OptionItem(true, String.Empty, String.Empty)
            };
            if ((args != null) && (args.Length > 0))
            {
                args.Process(
                () => iLog.LogInfo(Properties.Resources.cmdEmptyCmd),
                new CommandLine.Switch(Opt.IRCServer.tag1, val => Opt.IRCServer.value = string.Join("", val), Opt.IRCServer.tag2),
                new CommandLine.Switch(Opt.IRCPort.tag1, val => Opt.IRCPort.num = Convert.ToInt32(val), Opt.IRCPort.tag2),
                new CommandLine.Switch(Opt.IRCNik.tag1, val => Opt.IRCNik.value = string.Join("", val), Opt.IRCNik.tag2),
                new CommandLine.Switch(Opt.IRCPassword.tag1, val => Opt.IRCPassword.value = string.Join("", val), Opt.IRCPassword.tag2),
                new CommandLine.Switch(Opt.IRCChannel.tag1, val => Opt.IRCChannel.value = string.Join("", val), Opt.IRCChannel.tag2),
                new CommandLine.Switch(Opt.IRCSOutFileName.tag1, val => Opt.IRCSOutFileName.value = string.Join("", val), Opt.IRCSOutFileName.tag2),
                new CommandLine.Switch(Opt.IRCSOutDirName.tag1, val => Opt.IRCSOutDirName.value = string.Join("", val), Opt.IRCSOutDirName.tag2),
                new CommandLine.Switch(Opt.SYSROOTPath.tag1, val => Opt.SYSROOTPath.value = string.Join("", val), Opt.SYSROOTPath.tag2),
                new CommandLine.Switch(Opt.SYSCONFPath.tag1, val => Opt.SYSCONFPath.value = string.Join("", val), Opt.SYSCONFPath.tag2),
                new CommandLine.Switch(Opt.SYSGEOPath.tag1, val => Opt.SYSGEOPath.value = string.Join("", val), Opt.SYSGEOPath.tag2),
                new CommandLine.Switch(Opt.SYSIRCLOGPath.tag1, val => Opt.SYSIRCLOGPath.value = string.Join("", val), Opt.SYSIRCLOGPath.tag2),
                new CommandLine.Switch(Opt.SYSTMPLPath.tag1, val => Opt.SYSTMPLPath.value = string.Join("", val), Opt.SYSTMPLPath.tag2),
                new CommandLine.Switch(Opt.CLANTag.tag1, val => Opt.CLANTag.value = string.Join("", val), Opt.CLANTag.tag2),
                new CommandLine.Switch(Opt.CLANAPIKey.tag1, val => Opt.CLANAPIKey.value = string.Join("", val), Opt.CLANAPIKey.tag2),
                new CommandLine.Switch(Opt.SQLDBPath.tag1, val => Opt.SQLDBPath.value = string.Join("", val), Opt.SQLDBPath.tag2),
                new CommandLine.Switch(Opt.SQLDBUpdateTime.tag1, val => Opt.SQLDBUpdateTime.value = string.Join("", val), Opt.SQLDBUpdateTime.tag2),
                new CommandLine.Switch(Opt.PrnQuiet.tag1, val => Opt.PrnQuiet.bval = true, Opt.PrnQuiet.tag2)
                );
            }
            Opt.SYSAppName.value = stCore.IOBaseAssembly.BaseName(asm);
            Opt.SYSROOTPath.value = ((string.IsNullOrWhiteSpace(Opt.SYSROOTPath.value)) ?
                    stCore.IOBaseAssembly.BaseDataDir() : Opt.SYSROOTPath.value
            );
            Opt.SYSCONFPath.value = ((string.IsNullOrWhiteSpace(Opt.SYSCONFPath.value)) ?
                    Path.Combine(
                        Opt.SYSROOTPath.value,
                        (((args != null) && (args.Length > 0)) ?
                            Path.GetFileNameWithoutExtension(args[0]) :
                            Opt.SYSAppName.value
                        ) + ".ini"
                    ) : Opt.SYSCONFPath.value
            );
            {
                Opt.IPFLocation = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFLocationEnable = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFType = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFIsIpBlackList = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFIsGeoAsnBlackList = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFIsGeoCountryBlackList = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFIpList = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFGeoListCountry = new List<OptionItem>(BuildConfig.numFilters);
                Opt.IPFGeoListASN = new List<OptionItem>(BuildConfig.numFilters);
            }
            for (int i = 0; i < BuildConfig.numFilters; i++)
            {
                BuildConfig._InitFilter(ref Opt);
            }

            bool isIniFile = BuildConfig.GetIniConfig(ref Opt, iLog);

            BuildConfig.MergeOptionDefault(ref Opt);

            if ((overwrite) && (!isIniFile))
            {
                BuildConfig.SetIniConfig(Opt, iLog, false);
            }
            return Opt;
        }
        public static void MergeOptionDefault(ref CoCServerConfigData.Option Opt)
        {
            Opt.SYSLANGConsole.value = ((string.IsNullOrWhiteSpace(Opt.SYSLANGConsole.value)) ? Properties.Settings.Default.SYSLANGConsole : Opt.SYSLANGConsole.value);
            Opt.IRCLanguage.value = ((string.IsNullOrWhiteSpace(Opt.IRCLanguage.value)) ? Opt.SYSLANGConsole.value : Opt.IRCLanguage.value);

            Opt.IRCPort.num = ((string.IsNullOrWhiteSpace(Opt.IRCPort.value)) ? Properties.Settings.Default.IRCPort : BuildConfig._GetIntConfig(Opt.IRCPort.value));
            Opt.IRCServer.value = ((string.IsNullOrWhiteSpace(Opt.IRCServer.value)) ? Properties.Settings.Default.IRCServer : Opt.IRCServer.value);
            Opt.IRCPassword.value = ((string.IsNullOrWhiteSpace(Opt.IRCPassword.value)) ? Properties.Settings.Default.IRCPassword : Opt.IRCPassword.value);
            Opt.IRCAdminPassword.value = ((string.IsNullOrWhiteSpace(Opt.IRCAdminPassword.value)) ? Properties.Settings.Default.IRCAdminPassword : Opt.IRCAdminPassword.value);
            Opt.IRCChannel.value = BuildConfig._GetIRCChannelName(Opt.IRCChannel.value);
            Opt.IRCSOutDirName.value = ((string.IsNullOrWhiteSpace(Opt.IRCSOutDirName.value)) ? Properties.Settings.Default.IRCSOutDirName : Opt.IRCSOutDirName.value);
            Opt.IRCSOutFileName.value = ((string.IsNullOrWhiteSpace(Opt.IRCSOutFileName.value)) ? Properties.Settings.Default.IRCSOutFileName : Opt.IRCSOutFileName.value);
            Opt.IRCLogTimeFormat.value = ((string.IsNullOrWhiteSpace(Opt.IRCLogTimeFormat.value)) ? Properties.Settings.Default.IRCLogTimeFormat : Opt.IRCLogTimeFormat.value);
            Opt.IRCServerMessage.bval = ((string.IsNullOrWhiteSpace(Opt.IRCServerMessage.value)) ? Properties.Settings.Default.IRCServerMessage : BuildConfig._GetBoolConfig(Opt.IRCServerMessage.value));
            Opt.IRCNoticeMessage.bval = ((string.IsNullOrWhiteSpace(Opt.IRCNoticeMessage.value)) ? Properties.Settings.Default.IRCNoticeMessage : BuildConfig._GetBoolConfig(Opt.IRCNoticeMessage.value));
            Opt.IRCKickRespawn.bval = ((string.IsNullOrWhiteSpace(Opt.IRCKickRespawn.value)) ? Properties.Settings.Default.IRCKickRespawn : BuildConfig._GetBoolConfig(Opt.IRCKickRespawn.value));
            Opt.IRCSetNewChannel.bval = ((string.IsNullOrWhiteSpace(Opt.IRCSetNewChannel.value)) ? Properties.Settings.Default.IRCSetNewChannel : BuildConfig._GetBoolConfig(Opt.IRCSetNewChannel.value));
            Opt.IRCFloodTimeOut.num = ((string.IsNullOrWhiteSpace(Opt.IRCFloodTimeOut.value)) ? Properties.Settings.Default.IRCFloodTimeOut : BuildConfig._GetIntConfig(Opt.IRCFloodTimeOut.value));

            Opt.IRCPluginSayEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginSayEnable.value)) ? Properties.Settings.Default.IRCPluginSayEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginSayEnable.value));
            Opt.IRCPluginClanEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginClanEnable.value)) ? Properties.Settings.Default.IRCPluginClanEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginClanEnable.value));
            Opt.IRCPluginHelpEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginHelpEnable.value)) ? Properties.Settings.Default.IRCPluginHelpEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginHelpEnable.value));
            Opt.IRCPluginModeEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginModeEnable.value)) ? Properties.Settings.Default.IRCPluginModeEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginModeEnable.value));
            Opt.IRCPluginTimeEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginTimeEnable.value)) ? Properties.Settings.Default.IRCPluginTimeEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginTimeEnable.value));
            Opt.IRCPluginTopicEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginTopicEnable.value)) ? Properties.Settings.Default.IRCPluginTopicEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginTopicEnable.value));
            Opt.IRCPluginUpTimeEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginUpTimeEnable.value)) ? Properties.Settings.Default.IRCPluginUpTimeEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginUpTimeEnable.value));
            Opt.IRCPluginVersionEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginVersionEnable.value)) ? Properties.Settings.Default.IRCPluginVersionEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginVersionEnable.value));
            Opt.IRCPluginUrlShortEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginUrlShortEnable.value)) ? Properties.Settings.Default.IRCPluginUrlShortEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginUrlShortEnable.value));
            Opt.IRCPluginLangSetupEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginLangSetupEnable.value)) ? Properties.Settings.Default.IRCPluginLangSetupEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginLangSetupEnable.value));
            Opt.IRCPluginNotifySetupEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginNotifySetupEnable.value)) ? Properties.Settings.Default.IRCPluginNotifySetupEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginNotifySetupEnable.value));
            Opt.IRCPluginContextUrlTitleEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginContextUrlTitleEnable.value)) ? Properties.Settings.Default.IRCPluginContextUrlTitleEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginContextUrlTitleEnable.value));
            Opt.IRCPluginLoopClanNotifyEnable.bval = ((string.IsNullOrWhiteSpace(Opt.IRCPluginLoopClanNotifyEnable.value)) ? Properties.Settings.Default.IRCPluginLoopClanNotifyEnable : BuildConfig._GetBoolConfig(Opt.IRCPluginLoopClanNotifyEnable.value));
            Opt.IRCPluginLoopClanNotifyPeriod.num = ((string.IsNullOrWhiteSpace(Opt.IRCPluginLoopClanNotifyPeriod.value)) ? Properties.Settings.Default.IRCPluginLoopClanNotifyPeriod : BuildConfig._GetIntConfig(Opt.IRCPluginLoopClanNotifyPeriod.value));

            Opt.IRCNik.value = ((string.IsNullOrWhiteSpace(Opt.IRCNik.value)) ? 
                    ((!string.IsNullOrWhiteSpace(Properties.Settings.Default.IRCNik)) ?
                        Properties.Settings.Default.IRCNik :
                        stNet.Account.RandomNik(BuildConfig.ircNikDefault)
                    ) : Opt.IRCNik.value
            );
            Opt.SYSGEOPath.value =  ((string.IsNullOrWhiteSpace(Opt.SYSGEOPath.value)) ? 
                Path.Combine(
                    Opt.SYSROOTPath.value,
                    Properties.Settings.Default.SYSGEOPath
                ) : Opt.SYSGEOPath.value
            );
            Opt.SYSIRCLOGPath.value =  ((string.IsNullOrWhiteSpace(Opt.SYSIRCLOGPath.value)) ?
                Path.Combine(
                    Opt.SYSROOTPath.value,
                    Properties.Settings.Default.SYSIRCLOGPath
                ) : Opt.SYSIRCLOGPath.value
            );
            Opt.SYSTMPLPath.value = ((string.IsNullOrWhiteSpace(Opt.SYSTMPLPath.value)) ?
                Path.Combine(
                    Opt.SYSROOTPath.value,
                    Properties.Settings.Default.SYSTMPLPath
                ) : Opt.SYSTMPLPath.value
            );
            Opt.CLANTag.value = ((string.IsNullOrWhiteSpace(Opt.CLANTag.value)) ? String.Empty : Opt.CLANTag.value);
            Opt.CLANAPIKey.value = ((string.IsNullOrWhiteSpace(Opt.CLANAPIKey.value)) ? String.Empty : Opt.CLANAPIKey.value);
            Opt.CLANInformerStaticEnable.bval = ((string.IsNullOrWhiteSpace(Opt.CLANInformerStaticEnable.value)) ? Properties.Settings.Default.CLANInformerStaticEnable : BuildConfig._GetBoolConfig(Opt.CLANInformerStaticEnable.value));

            Opt.SQLDBPath.value = ((string.IsNullOrWhiteSpace(Opt.SQLDBPath.value)) ? Properties.Settings.Default.SQLDBPath : Opt.SQLDBPath.value);
            Opt.SQLDBUri.value = ((string.IsNullOrWhiteSpace(Opt.SQLDBUri.value)) ? Properties.Settings.Default.SQLDBUri : Opt.SQLDBUri.value);
            Opt.SQLDBUpdateTime.num = ((string.IsNullOrWhiteSpace(Opt.SQLDBUpdateTime.value)) ? Properties.Settings.Default.SQLDBUpdateTime : BuildConfig._GetIntConfig(Opt.SQLDBUpdateTime.value));
            Opt.SQLDBFilterMemberTag.collection = BuildConfig._GetCollectionConfig(Opt.SQLDBFilterMemberTag.value);

            Opt.WEBRootUri.value = ((string.IsNullOrWhiteSpace(Opt.WEBRootUri.value)) ? Properties.Settings.Default.WEBRootUri : Opt.WEBRootUri.value);
            Opt.WEBRootPort.num = ((string.IsNullOrWhiteSpace(Opt.WEBRootPort.value)) ? Properties.Settings.Default.WEBRootPort : BuildConfig._GetIntConfig(Opt.WEBRootPort.value));
            Opt.WEBLANGDefault.value = ((string.IsNullOrWhiteSpace(Opt.WEBLANGDefault.value)) ? Opt.SYSLANGConsole.value : Opt.WEBLANGDefault.value);
            Opt.WEBCacheEnable.bval = ((string.IsNullOrWhiteSpace(Opt.WEBCacheEnable.value)) ? Properties.Settings.Default.WEBCacheEnable : BuildConfig._GetBoolConfig(Opt.WEBCacheEnable.value));
            Opt.WEBFrontEndEnable.bval = ((string.IsNullOrWhiteSpace(Opt.WEBFrontEndEnable.value)) ? Properties.Settings.Default.WEBFrontEndEnable : BuildConfig._GetBoolConfig(Opt.WEBFrontEndEnable.value));
            Opt.WEBRequestDebugEnable.bval = ((string.IsNullOrWhiteSpace(Opt.WEBRequestDebugEnable.value)) ? Properties.Settings.Default.WEBRequestDebugEnable : BuildConfig._GetBoolConfig(Opt.WEBRequestDebugEnable.value));

            Opt.DOKUWikiRootUrl.value = ((string.IsNullOrWhiteSpace(Opt.DOKUWikiRootUrl.value)) ? Properties.Settings.Default.DOKUWikiRootUrl : Opt.DOKUWikiRootUrl.value);
            Opt.DOKUWikiRootPath.value = ((string.IsNullOrWhiteSpace(Opt.DOKUWikiRootPath.value)) ? Properties.Settings.Default.DOKUWikiRootPath : Opt.DOKUWikiRootPath.value);
            Opt.DOKUWikiQuestLogin.value = ((string.IsNullOrWhiteSpace(Opt.DOKUWikiQuestLogin.value)) ? Properties.Settings.Default.DOKUWikiQuestLogin : Opt.DOKUWikiQuestLogin.value);
            Opt.DOKUWikiQuestPassword.value = ((string.IsNullOrWhiteSpace(Opt.DOKUWikiQuestPassword.value)) ? Properties.Settings.Default.DOKUWikiQuestPassword : Opt.DOKUWikiQuestPassword.value);
            Opt.DOKUWikiDefaultGroup.value = ((string.IsNullOrWhiteSpace(Opt.DOKUWikiDefaultGroup.value)) ? Properties.Settings.Default.DOKUWikiDefaultGroup : Opt.DOKUWikiDefaultGroup.value);
            Opt.DOKUWikiAuthEnable.bval = ((string.IsNullOrWhiteSpace(Opt.DOKUWikiRootPath.value)) ? false :
                ((string.IsNullOrWhiteSpace(Opt.DOKUWikiAuthEnable.value)) ?
                    Properties.Settings.Default.DOKUWikiAuthEnable :
                    BuildConfig._GetBoolConfig(Opt.DOKUWikiAuthEnable.value)
                 )
            );

            if (
                (!string.IsNullOrWhiteSpace(Opt.LOGRemoteServerEnable.value)) &&
                (!string.IsNullOrWhiteSpace(Opt.LOGRemoteServerAddress.value))
               )
            {
                Opt.LOGRemoteServerAddress.value = Opt.LOGRemoteServerAddress.value;
                Opt.LOGRemoteServerEnable.bval = BuildConfig._GetBoolConfig(Opt.LOGRemoteServerEnable.value);
                Opt.LOGRemoteServerPort.num = ((string.IsNullOrWhiteSpace(Opt.LOGRemoteServerPort.value)) ? Properties.Settings.Default.LOGRemoteServerPort : BuildConfig._GetIntConfig(Opt.LOGRemoteServerPort.value));
            }
            else
            {
                Opt.LOGRemoteServerEnable.bval = Properties.Settings.Default.LOGRemoteServerEnable;
            }
            Opt.LOGDuplicateEntry.bval = ((string.IsNullOrWhiteSpace(Opt.LOGDuplicateEntry.value)) ? Properties.Settings.Default.LOGDuplicateEntry : BuildConfig._GetBoolConfig(Opt.LOGDuplicateEntry.value));
            Opt.LOGDebug.bval = ((string.IsNullOrWhiteSpace(Opt.LOGDebug.value)) ? Properties.Settings.Default.LOGDebug : BuildConfig._GetBoolConfig(Opt.LOGDebug.value));

            for (int i = 0; i < BuildConfig.numFilters; i++)
            {
                Opt.IPFLocation[i].value = ((string.IsNullOrWhiteSpace(Opt.IPFLocation[i].value)) ?
                    ((Properties.Settings.Default.IPFLocation.Count > i) ?
                        String.Empty :
                        Properties.Settings.Default.IPFLocation[i]) :
                    Opt.IPFLocation[i].value
                );
                Opt.IPFLocationEnable[i].bval = ((string.IsNullOrWhiteSpace(Opt.IPFLocation[i].value)) ? false :
                    ((string.IsNullOrWhiteSpace(Opt.IPFLocationEnable[i].value)) ?
                        ((Properties.Settings.Default.IPFLocationEnable.Count > i) ?
                            false :
                            BuildConfig._GetBoolConfig(Properties.Settings.Default.IPFLocationEnable[i])) :
                        BuildConfig._GetBoolConfig(Opt.IPFLocationEnable[i].value)
                    )
                );
                Opt.IPFType[i].value = ((string.IsNullOrWhiteSpace(Opt.IPFType[i].value)) ?
                    ((Properties.Settings.Default.IPFType.Count > i) ?
                        Properties.Settings.Default.IPFType[i] :
                        stNet.WebHandleTypes.FileWebRequest.ToString()) : 
                    Opt.IPFType[i].value
                );
                Opt.IPFIsIpBlackList[i].bval = BuildConfig._GetBoolConfig(Opt.IPFIsIpBlackList[i].value);
                Opt.IPFIsGeoAsnBlackList[i].bval = BuildConfig._GetBoolConfig(Opt.IPFIsGeoAsnBlackList[i].value);
                Opt.IPFIsGeoCountryBlackList[i].bval = BuildConfig._GetBoolConfig(Opt.IPFIsGeoCountryBlackList[i].value);

                Opt.IPFIpList[i].collection = BuildConfig._GetCollectionConfig(Opt.IPFIpList[i].value);
                Opt.IPFGeoListCountry[i].collection = BuildConfig._GetCollectionConfig(Opt.IPFGeoListCountry[i].value);
                Opt.IPFGeoListASN[i].collection = BuildConfig._GetCollectionConfig(Opt.IPFGeoListASN[i].value);
            }
        }
        public static bool GetIniConfig(ref CoCServerConfigData.Option Opt, IMessage iLog)
        {
            if (!File.Exists(Opt.SYSCONFPath.value))
            {
                return false;
            }
            stCore.IniConfig.IniFile iniFile = null;
            try
            {
                iniFile = new stCore.IniConfig.IniFile(Opt.SYSCONFPath.value);
                if (iniFile == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.iniFileInternalError,
                            Opt.SYSCONFPath.value
                        )
                    );
                }
            }
            catch (Exception e)
            {
                if (iLog != null)
                {
                    iLog.LogError(e.Message);
                }
                return false;
            }

            /// Calculate IP Locations/Filter
            int realFilters = 0;

            foreach (var section in iniFile.Sections)
            {
                if (section.Name.StartsWith(BuildConfig.tagFilters))
                {
                    realFilters++;
                }
                if (realFilters > BuildConfig.numFilters)
                {
                    BuildConfig._InitFilter(ref Opt);
                }
            }
            BuildConfig.numFilters = ((realFilters > BuildConfig.numFilters) ? realFilters : BuildConfig.numFilters);
            /// End calculate
            
            foreach (SettingsProperty setting in Properties.Settings.Default.Properties)
            {
                string pname = setting.Name.Trim();

                if (pname.StartsWith(BuildConfig.tagFilters))
                {
                    List<OptionItem> ol = (List<OptionItem>)Opt[pname];

                    for (int i = 0; i < BuildConfig.numFilters; i++)
                    {
                        OptionItem oi = ol[i];
                        string sname = BuildConfig.nameFilters + i;
                        BuildConfig._GetIniValue(
                            iniFile.Section(sname).Get(pname),
                            ref oi
                        );
                    }
                }
                else
                {
                    string sname = pname.Substring(0, 3);
                    OptionItem oi = (OptionItem)Opt[pname];

                    BuildConfig._GetIniValue(
                        iniFile.Section(sname).Get(pname),
                        ref oi
                    );
                }
            }
            iniFile.Clear();
            return true;
        }
        public static void SetIniConfig(CoCServerConfigData.Option Opt, IMessage iLog, bool overwrite = false)
        {
            if (File.Exists(Opt.SYSCONFPath.value))
            {
                if (!overwrite)
                {
                    return;
                }
                try
                {
                    File.Delete(Opt.SYSCONFPath.value);
                }
                catch (Exception e)
                {
                    if (iLog != null)
                    {
                        iLog.LogError(e.Message);
                    }
                    return;
                }
            }

            stCore.IniConfig.IniFile iniFile = null;
            try
            {
                iniFile = new stCore.IniConfig.IniFile();
                if (iniFile == null)
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.iniFileInternalError,
                            Opt.SYSCONFPath.value
                        )
                    );
                }
            }
            catch (Exception e)
            {
                if (iLog != null)
                {
                    iLog.LogError(e.Message);
                }
                return;
            }

            List<string> sect = new List<string>();

            foreach (SettingsProperty setting in Properties.Settings.Default.Properties)
            {
                string pname = setting.Name.Substring(0, 3);
                if (!sect.Contains(pname))
                {
                    sect.Add(pname);
                }
            }
            foreach (string section in sect)
            {
                foreach (SettingsProperty setting in Properties.Settings.Default.Properties)
                {
                    string pname = setting.Name.Trim();

                    if ((section.Equals(BuildConfig.tagFilters)) && (pname.Substring(0, 3).Equals(BuildConfig.tagFilters)))
                    {
                        List<OptionItem> ol = (List<OptionItem>)Opt[pname];
                        if (ol == null)
                        {
                            continue;
                        }
                        for (int i = 0; i < BuildConfig.numFilters; i++)
                        {
                            OptionItem oi = ol[i];
                            string sname = BuildConfig.nameFilters + i;
                            iniFile.Section(sname).Set(pname, BuildConfig._ValueToString(setting, oi));
                        }
                    }
                    else if (section.Equals(pname.Substring(0, 3)))
                    {
                        OptionItem oi = (OptionItem)Opt[pname];
                        if (oi == null)
                        {
                            continue;
                        }
                        iniFile.Section(section).Set(pname, BuildConfig._ValueToString(setting, oi));
                    }
                }
            }
            if (iLog != null)
            {
                iLog.LogInfo(
                    string.Format(
                        Properties.Resources.iniFileBuildNew,
                        Opt.SYSCONFPath.value
                    )
                );
            }
            iniFile.Save(Opt.SYSCONFPath.value);
            iniFile.Clear();
        }
        private static string _ValueToString(SettingsProperty sp, CoCServerConfigData.OptionItem oi)
        {
            if ((sp == null) || (oi == null))
            {
                return String.Empty;
            }
            switch (sp.PropertyType.ToString())
            {
                case "System.String":
                    {
                        return oi.value;
                    }
                case "System.Boolean":
                    {
                        return oi.bval.ToString();
                    }
                case "System.Decimal":
                case "System.Int32":
                case "System.Int64":
                case "System.Double":
                case "System.Single":
                    {
                        return oi.num.ToString();
                    }
                case "System.Collections.Specialized.StringCollection":
                    {
                        if ((oi.collection == null) && (!string.IsNullOrWhiteSpace(oi.value)))
                        {
                            return oi.value;
                        }
                        else if (oi.collection == null)
                        {
                            break;
                        }

                        System.Collections.Specialized.StringCollection sc = oi.collection;

                        if ((sc != null) && (sc.Count > 0))
                        {
                            return string.Join(",", sc);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return String.Empty;
        }
        private static void _GetIniValue(string res, ref stCoCServerConfig.CoCServerConfigData.OptionItem oi)
        {
            if (string.IsNullOrWhiteSpace(oi.value))
            {
                if (!string.IsNullOrWhiteSpace(res))
                {
                    oi.value = res;
                }
                else
                {
                    oi.value = String.Empty;
                }
            }
        }
        private static void _InitFilter(ref CoCServerConfigData.Option Opt)
        {
            Opt.IPFLocation.Add(new OptionItem(String.Empty, String.Empty, String.Empty));
            Opt.IPFLocationEnable.Add(new OptionItem(true, String.Empty, String.Empty));
            Opt.IPFType.Add(new OptionItem(String.Empty, String.Empty, String.Empty));
            Opt.IPFIsIpBlackList.Add(new OptionItem(false, String.Empty, String.Empty));
            Opt.IPFIsGeoAsnBlackList.Add(new OptionItem(false, String.Empty, String.Empty));
            Opt.IPFIsGeoCountryBlackList.Add(new OptionItem(false, String.Empty, String.Empty));
            Opt.IPFIpList.Add(new OptionItem(String.Empty, String.Empty, String.Empty));
            Opt.IPFGeoListCountry.Add(new OptionItem(String.Empty, String.Empty, String.Empty));
            Opt.IPFGeoListASN.Add(new OptionItem(String.Empty, String.Empty, String.Empty));
        }
        private static string _GetIRCChannelName(string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return String.Empty;
            }
            return ((!src.StartsWith("#")) ? "#" + src.Trim() : src.Trim());
        }
        private static bool _GetBoolConfig(string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return false;
            }
            switch (src.ToLowerInvariant())
            {
                case "1":
                case "ok":
                case "yes":
                case "true":
                case "enable":
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
        private static int _GetIntConfig(string src)
        {
            if (!string.IsNullOrWhiteSpace(src))
            {
                int num;
                if (Int32.TryParse(src.Trim(), out num))
                {
                    return num;
                }
            }
            return 0;
        }
        private static System.Collections.Specialized.StringCollection _GetCollectionConfig(string src)
        {
            System.Collections.Specialized.StringCollection col = new System.Collections.Specialized.StringCollection();
            if (!string.IsNullOrWhiteSpace(src))
            {
                string [] srcspl = src.Split(new char[] { ',', '|', ':' });
                if (srcspl.Length > 0)
                {
                    foreach (string item in srcspl)
                    {
                        col.Add(item.Trim());
                    }
                }
            }
            return col;
        }

    }
}
