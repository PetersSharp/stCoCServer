using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using stNet.stWebServerUtil;
using stSqlite;
using stDokuWiki.WikiEngine;
using System.Data;
using stCoCServerConfig;
using stCoCServerConfig.CoCServerConfigData;
using System.Collections.Generic;
using System.Diagnostics;

namespace stCoCServer.CoCAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.ReturnValue |
    AttributeTargets.Property, AllowMultiple = false)]
    public class CallTracingAttribute : Attribute
    {
        public CallTracingAttribute()
        {

            try
            {
                StackTrace stackTrace = new StackTrace();
                StackFrame stackFrame = stackTrace.GetFrame(1);

                Trace.TraceInformation("{0}->{1} {2}:{3}",
                    stackFrame.GetMethod().ReflectedType.Name,
                    stackFrame.GetMethod().Name,
                    stackFrame.GetFileName(),
                    stackFrame.GetFileLineNumber());

                Debug.WriteLine(string.Format("{0}->{1} {2}:{3}",
                    stackFrame.GetMethod().ReflectedType.Name,
                    stackFrame.GetMethod().Name,
                    stackFrame.GetFileName(),
                    stackFrame.GetFileLineNumber()));
            }
            catch
            {
            }
        }
    }

    public static partial class CoCWebSrv
    {

        #region private enum WikiRequestType
        /// <summary>
        /// Type Wiki Web Reguest
        /// </summary>
        private enum WikiRequestType : int
        {
            None,
            Get,
            Put,
            Del,
            List,
            Find
        };

        #endregion

        #region public WebWikiRouteTree
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
        /// enum <see cref="WikiRequestType"/>WikiRequestType: get|put|del|list|find
        /// enum <see cref="WikiFileType"/>WikiFileType: pages|attic|media|meta
        /// </example>
        /// <exception cref="WikiHomePageException">WikiHomePageException</exception>
        /// <exception cref="WikiErrorPageException">WikiErrorPageException</exception>
        /// <exception cref="WikiNotImplementPageException">WikiNotImplementPageException</exception>
        /// <param name="url">given raw url requested</param>
        /// <param name="conf"></param>
        /// <param name="wfm"><see cref="WikiEngine.WikiFileMeta"/>WikiEngine.WikiFileMeta</param>
        /// <returns>byte[] from requested source</returns>
        //[Conditional("DEBUG")]
        //[DebuggerStepThrough]
        [CallTracing]
        public static byte[] WebWikiRouteTree(string url, Configuration conf, WikiFileMeta wfm = null)
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
                            stCore.stConsole.WriteHeader("0: Count");
                            throw new WikiHomePageException();
                        }
                    case 1:
                        {
                            // not complette request,
                            // only WikiRequestType is set?
                            switch (CoCWebSrv._WikiGetAction(urlPart[0]))
                            {
                                default:
                                case WikiRequestType.None:
                                    {
                                        // get Exception Home page default (html template)
                                        // return Exception WikiHomePageException
                                        stCore.stConsole.WriteHeader("1: WikiRequestType.None:0");
                                        _WikiGetHome(conf.WikiEngine, urlPart[0]);
                                        break;
                                    }
                                case WikiRequestType.List:
                                    {
                                        // get page list default ?
                                        // return Md format txt
                                        stCore.stConsole.WriteHeader("1: WikiRequestType.List:0");
                                        return _WikiGetList(conf.WikiEngine, WikiFileType.FileReadMd, String.Empty);
                                    }
                                case WikiRequestType.Find:
                                    {
                                        // get find page default ?
                                        // return Md format txt
                                        stCore.stConsole.WriteHeader("1: WikiRequestType.Find:0");
                                        throw new WikiSearchException("search pattern is empty");
                                    }
                            }
                            break;
                        }
                    case 2:
                        {
                            // get List/Page/Media ?
                            WikiRequestType wrt = CoCWebSrv._WikiGetAction(urlPart[0]);
                            WikiFileType wft = CoCWebSrv._WikiGetTarget(wrt, urlPart[1]);
                            switch (wrt)
                            {
                                default:
                                case WikiRequestType.Del:
                                case WikiRequestType.Put:
                                case WikiRequestType.None:
                                    {
                                        // get Exception Home page default (html template)
                                        // return Exception WikiHomePageException
                                        stCore.stConsole.WriteHeader("2: _WikiGetHome");
                                        _WikiGetHome(
                                            conf.WikiEngine,
                                            ((wft == WikiFileType.None) ? urlPart[1] : String.Empty)
                                        );
                                        break;
                                    }
                                case WikiRequestType.Find:
                                    {
                                        // get page list default ?
                                        stCore.stConsole.WriteHeader("2: _WikiFindList");
                                        if (
                                            ((wft == WikiFileType.None) &&
                                             (string.IsNullOrWhiteSpace(urlPart[1]))) ||
                                            (wft != WikiFileType.None)
                                           )
                                        {
                                            throw new WikiSearchException("search pattern is empty");
                                        }
                                        return _WikiFindList(
                                            conf.WikiEngine,
                                            ((wft == WikiFileType.None) ? WikiFileType.FileReadMd : wft),
                                            urlPart[1]
                                        );
                                    }
                                case WikiRequestType.List:
                                    {
                                        // get page list default ?
                                        stCore.stConsole.WriteHeader("2: _WikiGetList");
                                        return _WikiGetList(
                                            conf.WikiEngine,
                                            ((wft == WikiFileType.None) ? WikiFileType.FileReadMd : wft),
                                            ((wft == WikiFileType.None) ? urlPart[1] : String.Empty)
                                        );
                                    }
                                case WikiRequestType.Get:
                                    {
                                        // get page/media default ? eturn start page
                                        stCore.stConsole.WriteHeader("2: _WikiGetFile");
                                        return _WikiGetFile(
                                            conf.WikiEngine,
                                            ((wft == WikiFileType.None) ? WikiFileType.FileReadMd : wft),
                                            ((wft == WikiFileType.None) ? urlPart[1] : String.Empty)
                                        );
                                    }
                            }
                            break;
                        }
                    case 3:
                        {
                            WikiRequestType wrtAct = CoCWebSrv._WikiGetAction(urlPart[0]);
                            WikiFileType wrtTarget = CoCWebSrv._WikiGetTarget(wrtAct, urlPart[1]);

                            if (
                                (wrtAct == WikiRequestType.None) ||
                                (wrtTarget == WikiFileType.None) ||
                                (string.IsNullOrWhiteSpace(urlPart[2])) ||
                                (!urlPart[2].Contains(":"))
                               )
                            {
                                // error request param
                                // return Exception WikiHomePageException
                                _WikiGetHome(conf.WikiEngine, urlPart[2]);
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
                                                    return _WikiGetFile(
                                                        conf.WikiEngine,
                                                        wrtTarget,
                                                        String.Empty
                                                    );
                                                }
                                            case WikiRequestType.Find:
                                                {
                                                    if (string.IsNullOrWhiteSpace(urlPart[2]))
                                                    {
                                                        throw new WikiSearchException("search pattern is empty");
                                                    }
                                                    return _WikiFindList(
                                                        conf.WikiEngine,
                                                        wrtTarget,
                                                        urlPart[2]
                                                    );
                                                }
                                            case WikiRequestType.List:
                                                {
                                                    return _WikiGetList(
                                                        conf.WikiEngine,
                                                        wrtTarget,
                                                        String.Empty
                                                    );
                                                }
                                            case WikiRequestType.Put:
                                            case WikiRequestType.Del:
                                                {
                                                    throw new WikiErrorPageException(
                                                        string.Format(
                                                            "not update, name space {0} incorrected",
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
                                                    stCore.stConsole.WriteHeader("3:default _WikiGetFile: " + urlPart[2]);
                                                    return _WikiGetFile(conf.WikiEngine, wrtTarget, urlPart[2]);
                                                }
                                            case WikiRequestType.List:
                                                {
                                                    stCore.stConsole.WriteHeader("3:default _WikiGetList: " + " : " + wrtTarget + " : " + urlPart[2]);
                                                    return _WikiGetList(conf.WikiEngine, wrtTarget, urlPart[2]);
                                                }
                                            case WikiRequestType.Find:
                                                {
                                                    stCore.stConsole.WriteHeader("3:default _WikiFindList: " + " : " + wrtTarget + " : " + urlPart[2] + " : " + wikiPart[(wikiPart.Count - 1)]);
                                                    int idx = (wikiPart.Count - 1);
                                                    if (
                                                        (idx < 0) ||
                                                        (string.IsNullOrWhiteSpace(wikiPart[idx]))
                                                       )
                                                    {
                                                        throw new WikiSearchException("search pattern is empty, index: " + idx);
                                                    }
                                                    return _WikiFindList(
                                                        conf.WikiEngine,
                                                        wrtTarget,
                                                        wikiPart[idx]
                                                    );
                                                }
                                            case WikiRequestType.Put:
                                            case WikiRequestType.Del:
                                                {
                                                    stCore.stConsole.WriteHeader("3:default _WikiPutFile: " + urlPart[2]);
                                                    if (wfm == null)
                                                    {
                                                        throw new WikiErrorPageException(
                                                            string.Format(
                                                                "meta data is empty, not change",
                                                                urlPart[2]
                                                            )
                                                        );
                                                    }
                                                    if (!wfm.IsAuth)
                                                    {
                                                        throw new WikiAuthException(
                                                            string.Format(
                                                                "Auth error, file not change",
                                                                urlPart[2]
                                                            ),
                                                            wfm
                                                        );
                                                    }
                                                    return _WikiPutFile(conf.WikiEngine, wrtTarget, urlPart[2], wfm);
                                                }
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            _WikiGetHome(conf.WikiEngine, urlPart[0]);
                            break;
                        }
                }
            }
            catch (WikiErrorPageException e)
            {
                // error page WikiErrorPageException
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "Wiki Parser"),
                        e.GetType().Name,
                        e.Message
                    )
                );
                return null;
            }
            catch (WikiHomePageException e)
            {
                // home page WikiHomePageException
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "Wiki Parser"),
                        e.GetType().Name,
                        e.Message
                    )
                );
                return null;
            }
