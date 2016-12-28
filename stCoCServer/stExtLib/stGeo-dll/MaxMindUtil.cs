using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using stCore;
using System.Threading.Tasks;
using System.Globalization;

namespace stGeo
{
    /// <summary>
    /// Download the latest location data from http://dev.maxmind.com/geoip/legacy/geolite/
    /// You will need "GeoIPCountryCSV.zip" and "GeoIPASNum2.zip" (the ZIP CSV variant).
    /// The ZIP file contains the two CSV files you will need for the import.
    /// If first line is copyright, you will need to remove the first line from each, which is copyright text and is invalid in a CSV file. 
    /// </summary>
    public static partial class MaxMindUtil
    {
        private static stCore.IMessage _iMsg = new stCore.IMessage();
        public static stCore.IMessage iMsg
        {
            set { MaxMindUtil._iMsg = value; }
        }
        public static string GetGeoDataFileName
        {
            get { return Properties.Settings.Default.stFileGeoData; }
        }
        public static string GetGeoDataLockFileName
        {
            get { return Properties.Settings.Default.stFileGeoData + ".build"; }
        }
        public static string GetASNFileName
        {
            get { return Properties.Settings.Default.stFileMaxMindASN; }
        }
        public static string GetCountryFileName
        {
            get { return Properties.Settings.Default.stFileMaxMindCountry; }
        }
        public static string GetASNZipFileName
        {
            get { return Properties.Settings.Default.stZipFileMaxMindASN; }
        }
        public static string GetCountryZipFileName
        {
            get { return Properties.Settings.Default.stZipFileMaxMindCountry; }
        }
        public static string MaxMindDownloadASNURL
        {
            get { return Properties.Settings.Default.stMaxMindDownloadASNURL; }
        }
        public static string MaxMindDownloadCountryURL
        {
            get { return Properties.Settings.Default.stMaxMindDownloadCountryURL; }
        }

        private static stGeoRange _ConvertASNGeoData(string[] cvstag, string find, int cnt, int total, int top)
        {
            MaxMindUtil._iMsg.ProgressBar(top, cnt, total, Properties.Resources.GeoPbASN, false);
            return new stGeoRange()
            {
                ipStart = (uint)((cvstag.Length > 0) ? Convert.ToUInt32(cvstag[0]) : 0),
                ipEnd = (uint)((cvstag.Length > 1) ? Convert.ToUInt32(cvstag[1]) : 0),
                ipASNumber = (int)((cvstag.Length > 2) ? MaxMindUtil._getIntFromPartOfString(cvstag[2], find) : 0),
                ipCountry = 0
            };
        }
        private static stGeoRange _ConvertCountryGeoData(string[] cvstag, string find, int cnt, int total, int top)
        {
            MaxMindUtil._iMsg.ProgressBar(top, cnt, total, Properties.Resources.GeoPbCountry, false);
            return new stGeoRange()
            {
                ipStart = (uint)((cvstag.Length > 2)  ? MaxMindUtil._getIntFromQuotedString(cvstag[2]) : 0),
                ipEnd = (uint)((cvstag.Length > 3)    ? MaxMindUtil._getIntFromQuotedString(cvstag[3]) : 0),
                ipCountry = (int)((cvstag.Length > 4) ? cvstag[4].getCountryIdByTagQuoted() : 0),
                ipASNumber = 0
            };
        }
        private static Int32 _getIntFromQuotedString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }
            int num,
                start = ((str.StartsWith("\"")) ? 1 : 0),
                end = ((str.EndsWith("\"")) ? (str.Length - 2) : (str.Length - 1));

