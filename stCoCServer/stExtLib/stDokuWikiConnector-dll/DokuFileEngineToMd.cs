using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.IO;

using stDokuWiki;
using stDokuWiki.Util;
using stDokuWiki.WikiEngine;
using stDokuWiki.WikiEngine.Exceptions;

namespace stDokuWiki.WikiEngine
{
    public partial class WikiFile
    {
        #region method PageToMdString
        /// <summary>
        /// ToString method from <see cref="WikiEngine.WikiData"/>WikiEngine.WikiData to MarkDown
        /// </summary>
        /// <param name="wd">Data in WikiEngine.WikiData format</param>
        /// <param name="isPageInfo">Bolean print Page information before</param>
        /// <param name="sb">StringBuilder instance or null</param>
        /// <returns>String or String.Empty</returns>
        public string PageToMdString(WikiData wd, bool isPageInfo = false, StringBuilder sb = null)
        {
            bool IsSBuilder = this._CheckStringBuilder(ref sb);
            return this._ToMd(
                (object)wd,
                (Action)(() => this._ActionPageSource(wd, sb, isPageInfo)),
                IsSBuilder,
                sb
            );
        }
        private void _ActionPageSource(WikiData wd, StringBuilder sb, bool isPageInfo)
        {
            if (
                (wd.FileContent == null) ||
                (wd.FileContent.Length == 0)
               )
            {
                sb.AppendFormat(
                    Properties.ResourceWikiEngine.PageToMdError2,
                    Environment.NewLine,
                    wd.NameSpace,
                    wd.FileName,
                    wd.FileExt
                );
                return;
            }
            if (
                (wd.FileType != WikiEngine.WikiFileType.FileReadMd) &&
                (wd.FileType != WikiEngine.WikiFileType.FileWriteMd)
               )
            {
                sb.AppendFormat(
                    Properties.ResourceWikiEngine.PageToMdError3,
                    Environment.NewLine,
                    wd.NameSpace,
                    wd.FileName,
                    wd.FileExt
                );
                return;
            }
            if (isPageInfo)
            {
                sb.AppendFormat(
                    Properties.ResourceWikiEngine.PageToMdError4,
                    Environment.NewLine,
                    wd.NameSpace,
                    wd.FileName,
                    wd.FileExt,
                    wd.FilePath,
                    Encoding.UTF8.GetString(wd.FileContent)
                );
            }
            else
            {
                sb.Append(Encoding.UTF8.GetString(wd.FileContent));
            }
        }
        #endregion

