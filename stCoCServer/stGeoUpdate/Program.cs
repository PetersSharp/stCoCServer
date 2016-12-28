using System;
using System.IO;
using System.Threading;
using stCore;
using stNet;
using stGeo;
using System.Reflection;
using System.Globalization;

namespace stGeoUpdate
{
    class Program
    {
        private static string rootAppPath = String.Empty;
        private static bool downloadComplete = false;
        private static bool isQuiet = false;

        static void Main(string[] args)
        {
            bool isRemove = false,
                 isCreate = false;
            string geoPath = String.Empty;
            stGeo.GeoFilter ge = null;
            stCore.IniConfig.IniFile iniFile = null;
            stCore.IMessage iLog = null;
            Program.rootAppPath = stCore.IOBaseAssembly.BaseDataDir();

            Thread.CurrentThread.Name = stCore.IOBaseAssembly.BaseName(Assembly.GetExecutingAssembly());

            args.Process(
               () => { },
               new CommandLine.Switch("geodir", val => geoPath = string.Join("", val), "-d"),
               new CommandLine.Switch("clear",  val => isRemove = true, "-c"),
               new CommandLine.Switch("create", val => isCreate = true, "-a"),
               new CommandLine.Switch("quiet",  val => Program.isQuiet = true, "-q")
            );

            if (Program.isQuiet)
            {
                iLog = new IMessage();
            }
            else
            {
                iLog = new IMessage()
                {
                    LogInfo = Program.PrnInfo,
                    LogError = Program.PrnError,
                    ProgressBar = stConsole.ProgressTxt
                };
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
            try
            {
                if (string.IsNullOrWhiteSpace(geoPath))
                {
                    try
                    {
                        string iniPath =  Path.Combine(
                                Program.rootAppPath,
                                "stCoCServer.ini"
                        );
                        if (!File.Exists(iniPath))
                        {
                            throw new FileNotFoundException(
                                string.Format(
                                    Properties.Resources.GeoIniNotFound,
                                    iniPath
                                )
                            );
                        }
                        iniFile = new stCore.IniConfig.IniFile(iniPath);
                        if (iniFile == null)
                        {
                            throw new FileLoadException(
                                string.Format(
                                    Properties.Resources.GeoInErrorLoad,
                                    iniPath
                                )
                            );
                        }
                        string lng = iniFile.Section("SYS").Get("SYSLANGConsole");
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
                        geoPath = iniFile.Section("SYS").Get("SYSGEOPath");
                        if (geoPath == null)
                        {
                            throw new ArgumentNullException(Properties.Resources.GeoConfigNotFound);
                        }
                    }
                    catch (Exception e)
                    {
                        geoPath = Path.Combine(
                                Program.rootAppPath,
                                "geo"
                        );
                        if (!Directory.Exists(geoPath))
                        {
                            iLog.LogError(e.Message);
                            return;
                        }
                    }
                    finally
                    {
                        if (iniFile != null)
                        {
                            iniFile.Clear();
                        }
                    }
                }
                if (!Directory.Exists(geoPath))
                {
                    if (isCreate)
                    {
                        Directory.CreateDirectory(geoPath);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(Properties.Resources.GeoDirNotFound);
                    }
                }

                string[] allFiles = Program.MakeFileList(geoPath);

                if (isRemove)
                {
                    foreach (string fn in allFiles)
                    {
                        if (File.Exists(fn))
                        {
                            File.Delete(fn);
                        }
                    }
                }
                if (
                    (!File.Exists(allFiles[0])) &&
                    (!File.Exists(allFiles[1]))
                   )
                {
                    if (!Program.GetUrlToFile(
                            stGeo.MaxMindUtil.MaxMindDownloadASNURL,
                            stGeo.MaxMindUtil.GetASNZipFileName,
                            geoPath,
                            iLog)
                       )
                    {
                        throw new FileNotFoundException(Properties.Resources.GeoFileNotFound);
                    }
                }
                if (
                    (!File.Exists(allFiles[2])) &&
                    (!File.Exists(allFiles[3]))
                   )
                {
                    if (!Program.GetUrlToFile(
                            stGeo.MaxMindUtil.MaxMindDownloadCountryURL,
                            stGeo.MaxMindUtil.GetCountryZipFileName,
                            geoPath,
                            iLog)
                       )
                    {
                        throw new FileNotFoundException(Properties.Resources.GeoFileNotFound);
                    }
                }
                if (File.Exists(allFiles[4]))
                {
                    File.Delete(allFiles[4]);
                }
                ge = new GeoFilter(iLog, false);
                if (ge == null)
                {
                    throw new ArgumentNullException(Properties.Resources.GeoClassInitError);
                }
                if (!ge.InitBase(geoPath, false, stConsole.GetCursorAlign(2)))
                {
                    Program.PrnError(Properties.Resources.GeoClassInitError);
                }
                else
                {
                    Program.PrnInfo(
                        string.Format(
                            Properties.Resources.GeoComplette,
                            geoPath
                        )
                    );
                }
            }
            catch (Exception e)
            {
                Program.PrnError(e.Message);
            }
            finally
            {
                if (ge != null)
                {
                    ge.Dispose();
                }
            }

#if DEBUG
            Console.ReadLine();
#endif
            return;
        }

        #region File list

        private static string[] MakeFileList(string geoPath)
        {
            return new string[] { 
                        Path.Combine(
                            geoPath,
                            stGeo.MaxMindUtil.GetASNFileName
                        ),
                        Path.Combine(
                            geoPath,
                            stGeo.MaxMindUtil.GetASNZipFileName
                        ),
                        Path.Combine(
                            geoPath,
                            stGeo.MaxMindUtil.GetCountryFileName
                        ),
                        Path.Combine(
                            geoPath,
                            stGeo.MaxMindUtil.GetCountryZipFileName
                        ),
                        Path.Combine(
                            geoPath,
                            stGeo.MaxMindUtil.GetGeoDataFileName
                        )
                };
        }

        #endregion
        
        #region Get files from URL

        public static bool GetUrlToFile(string url, string file, string path, stCore.IMessage iLog)
        {
            if (
                (string.IsNullOrWhiteSpace(url)) ||
                (string.IsNullOrWhiteSpace(file)) ||
                (string.IsNullOrWhiteSpace(path))
               )
            {
                throw new ArgumentNullException();
            }

            stNet.stWebClient wcl = null;
            string geoPath =
                Path.Combine(
                    path,
                    file
                );
            Program.downloadComplete = false;
            var cursor = stConsole.WriteGetPosition(
                string.Format(
                    Properties.Resources.GeoFileDownload,
                    "     ",
                    file
                )
            );

            try
            {
                wcl = new stWebClient();
                wcl.wUserAgent = stNet.stWebServerUtil.HttpUtil.GetHttpUA(@"stGeoUpdate Client");
                wcl.iLog = iLog;
                wcl.DownloadProgressChanged += (s, e) =>
                {
                    stConsole.WriteToPosition(" " + e.ProgressPercentage + "% ", cursor);
                };
                wcl.DownloadFileCompleted += (s, e) =>
                {
                    Program.downloadComplete = true;
                };
                wcl.DownloadFileAsync(new Uri(url + file), geoPath, null);
                while (!Program.downloadComplete)
                {
                    Thread.Sleep(1000);
                }
                stConsole.NewLine();
                return true;
            }
            catch (Exception e)
            {
                Program.PrnError(e.Message);
                if (!File.Exists(geoPath))
                {
                    File.Delete(geoPath);
                }
                return false;
            }
            finally
            {
                if (wcl != null)
                {
                    wcl.Dispose();
                }
            }
        }

        #endregion

        #region CONSOLE MSG

        public static void PrnInfo(string msg)
        {
            stConsole.MessageInfo(Properties.Resources.PrnOK, msg, ((!Program.isQuiet) ? true : false));
        }
        public static void PrnError(string msg)
        {
            stConsole.MessageError(Properties.Resources.PrnError, msg, ((!Program.isQuiet) ? true : false));
        }

    #endregion

    }
}
