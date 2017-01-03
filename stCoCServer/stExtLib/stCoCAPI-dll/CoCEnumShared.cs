

#if STCLIENTBUILD
namespace stClient
{

#else

namespace stCoCAPI
{
    public partial class CoCAPI
    {
#endif
        ///<summary>
        ///Clash of Clan main selector
        ///</summary>
        public static partial class CoCEnum
        {
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

        }
#if !STCLIENTBUILD
    }
#endif
}
