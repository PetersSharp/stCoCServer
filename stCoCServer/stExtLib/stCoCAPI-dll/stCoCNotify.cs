using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Net;
using stCore;
using stNet;
using stNet.stWebServerUtil;
using stSqlite;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        private class CoCNotify
        {
            #region VARIABLES

            private const int timerRespawn = 60000;
            private const string formatSseData  = "event: {0}\nid: {1}\nretry: {2}\ndata: {3}\n\n";
            private readonly string[] fieldName = new string[] { "id", "name", "tag", "vold", "vnew", "vres", "vs", "vcalc" };

            private long _evid = 0;
            private Timer _tm = null;
            private CoCAPI _parent = null;
            private readonly object _lockRss;
            private readonly object _lockData;
            private readonly object _lockClient;
            private DataTable _data = null;
            private string _rssString = null;
            private string _sseSetup = null;
            private stRSS.RSSWriter _rss = null;
            private List<CoCNotifyHost> _notifySseClient = null;
            private List<CoCNotifyField> _notifyTable = new List<CoCNotifyField>()
            {
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeName, NameField = "name" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeType, NameField = "type" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeLevel, NameField = "level" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeMembers, NameField = "members" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangePoints, NameField = "points" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeTrophies, NameField = "trophies" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeWarWin, NameField = "warwin" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeWarSeries, NameField = "warstr" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeWarPublic, NameField = "warpub" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Clan, EventId = CoCEnum.EventNotify.ClanChangeWarFrequency, NameField = "warfreq" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberNew, NameField = "nik" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberExit, NameField = "nik" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberChangeNik, NameField = "nik" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberChangeRole, NameField = "role" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberChangeLevel, NameField = "level" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberChangeLeague, NameField = "league" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberChangeTrophies, NameField = "trophies" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberChangeDonationSend, NameField = "send" },
                new CoCNotifyField() { TableId = CoCEnum.CoCFmtReq.Members, EventId = CoCEnum.EventNotify.MemberChangeDonationReceive, NameField = "receive" }
            };

            private long getEventId
            {
                get
                {
                    try
                    {
                        return this._evid;
                    }
                    finally
                    {
                        this._evid = ((this._evid >= long.MaxValue) ? 0 : (this._evid + 1));
                    }
                }
            }
            public bool ClientOnLine
            {
                get
                {
                    return ((this._notifySseClient.Count == 0) ? false : true);
                }
            }

            #endregion

            #region Constructor

            public CoCNotify(CoCAPI api)
            {
                if ((api == null) || (api.GetType() != typeof(CoCAPI)))
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.CoCInitError, typeof(CoCProcess).Name, 3
                        )
                    );
                }
                this._lockRss = new object();
                this._lockData = new object();
                this._lockClient = new object();
                this._parent = api;
                this._notifySseClient = new List<CoCNotifyHost>();
                this._data = SqliteConvertExtension.MapToDataTable<CoCNotifyEntry>();
                this._data.Locale = CultureInfo.InvariantCulture;
                this._EventRssHeader();
                this._sseSetup = this._EventSseSetupString();

                this._tm = new Timer(t =>
                {
                    this._EventSseSend(SendSseStreamAlive);
                }, null, 0, CoCNotify.timerRespawn);

#if DEBUG_TestLoop
                Task.Factory.StartNew(() =>
                {
                    this.TestLoop();
                });
