
namespace stDokuWiki.Connector
{
    public partial class RpcXml
    {
        #region Action Enum

        /// <summary>
        /// See API https://www.dokuwiki.org/devel:xmlrpc
        /// </summary>
        public enum XmlRpcRequest : int
        {
            /// <summary>
            /// Lists all pages within a given namespace.
            /// </summary>
            dokuwiki_getPagelist,
            /// <summary>
            /// Returns the DokuWiki version of the remote Wiki.
            /// </summary>
            dokuwiki_getVersion,
            /// <summary>
            /// Returns the current time at the remote wiki server as Unix timestamp.
            /// </summary>
            dokuwiki_getTime,
            /// <summary>
            /// Returns the XML RPC interface version of the remote Wiki.
            /// </summary>
            dokuwiki_getXMLRPCAPIVersion,
            /// <summary>
            /// Uses the provided credentials to execute a login and will set cookies.
            /// </summary>
            dokuwiki_login,
            /// <summary>
            /// Performs a fulltext search.
            /// </summary>
            dokuwiki_search,
            /// <summary>
            /// Returns the title of the wiki.
            /// </summary>
            dokuwiki_getTitle,
            /// <summary>
            /// Appends text to a Wiki page.
            /// </summary>
            dokuwiki_appendPage,
            /// <summary>
            /// Allows you to lock or unlock a whole bunch of pages at once.
            /// </summary>
            dokuwiki_setLocks,
            /// <summary>
            /// Returns number with the supported RPC API version.
            /// </summary>
            wiki_getRPCVersionSupported,
            /// <summary>
            /// Returns the permission of the given Wiki page.
            /// </summary>
            wiki_aclCheck,
            /// <summary>
            /// Returns the raw Wiki text for a page.
            /// </summary>
            wiki_getPage,
            /// <summary>
            /// Returns the raw Wiki text for a specific revision of a Wiki page.
            /// </summary>
            wiki_getPageVersion,
            /// <summary>
            /// Returns the available versions of a Wiki page.
            /// </summary>
            wiki_getPageVersions,
            /// <summary>
            /// Returns information about a Wiki page.
            /// </summary>
            wiki_getPageInfo,
            /// <summary>
            /// Returns information about a specific version of a Wiki page.
            /// </summary>
            wiki_getPageInfoVersion,
            /// <summary>
            /// Returns the rendered XHTML body of a Wiki page.
            /// </summary>
            wiki_getPageHTML,
            /// <summary>
            /// Returns the rendered HTML of a specific version of a Wiki page.
            /// </summary>
            wiki_getPageHTMLVersion,
            /// <summary>
            /// Saves a Wiki page.
            /// </summary>
            wiki_putPage,
            /// <summary>
            /// Returns a list of all links contained in a Wiki page.
            /// </summary>
            wiki_listLinks,
            /// <summary>
            /// Returns a list of all Wiki pages in the remote Wiki.
            /// </summary>
            wiki_getAllPages,
            /// <summary>
            /// Returns a list of backlinks of a Wiki page.
            /// </summary>
            wiki_getBackLinks,
            /// <summary>
            /// Returns a list of recent changes since given timestamp.
            /// </summary>
            wiki_getRecentChanges,
            /// <summary>
            /// Returns a list of recent changed media since given timestamp.
            /// </summary>
            wiki_getRecentMediaChanges,
            /// <summary>
            /// Returns a list of media files in a given namespace.
            /// </summary>
            wiki_getAttachments,
            /// <summary>
            /// Returns the binary data of a media attachments file.
            /// </summary>
            wiki_getAttachment,
            /// <summary>
            /// Returns information about a media attachments file.
            /// </summary>
            wiki_getAttachmentInfo,
            /// <summary>
            /// Uploads a file as a given media id.
            /// </summary>
            wiki_putAttachment,
            /// <summary>
            /// Deletes a file. Fails if the file is still referenced from any page in the wiki.
            /// </summary>
            wiki_deleteAttachment,
            /// <summary>
            /// Add an ACL rule. Use @groupname instead of user to add an ACL rule for a group.
            /// </summary>
            plugin_acl_addAcl,
            /// <summary>
            /// Delete any ACL rule matching the given scope and user.
            /// </summary>
            plugin_acl_delAcl
        }

        #endregion

        #region Page Action Enum

        /// <summary>
        /// Page Action: Insert, Update, Delete, None
        /// </summary>
        public enum XmlRpcPageAction : int
        {
            #pragma warning disable 1591
            None,
            Overwrite,
            Insert,
            Append,
            Update,
            Clear,
            Delete
            #pragma warning restore 1591
        }
        
        #endregion
    }
}
