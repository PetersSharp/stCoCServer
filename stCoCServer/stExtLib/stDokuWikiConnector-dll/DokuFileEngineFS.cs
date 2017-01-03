using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using stDokuWiki.Util;
using stDokuWiki.WikiEngine.Exceptions;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace stDokuWiki.WikiEngine
{
    public partial class WikiFile
    {
        private const string cacheReplace1 = "_x_";
        private const string cacheReplace2 = "_c_";
        private const string cacheExtension = "cache";
        private const int waitReadFsOnStart = 15000;
        private const int waitWriteFsOnStart = 10000;
        private const int waitWriteFsOnProcess = 50000;
        private int waitReadFsOnProcess = 1000;

        #region internal class WikiFileParse
        internal class WikiFileParse
        {
            /// <summary>
            /// enum WikiFileType
            /// </summary>
            public WikiFileType FolderType { get; set; }
            /// <summary>
            /// Namespace string array
            /// </summary>
            public List<string> UriPart { get; set; }
            /// <summary>
            /// Namespace path string
            /// </summary>
            public string NameSpacePatern
            {
                get
                {
                    return this._nameSpacePatern;
                }
                set
                {
                    this._nameSpacePatern = value;
                    this.IsNameSpaceValid = ((string.IsNullOrWhiteSpace(this._nameSpacePatern)) ?
                        false :
                        this._nameSpacePatern.Contains(":")
                    );
                    this.IsNameSpaceOnly = ((this.IsNameSpaceValid) ?
                        this._nameSpacePatern.EndsWith(":") :
                        this.IsNameSpaceValid
                    );
                }
            }
            /// <summary>
            /// Search string
            /// </summary>
            public string SearchPatern {
                get
                {
                    return this._searchPatern;
                }
                set
                {
                    this._searchPatern = value;
                    this.IsSearchPaternValid = ((string.IsNullOrWhiteSpace(this._searchPatern)) ?
                        false :
                        !this._searchPatern.Contains(":")
                    );
                }
            }
            /// <summary>
            /// Cache ID string
            /// </summary>
            public string CacheId { get; set; }
            /// <summary>
            /// Attic ID string (TemeStamp)
            /// </summary>
            public string AtticId { get; set; }
            /// <summary>
            /// Search File = true, search Files = false
            /// </summary>
            public bool IsStrongSearch { get; set; }
            /// <summary>
            /// Namespace is only namespace, not file/page
            /// </summary>
            public bool IsNameSpaceOnly { get; set; }
            /// <summary>
            /// Namespace is only namespace, not file/page
            /// </summary>
            public bool IsNameSpaceValid { get; set; }
            /// <summary>
            /// Namespace is only namespace, not file/page
            /// </summary>
            public bool IsSearchPaternValid { get; set; }
            /// <summary>
            /// Flag to write object
            /// </summary>
            public bool IsWriteOperation { get; set; }
            /// <summary>
            /// class WikiFolderInfo
            /// </summary>
            public WikiFolderInfo FolderInfo { get; set; }

            private string _searchPatern { get; set; }
            private string _nameSpacePatern { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="wft"></param>
            /// <param name="strong"></param>
            /// <param name="namesspace"></param>
            /// <param name="search"></param>
            /// <param name="atticid"></param>
            /// <param name="iswrite"></param>
            public WikiFileParse(WikiFileType wft, bool strong, string namesspace = null, string search = null, string atticid = null, bool iswrite = false)
            {
                FolderType = wft;
                UriPart = new List<string>();
                CacheId = String.Empty;
                AtticId = ((string.IsNullOrWhiteSpace(atticid)) ? String.Empty : atticid);
                SearchPatern = ((string.IsNullOrWhiteSpace(search)) ? String.Empty : search);
                NameSpacePatern = ((string.IsNullOrWhiteSpace(namesspace)) ? ":" : namesspace);
                IsStrongSearch = strong;
                FolderInfo = default(WikiFolderInfo);
                IsWriteOperation = iswrite;
            }
        }
        #endregion

        #region Create Files List methods

        #region internal _CreateFilesList
        internal void _CreateFilesList()
        {
            try
            {
                if (this._lockFs.TryEnterWriteLock((this._isOnstart) ? waitWriteFsOnStart : waitWriteFsOnProcess))
                {
                    try
                    {
                        DateTime dtStart = DateTime.Now;
                        this._wikiFSDict.Clear();
                        this._rootTree.ForEach(r =>
                        {
                            WikiFolderInfo wfi = new WikiFolderInfo();
                            WikiFileType type = DokuUtil.WikiFileStringToType(WikiRequestType.None, r);
                            wfi.Files = this._WikiFileCreateFilesList(Path.Combine(this._rootPath, r), type);
                            wfi.Dirs = this._WikiFileCreateDictFolders(Path.Combine(this._rootPath, r), type);
                            this._wikiFSDict.Add(r, wfi);
                        });
                        this._isFsModify = true;
                        this.waitReadFsOnProcess = ((DateTime.Now.Millisecond - dtStart.Millisecond) + 1000);
                        this.Fire_WikiFSChange(new WikiFSChangeEventArgs(this, this._wikiFSDict.Count, (this.waitReadFsOnProcess - 1000)));
                    }
                    finally
                    {
                        try
                        {
                            this._lockFs.ExitWriteLock();
                        }
                        catch (LockRecursionException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                        catch (SynchronizationLockException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
            }
        }
        #endregion

        #region internal _WikiFileCreateDictFolders
        internal Dictionary<string, WikiFolderInfo> _WikiFileCreateDictFolders(string dir, WikiFileType type)
        {
            Dictionary<string, WikiFolderInfo> foldrDic = new Dictionary<string, WikiFolderInfo>();

            foreach (string d in Directory.GetDirectories(dir))
            {
                WikiFolderInfo wfi = new WikiFolderInfo();
                wfi.Files = _WikiFileCreateFilesList(d, type);
                wfi.Dirs = _WikiFileCreateDictFolders(d, type);
                d.Substring(this._rootPath.Length, (d.Length - this._rootPath.Length))
                    .Split(new string[] { WikiFile.wikiDefaultSeparator }, StringSplitOptions.RemoveEmptyEntries)
                    .ToArray<string>()
                    .Skip(1)
                    .ToList<string>()
                    .ForEach(o =>
                    {
                        wfi.NameSpace = String.Concat(wfi.NameSpace, o, ":");
                    });

                foldrDic.Add(Path.GetFileName(d), wfi);
            }
            return foldrDic;
        }
        #endregion

        #region internal _WikiFileCreateFilesList
        internal List<WikiFileInfo> _WikiFileCreateFilesList(string dir, WikiFileType type)
        {
            List<WikiFileInfo> flst = new List<WikiFileInfo>();
            foreach (string f in Directory.GetFiles(dir))
            {
                FileInfo fi = new FileInfo(f);
                WikiFileInfo wl = new WikiFileInfo();
                wl.TimeWrite = fi.LastWriteTime;
                wl.TimeAccess = fi.LastAccessTime;
                wl.TimeAdd = DateTime.Now;
                wl.FilePath = f;
                wl.FileLock = DateTime.MinValue;
                wl.FileType = type;

                if (wl.FileType == WikiFileType.FileReadMd)
                {
                    wl.FileName = Path.GetFileNameWithoutExtension(f);
                    wl.FileExt = DokuUtil.WikiFileExtToString(f);
                }
                else
                {
                    wl.FileName = Path.GetFileName(f);
                    wl.FileExt = DokuUtil.WikiFileExtToString(f);
                }
                flst.Add(wl);
            }
            return flst;
        }
        
        #endregion

        #endregion

        #region internal _WikiFilesParse (WikiFileParse)
        /// <summary>
        /// internal Uri/Namespace parse
        /// </summary>
        /// <param name="wft">type enum <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</param>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="file">page/media name (serch patern string)</param>
        /// <param name="atticid">Attic ID (Unix Timestamp, Int32)</param>
        /// <param name="strong">Bolean, search one is true, search all is false</param>
        /// <param name="iswrite">Bolean, flag to write resource</param>
        /// <returns><see cref="WikiEngine.WikiFile.WikiFileParse"/>WikiEngine.WikiFile.WikiFileParse</returns>
        internal WikiFileParse _WikiFilesParse(WikiFileType wft, string namesspace, string file, string atticid = null, bool strong = false, bool iswrite = false)
        {
            try
            {
                WikiFileParse wfp = new WikiFileParse(wft, strong, namesspace, file, atticid, iswrite);

                if ((wfp = this.__WikiFilesParse(wfp)) == null)
                {
                    throw new WikiEngineInternalSearchErrorException(namesspace + file);
                }
                return wfp;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// internal Uri/Namespace parse
        /// </summary>
        /// <param name="wfp"></param>
        /// <returns><see cref="WikiFile.WikiFileParse"/>WikiFile.WikiFileParse</returns>
        internal WikiFileParse __WikiFilesParse(WikiFileParse wfp)
        {
            try
            {
                bool checkNs = !string.IsNullOrWhiteSpace(wfp.NameSpacePatern),
                     checkFn = !string.IsNullOrWhiteSpace(wfp.SearchPatern);

                if ((!checkNs) && (!checkFn))
                {
                    throw new WikiEngineInternalNameSpaceErrorException(
                        ((checkNs) ? wfp.NameSpacePatern : "") +
                        ((checkFn) ? wfp.SearchPatern : "")
                    );
                }
                else if ((checkNs) && (!checkFn))
                {
                    if (!wfp.IsNameSpaceValid)
                    {
                        wfp.SearchPatern = wfp.NameSpacePatern;
                        wfp.NameSpacePatern = ":";
                        checkNs = !checkNs;
                        checkFn = !checkFn;
                    }
                }
                else if ((!checkNs) && (checkFn))
                {
                    if (!wfp.IsSearchPaternValid)
                    {
                        wfp.NameSpacePatern = wfp.SearchPatern;
                        wfp.SearchPatern = String.Empty;
                        checkNs = !checkNs;
                        checkFn = !checkFn;
                    }
                }
                if (checkNs)
                {
                    WikiFileType dwft = DokuUtil.WikiFileStringToDefaultType(wfp.NameSpacePatern, wfp.IsWriteOperation);
                    switch (dwft)
                    {
                        case WikiFileType.None:
                        case WikiFileType.FileUnknown:
                            {
                                switch (wfp.FolderType)
                                {
                                    case WikiFileType.None:
                                    case WikiFileType.FileUnknown:
                                        {
                                            throw new WikiEngineInternalFileTypeException(
                                                ((checkNs) ? wfp.NameSpacePatern : "") +
                                                ((checkFn) ? wfp.SearchPatern : "")
                                            );
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                                break;
                            }
                        case WikiFileType.NameSpace:
                            {
                                /* 
                                // strong check files
                                if (!checkFn)
                                {
                                    throw new WikiEngineInternalNameSpaceErrorException(
                                        ((checkFn) ? wfp.SearchPatern : "")
                                    );
                                }
                                 */
                                break;
                            }
                        default:
                            {
                                switch (wfp.FolderType)
                                {
                                    case WikiFileType.None:
                                    case WikiFileType.FileUnknown:
                                        {
                                            wfp.FolderType = dwft;
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                }
                switch (wfp.FolderType)
                {
                    case WikiFileType.None:
                    case WikiFileType.FileUnknown:
                        {
                            throw new WikiEngineInternalFileTypeException(
                                ((checkNs) ? wfp.NameSpacePatern : "") +
                                ((checkFn) ? wfp.SearchPatern : "")
                            );
                        }
                    case WikiFileType.FileReadAttic:
                    case WikiFileType.FileWriteAttic:
                        {
                            this.__WikiFilesParseAtticExtension(ref wfp, checkNs, checkFn, atticExtension);
                            break;
                        }
                    case WikiFileType.FileReadMeta:
                    case WikiFileType.FileWriteMeta:
                        {
                            this.__WikiFilesParseMetaExtension(ref wfp, checkNs, checkFn, metaExtension);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                // Cache read request
                wfp.CacheId = ((this._isCacheEnable) ?
                    _WikiFileCacheId(
                        wfp.FolderType,
                        string.Concat(
                            ((checkNs) ? wfp.NameSpacePatern : "-"),
                            (((!checkNs) && (checkFn)) ? wfp.SearchPatern : "-")
                        )
                    ) :
                    String.Empty
                );
                if ((this._isCacheEnable) && ((wfp.FolderInfo = _WikiFileCacheRead(wfp.CacheId)) != null))
                {
                    return wfp;
                }
                // End Cache

                if ((wfp.UriPart = wfp.NameSpacePatern
                    .Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList<string>()) == null)
                {
                    throw new WikiEngineInternalSearchOkException();
                }
                if (
                    (!checkFn) &&
                    (wfp.UriPart.Count > 1)
                   )
                {
                    wfp.SearchPatern = wfp.UriPart[(wfp.UriPart.Count - 1)];
                    wfp.UriPart.RemoveAt(wfp.UriPart.Count - 1);
                }
                return wfp;
            }
            catch (WikiEngineInternalSearchOkException)
            {
                return null;
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                return null;
            }
        }
        /// <summary>
        /// Meta append path to specific locations
        /// part of __WikiFilesParse
        /// </summary>
        /// <param name="checkNs"></param>
        /// <param name="checkFn"></param>
        /// <param name="wfp"></param>
        /// <param name="app"></param>
        internal void __WikiFilesParseMetaExtension(ref WikiFileParse wfp, bool checkNs, bool checkFn, string app)
        {
            try
            {
                if (checkFn)
                {
                    wfp.SearchPatern = ___WikiFilesParseMetaExtension(wfp.SearchPatern, app);
                }
                else if (checkNs)
                {
                    wfp.NameSpacePatern = ___WikiFilesParseMetaExtension(wfp.NameSpacePatern, app);
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
            }
        }
        /// <summary>
        /// Sub function WikiFilesParseMetaExtension
        /// </summary>
        /// <param name="src"></param>
        /// <param name="app"></param>
        /// <returns>String</returns>
        internal string ___WikiFilesParseMetaExtension(string src, string app)
        {
            return ((string.IsNullOrWhiteSpace(Path.GetExtension(src))) ?
                string.Format(Properties.ResourceWikiEngine.fmtFailMeta3Format, src, mdExtension, app) :
                string.Format(Properties.ResourceWikiEngine.fmtFailMeta2Format, src, app));
        }
        /// <summary>
        /// Attic append path to specific locations
        /// part of __WikiFilesParse
        /// </summary>
        /// <param name="wfp"></param>
        /// <param name="checkNs"></param>
        /// <param name="checkFn"></param>
        /// <param name="app"></param>
        internal void __WikiFilesParseAtticExtension(ref WikiFileParse wfp, bool checkNs, bool checkFn, string app)
        {
            try
            {
                // Test:
                // wfp.AtticId = "123456789";
                // wfp.NameSpacePatern = "clan:xyz007.123456789.jpg";
                // wfp.NameSpacePatern = "clan:xyz007.123456789";
                // wfp.NameSpacePatern = "clan:xyz007.jpg";
                // wfp.NameSpacePatern = "clan:xyz007";

                int t, pos = 0;
                string sdate = String.Empty,
                       sext  = String.Empty;

                bool isAtticId = !string.IsNullOrWhiteSpace(wfp.AtticId);
                string[] part = ((checkFn) ? wfp.SearchPatern : wfp.NameSpacePatern)
                                    .Split(new string[] {"."}, StringSplitOptions.RemoveEmptyEntries);

                switch (part.Length)
                {
                    case 1:
                        {
                            if (!isAtticId)
                            {
                                throw new WikiEngineInternalCacheExceptionAtticException();
                            }
                            sext = mdExtension;
                            sdate = wfp.AtticId;
                            break;
                        }
                    case 2:
                        {
                            if (Int32.TryParse(part[1], out t))
                            {
                                sext  = mdExtension;
                                sdate = part[1];
                                pos  += part[1].Length + 1;
                            }
                            else if (!isAtticId)
                            {
                                throw new WikiEngineInternalCacheExceptionAtticException();
                            }
                            else
                            {
                                sext = part[1];
                                pos += part[1].Length + 1;
                                sdate = wfp.AtticId;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (Int32.TryParse(part[1], out t))
                            {
                                sdate = part[1];
                                pos  += part[1].Length + 1;
                            }
                            else if (!isAtticId)
                            {
                                throw new WikiEngineInternalCacheExceptionAtticException();
                            }
                            else
                            {
                                sdate = wfp.AtticId;
                            }
                            sext = part[2];
                            pos += part[2].Length + 1;
                            break;
                        }
                    default:
                        {
                            throw new WikiEngineInternalCacheExceptionAtticException();
                        }
                }
                if (checkFn)
                {
                    wfp.SearchPatern = string.Format(
                        Properties.ResourceWikiEngine.fmtFailAtticFormat,
                        wfp.SearchPatern.Substring(0, (wfp.SearchPatern.Length - pos)),
                        sdate,
                        sext,
                        app
                    );
                }
                else if (checkNs)
                {
                    wfp.NameSpacePatern = string.Format(
                        Properties.ResourceWikiEngine.fmtFailAtticFormat,
                        wfp.NameSpacePatern.Substring(0, (wfp.NameSpacePatern.Length - pos)),
                        sdate,
                        sext,
                        app
                    );
                }
            }
            catch (WikiEngineInternalCacheExceptionAtticException)
            {
                WikiEngineInternalCacheExceptionAtticException ex = 
                    new WikiEngineInternalCacheExceptionAtticException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorAtticIdEmpty,
                            ((checkFn) ?
                                wfp.SearchPatern :
                                ((checkNs) ?
                                    wfp.NameSpacePatern :
                                    "none.."
                                )
                            )
                        )
                    );
                this.Fire_ProcessError(new WikiErrorEventArgs(ex, MethodBase.GetCurrentMethod().Name));
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
            }
        }
        #endregion

        #region internal _WikiFilesFindFiles (WikiFolderInfo)
        /// <summary>
        /// internal Wiki find files
        /// </summary>
        /// <param name="wfp"><see cref="WikiFile.WikiFileParse"/>WikiFile.WikiFileParse data</param>
        /// <returns>WikiFolderInfo data</returns>
        internal WikiFolderInfo __WikiFilesFindFiles(WikiFileParse wfp)
        {
            WikiFolderInfo wfi = null, wfo = null;
            string key = String.Empty;

            try
            {
                if (this._taskFilesList.Status == TaskStatus.Running)
                {
                    this._taskFilesList.Wait();
                    if (this._taskFilesList.Exception != null)
                    {
                        throw new WikiEngineInternalSearchErrorException(this._taskFilesList.Exception.Message);
                    }
                }
                if (this._lockFs.TryEnterReadLock((this._isOnstart) ? waitReadFsOnStart : waitReadFsOnProcess))
                {
                    try
                    {
                        if (
                            (this._wikiFSDict == null) ||
                            (this._wikiFSDict.Count == 0)
                           )
                        {
                            throw new WikiEngineInternalSearchErrorException("FS base is empty");
                        }
                        key = DokuUtil.WikiFileTypeToString(wfp.FolderType);
                        if ((wfi =
                            this._wikiFSDict.Where(o => o.Key.Equals(key))
                               .Select(o =>
                               {
                                   return ((WikiFolderInfo)o.Value);
                               })
                               .FirstOrDefault<WikiFolderInfo>()) == null)
                        {
                            throw new WikiEngineInternalSearchEmptyException();
                        }
                        if (
                            (wfp.UriPart != null) &&
                            (wfp.UriPart.Count > 0)
                           )
                        {
                            wfp.UriPart.ForEach(fpart =>
                            {
                                if ((wfi = this.__WikiFilesFindNamespaceFolder(wfi, fpart)) == null)
                                {
                                    return;
                                }
                                key = fpart;
                            });
                        }
                        if (wfi == null)
                        {
                            throw new WikiEngineInternalSearchEmptyException();
                        }

                        wfo = new WikiFolderInfo();
                        wfo.SearchPatern = wfp.SearchPatern;
                        wfo.SearchStrong = wfp.IsStrongSearch;

                        if (this.__WikiFilesFindNamespaceFiles(ref wfo, wfi, key))
                        {
                            throw new WikiEngineInternalSearchOkException();
                        }
                        throw new WikiEngineInternalSearchEmptyException();
                    }
                    finally
                    {
                        try
                        {
                            this._lockFs.ExitReadLock();
                        }
                        catch (LockRecursionException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                        catch (SynchronizationLockException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                    }
                }
                throw new WikiEngineInternalSearchEmptyException();
            }
            catch (WikiEngineInternalSearchOkException)
            {
                // Cache write result
                if (this._isCacheEnable)
                {
                    _WikiFileCacheWrite(wfo, wfp.CacheId);
                }
                //
                return wfo;
            }
            catch (WikiEngineInternalSearchEmptyException)
            {
                return null;
            }
            catch (WikiEngineInternalSearchErrorException e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                return null;
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                return null;
            }
        }
        internal string _WikiFileCacheId(WikiFileType wft, string src)
        {
            return
                DokuUtil.WikiFileTypeToString(wft) + cacheReplace1 +
                src
                .Replace(":", cacheReplace1)
                .Replace(WikiFile.wikiDefaultSeparator, cacheReplace2);
        }
        #endregion

        #region internal _WikiFilesFindNamespace (WikiFolderInfo)
        /// <summary>
        /// internal find Namespace
        /// </summary>
        /// <param name="wfp"><see cref="WikiFile.WikiFileParse"/>WikiFile.WikiFileParse data</param>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        internal WikiFolderInfo __WikiFilesFindNamespace(WikiFileParse wfp)
        {
            WikiFolderInfo wfi = null;

            try
            {
                if (this._taskFilesList.Status == TaskStatus.Running)
                {
                    this._taskFilesList.Wait();
                    if (this._taskFilesList.Exception != null)
                    {
                        throw this._taskFilesList.Exception;
                    }
                }
                if (this._lockFs.TryEnterReadLock((this._isOnstart) ? waitReadFsOnStart : waitReadFsOnProcess))
                {
                    try
                    {
                        if (
                            (this._wikiFSDict == null) ||
                            (this._wikiFSDict.Count == 0)
                           )
                        {
                            throw new WikiEngineInternalSearchErrorException("FS base is empty");
                        }
                        if ((wfi =
                            this._wikiFSDict.Where(o => o.Key.Equals(DokuUtil.WikiFileTypeToString(wfp.FolderType)))
                               .Select(o =>
                               {
                                   return ((WikiFolderInfo)o.Value);
                               })
                               .FirstOrDefault<WikiFolderInfo>()) == null)
                        {
                            throw new WikiEngineInternalSearchErrorException();
                        }
                        if (
                            (wfp.UriPart == null) ||
                            (wfp.UriPart.Count == 0)
                           )
                        {
                            throw new WikiEngineInternalSearchOkException();
                        }

                        int i = 0, count = ((wfp.IsNameSpaceOnly) ? wfp.UriPart.Count : (wfp.UriPart.Count - 1));

                        wfp.UriPart.ForEach(fpart =>
                        {
                            if (i == count)
                            {
                                return;
                            }
                            if ((wfi = this.__WikiFilesFindNamespaceFolder(wfi, fpart)) == null)
                            {
                                return;
                            }
                            i++;
                        });
                        if (wfi != null)
                        {
                            wfi.SearchPatern = ((wfp.IsNameSpaceOnly) ? String.Empty : wfp.UriPart[count]);
                            throw new WikiEngineInternalSearchOkException();
                        }
                        else
                        {
                            throw new WikiEngineInternalSearchErrorException();
                        }
                    }
                    finally
                    {
                        try
                        {
                            this._lockFs.ExitReadLock();
                        }
                        catch (LockRecursionException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                        catch (SynchronizationLockException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                    }
                }
                throw new WikiEngineInternalSearchErrorException();
            }
            catch (WikiEngineInternalSearchOkException)
            {
                return wfi;
            }
            catch (WikiEngineInternalSearchErrorException)
            {
                return null;
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                return null;
            }
        }
        #endregion

        #region internal __WikiFilesFindNamespaceFolder (WikiFolderInfo)
        /// <summary>
        /// internal find Namespace folder, iteration recursive
        /// </summary>
        /// <param name="wfi">WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo input</param>
        /// <param name="search">serch patern string</param>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        internal WikiFolderInfo __WikiFilesFindNamespaceFolder(WikiFolderInfo wfi, string search)
        {
            if (
                (wfi == null) ||
                (wfi.Dirs.Count == 0) ||
                (string.IsNullOrWhiteSpace(search))
               )
            {
                return null;
            }
            return wfi.Dirs.Where(o => o.Key.Equals(search))
                .Select(o =>
                {
                    WikiFolderInfo wfo = (WikiFolderInfo)((WikiFolderInfo)o.Value).Clone();
                    return wfo;
                })
                .FirstOrDefault<WikiFolderInfo>();
        }
        #endregion

        #region internal __WikiFilesFindNamespaceFiles (bool)
        /// <summary>
        /// internal find Namespace folder, iteration recursive
        /// </summary>
        /// <param name="wfo">ref <see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo, out data</param>
        /// <param name="wfi"><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo, input data</param>
        /// <param name="key">Start key to search from Dictionary, its directory name</param>
        /// <returns>Bolean</returns>
        internal bool __WikiFilesFindNamespaceFiles(ref WikiFolderInfo wfo, WikiFolderInfo wfi, string key)
        {
            bool isUpdate = false;

            if (wfi == null)
            {
                return isUpdate;
            }
            if (wfi.Files.Count > 0)
            {
                bool isStrongSearch = wfo.SearchStrong;
                string search = wfo.SearchPatern;
                WikiFolderInfo nwfo = new WikiFolderInfo();
                nwfo.NameSpace = wfi.NameSpace;

                wfi.Files.ForEach(o =>
                {
                    if (
                        ((isStrongSearch) && (o.FileName.Equals(search))) ||
                        ((!isStrongSearch) && (o.FileName.Contains(search)))
                       )
                    {
                        WikiFileInfo nwfi = (WikiFileInfo)o.Clone();
                        nwfi.NameSpace = nwfo.NameSpace;
                        nwfo.Files.Add(nwfi);
                        isUpdate = true;
                    }
                });
                if (isUpdate)
                {
                    wfo.Dirs.Add(key, nwfo);
                    if (isStrongSearch)
                    {
                        return isUpdate;
                    }
                }
            }
            if (wfi.Dirs.Count > 0)
            {
                foreach (KeyValuePair<string, WikiFolderInfo> item in wfi.Dirs)
                {
                    bool ret = __WikiFilesFindNamespaceFiles(ref wfo, ((WikiFolderInfo)item.Value), ((string)item.Key));
                    isUpdate = ((isUpdate) ? isUpdate : ret);
                }
            }
            return isUpdate;
        }
        #endregion

        #region internal _WikiFileActionRecursive (bool)
        /// <summary>
        /// Recursive Action, use in:
        /// <see cref="_SetFileLock(WikiEngine.WikiData, bool)"/>
        /// </summary>
        /// <param name="wfi"><see cref="WikiEngine.WikiFolderInfo"/></param>
        /// <param name="wfa"><see cref="WikiEngine.WikiFileAction"/></param>
        /// <param name="act">Action&lt;WikiEngine.WikiFileAction&gt;</param>
        /// <returns>Bolean</returns>
        internal bool _WikiFileActionRecursive(
                            WikiFolderInfo wfi,
                            WikiFileAction wfa,
                            Action<WikiFileAction> act
                      )
        {
            bool isUpdate = false;

            if (
                (act == null) ||
                (wfa == null) ||
                (wfi == null)
               )
            {
                return false;
            }
            if (wfi.Files.Count > 0)
            {
                wfa.ConutFile = 0;
                wfa.NameSpace = wfi.NameSpace;
                wfa.ConutFiles += wfi.Files.Count;

                wfi.Files.ForEach(o =>
                {
                    if (
                        (wfa.IsAllFiles) ||
                        (o.FileName.Equals(wfa.Search))
                       )
                    {
                        wfa.FileInfo = o;
                        wfa.ConutFile++;
                        act(wfa);
                        isUpdate = true;
                        if (!wfa.IsAllFiles)
                        {
                            return;
                        }
                    }
                });
                if (
                    (!wfa.IsAllFiles) &&
                    (isUpdate)
                   )
                {
                    return isUpdate;
                }
            }
            if (wfi.Dirs.Count > 0)
            {
                wfa.ConutDirs += wfi.Dirs.Count;

                foreach (KeyValuePair<string, WikiFolderInfo> item in wfi.Dirs)
                {
                    wfa.Key = ((string)item.Key);

                    bool ret = _WikiFileActionRecursive(
                        ((WikiFolderInfo)item.Value),
                        wfa,
                        act
                    );
                    isUpdate = ((isUpdate) ? isUpdate : ret);
                }
            }
            return isUpdate;
        }
        #endregion

        #region internal _WikiFileGetFileInfo (WikiFileInfo)
        /// <summary>
        /// Get file information from FS from string
        /// </summary>
        /// <param name="wft">type enum <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</param>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="file">page/media name (serch patern string)</param>
        /// <returns><see cref="WikiEngine.WikiFileInfo"/>WikiEngine.WikiFileInfo</returns>
        internal WikiFileInfo _WikiFileGetFileInfo(WikiFileType wft, string namesspace, string file)
        {
            try
            {
                WikiFileParse wfp = new WikiFileParse(wft, true, namesspace, file);

                if ((wfp = this.__WikiFilesParse(wfp)) == null)
                {
                    throw new WikiEngineInternalSearchErrorException(namesspace + file);
                }
                return this.__WikiFileGetFileInfoSelect(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Get file information from FS from WikiEngine.WikiFileParse internal class
        /// </summary>
        /// <param name="wfp">internal class <see cref="WikiEngine.WikiFile.WikiFileParse"/>WikiEngine.WikiFile.WikiFileParse</param>
        /// <returns><see cref="WikiEngine.WikiFileInfo"/>WikiEngine.WikiFileInfo</returns>
        internal WikiFileInfo _WikiFileGetFileInfo(WikiFileParse wfp)
        {
            try
            {
                if (wfp == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    );
                }
                return this.__WikiFileGetFileInfoSelect(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// part from _WikiFileGetFileInfo(WikiFileParse) internal method
        /// </summary>
        /// <param name="wfp">internal class <see cref="WikiEngine.WikiFile.WikiFileParse"/>WikiEngine.WikiFile.WikiFileParse</param>
        /// <returns><see cref="WikiEngine.WikiFileInfo"/>WikiEngine.WikiFileInfo</returns>
        internal WikiFileInfo __WikiFileGetFileInfoSelect(WikiFileParse wfp)
        {
            WikiFileInfo wfo = null;
            WikiFolderInfo wfi = null;

            try
            {
                if (wfp.FolderInfo != null)
                {
                    wfi = wfp.FolderInfo;
                }
                else
                {
                    if ((wfi = this.__WikiFilesFindFiles(wfp)) == null)
                    {
                        throw new WikiEngineInternalSearchEmptyException(
                            string.Format(
                                Properties.ResourceWikiEngine.mdErrorSearchEmpty,
                                wfp.NameSpacePatern,
                                wfp.SearchPatern,
                                wfp.FolderType
                            )
                        );
                    }
                }
                if (wfi.Dirs.Count != 1)
                {
                    string exceptMessage = string.Format(
                        "Directory count: {0} '{1}{2}'",
                        wfi.Dirs.Count,
                        wfp.NameSpacePatern,
                        wfp.SearchPatern
                    );
                    if (wfi.Dirs.Count == 0)
                    {
                        throw new WikiEngineInternalSearchEmptyException(exceptMessage);
                    }
                    throw new WikiEngineInternalSearchManyResultException(exceptMessage);
                }
                if ((wfo = wfi.Dirs
                    .ElementAtOrDefault<KeyValuePair<string, WikiFolderInfo>>(0)
                    .Value
                    .Files
                    .FirstOrDefault<WikiFileInfo>()) == null)
                {
                    throw new WikiEngineInternalSearchEmptyException(
                        string.Format(
                            Properties.ResourceWikiEngine.mdErrorSearchEmpty,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern,
                            wfp.FolderType
                        )
                    );
                }
                return wfo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region private _OnWikiFSChangeWatch (Watch Wiki FS)
        /// <summary>
        /// CallBack from <see cref="System.IO.FileSystemWatcher"/>FileSystemWatcher
        /// </summary>
        /// <param name="source">object, not used</param>
        /// <param name="ev"><see cref="System.IO.FileSystemEventArgs"/>FileSystemEventArgs</param>
        private void _OnWikiFSChangeWatch(object source, FileSystemEventArgs ev)
        {
            if (
                (ev != null) &&
                (ev.FullPath.Contains(this._cacheDirPath))
               )
            {
                return;
            }
            try
            {
                if (this._taskFilesList != null)
                {
                    if (this._taskFilesList.Status == TaskStatus.RanToCompletion)
                    {
                        this._taskFilesList.Dispose();
                        this._taskFilesList = null;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
            }
            this._taskFilesList = Task.Factory.StartNew(() =>
            {
                this._isOnstart = false;
                this._WikiFileCacheClear();
                this._CreateFilesList();
            });
        }
        #endregion
    }
}
