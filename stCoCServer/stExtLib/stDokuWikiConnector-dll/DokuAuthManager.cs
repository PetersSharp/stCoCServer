using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using stDokuWiki;
using stDokuWiki.Data;
using stDokuWiki.Crypt;

namespace stDokuWiki.AuthManager
{
    /// <summary>
    /// Manager DokuWiki Auth (file: conf/users.auth.php)
    /// </summary>
    /// <code>
    /// using stDokuWiki;
    /// using stDokuWiki.Data;
    /// using stDokuWiki.AuthManager;
    /// </code>
    /// <exception cref="RpcXmlException">RpcXmlException thrown if error internal DokuAuthManager class</exception>
    /// <exception cref="Exception">Exception thrown if other error</exception>
    public partial class DokuAuthManager
    {
        #region Variables

        private const string className = "Auth Manager: ";
        private const string authDir = "conf";
        private const string authFile = "users.auth.php";
        private const string groupDefault = "questrpc";
        private const string authHeader = "# users.auth.php\n# <?php exit()?>\n\n";

        private List<DokuAuthUser> _userList = null;
        private string _path = String.Empty;
        private string _group = String.Empty;
        private object _lockAuthFile = null;
        private bool _isChanged = false;

        /// <summary>
        /// (String) Set DokuWiki local path
        /// </summary>
        public string DokuWikiPath
        {
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    this._path = Path.Combine(value, authDir, authFile);
                }
                else
                {
                    throw new RpcXmlException(
                        string.Format(
                            Properties.ResourceCodeError.rpcErrorIntFormat,
                            className,
                            "Path"
                        ),
                        5080
                    );
                }
            }
        }
        /// <summary>
        /// (String) Set DokuWiki users group
        /// </summary>
        public string DokuWikiGroup
        {
            set
            {
                this._group = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// (class) DokuAuthManager Constructor
        /// </summary>
        /// <code>
        /// try
        /// {
        ///     DokuAuthManager dam = new DokuAuthManager("/path/to/dokuwiki/root/dir","mygroup");
        /// }
        /// catch (RpcXmlException e)
        /// {
        ///     Console.WriteLine("[" + e.errcode + "] " +e.Message); return;
        /// }
        /// catch (Exception e)
        /// {
        ///     Console.WriteLine(e.Message); return;
        /// }
        /// </code>
        /// <param name="path">path to DokuWiki root folder</param>
        /// <param name="group">default DokuWiki users group</param>
        public DokuAuthManager(string path, string group = null)
        {
            this.DokuWikiPath = path;
            this.DokuWikiGroup = group;
            this._userList = new List<DokuAuthUser>();
            this._lockAuthFile = new Object();
        }

        #endregion

        #region Public Method

        #region public UserAdd

        /// <summary>
        /// (Doku Manager) Add users to auth table (input DokuAuthUser)
        /// </summary>
        /// <code>
        /// dam.UserAdd(
        ///   new DokuAuthUser() 
        ///   { 
        ///      Login = "userLogin",
        ///      Password = "pwd1234",
        ///      Name = "Nikolas",
        ///      Email = "Nikolas@nomail.com",
        ///      Group = "personalGroup"
        ///   }
        /// );
        /// dam.AuthSave();
        /// </code>
        /// <param name="user">Class <see cref="Data.DokuAuthUser"/>DokuAuthUser</param>
        /// <returns>Class <see cref="Data.DokuAuthUser"/>DokuAuthUser</returns>
        public DokuAuthUser UserAdd(DokuAuthUser user)
        {
            return this._UserAdd(user);
        }

        /// <summary>
        /// (Doku Manager) Add user to auth table (input strings)
        /// </summary>
        /// <code>
        /// dam.UserAdd(
        ///   "userLogin",
        ///   "pwd1234",
        ///   "Nikolas",
        ///   "personalGroup",
        ///   "Nikolas@nomail.com"
        /// );
        /// dam.AuthSave();
        /// </code>
        /// <param name="login">string DokuWiki user Login</param>
        /// <param name="passwd">string DokuWiki user Password</param>
        /// <param name="name">string DokuWiki user Name</param>
        /// <param name="group">string DokuWiki user Group, null is default</param>
        /// <param name="email">string user E-Mail, null is default</param>
        /// <returns>Class <see cref="Data.DokuAuthUser"/>DokuAuthUser</returns>
        public DokuAuthUser UserAdd(string login, string passwd, string name, string group = null, string email = null)
        {
            return this._UserAdd(
                new DokuAuthUser() { Login = login, Password = passwd, Name = name, Group = group, Email = email }
            );
        }

        /// <summary>
        /// (Doku Manager) Add users to auth table
        /// </summary>
        /// <code>
        /// List&lt;DokuAuthUser&gt; uadd = new List&lt;DokuAuthUser&gt;()
        /// {
        ///     new DokuAuthUser()
        ///     { 
        ///       Login = "userLogin",
        ///       Password = "pwd1234",
        ///       Name = "Nikolas",
        ///       Email = "Nikolas@nomail.com",
        ///       Group = "personalGroup"
        ///     },
        ///     new DokuAuthUser() { ... },
        /// };
        /// dam.UsersAdd(uadd);
        /// dam.AuthSave();
        /// </code>
        /// <param name="userlst"></param>
        public void UsersAdd(List<DokuAuthUser> userlst)
        {
            if (!this._CheckMethodParam(userlst))
            {
                return;
            }
            lock (_lockAuthFile)
            {
                foreach (var user in userlst)
                {
                    this._UserAdd(user);
                }
            }
        }
        #endregion

        #region public UserGet
        /// <summary>
        /// (Doku Manager) Get user from auth table
        /// </summary>
        /// <code>
        /// DokuAuthUser dau = dam.UserGet("Nikolas");
        /// if (dau != null)
        /// {
        ///    Console.WriteLine(
        ///       dau.Name + " : " + dau.Email
        ///    );
        /// }
        /// </code>
        /// <param name="userlogin"></param>
        /// <returns>class <see cref="Data.DokuAuthUser"/>Data.DokuAuthUser</returns>
        public DokuAuthUser UserGet(string userlogin)
        {
            if (
                (!this._CheckMethodList()) ||
                (string.IsNullOrWhiteSpace(userlogin))
               )
            {
                return default(DokuAuthUser);
            }
            lock (_lockAuthFile)
            {
                return (DokuAuthUser)this._userList.SingleOrDefault(o => o.Login.Equals(userlogin));
            }
        }
        #endregion

        #region public UserDelete
        /// <summary>
        /// (Doku Manager) Delete user from auth table
        /// </summary>
        /// <code>
        /// List&lt;DokuAuthUser&gt; udel = new List&lt;DokuAuthUser&gt;()
        /// {
        ///     new DokuAuthUser() { Login = "userLogin" },
        ///     new DokuAuthUser() { ... },
        /// };
        /// dam.UserDelete(udel);
        /// dam.AuthSave();
        /// </code>
        /// <param name="userlst"></param>
        public void UserDelete(List<DokuAuthUser> userlst)
        {
            if (!this._CheckMethodParam(userlst))
            {
                return;
            }
            lock (_lockAuthFile)
            {
                foreach (var user in userlst)
                {
                    DokuAuthUser userdel = this._userList.SingleOrDefault(o => o.Login.Equals(user.Login));
                    if (userdel == null)
                    {
                        continue;
                    }
                    this._userList.Remove(userdel);
                    this._isChanged = true;
                }
            }
        }
        #endregion

        #region public UserFind
        /// <summary>
        /// (Doku Manager) Find users where Login or Name contains usermask
        /// </summary>
        /// <code>
        /// List&lt;DokuAuthUser&gt; ldau = dam.UserFind("nikol");
        /// if (ldau != null)
        /// {
        ///    foreach (DokuAuthUser dau in idau)
        ///    {
        ///       Console.WriteLine(
        ///          dau.Login + " : " + dau.Name + " : " + dau.Email
        ///       );
        ///    }
        /// }
        /// </code>
        /// <param name="usermask">contains user Login or Name</param>
        /// <returns>List&lt;DokuAuthUser&gt;</returns>
        public List<DokuAuthUser> UserFind(string usermask)
        {
            if (
                (!this._CheckMethodList()) ||
                (string.IsNullOrWhiteSpace(usermask))
               )
            {
                return default(List<DokuAuthUser>);
            }
            lock (_lockAuthFile)
            {
                return (List<DokuAuthUser>)this._userList
                    .Where(o => o.Login.Contains(usermask) || o.Name.Contains(usermask))
                    .ToList<DokuAuthUser>();
            }
        }
        #endregion

        #region public UserList
        /// <summary>
        /// (Doku Manager) Get all users from auth table
        /// </summary>
        /// <code>
        /// List&lt;DokuAuthUser&gt; ldau = dam.UserList();
        /// if (ldau != null)
        /// {
        ///    foreach (DokuAuthUser dau in idau)
        ///    {
        ///       Console.WriteLine(
        ///          dau.Login + " : " + dau.Name + " : " + dau.Email
        ///       );
        ///    }
        /// }
        /// </code>
        /// <returns>List&lt;DokuAuthUser&gt;</returns>
        public List<DokuAuthUser> UserList()
        {
            if (!this._CheckMethodList())
            {
                return default(List<DokuAuthUser>);
            }
            lock (_lockAuthFile)
            {
                List<DokuAuthUser> ldau = new List<DokuAuthUser>();
                this._userList.CopyTo(ldau.ToArray());
                return (List<DokuAuthUser>)ldau;
            }
        }
        #endregion

        #region public UserSave
        /// <summary>
        /// (Doku Manager) Save user auth table
        /// </summary>
        /// <code>
        /// dam.AuthSave();
        /// </code>
        public void UserSave()
        {
            if (!this._isChanged)
            {
                return;
            }

            this._CheckDokuAuth();

            if (this._userList.Count == 0)
            {
                throw new RpcXmlException(
                    string.Format(
                        Properties.ResourceCodeError.rpcErrorIntFormat,
                        className,
                        "Auth table is empty.."
                    ),
                    5084
                );
            }

            StringBuilder sb = new StringBuilder(authHeader);
            lock (_lockAuthFile)
            {
                this._userList.ForEach(o =>
                {
                    if (!string.IsNullOrWhiteSpace(o.PasswdHash))
                    {
                        sb.AppendLine(
                            string.Format(
                                "{0}:{1}:{2}:{3}:{4}",
                                o.Login,
                                o.PasswdHash,
                                o.Name,
                                o.Email,
                                o.Group
                            )
                        );
                    }
                });
                if (sb.Length > 0)
                {
                    File.WriteAllText(this._path, sb.ToString());
                }
            }
            sb.Clear();
            this._isChanged = false;
        }
        #endregion

        #endregion

        #region Private Method

        private void _CheckDokuAuth()
        {
            if (!File.Exists(this._path))
            {
                throw new RpcXmlException(
                    string.Format(
                        Properties.ResourceCodeError.rpcErrorIntFormat,
                        className,
                        "Path not found"
                    ),
                    5081
                );
            }
        }
        private bool _CheckMethodParam(List<DokuAuthUser> userlst)
        {
            if (
                (userlst == null) ||
                (userlst.Count == 0)
               )
            {
                return false;
            }
            return this._CheckMethodList();
        }
        private bool _CheckMethodList()
        {
            if (this._userList.Count == 0)
            {
                this._GetDokuAuth();

                if (this._userList.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }
        private void _GetDokuAuth()
        {
            try
            {
                this._CheckDokuAuth();

                lock (_lockAuthFile)
                {
                    string[] lines = File.ReadAllLines(this._path, Encoding.UTF8);
                    if (lines == null)
                    {
                        throw new RpcXmlException(
                            string.Format(
                                Properties.ResourceCodeError.rpcErrorIntFormat,
                                className,
                                "DokuWiki auth file is empty.."
                            ),
                            5082
                        );
                    }

                    this._userList.Clear();

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (
                            (string.IsNullOrWhiteSpace(lines[i])) ||
                            (lines[i].StartsWith("#"))
                           )
                        {
                            continue;
                        }
                        string[] items = lines[i].Split(':');
                        if (
                            (items == null) ||
                            (items.Length != 5)
                           )
                        {
                            continue;
                        }
                        DokuAuthUser dau = new DokuAuthUser()
                        {
                            Login = items[0],
                            PasswdHash = items[1],
                            Name = items[2],
                            Email = items[3],
                            Group = items[4],
                            Password = String.Empty
                        };
                        this._userList.Add(dau);
                    }
                }
            }
            catch (RpcXmlException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new RpcXmlException(
                    className + e.Message,
                    5083
                );
            }
        }

        private DokuAuthUser _UserAdd(DokuAuthUser user)
        {
            DokuAuthUser userexist = this._userList.SingleOrDefault(o => o.Login.Equals(user.Login));
            if (userexist == null)
            {
                if (
                    (string.IsNullOrWhiteSpace(user.Password)) &&
                    (string.IsNullOrWhiteSpace(user.PasswdHash))
                   )
                {
                    return default(DokuAuthUser);
                }
                user.PasswdHash = ((string.IsNullOrWhiteSpace(user.PasswdHash)) ? CryptUtils.Crypt(user.Password) : user.Password);
                user.Group = ((string.IsNullOrWhiteSpace(user.Group)) ?
                    ((string.IsNullOrWhiteSpace(this._group)) ? groupDefault : this._group) : user.Group
                );
                this._userList.Add(user);
                userexist = user;
            }
            else
            {
                userexist.Name = user.Name;
                userexist.Email = user.Email;
                userexist.Group = ((string.IsNullOrWhiteSpace(user.Group)) ? userexist.Group : user.Group);
                userexist.PasswdHash =
                    ((string.IsNullOrWhiteSpace(user.PasswdHash)) ?
                        ((string.IsNullOrWhiteSpace(user.Password)) ? userexist.PasswdHash : CryptUtils.Crypt(user.Password)) :
                        user.PasswdHash
                    );
            }
            this._isChanged = true;
            return userexist;
        }

        #endregion

    }
}
