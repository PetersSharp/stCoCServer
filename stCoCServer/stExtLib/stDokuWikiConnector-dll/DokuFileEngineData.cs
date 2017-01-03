using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

using stDokuWiki;
using stDokuWiki.Util;
using stDokuWiki.WikiEngine;
using stDokuWiki.WikiEngine.Exceptions;

namespace stDokuWiki.WikiEngine
{

    #region private enum WikiRequestType
    /// <summary>
    /// Type Wiki Web Reguest
    /// </summary>
    [Serializable]
    public enum WikiRequestType : int
    {
        /// <summary>
        /// None - default
        /// </summary>
        None,
        /// <summary>
        /// Get page/file
        /// </summary>
        Get,
        /// <summary>
        /// Put page/file
        /// </summary>
        Put,
        /// <summary>
        /// Del page/file
        /// </summary>
        Del,
        /// <summary>
        /// List page/file
        /// </summary>
        List,
        /// <summary>
        /// Find page/file
        /// </summary>
        Find
    };

    #endregion

    #region public enum WikiFileType
    /// <summary>
    /// enum Wiki FS file type
    /// </summary>
    [Serializable]
    public enum WikiFileType : int
    {
        /// <summary>
        /// None - not set, default
        /// </summary>
        [Description("none")]
        None,
        /// <summary>
        /// Read Media or binary file
        /// </summary>
        [Description("media")]
        FileReadBinary,
        /// <summary>
        /// Read Page MarkDown file
        /// </summary>
        [Description("pages")]
        FileReadMd,
        /// <summary>
        /// Read Attic recent changes file
        /// </summary>
        [Description("attic")]
        FileReadAttic,
        /// <summary>
        /// Read meta file
        /// </summary>
        [Description("meta")]
        FileReadMeta,
        /// <summary>
        /// Write Media or binary file
        /// </summary>
        [Description("media")]
        FileWriteBinary,
        /// <summary>
        /// Write Page MarkDown file
        /// </summary>
        [Description("pages")]
        FileWriteMd,
        /// <summary>
        /// Write Attic recent changes file
        /// </summary>
        [Description("attic")]
        FileWriteAttic,
        /// <summary>
        /// Write meta file
        /// </summary>
        [Description("meta")]
        FileWriteMeta,
        /// <summary>
        /// File type Unknown
        /// </summary>
        [Description("unknown")]
        FileUnknown,
        /// <summary>
        /// File type is NameSpace
        /// </summary>
        [Description("unknown")]
        NameSpace
    };

    #endregion

    #region public class WikiFileMeta
    /// <summary>
    /// Wiki file Meta class
    /// </summary>
    [DebuggerDisplay("{Author},{AuthorIp} => {IsAuth} => {Data}")]
    public class WikiFileMeta
    {
        /// <summary>
        /// Is auth users, default false
        /// </summary>
        public bool IsAuth { get; set; }
        /// <summary>
        /// Author name
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Author IP
        /// </summary>
        public string AuthorIp { get; set; }
        /// <summary>
        /// Class <see cref="WikiEngine.WikiData"/>WikiEngine.WikiData
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public WikiData Data { get; set; }
        /// <summary>
        /// Wiki input file content as byte []
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public byte[] InputContent
        {
            get { return _InputContent; }
            set
            {
                if (value != null)
                {
                    _InputContent = value;
                }
            }
        }
        /// <summary>
        /// Wiki input file content as String
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public string InputContentString
        {
            get
            {
                if (IsDataEmpty)
                {
                    return String.Empty;
                }
                return Encoding.UTF8.GetString(InputContent);
            }
        }
        /// <summary>
        /// Test empty Data.FileContent (readonly)
        /// </summary>
        public bool IsDataEmpty
        {
            get
            {
                if (
                    (InputContent == null) ||
                    (InputContent.Length == 0)
                   )
                {
                    return true;
                }
                return false;
            }
        }

        private byte[] _InputContent { get; set; }

