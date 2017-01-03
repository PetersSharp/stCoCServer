using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using stDokuWiki.Util;
using stDokuWiki.WikiEngine;
using stDokuWiki.WikiEngine.Exceptions;
using stDokuWiki.WikiSyntax;

using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace stDokuWiki.WikiEngine
{
    /// <summary>
    /// DokuWiki compatible file engine.
    /// Get/Put/Delete/List page and media files.
    /// </summary>
    public partial class WikiFile
    {
        #region Variables

        #region Constant
        /// <summary>
        /// Default Wiki root folder
        /// </summary>
        public const string wikiLocalPath = "wiki";
        /// <summary>
        /// Default Wiki root/pages folder
        /// </summary>
        public const string wikiLocalPage = "pages";
        /// <summary>
        /// Default Wiki root/media folder
        /// </summary>
        public const string wikiLocalMedia = "media";
        /// <summary>
        /// Default Wiki root/attic folder
        /// </summary>
        public const string wikiLocalAttic = "attic";
        /// <summary>
        /// Default Wiki root/meta folder
        /// </summary>
        public const string wikiLocalMeta = "meta";
        /// <summary>
        /// Default Wiki root/cache folder
        /// </summary>
        public const string wikiLocalCache = "cache";
        /// <summary>
        /// Default Wiki root/cache/find folder, store and read find request
        /// </summary>
        public const string wikiLocalCacheFind = "find";
        /// <summary>
        /// Default Namespace to unknown write namespace
        /// </summary>
        public const string wikiDefaultEmptyNS = "playground";
        /// <summary>
        /// Default separator from runing system
        /// </summary>
        public static readonly string wikiDefaultSeparator = Path.DirectorySeparatorChar.ToString();
        /// <summary>
        /// Default meta extensions
        /// </summary>
        public const string metaExtension = "changes";
        /// <summary>
        /// Default attic extensions
        /// </summary>
        public const string atticExtension = "gz";
        /// <summary>
        /// Default pages extensions
        /// </summary>
        public const string mdExtension = "txt";
        /// <summary>
        /// Default media extensions
        /// </summary>
        public const string binExtension = "bin";
        #endregion

        private string _rootPath = String.Empty;
        private string _defaultNameSpace = String.Empty;
        private string _cacheDirPath = String.Empty;
        private bool _isOnstart = true;
        private bool _isFsModify = false;
        private bool _isCacheEnable = true;
        private bool _isMapExceptions = true;

        private List<string> _fileExtensionAllow = null;
        private Dictionary<string, WikiFolderInfo> _wikiFSDict = null;
        private ReaderWriterLockSlim _lockCache = null;
        private ReaderWriterLockSlim _lockFs = null;
        private Task _taskFilesList = null;
        private FileSystemWatcher _wikiFSWatch = null;
        private DokuWikiFormat _dwFormat = null;

        #region Directory list
        private readonly List<string> _rootTree = new List<string>()
            {
                wikiLocalPage,
                wikiLocalMedia,
                wikiLocalAttic,
                wikiLocalMeta
            };
        #endregion

        /// <summary>
        /// DokuWiki Syntax class <see cref="WikiSyntax.DokuWikiFormat"/>WikiSyntax.DokuWikiFormat
        /// </summary>
        private DokuWikiFormat wikiFormat
        {
            get
            {
                if (this._dwFormat == null)
                {
                    this._dwFormat = new DokuWikiFormat();
                }
                return this._dwFormat;
            }
        }

        /// <summary>
        /// Set root path to wiki folder.
        /// Create all path if necessary.
        /// </summary>
        public string RootPath
        {
            get { return this._rootPath; }
            set
            {
                this._rootPath = ((string.IsNullOrWhiteSpace(value)) ? Path.Combine("data", wikiLocalPath) : value);
                this._cacheDirPath = Path.Combine(
                    this._rootPath,
                    wikiLocalCache,
                    wikiLocalCacheFind
                );
                try
                {
                    List<string> toPath = new List<string>()
                        {
                            this._rootPath,
                            this._cacheDirPath
                        };
                    List<string> toSubPath = new List<string>()
                        {
                            wikiLocalPage,
                            wikiLocalMedia,
                            wikiLocalAttic,
                            wikiLocalMeta
                        };
                    toPath.ForEach(o =>
                    {
                        if (!Directory.Exists(o))
                        {
                            Directory.CreateDirectory(o);
                        }
                    });
                    toSubPath.ForEach(o =>
                    {
                        string path = Path.Combine(this._rootPath, o);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    });
                }
                catch (Exception e)
                {
                    this._rootPath = String.Empty;
                    this._cacheDirPath = String.Empty;
                    throw e;
                }
            }
        }

        /// <summary>
        /// Set default Namespace
        /// </summary>
        public string DefaultNameSpace
        {
            get { return this._defaultNameSpace; }
            set
            {
                this._defaultNameSpace = ((string.IsNullOrWhiteSpace(value)) ?
                    ((string.IsNullOrWhiteSpace(this._defaultNameSpace)) ? ":" : this._defaultNameSpace) :
                    ((value.EndsWith(":")) ? value : string.Concat(value, ":"))
                );
            }
        }
        /// <summary>
        /// Set/Get file Extension Allow to put/get (Upload/Download)
        /// </summary>
        public string FileExtensionAllow
        {
            set {
                if (
                    (value.Contains(",")) ||
                    (value.Contains(":")) ||
                    (value.Contains(";"))
                   )
                {
                    this._fileExtensionAllow.Clear();

                    foreach (string s in value.Split(new char[] { ',',':',';' }))
                    {
                        this._fileExtensionAllow.Add(s);
                    }
                }
            }
            get
            {
                if (this._fileExtensionAllow.Count > 0)
                {
                    return string.Join(",", this._fileExtensionAllow.ToArray());
                }
                return String.Empty;
            }
        }
        /// <summary>
        /// Get only CacheDirPath
        /// </summary>
        public string CacheDirPath
        {
            get { return this._cacheDirPath; }
        }

        /// <summary>
        /// (Bolean) is Wiki FS modify
        /// </summary>
        public bool IsFsModify
        {
            get
            {
                if (this._isFsModify)
                {
                    this._isFsModify = false;
                    return true;
                }
                return false;
            }
        }
        /// <summary>
        /// Enable/Disable cache result
        /// Source is find method result
        /// </summary>
        public bool IsCacheEnable
        {
            get { return this._isCacheEnable; }
            set { this._isCacheEnable = value; }
        }

        /// <summary>
        /// Enable/Disable cache result
        /// Source is find method result
        /// </summary>
        public bool IsMapExceptions
        {
            get { return this._isMapExceptions; }
            set { this._isMapExceptions = value; }
        }

        /// <summary>
        /// Root Namspace count (read only)
        /// </summary>
        public Int32 CountRootNamspace
        {
            get { return this._wikiFSDict.Count; }
        }

        /// <summary>
        /// Last Scaning FS total time is milliseconds. (read only)
        /// </summary>
        public Int32 ScanTime
        {
            get { return (this.waitReadFsOnProcess - 1000); }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event Wiki Error, return <see cref="WikiEngine.WikiErrorEventArgs"/>WikiEngine.WikiErrorEventArgs
        /// </summary>
        public event EventHandler<WikiErrorEventArgs> OnProcessError = delegate { };

        /// <summary>
        /// Event Wiki File System change <see cref="WikiEngine.WikiFSChangeEventArgs"/>WikiEngine.WikiFSChangeEventArgs
        /// </summary>
        public event EventHandler<WikiFSChangeEventArgs> OnWikiFSChange = delegate { };

        private AsyncOperation op;

        private void Fire_ProcessError(WikiErrorEventArgs o)
        {
            op.Post(x => OnProcessError(this, (WikiErrorEventArgs)x), o);
        }
        private void Fire_WikiFSChange(WikiFSChangeEventArgs o)
        {
            op.Post(x => OnWikiFSChange(this, (WikiFSChangeEventArgs)x), o);
        }

        #endregion

        #region Constructor
        
        /// <summary>
        /// Init DokuWiki compatible file engine.
        /// </summary>
        /// <code>
        /// using stDokuWiki.WikiEngine;
        /// using stDokuWiki.WikiEngine.Exceptions;
        /// 
        /// try
        /// {
        ///    WikiFile wf = new WikiFile("data");
        ///    wf.OnProcessError += (o, e) =&gt;
        ///    {
        ///       Console.WriteLine(
        ///         "OnError: "
        ///         + Environment.NewLine + " Type:    " + e.ex.GetType().Name
        ///         + Environment.NewLine + " Method:  " + e.MethodName
        ///         + Environment.NewLine + " Message: " + e.ex.Message
        ///       );
        ///    };
        ///    wf.OnWikiFSChange += (o, e) =&gt;
        ///    {
        ///       Console.WriteLine("Root namspace count: " + e.Count + ":" + e.WikiFile.CountRootNamspace);
        ///    };
        /// }
        /// catch (Exception e)
        /// {
        ///    Console.WriteLine(e.GetType().Name + ": " + e.Message);
        /// }
        /// </code>
        /// <param name="rootPath">Root path to wiki folder</param>
        public WikiFile(string rootPath)
        {
            this.op = AsyncOperationManager.CreateOperation(null);
            this.RootPath = rootPath;
            this._defaultNameSpace = "clan:";
            this._wikiFSDict = new Dictionary<string, WikiFolderInfo>();
            this._lockCache = new ReaderWriterLockSlim();
            this._lockFs = new ReaderWriterLockSlim();
            this._fileExtensionAllow = new List<string>();
            this._OnWikiFSChangeWatch(null, null);

            this._wikiFSWatch = new FileSystemWatcher();
            this._wikiFSWatch.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime | NotifyFilters.Size | NotifyFilters.FileName;
            this._wikiFSWatch.Path = this._rootPath;
            this._wikiFSWatch.Changed += new FileSystemEventHandler(this._OnWikiFSChangeWatch);
            this._wikiFSWatch.EnableRaisingEvents = true;
            this._wikiFSWatch.IncludeSubdirectories = true;
        }
        /// <summary>
        /// Destructor class WikiFile
        /// </summary>
        ~WikiFile()
        {
            if (this._taskFilesList != null)
            {
                if (
                    (this._taskFilesList.Status == TaskStatus.Running) ||
                    (this._taskFilesList.Status == TaskStatus.WaitingToRun) ||
                    (this._taskFilesList.Status == TaskStatus.WaitingForChildrenToComplete)
                   )
                {
                    this._taskFilesList.Wait();
                }
                this._taskFilesList.Dispose();
                this._taskFilesList = null;
            }
            if (this._lockCache != null)
            {
                this._lockCache.Dispose();
                this._lockCache = null;
            }
        }

        #endregion

        #region public FS Methods

        #region methods GetFileInfo
        /// <summary>
        /// Get wiki file <see cref="WikiEngine.WikiFileInfo"/> iformation
        /// </summary>
        /// <code>
        ///   WikiEngine.WikiFileInfo wfi = wf.GetFileInfo(WikiEngine.WikiFileType.FileReadMd,"testns:filepage1");
        ///   if (wfi != null)
        ///   {
        ///      Console.WriteLine(
        ///         " File: " + wfi.FileName +
        ///         " LastAccessTime: " + wfi.LastAccessTime +
        ///         " LastWriteTime: " + wfi.LastWriteTime
        ///      );
        ///   }
        /// </code>
        /// <param name="wft">object type: enum <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</param>
        /// <param name="namesspace">Wiki name space include page name</param>
        /// <returns>WikiEngine.WikiFileInfo</returns>
        public WikiFileInfo GetFileInfo(WikiFileType wft, string namesspace)
        {
            try
            {
                WikiFileParse wfp = null;
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                        wft, namesspace, null, null, true
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                }
                return this._GetFileInfo(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private WikiFileInfo _GetFileInfo(WikiFileParse wfp)
        {
            try
            {
                Task<WikiFileInfo> t1 = Task<WikiFileInfo>.Factory.StartNew(() =>
                {
                    try
                    {
                        WikiFileInfo wfi = null;

                        if (wfp == null)
                        {
                            throw new WikiEngineInternalNameSpaceErrorException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                                    MethodBase.GetCurrentMethod().Name
                                )
                            );
                        }
                        if ((wfi = (WikiFileInfo)this._WikiFileGetFileInfo(wfp)) == null)
                        {
                            throw new WikiEngineInternalSearchEmptyException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorGetResource,
                                    Properties.ResourceWikiEngine.txtErrorSearchEmpty,
                                    string.Concat(wfp.NameSpacePatern, wfp.SearchPatern)
                                )
                            );
                        }
                        return wfi;
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        return default(WikiFileInfo);
                    }
                });

                t1.Wait();

                WikiFileInfo wfo = t1.Result;
                this._TaskEnd(t1, wfo,
                    ((wfp != null) ?
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorTaskEnd,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern
                        ) :
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    )
                );
                return wfo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get wiki page file <see cref="WikiEngine.WikiFileInfo"/> iformation
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.GetFileInfo"/>WikiEngine.WikiFile.GetFileInfo()</example>
        /// <param name="namesspace">Wiki name space include page name</param>
        /// <returns>WikiEngine.WikiFileInfo</returns>
        public WikiFileInfo GetFileInfoPage(string namesspace)
        {
            return this.GetFileInfo(WikiFileType.FileReadMd, namesspace);
        }
        /// <summary>
        /// Get wiki media file <see cref="WikiEngine.WikiFileInfo"/> iformation
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.GetFileInfo"/>WikiEngine.WikiFile.GetFileInfo()</example>
        /// <param name="namesspace">Wiki name space include page name</param>
        /// <returns>WikiEngine.WikiFileInfo</returns>
        public WikiFileInfo GetFileInfoMedia(string namesspace)
        {
            return this.GetFileInfo(WikiFileType.FileReadBinary, namesspace);
        }
        /// <summary>
        /// Get wiki attic file <see cref="WikiEngine.WikiFileInfo"/> iformation
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.GetFileInfo"/>WikiEngine.WikiFile.GetFileInfo()</example>
        /// <param name="namesspace">Wiki name space include page name</param>
        /// <returns>WikiEngine.WikiFileInfo</returns>
        public WikiFileInfo GetFileInfoAttic(string namesspace)
        {
            return this.GetFileInfo(WikiFileType.FileReadAttic, namesspace);
        }
        #endregion

        #region method GetFile
        /// <summary>
        /// Get file content as byte [] from wiki namespace
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="wft">type of request <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</param>
        /// <returns>WikiEngine.WikiData</returns>
        public WikiData GetFile(string namesspace, WikiFileType wft = WikiFileType.None)
        {
            try
            {
                WikiFileParse wfp = null;
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                        wft, namesspace, null, null, true
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                }
                return this._GetFile(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private WikiData _GetFile(WikiFileParse wfp)
        {
            try
            {
                Task<WikiData> t1 = Task<WikiData>.Factory.StartNew(() =>
                {
                    try
                    {
                        WikiFileInfo wfi = null;

                        if (wfp == null)
                        {
                            throw new WikiEngineInternalNameSpaceErrorException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                                    MethodBase.GetCurrentMethod().Name
                                )
                            );
                        }
                        if ((wfi = (WikiFileInfo)this._WikiFileGetFileInfo(wfp)) == null)
                        {
                            throw new WikiEngineInternalSearchEmptyException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorGetResource,
                                    Properties.ResourceWikiEngine.txtErrorSearchEmpty,
                                    string.Concat(wfp.NameSpacePatern, wfp.SearchPatern)
                                )
                            );
                        }
                        WikiData wd = new WikiData();
                        wd.MergeWFI(wfi);
                        wd.FileContent = File.ReadAllBytes(wd.FilePath);
                        if (
                            (wd.FileContent == null) ||
                            (wd.FileContent.Length == 0)
                           )
                        {
                            throw new WikiEngineInternalSearchEmptyException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorGetResource,
                                    Properties.ResourceWikiEngine.txtErrorFileEmpty,
                                    string.Concat(wfp.NameSpacePatern, wfp.SearchPatern)
                                )
                            );
                        }
                        wd.FileContentString = String.Empty;
                        return wd;
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        return null;
                    }
                });

                t1.Wait();

                WikiData wdo = t1.Result;
                this._TaskEnd(t1, wdo,
                    ((wfp != null) ?
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorTaskEnd,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern
                        ) :
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    )
                );
                return wdo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region method GetFileToString
        /// <summary>
        /// Get file content as string from wiki namespace
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="wft"><see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType, return in namespace, is found</param>
        /// <returns>WikiEngine.WikiData</returns>
        public WikiData GetFileToString(string namesspace, WikiFileType wft = WikiFileType.None)
        {
            try
            {
                WikiData wd = null;
                if ((wd = this.GetFile(namesspace, wft)) == null)
                {
                    throw new WikiEngineInternalSearchEmptyException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorGetResource,
                            Properties.ResourceWikiEngine.txtErrorWikiDataEmpty,
                            namesspace
                        )
                    );
                }
                if (
                    (wd.FileContent == null) ||
                    (wd.FileContent.Length == 0)
                   )
                {
                    throw new WikiEngineInternalSearchEmptyException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorGetResource,
                            Properties.ResourceWikiEngine.txtErrorWikiContentEmpty,
                            namesspace
                        )
                    );
                }
                wd.FileContentString = Encoding.UTF8.GetString(wd.FileContent);
                if (string.IsNullOrWhiteSpace(wd.FileContentString))
                {
                    throw new WikiEngineInternalSearchErrorException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorGetResource,
                            Properties.ResourceWikiEngine.txtErrorWikiContentConvert,
                            namesspace
                        )
                    );
                }
                return wd;
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                return null;
            }
        }

        #endregion

        #region method GetFileFromAttic
        /// <summary>
        /// See <see cref="WikiEngine.WikiFile.GetFileFromAttic(string, string)"/> WikiFile.GetFileFromAttic(string, string)
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="uts">Unix Unix TimeStamp, Int32</param>
        /// <returns></returns>
        public WikiData GetFileFromAttic(string namesspace, Int32 uts)
        {
            return this.GetFileFromAttic(
                namesspace,
                uts.ToString()
            );
        }
        /// <summary>
        /// See <see cref="WikiEngine.WikiFile.GetFileFromAttic(string, string)"/> WikiFile.GetFileFromAttic(string, string)
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="dt">DateTime format</param>
        /// <returns></returns>
        public WikiData GetFileFromAttic(string namesspace, DateTime dt)
        {
            return this.GetFileFromAttic(
                namesspace,
                DokuUtil.GetUnixTimeStamp(dt).ToString()
            );
        }
        /// <summary>
        /// Get file content from Attic backup (wiki namespace required)
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="stime">string date and time</param>
        /// <returns>WikiEngine.WikiData</returns>
        public WikiData GetFileFromAttic(string namesspace, string stime = null)
        {
            try
            {
                WikiFileParse wfp = null;
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                        WikiFileType.FileReadAttic, namesspace, null, null, true
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                }
                return this._GetFileFromAttic(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private WikiData _GetFileFromAttic(WikiFileParse wfp)
        {
            try
            {
                Task<WikiData> t1 = Task<WikiData>.Factory.StartNew(() =>
                {
                    try
                    {
                        WikiFileInfo wfi = null;

                        if (wfp == null)
                        {
                            throw new WikiEngineInternalNameSpaceErrorException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                                    MethodBase.GetCurrentMethod().Name
                                )
                            );
                        }
                        if ((wfi = (WikiFileInfo)this._WikiFileGetFileInfo(wfp)) == null)
                        {
                            throw new WikiEngineInternalSearchEmptyException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorGetResource,
                                    Properties.ResourceWikiEngine.txtErrorAtticEmpty,
                                    string.Concat(wfp.NameSpacePatern, wfp.SearchPatern)
                                )
                            );
                        }
                        WikiData wd = new WikiData();
                        wd.MergeWFI(wfi);
                        wd.FileContent = DokuUtil.AtticDeCompressFile(wd.FilePath);
                        if (
                            (wd.FileContent == null) ||
                            (wd.FileContent.Length == 0)
                           )
                        {
                            throw new WikiEngineInternalSearchEmptyException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorGetResource,
                                    Properties.ResourceWikiEngine.txtErrorAtticEmpty,
                                    string.Concat(wfp.NameSpacePatern, wfp.SearchPatern)
                                )
                            );
                        }
                        wd.FileContentString = String.Empty;
                        return wd;
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        return null;
                    }
                });

                t1.Wait();

                WikiData wdo = t1.Result;
                this._TaskEnd(t1, wdo,
                    ((wfp != null) ?
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorTaskEnd,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern
                        ) :
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    )
                );
                return wdo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region public GetFileMeta
        /// <summary>
        /// Get Meta data from file, return List <see cref="WikiEngine.WikiMetaChanges"/>WikiEngine.WikiMetaChanges
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <returns>List&lt;WikiEngine.WikiMetaChanges&gt;</returns>
        public List<WikiMetaChanges> GetFileMeta(string namesspace)
        {
            try
            {
                WikiFileParse wfp = null;
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                        WikiFileType.FileReadMeta, namesspace, null, null, true
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                }
                return this._GetFileMeta(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private List<WikiMetaChanges> _GetFileMeta(WikiFileParse wfp)
        {
            try
            {
                Task<List<WikiMetaChanges>> t1 = Task<List<WikiMetaChanges>>.Factory.StartNew(() =>
                {
                    try
                    {
                        WikiFileInfo wfi = null;

                        if (wfp == null)
                        {
                            throw new WikiEngineInternalNameSpaceErrorException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                                    MethodBase.GetCurrentMethod().Name
                                )
                            );
                        }
                        if ((wfi = (WikiFileInfo)this._WikiFileGetFileInfo(wfp)) == null)
                        {
                            throw new WikiEngineInternalSearchEmptyException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorGetResource,
                                    Properties.ResourceWikiEngine.txtErrorMetaEmpty,
                                    string.Concat(wfp.NameSpacePatern, wfp.SearchPatern)
                                )
                            );
                        }
                        List<WikiMetaChanges> lwmc = new List<WikiMetaChanges>();
                        WikiData wd = new WikiData();
                        wd.MergeWFI(wfi);
                        using (var reader = File.OpenText(wd.FilePath))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                string[] part = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                if (part.Length == 7)
                                {
                                    WikiMetaChanges twmc = new WikiMetaChanges()
                                    {
                                        AuthorIp = part[1],
                                        Mode = part[2],
                                        NameSpace = part[3],
                                        Author = part[4],
                                        Title = part[5],
                                        Size = Convert.ToInt32(part[6], CultureInfo.InvariantCulture),
                                        UnixTimeStamp = Convert.ToInt32(part[0], CultureInfo.InvariantCulture)
                                    };
                                    lwmc.Add(twmc);
                                }
                            }
                        }
                        if (lwmc.Count == 0)
                        {
                            throw new WikiEngineMetaException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorGetResource,
                                    Properties.ResourceWikiEngine.txtErrorMetaEmpty,
                                    string.Concat(wfp.NameSpacePatern, wfp.SearchPatern)
                                )
                            );
                        }
                        return lwmc;
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        return null;
                    }
                });

                t1.Wait();

                List<WikiMetaChanges> wmc = t1.Result;
                this._TaskEnd(t1, wmc,
                    ((wfp != null) ?
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorTaskEnd,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern
                        ) :
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    )
                );
                return wmc;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region method PutFile
        /// <summary>
        /// Put file content to wiki namespace
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="data">byte [] data to wiki file</param>
        /// <param name="wfm">Meta data <see cref="WikiEngine.WikiFileMeta"/>WikiEngine.WikiFileMeta</param>
        /// <returns>WikiEngine.WikiData</returns>
        public WikiData PutFile(string namesspace, byte[] data, WikiFileMeta wfm = null)
        {
            try
            {
                WikiFileParse wfp = null;
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                        WikiFileType.None, namesspace, null, null, true
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                }
                return this._PutFile(wfp, data, wfm);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private WikiData _PutFile(WikiFileParse wfp, byte[] data, WikiFileMeta wfm = null)
        {
            try
            {
                Task<WikiData> t1 = Task<WikiData>.Factory.StartNew(() =>
                {
                    WikiData wd = new WikiData();

                    try
                    {
                        WikiFileInfo wfi = null;
                        wd.FileContent = default(byte[]);
                        wd.FileContentString = String.Empty;
                        wd.NsParsed = null;

                        try
                        {
                            if (wfp == null)
                            {
                                throw new WikiEngineInternalNameSpaceErrorException("");
                            }
                            if ((wfi = this.__WikiFileGetFileInfoSelect(wfp)) == null)
                            {
                                throw new WikiEngineInternalSearchEmptyException();
                            }
                            if (wfi.FileLock.AddMinutes(5) > DateTime.Now)
                            {
                                throw new WikiEngineInternalLockFile();
                            }
                            throw new WikiEngineInternalSearchOkException();
                        }
                        catch (WikiEngineInternalNameSpaceErrorException e)
                        {
                            throw e;
                        }
                        catch (WikiEngineInternalSearchOkException)
                        {
                            wd.MergeWFI(wfi, wfp);
                        }
                        catch (WikiEngineInternalSearchEmptyException)
                        {
                            wd.MergeWFT(wfp, this._rootPath);
                        }
                        catch (WikiEngineInternalSearchErrorException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                        catch (WikiEngineInternalSearchManyResultException e)
                        {
                            this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        }
                        catch (WikiEngineInternalLockFile e)
                        {
                            throw e;
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        if (wfm == null)
                        {
                            wfm = new WikiFileMeta("LocalAuthor", "127.0.0.1");
                        }
                        wfm.InputContent = (((data == null) || (data.Length == 0)) ? wfm.InputContent : data);
                        wfm.Data = wd;
                        wfm.MergeData();

                        if (wd.IsFileNew)
                        {
                            if (wfm.IsDataEmpty)
                            {
                                /// TODO: no warning! delete file
                                /// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                throw new WikiEnginePutException(
                                    string.Format(
                                        Properties.ResourceWikiEngine.fmtErrorGetResource,
                                        Properties.ResourceWikiEngine.txtErrorWikiDataEmpty,
                                        wfp.NameSpacePatern
                                    )
                                );
                            }
                            if (!Directory.Exists(Path.GetDirectoryName(wd.FilePath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(wd.FilePath));
                            }
                        }
                        else
                        {
                            if (!this._SetFileLock(wd, true))
                            {
                                throw new WikiEnginePutException(
                                    string.Format(
                                        Properties.ResourceWikiEngine.fmtErrorSetLock,
                                        wd.NameSpace,
                                        wd.FileName
                                    )
                                );
                            }
                            if (wfm.IsDataEmpty)
                            {
                                // Delete this file
                                if (File.Exists(wd.FilePath))
                                {
                                    File.Delete(wd.FilePath);
                                }
                                throw new WikiEngineInternalSearchOkException();
                            }
                            if (File.Exists(wd.FilePath))
                            {
                                try
                                {
                                    DokuUtil.AtticCompressFile(wd.FilePath, wd.AtticPath);
                                }
                                catch (Exception e)
                                {
                                    WikiEngineInternalCacheExceptionAtticException ex =
                                        new WikiEngineInternalCacheExceptionAtticException(
                                            string.Format(
                                                Properties.ResourceWikiEngine.fmtErrorGetResource,
                                                wfp.NameSpacePatern,
                                                e.Message
                                            )
                                        );
                                    this.Fire_ProcessError(new WikiErrorEventArgs(ex, MethodBase.GetCurrentMethod().Name));
                                }
                            }
                        }
                        File.WriteAllBytes(wd.FilePath, wfm.InputContent);
                        this._WriteMetaFile(wfm);
                        if ((!wd.IsFileNew) && (!this._SetFileLock(wd, false)))
                        {
                            throw new WikiEnginePutException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorSetLock,
                                    wd.NameSpace,
                                    wd.FileName
                                )
                            );
                        }
                        throw new WikiEngineInternalSearchOkException();
                    }
                    catch (WikiEngineInternalSearchOkException)
                    {
                        this._isFsModify = true;
                        return wd;
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        throw new WikiEnginePutException(e.Message);
                    }
                });

                t1.Wait();

                WikiData wdo = t1.Result;
                this._TaskEnd(t1, wdo,
                    ((wfp != null) ?
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorTaskEnd,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern
                        ) :
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    )
                );
                return wdo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region method PutFileHtmlText
        /// <summary>
        /// Put file content to wiki namespace,
        /// input source is HTML fragment, output is Markdown
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="data">byte [] data to wiki file</param>
        /// <param name="wfm">Meta data <see cref="WikiEngine.WikiFileMeta"/>WikiEngine.WikiFileMeta</param>
        /// <returns>WikiEngine.WikiData</returns>
        public WikiData PutFileHtmlText(string namesspace, byte[] data, WikiFileMeta wfm = null)
        {
            return this._PutFileHtml(false, namesspace, data, wfm);
        }
        #endregion

        #region method PutFileHtmlDoc
        /// <summary>
        /// Put file content to wiki namespace,
        /// input source is completed HTML document, output is Markdown
        /// </summary>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="data">byte [] data to wiki file</param>
        /// <param name="wfm">Meta data <see cref="WikiEngine.WikiFileMeta"/>WikiEngine.WikiFileMeta</param>
        /// <returns>WikiEngine.WikiData</returns>
        public WikiData PutFileHtmlDoc(string namesspace, byte[] data, WikiFileMeta wfm = null)
        {
            return this._PutFileHtml(true, namesspace, data, wfm);
        }
        #endregion

        #region methods GetFileList
        /// <summary>
        /// Get file list
        /// </summary>
        /// <example></example>
        /// <code>
        ///   WikiFolderInfo wfi = wf.GetFileList(WikiEngine.WikiFileType.FileReadMd,"testns:");
        ///   if (wfi == null)
        ///   {
        ///     Console.WriteLine("return empty..");
        ///     return;
        ///   }
        ///   Console.WriteLine(
        ///     "total directories: " + wfi.Dirs.Count + Environment.NewLine +
        ///     "total files: " + wfi.Files.Count
        ///   );
        ///   foreach (var items in wfi.Files)
        ///   {
        ///      Console.WriteLine(" name space: " + items.Key);
        ///      foreach (var item in items.Value)
        ///      {
        ///         Console.WriteLine("   page/file: " + item.FileName);
        ///      }
        ///   }
        /// </code>
        /// <param name="wft">object type: enum <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</param>
        /// <param name="namesspace">Wiki name space</param>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo GetFileList(WikiFileType wft, string namesspace)
        {
            try
            {
                WikiFileParse wfp = null;
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                        wft, namesspace, null, null, false
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                }
                return this._GetFileList(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private WikiFolderInfo _GetFileList(WikiFileParse wfp)
        {
            try
            {
                Task<WikiFolderInfo> t1 = Task<WikiFolderInfo>.Factory.StartNew(() =>
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
                        return this.__WikiFilesFindNamespace(wfp);
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        return null;
                    }
                });
                
                t1.Wait();

                WikiFolderInfo wfi = t1.Result;
                this._TaskEnd(t1, wfi,
                    ((wfp != null) ?
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorTaskEnd,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern
                        ) :
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    )
                );
                return wfi;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get Root page file list
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.GetFileList"/>WikiEngine.GetFileList()</example>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo GetFileListPages()
        {
            return this.GetFileList(WikiFileType.FileReadMd, this._defaultNameSpace);
        }

        /// <summary>
        /// Get Root media file list
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.GetFileList"/>WikiEngine.GetFileList()</example>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo GetFileListMedia()
        {
            return this.GetFileList(WikiFileType.FileReadBinary, this._defaultNameSpace);
        }

        /// <summary>
        /// Get Root attic file list
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.GetFileList"/>WikiEngine.GetFileList()</example>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo GetFileListAttic()
        {
            return this.GetFileList(WikiFileType.FileReadAttic, this._defaultNameSpace);
        }
        #endregion

        #region methods FindFiles
        /// <summary>
        /// Find wiki files, return data:
        /// class <see cref="WikiEngine.WikiFolderInfo"/>WikiFolderInfo
        /// class <see cref="WikiEngine.WikiFileInfo"/>WikiFileInfo
        /// for file/page name mask
        /// </summary>
        /// <code>
        ///   WikiFolderInfo wfi = wf.FindFiles(WikiEngine.WikiFileType.FileReadMd,"myfilename","mynamespace:");
        ///   if (wfi == null)
        ///   {
        ///      return;
        ///   }
        ///   Console.WriteLine("find pattern: " + wfi.SearchPatern);
        ///   Console.WriteLine("start name space: " + wfi.NamespacePath);
        ///   foreach (var items in (Dictionary&lt;string, WikiFolderInfo&gt;)wfi.Dirs)
        ///   {
        ///      Console.WriteLine(" name space: " + items.Key);
        ///      foreach (WikiFileInfo item in ((WikiFolderInfo)items.Value).Files)
        ///      {
        ///         Console.WriteLine("   page/media name: " + item.FileName);
        ///      }
        ///   }
        /// </code>
        /// <param name="type">object type: enum <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType</param>
        /// <param name="search">String search pattern</param>
        /// <param name="namesspace">String Namespace (optionale)</param>
        /// <param name="strong">Bolean, is true search stop is one matching (optionale)</param>
        /// <returns>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo FindFiles(WikiFileType type, string search, string namesspace = null, bool strong = false)
        {
            try
            {
                WikiFileParse wfp = null;
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                        type, namesspace, search, null, strong
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                }
                return this._FindFiles(wfp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private WikiFolderInfo _FindFiles(WikiFileParse wfp)
        {
            try
            {
                Task<WikiFolderInfo> t1 = Task<WikiFolderInfo>.Factory.StartNew(() =>
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
                        return this.__WikiFilesFindFiles(wfp);
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        return null;
                    }
                });

                t1.Wait();

                WikiFolderInfo wfo = t1.Result;
                this._TaskEnd(t1, wfo,
                    ((wfp != null) ?
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorTaskEnd,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.NameSpacePatern,
                            wfp.SearchPatern
                        ) :
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorWikiFileParse,
                            MethodBase.GetCurrentMethod().Name
                        )
                    )
                );
                return wfo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Find wiki page files <see cref="WikiEngine.WikiFileInfo"/> for file name mask
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.FindFiles"/>WikiEngine.WikiFile.FindFiles()</example>
        /// <param name="search">Wiki page/file name mask</param>
        /// <param name="strong">Bolean, strong search if true</param>
        /// <returns>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo FindFilesPage(string search, bool strong = false)
        {
            return this.FindFiles(WikiFileType.FileReadMd, search, null, strong);
        }

        /// <summary>
        /// Get wiki media file <see cref="WikiEngine.WikiFileInfo"/> media
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.FindFiles"/>WikiEngine.WikiFile.FindFiles()</example>
        /// <param name="search">Wiki media/file name mask</param>
        /// <param name="strong">Bolean, strong search if true</param>
        /// <returns>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo FindFilesMedia(string search, bool strong = false)
        {
            return this.FindFiles(WikiFileType.FileReadBinary, search, null, strong);
        }

        /// <summary>
        /// Get wiki attic file <see cref="WikiEngine.WikiFileInfo"/> iformation
        /// </summary>
        /// <example>Example view code: <see cref="WikiEngine.WikiFile.FindFiles"/>WikiEngine.WikiFile.FindFiles()</example>
        /// <param name="search">Wiki archive/file name mask</param>
        /// <param name="strong">Bolean, strong search if true</param>
        /// <returns>WikiEngine.WikiFolderInfo</returns>
        public WikiFolderInfo FindFilesAttic(string search, bool strong = false)
        {
            return this.FindFiles(WikiFileType.FileReadAttic, search, null, strong);
        }

        #endregion

        #region methods LockFile
        /// <summary>
        /// Lock/Unlock page file
        /// See <see cref="WikiFile.LockFile(WikiFileType,string,string,bool)"/> WikiFile.LockFile(WikiFileType,string,string,bool)
        /// </summary>
        /// <param name="namesspace">wiki namespace include page name</param>
        /// <param name="status">Lock = true, Unlock = false</param>
        /// <returns>Bolean, true is succesful</returns>
        public bool LockFilePage(string namesspace, bool status)
        {
            return this.LockFile(WikiFileType.FileReadMd, namesspace, null, status);
        }
        /// <summary>
        /// Lock/Unlock media file
        /// See <see cref="WikiFile.LockFile(WikiFileType,string,string,bool)"/> WikiFile.LockFile(WikiFileType,string,string,bool)
        /// </summary>
        /// <param name="namesspace">wiki namespace include media file name</param>
        /// <param name="status">Lock = true, Unlock = false</param>
        /// <returns>Bolean, true is succesful</returns>
        public bool LockFileMedia(string namesspace, bool status)
        {
            return this.LockFile(WikiFileType.FileReadBinary, namesspace, null, status);
        }
        /// <summary>
        /// Lock/Unlock file for writing (pages/media)
        /// </summary>
        /// <param name="type">enum WikiFileType</param>
        /// <param name="namesspace">wiki namespace include ':'</param>
        /// <param name="fname">file/page/media name or null</param>
        /// <param name="status">Lock = true, Unlock = false</param>
        /// <returns>Bolean, true is succesful</returns>
        public bool LockFile(WikiFileType type, string namesspace, string fname, bool status)
        {
            try
            {
                Task<bool> t1 = Task<bool>.Factory.StartNew(() =>
                {
                    try
                    {
                        WikiFileParse wftr = new WikiFileParse(type, true, namesspace, fname, null, false);
                        if ((wftr = this.__WikiFilesParse(wftr)) == null)
                        {
                            throw new WikiEngineInternalNameSpaceErrorException(namesspace);
                        }
                        WikiData wd = new WikiData();
                        wd.MergeWFT(wftr, this._rootPath);
                        return this._SetFileLock(wd, status);
                    }
                    catch (Exception e)
                    {
                        this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                        return false;
                    }
                });

                t1.Wait();

                bool res = t1.Result;
                this._TaskEnd(t1, new Object(), ((string.IsNullOrWhiteSpace(fname)) ? namesspace : fname));
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #endregion

        #region private Methods

        #region private _SetFileLock
        private bool _SetFileLock(WikiData wd, bool status)
        {
            try
            {
                if (!wd.IsParsed)
                {
                    WikiEngineInternalNameSpaceErrorException ex =
                        new WikiEngineInternalNameSpaceErrorException(
                            string.Format(
                                "Parsed data is empty: {0}{1}",
                                wd.NameSpace,
                                wd.FileName
                            )
                        );
                    this.Fire_ProcessError(new WikiErrorEventArgs(ex, MethodBase.GetCurrentMethod().Name));
                    return false;
                }
                WikiFolderInfo wfi = this.__WikiFilesFindNamespace(wd.NsParsed);
                if (wfi == null)
                {
                    return true;
                }
                WikiFileAction wfa = new WikiFileAction(wd, status);
                return this._WikiFileActionRecursive(wfi, wfa, this._FilesAction_SetFileLock);
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
                return false;
            }
        }
        /// <summary>
        ///  Call Back to _SetFileLock
        /// </summary>
        /// <param name="wfa">WikiFileAction</param>
        private void _FilesAction_SetFileLock(WikiFileAction wfa)
        {
            if (wfa.FileInfo != null)
            {
                wfa.FileInfo.FileLock = ((wfa.Status) ? DateTime.Now : DateTime.MinValue);
            }
        }
        #endregion

        #region private _WriteMetaFile
        private void _WriteMetaFile(WikiFileMeta wfm)
        {
            try
            {
                bool isCreate = false;
                string metaPath;

                if (
                    (wfm == null) ||
                    (wfm.Data == null) ||
                    (string.IsNullOrWhiteSpace(wfm.Data.FilePath))
                   )
                {
                    throw new WikiEngineMetaException(Properties.ResourceWikiEngine.txtErrorMetaData);
                }
                if (string.IsNullOrWhiteSpace((metaPath = wfm.Data.MetaPath)))
                {
                    throw new WikiEngineMetaException(Properties.ResourceWikiEngine.txtErrorMetaWritePath);
                }
                if (!Directory.Exists(Path.GetDirectoryName(metaPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(metaPath));
                }
                if (!File.Exists(metaPath))
                {
                    isCreate = true;
                }
                WikiMetaChanges wmc = DokuUtil.WikiFileMetaDataMerge(wfm, isCreate);
                File.AppendAllText(
                    metaPath,
                     string.Format(
                        Properties.ResourceWikiEngine.PutPageMeta,
                        "\t",
                        wmc.UnixTimeStamp,
                        wmc.AuthorIp,
                        wmc.Mode,
                        wmc.NameSpace,
                        wmc.Author,
                        wmc.Title,
                        wfm.Data.FileContent.Length,
                        Environment.NewLine
                     ),
                     Encoding.UTF8
                );
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e, MethodBase.GetCurrentMethod().Name));
            }
        }

        #endregion

        #region method _PutFileHtml (private) use: PutFileHtmlDoc, PutFileHtmlText
        private WikiData _PutFileHtml(bool isHtmlDoc, string namesspace, byte[] data, WikiFileMeta wfm = null)
        {
            try
            {
                if (
                    (data == null) ||
                    (data.Length == 0)
                   )
                {
                    throw new WikiEngineInternalSearchEmptyException();
                }
                return this.PutFile(
                    namesspace,
                    Encoding.UTF8.GetBytes(
                        ((isHtmlDoc) ?
                            this.wikiFormat.HtmlTextToDokuWiki(
                                Encoding.UTF8.GetString(data)
                            ) :
                            this.wikiFormat.HtmlTextToDokuWiki(
                                Encoding.UTF8.GetString(data)
                            )
                        )
                    ),
                    wfm
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region private _TaskEnd
        private void _TaskEnd(Task t, object obj, string source)
        {
            try
            {
                if (t == null)
                {
                    return;
                }
                if (t.Exception != null)
                {
                    this.Fire_ProcessError(new WikiErrorEventArgs(t.Exception, MethodBase.GetCurrentMethod().Name));
                    throw t.Exception;
                }
                if ((t.IsFaulted) || (t.IsCanceled) || (obj == null))
                {
                    t.Dispose();
                    throw new TaskCanceledException(source);
                }
                t.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        #endregion

        #endregion
    }
}
