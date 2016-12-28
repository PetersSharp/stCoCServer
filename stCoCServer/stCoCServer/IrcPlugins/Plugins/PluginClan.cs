using System;
using System.Collections.Generic;
using System.Data;

using stCore;
using stNet;
using stCoCAPI;
using stCoCServer.CoCAPI;
using stSqlite;
using System.Threading;
using System.Threading.Tasks;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        /// <summary>
        /// Default No IRC flood exceed error timeout 750ms
        /// </summary>

        #region CMD Clash of Clan

        private void PluginClan(bool isPrivate, string channel, string nik, string [] cmd, string cmds)
        {
            if (this.Conf.Api == null)
            {
                this._SendNotice(nik, (string)Properties.Resources.ResourceManager.GetString("cmdNoSQL", this._ci));
                return;
            }

            isPrivate = true;
            string query = null;
            List<string> txtout = new List<string>();
            stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq cReq = stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None;

            try
            {
                query = this.Conf.Api.GetQueryString(cmd, ref cReq, this.Conf.Opt.SQLDBFilterMemberTag.collection, this.Conf.ILog.LogError);

#if DEBUG_PluginClanQuery
                stConsole.WriteHeader("Plugin Clan Query: " + query);
#endif

                if (string.IsNullOrWhiteSpace(query))
                {
                    throw new ArgumentNullException((string)Properties.Resources.ResourceManager.GetString("cmdIrcQueryEmpty", this._ci));
                }
            }
            catch (CoCDBException e)
            {
                string msg;

                switch(e.enumId)
                {
                    case stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtNone:
                        {
                            msg = ((string.IsNullOrWhiteSpace(e.Message)) ? null : e.Message);
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtClanHelp:
                        {
                            msg = (string)Properties.Resources.ResourceManager.GetString("fmtClanHelp", this._ci);
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtClanPlayerHelp:
                        {
                            msg = (string)Properties.Resources.ResourceManager.GetString("fmtClanPlayerHelp", this._ci);
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtWarLog:
                        {
                            msg = (string)Properties.Resources.ResourceManager.GetString("fmtWarLogHelp", this._ci);
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtWarLast:
                        {
                            msg = (string)Properties.Resources.ResourceManager.GetString("fmtWarLastHelp", this._ci);
                            break;
                        }
                    case stCoCAPI.CoCAPI.CoCEnum.ClanFmtReq.fmtDonation:
                        {
                            msg = (string)Properties.Resources.ResourceManager.GetString("fmtClanDonationHelp", this._ci);
                            break;
                        }
                    default:
                        {
                            msg = null;
                            break;
                        }
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    this._SendNotice(nik, msg);
                    return;
                }
            }
            catch (Exception e)
            {
                this._SendNotice(nik, e.Message);
                return;
            }
            if (
                (cReq == stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None) ||
                (string.IsNullOrWhiteSpace(query))
               )
            {
                this._SendNotice(nik,
                    ((cReq == stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None) ?
                        (string)Properties.Resources.ResourceManager.GetString("cmdQueryWrong", this._ci) :
                        (string)Properties.Resources.ResourceManager.GetString("cmdQueryError", this._ci)
                    )
                );
                return;
            }

            DataTable dTable = null;

            try
            {
                dTable = this.Conf.Api.QueryData(query);

#if DEBUG_DataTablePrint
                dTable.DataTableToPrint();
#endif
            }
            catch (Exception e)
            {
                if (dTable != null) { dTable.Dispose(); }
                this._SendNotice(nik, e.Message);
                return;
            }
            if ((dTable == null) || (dTable.Rows.Count == 0))
            {
                this._SendNotice(nik, (string)Properties.Resources.ResourceManager.GetString("cmdQueryEmpty", this._ci));
                return;
            }
            switch (cReq)
            {
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.List:
                    {
                        txtout.Add(
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtMembersTotal", this._ci),
                                dTable.Rows.Count.ToString()
                            ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkViolet)
                        );
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationSend:
                    {
                        txtout.Add(
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtClanDonation", this._ci),
                                dTable.Rows.Count.ToString(),
                                "sender"
                            ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkViolet)
                        );
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationReceive:
                    {
                        txtout.Add(
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtClanDonation", this._ci),
                                dTable.Rows.Count.ToString(),
                                "reciver"
                            ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkViolet)
                        );
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationRatio:
                    {
                        txtout.Add(
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtClanDonation", this._ci),
                                dTable.Rows.Count.ToString(),
                                "ratio"
                            ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkViolet)
                        );
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationTotal:
                    {
                        txtout.Add(
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtClanDonation", this._ci),
                                dTable.Rows.Count.ToString(),
                                "total"
                            ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkViolet)
                        );
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.WarLog:
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.WarLast:
                    {
                        txtout.Add(
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtClanWar", this._ci),
                                dTable.Rows.Count.ToString()
                            ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkViolet)
                        );
                        break;
                    }
            }
            switch (cReq)
            {
                default:
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.None: 
                    {
                        dTable.Dispose();
                        this._SendNotice(nik, (string)Properties.Resources.ResourceManager.GetString("cmdQueryWrong", this._ci));
                        return;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Info:
                    {
                        foreach (DataRow row in dTable.Rows)
                        {
                          txtout.Add(
                            CreateString.Build(
                                new string[] {
                                    " Clan: ", (string)row["name"],
                                    " / Level: ", ((Int64)row["level"]).ToString(),
                                    " / Country: ", (string)row["locname"],
                                    " | Tag: ", (string)row["tag"],
                                    " | Members: ", ((Int64)row["members"]).ToString(), "/50 "
                                },
                                null,
                                this.Conf.ILog.LogError
                            ).ColorText(IrcFormatText.Color.White,IrcFormatText.Color.DarkGray)
                          );
                        }
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.War:
                    {
                        foreach (DataRow row in dTable.Rows)
                        {
                            txtout.Add(
                              CreateString.Build(
                                  new string[] {
                                    " Clan: ", (string)row["name"],
                                    " | War freq: ", (string)row["warfreq"],
                                    " | War win: ", ((Int64)row["warwin"]).ToString(),
                                    " | War strenght: ", ((Int64)row["warstr"]).ToString(),
                                    " | War public: ", 
                                        ((
                                            ((Int64)row["warpub"]).Equals(1)
                                         ) ?
                                            (string)Properties.Resources.ResourceManager.GetString("prnYes", this._ci) :
                                            (string)Properties.Resources.ResourceManager.GetString("prnNo", this._ci)
                                        )
                                },
                                null,
                                this.Conf.ILog.LogError
                              ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkGray)
                            );
                        }
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.WarLog:
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.WarLast:
                    {
                        foreach (DataRow row in dTable.Rows)
                        {
                            txtout.Add(
                              CreateString.Build(
                                  new string[] {
                                    " ", ((DateTime)row["dtend"]).ToString(),
                                    " : ", (string)row["result"],
                                    " | \"", (string)row["cname"],
                                    "\" (", ((Int64)row["clevel"]).ToString(),
                                    ") vs \"", (string)row["opname"],
                                    "\" (", ((Int64)row["oplevel"]).ToString(),
                                    ") <", ((Int64)row["members"]).ToString(),
                                    "> | attacks: ",((Int64)row["cattacks"]).ToString(),"/",((Int64)row["opattacks"]).ToString(),
                                    " | stars: ",((Int64)row["cstars"]).ToString(),"/",((Int64)row["opstars"]).ToString(),
                                    " | destruct: ",((System.Single)row["cdestruct"]).ToString(),"/",((System.Single)row["opdestruct"]).ToString()
                                },
                                null,
                                this.Conf.ILog.LogError
                              ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkGray)
                            );
                        }
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Statistic:
                    {
                        foreach (DataRow row in dTable.Rows)
                        {
                            txtout.Add(
                              CreateString.Build(
                                  new string[] {
                                    " Clan: ", (string)row["name"],
                                    " | Type: ", (string)row["type"],
                                    " | Points: ", ((Int64)row["points"]).ToString(),
                                    " | Needed trophies: ", ((Int64)row["trophies"]).ToString()
                                },
                                null,
                                this.Conf.ILog.LogError
                              ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkGray)
                            );
                        }
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Description:
                    {
                        foreach (DataRow row in dTable.Rows)
                        {
                            txtout.Add(
                              CreateString.Build(
                                  new string[] {
                                    " Clan: ", (string)row["name"],
                                    " | Description: \"", (string)row["desc"], "\""
                                },
                                null,
                                this.Conf.ILog.LogError
                              ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkBlue)
                            );
                        }
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.List:
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.Player:
                    {
                        int i = 1;
                        txtout.Add(
                            ((string)Properties.Resources.ResourceManager.GetString("fmtAnswerList", this._ci)).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Orange)
                        );
                        foreach (DataRow row in dTable.Rows)
                        {
                            txtout.Add(
                              CreateString.Build(
                                  new string[] {
                                    ((i < 10) ? "  " : "") + i.ToString(), " | ",
                                    CreateString.Build(
                                        new string[] {
                                            " ",
                                            (string)row["nik"], " | ",
                                            (string)row["tag"], " | ",
                                            ((Int64)row["level"]).ToString(), " | ",
                                            ((Int64)row["rank"]).ToString(), "/", ((Int64)row["prank"]).ToString(), " | ",
                                            ((Int64)row["trophies"]).ToString(), " | '",
                                            (string)row["league"], "' | '",
                                            (string)row["role"], "' | ",
                                            ((Int64)row["send"]).ToString(), " | ",
                                            ((Int64)row["receive"]).ToString(), " | ",
                                            ((System.Single)row["ratio"]).ToString(), " | "
                                        }
                                    ).ColorText(IrcFormatText.Color.Black, IrcFormatText.Color.LightGray)
                                },
                                null,
                                this.Conf.ILog.LogError
                              )
                            );
                            i++;
                        }
#if DEBUG_ListPrint
                        foreach (string str in txtout)
                        {
                            stConsole.WriteLine(str);
                        }
#endif
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationSend:
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationReceive:
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationRatio:
                    {
                        int i = 1;
                        foreach (DataRow row in dTable.Rows)
                        {
                            txtout.Add(
                              CreateString.Build(
                                  new string[] {
                                    i.ToString(), " | ",
                                    CreateString.Build(
                                        new string[] {
                                            " ",
                                            (string)row["nik"], " | '",
                                            (string)row["role"], "' | ",
                                            ((Int64)row["send"]).ToString(), " | ",
                                            ((Int64)row["receive"]).ToString(), " | ",
                                            ((System.Single)row["ratio"]).ToString(), " | "
                                        }
                                    ).ColorText(IrcFormatText.Color.Black, IrcFormatText.Color.LightGray)
                                },
                                null,
                                this.Conf.ILog.LogError
                              )
                            );
                            i++;
                        }
                        break;
                    }
                case stCoCAPI.CoCAPI.CoCEnum.ClanTypeReq.DonationTotal:
                    {
                        foreach (DataRow row in dTable.Rows)
                        {
                            txtout.Add(
                              CreateString.Build(
                                  new string[] {
                                    "   Total send in Clan: ", ((Int64)row["tsend"]).ToString(),
                                    " | Total recive in Clan: ", ((Int64)row["treceive"]).ToString(),
                                    " | Total send  out Clan: ", ((Int64)row["tsend"] - (Int64)row["treceive"]).ToString(),
                                    "   "
                                },
                                null,
                                this.Conf.ILog.LogError
                              ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkBlue)
                            );
                        }
                        break;
                    }
            }

            dTable.Dispose();

            if (txtout.Count == 0)
            {
                this._SendNotice(nik, (string)Properties.Resources.ResourceManager.GetString("cmdQueryEmpty", this._ci));
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    AutoResetEvent are = new AutoResetEvent(false);
                    foreach (string str in txtout)
                    {
                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            this._SendFromTask(isPrivate, channel, nik, str);
                            are.WaitOne(TimeSpan.FromMilliseconds(this.Conf.Opt.IRCFloodTimeOut.num), true);
                        }
                    }
                    are.Close();
                    are.Dispose();
                });
            }
        }

        #endregion
    }
}
