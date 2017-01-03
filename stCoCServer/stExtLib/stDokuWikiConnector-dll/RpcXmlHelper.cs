using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using stDokuWiki.Util;

namespace stDokuWiki.Connector
{
    /// <summary>
    /// DokuWiki Rpc-Xml connector
    /// </summary>
    public partial class RpcXml
    {
        private const string _defaultns = "wiki:";

        #region public DokuWiki Helper

        #region public DokuAuth
        /// <summary>
        /// Uses the provided credentials to execute a login and will set cookies. This can be used to make authenticated requests afterwards.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="passwd"></param>
        /// <returns>Boolean</returns>
        public bool DokuAuth(string login = null, string passwd = null)
        {
            try
            {
                this._cookie = new CookieContainer();

                Data.XMLMethodBool ma = this.Get<Data.XMLMethodBool>(
                    XmlRpcRequest.dokuwiki_login,
                    ((string.IsNullOrWhiteSpace(login)) ? this._login : login),
                    ((string.IsNullOrWhiteSpace(passwd)) ? this._passwd : passwd)
                );
                if (ma == null)
                {
                    throw new ArgumentNullException();
                }
                int result = 0;
                if (Int32.TryParse(ma.Params.Param.Value.Boolean, out result))
                {
                    return this._isAuth = ((result == 1) ? true : false);
                }
                throw new ArgumentNullException();
            }
            catch (RpcXmlException e)
            {
                this._isAuth = false;
                throw e;
            }
            catch (Exception)
            {
                return this._isAuth = false;
            }
        }
        #endregion