        /// <summary>
        /// Constructor WikiFileMeta class
        /// </summary>
        /// <param name="author">Author name</param>
        /// <param name="authorip">Author IP</param>
        public WikiFileMeta(string author, string authorip, byte [] context = null)
        {
            this.Author = author;
            this.AuthorIp = authorip;
            this.IsAuth = false;
            this._InputContent = null;
            InputContent = context;
        }
        /// <summary>
        /// Merge WikiData class
        /// </summary>
        public void MergeData()
        {
            if (!IsDataEmpty)
            {
                Data.FileContent = InputContent;
            }
        }
    }
    #endregion

    #region public class WikiData
    /// <summary>
    /// Wiki request file class
    /// </summary>
    [DebuggerDisplay("{NameSpace} -> {FileName}.{FileExt} -> {FileType} => {ChangeDate} => {FileContent.Length}")]
    public class WikiData
    {
        /// <summary>
        /// Wiki name space
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// Wiki full file path
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Wiki request file name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// Wiki request file extension
        /// </summary>
        public string FileExt { get; set; }
        /// <summary>
        /// Wiki request file content as string
        /// </summary>
        public string FileContentString { get; set; }
        /// <summary>
        /// Wiki request file content as byte []
        /// </summary>
        public byte[] FileContent { get; set; }
        /// <summary>
        /// Author to last change
        /// </summary>
        public string ChangeAuthor { get; set; }
        /// <summary>
        /// Author IP from change
        /// </summary>
        public string ChangeIp { get; set; }
        /// <summary>
        /// Change Date and Time
        /// </summary>
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// Wiki request file type <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType
        /// </summary>
        public WikiFileType FileType { get; set; }
        /// <summary>
        /// is file Create mode, not exist
        /// </summary>
        public bool IsFileNew { get; set; }
        /// <summary>
        /// class WikiFileParse
        /// </summary>
        internal WikiFile.WikiFileParse NsParsed { get; set; }
        /// <summary>
        /// Wiki meta file path
        /// </summary>
        public string MetaPath
        {
            get
            {
                return DokuUtil.WikiFilePathRewrite(
                    this.FileType,
                    this.FilePath,
                    WikiFile.wikiLocalMeta,
                    string.Format(
                        ".{0}",
                        WikiFile.metaExtension
                    )
                );
            }
        }
        /// <summary>
        /// Wiki attic file path
        /// </summary>
        public string AtticPath
        {
            get
            {
                this.ChangeDate = DateTime.Now;

                return DokuUtil.WikiFilePathRewrite(
                    this.FileType,
                    this.FilePath.Substring(0, (this.FilePath.Length - this.FileExt.Length - 1)),
                    WikiFile.wikiLocalAttic,
                    string.Format(
                        ".{0}.{1}.{2}",
                        DokuUtil.GetUnixTimeStamp(this.ChangeDate),
                        this.FileExt,
                        WikiFile.atticExtension
                    )
                );
            }
        }
        /// <summary>
        /// Check WikiFileTransfer, empty or no
        /// </summary>
        public bool IsParsed
        {
            get { return (this.NsParsed != null); }
        }

        /// <summary>
        /// Merge data WikiFileInfo to WikiData
        /// </summary>
        /// <param name="wfi">WikiFileInfo</param>
        internal void MergeWFI(WikiFileInfo wfi, WikiFile.WikiFileParse wfp = null)
        {
            if (wfi == null)
            {
                return;
            }
            this.FileType = wfi.FileType;
            this.FileExt = wfi.FileExt;
            this.FileName = wfi.FileName;
            this.FilePath = wfi.FilePath;
            this.NameSpace = wfi.NameSpace;
            this.IsFileNew = false;
            this.NsParsed = ((wfp == null) ? this.NsParsed : wfp);
        }
        /// <summary>
        /// Merge data WikiFile.WikiFileParse to WikiData
        /// </summary>
        /// <param name="wfp">internal WikiFile.WikiFileParse</param>
        /// <param name="path">root path</param>
        internal void MergeWFT(WikiFile.WikiFileParse wfp, string path)
        {
            if (wfp == null)
            {
                return;
            }

            this.FileType = wfp.FolderType;
            if (string.IsNullOrEmpty(wfp.SearchPatern))
            {
                this.FileExt = __getFileExt(wfp.FolderType);
                this.FileName = string.Format(
                    "{0}_{1}{2}",
                    DokuUtil.WikiFileTypeToString(wfp.FolderType),
                    WikiFile.wikiDefaultEmptyNS, ".",
                    this.FileExt
                );
            }
            else
            {
                this.FileName = wfp.SearchPatern;
                this.FileExt = Path.GetExtension(this.FileName);
                if (string.IsNullOrEmpty(this.FileExt))
                {
                    this.FileExt = __getFileExt(wfp.FolderType);
                    this.FileName = string.Concat(
                        this.FileName, ".",
                        this.FileExt
                    );
                }
            }
            this.NameSpace = wfp.NameSpacePatern;
            this.FilePath = Path.Combine(path, DokuUtil.WikiFileTypeToString(wfp.FolderType));
            if (wfp.UriPart.Count > 0)
            {
                wfp.UriPart.ForEach(o =>
                {
                    this.FilePath = Path.Combine(this.FilePath, o);
                });
            }
            this.FilePath = Path.Combine(
                this.FilePath,
                this.FileName
            );
            this.FileName = Path.GetFileNameWithoutExtension(this.FileName);
            this.IsFileNew = true;
        }
        private string __getFileExt(WikiFileType wft)
        {
            return ((wft == WikiFileType.FileWriteMd) ?
                WikiFile.mdExtension :
                WikiFile.binExtension
            );
        }
    }
    #endregion

