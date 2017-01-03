using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using stDokuWiki.Util;
using stDokuWiki.WikiEngine;
using stDokuWiki.WikiEngine.Exceptions;

using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;


namespace stDokuWiki.WikiEngine
{
    public partial class WikiFile
    {
        #region public RouteTree
        /// <summary>
        /// Wiki Route tree (Web)
        /// return requested resource from wiki file system.
        /// </summary>
        /// <example>
        /// Uri: /wiki/get/pages|attic|media|meta/clan:test1
        /// Uri: /wiki/put/pages|attic|media/clan:test1
        /// Uri: /wiki/del/pages|attic|media/clan:test1
        /// Uri: /wiki/list/pages|attic|media/clan:
        /// Uri: /wiki/find/part-of-serch-page-name
        /// enum <see cref="WikiEngine.WikiRequestType"/>WikiEngine.WikiRequestType: get|put|del|list|find
        /// enum <see cref="WikiEngine.WikiFileType"/>WikiEngine.WikiFileType: pages|attic|media|meta
        /// </example>
        /// <remarks>
        /// Example routing Uri:
        /// 
        /// "/wiki/get/pages/clan:test1" OK
        /// "/wiki/get/clan:test1" OK
        /// "/wiki/clan:test1" -> HomeException
        /// "/wiki/clan:" -> HomeException
        /// "/wiki/" -> HomeException (redirect)
        ///
        /// "/wiki/list/pages/clan:" OK
        /// "/wiki/list/clan:" OK (default pages)
        /// "/wiki/list/" OK (default pages and default namespace)
        /// 
        /// "/wiki/list/media/clan:" OK
        /// "/wiki/list/media/" OK (default namespace)
        ///
        /// "/wiki/find/pages/tes" OK
        /// "/wiki/find/media/tes" OK
        /// "/wiki/find/attic/tes" OK
        /// "/wiki/find/tes" OK (default pages and default namespace)
        /// 
        /// </remarks>
        /// <exception cref="WikiEngine.Exceptions.WikiEngineAuthException">WikiEngineAuthException</exception>
        /// <exception cref="WikiEngine.Exceptions.WikiEngineSearchException">WikiEngineSearchException</exception>
        /// <exception cref="WikiEngine.Exceptions.WikiEngineHomePageException">WikiEngineHomePageException</exception>
        /// <exception cref="WikiEngine.Exceptions.WikiEngineErrorPageException">WikiEngineErrorPageException</exception>
        /// <exception cref="WikiEngine.Exceptions.WikiEngineNotImplementPageException">WikiEngineNotImplementPageException</exception>
        /// <param name="url">given raw url requested</param>
        /// <param name="wfm"><see cref="WikiEngine.WikiFileMeta"/>WikiEngine.WikiFileMeta</param>
        /// <returns>byte[] from requested source</returns>
        public byte [] RouteTree(string url, WikiFileMeta wfm = null)
        {
            try
            {
                List<string> urlPart = url.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();

                // Uri part, folders array
                switch (urlPart.Count)
                {
                    case 0:
                        {
                            // root, get home page template
                            // return Exception WikiHomePageException
                            Console.WriteLine("0: Count");
                            throw new WikiEngine.Exceptions.WikiEngineHomePageException();
                        }
                    case 1:
                        {
                            // command line: (/get | /put | /del | /list)
                            // not complette request,
                            // only WikiRequestType is set?
                            switch (DokuUtil.WikiFileStringToMethod(urlPart[0]))
                            {
                                default:
                                case WikiRequestType.None:
                                    {
                                        // get Exception Home page default (html template)
                                        // return Exception WikiHomePageException
                                        Console.WriteLine("1: WikiRequestType.None:0");
                                        if (!string.IsNullOrWhiteSpace(urlPart[0]))
                                        {
                                            if (urlPart[0].EndsWith(":"))
                                            {
                                                return _WikiGetList(
                                                    WikiFileType.FileReadMd,
                                                    urlPart[0]
                                                );
                                            }
                                            else if (urlPart[0].Contains(":"))
                                            {
                                                return _WikiGetFile(
                                                    WikiFileType.FileReadMd,
                                                    urlPart[0]
                                                );
                                            }
                                        }
                                        throw new WikiEngine.Exceptions.WikiEngineHomePageException(urlPart[0]);
                                    }
                                case WikiRequestType.List:
                                    {
                                        // get page list default ?
                                        // return Md format txt
                                        Console.WriteLine("1: WikiRequestType.List:0");
                                        return _WikiGetList(WikiFileType.FileReadMd, String.Empty);
                                    }
                                case WikiRequestType.Find:
                                    {
                                        // get find page default ?
                                        // return Md format txt
                                        Console.WriteLine("1: WikiRequestType.Find:0");
                                        throw new WikiEngineSearchException("search pattern is empty");
                                    }
                            }
                        }
                    case 2:
                        {
                            // command line: /get (/pages | /media | /attic | /meta)
                            // get List/Page/Media ?
                            string nssource = String.Empty;
                            WikiRequestType wrt = DokuUtil.WikiFileStringToMethod(urlPart[0]);
                            WikiFileType wft = DokuUtil.WikiFileStringToType(wrt, urlPart[1]);
                            switch (wrt)
                            {
                                case WikiRequestType.None:
                                    {
                                        if (wft == WikiFileType.None)
                                        {
                                            if ((wft = DokuUtil.WikiFileStringToType(wrt, urlPart[0])) != WikiFileType.None)
                                            {
                                                wrt = WikiRequestType.Get;
                                                nssource = urlPart[1];
                                            }
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        if (wft == WikiFileType.None)
                                        {
                                            nssource = urlPart[1];
                                            wft = WikiFileType.FileReadMd;
                                        }
                                        else
                                        {
                                            nssource = this._defaultNameSpace;
                                        }
                                        break;
                                    }
                            }
                            switch (wrt)
                            {
                                default:
                                case WikiRequestType.Del:
                                case WikiRequestType.Put:
                                case WikiRequestType.None:
                                    {
                                        // get Exception Home page default (html template)
                                        // return Exception WikiHomePageException
                                        Console.WriteLine("2: _WikiGetHome");
                                        throw new WikiEngine.Exceptions.WikiEngineHomePageException(
                                            nssource
                                        );
                                    }
                                case WikiRequestType.Find:
                                    {
                                        // find pages list (default from nssource)
                                        Console.WriteLine("2: _WikiFindList");
                                        return _WikiFindList(wft, nssource);
                                    }
                                case WikiRequestType.List:
                                    {
                                        // get pages list (default from nssource)
                                        Console.WriteLine("2: _WikiGetList");
                                        return _WikiGetList(wft, nssource);
                                    }
                                case WikiRequestType.Get:
                                    {
                                        // get page/media resource (default from nssource)
                                        Console.WriteLine("2: _WikiGetFile");
                                        return _WikiGetFile(wft, nssource);
                                    }
                            }
                        }
                    case 3:
                        {
                            WikiRequestType wrtAct = DokuUtil.WikiFileStringToMethod(urlPart[0]);
                            WikiFileType wrtTarget = DokuUtil.WikiFileStringToType(wrtAct, urlPart[1]);

                            if (
                                (wrtAct == WikiRequestType.None) ||
                                (wrtTarget == WikiFileType.None) ||
                                (string.IsNullOrWhiteSpace(urlPart[2])) ||
                                (
                                 (wrtAct != WikiRequestType.Find) &&
                                 (!urlPart[2].Contains(":"))
                                )
                               )
                            {
                                // error request param
                                // return Exception WikiHomePageException
                                throw new WikiEngineHomePageException(urlPart[2]);
                            }

                            List<string> wikiPart = urlPart[2].Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                            switch (wikiPart.Count)
                            {
                                case 0:
                                    {
                                        // WikiRequestType and WikiFileType is valid,
                                        // namespace not found, return default
                                        switch (wrtAct)
                                        {
                                            case WikiRequestType.Get:
                                                {
                                                    return _WikiGetFile(wrtTarget, this._defaultNameSpace);
                                                }
                                            case WikiRequestType.List:
                                                {
                                                    return _WikiGetList(wrtTarget, this._defaultNameSpace);
                                                }
                                            case WikiRequestType.Find:
                                                {
                                                    if (string.IsNullOrWhiteSpace(urlPart[2]))
                                                    {
                                                        throw new WikiEngineSearchException(
                                                            string.Format(
                                                                Properties.ResourceWikiEngine.txtErrorSearchPaternEmpty,
                                                                "0"
                                                            )
                                                        );
                                                    }
                                                    return _WikiFindList(wrtTarget, urlPart[2]);
                                                }
                                            case WikiRequestType.Put:
                                            case WikiRequestType.Del:
                                                {
                                                    throw new WikiEngineErrorPageException(
                                                        string.Format(
                                                            Properties.ResourceWikiEngine.fmtErrorNSError,
                                                            wrtAct.ToString(),
                                                            urlPart[2]
                                                        )
                                                    );
                                                }
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        // wikiPart.Count > 0
                                        // namespace and page/file name
                                        switch (wrtAct)
                                        {
                                            case WikiRequestType.Get:
                                                {
                                                    Console.WriteLine("3:default _WikiGetFile: " + urlPart[2]);
                                                    return _WikiGetFile(wrtTarget, urlPart[2]);
                                                }
                                            case WikiRequestType.List:
                                                {
                                                    Console.WriteLine("3:default _WikiGetList: " + " : " + wrtTarget + " : " + urlPart[2]);
                                                    return _WikiGetList(wrtTarget, urlPart[2]);
                                                }
                                            case WikiRequestType.Find:
                                                {
                                                    Console.WriteLine("3:default _WikiFindList: " + " : " + wrtTarget + " : " + urlPart[2] + " : " + wikiPart[(wikiPart.Count - 1)]);
                                                    int idx = (wikiPart.Count - 1);
                                                    if (
                                                        (idx < 0) ||
                                                        (string.IsNullOrWhiteSpace(wikiPart[idx]))
                                                       )
                                                    {
                                                        throw new WikiEngineSearchException(
                                                            string.Format(
                                                                Properties.ResourceWikiEngine.txtErrorSearchPaternEmpty,
                                                                idx
                                                            )
                                                        );
                                                    }
                                                    return _WikiFindList(wrtTarget, wikiPart[idx]);
                                                }
                                            case WikiRequestType.Put:
                                            case WikiRequestType.Del:
                                                {
                                                    Console.WriteLine("3:default _WikiPutFile: " + urlPart[2]);
                                                    if (wfm == null)
                                                    {
                                                        throw new WikiEngineAuthException(
                                                            string.Format(
                                                                Properties.ResourceWikiEngine.fmtErrorMetaEmpty,
                                                                urlPart[2]
                                                            )
                                                        );
                                                    }
                                                    if (!wfm.IsAuth)
                                                    {
                                                        throw new WikiEngineAuthException(
                                                            string.Format(
                                                                Properties.ResourceWikiEngine.fmtErrorAuthEmpty,
                                                                urlPart[2]
                                                            ),
                                                            wfm
                                                        );
                                                    }
                                                    return this._WikiPutFile(wrtTarget, urlPart[2], wfm);
                                                }
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            throw new WikiEngineHomePageException(urlPart[0]);
                        }
                }
            }
            catch (Exception e)
            {
                if (!this._isMapExceptions)
                {
                    throw e;
                }
            }
            return null;
        }
        #endregion

        #region private _WikiGetFile
        /// <summary>
        /// get page/media/attic/meta
        /// </summary>
        private byte[] _WikiGetFile(WikiFileType wft, string namesspace)
        {
            try
            {
                WikiData wd = null;
                WikiFileParse wfp = null;

                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                    wft, namesspace, null, null, true
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wft.ToString()
                        )
                    );
                }
                switch (wfp.FolderType)
                {
                    default:
                    case WikiFileType.FileReadMd:
                    case WikiFileType.FileWriteMd:
                    case WikiFileType.FileReadBinary:
                    case WikiFileType.FileWriteBinary:
                        {
                            wd = this._GetFile(wfp);
                            break;
                        }
                    case WikiFileType.FileReadAttic:
                    case WikiFileType.FileWriteAttic:
                        {
                            wd = this._GetFileFromAttic(wfp);
                            break;
                        }
                    case WikiFileType.FileReadMeta:
                        {
                            return Encoding.UTF8.GetBytes(
                                this.MetaListToMdString(
                                    this._GetFileMeta(wfp)
                                )
                            );
                        }
                    case WikiFileType.FileWriteMeta:
                        {
                            throw new WikiEngineNotImplementPageException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorMapTree,
                                    MethodBase.GetCurrentMethod().Name,
                                    namesspace,
                                    wfp.FolderType.ToString()
                                )
                            );
                        }
                }
                if (
                    (wd == null) ||
                    (wd.FileContent == null) ||
                    (wd.FileContent.Length == 0)
                   )
                {
                    throw new WikiEngineInternalSearchEmptyException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wfp.FolderType.ToString()
                        )
                    );
                }
                return wd.FileContent;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region private _WikiPutFile
        /// <summary>
        /// put page/media
        /// </summary>
        private byte[] _WikiPutFile(WikiFileType wft, string namesspace, WikiFileMeta wfm = null)
        {
            try
            {
                WikiData wd = null;
                WikiFileParse wfp = null;

                if (wfm == null)
                {
                    throw new WikiEnginePutException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wft.ToString()
                        )
                    );
                }
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                    wft, namesspace, null, null, true
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wft.ToString()
                        )
                    );
                }
                if ((wfp.IsNameSpaceValid) && (!wfp.IsNameSpaceOnly))
                {
                    switch (wfp.FolderType)
                    {
                        case WikiFileType.FileWriteMd:
                        case WikiFileType.FileWriteBinary:
                            {
                                break;
                            }
                        case WikiFileType.FileWriteAttic:
                        case WikiFileType.FileWriteMeta:
                            {
                                throw new WikiEngineNotImplementPageException(
                                    string.Format(
                                        Properties.ResourceWikiEngine.fmtErrorMapTree,
                                        MethodBase.GetCurrentMethod().Name,
                                        namesspace,
                                        wfp.FolderType.ToString()
                                    )
                                );
                            }
                        default:
                        case WikiFileType.FileReadMd:
                        case WikiFileType.FileReadMeta:
                        case WikiFileType.FileReadAttic:
                        case WikiFileType.FileReadBinary:
                            {
                                throw new WikiEngineInternalFileTypeException(
                                    string.Format(
                                        Properties.ResourceWikiEngine.fmtErrorMapTreePut,
                                        MethodBase.GetCurrentMethod().Name,
                                        wfp.FolderType.ToString(),
                                        namesspace
                                    )
                                );
                            }
                    }
                }
                else
                {
                    throw new WikiEngineInternalNameSpaceErrorException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTreeNameSpace,
                            MethodBase.GetCurrentMethod().Name,
                            wfp.FolderType.ToString(),
                            namesspace
                        )
                    );
                }
                if (
                    ((wd = this._PutFile(wfp, null, wfm)) == null) ||
                    (wd.FileContent == null) ||
                    (wd.FileContent.Length == 0)
                   )
                {
                    throw new WikiEngineInternalSearchEmptyException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wfp.FolderType.ToString()
                        )
                    );
                }
                return wd.FileContent;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region private _WikiGetList
        /// <summary>
        /// get page list default ?
        /// </summary>
        private byte[] _WikiGetList(WikiFileType wft, string namesspace)
        {
            try
            {
                WikiFolderInfo wfi = null;
                WikiFileParse wfp = null;

                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                    wft, namesspace, null
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wft.ToString()
                        )
                    );
                }
                switch (wfp.FolderType)
                {
                    case WikiFileType.FileReadMd:
                    case WikiFileType.FileReadBinary:
                    case WikiFileType.FileReadMeta:
                    case WikiFileType.FileReadAttic:
                    case WikiFileType.NameSpace:
                        {
                            break;
                        }
                    default:
                        {
                            throw new WikiEngineNotImplementPageException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorMapTree,
                                    MethodBase.GetCurrentMethod().Name,
                                    namesspace,
                                    wfp.FolderType.ToString()
                                )
                            );
                        }
                }
                if ((wfi = (WikiFolderInfo)this._GetFileList(wfp)) == null)
                {
                    throw new WikiEngineInternalSearchEmptyException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wfp.FolderType.ToString()
                        )
                    );
                }
                return Encoding.UTF8.GetBytes(
                    this.ResourceListToMdString(wfi, null, wfp.FolderType)
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region private _WikiFindList
        /// <summary>
        /// find pages list
        /// </summary>
        private byte[] _WikiFindList(WikiFileType wft, string namesspace)
        {
            WikiFileParse wfp = null;
            WikiFolderInfo wfi = null;
            try
            {
                if ((wfp = (WikiFileParse)this._WikiFilesParse(
                    wft, namesspace, null, null, false
                    )) == null)
                {
                    throw new WikiEngineInternalNameSpaceErrorException(
                        string.Format(
                            Properties.ResourceWikiEngine.fmtErrorMapTree,
                            MethodBase.GetCurrentMethod().Name,
                            namesspace,
                            wft.ToString()
                        )
                    );
                }
                switch (wfp.FolderType)
                {
                    case WikiFileType.FileReadMd:
                    case WikiFileType.FileWriteMd:
                    case WikiFileType.FileReadBinary:
                    case WikiFileType.FileWriteBinary:
                    case WikiFileType.FileReadAttic:
                    case WikiFileType.FileWriteAttic:
                        {
                            if ((wfi = (WikiFolderInfo)this._FindFiles(wfp)) == null)
                            {
                                throw new WikiEngineInternalSearchEmptyException(
                                    string.Format(
                                        Properties.ResourceWikiEngine.fmtErrorMapTree,
                                        MethodBase.GetCurrentMethod().Name,
                                        namesspace,
                                        wfp.FolderType.ToString()
                                    )
                                );
                            }
                            break;
                        }
                    default:
                        {
                            throw new WikiEngineNotImplementPageException(
                                string.Format(
                                    Properties.ResourceWikiEngine.fmtErrorMapTree,
                                    MethodBase.GetCurrentMethod().Name,
                                    namesspace,
                                    wfp.FolderType.ToString()
                                )
                            );
                        }
                }
                return Encoding.UTF8.GetBytes(
                    this.ResourceListToMdString(wfi, null, wfp.FolderType)
                );
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region private _WikiGetHome TODO: delete this!!!!!!!!!!
        /// <summary>
        /// get Home location, Exception to load Home page default (html template)
        /// </summary>
        private void _WikiGetHome(string namesspace)
        {
            if (DokuUtil.WikiFileStringToType(WikiRequestType.None, namesspace) == WikiFileType.None)
            {
                throw new WikiEngineErrorPageException(
                    ((namesspace.Contains(":")) ? namesspace : String.Empty)
                );
            }
            throw new WikiEngineErrorPageException();
        }

        #endregion

    }
}
