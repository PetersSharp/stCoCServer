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
    class GeoUpdateMain
    {
        private static string rootAppPath = String.Empty;
        private static bool downloadComplete = false;
        private static bool isQuiet = false;

        static void Main(string[] args)
        {
            bool _isRemove = false,
                 _isCreate = false;
            string _geoPath = String.Empty,
                   _geoLock = String.Empty;
            stGeo.GeoFilter _geof = null;
            stCore.IniConfig.IniFile _iniFile = null;
            stCore.IMessage _iLog = null;
            GeoUpdateMain.rootAppPath = stCore.IOBaseAssembly.BaseDataDir();

            Thread.CurrentThread.Name = stCore.IOBaseAssembly.BaseName(Assembly.GetExecutingAssembly());

            args.Process(
               () => { },
               new CommandLine.Switch("geodir", val => _geoPath = string.Join("", val), "-d"),
               new CommandLine.Switch("clear",  val => _isRemove = true, "-c"),
               new CommandLine.Switch("create", val => _isCreate = true, "-a"),
               new CommandLine.Switch("quiet",  val => GeoUpdateMain.isQuiet = true, "-q")
            );

            if (GeoUpdateMain.isQuiet)
            {
                _iLog = new IMessage();
            }
            else
            {
                _iLog = new IMessage()
                {
                    LogInfo = GeoUpdateMain.PrnInfo,
                    LogError = GeoUpdateMain.PrnError,
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
                if (string.IsNullOrWhiteSpace(_geoPath))
                {
                    try
                    {
                        string iniPath =  Path.Combine(
                                GeoUpdateMain.rootAppPath,
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
                        _iniFile = new stCore.IniConfig.IniFile(iniPath);
                        if (_iniFile == null)
                        {
                            throw new FileLoadException(
                                string.Format(
                                    Properties.Resources.GeoInErrorLoad,
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
                        _geoPath = _iniFile.Section("SYS").Get("SYSGEOPath");
                        if (_geoPath == null)
                        {
                            throw new ArgumentNullException(Properties.Resources.GeoConfigNotFound);
                        }
                    }
                    catch (Exception e)
                    {
                        _geoPath = Path.Combine(
                                GeoUpdateMain.rootAppPath,
                                "geo"
                        );
                        if (!Directory.Exists(_geoPath))
                        {
                            _iLog.LogError(e.Message);
                            return;
                        }
                    }
                    finally
                    {
                        if (_iniFile != null)
                        {
                            _iniFile.Clear();
                        }
                    }
                }
                if (!Directory.Exists(_geoPath))
                {
                    if (_isCreate)
                    {
                        Directory.CreateDirectory(_geoPath);
                    }
                    else
                    {
                        throw new DirectoryNotFoundException(Properties.Resources.GeoDirNotFound);
                    }
                }

                string[] allFiles = GeoUpdateMain.MakeFileList(_geoPath);

                if (_isRemove)
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
                    if (!GeoUpdateMain.GetUrlToFile(
                            stGeo.MaxMindUtil.MaxMindDownloadASNURL,
                            stGeo.MaxMindUtil.GetASNZipFileName,
                            _geoPath,
                            _iLog)
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
                    if (!GeoUpdateMain.GetUrlToFile(
                            stGeo.MaxMindUtil.MaxMindDownloadCountryURL,
                            stGeo.MaxMindUtil.GetCountryZipFileName,
                            _geoPath,
                            _iLog)
                       )
                    {
                        throw new FileNotFoundException(Properties.Resources.GeoFileNotFound);
                    }
                }
                if (File.Exists(allFiles[4]))
                {
                    File.Delete(allFiles[4]);
                }
                _geof = new GeoFilter(_iLog, false);
                if (_geof == null)
                {
                    throw new ArgumentNullException(Properties.Resources.GeoClassInitError);
                }

                _geoLock = Path.Combine(_geoPath, MaxMindUtil.GetGeoDataLockFileName);
                File.Create(_geoLock);

                if (!_geof.InitBase(_geoPath, false, stConsole.GetCursorAlign(2)))
                {
                    GeoUpdateMain.PrnError(Properties.Resources.GeoClassInitError);
                }
                else
                {
                    GeoUpdateMain.PrnInfo(
                        string.Format(
                            Properties.Resources.GeoComplette,
                            _geoPath
                        )
                    );
                }
            }
            catch (Exception e)
            {
                GeoUpdateMain.PrnError(e.Message);
            }
            finally
            {
                try
                {
                    if (_geof != null)
                    {
                        _geof.Dispose();
                    }
                    if (
                        (!string.IsNullOrWhiteSpace(_geoLock)) &&
                        (File.Exists(_geoLock))
                       )
                    {
                        File.Delete(_geoLock);
                        string geoDb = Path.Combine(_geoPath, MaxMindUtil.GetGeoDataFileName);
                        if (File.Exists(geoDb))
                        {
                            File.SetCreationTime(geoDb, DateTime.Now);
                        }
                    }
                }
                catch (Exception e)
                {
                    GeoUpdateMain.PrnError(e.Message);
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
            GeoUpdateMain.downloadComplete = false;
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
                    GeoUpdateMain.downloadComplete = true;
                };
                wcl.DownloadFileAsync(new Uri(url + file), geoPath, null);
                while (!GeoUpdateMain.downloadComplete)
                {
                    Thread.Sleep(1000);
                }
                stConsole.NewLine();
                return true;
            }
            catch (Exception e)
            {
                GeoUpdateMain.PrnError(e.Message);
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
            stConsole.MessageInfo(Properties.Resources.PrnOK, msg, ((!GeoUpdateMain.isQuiet) ? true : false));
        }
        public static void PrnError(string msg)
        {
            stConsole.MessageError(Properties.Resources.PrnError, msg, ((!GeoUpdateMain.isQuiet) ? true : false));
        }

    #endregion

    }
}
