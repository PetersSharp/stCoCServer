using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace stCoCAPI
{
    public partial class CoCAPI
    {
        ///<summary>
        ///Clash of Clan main selector
        ///</summary>
        public static class CoCEnum
        {
            #region CoCDB enums

            ///<summary>Clan type request</summary>
            public enum ClanTypeReq : int
            {
                ///<summary>Empty selection</summary>
                None,
                ///<summary>Information About Clan</summary>
                Info,
                ///<summary>war statistic Clan</summary>
                War,
                ///<summary>score statistic Clan</summary>
                Statistic,
                ///<summary>Clan Description</summary>
                Description,
                ///<summary>Last 3 Clan War statistic</summary>
                WarLast,
                ///<summary>Log all Clan War statistic</summary>
                WarLog,
                ///<summary>Clan members llist</summary>
                List,
                ///<summary>Clan member statistic</summary>
                Player,
                ///<summary>Clan Donation Send one month</summary>
                DonationSend,
                ///<summary>Clan Donation Receive one month</summary>
                DonationReceive,
                ///<summary>Clan Donation Ratio one month</summary>
                DonationRatio,
                ///<summary>Clan Donation Total one month</summary>
                DonationTotal,
                ///<summary>Get server setup, http SSE url, IRC server, IRC port, IRC Channel </summary>
                ServerSetup
            }

            ///<summary>League type request</summary>
            public enum LeagueTypeReq : int
            {
                ///<summary>GetLeague return Name</summary>
                Name,
                ///<summary>GetLeague return small image size URL</summary>
                UrlSmall,
                ///<summary>GetLeague return medium image size URL</summary>
                UrlMedium,
                ///<summary>GetLeague return mall tiny size URL</summary>
                UrlTiny
            }
            ///<summary>Query parse exception return</summary>
            public enum ClanFmtReq : int
            {
                ///<summary>return Help</summary>
                fmtNone,
                ///<summary>return All Help string</summary>
                fmtClanHelp,
                ///<summary>return Player Help string</summary>
                fmtClanPlayerHelp,
                ///<summary>return Wars Log Help string</summary>
                fmtWarLog,
                ///<summary>return Last Wars Help string</summary>
                fmtWarLast,
                ///<summary>return Clan Donation Help string</summary>
                fmtDonation
            }
            ///<summary>
            /// URL request exception
            /// or
            /// Download Media type
            /// </summary>
            public enum CoCFmtReq : int
            {
                None,
                Clan,
                Members,
                Leagues,
                Warlog,
                Locations,
                ClanRank,
                Badges,
                Flags
            };
            /// <summary>
            /// Clan Badge size type
            /// </summary>
            public enum CoCBadgeType : int
            {
                Small,
                Medium,
                Large
            };

            #endregion

            #region CoCNotify enums

            public enum EventNotify : int
            {
                None,
                Normal,
                NoKeepAlive,
                ServerError,
                ServerShutDown,
                All,
                EventSetup,
                TestAlive,
                MemberNew,
                MemberExit,
                MemberChangeNik,
                MemberChangeRole,
                MemberChangeLevel,
                MemberChangeLeague,
                MemberChangeTrophies,
                MemberChangeDonationSend,
                MemberChangeDonationReceive,
                ClanChangeName,
                ClanChangeType,
                ClanChangeLevel,
                ClanChangePoints,
                ClanChangeMembers,
                ClanChangeTrophies,
                ClanChangeWarWin,
                ClanChangeWarFrequency,
                ClanChangeWarSeries,
                ClanChangeWarPublic,
                WarClanEnd
            }

            #endregion

            #region CoCNotify RRD enums

            public enum RrdGrapPeriod : int
            {
                Day,
                Week,
                Month,
                Year
            }

            #endregion
        }
    }
}
