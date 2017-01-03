using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stCore;
using stRrd.Core;
using stRrd.Graph;
using System.IO;
using System.Data;
using SysDraw = global::System.Drawing;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        #region class stCoCRrdData

        private class stCoCRrdData
        {
            public Int64 a;
            public string type;
            public stCoCRrdData(Int64 a, string type)
            {
                this.a = a;
                this.type = type;
            }
        }

        #endregion
        
        #region class RrdPeriod

        private static class RrdPeriod
        {
            public static int Day(int period)
            {
                return RrdPeriod._PeriodCalc(period, 1);
            }
            public static int Week(int period)
            {
                return RrdPeriod._PeriodCalc(period, 7);
            }
            public static int Month(int period)
            {
                return RrdPeriod._PeriodCalc(period, 30);
            }
            public static int Year(int period)
            {
                return RrdPeriod._PeriodCalc(period, 365);
            }
            private static int _PeriodCalc(int period, int days)
            {
                return (int)((60 * 60 * 24 * days) / (period * 60));
            }
        }

        #endregion

        private static class CoCRrdUtil
        {

            #region getUnixTimeStamp

            public static long getUnixTimeStamp(DateTime time)
            {
                return Convert.ToInt32((time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            }

            #endregion

            #region getFilePath

            public static string getRrdDbPath(CoCEnum.EventNotify id, string path)
            {
                return Path.Combine(
                    path,
                    id.ToString() + ".rrd"
                );
            }
            public static string getRrdFileImgPath(CoCEnum.EventNotify id, CoCEnum.RrdGrapPeriod pr, string path)
            {
                return Path.Combine(
                    path,
                    "assets",
                    "images",
                    "rrd",
                    id.ToString() + "-" + pr.ToString() + ".png"
                );
            }
            public static string getRrdDirImgPath(string path)
            {
                return Path.Combine(
                    path,
                    "assets",
                    "images",
                    "rrd"
                );
            }

            #endregion

            #region calculate setData

            public static void setData(CoCEnum.EventNotify id, DataRow dr, stCoCRrdData data)
            {
                switch (id)
                {
                    case CoCEnum.EventNotify.MemberChangeDonationSend:
                        {
                            data.a += (
                                dr.Field<Int64>("send", DataRowVersion.Current) -
                                dr.Field<Int64>("send", DataRowVersion.Original)
                            );
                            break;
                        }
                    case CoCEnum.EventNotify.MemberChangeDonationReceive:
                        {
                            data.a += (
                                dr.Field<Int64>("receive", DataRowVersion.Current) -
                                dr.Field<Int64>("receive", DataRowVersion.Original)
                            );
                            break;
                        }
                    case CoCEnum.EventNotify.ClanChangePoints:
                        {
                            data.a = dr.Field<Int64>("points", DataRowVersion.Current);
                            break;
                        }
                    case CoCEnum.EventNotify.ClanChangeWarWin:
                        {
                            data.a = dr.Field<Int64>("warwin", DataRowVersion.Current);
                            break;
                        }
                }
            }

            #endregion
        }

        private class CoCRrd
        {
            private CoCAPI _parent = null;
            private long _startTime = 0;
            private int  _periodTime = 0;

            private Dictionary<CoCEnum.EventNotify, stCoCRrdData> _data = new Dictionary<CoCEnum.EventNotify, stCoCRrdData>()
            {
                { CoCEnum.EventNotify.MemberChangeDonationSend, new stCoCRrdData(0,"GAUGE") },
                { CoCEnum.EventNotify.MemberChangeDonationReceive, new stCoCRrdData(0,"GAUGE") },
                { CoCEnum.EventNotify.ClanChangePoints, new stCoCRrdData(0,"GAUGE") },
                { CoCEnum.EventNotify.ClanChangeWarWin, new stCoCRrdData(0,"GAUGE") }
            };

            public CoCRrd(CoCAPI parent, int period = 0)
            {
                this._parent = parent;
                this._startTime = CoCRrdUtil.getUnixTimeStamp(DateTime.Now);
                this._periodTime = period;

                if (!Directory.Exists(CoCRrdUtil.getRrdDirImgPath(this._parent._rootpath)))
                {
                    try
                    {
                        Directory.CreateDirectory(CoCRrdUtil.getRrdDirImgPath(this._parent._rootpath));
                    }
                    catch (Exception e)
                    {
                        if (this._parent.isLogEnable)
                        {
                            this._parent._ilog.LogError(e.Message);
                        }
                        throw e;
                    }
                }
            }
            public void Create()
            {
                this.SyncPeriod(0);

                foreach (var d in this._data)
                {
                    this.CreateOne((CoCEnum.EventNotify)d.Key);
                }
            }
            public void UpdateRrd()
            {
                long start = CoCRrdUtil.getUnixTimeStamp(DateTime.Now);
                CoCEnum.RrdGrapPeriod[] enumGrapPeriod = 
                    (CoCEnum.RrdGrapPeriod[])Enum.GetValues(typeof(CoCEnum.RrdGrapPeriod)).Cast<CoCEnum.RrdGrapPeriod>();

                foreach (var d in this._data)
                {
                    stCoCRrdData data = d.Value as stCoCRrdData;
                    this.UpdateRrdOne((CoCEnum.EventNotify)d.Key, data, start);

                    foreach (CoCEnum.RrdGrapPeriod pr in enumGrapPeriod)
                    {
                        this.UpdateImageOne((CoCEnum.EventNotify)d.Key, pr, this._parent._ci);
                    }
                    data.a = 0;
                }
            }
            public void UpdateValue(CoCEnum.EventNotify id, DataRow dr)
            {
                this._data.Where(o => (o.Key == id)).Select(o =>
                {
                    CoCRrdUtil.setData(id, dr, (stCoCRrdData)o.Value);
                    return o.Value;
                }).FirstOrDefault();
            }

            #region private method

            private void SyncPeriod(int period = 0)
            {
                this._periodTime = ((this._periodTime > 0) ?
                    this._periodTime :
                    ((period > 0) ? period : this._parent._pooltime));
            }
            private void CreateOne(CoCEnum.EventNotify id)
            {
                string path = CoCRrdUtil.getRrdDbPath(id, this._parent._rootpath);
                if ((string.IsNullOrWhiteSpace(path)) || (File.Exists(path)))
                {
                    return;
                }
                stCoCRrdData data = this._data.Where(o => (o.Key == id)).Select(o =>
                {
                    return (stCoCRrdData)o.Value;
                }).FirstOrDefault();

                if (data == null)
                {
                    return;
                }

                RrdDef rrdDef = null;
                RrdDb rrdDb = null;

                try
                {
                    rrdDef = new RrdDef(path);
                    rrdDef.StartTime = this._startTime;
                    rrdDef.Step = (this._periodTime * 60);

                    rrdDef.AddDatasource("a", data.type, (rrdDef.Step * 2), Double.NaN, Double.NaN);

                    rrdDef.AddArchive("AVERAGE", 0, 1, 1);
                    rrdDef.AddArchive("AVERAGE", 0, 1, RrdPeriod.Week(this._periodTime));
                    rrdDef.AddArchive("AVERAGE", 0, 1, RrdPeriod.Month(this._periodTime));
                    rrdDef.AddArchive("AVERAGE", 0, 1, RrdPeriod.Year(this._periodTime));

                    rrdDef.AddArchive("MIN", 0, 1, RrdPeriod.Day(this._periodTime));
                    rrdDef.AddArchive("MIN", 0, 1, RrdPeriod.Week(this._periodTime));
                    rrdDef.AddArchive("MIN", 0, 1, RrdPeriod.Month(this._periodTime));
                    rrdDef.AddArchive("MIN", 0, 1, RrdPeriod.Year(this._periodTime));

                    rrdDef.AddArchive("MAX", 0, 1, RrdPeriod.Day(this._periodTime));
                    rrdDef.AddArchive("MAX", 0, 1, RrdPeriod.Week(this._periodTime));
                    rrdDef.AddArchive("MAX", 0, 1, RrdPeriod.Month(this._periodTime));
                    rrdDef.AddArchive("MAX", 0, 1, RrdPeriod.Year(this._periodTime));

                    rrdDb = new RrdDb(rrdDef);
                }
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(e.Message);
                    }
                }
                finally
                {
                    if (rrdDb != null)
                    {
                        rrdDb.Close();
                    }
                }
            }
            private void UpdateRrdOne(CoCEnum.EventNotify id, stCoCRrdData data, long start)
            {
                string path1Db = CoCRrdUtil.getRrdDbPath(id, this._parent._rootpath);
                if ((string.IsNullOrWhiteSpace(path1Db)) || (!File.Exists(path1Db)))
                {
                    return;
                }
                RrdDb rrdDb = new RrdDb(path1Db);
                Sample sample = rrdDb.CreateSample(start);
                sample.SetValue("a", data.a);
                sample.Update();
                rrdDb.Close();
            }
            private void UpdateImageOne(CoCEnum.EventNotify id, CoCEnum.RrdGrapPeriod pr, CultureInfo ci)
            {
                string  path1Db = String.Empty,
                        path2Db = String.Empty,
                        pathImg = String.Empty;

                switch (id)
                {
                    case CoCEnum.EventNotify.MemberChangeDonationReceive:
                        {
                            return;
                        }
                    case CoCEnum.EventNotify.ClanChangeWarWin:
                        {
                            if (pr == CoCEnum.RrdGrapPeriod.Day)
                            {
                                return;
                            }
                            break;
                        }
                    case CoCEnum.EventNotify.MemberChangeDonationSend:
                        {
                            path2Db = CoCRrdUtil.getRrdDbPath(CoCEnum.EventNotify.MemberChangeDonationReceive, this._parent._rootpath);
                            if ((string.IsNullOrWhiteSpace(path2Db)) || (!File.Exists(path2Db)))
                            {
                                return;
                            }
                            break;
                        }
                }

                path1Db = CoCRrdUtil.getRrdDbPath(id, this._parent._rootpath);
                if ((string.IsNullOrWhiteSpace(path1Db)) || (!File.Exists(path1Db)))
                {
                    return;
                }
                pathImg = CoCRrdUtil.getRrdFileImgPath(id, pr, this._parent._rootpath);
                if (string.IsNullOrWhiteSpace(pathImg))
                {
                    return;
                }

                long dtstart, dtend;
                TimeSpan tsoffset = TimeSpan.MinValue;

                switch (pr)
                {
                    case CoCEnum.RrdGrapPeriod.Day:
                        {
                            tsoffset = new TimeSpan(1, 0, 0, 0, 0);
                            break;
                        }
                    case CoCEnum.RrdGrapPeriod.Week:
                        {
                            tsoffset = new TimeSpan(7, 0, 0, 0, 0);
                            break;
                        }
                    case CoCEnum.RrdGrapPeriod.Month:
                        {
                            tsoffset = new TimeSpan(30, 0, 0, 0, 0);
                            break;
                        }
                    case CoCEnum.RrdGrapPeriod.Year:
                        {
                            tsoffset = new TimeSpan(365, 0, 0, 0, 0);
                            break;
                        }
                }

                dtstart = CoCRrdUtil.getUnixTimeStamp(DateTime.Now.Subtract(tsoffset));
                dtend = CoCRrdUtil.getUnixTimeStamp(DateTime.Now);

                RrdGraphDef graphDef = null;

                try
                {
                    graphDef = new RrdGraphDef();
                    graphDef.SetTimePeriod(dtstart, dtend);
                    graphDef.ShowSignature = false;
                    graphDef.AntiAliasing = true;
                    graphDef.SetImageBorder(Color.White, 0);
                    graphDef.VerticalLabel = (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Vlabel", ci);

                    switch (id)
                    {
                        case CoCEnum.EventNotify.MemberChangeDonationSend:
                            {
                                graphDef.Title = string.Format(
                                    "{0} / {1} ({2})",
                                    (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationSendTitle", ci),
                                    (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationReceiveTitle", ci),
                                    (string)Properties.Resources.ResourceManager.GetString("RrdPeriodName" + pr.ToString(), ci)
                                );
                                graphDef.BackgroundResource = Properties.Resources.RrdBgMemberChangeDonationSend;
                                graphDef.Datasource("ida", path1Db, "a", "AVERAGE");
                                graphDef.Datasource("idb", path2Db, "a", "AVERAGE");
                                graphDef.Line("ida", SysDraw.Color.Lime, (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationSendTitle", ci), 4);
                                graphDef.Line("idb", SysDraw.Color.Red, (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationReceiveTitle", ci), 2);
                                graphDef.Gprint("ida", "MAX", (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationSendMax", ci));
                                //graphDef.Gprint("ida", "MIN", (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationSendMin", ci));
                                graphDef.Gprint("idb", "MAX", (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationReceiveMax", ci));
                                //graphDef.Gprint("idb", "MIN", (string)Properties.Resources.ResourceManager.GetString("fmtRrdMemberChangeDonationReceiveMin, ci"));
                                break;
                            }
                        case CoCEnum.EventNotify.ClanChangePoints:
                            {
                                graphDef.Title = string.Format(
                                    "{0} ({1})",
                                    (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Title", ci),
                                    (string)Properties.Resources.ResourceManager.GetString("RrdPeriodName" + pr.ToString(), ci)
                                );
                                graphDef.BackgroundResource = Properties.Resources.RrdBgClanChangePoints;
                                graphDef.Datasource("ida", path1Db, "a", "AVERAGE");
                                graphDef.Line("ida", SysDraw.Color.Red, (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Title", ci), 2);
                                graphDef.Gprint("ida", "MAX", (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Max", ci));
                                graphDef.Gprint("ida", "MIN", (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Min", ci));
                                break;
                            }
                        case CoCEnum.EventNotify.ClanChangeWarWin:
                            {
                                graphDef.Title = string.Format(
                                    "{0} ({1})",
                                    (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Title", ci),
                                    (string)Properties.Resources.ResourceManager.GetString("RrdPeriodName" + pr.ToString(), ci)
                                );
                                graphDef.BackgroundResource = Properties.Resources.RrdBgClanChangeWarWin;
                                graphDef.Datasource("ida", path1Db, "a", "AVERAGE");
                                graphDef.Line("ida", SysDraw.Color.Red, (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Title", ci), 2);
                                graphDef.Gprint("ida", "MAX", (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Max", ci));
                                graphDef.Gprint("ida", "MIN", (string)Properties.Resources.ResourceManager.GetString("fmtRrd" + id.ToString() + "Min", ci));
                                break;
                            }
                        default:
                            {
                                return;
                            }
                    }

                    RrdGraph graph = new RrdGraph(graphDef);
                    graph.SaveAsPNG(pathImg, 760, 200);
                }
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(e.Message);
                    }
                }
            }

            #endregion
        }
    }
}
