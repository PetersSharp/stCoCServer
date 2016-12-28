using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Specialized;
using stCore;

namespace stGeo
{
    public class Geo
    {
        private const string fmt = "{0}.{1}: {2}";
        private static int stGeoProvider = 0;

        private static bool _CheckIdx(int idx)
        {
            if (
                (idx < 0) ||
                ((global::stGeo.Properties.Settings.Default.stGeoURL.Count - 1) < idx)
               )
            {
                throw new stCore.IExceptionInfo(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoErrorIndex
                );
                // -> stGeo.Geo.IMsg.Line
                // -> stGeo.Geo.IMsg.Log
            }
            return true;
        }

        private static bool _CheckKey(int idx)
        {
            bool chkIdx = false;
            try
            {
                chkIdx = stGeo.Geo._CheckIdx(idx);
            }
            catch (Exception ex)
            {
                throw new stCore.IExceptionInfo(ex.Message);
            }
            if (!chkIdx)
            {
                throw new stCore.IExceptionInfo(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoErrorIndex
                );
            }
            if (
                 (Convert.ToBoolean(global::stGeo.Properties.Settings.Default.stGeoKeyRequire[idx])) &&
                 (string.IsNullOrWhiteSpace(global::stGeo.Properties.Settings.Default.stGeoKey[idx]))
                )
            {
                throw new stCore.IExceptionInfo(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoErrorKeyEmpty
                );
                // -> stGeo.Geo.IMsg.Line
                // -> stGeo.Geo.IMsg.Log
            }
            return true;
        }

        private static string _LatLongToString(double l1, double l2)
        {
            CultureInfo cult = CultureInfo.CreateSpecificCulture("en");
            return (string)l1.ToString(@"G", cult) + "," + l2.ToString(@"G", cult);
        }

        public static NameValueCollection GetQuery(double lat, double lon, int GeoProvider)
        {
            bool chkIdx = false,
                 chkKey = false;
            string loc;

            stGeo.Geo.SetEngineId(GeoProvider);

            try
            {
                chkIdx = stGeo.Geo._CheckIdx(stGeo.Geo.stGeoProvider);
                chkKey = stGeo.Geo._CheckKey(stGeo.Geo.stGeoProvider);
            }
            catch (Exception ex)
            {
                throw new stCore.IExceptionInfo(ex.Message);
            }
            if ((!chkIdx) || (!chkKey))
            {
                throw new stCore.IExceptionInfo(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoNotSupport
                );
            }

            NameValueCollection QueryCollection = new NameValueCollection();

            switch (stGeo.Geo.stGeoProvider)
            {
                case 0:
                    {
                        QueryCollection.Clear();
                        QueryCollection = null;

                        throw new stCore.IExceptionAlert(
                            fmt,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            Properties.Resources.GeoNotSupport
                        );
                        // stGeo.Geo.IMsg.Box
                    }
                case 1:
                    {
                        loc = _LatLongToString(lon, lat);
                        QueryCollection.Add("key", global::stGeo.Properties.Settings.Default.stGeoKey[1]);
                        QueryCollection.Add("location", loc);
                        QueryCollection.Add("type", "map");
                        QueryCollection.Add("zoom", "9");
                        QueryCollection.Add("size", "412,505");
                        QueryCollection.Add("imagetype", "png");
                        QueryCollection.Add("showicon", "orange_1-s");
                        break;
                    }
                case 2:
                    {
                        loc = _LatLongToString(lat, lon);
                        QueryCollection.Add("ll", loc);
                        QueryCollection.Add("l", "map");
                        QueryCollection.Add("z", "9");
                        QueryCollection.Add("size", "412,450");
                        //QueryCollection.Add("size", "412,550");  Yandex Error size
                        QueryCollection.Add("pt", loc + ",pm2rdl");
                        break;
                    }
                case 3:
                    {
                        loc = _LatLongToString(lon, lat);
                        QueryCollection.Add("key", global::stGeo.Properties.Settings.Default.stGeoKey[3]);
                        QueryCollection.Add("center", loc);
                        QueryCollection.Add("maptype", "roadmap");
                        QueryCollection.Add("zoom", "9");
                        QueryCollection.Add("size", "412,505");
                        QueryCollection.Add("format", "png");
                        QueryCollection.Add("markers", "color:red%7Clabel:C%7C" + loc);
                        break;
                    }
                default:
                    {
                        throw new stCore.IExceptionAlert(
                            fmt,
                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                            System.Reflection.MethodBase.GetCurrentMethod().Name,
                            Properties.Resources.GeoErrorIndex
                        );
                    }
            }
            return QueryCollection;
        }