    #region public class WikiFolderInfo (ICloneable)
    /// <summary>
    /// Wiki file system folder
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{SearchPatern} => {Dirs} -> {Files}")]
    public class WikiFolderInfo : ICloneable
    {
        /// <summary>
        /// Folder data
        /// </summary>
        //[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public Dictionary<string, WikiFolderInfo> Dirs { get; set; }
        /// <summary>
        /// File data
        /// </summary>
        //[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public List<WikiFileInfo> Files { get; set; }
        /// <summary>
        /// Namespace search patern
        /// (page,media,attic,meta - file name)
        /// return value from _GetFileList()
        /// </summary>
        public string SearchPatern { get; set; }
        /// <summary>
        /// Full path namespace: main:example:info:text:
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// Search Strong
        /// if true use Equals, false use Contains
        /// </summary>
        public bool SearchStrong { get; set; }

        /// <summary>
        /// Constructor WikiFolderInfo
        /// </summary>
        public WikiFolderInfo()
        {
            Dirs = new Dictionary<string, WikiFolderInfo>();
            Files = new List<WikiFileInfo>();
            SearchPatern = String.Empty;
            SearchStrong = false;
        }
        /// <summary>
        /// Clone this
        /// </summary>
        /// <returns>(object) WikiFolderInfo</returns>
        public object Clone()
        {
            return (object)this.MemberwiseClone();
        }
    }
    #endregion

    #region public class WikiFileInfo (ICloneable)
    /// <summary>
    /// Wiki file Info class
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{FileName} {FileExt} => {TimeWrite} / {TimeAccess} / {TimeAdd} => {FileLock}")]
    public class WikiFileInfo : ICloneable
    {
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// File extension
        /// </summary>
        public string FileExt { get; set; }
        /// <summary>
        /// Full file path
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Last add to dictionary time
        /// </summary>
        public DateTime TimeAdd { get; set; }
        /// <summary>
        /// Last write time
        /// </summary>
        public DateTime TimeWrite { get; set; }
        /// <summary>
        /// Last access time
        /// </summary>
        public DateTime TimeAccess { get; set; }
        /// <summary>
        /// Date and time Lock file
        /// </summary>
        public DateTime FileLock { get; set; }
        /// <summary>
        /// File type <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType
        /// </summary>
        public WikiFileType FileType { get; set; }
        /// <summary>
        /// Full path namespace: main:example:info:text:
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// Clone this
        /// </summary>
        /// <returns>(object) WikiFileInfo</returns>
        public object Clone()
        {
            return (object)this.MemberwiseClone();
        }
    }
    #endregion