            if (end <= 0)
            {
                return 0;
            }
            if (Int32.TryParse(str.Substring(start, end).Trim(), out num))
            {
                return num;
            }
            return 0;
        }
        private static Int32 _getIntFromPartOfString(string str, string find)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return 0;
            }
            int num,
                start = (str.IndexOf(find) + find.Length),
                end = (str.IndexOf(" ") - find.Length);
                end = (((end > 0) && (str.Length > end)) ? end : (str.Length - find.Length));

            if (Int32.TryParse(str.Substring(start, end).Trim(), out num))
            {
                return num;
            }
            return 0;
        }
        private static List<stGeoRange> _formatToList(
            string path, string find, int skipline, int top,
            Func<string[], string, int, int, int, stGeoRange> act
        )
        {
            if (!File.Exists(path))
            {
                return new List<stGeoRange>();
            }

            int cnt = 1,
                cot = 0;
            string [] lines = null;

            try
            {
                lines = File.ReadAllLines(path);
                if (lines == null)
                {
                    throw new ArgumentException();
                }
                cot = (int)lines.Count();
            }
            catch (Exception)
            {
                return new List<stGeoRange>();
            }
            List<stGeoRange> GeoRange = (from line in lines.Skip(skipline)
                                      let columns = line.Split(',')
                                         select act(columns, find, cnt++, cot, top)
            ).ToList<stGeoRange>();
            return GeoRange;
        }
        /// <summary>
        /// parse Maxmind CVS or read produced .bin
        /// </summary>
        /// <param name="path">path to directory for Geo source and binary</param>
        /// <param name="skipline">skip begin line in CSV file</param>
        /// <param name="pb">Progress Bar function (Action)</param>
        /// <param name="isGzip">gZip input/output serialize file</param>
        /// <returns>GeoData - ASN and Country List stGeoRange base</returns>
        public static List<stGeoRange> getGeoData(string path, int top = -1)
        {
            return MaxMindUtil._getGeoData(path, 0, null, false, top, true, null);
        }
        public static List<stGeoRange> getGeoData(string path, bool isGzip = false, int top = -1, bool isLoadDb = true, stCore.IMessage ilog = null)
        {
            return MaxMindUtil._getGeoData(path, 0, null, isGzip, top, isLoadDb, ilog);
        }
        public static List<stGeoRange> getGeoData(string path, Action<int, int, int, string, bool> pb, bool isGzip = false, int top = -1, bool isLoadDb = true, stCore.IMessage ilog = null)
        {
            return MaxMindUtil._getGeoData(path, 0, pb, isGzip, top, isLoadDb, ilog);
        }
        public static List<stGeoRange> getGeoData(string path, int skipline, Action<int, int, int, string, bool> pb, bool isGzip = false, int top = -1, bool isLoadDb = true, stCore.IMessage ilog = null)
        {
            return MaxMindUtil._getGeoData(path, skipline, pb, isGzip, top, isLoadDb, ilog);
        }
        private static List<stGeoRange> _getGeoData(string path, int skipline, Action<int, int, int, string, bool> pb, bool isGzip, int top, bool isLoadDb, stCore.IMessage ilog)
        {
            int cnt = 1;
            string pathSource1 = String.Empty,
                   pathSource2 = String.Empty;
            List<stGeoRange> DataASN = null,
                             DataCountry = null;
            if (pb != null)
            {
                MaxMindUtil._iMsg.ProgressBar = pb;
            }
            pathSource1 = Path.Combine(path, MaxMindUtil.GetGeoDataFileName);
            if (File.Exists(pathSource1))
            {
                if (!isLoadDb)
                {
                    return new List<stGeoRange>();
                }
                try
                {
                    if (ilog != null)
                    {
                        ilog.LogInfo(
                            string.Format(
                                Properties.Resources.GeoBaseLoad,
                                pathSource1
                            )
                        );
                    }
                    DataASN = pathSource1.stDeserialize<stGeoRange>(isGzip);
                    return DataASN;
                }
                catch (Exception e)
                {
                    if (DataASN != null) { DataASN.Clear(); }
                    throw new ArgumentException(e.Message);
                }
            }
            pathSource1 = Path.Combine(path, MaxMindUtil.GetASNFileName);
            if (!File.Exists(pathSource1))
            {
                if (!MaxMindUtil.unzipMaxmindData(
                            path,
                            MaxMindUtil.GetASNZipFileName,
                            MaxMindUtil.GetASNFileName
                    ))
                {
                    throw new FileNotFoundException(
                        string.Format(
                            Properties.Resources.GeoMaxMindFileNotFound,
                            MaxMindUtil.GetASNFileName
                        )
                    );
                }
            }
            pathSource2 = Path.Combine(path, MaxMindUtil.GetCountryFileName);
            if (!File.Exists(pathSource2))
            {
                if (!MaxMindUtil.unzipMaxmindData(
                            path,
                            MaxMindUtil.GetCountryZipFileName,
                            MaxMindUtil.GetCountryFileName
                    ))
                {
                    throw new FileNotFoundException(
                        string.Format(
                            Properties.Resources.GeoMaxMindFileNotFound,
                            MaxMindUtil.GetCountryFileName
                        )
                    );
                }
            }
            Task<List<stGeoRange>> t1 = Task<List<stGeoRange>>.Factory.StartNew(() =>
            {
                return MaxMindUtil._formatToList(
                    pathSource1,
                    Properties.Settings.Default.stMaxMindASNPrefix,
                    skipline,
                    top,
                    MaxMindUtil._ConvertASNGeoData
                );
            });
            Task<List<stGeoRange>> t2 = Task<List<stGeoRange>>.Factory.StartNew(() =>
            {
                return MaxMindUtil._formatToList(
                    pathSource2,
                    "",
                    skipline,
                    (top + 1),
                    MaxMindUtil._ConvertCountryGeoData
                );
            });
            try
            {
                t2.Wait();
                DataCountry = t2.Result;
                if ((t2.IsFaulted) || (t2.IsCanceled) || (DataCountry == null) || (DataCountry.Count == 0))
                {
                    t2.Dispose();
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.GeoMaxMindListEmpty,
                            "Country"
                        )
                    );
                }
                t2.Dispose();
            }
            catch (Exception e)
            {
                throw new ArgumentException("DATA Country: " + e.Message);
            }
            try
            {
                t1.Wait();
                DataASN = t1.Result;
                if ((t1.IsFaulted) || (t1.IsCanceled) || (DataASN == null) || (DataASN.Count == 0))
                {
                    t1.Dispose();
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.GeoMaxMindListEmpty,
                            "ASN"
                        )
                    );
                }
                t1.Dispose();
            }
            catch (Exception e)
            {
                throw new ArgumentException("DATA ASN: " + e.Message);
            }

            MaxMindUtil._iMsg.ProgressBar((top + 1), 0, 0, null, true);

            DataASN.Where(a => ((a.ipStart > 0) && (a.ipEnd > 0))).Select(a =>
            {
                stGeoRange country = DataCountry.Where(c => 
                    (
                      ((c.ipStart >= a.ipStart) && (c.ipEnd <= a.ipEnd)) ||
                      ((a.ipStart >= c.ipStart) && (a.ipEnd <= c.ipEnd))
                    )
                ).FirstOrDefault();
                MaxMindUtil._iMsg.ProgressBar(-1, cnt++, DataASN.Count, Properties.Resources.GeoPbMerge, false);
                a.ipCountry = ((country == null) ? 0 : country.ipCountry);
                return a;
            }).ToList();

            MaxMindUtil._iMsg.ProgressBar(-1, 0, 0, null, true);

            try
            {
                DataASN.stSerialize(
                    Path.Combine(
                        path,
                        MaxMindUtil.GetGeoDataFileName
                    ),
                    isGzip
                );
            }
            catch (Exception e)
            {
                if (DataASN != null) { DataASN.Clear(); }
                throw new ArgumentException("DATA Serialize: " + e.Message);
            }
            finally
            {
                if (DataCountry != null) { DataCountry.Clear(); }
            }