#endif
            }
            ~CoCNotify()
            {
                this._Dispose();
            }

            #endregion

            #region Destructor

            private void _Dispose()
            {
                if (this._tm != null)
                {
                    this._tm.Dispose();
                }
                if (this._data != null)
                {
                    lock (this._lockData)
                    {
                        this._data.Clear();
                    }
                }
                if (this._rss != null)
                {
                    lock (this._lockRss)
                    {
                        this._rss.Clear();
                    }
                }
                if (!this.ClientOnLine)
                {
                    return;
                }
                string evstr = this._EventSseToString(
                    CoCEnum.EventNotify.ServerShutDown.ToString(),
                    DateTime.Now.ToString(),
                    this.getEventId
                );
                lock (_lockClient)
                {
                    this._notifySseClient.ForEach(res =>
                    {
                        if (!this._SendPartSse(res, evstr))
                        {
                            try
                            {
                                res.Response.Abort();
                            }
#if DEBUG
                            catch (Exception e)
                            {
                                if (this._parent.isLogEnable)
                                {
                                    this._parent._ilog.LogError(
                                        string.Format(
                                            Properties.Resources.CoCNotifyDisposeError,
                                            "Abort",
                                            res.IpAddress,
                                            e.Message
                                        )
                                    );
                                }
#else
                            catch (Exception)
                            {
#endif
                            }
                        }
                        else
                        {
                            try
                            {
                                if (res.Response.OutputStream != null)
                                {
                                    res.Response.OutputStream.Close();
                                }
                                res.Response.Close();
                            }
#if DEBUG
                            catch (Exception e)
                            {
                                if (this._parent.isLogEnable)
                                {
                                    this._parent._ilog.LogError(
                                        string.Format(
                                            Properties.Resources.CoCNotifyDisposeError,
                                            "Close",
                                            res.IpAddress,
                                            e.Message
                                        )
                                    );
                                }
#else
                            catch (Exception)
                            {
#endif
                                try
                                {
                                    res.Response.Abort();
                                }
#if DEBUG
                                catch (Exception ex)
                                {
                                    if (this._parent.isLogEnable)
                                    {
                                        this._parent._ilog.LogError(
                                            string.Format(
                                                Properties.Resources.CoCNotifyDisposeError,
                                                "Abort",
                                                res.IpAddress,
                                                ex.Message
                                            )
                                        );
                                    }
#else
                                catch (Exception)
                                {
#endif
                                }
                            }
                        }
                    });
                    this._notifySseClient.Clear();
                }
                this._rssString = String.Empty;
            }

            #endregion

            #region Private Rss method

            private void _EventRssHeader()
            {
                lock (this._lockRss)
                {
                    try
                    {
                        if ((this._rss = this._RssRootAdd(this._parent._ci)) == null)
                        {
                            throw new ArgumentNullException();
                        }
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        if (this._parent.isLogEnable)
                        {
                            this._parent._ilog.LogError(
                                string.Format(
                                    Properties.Resources.CoCNotifyProcessError,
                                    Properties.Resources.CoCNotifyDataRss,
                                    Properties.Resources.CoCNotifyDataOtherError,
                                    e.Message
                                )
                            );
                        }
#else
                    catch (Exception)
                    {
#endif
                    }
                }
            }
            private string _EventRssToString()
            {
                try
                {
                    int count = 0;
                    string xmlString = String.Empty;

                    if (this._rss == null)
                    {
                        return String.Empty;
                    }
                    lock (this._lockRss)
                    {
                        foreach (DataRow row in this._data.Rows)
                        {
                            RSSFeedItem itm = this._RssItemAdd(row, (count++), this._parent._ci);
                            if (itm != null)
                            {
                                this._rss.Add(itm);
                            }
                        }

                        xmlString = this._rss.WriteString(this._parent._ilog);

                        if (!string.IsNullOrWhiteSpace(xmlString))
                        {
                            return xmlString;
                        }
                    }
                }
#if DEBUG
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessError,
                                Properties.Resources.CoCNotifyDataRss,
                                Properties.Resources.CoCNotifyDataOtherError,
                                e.Message
                            )
                        );
                    }
#else
                catch (Exception)
                {
#endif
                }
                return String.Empty;
            }
            private string _EventRssToString(CoCNotifyHost host)
            {
                int count = 0;
                stRSS.RSSWriter rssRoot = null;
                CultureInfo ci = stNet.stWebServerUtil.HttpUtil.GetHttpClientLanguage(host.Language, this._parent._ci);

                try
                {
                    if ((rssRoot = this._RssRootAdd(ci)) == null)
                    {
                        throw new ArgumentNullException();
                    }
                    foreach (DataRow row in this._data.Rows)
                    {
                        RSSFeedItem itm = this._RssItemAdd(row, (count++), ci);
                        if (itm != null)
                        {
                            rssRoot.Add(itm);
                        }
                    }
                    return rssRoot.WriteString(this._parent._ilog);
                }
#if DEBUG
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessError,
                                Properties.Resources.CoCNotifyDataRss,
                                Properties.Resources.CoCNotifyDataOtherError,
                                e.Message
                            )
                        );
                    }
