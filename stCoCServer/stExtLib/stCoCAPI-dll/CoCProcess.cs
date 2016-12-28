#if DEBUG
// #define DEBUG_START
// #define DEBUG_MEMBERS
// #define DEBUG_CLAN
// #define DEBUG_WARLOG
// #define DEBUG_LIST
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using stCore;
using stSqlite;
using System.Data;
using System.IO;
using System.Reflection;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        private class CoCProcess : IDisposable
        {
            private dynamic _ccl = null;
            private CoCAPI _parent = null;
            private readonly object _lock = new object();
            private const string _jsondir = "jsonin";
            private const string _jsonext = ".json";
            
            private class CoCInstance
            {
                public Action<stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq, string> Update = null;
                public Func<string> Create = null;
            }

            private readonly Dictionary<stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq, CoCProcess.CoCInstance> _handleMap =
                new Dictionary<stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq, CoCProcess.CoCInstance>()
            {
                { stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Clan, null },
                { stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Members, null },
                { stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Leagues, null },
                { stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Locations, null },
                { stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Warlog, null },
                { stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.None, null }
            };

            private void InitHandleMap()
            {
                this._handleMap[stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Clan] =
                    new CoCInstance()
                    {
                        Update = this._updateClan,
                        Create = SqliteConvertExtension.MapToSQLCreateTable<ClanInfo>
                    };
                this._handleMap[stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Members] =
                    new CoCInstance()
                    {
                        Update = this._updateMember,
                        Create = SqliteConvertExtension.MapToSQLCreateTable<ClanMember>
                    };
                this._handleMap[stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Leagues] =
                    new CoCInstance()
                    {
                        Update = this._updateAllList<AllLeague>,
                        Create = SqliteConvertExtension.MapToSQLCreateTable<AllLeague>
                    };
                this._handleMap[stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Locations] =
                    new CoCInstance()
                    {
                        Update = this._updateAllList<AllLocations>,
                        Create = SqliteConvertExtension.MapToSQLCreateTable<AllLocations>
                    };
                this._handleMap[stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Warlog] =
                    new CoCInstance()
                    {
                        Update = this._updateWarlog,
                        Create = SqliteConvertExtension.MapToSQLCreateTable<WarLog>
                    };
            }

            public CoCProcess(CoCAPI api)
            {
                if ((api == null) || (api.GetType() != typeof(CoCAPI)))
                {
                    throw new ArgumentNullException(
                        string.Format(
                            Properties.Resources.CoCInitError, typeof(CoCProcess).Name, 2
                        )
                    );
                }
                this._parent = api;
                this.InitHandleMap();
                this._parent.dbMgr.RegisterFunction(typeof(WhereSeasonFunction));
            }
            ~CoCProcess()
            {
                this.Dispose();
            }
            public void Dispose()
            {
                this._curlClear();
                this._handleMap.Clear();
                GC.SuppressFinalize(this);
            }
            private void _curlClear()
            {
                if (_ccl != null)
                {
                    try
                    {
                        _ccl.Dispose();
                    }
                    catch (Exception) { }
                    _ccl = null;
                }
            }

            public void CheckTable()
            {
                if (this._parent.dbMgr.isNewDB)
                {
                    lock (_lock)
                    {
                        foreach (var handl in _handleMap)
                        {
                            if ((handl.Value != null) && (handl.Value.Create != null))
                            {
                                this._parent.dbMgr.QueryNoReturn(
                                    handl.Value.Create()
                                );
                            }
                        }
                        foreach (string cv in SQLViewRequest.AllView)
                        {
                            this._parent.dbMgr.QueryNoReturn(cv);
                        }
                    }
                }
            }
            public void Start(string clanTag)
            {
                lock (_lock)
                {
                    if (!this._StartFile())
                    {
                        this._StartRequest(clanTag);
                    }
                }
            }
            
            #region private method

            private void _StartRequest(string clanTag)
            {
                string jsonOut = String.Empty;

                try
                {
                    if (string.IsNullOrWhiteSpace(clanTag))
                    {
                        throw new ArgumentNullException(Properties.Resources.CoCClanTagEmpty);
                    }
                    if (this._parent._cocNotifier != null)
                    {
                        this._parent._cocNotifier.EventClear();
                    }
                    this._ccl = CoCRequest.InitCUrlObj(this._parent.CurlPath, this._parent.KeyAPI, this._parent.RootPath, this._parent._ilog);
                    this._parent._cocRrd.Create();

                    foreach (var handl in _handleMap)
                    {
                        if ((handl.Value != null) && (handl.Value.Update != null))
                        {
                            try
                            {
                                jsonOut = CoCRequest.GetCoCUrlMulti(handl.Key, this._parent.KeyAPI, clanTag, this._ccl, this._parent._ilog);
                                if (string.IsNullOrWhiteSpace(jsonOut))
                                {
                                    throw new CoCDBExceptionReason(
                                        string.Format(
                                            Properties.Resources.DataTableEmptyIn,
                                            handl.Key.ToString()
                                        )
                                    );
                                }
                            }
                            catch (CoCDBExceptionReason e)
                            {
                                if (handl.Key == stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq.Warlog)
                                {
                                    try
                                    {
                                        this._parent.dbMgr.QueryNoReturn(
                                            string.Format(
                                                Properties.Settings.Default.DBSysUpdateNotPublicWar,
                                                clanTag
                                            )
                                        );
                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                }
                                if (this._parent.isLogEnable)
                                {
                                    this._parent._ilog.LogError(e.Message);
                                }
                                continue;
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            handl.Value.Update(handl.Key, jsonOut);
                        }
                    }
                    this._parent._cocRrd.UpdateRrd();
                }
                catch (Exception e)
                {
#if DEBUG_START
                    stConsole.WriteHeader(e.ToString());
                    throw e;
#else
                    stCore.LogException.Error(e, this._parent._ilog);
                    return;
#endif
                }
                finally
                {
                    this._curlClear();
                }
            }
            private bool _StartFile()
            {
                bool isProccesed = false;
                string baseDir = Path.Combine(this._parent.RootPath, _jsondir);
                if (!Directory.Exists(baseDir))
                {
                    return isProccesed;
                }
                foreach (var handl in _handleMap)
                {
                    if ((handl.Value != null) && (handl.Value.Update != null))
                    {
                        string baseFile = Path.Combine(
                            baseDir,
                            handl.Key.ToString() + _jsonext
                        );
                        if (File.Exists(baseFile))
                        {
                            isProccesed = true;
                            string jsonIn = File.ReadAllText(baseFile);
                            File.Delete(baseFile);
                            handl.Value.Update(handl.Key, jsonIn);
                        }
                    }
                }
                return isProccesed;
            }

            private void _setDataTableRow(ref DataRow dro, DataRow dri, DataColumnCollection dc, string pkeySkip = null)
            {
                foreach (var rc in dc)
                {
                    string colname = rc.ToString();
                    if ((pkeySkip != null) && (pkeySkip.Equals(colname)))
                    {
                        continue;
                    }
                    dro[colname] = dri[colname];
                }
            }

            #region Update tables method

            private void _updateMember(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string jsonIn)
            {
                DataTable dt1 = null,
                          dt2 = null;

                int[] intSeason = stCoCAPI.CoCAPI.CoCSeason.GetSeasonDateInt();

                try
                {
                    dt1 = stSqlite.SqliteConvertExtension.JsonToDataTable<ClanMember>(jsonIn, true, false);
                    if ((dt1 == null) || (dt1.Rows.Count == 0))
                    {
                        stCore.LogException.Error(
                            string.Format(
                                Properties.Resources.DataTableEmptyIn,
                                typeof(ClanMember).Name
                            ),
                            this._parent._ilog
                        );
                        return;
                    }
                    dt2 = this._parent.dbMgr.Query(Properties.Settings.Default.DBSysUpdateMembers);
                    if (dt2 == null)
                    {
                        dt2 = SqliteConvertExtension.MapToDataTable<ClanMember>();
                    }
#if DEBUG_MEMBERS
                else
                {
                    if (dt2.Rows.Count > 0)
                    {
                        dt2.DataTableToPrint();
                    }
                }
#endif
                    bool isNewTable = ((dt2.Rows.Count > 0) ? false : true);

                    if (!isNewTable)
                    {
                        dt2.AsEnumerable().Where(dr => Convert.ToInt32(dr["status"]) > 0)
                             .Select(dri => dri["status"] = 2)
                             .ToList();
                    }
                    foreach (DataRow row in dt1.Rows)
                    {
                        DataRow dr = dt2.AsEnumerable()
                            .Where(r => r.Field<string>("tag")
                                .Equals(row.Field<string>("tag")))
                                    .FirstOrDefault();

                        double ratio = CoCDataUtils.DonationsRatio((int)row["send"], (int)row["receive"]);

                        if (dr == null)
                        {
                            dr = dt2.NewRow();
                            this._setDataTableRow(ref dr, row, dt1.Columns);
                            dr["status"] = 1;
                            dr["ratio"] = ratio;
                            dr["season"] = intSeason[1];
                            dr["year"] = intSeason[0];
                            dr["dtin"] = DateTime.Now;
                            dr["note"] = "";
                            dt2.Rows.Add(dr);
                        }
                        else
                        {
                            this._setDataTableRow(ref dr, row, dt1.Columns, "tag");
                            dr["status"] = 1;
                            dr["ratio"] = ratio;
                        }
                    }

                    dt1.Clear();

                    if (!isNewTable)
                    {
                        dt2.AsEnumerable().Where(r => Convert.ToInt32(r["status"]) == 2)
                             .Select(dr =>
                             {
                                 dr["status"] = 0;
                                 dr["dtout"] = DateTime.Now;
                                 return dr;
                             }).ToList();
                    }
                    if (this._parent._cocNotifier != null)
                    {
                        this._parent.NotifyEvent(type, dt2);
                    }
                    if (!this._parent.dbMgr.Update(Properties.Settings.Default.DBSysUpdateMembers, dt2))
                    {
                        throw new ArgumentException(typeof(ClanMember).Name);
                    }

#if DEBUG_MEMBERS
                dt2.DataTableToPrint();
#endif
                    dt2.Clear();
                }
                catch (Exception e)
                {
#if DEBUG_MEMBERS
                    stConsole.WriteHeader(e.ToString());
                    throw e;
#else
                    stCore.LogException.Error(e, this._parent._ilog);
                    return;
#endif
                }
            }
            private void _updateClan(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string jsonIn)
            {
                DataTable dt1 = null,
                          dt2 = null;
                CoCAPI.CoCMediaDownload md = null;

                int[] intSeason = stCoCAPI.CoCAPI.CoCSeason.GetSeasonDateInt();

                try
                {
                    dt1 = stSqlite.SqliteConvertExtension.JsonToDataTable<ClanInfo>(jsonIn, true, false);
                    if ((dt1 == null) || (dt1.Rows.Count == 0))
                    {
                        stCore.LogException.Error(
                            string.Format(
                                Properties.Resources.DataTableEmptyIn,
                                typeof(ClanInfo).Name
                            ),
                            this._parent._ilog
                        );
                        return;
                    }
                    dt2 = this._parent.dbMgr.Query(Properties.Settings.Default.DBSysUpdateClanInfo);
                    if (dt2 == null)
                    {
                        dt2 = SqliteConvertExtension.MapToDataTable<ClanInfo>();
                    }
#if DEBUG_CLAN
                    else
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            dt2.DataTableToPrint();
                        }
                    }
#endif
                    foreach (DataRow row in dt1.Rows)
                    {
                        DataRow dr = dt2.AsEnumerable()
                            .Where(r => r.Field<string>("tag")
                                .Equals(row.Field<string>("tag")))
                                    .FirstOrDefault();

                        if (dr == null)
                        {
                            dr = dt2.NewRow();
                            this._setDataTableRow(ref dr, row, dt1.Columns);
                            dr["dtup"] = DateTime.Now;
                            dt2.Rows.Add(dr);
                        }
                        else
                        {
                            this._setDataTableRow(ref dr, row, dt1.Columns, "tag");
                            dr["dtup"] = DateTime.Now;
                        }
                    }

                    dt1.Clear();

                    if (this._parent._cocNotifier != null)
                    {
                        this._parent.NotifyEvent(type, dt2);
                    }
                    if (!this._parent.dbMgr.Update(Properties.Settings.Default.DBSysUpdateClanInfo, dt2))
                    {
                        throw new ArgumentException(typeof(ClanMember).Name);
                    }

                    md = new CoCAPI.CoCMediaDownload(this._ccl, this._parent.RootPath, this._parent);
                    md.Download(CoCEnum.CoCFmtReq.Clan, dt2);

#if DEBUG_CLAN
                    dt2.DataTableToPrint();
#endif
                    dt2.Clear();
                }
                catch (Exception e)
                {
#if DEBUG_CLAN
                    stConsole.WriteHeader(e.ToString());
                    throw e;
#else
                    stCore.LogException.Error(e, this._parent._ilog);
                    return;
#endif
                }
                finally
                {
                    if (md != null)
                    {
                        md.Dispose();
                    }
                }
            }
            private void _updateWarlog(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string jsonIn)
            {
                DataTable dt1 = null,
                          dt2 = null;
                string query = string.Format(
                    Properties.Settings.Default.DBSysUpdateSelect,
                    typeof(WarLog).Name
                );
                CoCAPI.CoCMediaDownload md = null;

                try
                {
                    dt1 = stSqlite.SqliteConvertExtension.JsonToDataTable<WarLog>(jsonIn, true, false);
                    if ((dt1 == null) || (dt1.Rows.Count == 0))
                    {
                        stCore.LogException.Error(
                            string.Format(
                                Properties.Resources.DataTableEmptyIn,
                                typeof(WarLog).Name
                            ),
                            this._parent._ilog
                        );
                        return;
                    }
                    dt2 = this._parent.dbMgr.Query(query);
                    if (dt2 == null)
                    {
                        dt2 = SqliteConvertExtension.MapToDataTable<WarLog>();
                    }
#if DEBUG_WARLOG
                    else
                    {
                        if (dt2.Rows.Count > 0)
                        {
                            dt2.DataTableToPrint();
                        }
                    }
#endif
                    foreach (DataRow row in dt1.Rows)
                    {
                        DataRow dr = dt2.AsEnumerable()
                            .Where(r => r.Field<DateTime>("dtend").ToString()
                                .Equals(row.Field<string>("dtend")))
                                    .FirstOrDefault();

                        if (dr == null)
                        {
                            dr = dt2.NewRow();
                            this._setDataTableRow(ref dr, row, dt1.Columns, "dtend");
                            dr["dtend"] = stCoCAPI.CoCAPI.CoCDataUtils.FieldConvertDateTime((string)row["dtend"]);
                            dr["cdestruct"]  = CoCDataUtils.DestructionWar((double)row["cdestruct"]);
                            dr["opdestruct"] = CoCDataUtils.DestructionWar((double)row["opdestruct"]);
                            dt2.Rows.Add(dr);
                        }
                    }

                    dt1.Clear();

                    if (this._parent._cocNotifier != null)
                    {
                        this._parent.NotifyEvent(type, dt2);
                    }
                    if (!this._parent.dbMgr.Update(query, dt2))
                    {
                        throw new ArgumentException(typeof(WarLog).Name);
                    }

                    md = new CoCAPI.CoCMediaDownload(this._ccl, this._parent.RootPath, this._parent);
                    md.Download(CoCEnum.CoCFmtReq.Warlog, dt2);

#if DEBUG_WARLOG
                    dt2.DataTableToPrint();
#endif
                    dt2.Clear();
                }
                catch (Exception e)
                {
#if DEBUG_WARLOG
                    stConsole.WriteHeader(e.ToString());
                    throw e;
#else
                    stCore.LogException.Error(e, this._parent._ilog);
                    return;
#endif
                }
                finally
                {
                    if (md != null)
                    {
                        md.Dispose();
                    }
                }
            }
            private void _updateAllList<T>(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq type, string jsonIn) where T : class, new()
            {
                DataTable dt1 = null,
                          dt2 = null;
                string query = string.Format(
                    @"SELECT * FROM {0}",
                    typeof(T).Name
                );
                CoCAPI.CoCMediaDownload md = null;

                try
                {
                    dt1 = stSqlite.SqliteConvertExtension.JsonToDataTable<T>(jsonIn, true, false);
                    if ((dt1 == null) || (dt1.Rows.Count == 0))
                    {
                        stCore.LogException.Error(
                            string.Format(
                                Properties.Resources.DataTableEmptyIn,
                                typeof(T).Name
                            ),
                            this._parent._ilog
                        );
                        return;
                    }
                    dt2 = this._parent.dbMgr.Query(query);
                    if (dt2 == null)
                    {
                        dt2 = SqliteConvertExtension.MapToDataTable<T>();
                    }
                    foreach (DataRow row in dt1.Rows)
                    {
                        DataRow dr = dt2.AsEnumerable()
                            .Where(r => (Convert.ToInt32(r["id"]) == Convert.ToInt32(row["id"])))
                               .FirstOrDefault();

                        if (dr == null)
                        {
                            dr = dt2.NewRow();
                            this._setDataTableRow(ref dr, row, dt1.Columns);
                            dt2.Rows.Add(dr);
                        }
                        else
                        {
                            this._setDataTableRow(ref dr, row, dt1.Columns, "id");
                        }
                    }

                    dt1.Clear();

                    if (!this._parent.dbMgr.Update(query, dt2))
                    {
                        throw new ArgumentException(typeof(T).Name);
                    }

                    md = new CoCAPI.CoCMediaDownload(this._ccl, this._parent.RootPath, this._parent);
                    md.Download(type, dt2);

#if DEBUG_LIST
                dt2.DataTableToPrint();
#endif
                    dt2.Clear();
                }
                catch (Exception e)
                {
#if DEBUG_LIST
                    stConsole.WriteHeader(e.ToString());
                    throw e;
#else
                    stCore.LogException.Error(e, this._parent._ilog);
                    return;
#endif
                }
                finally
                {
                    if (md != null)
                    {
                        md.Dispose();
                    }
                }
            }

            #endregion
            #endregion

        }
    }
}