#if DEBUG
            catch (Exception e)
            {
                conf.ILog.LogError(
                    string.Format(
                        Properties.Resources.fmtMainError,
                        string.Format(fmtClassName, "Wiki Parser"),
                        e.GetType().Name,
                        e.Message
                    )
                );
#else
            catch (Exception)
            {
#endif
                return null;
            }
            return null;
        }

        #endregion

        #region private _WikiGetAction
        /// <summary>
        /// get action, enum WikiRequestType
        /// </summary>
        private static WikiRequestType _WikiGetAction(string src)
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

        #region private _WikiGetTarget
        /// <summary>
        /// get target, enum WikiFileType
        /// </summary>
        private static WikiFileType _WikiGetTarget(WikiRequestType type, string src)
        {
            switch (src)
            {
                case stDokuWiki.WikiEngine.WikiFile.wikiLocalPage:
                    {
                        return (((type == WikiRequestType.Put) || (type == WikiRequestType.Del)) ?
                            WikiFileType.FileWriteMd :
                            WikiFileType.FileReadMd
                        );
                    }
                case stDokuWiki.WikiEngine.WikiFile.wikiLocalMedia:
                    {
                        return (((type == WikiRequestType.Put) || (type == WikiRequestType.Del)) ?
                            WikiFileType.FileWriteBinary :
                            WikiFileType.FileReadBinary
                        );
                    }
                case stDokuWiki.WikiEngine.WikiFile.wikiLocalAttic:
                    {
                        return (((type == WikiRequestType.Put) || (type == WikiRequestType.Del)) ?
                            WikiFileType.FileWriteAttic :
                            WikiFileType.FileReadAttic
                        );
                    }
                case stDokuWiki.WikiEngine.WikiFile.wikiLocalMeta:
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

        #region private _WikiGetFile
        /// <summary>
        /// get page/media
        /// </summary>
        private static byte[] _WikiGetFile(stDokuWiki.WikiEngine.WikiFile wiki, WikiFileType type, string namesspace)
        {
            WikiData wd = null;
            switch (namesspace.Contains(":"))
            {
                case true:
                    {
                        switch (type)
                        {
                            default:
                            case WikiFileType.FileReadMd:
                            case WikiFileType.FileWriteMd:
                                {
                                    wd = wiki.GetFile(namesspace);
                                    break;
                                }
                            case WikiFileType.FileReadBinary:
                            case WikiFileType.FileWriteBinary:
                                {
                                    wd = wiki.GetFile(namesspace);
                                    break;
                                }
                            case WikiFileType.FileReadAttic:
                            case WikiFileType.FileWriteAttic:
                                {
                                    wd = wiki.GetFileFromAttic(namesspace, "0");
                                    break;
                                }
                            case WikiFileType.FileReadMeta:
                                {
                                    return Encoding.UTF8.GetBytes(
                                        wiki.MetaListToMdString(
                                            wiki.GetFileMeta(namesspace)
                                        )
                                    );
                                }
                            case WikiFileType.FileWriteMeta:
                                {
                                    throw new WikiNotImplementPageException(WikiFileType.FileWriteMeta.ToString());
                                }
                        }
                        break;
                    }
                /// operation default
                case false:
                    {
                        switch (type)
                        {
                            default:
                            case WikiFileType.FileReadMd:
                            case WikiFileType.FileWriteMd:
                                {
                                    wd = wiki.GetFile(":start");
                                    break;
                                }
                            case WikiFileType.FileReadBinary:
                            case WikiFileType.FileWriteBinary:
                                {
                                    wd = wiki.GetFile(":logo.png");
                                    break;
                                }
                            case WikiFileType.FileReadAttic:
                            case WikiFileType.FileWriteAttic:
                                {
                                    // TODO: get last from attic
                                    wd = wiki.GetFileFromAttic(":start", "0");
                                    break;
                                }
                            case WikiFileType.FileReadMeta:
                                {
                                    throw new WikiNotImplementPageException(WikiFileType.FileReadMeta.ToString());
                                }
                            case WikiFileType.FileWriteMeta:
                                {
                                    throw new WikiNotImplementPageException(WikiFileType.FileWriteMeta.ToString());
                                }
                        }
                        break;
                    }
            }
            if (
                (wd == null) ||
                (wd.FileContent == null) ||
                (wd.FileContent.Length == 0)
               )
            {
                return null;
            }
            return wd.FileContent;
        }

        #endregion

        #region private _WikiPutFile
        /// <summary>
        /// put page/media
        /// </summary>
        private static byte[] _WikiPutFile(stDokuWiki.WikiEngine.WikiFile wiki, WikiFileType type, string namesspace, WikiFileMeta wfm = null)
        {
            WikiData wd = null;
            switch (namesspace.Contains(":"))
            {
                case true:
                    {
                        switch (type)
                        {
                            case WikiFileType.FileWriteMd:
                                {
                                    if (wfm == null)
                                    {
                                        throw new WikiErrorPageException(
                                            string.Format(
                                                "not write {0}, input data is empty",
                                                namesspace
                                            )
                                        );
                                    }
                                    wd = wiki.PutFile(namesspace, null, wfm);
                                    break;
                                }
                            case WikiFileType.FileWriteBinary:
                                {
                                    wd = wiki.GetFile(namesspace);
                                    break;
                                }
                            case WikiFileType.FileWriteAttic:
                                {
                                    throw new WikiNotImplementPageException(WikiFileType.FileWriteAttic.ToString());
                                }
                            case WikiFileType.FileWriteMeta:
                                {
                                    throw new WikiNotImplementPageException(WikiFileType.FileWriteMeta.ToString());
                                }
                            default:
                            case WikiFileType.FileReadMd:
                            case WikiFileType.FileReadMeta:
                            case WikiFileType.FileReadAttic:
                            case WikiFileType.FileReadBinary:
                                {
                                    throw new WikiErrorPageException(
                                        string.Format(
                                            "type set is read, not support for this version, name space: {0}",
                                            namesspace
                                        )
                                    );
                                }
                        }
                        break;
                    }
                case false:
                    {
                        throw new WikiErrorPageException(
                            string.Format(
                                "not valid name space: {0}",
                                namesspace
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
                return null;
            }
            return wd.FileContent;
        }

        #endregion

        #region private _WikiGetList
        /// <summary>
        /// get page list default ?
        /// </summary>
        private static byte[] _WikiGetList(WikiFile wiki, WikiFileType type, string namesspace)
        {
            /*
            Dictionary<string, List<WikiFileInfo>> dwf = null;
            bool isNameSpace = namesspace.Contains(":");
            switch (type)
            {
                default:
                case WikiFileType.FileReadMd:
                case WikiFileType.FileWriteMd:
                    {
                        dwf = ((isNameSpace) ?
                            wiki.GetFileListPages(namesspace) :
                            wiki.GetFileListPages()
                        );
                        break;
                    }
                case WikiFileType.FileReadBinary:
                case WikiFileType.FileWriteBinary:
                    {
                        dwf = ((isNameSpace) ?
                            wiki.GetFileListMedia(namesspace) :
                            wiki.GetFileListMedia()
                        );
                        break;
                    }
                case WikiFileType.FileReadAttic:
                case WikiFileType.FileWriteAttic:
                    {
                        dwf = ((isNameSpace) ?
                            wiki.GetFileListAttic(namesspace) :
                            wiki.GetFileListAttic()
                        );
                        break;
                    }
                case WikiFileType.FileWriteMeta:
                case WikiFileType.FileReadMeta:
                    {
                        throw new WikiNotImplementPageException(type.ToString());
                    }
            }
            if (dwf == null)
            {
                return null;
            }
            return Encoding.UTF8.GetBytes(
                wiki.PageListToMdString(dwf, null)
            );
             */
            return null;
        }

        #endregion

        #region private _WikiFindList
        /// <summary>
        /// find pages list
        /// </summary>
        private static byte[] _WikiFindList(WikiFile wiki, WikiFileType type, string namesspace)
        {
            /*
            bool isNameSpace = namesspace.Contains(":");
            if (namesspace.Contains(":"))
            {
                int offset = namesspace.LastIndexOf(":");
                namesspace = namesspace.Substring(offset, (namesspace.Length - offset));
            }
            if (string.IsNullOrWhiteSpace(namesspace))
            {
                throw new WikiSearchException("empty");
            }
            Dictionary<string, List<WikiFileInfo>> dwf = null;

            switch (type)
            {
                case WikiFileType.FileReadMd:
                case WikiFileType.FileWriteMd:
                case WikiFileType.FileReadBinary:
                case WikiFileType.FileWriteBinary:
                case WikiFileType.FileReadAttic:
                case WikiFileType.FileWriteAttic:
                    {
                        dwf = wiki.FindFiles(type, namesspace);
                        break;
                    }
                default:
                    {
                        throw new WikiNotImplementPageException(type.ToString());
                    }
            }
            if (dwf == null)
            {
                return null;
            }
            return Encoding.UTF8.GetBytes(
                wiki.PageListToMdString(dwf, null)
            );
             */
            return null;
        }

        #endregion

        #region private _WikiGetHome

        /// <summary>
        /// get Home location, Exception to load Home page default (html template)
        /// </summary>
        private static void _WikiGetHome(stDokuWiki.WikiEngine.WikiFile wiki, string namesspace)
        {
            if (CoCWebSrv._WikiGetTarget(WikiRequestType.None, namesspace) == WikiFileType.None)
            {
                throw new WikiErrorPageException(
                    ((namesspace.Contains(":")) ? namesspace : String.Empty)
                );
            }
            throw new WikiErrorPageException();
        }

        #endregion

    }
}