        #region method ResourceListToMdString
        /// <summary>
        /// ToString method from <see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo to MarkDown
        /// Page/Media/Attic List
        /// </summary>
        /// <param name="wfi"><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo input</param>
        /// <param name="sb">StringBuilder instance or null</param>
        /// <param name="type"><see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType, if this equals WikiFileType.FileReadAttic, return only date from attic</param>
        /// <returns>String or String.Empty</returns>
        public string ResourceListToMdString(WikiFolderInfo wfi, StringBuilder sb = null, WikiFileType type = WikiFileType.None)
        {
            Action act;
            WikiFileAction wfa = new WikiFileAction(sb);
            wfa.IsAllFiles = true;

            switch (type)
            {
                // Attic date and time list to Md string
                case WikiFileType.FileReadAttic:
                case WikiFileType.FileWriteAttic:
                    {
                        act = (Action)(() => this._WikiFileActionRecursive(wfi, wfa, this._ActionAtticDateList));
                        break;
                    }
                default:
                    {
                        act = (Action)(() => this._WikiFileActionRecursive(wfi, wfa, this._ActionResourceList));
                        break;
                    }
            }
            string sOut = this._ToMd(
                (object)new Object(),
                act,
                wfa.IsSBuilder,
                wfa.SBuilder
            );
            return string.Format(
                Properties.ResourceWikiEngine.ListFormatPageStatistic,
                sOut,
                ((string.IsNullOrWhiteSpace(wfi.SearchPatern)) ? wfi.NameSpace : wfi.SearchPatern),
                wfa.ConutDirs,
                wfa.ConutFiles,
                Environment.NewLine
            );
        }
        internal void _ActionResourceList(WikiFileAction wfa)
        {
            if (
                (wfa == null) ||
                (wfa.FileInfo == null)
               )
            {
                return;
            }
            bool isNewNameSpace = ((string.IsNullOrWhiteSpace(wfa.LastNameSpace)) ? true :
                ((wfa.LastNameSpace.Equals(wfa.NameSpace)) ? false : true)
            );
            if (isNewNameSpace)
            {
                wfa.LastNameSpace = wfa.NameSpace;
                wfa.SBuilder.AppendFormat(
                    Properties.ResourceWikiEngine.ListFormatNameSpace,
                    WikiFile.wikiLocalPath,
                    DokuUtil.WikiFileTypeToString(wfa.FileInfo.FileType),
                    wfa.NameSpace,
                    ((string.IsNullOrWhiteSpace(wfa.NameSpace)) ?
                        Properties.ResourceWikiEngine.txtHomePageTitle :
                        wfa.NameSpace
                    ),
                    Environment.NewLine
                );
            }
            switch (wfa.FileInfo.FileType)
            {
                case WikiFileType.FileReadAttic:
                case WikiFileType.FileWriteAttic:
                    {
                        string FileNameUri, FileNameTxt, FileTimeStamp;
                        FileNameUri = Path.GetFileNameWithoutExtension(wfa.FileInfo.FileName);
                        FileNameUri = Path.GetFileNameWithoutExtension(FileNameUri);
                        FileNameTxt = Path.GetFileNameWithoutExtension(FileNameUri);
                        FileTimeStamp = Path.GetExtension(FileNameUri);
                        DateTime TimeStamp = Util.DokuUtil.GetDateTimeFromUnixTimeStampString(
                            FileTimeStamp.Substring(1, (FileTimeStamp.Length - 1))
                        );
                        wfa.SBuilder.AppendFormat(
                            Properties.ResourceWikiEngine.ListFormatAttic,
                            WikiFile.wikiLocalPath,
                            WikiFile.wikiLocalAttic,
                            wfa.NameSpace,
                            FileNameUri,
                            FileNameTxt,
                            TimeStamp.ToString(),
                            Environment.NewLine
                        );
                        break;
                    }
                case WikiFileType.FileReadMd:
                case WikiFileType.FileWriteMd:
                    {
                        wfa.SBuilder.AppendFormat(
                            Properties.ResourceWikiEngine.ListFormatPage,
                            WikiFile.wikiLocalPath,
                            WikiFile.wikiLocalPage,
                            wfa.NameSpace,
                            wfa.FileInfo.FileName,
                            wfa.FileInfo.TimeWrite.ToShortDateString(),
                            Environment.NewLine
                        );
                        break;
                    }
                case WikiFileType.FileReadBinary:
                case WikiFileType.FileWriteBinary:
                    {
                        wfa.SBuilder.AppendFormat(
                            Properties.ResourceWikiEngine.ListFormatMedia,
                            WikiFile.wikiLocalPath,
                            WikiFile.wikiLocalMedia,
                            wfa.NameSpace,
                            wfa.FileInfo.FileName,
                            Path.GetFileNameWithoutExtension(wfa.FileInfo.FileName),
                            wfa.FileInfo.FileExt,
                            wfa.FileInfo.TimeWrite.ToShortDateString(),
                            Environment.NewLine
                        );
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
        }
        internal void _ActionAtticDateList(WikiFileAction wfa)
        {
            if (
                (wfa == null) ||
                (wfa.FileInfo == null)
               )
            {
                return;
            }
            switch (wfa.FileInfo.FileType)
            {
                case WikiFileType.FileReadAttic:
                case WikiFileType.FileWriteAttic:
                    {
                        string FileNameUri, FileTimeStamp;
                        FileNameUri = Path.GetFileNameWithoutExtension(wfa.FileInfo.FileName);
                        FileNameUri = Path.GetFileNameWithoutExtension(FileNameUri);
                        FileTimeStamp = Path.GetExtension(FileNameUri);
                        DateTime TimeStamp = Util.DokuUtil.GetDateTimeFromUnixTimeStampString(
                            FileTimeStamp.Substring(1, (FileTimeStamp.Length - 1))
                        );
                        wfa.SBuilder.AppendFormat(
                            Properties.ResourceWikiEngine.ListFormatAtticDate,
                            WikiFile.wikiLocalPath,
                            WikiFile.wikiLocalAttic,
                            wfa.NameSpace,
                            FileNameUri,
                            TimeStamp.ToString()
                        );
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
        }
        #endregion

        #region method MetaListToMdString
        /// <summary>
        /// ToString method from List&lt;WikiMetaChanges&gt; to MarkDown
        /// </summary>
        /// <param name="lwmc">Data in List&lt;WikiFileAttic&gt; format</param>
        /// <param name="sb">StringBuilder instance or null</param>
        /// <returns>String or String.Empty</returns>
        public string MetaListToMdString(List<WikiMetaChanges> lwmc, StringBuilder sb = null)
        {
            bool IsSBuilder = this._CheckStringBuilder(ref sb);
            return this._ToMd(
                (object)lwmc,
                (Action)(() => this._ActionMetaList(lwmc, sb)),
                IsSBuilder,
                sb
            );
        }
        private void _ActionMetaList(List<WikiMetaChanges> lwmc, StringBuilder sb)
        {
            foreach (var item in lwmc)
            {
                sb.AppendFormat(
                    Properties.ResourceWikiEngine.ListFormatMetaChanges,
                    item.NameSpace,
                    item.Author,
                    item.Size,
                    ((string.IsNullOrWhiteSpace(item.Title)) ? " " : item.Title),
                    item.DateTimeStamp.ToString("d", CultureInfo.InvariantCulture),
                    Environment.NewLine
                );
            }
        }
        #endregion

        #region private all method

        internal bool _CheckStringBuilder(ref StringBuilder sb)
        {
            bool IsSBuilder = ((sb == null) ? false : true);
            sb = ((IsSBuilder) ? sb : new StringBuilder());
            return IsSBuilder;
        }
        internal string _ToMd(object data, Action act, bool IsSBuilder, StringBuilder sb)
        {
            try
            {
                if (data == null)
                {
                    sb.AppendFormat(
                        Properties.ResourceWikiEngine.PageToMdError1,
                        Environment.NewLine
                    );
                }
                else
                {
                    act();
                }
                return ((IsSBuilder) ? String.Empty : sb.ToString());
            }
            catch (Exception e)
            {
                sb.AppendFormat(
                    Properties.ResourceWikiEngine.ToMdException,
                    e.GetType().Name,
                    e.Message
                );
                return ((IsSBuilder) ? String.Empty : sb.ToString());
            }
        }
        #endregion
    }
}