        public static string GetUri()
        {
            bool chkIdx = false;
            try
            {
                chkIdx = stGeo.Geo._CheckIdx(stGeo.Geo.stGeoProvider);
            }
            catch (Exception ex)
            {
                throw new stCore.IExceptionInfo(ex.Message);
            }
            if (!chkIdx)
            {
                throw new IExceptionInfo(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoNotSupport
                );
            }
            return global::stGeo.Properties.Settings.Default.stGeoURL[stGeo.Geo.stGeoProvider];
        }

        public static string GetHelpUrl()
        {
            string src = null;
            try
            {
                src = stGeo.Geo.GetHelpUrl(stGeo.Geo.stGeoProvider);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            return src;
        }

        public static string GetHelpUrl(int idx)
        {
            bool chkIdx = false;
            try
            {
                chkIdx = stGeo.Geo._CheckIdx(idx);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            if (!chkIdx)
            {
                throw new IExceptionInfo(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoNotSupport
                );
            }
            return global::stGeo.Properties.Settings.Default.stGeoURL[idx];
        }

        public static string GetKey()
        {
            string src = null;
            try
            {
                src = stGeo.Geo.GetKey(stGeo.Geo.stGeoProvider);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            return src;
        }

        public static string GetKey(int idx)
        {
            bool chkIdx = false;
            try
            {
                chkIdx = stGeo.Geo._CheckIdx(idx);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            if (!chkIdx)
            {
                throw new IExceptionAlert(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoNotSupport
                );
            }
            return global::stGeo.Properties.Settings.Default.stGeoKey[idx];
        }

        public static bool CheckKey()
        {
            bool chkObj = false;
            try
            {
                chkObj = stGeo.Geo._CheckKey(stGeo.Geo.stGeoProvider);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            return chkObj;
        }

        public static bool CheckKey(int idx)
        {
            bool chkObj = false;
            try
            {
                chkObj = stGeo.Geo._CheckKey(idx);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            return chkObj;
        }

        public static bool SetIdx(int idx)
        {
            bool chkIdx = false;
            try
            {
                chkIdx = stGeo.Geo._CheckIdx(idx);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            if (!chkIdx)
            {
                throw new IExceptionAlert(
                    string.Format(
                        fmt,
                        System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                        System.Reflection.MethodBase.GetCurrentMethod().Name,
                        Properties.Resources.GeoNotSupport
                    )
                );
            }
            if (stGeo.Geo.stGeoProvider != idx)
            {
                stGeo.Geo.stGeoProvider =
                    ((idx >= 0) ? idx : stGeo.Geo.stGeoProvider);
            }
            return true;
        }

        public static bool SetKey(int idx, string src)
        {
            bool chkIdx = false;
            try
            {
                chkIdx = stGeo.Geo._CheckIdx(idx);
            }
            catch (Exception ex)
            {
                throw new IExceptionInfo(ex.Message);
            }
            if (!chkIdx)
            {
                throw new IExceptionAlert(
                    fmt,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    Properties.Resources.GeoNotSupport
                );
            }
            global::stGeo.Properties.Settings.Default.stGeoKey[idx] = src;
            return true;
        }

        public static void SetDefault(System.Collections.Specialized.StringCollection stGeoKey, int GeoProvider)
        {
            global::stGeo.Properties.Settings.Default.stGeoKey = stGeoKey;
            stGeo.Geo.SetEngineId(GeoProvider);
        }

        public static void SetEngineId(int idx)
        {
            stGeo.Geo.stGeoProvider = ((idx >= 0) ? idx : 0);
        }
        public static int GetEngineId()
        {
            return stGeo.Geo.stGeoProvider;
        }
    }
}
