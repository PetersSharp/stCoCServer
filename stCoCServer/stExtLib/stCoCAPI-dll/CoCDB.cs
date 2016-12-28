using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stCore;
using System.Collections.Specialized;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        ///<summary>
        ///Clash of Clan main selector
        ///</summary>
        private static class CoCDB
        {
            private const string thisClass = "[CoCDB]: ";

            #region GetLeague

            ///<summary>
            /// Get League string, see enum LeagueTypeReq
            ///</summary>
            // [Obsolete("Use CoCMediaSet, CoCMediaSetInfo")]
            public static string _GetLeague(stSqlite.Wrapper dbMgr, string leagueId, stCoCAPI.CoCAPI.CoCEnum.LeagueTypeReq type)
            {
                string field = "",
                       league = "none";

                if (string.IsNullOrWhiteSpace(leagueId))
                {
                    return league;
                }
                switch (type)
                {
                    case stCoCAPI.CoCAPI.CoCEnum.LeagueTypeReq.Name:
                        {
                            field = "name";
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.LeagueTypeReq.UrlSmall:
                        {
                            field = "small";
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.LeagueTypeReq.UrlMedium:
                        {
                            field = "medium";
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.LeagueTypeReq.UrlTiny:
                        {
                            field = "tiny";
                            break;
                        }
                    default:
                        {
                            return league;
                        }
                }
                try
                {
                    league = dbMgr.QueryOneReturnString(
                        string.Format(
                            Properties.Settings.Default.DBSelectLeaque,
                            field,
                            leagueId
                        )
                    );
                    return league;
                }
                catch (Exception e)
                {
                    throw new CoCDBException(
                        stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtNone,
                        CoCDB.thisClass + e.Message
                    );
                }
            }

            #endregion

            #region GetHideTagSelect

            private static string _GetHideTagSelect(StringCollection hide)
            {
                if ((hide == null) || (hide.Count == 0))
                {
                    return "";
                }
                
                StringBuilder sb = new StringBuilder();

                foreach (string tag in hide)
                {
                    sb.Append(" AND tag != '" + tag + "'");
                }
                return sb.ToString();
            }

            #endregion
            
            #region Get DB Query string

            /// <summary>
            /// Get Sqlite DB Query string
            /// </summary>
            /// <param name="cmd">array Query string</param>
            /// <param name="cReq">enum ClanTypeReq</param>
            /// <param name="LogError">Log Error <code>Action{string}</code></param>
            /// <returns>DB query syting</returns>
            public static string _GetQueryString(
                string[] cmd,
                ref stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq cReq,
                StringCollection hide,
                Action<string> LogError
                )
            {
                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None;
                string query = String.Empty,
                       playerId = "";

                switch (cmd.Length)
                {
                    default:
                    case 0:
                    case 1:
                        {
                            throw new CoCDBException(
                                stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtClanHelp,
                                string.Format(
                                    Properties.Resources.CoCDBExceptionLength,
                                    "QueryString->begin"
                                )
                            );
                        }
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                        {
                            int idx = 1;
                            if (cmd[1].Equals("notify"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.ServerSetup;
                                throw new CoCDBException(
                                    stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtNone,
                                    CoCDB.thisClass
                                );
                            }
                            if (cmd[1].Equals("info"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Info;
                                query = Properties.Settings.Default.DBSelectClanInfo;
                            }
                            else if (cmd[1].Equals("war"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.War;
                                query = Properties.Settings.Default.DBSelectClanInfo;
                            }
                            else if (cmd[1].Equals("stat"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Statistic;
                                query = Properties.Settings.Default.DBSelectClanInfo;
                            }
                            else if (cmd[1].Equals("desc"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Description;
                                query = Properties.Settings.Default.DBSelectClanInfo;
                            }
                            else if (cmd[1].Equals("warlog"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.WarLog;
                                query = Properties.Settings.Default.DBSelectWarClan;
                            }
                            else if (cmd[1].Equals("warlast"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.WarLast;
                                query = CreateString.Build(
                                    new string[] {
                                        Properties.Settings.Default.DBSelectWarClan,
                                        Properties.Settings.Default.DBSelectWarLast
                                    }, " ",
                                LogError);
                            }
                            else if (cmd[1].Equals("list"))
                            {
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.List;
                                query = CreateString.Build(
                                    new string[] {
                                    string.Format(
                                        Properties.Settings.Default.DBSelectMember, 1
                                    ),
                                    CoCSeason.GetSeasonDateDB(idx, cmd),
                                    CoCDB._GetHideTagSelect(hide),
                                    Properties.Settings.Default.DBSelectMemberListOrder
                                }, " ",
                                LogError);
                            }
                            else if (cmd[1].Equals("player"))
                            {
                                if (cmd.Length == 2)
                                {
                                    throw new CoCDBException(
                                        stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtClanPlayerHelp,
                                        string.Format(
                                            Properties.Resources.CoCDBExceptionLength,
                                            "QueryString->player"
                                        )
                                    );
                                }
                                idx++;
                                cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Player;
                                playerId = ((cmd[2].StartsWith("#")) ? cmd[2].Substring(1) : cmd[2]);
                                query = CreateString.Build(
                                    new string[] {
                                    string.Format(
                                        Properties.Settings.Default.DBSelectMember, 1
                                    ),
                                    CoCSeason.GetSeasonDateDB(idx, cmd),
                                    // add
                                    string.Format(
                                        Properties.Settings.Default.DBSelectMemberTag,
                                        playerId
                                    )
                                }, " ",
                                LogError);
                            }
                            else if (cmd[1].Equals("donation"))
                            {
                                if (cmd.Length > 2)
                                {
                                    if (cmd[2].Equals("send"))
                                    {
                                        cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationSend;
                                        query = CreateString.Build(
                                            new string[] {
                                            Properties.Settings.Default.DBSelectDonation,
                                            CoCSeason.GetSeasonDateDB(idx, cmd),
                                            CoCDB._GetHideTagSelect(hide),
                                            Properties.Settings.Default.DBSelectDonationSendOrder
                                        }, " ",
                                        LogError);
                                        idx++;
                                    }
                                    else if (cmd[2].Equals("receive"))
                                    {
                                        cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationReceive;
                                        query = CreateString.Build(
                                            new string[] {
                                            Properties.Settings.Default.DBSelectDonation,
                                            CoCSeason.GetSeasonDateDB(idx, cmd),
                                            CoCDB._GetHideTagSelect(hide),
                                            Properties.Settings.Default.DBSelectDonationReceiveOrder
                                        }, " ",
                                        LogError);
                                        idx++;
                                    }
                                    else if ((cmd[2].Equals("ratio")) || (cmd[2].Equals("rate")))
                                    {
                                        cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationRatio;
                                        query = CreateString.Build(
                                            new string[] {
                                            Properties.Settings.Default.DBSelectDonation,
                                            Properties.Settings.Default.DBSelectDonationRatio,
                                            CoCSeason.GetSeasonDateDB(idx, cmd),
                                            CoCDB._GetHideTagSelect(hide),
                                            Properties.Settings.Default.DBSelectDonationRatioOrder
                                        }, " ",
                                        LogError);
                                        idx++;
                                    }
                                    else if (cmd[2].Equals("total"))
                                    {
                                        cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationTotal;
                                        query = CreateString.Build(
                                            new string[] {
                                            Properties.Settings.Default.DBSelectDonationTotal,
                                            CoCSeason.GetSeasonDateDB(idx, cmd)
                                        }, " ",
                                        LogError);
                                        idx++;
                                    }
                                }
                                else
                                {
                                    cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationSend;
                                    throw new CoCDBException(
                                        stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtDonation,
                                        CoCDB.thisClass
                                    );
                                }
                            }
                            else
                            {
                                return null;
                            }
                            return query;
                        }
                }
            }

            #endregion
        }

        ///<summary>
        /// Get League string, see enum LeagueTypeReq
        ///</summary>
        [Obsolete("Use CoCMediaSet, CoCMediaSetInfo")]
        public string GetLeague(stSqlite.Wrapper dbMgr, string leagueId, stCoCAPI.CoCAPI.CoCEnum.LeagueTypeReq type)
        {
                return CoCAPI.CoCDB._GetLeague(this.dbMgr, leagueId, type);
        }

        /// <summary>
        /// Get Sqlite DB Query string
        /// </summary>
        /// <param name="cmd">array Query string</param>
        /// <param name="cReq">enum ClanTypeReq</param>
        /// <param name="LogError">Log Error <code>Action{string}</code></param>
        /// <returns>DB query syting</returns>
        public string GetQueryString(
                string[] cmd,
                ref stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq cReq,
                StringCollection hide,
                Action<string> LogError
            )
        {
                return CoCAPI.CoCDB._GetQueryString(cmd, ref cReq, hide, LogError);
        }
    }
}
