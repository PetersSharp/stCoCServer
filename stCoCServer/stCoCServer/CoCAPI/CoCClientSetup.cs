using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stCore;
using stCoCAPI;
using stSqlite;
using System.Data;
using System.IO;

namespace stCoCServer.CoCAPI
{
    public static class CoCClientSetup
    {
        private static byte[] _setupJsonByte = null;

        public static void SaveJsonSetup(stCoCServerConfig.CoCServerConfigData.Configuration conf)
        {
            try
            {
                CoCClientSetup._setupJsonByte = CoCClientSetup.GetJsonSetup(conf);
                if (CoCClientSetup._setupJsonByte == null)
                {
                    throw new ArgumentNullException();
                }
                File.WriteAllBytes(
                    Path.Combine(
                        conf.Opt.SYSROOTPath.value,
                        conf.Opt.IPFLocation[0].value,
                        Properties.Settings.Default.setJsonSetupFile
                    ),
                    CoCClientSetup._setupJsonByte
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static byte[] GetJsonSetup(stCoCServerConfig.CoCServerConfigData.Configuration conf)
        {
            try
            {
                if (CoCClientSetup._setupJsonByte == null)
                {
                    string ver = stApp.AppInformation.GetAppVersion();
                    DataTable dt = SqliteConvertExtension.MapToDataTable<ServerSetup>();
                    DataRow dr = dt.NewRow();
                    dr["IRCServer"] = conf.Opt.IRCServer.value;
                    dr["IRCPort"] = (System.Int64)conf.Opt.IRCPort.num;
                    dr["IRCChannel"] = conf.Opt.IRCChannel.value;
                    dr["IRCLanguage"] = conf.Opt.IRCLanguage.value;
                    dr["NotifyUpdateTime"] = (System.Int64)conf.Opt.SQLDBUpdateTime.num;
                    dr["URLClan"] = CoCClientSetup._GetIPFLocation(conf, stNet.WebHandleTypes.JsonWebRequest);
                    dr["URLNotify"] = CoCClientSetup._GetIPFLocation(conf, stNet.WebHandleTypes.ServerSentEventWebRequest);
                    dr["URLInformer"] = CoCClientSetup._GetIPFLocation(conf, stNet.WebHandleTypes.InformerWebRequest);
                    dr["URLIrcLog"] = CoCClientSetup._GetIPFLocation(conf, stNet.WebHandleTypes.TemplateWebRequest);
                    dr["URLWiki"] = CoCClientSetup._GetIPFLocation(conf, stNet.WebHandleTypes.WikiWebRequest);
                    dr["ServerVersion"] = ver;
                    dr["ServerMagic"] = ver.ToMD5();

                    dt.Rows.Add(dr);
                    if ((CoCClientSetup._setupJsonByte = Encoding.UTF8.GetBytes(
                            dt.ToJson(false, true, (conf.Opt.SQLDBUpdateTime.num * 60))
                       )) == null)
                    {
                        throw new ArgumentNullException();
                    }
                }
                return CoCClientSetup._setupJsonByte;
            }
            catch (Exception e)
            {
                conf.ILog.LogError(e.Message);
                return default(byte[]);
            }
        }
        private static string _GetIPFLocation(stCoCServerConfig.CoCServerConfigData.Configuration conf, stNet.WebHandleTypes type)
        {
            int idx = conf.Opt.IPFType.FindIndex(o => o.value.Equals(type.ToString()));
            if (idx != -1)
            {
                if (conf.Opt.IPFLocationEnable[idx].bval)
                {
                    return conf.Opt.IPFLocation[idx].value;
                }
            }
            return String.Empty;
        }
    }
}