#else
                catch (Exception)
                {
#endif
                }
                return this._rssString;
            }
            private stRSS.RSSWriter _RssRootAdd(CultureInfo ci)
            {
                try
                {
                    return new stRSS.RSSWriter(
                        new RSSFeedChannel()
                        {
                            title = (string)Properties.Resources.ResourceManager.GetString("CoCNotifyRssTitle", ci),
                            link = (string)Properties.Settings.Default.NotifyRssClanLink,
                            description = string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("CoCNotifyRssDesc", ci),
                                this._parent.ClanTag,
                                DateTime.Now
                            ),
                            copyright = (string)Properties.Resources.ResourceManager.GetString("CoCNotifyRssCopy", ci)
                        }
                    );
                }
                catch (Exception)
                {
                    return null;
                }
            }
            private RSSFeedItem _RssItemAdd(DataRow row, int count, CultureInfo ci)
            {
                try
                {
                    return new RSSFeedItem()
                    {
                        title = string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("fmtNotifyRssItemTitle", ci),
                            (string)row["name"],
                            stConsole.SplitCapitalizeString((string)row["id"])
                        ).TrimEnd(),
                        link = ((((string)row["id"]).StartsWith("Clan")) ?
                            (string)Properties.Settings.Default.NotifyRssClanLink :
                            (string)Properties.Settings.Default.NotifyRssMemberLink
                        ),
                        guid = count.ToString(),
                        description = this._GetResourceFormat(row, ci),
                        category = (string)row["id"],
                        pubdate = DateTime.Now
                    };
                }
                catch (Exception)
                {
                    return null;
                }
            }
            private string _GetResourceFormat(DataRow row, CultureInfo ci = null)
            {
                try
                {
                    return string.Format(
                        (string)Properties.Resources.ResourceManager.GetObject("fmtNotify" + (string)row["id"], ci),
                        ((string.IsNullOrWhiteSpace((string)row["vres"])) ? (string)row["name"] : (string)row["vres"] + "! " + (string)row["name"]),
                        ((string.IsNullOrWhiteSpace((string)row["vcalc"])) ? "" : (string)row["vcalc"] + ", "),
                        (string)row["vold"],
                        (string)row["vnew"],
                        ((string.IsNullOrWhiteSpace((string)row["vs"])) ?
                            "" :
                            (string)row["vs"] +
                            (string)Properties.Resources.ResourceManager.GetObject("fmtNotifyWarClanEndVs", ci) +
                            (string)row["vs"]
                        )
                    ).TrimEnd();
                }
                catch (Exception)
                {
                    return "";
                }
            }

            #endregion

            #region Private Sse/Event method

            private string _EventSseToString(string evname, string evdata, long evid = 0)
            {
                return string.Format(
                    CoCNotify.formatSseData,
                    evname,
                    evid,
                    (this._parent.UpdateNextMilliseconds + 30000),
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(evdata), Base64FormattingOptions.None)
                );
            }

            private bool _EventFilterMemberTag(DataRow row)
            {
                if ((this._parent._cocFilterMemberTag == null) || (this._parent._cocFilterMemberTag.Count == 0))
                {
                    return false;
                }
                return this._parent._cocFilterMemberTag.Contains(
                    Convert.ToString(row["tag"], CultureInfo.InvariantCulture)
                );
            }

            private string _EventSseSetupString()
            {
                DataTable dt = SqliteConvertExtension.MapToDataTable<CoCNotifyEventSetup>();
                var enumNotify = (CoCEnum.EventNotify[])Enum.GetValues(typeof(CoCEnum.EventNotify)).Cast<CoCEnum.EventNotify>();

                foreach (CoCEnum.EventNotify en in enumNotify)
                {
                    if ((int)en > (int)CoCEnum.EventNotify.TestAlive)
                    {
                        DataRow dr  = dt.NewRow();
                        dr["name"]  = en.ToString();
                        dr["desc"]  = stConsole.SplitCapitalizeString(en.ToString());
                        dr["check"] = true;
                        dt.Rows.Add(dr);
                    }
                }
                return dt.ToJson(false,true,0);
            }

            private bool _EventCheckRow(DataRow row, string field)
            {
                try
                {
                    return ((
                             (row.HasVersion(DataRowVersion.Original)) &&
                             (row[field, DataRowVersion.Original] != DBNull.Value) &&
                             (!row[field, DataRowVersion.Original].Equals(row[field, DataRowVersion.Current]))
                             ) ? true : false);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private void _EventToTable(CoCEnum.EventNotify evtype, string[] fields, DataRow row)
            {
                if (fields.Length == 0)
                {
                    return;
                }
                try
                {
                    DataRow nrow = this._data.NewRow();
                    nrow[this.fieldName[0]] = evtype.ToString();

                    for (int i = 0; ((i < fields.Length) && (i < (this.fieldName.Length - 1))); i++)
                    {
                        nrow[this.fieldName[(i + 1)]] = Convert.ToString(row[fields[i]], CultureInfo.InvariantCulture);
                    }
                    nrow[this.fieldName[7]] = "";
                    this._data.Rows.Add(nrow);
                }
#if DEBUG
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessError,
                                Properties.Resources.CoCNotifyAddDataTable,
                                evtype.ToString(),
                                e.Message
                            )
                        );
                    }