#if DEBUGEO            
            foreach (var data in DataASN)
            {
                Console.WriteLine(data.ToString());
                Console.Write(" ipStart: " + data.ipStart + "\r\n");
                Console.Write(" ipEnd: " + data.ipEnd + "\r\n");
                Console.Write(" ipASNumber: " + data.ipASNumber + "\r\n");
                Console.Write(" ipCountry: " + data.ipCountry + "\r\n");
            }
#endif
            return DataASN;
        }
        public static bool unzipMaxmindData(string path, string zipName, string archName)
        {
            stCore.stZipStorer zip = null;

            try
            {
                string pathSource = Path.Combine(
                    path,
                    zipName
                );
                if (!File.Exists(pathSource))
                {
                    throw new FileNotFoundException(
                        string.Format(
                            Properties.Resources.GeoMaxMindFileNotFound,
                            zipName
                        )
                    );
                }
                zip = stCore.stZipStorer.Open(pathSource, System.IO.FileAccess.Read);
                foreach (stCore.stZipStorer.ZipFileEntry entry in (List<stCore.stZipStorer.ZipFileEntry>)zip.ReadCentralDir())
                {
                    if (Path.GetFileName(entry.FilenameInZip).Equals(archName))
                    {
                        pathSource = Path.Combine(
                            path,
                            archName
                        );
                        zip.ExtractFile(entry, pathSource);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
            finally
            {
                if (zip != null)
                {
                    zip.Close();
                    zip.Dispose();
                }
            }
        }
        public static int GetCountryId(string sid)
        {
            stGeoCountry geoc = null;
            if (
                (string.IsNullOrWhiteSpace(sid)) ||
                (sid.Length > 2) ||
                ((geoc = geoCountrys.Find(c => (c.Tag.Equals(sid)))) == null)
               )
            {
                return -1;
            }
            return geoc.Id;
        }
        public static string GetCountryNativeName(string sid)
        {
            if (string.IsNullOrWhiteSpace(sid))
            {
                return String.Empty;
            }
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => (c.Name.Equals(sid.ToLowerInvariant())))
                .Select(c =>
            {
                return c.NativeName;
            }).First();
        }
    }
}
