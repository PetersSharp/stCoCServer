using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        private class CoCMediaSetInfo
        {
            public int[] msize { get; set; }
            public string[] mpath { get; set; }
            public string mdir { get; set; }
        }
        private static class CoCMediaSet
        {
            private static readonly int[] sizeLigue = new int[] { 36, 72, 88 };
            private static readonly int[] sizeBadge = new int[] { 70, 200, 512 };
            private static readonly int[] sizeFlags = new int[] { 16, 16, 64 };
            private static readonly string[] mediaPath = new string[] { "assets", "images" };

            public static CoCMediaSetInfo GetMediaSet(CoCEnum.CoCFmtReq mediaid)
            {
                switch (mediaid)
                {
                    case CoCEnum.CoCFmtReq.Leagues:
                        {
                            return new CoCMediaSetInfo()
                            {
                                msize = CoCMediaSet.sizeLigue,
                                mpath = CoCMediaSet.mediaPath,
                                mdir = CoCEnum.CoCFmtReq.Leagues.ToString().ToLowerInvariant()
                            };
                        }
                    case CoCEnum.CoCFmtReq.Clan:
                    case CoCEnum.CoCFmtReq.Warlog:
                    case CoCEnum.CoCFmtReq.Badges:
                        {
                            return new CoCMediaSetInfo()
                            {
                                msize = CoCMediaSet.sizeBadge,
                                mpath = CoCMediaSet.mediaPath,
                                mdir = CoCEnum.CoCFmtReq.Badges.ToString().ToLowerInvariant()
                            };
                        }
                    case CoCEnum.CoCFmtReq.Flags:
                        {
                            return new CoCMediaSetInfo()
                            {
                                msize = CoCMediaSet.sizeFlags,
                                mpath = CoCMediaSet.mediaPath,
                                mdir = CoCEnum.CoCFmtReq.Flags.ToString().ToLowerInvariant()
                            };
                        }
                    default:
                        {
                            return null;
                        }
                }
            }
            public static string _GetMediaUrl(CoCEnum.CoCFmtReq type, CoCEnum.CoCBadgeType size, string imgId)
            {
                CoCMediaSetInfo cms = null;
                if ((cms = CoCMediaSet.GetMediaSet(type)) == null)
                {
                    return null;
                }
                return string.Format(
                    Properties.Settings.Default.CoCLocalMediaURL,
                    cms.mpath[0],
                    cms.mpath[1],
                    cms.mdir,
                    cms.msize[(int)size].ToString(),
                    ((string.IsNullOrWhiteSpace(imgId)) ? "" : imgId + ".png")
                );
            }
            public static string _CheckMediaFlag(string imgId, bool isRealCountry)
            {
                if ((!isRealCountry) || (string.IsNullOrWhiteSpace(imgId)) || (imgId.Contains("-")))
                {
                    return @"DEFAULT";
                }
                return imgId;
            }
        }
        public static string GetLeagueUrl(CoCEnum.CoCBadgeType tsize, string imgId = null)
        {
            return CoCMediaSet._GetMediaUrl(CoCEnum.CoCFmtReq.Leagues, tsize, imgId);
        }
        public static string GetLeagueUrl(string imgId)
        {
            return CoCMediaSet._GetMediaUrl(CoCEnum.CoCFmtReq.Leagues, CoCEnum.CoCBadgeType.Small, imgId);
        }
        public static string GetBadgeUrl(CoCEnum.CoCBadgeType tsize, string imgId = null)
        {
            return CoCMediaSet._GetMediaUrl(CoCEnum.CoCFmtReq.Badges, tsize, imgId);
        }
        public static string GetBadgeUrl(string imgId)
        {
            return CoCMediaSet._GetMediaUrl(CoCEnum.CoCFmtReq.Badges, CoCEnum.CoCBadgeType.Small, imgId);
        }
        public static string GetFlagUrl(CoCEnum.CoCBadgeType tsize, string imgId = null)
        {
            return CoCMediaSet._GetMediaUrl(CoCEnum.CoCFmtReq.Flags, tsize, imgId);
        }
        public static string GetFlagUrl(string imgId, bool isRealCountry)
        {
            return CoCMediaSet._GetMediaUrl(CoCEnum.CoCFmtReq.Flags, CoCEnum.CoCBadgeType.Small, CoCMediaSet._CheckMediaFlag(imgId, isRealCountry));
        }
        public static string GetFlagUrl(string imgId)
        {
            return CoCMediaSet._GetMediaUrl(CoCEnum.CoCFmtReq.Flags, CoCEnum.CoCBadgeType.Small, CoCMediaSet._CheckMediaFlag(imgId, true));
        }
    }
}