    #region public class WikiFileAction
    /// <summary>
    /// Wiki file Action class
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{NameSpace}.{Search}.{Key} => {Status} / {ConutNameSpace}:{ConutFile}:{ConutDirs}:{ConutFiles} => {FileInfo}")]
    public class WikiFileAction
    {
        /// <summary>
        /// Key NameSpace
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Full Namespace path
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// Search patern
        /// </summary>
        public string Search { get; internal set; }
        /// <summary>
        /// Last Namespace
        /// </summary>
        public string LastNameSpace { get; set; }
        /// <summary>
        /// Bolean, status to set
        /// </summary>
        public bool Status { get; internal set; }
        /// <summary>
        /// Bolean, process all files
        /// </summary>
        public bool IsAllFiles { get; internal set; }
        /// <summary>
        /// Count current Namespace
        /// </summary>
        public Int32 ConutNameSpace { get; set; }
        /// <summary>
        /// Count current file
        /// </summary>
        public Int32 ConutFile { get; set; }
        /// <summary>
        /// Count current all derictories
        /// </summary>
        public Int32 ConutDirs { get; set; }
        /// <summary>
        /// Count current all files
        /// </summary>
        public Int32 ConutFiles { get; set; }
        /// <summary>
        /// FileInfo class
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public WikiFileInfo FileInfo { get; set; }
        /// <summary>
        /// String Builder instance
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public StringBuilder SBuilder { get; set; }
        /// <summary>
        /// StringBulder create or exist
        /// (return mode)
        /// </summary>
        public bool IsSBuilder { get; internal set; }

