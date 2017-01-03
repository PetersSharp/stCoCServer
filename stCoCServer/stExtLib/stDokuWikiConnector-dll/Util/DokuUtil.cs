using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

using stDokuWiki;
using stDokuWiki.WikiEngine;
using stDokuWiki.WikiEngine.Exceptions;

namespace stDokuWiki.Util
{
    /// <summary>
    /// Internal utilites Class
    /// </summary>
    public static class DokuUtil
    {
        #region Get/Set Unix TimeStamp

        /// <summary>
        /// Convert DateTime to Unix TimeStamp
        /// </summary>
        /// <param name="time">Time and Date in DateTime format</param>
        /// <returns><see cref="System.DateTime"/>DateTime format</returns>
        public static Int32 GetUnixTimeStamp(this DateTime time)
        {
            return Convert.ToInt32((time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
        }

        /// <summary>
        /// Convert String date and time to Unix TimeStamp
        /// </summary>
        /// <param name="stime">Time and Date in string format</param>
        /// <returns>Int32 format</returns>
        public static Int32 GetUnixTimeStamp(this string stime)
        {
            int pos;
            if ((pos = stime.IndexOf("+", 0, stime.Length)) > 0)
            {
                stime = stime.Substring(0, (stime.Length - (stime.Length - pos)));
            }
            return GetUnixTimeStamp(
                DateTime.Parse(stime, null, System.Globalization.DateTimeStyles.RoundtripKind)
            );
        }

        /// <summary>
        /// Convert Unix TimeStamp to DateTime
        /// </summary>
        /// <param name="timestamp">Unix timestamp in int format</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTimeFromUnixTimeStamp(this Int32 timestamp)
        {
            return (DateTime)(new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(timestamp);
        }

        /// <summary>
        /// Convert Unix TimeStamp string to DateTime
        /// </summary>
        /// <param name="stime">Unix timestamp in string format</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTimeFromUnixTimeStampString(this string stime)
        {
            Int32 timestamp = 0;
            if (Int32.TryParse(stime, out timestamp))
            {
                return (DateTime)(new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(timestamp);
            }
            return DateTime.MinValue;
        }

        #endregion

        #region (internal) Wiki FS AtticCompressFile / AtticDeCompressFile
        /// <summary>
        /// Wiki attic history change create page backup
        /// </summary>
        /// <param name="ppath">input page path</param>
        /// <param name="apath">output attic path</param>
        internal static void AtticCompressFile(string ppath, string apath)
        {
            try
            {
                byte [] src = File.ReadAllBytes(ppath);
                using (FileStream fs = new FileStream(apath, FileMode.CreateNew))
                {
                    using (GZipStream zs = new GZipStream(fs, CompressionMode.Compress, false))
                    {
                        zs.Write(src, 0, src.Length);
                        zs.Flush();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Wiki attic history change read page backup
        /// </summary>
        /// <param name="apath">input attic path</param>
        /// <returns>decompressed body as byte []</returns>
        internal static byte[] AtticDeCompressFile(string apath)
        {
            try
            {
                byte[] src = File.ReadAllBytes(apath);

                using (MemoryStream msi = new MemoryStream(src))
                {
                    using (GZipStream zs = new GZipStream(msi, CompressionMode.Decompress, false))
                    {
                        using (MemoryStream mso = new MemoryStream())
                        {
                            zs.CopyTo(mso);
                            return mso.ToArray();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region (internal) Transform Wiki Types

        #region internal (string) WikiFileNameSpaceNormalize
        internal static string WikiFileNameSpaceNormalize(string namesspace)
        {
            const string sep = ":";
            return ((string.IsNullOrWhiteSpace(namesspace)) ? sep :
                ((namesspace.Contains(sep)) ?
                    ((namesspace.EndsWith(sep)) ? namesspace : string.Concat(namesspace, sep)) : sep
                )
            );
        }
        #endregion

        #region internal (string) WikiFileTypeToString
        /// <summary>
        /// map string to type, enum WikiEngine.WikiFileType
        /// </summary>
        /// <param name="type"><see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</param>
        /// <returns>String</returns>
        [DebuggerHidden]
        internal static string WikiFileTypeToString(WikiFileType type)
        {
            switch (type)
            {
                case WikiFileType.FileReadMd:
                case WikiFileType.FileWriteMd:
                    {
                        return WikiFile.wikiLocalPage;
                    }
                case WikiFileType.FileReadAttic:
                case WikiFileType.FileWriteAttic:
                    {
                        return WikiFile.wikiLocalAttic;
                    }
                case WikiFileType.FileReadBinary:
                case WikiFileType.FileWriteBinary:
                    {
                        return WikiFile.wikiLocalMedia;
                    }
                case WikiFileType.FileReadMeta:
                case WikiFileType.FileWriteMeta:
                    {
                        return WikiFile.wikiLocalMeta;
                    }
                case WikiFileType.None:
                case WikiFileType.FileUnknown:
                    {
                        throw new WikiEngineInternalFileTypeException("detect empty type: " + type.ToString());
                    }
                default:
                    {
                        throw new WikiEngineInternalFileTypeException("not detect type from WikiFileType");
                    }
            }
        }
        #endregion

        #region internal (WikiFileType) WikiFileStringToType
        /// <summary>
        /// map string to type, enum WikiEngine.WikiFileType
        /// </summary>
        /// <param name="type"><see cref="WikiEngine.WikiRequestType"/>WikiEngine.WikiRequestType</param>
        /// <param name="src">String: pages, media, attic, meta</param>
        /// <returns><see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</returns>
        [DebuggerHidden]
        internal static WikiFileType WikiFileStringToType(WikiRequestType type, string src)
        {
            switch (src)
            {
                case WikiEngine.WikiFile.wikiLocalPage:
                    {
                        return (((type == WikiRequestType.Put) || (type == WikiRequestType.Del)) ?
                            WikiFileType.FileWriteMd :
                            WikiFileType.FileReadMd
                        );
                    }
                case WikiEngine.WikiFile.wikiLocalMedia:
                    {
                        return (((type == WikiRequestType.Put) || (type == WikiRequestType.Del)) ?
                            WikiFileType.FileWriteBinary :
                            WikiFileType.FileReadBinary
                        );
                    }
                case WikiEngine.WikiFile.wikiLocalAttic:
                    {
                        return (((type == WikiRequestType.Put) || (type == WikiRequestType.Del)) ?
                            WikiFileType.FileWriteAttic :
                            WikiFileType.FileReadAttic
                        );
                    }
                case WikiEngine.WikiFile.wikiLocalMeta:
                    {
                        return (((type == WikiRequestType.Put) || (type == WikiRequestType.Del)) ?
                            WikiFileType.FileWriteMeta :
                            WikiFileType.FileReadMeta
                        );
                    }
                default:
                    {
                        return WikiFileType.None;
                    }
            }
        }
        #endregion

        #region internal (WikiFileType) WikiFileStringToDefaultType
        /// <summary>
        /// map string full Namespace to type, enum WikiEngine.WikiFileType
        /// </summary>
        /// <param name="src">String: pages, media, attic, meta</param>
        /// <param name="rw">Bolean, read = false, write = true</param>
        /// <returns><see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</returns>
        [DebuggerHidden]
        internal static WikiFileType WikiFileStringToDefaultType(string src, bool rw = false)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return WikiFileType.None;
            }
            if (src.EndsWith(":"))
            {
                return WikiFileType.NameSpace;
            }
            if (!string.IsNullOrWhiteSpace(Path.GetExtension(src)))
            {
                return ((rw) ? WikiFileType.FileWriteBinary : WikiFileType.FileReadBinary);
            }
            return ((rw) ? WikiFileType.FileWriteMd : WikiFileType.FileReadMd);
        }
        #endregion

        #region internal (WikiRequestType) WikiFileStringToMethod
        /// <summary>
        /// map string to action, enum WikiRequestType
        /// </summary>
        /// <param name="src">String: get, put, del, list, find</param>
        /// <returns><see cref="WikiEngine.WikiRequestType"/>WikiEngine.WikiRequestType</returns>
        [DebuggerHidden]
        internal static WikiRequestType WikiFileStringToMethod(string src)
        {
            switch (src)
            {
                case "get":
                    {
                        return WikiRequestType.Get;
                    }
                case "put":
                    {
                        return WikiRequestType.Put;
                    }
                case "del":
                    {
                        return WikiRequestType.Del;
                    }
                case "list":
                    {
                        return WikiRequestType.List;
                    }
                case "find":
                    {
                        return WikiRequestType.Find;
                    }
                default:
                    {
                        return WikiRequestType.None;
                    }
            }
        }
        #endregion

        #region internal (string) WikiFileExtToString
        /// <summary>
        /// normalize file extension string
        /// </summary>
        /// <param name="src">String file extension</param>
        /// <returns>String</returns>
        [DebuggerHidden]
        internal static string WikiFileExtToString(string src)
        {
            return Path.GetExtension(src).Replace(".", "");
        }
        #endregion

        #region internal (string) WikiFileMetaDataMerge
        /// <summary>
        /// Merge meta data to WikiEngine.WikiMetaChanges
        /// </summary>
        /// <param name="wfm">WikiEngine.WikiFileMeta</param>
        /// <param name="isCreate">Bolean create or update file</param>
        /// <returns><see cref="WikiEngine.WikiMetaChanges"/>WikiEngine.WikiMetaChanges</returns>
        internal static WikiMetaChanges WikiFileMetaDataMerge(WikiFileMeta wfm, bool isCreate)
        {
            bool isPage = ((wfm.Data.FileExt.Equals(WikiFile.mdExtension)) ? true : false);
            WikiMetaChanges wmc = new WikiMetaChanges();
            wmc.DateTimeStamp = DateTime.Now;
            wmc.Author = wfm.Author;
            wmc.AuthorIp = wfm.AuthorIp;
            wmc.Mode = ((isCreate) ? "C" : "E");
            wmc.Size = wfm.Data.FileContent.Length;
            wmc.NameSpace = ((wfm.Data.NameSpace.EndsWith(wfm.Data.FileName)) ?
                                wfm.Data.NameSpace :
                                string.Concat(wfm.Data.NameSpace, wfm.Data.FileName));
            wmc.Title = ((isPage) ? Encoding.UTF8.GetString(wfm.Data.FileContent)
                .Substring(0, ((wfm.Data.FileContent.Length > 20) ? 20 : wfm.Data.FileContent.Length))
                .Replace("#", "")
                .Replace(">", "")
                .Replace("<", "")
                .Replace("*", "")
                .Trim() :
                wfm.Data.FileName + "." + wfm.Data.FileExt);
            return wmc;
        }
        #endregion

        #region internal (string) WikiFilePathRewrite
        /// <summary>
        /// Rewrite file path, change type
        /// </summary>
        /// <param name="wft">WikiFileType</param>
        /// <param name="path">file path</param>
        /// <param name="replace">replace string</param>
        /// <param name="append">append string to end</param>
        /// <returns></returns>
        internal static string WikiFilePathRewrite(WikiFileType wft, string path, string replace, string append)
        {
            if (
                (string.IsNullOrWhiteSpace(path)) ||
                (wft == WikiFileType.FileUnknown) ||
                (wft == WikiFileType.NameSpace) ||
                (wft == WikiFileType.None)
               )
            {
                return String.Empty;
            }
            return string.Format(
                "{0}{1}",
                path.Replace(
                    WikiFile.wikiDefaultSeparator +
                        DokuUtil.WikiFileTypeToString(wft) +
                            WikiFile.wikiDefaultSeparator,
                    WikiFile.wikiDefaultSeparator +
                        replace +
                            WikiFile.wikiDefaultSeparator
                ),
                ((string.IsNullOrWhiteSpace(append)) ? "" : append)
            );
        }
        #endregion

        #endregion
    }
}