        #region public DokuVersion
        /// <summary>
        /// Returns the DokuWiki version of the remote Wiki.
        /// </summary>
        /// <returns>String</returns>
        public string DokuVersion()
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodGetString pl = this.Get<Data.XMLMethodGetString>(
                    XmlRpcRequest.dokuwiki_getVersion
                );
                if (pl == null)
                {
                    throw new ArgumentNullException();
                }
                return pl.Params.Param.Value.String;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5011
                );
            }
        }
        #endregion

        #region public DokuXMLRPCAPIVersion
        /// <summary>
        /// Returns the XML RPC interface version of the remote Wiki.
        /// This is DokuWiki implementation specific and independent of the supported
        /// standard API version returned by wiki.getRPCVersionSupported
        /// </summary>
        /// <returns>Int32</returns>
        public int DokuXMLRPCAPIVersion()
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodGetInt rv = this.Get<Data.XMLMethodGetInt>(
                    XmlRpcRequest.dokuwiki_getXMLRPCAPIVersion
                );
                int id;
                if (
                    (rv == null) ||
                    (!Int32.TryParse(rv.Params.Param.Value.Int, out id))
                   )
                {
                    throw new ArgumentNullException();
                }
                return id;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5022
                );
            }
        }
        #endregion

        #region public DokuRPCVersionSupported
        /// <summary>
        /// Returns 2 with the supported RPC API version.
        /// </summary>
        /// <returns>Int32</returns>
        public int DokuRPCVersionSupported()
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodGetInt rv = this.Get<Data.XMLMethodGetInt>(
                    XmlRpcRequest.wiki_getRPCVersionSupported
                );
                int id;
                if (
                    (rv == null) ||
                    (!Int32.TryParse(rv.Params.Param.Value.Int, out id))
                   )
                {
                    throw new ArgumentNullException();
                }
                return id;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5021
                );
            }
        }
        #endregion

        #region public DokuGetTimeStamp
        /// <summary>
        /// Returns the current time at the remote wiki server as Unix timestamp.
        /// </summary>
        /// <returns>Int32</returns>
        public int DokuGetTimeStamp()
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodGetInt rv = this.Get<Data.XMLMethodGetInt>(
                    XmlRpcRequest.dokuwiki_getTime
                );
                int id;
                if (
                    (rv == null) ||
                    (!Int32.TryParse(rv.Params.Param.Value.Int, out id))
                   )
                {
                    throw new ArgumentNullException();
                }
                return id;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5023
                );
            }
        }
        #endregion

        #region public DokuGetDateTime
        /// <summary>
        /// Returns the current time at the remote wiki server as DateTime.
        /// </summary>
        /// <returns>DateTime</returns>
        public DateTime DokuGetDateTime()
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodGetInt rv = this.Get<Data.XMLMethodGetInt>(
                    XmlRpcRequest.dokuwiki_getTime
                );
                int id;
                if (
                    (rv == null) ||
                    (!Int32.TryParse(rv.Params.Param.Value.Int, out id))
                   )
                {
                    throw new ArgumentNullException();
                }
                return id.GetDateTimeFromUnixTimeStamp();
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5020
                );
            }
        }
        #endregion

        #region public DokuSearch
        /// <summary>
        /// Search return associative array with matching pages similar to what is returned by dokuwiki.getPagelist, snippets are provided for the first 15 results.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>class <see cref="Data.XMLMethodPageList"/>Data.XMLMethodPageList</returns>
        public Data.XMLMethodPageList DokuSearch(string query = null)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodPageList pl = this.Get<Data.XMLMethodPageList>(
                    XmlRpcRequest.dokuwiki_search,
                    ((string.IsNullOrWhiteSpace(query)) ? "*" : query)
                );
                if (pl == null)
                {
                    throw new ArgumentNullException();
                }
                return pl;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5010
                );
            }
        }
        #endregion

        #region public DokuTitle
        /// <summary>
        /// Returns the title of the wiki.
        /// </summary>
        /// <returns>String</returns>
        public string DokuTitle()
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodGetString pl = this.Get<Data.XMLMethodGetString>(
                    XmlRpcRequest.dokuwiki_getTitle
                );
                if (pl == null)
                {
                    throw new ArgumentNullException();
                }
                return pl.Params.Param.Value.String;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5011
                );
            }
        }
        #endregion

        #region public DokuPageList
        /// <summary>
        /// Lists all pages within a given namespace.
        /// </summary>
        /// <example>
        /// example foreach all page in namespace "wiki:*"
        /// </example>
        /// <code>
        ///    XMLMethodPageList dokuList = xml.DokuPageList("wiki:") as XMLMethodPageList;
        ///    foreach (var items in dokuList.Params.Param.Value.Array.Data.Value)
        ///    {
        ///        foreach (var item in items.Struct.Member)
        ///        {
        ///            Console.WriteLine(
        ///                item.Name +
        ///                ((string.IsNullOrWhiteSpace(item.Value.Int)) ? " " : " [" + item.Value.Int + "] ") +
        ///                ((string.IsNullOrWhiteSpace(item.Value.String)) ? "" : item.Value.String)
        ///            );
        ///        }
        ///    }
        /// </code>
        /// <param name="namesspace"></param>
        /// <param name="opt"></param>
        /// <returns>class <see cref="Data.XMLMethodPageList"/>Data.XMLMethodPageList</returns>
        public object DokuPageList(string namesspace = null, string opt = null)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodPageList pl = this.Get<Data.XMLMethodPageList>(
                    XmlRpcRequest.dokuwiki_getPagelist,
                    ((string.IsNullOrWhiteSpace(namesspace)) ?
                        ((string.IsNullOrWhiteSpace(this._namespace)) ? _defaultns : this._namespace) : namesspace),
                    opt
                );
                if (pl == null)
                {
                    throw new ArgumentNullException();
                }
                return pl;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5009
                );
            }
        }
        #endregion

        #region public DokuPageListAll
        /// <summary>
        /// Returns a list of all Wiki pages in the remote Wiki.
        /// </summary>
        /// <example>
        /// Return (array) One item for each page, each item containing the following data:
        /// id = id of the page.
        /// perms = integer denoting the permissions on the page.
        /// size = size in bytes.
        /// lastModified = dateTime object of last modification date.
        /// </example>
        /// <returns>class <see cref="Data.XMLMethodPageListAll"/>Data.XMLMethodPageListAll</returns>
        public Data.XMLMethodPageListAll DokuPageListAll()
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodPageListAll pl = this.Get<Data.XMLMethodPageListAll>(
                    XmlRpcRequest.wiki_getAllPages
                );
                if (pl == null)
                {
                    throw new ArgumentNullException();
                }
                return pl;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5015
                );
            }
        }
        #endregion

        #region public DokuPageAclCheck
        /// <summary>
        /// Returns the permission of the given wikipage.
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <returns>Int32</returns>
        public int DokuPageAclCheck(string pagename)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodPageAclCheck pa = this.Get<Data.XMLMethodPageAclCheck>(
                    XmlRpcRequest.wiki_aclCheck,
                    pagename
                );
                int id;
                if (
                    (pa == null) ||
                    (!Int32.TryParse(pa.Params.Param.Value.Int, out id))
                   )
                {
                    throw new ArgumentNullException();
                }
                return id;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5019
                );
            }
        }
        #endregion

        #region public DokuPageGet
        /// <summary>
        /// Get DokuWiki raw/html Wiki text for a page.
        /// </summary>
        /// <remarks>
        /// isHtml = false : Returns the raw Wiki text for a page
        /// isHtml = true  : Returns the rendered XHTML body of a Wiki page
        /// </remarks>
        /// <param name="pagename">page name</param>
        /// <param name="isHtml">wiki markup/html selector</param>
        /// <returns>String</returns>
        public string DokuPageGet(string pagename, bool isHtml = false)
        {
            try
            {
                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodPageGet pg = this.Get<Data.XMLMethodPageGet>(
                    ((isHtml) ? XmlRpcRequest.wiki_getPageHTML : XmlRpcRequest.wiki_getPage),
                    pagename
                );
                if (string.IsNullOrWhiteSpace(pg.Params.Param.Value.String))
                {
                    throw new ArgumentNullException();
                }
                return pg.Params.Param.Value.String;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5012
                );
            }
        }
        #endregion

        #region public DokuPageGetVersion

        /// <summary>
        /// Returns the raw/html Wiki text for a page with DateTime.
        /// <see cref="DokuPageGet"/>
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="date">DateTime format timestamp</param>
        /// <param name="isHtml">return Html content</param>
        /// <returns>String</returns>
        public string DokuPageGetVersion(string pagename, DateTime date, bool isHtml = false)
        {
            return this.DokuPageGetVersion(
                pagename,
                date.GetUnixTimeStamp(),
                isHtml
            );
        }
        /// <summary>
        /// Returns the raw/html Wiki text for a page with Date and Time string.
        /// <see cref="DokuPageGet"/>
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="date">string format Date and Time</param>
        /// <param name="isHtml">return Html content</param>
        /// <returns>String</returns>
        public string DokuPageGetVersion(string pagename, string date, bool isHtml = false)
        {
            return this.DokuPageGetVersion(
                pagename,
                date.GetUnixTimeStamp(),
                isHtml
            );
        }
        /// <summary>
        /// Returns the raw/html Wiki text for a page with timestamp.
        /// <see cref="DokuPageGet"/>
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="timestamp">Unix timestamp (int)</param>
        /// <param name="isHtml">return Html content</param>
        /// <returns>String</returns>
        public string DokuPageGetVersion(string pagename, Int32 timestamp, bool isHtml = false)
        {
            try
            {
                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodPageGet pg = this.Get<Data.XMLMethodPageGet>(
                    ((isHtml) ? XmlRpcRequest.wiki_getPageHTMLVersion : XmlRpcRequest.wiki_getPageVersion),
                    pagename,
                    timestamp
                );
                if (string.IsNullOrWhiteSpace(pg.Params.Param.Value.String))
                {
                    throw new ArgumentNullException();
                }
                return pg.Params.Param.Value.String;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5017
                );
            }
        }
        #endregion

        #region public DokuPageGetVersions
        /// <summary>
        /// Returns the available versions of a Wiki page.
        /// The number of pages in the result is controlled via the recent configuration setting.
        /// The offset can be used to list earlier versions in the history.
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="offset">offset page (int)</param>
        /// <returns>class <see cref="Data.XMLMethodPageGetVersions"/>Data.XMLMethodPageGetVersions</returns>
        public Data.XMLMethodPageGetVersions DokuPageGetVersions(string pagename, Int32 offset)
        {
            try
            {
                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodPageGetVersions pi = this.Get<Data.XMLMethodPageGetVersions>(
                    XmlRpcRequest.wiki_getPageVersions,
                    pagename,
                    offset
                );
                if (pi == null)
                {
                    throw new ArgumentNullException();
                }
                return pi;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5018
                );
            }
        }
        #endregion

        #region public DokuPageInfo
        /// <summary>
        /// Returns information about a Wiki page.
        /// </summary>
        /// <example>
        /// Returns (array) an array containing the following data: 
        /// name = page name.
        /// lastModified = modification date as IXR_Date Object.
        /// author = author of the Wiki page.
        /// version = page version as timestamp.
        /// </example>
        /// <param name="pagename"></param>
        /// <returns>class <see cref="Data.XMLMethodPageInfo"/>Data.XMLMethodPageInfo</returns>
        public Data.XMLMethodPageInfo DokuPageInfo(string pagename)
        {
            try
            {
                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodPageInfo pi = this.Get<Data.XMLMethodPageInfo>(
                    XmlRpcRequest.wiki_getPageInfo,
                    pagename
                );
                if (pi == null)
                {
                    throw new ArgumentNullException();
                }
                return pi;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5013
                );
            }
        }
        #endregion

        #region public DokuPageInfoVersion
        /// <summary>
        /// Returns information about a Wiki page with DateTime.
        /// <see cref="DokuPageInfo"/>
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="date">DateTime format timestamp</param>
        /// <returns>class <see cref="Data.XMLMethodPageInfo"/>Data.XMLMethodPageInfo</returns>
        public Data.XMLMethodPageInfo DokuPageInfoVersion(string pagename, DateTime date)
        {
            return this.DokuPageInfoVersion(
                pagename,
                date.GetUnixTimeStamp()
            );
        }
        /// <summary>
        /// Returns information about a Wiki page with Date and Time string.
        /// <see cref="DokuPageInfo"/>
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="date">string format Date and Time</param>
        /// <returns>class <see cref="Data.XMLMethodPageInfo"/>Data.XMLMethodPageInfo</returns>
        public Data.XMLMethodPageInfo DokuPageInfoVersion(string pagename, string date)
        {
            return this.DokuPageInfoVersion(
                pagename,
                date.GetUnixTimeStamp()
            );
        }
        /// <summary>
        /// Returns information about a Wiki page with timestamp.
        /// <see cref="DokuPageInfo"/>
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="timestamp">Unix timestamp (int)</param>
        /// <returns>class <see cref="Data.XMLMethodPageInfo"/>Data.XMLMethodPageInfo</returns>
        public Data.XMLMethodPageInfo DokuPageInfoVersion(string pagename, Int32 timestamp)
        {
            try
            {
                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodPageInfo pi = this.Get<Data.XMLMethodPageInfo>(
                    XmlRpcRequest.wiki_getPageInfoVersion,
                    pagename,
                    timestamp
                );
                if (pi == null)
                {
                    throw new ArgumentNullException();
                }
                return pi;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5013
                );
            }
        }
        #endregion

        #region public DokuPageLinks
        /// <summary>
        /// Returns a list of all links contained in a Wiki page.
        /// </summary>
        /// <example>
        /// Return (array) each array item holds the following data:
        /// type = local/extern
        /// page = the wiki page (or the complete URL if extern)
        /// href = the complete URL
        /// </example>
        /// <param name="pagename">page name</param>
        /// <returns>class <see cref="Data.XMLMethodPageLinks"/>Data.XMLMethodPageLinks</returns>
        public Data.XMLMethodPageLinks DokuPageLinks(string pagename)
        {
            try
            {
                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodPageLinks pi = this.Get<Data.XMLMethodPageLinks>(
                    XmlRpcRequest.wiki_listLinks,
                    pagename
                );
                if (pi == null)
                {
                    throw new ArgumentNullException();
                }
                return pi;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5014
                );
            }
        }
        #endregion

        #region public DokuPageLinksBack
        /// <summary>
        /// Returns a list of backlinks of a Wiki page.
        /// See: https://www.dokuwiki.org/backlinks
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <returns>class <see cref="Data.XMLMethodPageLinksBack"/>Data.XMLMethodPageLinksBack</returns>
        public Data.XMLMethodPageLinksBack DokuPageLinksBack(string pagename)
        {
            try
            {
                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodPageLinksBack pl = this.Get<Data.XMLMethodPageLinksBack>(
                    XmlRpcRequest.wiki_getBackLinks,
                    pagename
                );
                if (pl == null)
                {
                    throw new ArgumentNullException();
                }
                return pl;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5016
                );
            }
        }
        #endregion

        #region public DokuPagePut
        /// <summary>
        /// Create or update Wiki Page.
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="rawtxt">raw Wiki text</param>
        /// <param name="sum">change summary</param>
        /// <param name="minor">minor</param>
        /// <returns>bool</returns>
        public bool DokuPagePut(string pagename, string rawtxt, string sum = null, bool minor = false)
        {
            try
            {
                // TODO: add attr
                this._CheckAuth();
                if (
                    (string.IsNullOrWhiteSpace(pagename)) ||
                    (string.IsNullOrWhiteSpace(rawtxt))
                   )
                {
                    throw new ArgumentNullException();
                }
                Data.XMLMethodBool ma = this.Get<Data.XMLMethodBool>(
                    XmlRpcRequest.wiki_putPage,
                    pagename,
                    rawtxt,
                    String.Empty
                );
                if (ma == null)
                {
                    throw new ArgumentNullException();
                }
                int result = 0;
                if (Int32.TryParse(ma.Params.Param.Value.Boolean, out result))
                {
                    return ((result == 1) ? true : false);
                }
                throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5024
                );
            }
        }
        #endregion

        #region public DokuPageAppend
        /// <summary>
        /// Append text to Wiki Page.
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="rawtxt">wiki markup/html Wiki text</param>
        /// <param name="isHtml">wiki markup/html selector</param>
        /// <param name="sum">change summary</param>
        /// <param name="minor">minor</param>
        /// <returns>bool</returns>
        public bool DokuPageAppend(string pagename, string rawtxt, bool isHtml = false, string sum = null, bool minor = false)
        {
            return this._DokuPageActions(
                XmlRpcPageAction.Append,
                pagename,
                rawtxt,
                isHtml,
                sum,
                minor
            );
        }
        #endregion

        #region public DokuPageInsert
        /// <summary>
        /// Insert text to Wiki Page.
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <param name="rawtxt">wiki markup/html Wiki text</param>
        /// <param name="isHtml">wiki markup/html selector</param>
        /// <param name="sum">change summary</param>
        /// <param name="minor">minor</param>
        /// <returns>bool</returns>
        public bool DokuPageInsert(string pagename, string rawtxt, bool isHtml = false, string sum = null, bool minor = false)
        {
            return this._DokuPageActions(
                XmlRpcPageAction.Insert,
                pagename,
                rawtxt,
                isHtml,
                sum,
                minor
            );
        }
        #endregion

        #region public DokuPageClear
        /// <summary>
        /// Clear all text from Wiki Page.
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <returns>bool</returns>
        public bool DokuPageClear(string pagename)
        {
            return this._DokuPageActions(
                XmlRpcPageAction.Insert,
                pagename,
                String.Empty,
                false,
                String.Empty,
                false
            );
        }
        #endregion

        #region public DokuPageDelete
        /// <summary>
        /// Delete Wiki Page.
        /// </summary>
        /// <param name="pagename">page name</param>
        /// <returns>bool</returns>
        public bool DokuPageDelete(string pagename)
        {
            return this._DokuPageActions(
                XmlRpcPageAction.Delete,
                pagename,
                String.Empty,
                false,
                String.Empty,
                false
            );
        }
        #endregion

        #region public DokuPagesChange
        /// <summary>
        /// Returns a list of recent changes since given DateTime.
        /// </summary>
        /// <param name="date">DateTime format timestamp</param>
        /// <returns>class <see cref="Data.XMLMethodPagesRecentChange"/>Data.XMLMethodPagesRecentChange</returns>
        public Data.XMLMethodPagesRecentChange DokuPagesChange(DateTime date)
        {
            return this.DokuPagesChange(
                date.GetUnixTimeStamp()
            );
        }
        /// <summary>
        /// Returns a list of recent changes since given Date andTime string.
        /// </summary>
        /// <param name="date">string format Date and Time</param>
        /// <returns>class <see cref="Data.XMLMethodPagesRecentChange"/>Data.XMLMethodPagesRecentChange</returns>
        public Data.XMLMethodPagesRecentChange DokuPagesChange(string date)
        {
            return this.DokuPagesChange(
                date.GetUnixTimeStamp()
            );
        }
        /// <summary>
        /// Returns a list of recent changes since given timestamp.
        /// As stated: Only the most recent change for each page is listed,
        /// regardless of how many times that page was changed.
        /// </summary>
        /// <example>
        /// Return (array) each array item holds the following data:
        /// name = page id
        /// lastModified = modification date as UTC timestamp
        /// author = author
        /// version = page version as timestamp
        /// </example>
        /// <param name="timestamp">Unix timestamp (int)</param>
        /// <returns>class <see cref="Data.XMLMethodPagesRecentChange"/>Data.XMLMethodPagesRecentChange</returns>
        public Data.XMLMethodPagesRecentChange DokuPagesChange(int timestamp)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodPagesRecentChange pi = this.Get<Data.XMLMethodPagesRecentChange>(
                    XmlRpcRequest.wiki_getRecentChanges,
                    timestamp.ToString()
                );
                if (pi == null)
                {
                    throw new ArgumentNullException();
                }
                return pi;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5025
                );
            }
        }
        #endregion

        #region public DokuMediaChange
        /// <summary>
        /// Returns a list of recent changes since given DateTime.
        /// </summary>
        /// <param name="date">DateTime format timestamp</param>
        /// <returns>class <see cref="Data.XMLMethodMediaRecentChange"/>Data.XMLMethodMediaRecentChange</returns>
        public Data.XMLMethodMediaRecentChange DokuMediaChange(DateTime date)
        {
            return this.DokuMediaChange(
                date.GetUnixTimeStamp()
            );
        }
        /// <summary>
        /// Returns a list of recent changes since given Date and Time string.
        /// </summary>
        /// <param name="date">string format Date and Time</param>
        /// <returns>class <see cref="Data.XMLMethodMediaRecentChange"/>Data.XMLMethodMediaRecentChange</returns>
        public Data.XMLMethodMediaRecentChange DokuMediaChange(string date)
        {
            return this.DokuMediaChange(
                date.GetUnixTimeStamp()
            );
        }
        /// <summary>
        /// Returns a list of recent changes since given timestamp.
        /// As stated: Only the most recent change for each page is listed,
        /// regardless of how many times that page was changed.
        /// </summary>
        /// <example>
        /// Return (array) each array item holds the following data:
        /// name = page id
        /// lastModified = modification date as UTC timestamp
        /// author = author
        /// version = page version as timestamp
        /// perms = media permissions
        /// size = media size in bytes
        /// </example>
        /// <param name="timestamp">Unix timestamp (int)</param>
        /// <returns>class <see cref="Data.XMLMethodMediaRecentChange"/>Data.XMLMethodMediaRecentChange</returns>
        public Data.XMLMethodMediaRecentChange DokuMediaChange(int timestamp)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodMediaRecentChange pi = this.Get<Data.XMLMethodMediaRecentChange>(
                    XmlRpcRequest.wiki_getRecentChanges,
                    timestamp.ToString()
                );
                if (pi == null)
                {
                    throw new ArgumentNullException();
                }
                return pi;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5025
                );
            }
        }
        #endregion

        #region public DokuAttachmentList
        /// <summary>
        /// Returns a list of media files in a given namespace.
        /// </summary>
        /// <example>
        /// id = media id
        /// file = name of the file
        /// size = size in bytes
        /// mtime = upload date as a timestamp
        /// lastModified =  modification date as XML-RPC Date object
        /// isimg = true if file is an image, false otherwise
        /// writable = true if file is writable, false otherwise
        /// perms = permissions of file
        /// </example>
        /// <returns>class <see cref="Data.XMLMethodAttachmentList"/>Data.XMLMethodAttachmentList</returns>
        public Data.XMLMethodAttachmentList DokuAttachmentList(string namesspace = null, string opt = null)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodAttachmentList at = this.Get<Data.XMLMethodAttachmentList>(
                    XmlRpcRequest.wiki_getAttachments,
                    ((string.IsNullOrWhiteSpace(namesspace)) ?
                        ((string.IsNullOrWhiteSpace(this._namespace)) ? _defaultns : this._namespace) : namesspace),
                    opt
                );
                if (at == null)
                {
                    throw new ArgumentNullException();
                }
                return at;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5027
                );
            }
        }
        #endregion

        #region public DokuAttachmentInfo
        /// <summary>
        /// Returns information about a media file.
        /// </summary>
        /// <example>
        /// size = size in bytes
        /// lastModified = modification date as XML-RPC Date object
        /// </example>
        /// <param name="id">Attachment id (file name)</param>
        /// <returns>class <see cref="Data.XMLMethodAttachmentInfo"/>Data.XMLMethodAttachmentInfo</returns>
        public Data.XMLMethodAttachmentInfo DokuAttachmentInfo(string id)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodAttachmentInfo ati = this.Get<Data.XMLMethodAttachmentInfo>(
                    XmlRpcRequest.wiki_getAttachmentInfo,
                    id
                );
                if (ati == null)
                {
                    throw new ArgumentNullException();
                }
                return ati;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5028
                );
            }
        }
        #endregion

        #region public DokuAttachmentGet
        /// <summary>
        /// Returns the object data of a media file.
        /// </summary>
        /// <param name="id">Attachment id (file name)</param>
        /// <returns>the data of the file in byte[] format</returns>
        public byte[] DokuAttachmentGet(string id)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodAttachmentGet at = this.Get<Data.XMLMethodAttachmentGet>(
                    XmlRpcRequest.wiki_getAttachment,
                    id
                );
                if (
                    (at == null) ||
                    (string.IsNullOrWhiteSpace(at.Params.Param.Value.Base64))
                   )
                {
                    throw new ArgumentNullException();
                }
                return Convert.FromBase64String(at.Params.Param.Value.Base64);
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5026
                );
            }
        }
        #endregion

        #region public DokuAttachmentPut
        /// <summary>
        /// Uploads a file as a given media id, source as full file path.
        /// </summary>
        /// <param name="path">full file path</param>
        /// <param name="owr"></param>
        /// <returns>Bolean</returns>
        public bool DokuAttachmentPut(string path, bool owr = false)
        {
            try
            {
                if (!File.Exists(path))
                {
                    throw new RpcXmlException(
                        className + Properties.ResourceCodeError.rpcErrorInt5031,
                        5031
                    );
                }

                byte[] obj = File.ReadAllBytes(path);
                
                return this.DokuAttachmentPut(
                    ((string.IsNullOrWhiteSpace(this._namespace)) ? _defaultns : this._namespace) + Path.GetFileName(path),
                    Convert.ToBase64String(obj),
                    owr
                );
            }
            catch (RpcXmlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5032
                );
            }
        }
        /// <summary>
        /// Uploads a file as a given media id, source as byte array.
        /// </summary>
        /// <param name="id">media id</param>
        /// <param name="obj">byte array</param>
        /// <param name="owr"></param>
        /// <returns></returns>
        public bool DokuAttachmentPut(string id, byte[] obj, bool owr = false)
        {
            return this.DokuAttachmentPut(
                id,
                Convert.ToBase64String(obj),
                owr
            );
        }
        /// <summary>
        /// Uploads a file as a given media id, source as Base64 encoded.
        /// </summary>
        /// <param name="id">Attachment id (file name)</param>
        /// <param name="base64">source string - Base64 encoded</param>
        /// <param name="owr"></param>
        /// <returns></returns>
        public bool DokuAttachmentPut(string id, string base64, bool owr = false)
        {
            try
            {
                this._CheckAuth();
                id = ((id.Contains(":")) ? id :
                    (((string.IsNullOrWhiteSpace(this._namespace)) ? _defaultns : this._namespace) + id)
                );

                Data.XMLMethodGetInt ad = this.Get<Data.XMLMethodGetInt>(
                    XmlRpcRequest.wiki_putAttachment,
                    id,
                    base64,
                    ""
                );
                if (ad == null)
                {
                    throw new ArgumentNullException();
                }
                int result = 0;
                if (Int32.TryParse(ad.Params.Param.Value.Int, out result))
                {
                    return ((result == 0) ? true : false);
                }
                throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5030
                );
            }
        }
        #endregion

        #region public DokuAttachmentRemove
        /// <summary>
        /// Deletes a file. Fails if the file is still referenced from any page in the wiki.
        /// </summary>
        /// <param name="id">Attachment id (file name)</param>
        /// <returns>Bolean</returns>
        public bool DokuAttachmentRemove(string id)
        {
            try
            {
                this._CheckAuth();
                Data.XMLMethodGetInt ad = this.Get<Data.XMLMethodGetInt>(
                    XmlRpcRequest.wiki_deleteAttachment,
                    id
                );
                if (ad == null)
                {
                    throw new ArgumentNullException();
                }
                int result = 0;
                if (Int32.TryParse(ad.Params.Param.Value.Int, out result))
                {
                    return ((result == 0) ? true : false);
                }
                throw new ArgumentNullException();
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5029
                );
            }
        }
        #endregion

        #endregion

        #region private _DokuPageActions (Update/Insert/Append/Clear/Delete)

        private bool _DokuPageActions(XmlRpcPageAction type, string pagename, string rawtxt, bool isHtml = false, string sum = null, bool minor = false)
        {
            try
            {
                string pageSrc = String.Empty;

                this._CheckAuth();
                if (string.IsNullOrWhiteSpace(pagename))
                {
                    throw new ArgumentNullException();
                }
                switch (type)
                {
                    case XmlRpcPageAction.Update:
                    case XmlRpcPageAction.Insert:
                    case XmlRpcPageAction.Append:
                        {
                            if (string.IsNullOrWhiteSpace(rawtxt))
                            {
                                throw new ArgumentNullException();
                            }
                            break;
                        }
                    case XmlRpcPageAction.Delete:
                    case XmlRpcPageAction.Clear:
                        {
                            break;
                        }
                    default:
                        {
                            return false;
                        }
                }
                if (string.IsNullOrWhiteSpace(sum))
                {
                    sum = string.Format(
                        Properties.ResourceCodeError.rpcCreateSummary,
                        type.ToString(),
                        DateTime.Now.ToString()
                    );
                }
                switch (type)
                {
                    case XmlRpcPageAction.Insert:
                    case XmlRpcPageAction.Append:
                        {
                            pageSrc = this.DokuPageGet(pagename, isHtml);
                            break;
                        }
                    case XmlRpcPageAction.Update:
                        {
                            pageSrc = rawtxt;
                            break;
                        }
                    case XmlRpcPageAction.Clear:
                        {
                            pageSrc = sum;
                            break;
                        }
                    case XmlRpcPageAction.Delete:
                        {
                            pageSrc = "";
                            break;
                        }
                }
                switch (type)
                {
                    case XmlRpcPageAction.Append:
                        {
                            pageSrc = ((string.IsNullOrWhiteSpace(pageSrc)) ? rawtxt :
                                string.Format("{0}\n{1}\n", rawtxt, pageSrc)
                            );
                            break;
                        }
                    case XmlRpcPageAction.Insert:
                        {
                            pageSrc = ((string.IsNullOrWhiteSpace(pageSrc)) ? rawtxt :
                                string.Format("{0}\n{1}\n", pageSrc, rawtxt)
                            );
                            break;
                        }
                    case XmlRpcPageAction.Update:
                    case XmlRpcPageAction.Delete:
                    case XmlRpcPageAction.Clear:
                        {
                            break;
                        }
                }
                return this.DokuPagePut(pagename, pageSrc, sum, minor);
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + "[" + type.ToString() + "] " + e.Message,
                    5033
                );
            }
        }
        #endregion

    }
}