        /// <summary>
        /// Constructor WikiFileAction
        /// </summary>
        /// <param name="namesspace">full Namespace path</param>
        /// <param name="search">search string</param>
        /// <param name="isallfiles">process all files</param>
        /// <param name="status">set status</param>
        /// <param name="sb">String Builder instance</param>
        public WikiFileAction(string namesspace, string search, bool isallfiles = false, bool status = false, StringBuilder sb = null)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                throw new WikiEngineInternalStructErrorException(
                    string.Format(
                        Properties.ResourceWikiEngine.fmtErrorGetResource,
                        Properties.ResourceWikiEngine.txtErrorSearchEmpty,
                        this.GetType().Name
                    )
                );
            }
            Status = status;
            IsAllFiles = isallfiles;
            ConutNameSpace = ConutFile = ConutDirs = ConutFiles = 0;
            Search = search;
            Key = String.Empty;
            LastNameSpace = String.Empty;
            NameSpace = DokuUtil.WikiFileNameSpaceNormalize(namesspace);
            IsSBuilder = (sb != null);
            SBuilder = sb;
        }
        /// <summary>
        /// Constructor WikiFileAction
        /// </summary>
        /// <param name="wd">class WikiData</param>
        /// <param name="status">set status</param>
        /// <param name="sb">String Builder instance</param>
        public WikiFileAction(WikiData wd, bool status = false, StringBuilder sb = null)
        {
            Key = String.Empty;
            Status = status;
            IsAllFiles = false;
            LastNameSpace = String.Empty;
            ConutNameSpace = ConutFile = ConutDirs = ConutFiles = 0;
            if (wd == null)
            {
                Search = String.Empty;
                NameSpace = ":";
            }
            else
            {
                Search = wd.FileName;
                NameSpace = ((string.IsNullOrWhiteSpace(wd.NameSpace)) ? ":" : wd.NameSpace);
            }
            IsSBuilder = (sb != null);
            SBuilder = sb;
        }
        /// <summary>
        /// Constructor WikiFileAction
        /// create StringBuilder if exist
        /// </summary>
        /// <param name="sb">String Builder instance</param>
        public WikiFileAction(StringBuilder sb) : this(null, false, sb)
        {
            _CheckStringBuilder(sb);
        }
        /// <summary>
        /// Constructor WikiFileAction
        /// </summary>
        /// <param name="wd">class WikiData</param>
        /// <param name="status">set status</param>
        public WikiFileAction(WikiData wd, bool status = false) : this(wd, status, null)            
        {
            if (
                (wd == null) ||
                (string.IsNullOrWhiteSpace(wd.FileName))
               )
            {
                throw new WikiEngineInternalStructErrorException(
                    string.Format(
                        Properties.ResourceWikiEngine.fmtErrorGetResource,
                        Properties.ResourceWikiEngine.txtErrorSearchEmpty,
                        this.GetType().Name
                    )
                );
            }
        }
        internal void _CheckStringBuilder(StringBuilder sb)
        {
            try
            {
                IsSBuilder = (sb != null);
                SBuilder = ((IsSBuilder) ? sb : new StringBuilder());
            }
            catch (Exception e)
            {
                throw new WikiEngineInternalStructErrorException(
                    string.Format(
                        Properties.ResourceWikiEngine.fmtErrorGetResource,
                        e.Message,
                        this.GetType().Name
                    )
                );
            }
        }
    }
    #endregion

    #region public class WikiFileAttic
    /// <summary>
    /// Wiki file Attic class
    /// </summary>
    [DebuggerDisplay("{NameSpace} -> {ShortName} => {DateTimeStamp} => {FileInfo}")]
    public class WikiFileAttic
    {
        /// <summary>
        /// Root directory name
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// Page name / short file name
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Unix timestamp
        /// </summary>
        public DateTime DateTimeStamp { get; set; }
        /// <summary>
        /// FileInfo class
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public WikiFileInfo FileInfo { get; set; }
    }
    #endregion

    #region public class WikiMetaChanges
    /// <summary>
    /// Wiki file Meta .changes class
    /// </summary>
    [DebuggerDisplay("{Author},{AuthorIp} => {DateTimeStamp}/{Mode}/{Size} -> {NameSpace} -> {Title}")]
    public class WikiMetaChanges
    {
        /// <summary>
        /// Unix Time stamp
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public Int32 UnixTimeStamp = 0;
        /// <summary>
        /// DateTime Time stamp
        /// </summary>
        public DateTime DateTimeStamp
        {
            get { return Util.DokuUtil.GetDateTimeFromUnixTimeStamp(UnixTimeStamp); }
            set { UnixTimeStamp = Util.DokuUtil.GetUnixTimeStamp(value); }
        }
        /// <summary>
        /// Author IP
        /// </summary>
        public string AuthorIp { get; set; }
        /// <summary>
        /// Mode: 'C'reate,'E'dit or 'D'elete
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// Namespace include page/file name
        /// </summary>
        public string NameSpace { get; set; }
        /// <summary>
        /// Author name
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Short Title, create automaticaly
        /// </summary>
        public string Title  { get; set; }
        /// <summary>
        /// Page/File body size
        /// </summary>
        public Int32 Size { get; set; }
    }
    #endregion

    #region Event: public class WikiErrorEventArgs (EventArgs)
    /// <summary>
    /// Event Arguments Wiki Error
    /// </summary>
    public class WikiErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Exception
        /// </summary>
        public Exception ex { get; internal set; }
        /// <summary>
        /// string method Name (or another string)
        /// </summary>
        public string MethodName { get; internal set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ex">Exception <see cref="System.Exception"/></param>
        public WikiErrorEventArgs(Exception ex, string mname = null)
        {
            this.ex = ex;
            this.MethodName = mname;
        }
    }
    #endregion

    #region Event: public class WikiFSChangeEventArgs (EventArgs)
    /// <summary>
    /// Event Wiki File System change
    /// </summary>
    public class WikiFSChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Scaning FS total time
        /// </summary>
        public Int32 ScanTime { get; internal set; }
        /// <summary>
        /// Last file system total root name spaces
        /// </summary>
        public Int32 Count { get; internal set; }
        /// <summary>
        /// Engine instance class WikiFile
        /// </summary>
        public WikiFile WikiFile { get; internal set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wf">class WikiFile</param>
        /// <param name="count">File system total root name spaces</param>
        /// <param name="st">Scaning total time msec.</param>
        public WikiFSChangeEventArgs(WikiFile wf, int count, Int32 st = 0)
        {
            this.WikiFile = wf;
            this.Count = count;
            this.ScanTime = st;
        }
    }
    #endregion

    #region Event: public class WikiSyntaxErrorEventArgs (EventArgs)
    /// <summary>
    /// Event Arguments WikiSyntax Error
    /// </summary>
    public class WikiSyntaxErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Exception
        /// </summary>
        public Exception ex { get; internal set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ex">Exception <see cref="System.Exception"/></param>
        public WikiSyntaxErrorEventArgs(Exception ex)
        {
            this.ex = ex;
        }
    }
    #endregion

}