#else
                catch (Exception)
                {
#endif
                }
            }
            private void _EventToTable(CoCEnum.EventNotify evtype, string name, string field, DataRow row, bool isnew = false)
            {
                try
                {
                    DataRow nrow = this._data.NewRow();
                    nrow[this.fieldName[0]] = evtype.ToString();
                    nrow[this.fieldName[1]] = Convert.ToString(row[name, DataRowVersion.Current], CultureInfo.InvariantCulture);
                    nrow[this.fieldName[2]] = Convert.ToString(row["tag", DataRowVersion.Current], CultureInfo.InvariantCulture);
                    nrow[this.fieldName[3]] = ((!isnew) ?
                        Convert.ToString(row[field, DataRowVersion.Original], CultureInfo.InvariantCulture) :
                        String.Empty
                    );
                    nrow[this.fieldName[4]] = ((!isnew) ?
                        Convert.ToString(row[field, DataRowVersion.Current], CultureInfo.InvariantCulture) :
                        String.Empty
                    );
                    nrow[this.fieldName[5]] = String.Empty;
                    nrow[this.fieldName[6]] = String.Empty;
                    nrow[this.fieldName[7]] = this._EventCalculateField(field, row, isnew);
                    this._data.Rows.Add(nrow);

                    if (!isnew)
                    {
                        this._parent._cocRrd.UpdateValue(evtype, row);
                    }
                }
#if DEBUG
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessError,
                                Properties.Resources.CoCNotifyAddDataTable,
                                evtype.ToString(),
                                e.Message
                            )
                        );
                    }
#else
                catch (Exception)
                {
#endif
                }
            }
            private string _EventCalculateField(string field, DataRow row, bool isnew)
            {
                Type t = row[field].GetType();

                try
                {
                    if (t == typeof(String))
                    {
                        return "";
                    }
                    else if (isnew)
                    {
                        throw new ArgumentException();
                    }
                    else if (t == typeof(Int32))
                    {
                        return this._EventCalculateNumber(
                            Convert.ToInt64(row.Field<Int32>(field, DataRowVersion.Current)),
                            Convert.ToInt64(row.Field<Int32>(field, DataRowVersion.Original))
                        );
                    }
                    else if (t == typeof(Int64))
                    {
                        return this._EventCalculateNumber(
                            row.Field<Int64>(field, DataRowVersion.Current),
                            row.Field<Int64>(field, DataRowVersion.Original)
                        );
                    }
                    else if (t == typeof(Single))
                    {
                        return this._EventCalculateNumber(
                            Convert.ToInt64(row.Field<Single>(field, DataRowVersion.Current)),
                            Convert.ToInt64(row.Field<Single>(field, DataRowVersion.Original))
                        );
                    }
                }
                catch (Exception)
                {
                }
                return this._EventCalculateNumber(0,0);
            }
            private string _EventCalculateNumber(Int64 vnew, Int64 vold)
            {
                if (vold == 0)
                {
                    return @"=";
                }
                else if (vold < vnew)
                {
                    return @"+" + ((int)(vnew - vold)).ToString();
                }
                else if (vold > vnew)
                {
                    return @"-" + ((int)(vold - vnew)).ToString();
                }
                return "0";
            }
            private bool _SetSseHeader(CoCNotifyHost host, string head)
            {
                try
                {
                    host.Response.ContentEncoding = Encoding.UTF8;
                    host.Response.AddHeader("Cache-Control", "no-cache");
                    host.Response.AddHeader("Access-Control-Allow-Origin", "*");
                    host.Response.AddHeader("X-Accel-Buffering", "no");
                    host.Response.KeepAlive = true;
                    host.Response.ContentType = head;
                    host.Response.StatusCode = (int)HttpStatusCode.OK;
                    return true;
                }
#if DEBUG
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessError,
                                Properties.Resources.CoCNotifySetHeader,
                                host.IpAddress,
                                e.Message
                            )
                        );
                    }
