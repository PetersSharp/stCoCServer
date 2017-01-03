using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki DokuAuthUser ( class DokuAuthManager )

    /// <summary>
    /// class DokuWiki Auth user ( use from class DokuAuthManager )
    /// </summary>
    [Serializable]
    public class DokuAuthUser
    {
        /// <summary>
        /// (String) User Login
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// (String) User Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// (String) User real Name, or description
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// (String) User Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// (String) User Group, default: "questrpc" 
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// (String) User MD5 Salted Password Hash,
        /// PHP function crypt() compatible
        /// </summary>
        public string PasswdHash { get; set; }
        /// <summary>
        /// (String) User CoC Tag id
        /// </summary>
        public string Tag { get; set; }
    }
    #endregion
}