#else
                catch (Exception)
                {
#endif
                    return false;
                }
            }
            private bool _SendPartSse(CoCNotifyHost host, string msg)
            {
                try
                {
                    if (host.Response.OutputStream == null)
                    {
                        throw new ArgumentException();
                    }
                    byte [] bmsg = Encoding.UTF8.GetBytes(msg);
                    host.Response.OutputStream.Write(bmsg, 0, bmsg.Length);
                    host.Response.OutputStream.Flush();
                    return true;
                }
#if DEBUG
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessError,
                                Properties.Resources.CoCNotifySendPart,
                                host.IpAddress,
                                e.Message
                            )
                        );
                    }
#else
                catch (Exception)
                {
#endif
                    return false;
                }
            }
            private void _EventSseSend(Func<CoCNotifyHost, bool> funcSend)
            {
                Task.Factory.StartNew(() =>
                {
                    List<CoCNotifyHost> notifyend = new List<CoCNotifyHost>();

                    lock (_lockClient)
                    {
                        this._tm.Change(Timeout.Infinite, Timeout.Infinite);

                        this._notifySseClient.ForEach(host =>
                        {
                            lock (this._lockData)
                            {
                                if (!funcSend(host))
                                {
                                    notifyend.Add(host);
                                }
                            }
                        });
                        if (notifyend.Count > 0)
                        {
                            notifyend.ForEach(host =>
                            {
                                try
                                {
                                    this._notifySseClient.Remove(host);
                                    host.Response.Abort();
                                }
                                catch (Exception) { }
                            });
                            notifyend.Clear();
                        }

                        this._tm.Change(CoCNotify.timerRespawn, CoCNotify.timerRespawn);
                    }
                });
            }
            private bool _DataSseSend(CoCNotifyHost host)
            {
                if (this._data.Rows.Count == 0)
                {
                    if (!this.SendSseStreamAlive(host))
                    {
                        return false;
                    }
                    return true;
                }
                try
                {
                    foreach (DataRow row in this._data.Rows)
                    {
                        if (!this._SendPartSse(host, this._EventSseToString((string)row["id"], row.ToJson())))
                        {
                            return false;
                        }
                    }
                }
#if DEBUG
                catch (Exception e)
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessError,
                                Properties.Resources.CoCNotifyDataSseSend,
                                host.IpAddress,
                                e.Message
                            )
                        );
                    }
#else
                catch (Exception)
                {
#endif
                }
                return true;
            }
            private bool _SetupSseSend(CoCNotifyHost host)
            {
                return this._SendPartSse(host, this._EventSseToString(CoCEnum.EventNotify.EventSetup.ToString(), this._sseSetup, this.getEventId));
            }

            #endregion

            #region Public method json notify

            public bool SendJsonComplette(CoCNotifyHost host)
            {
                if (!this._SetSseHeader(host, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeJson)))
                {
                    return false;
                }
                return this._SendPartSse(host, this._data.ToJson(true, false, (this._parent.UpdateNextMilliseconds + 35000)));
            }

            #endregion

            #region Public method Rss/xml

            public bool SendRssComplette(CoCNotifyHost host)
            {
                if (!this._SetSseHeader(host, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeXml)))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(host.Language))
                {
                    return this._SendPartSse(host, this._rssString);
                }
                else
                {
                    string rssString = this._EventRssToString(host);
                    return this._SendPartSse(
                        host, 
                        ((string.IsNullOrWhiteSpace(rssString)) ?
                            CoCEnum.EventNotify.ServerError.ToString() :
                            rssString
                        )
                    );
                }
            }
            public bool SendRssCompletteStatic(CoCNotifyHost host)
            {
                if (!this._SetSseHeader(host, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeXml)))
                {
                    return false;
                }
                return this._SendPartSse(host, this._rssString);
            }
            public string GetRowToString(DataRow row, CultureInfo ci)
            {
                return this._GetResourceFormat(row, ci);
            }

            #endregion

            #region Public method Sse Stream

            public bool SendSseStreamPart(CoCNotifyHost host, string evname, string evdata)
            {
                return this._SendPartSse(host, this._EventSseToString(evname, evdata, this.getEventId));
            }
            public bool SendSseStreamAlive(CoCNotifyHost host)
            {
                return this._SendPartSse(host, this._EventSseToString(CoCEnum.EventNotify.TestAlive.ToString(), DateTime.Now.ToString(), this.getEventId));
            }
            public bool SendSseStreamComplette(CoCNotifyHost host, string evname, string evdata)
            {
                if (!this._SetSseHeader(host, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeSse)))
                {
                    return false;
                }
                return this._SendPartSse(host, this._EventSseToString(evname, evdata, this.getEventId));
            }
            public void AddSseHost(CoCNotifyHost host)
            {
                if (host == null)
                {
                    return;
                }
                if (
                    (!this._SetSseHeader(host, HttpUtil.GetMimeType("", HttpUtil.MimeType.MimeSse))) ||
                    (!this._SetupSseSend(host))
                   )
                {
                        try
                        {
                            host.Response.Abort();
                        }
                        catch (Exception) { }
                        return;
                }
                Task.Factory.StartNew(() =>
                {
                    lock (this._lockClient)
                    {
                        if (this._notifySseClient.Find(o => (o.Response.Equals(host.Response))) == null)
                        {
                            try
                            {
                                this._notifySseClient.Add(host);
                            }
#if DEBUG
                            catch (Exception e)
                            {
                                if (this._parent.isLogEnable)
                                {
                                    this._parent._ilog.LogError(
                                        string.Format(
                                            Properties.Resources.CoCNotifyProcessError,
                                            Properties.Resources.CoCNotifyAddClient,
                                            host.IpAddress,
                                            e.Message
                                        )
                                    );
                                }
#else
                            catch (Exception)
                            {
#endif
                                return;
                            }
                        }
                    } // end lock client
#if DEBUG
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogInfo(
                            string.Format(
                                Properties.Resources.CoCNotifyProcessDebug,
                                host.IpAddress,
                                Properties.Resources.CoCNotifyHostDataCount,
                                this._notifySseClient.Count + "/" + this._data.Rows.Count
                            )
                        );
                    }
#endif
                    if (this._data.Rows.Count > 0)
                    {
                        lock (this._lockData)
                        {
                            if (!this._DataSseSend(host))
                            {
                                lock (_lockClient)
                                {
                                    this._notifySseClient.Remove(host);
                                }
                            }
                        } // end lock data
                    }
                });
            }

            #endregion

            #region Public method Event

            public void EventClear()
            {
                if (this._data.Rows.Count > 0)
                {
                    lock (this._lockData)
                    {
                        this._data.Clear();
                    }
                }
                if (this._rss != null)
                {
                    lock (this._lockRss)
                    {
                        this._rss.Clear();
                    }
                }
            }
            public void Event(CoCEnum.CoCFmtReq typeId, DataTable dt)
            {
                switch (typeId)
                {
                    case CoCEnum.CoCFmtReq.Clan:
                        {
                            this._notifyTable.ForEach(notify =>
                            {
                                if (
                                    (notify.TableId == typeId) &&
                                    (!string.IsNullOrWhiteSpace(notify.NameField))
                                   )
                                {
                                    if (this._EventCheckRow(dt.Rows[0], notify.NameField))
                                    {
                                        this._EventToTable(notify.EventId, "name", notify.NameField, dt.Rows[0]);
                                    }
                                }
                            });
                            if ((this._parent._isInformerStatic) && (this._parent._cocInformer != null))
                            {
                                try
                                {
                                    this._parent._cocInformer.CreateClanInformerAll(dt.Rows[0]);
                                }
#if DEBUG
                                catch (Exception e)
                                {
                                    if (this._parent.isLogEnable)
                                    {
                                        this._parent._ilog.LogError(
                                            string.Format(
                                                Properties.Resources.CoCInformerError,
                                                e.Message
                                            )
                                        );
                                    }
#else
                                catch (Exception)
                                {
#endif
                                }
                            }
                            break;
                        }
                    case CoCEnum.CoCFmtReq.Warlog:
                        {
                            foreach (DataRow addedRow in dt.Select(null, null, DataViewRowState.Added))
                            {
                                this._EventToTable(
                                    CoCEnum.EventNotify.WarClanEnd,
                                    new string[] { "opname", "optag", "cdestruct", "opdestruct", "result", "members" },
                                    addedRow
                                );
                            }
                            break;
                        }
                    case CoCEnum.CoCFmtReq.Members:
                        {
                            foreach (DataRow addedRow in dt.Select(null, null, DataViewRowState.Added))
                            {
                                if (this._EventFilterMemberTag(addedRow))
                                {
                                    continue;
                                }
                                this._EventToTable(CoCEnum.EventNotify.MemberNew, "nik", "level", addedRow, true);
                            }
                            foreach (DataRow updateRow in dt.Select(null, null, DataViewRowState.ModifiedCurrent))
                            {
                                if (this._EventFilterMemberTag(updateRow))
                                {
                                    continue;
                                }
                                this._notifyTable.ForEach(notify =>
                                {
                                    if (
                                        (notify.TableId == typeId) &&
                                        (!string.IsNullOrWhiteSpace(notify.NameField))
                                       )
                                    {
                                        switch (notify.EventId)
                                        {
                                            case CoCEnum.EventNotify.MemberNew:
                                                {
                                                    break;
                                                }
                                            case CoCEnum.EventNotify.MemberExit:
                                                {
                                                    Int64 status = (Int64)updateRow["status", DataRowVersion.Current];
                                                    if (status == 0)
                                                    {
                                                        this._EventToTable(notify.EventId, "nik", notify.NameField, updateRow, true);
                                                    }
                                                    break;
                                                }
                                            default:
                                                {
                                                    if (this._EventCheckRow(updateRow, notify.NameField))
                                                    {
                                                        this._EventToTable(notify.EventId, "nik", notify.NameField, updateRow);
                                                    }
                                                    break;
                                                }
                                        }
                                    }

                                });
                            }
                            break;
                        }
                }
                if (this._data.Rows.Count > 0)
                {
                    this._rssString = this._EventRssToString();
                    this._EventSseSend(this._DataSseSend);
                }
            }
            public DataTable EventGetData()
            {
                lock (this._lockData)
                {
                    return this._data.Copy();
                }
            }

            #endregion

            #region Conditional DEBUG

            [Conditional("DEBUG")]
            public void TestLoop()
            {
                List<CoCNotifyHost> notifyend = new List<CoCNotifyHost>();

                while (true)
                {
                    string msg = this._EventSseToString("testkeepalive", "testing connect " + DateTime.Now.ToString());

                    lock (this._lockClient)
                    {
                        this._notifySseClient.ForEach(host =>
                        {
                            if (!this._SendPartSse(host, msg))
                            {
                                stCore.stConsole.WriteHeader("*** Catch to notify END, SendPartSse == false");
                                notifyend.Add(host);
                            }
                        });
                        notifyend.ForEach(host =>
                        {
                            try
                            {
                                this._notifySseClient.Remove(host);
                                host.Response.Abort();
                            }
                            catch (Exception e)
                            {
                                stCore.stConsole.WriteHeader("*** Catch remove TestLoop\n" + e.Message);
                            }
                        });
                        notifyend.Clear();
                    }
                    System.Threading.Thread.Sleep(10000);
                }
            }
            
            #endregion
        }

        #region Public CoCAPI method

        public void NotifySseAdd(CoCNotifyHost host)
        {
            this._cocNotifier.AddSseHost(host);
        }
        public bool NotifySendSseStreamPart(CoCNotifyHost host, string evname, string evdata)
        {
            return this._cocNotifier.SendSseStreamPart(host, evname, evdata);
        }
        public bool NotifySendSseStreamComplette(CoCNotifyHost host, string evname, string evdata)
        {
            return this._cocNotifier.SendSseStreamComplette(host, evname, evdata);
        }
        public bool NotifySendJsonComplette(CoCNotifyHost host)
        {
            return this._cocNotifier.SendJsonComplette(host);
        }
        public bool NotifySendRssComplette(CoCNotifyHost host)
        {
            return this._cocNotifier.SendRssComplette(host);
        }
        
        public void NotifyEvent(CoCEnum.CoCFmtReq typeId, DataTable dt)
        {
            this._cocNotifier.Event(typeId, dt);
        }
        public DataTable NotifyEventGetData()
        {
            return this._cocNotifier.EventGetData();
        }
        public string NotifyEventGetRowToString(DataRow row, CultureInfo ci)
        {
            return this._cocNotifier.GetRowToString(row, ci);
        }

        #endregion
    }
}
