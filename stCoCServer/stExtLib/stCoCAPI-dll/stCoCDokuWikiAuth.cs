using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stDokuWiki;
using stDokuWiki.AuthManager;
using stDokuWiki.Data;
using System.Data;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        public void DokuWikiAuthInit(string path, string group)
        {
            if (this._cocDWAuth == null)
            {
                try
                {
                    this._cocDWAuth = new DokuAuthManager(path, group);
                }
                catch (Exception e)
                {
                    this.DokuWikiAuthException(e);
                    this._cocDWAuth = null;
                }
            }
        }
        public void DokuWikiAuthAdd(string tag, string login, string passwd, string name, string group)
        {
            if (
                (this._cocDWAuth == null) ||
                (string.IsNullOrWhiteSpace(login)) ||
                (string.IsNullOrWhiteSpace(passwd))
               )
            {
                return;
            }
            try
            {
                DokuAuthUser dau = this._cocDWAuth.UserAdd(login, passwd, name, group);
            }
            catch (Exception e)
            {
                this.DokuWikiAuthException(e);
            }
        }
        public void DokuWikiAuthAdd(DokuAuthUser dauin)
        {
            if (
                (dauin == null) ||
                (this._cocDWAuth == null)
               )
            {
                return;
            }
            try
            {
                DokuAuthUser dau = this._cocDWAuth.UserAdd(dauin);
            }
            catch (Exception e)
            {
                this.DokuWikiAuthException(e);
            }
        }
        public void DokuWikiAuthDel(string login, string tag = null)
        {
            if (
                (this._cocDWAuth == null) ||
                (string.IsNullOrWhiteSpace(login))
               )
            {
                return;
            }
            try
            {
                this._cocDWAuth.UserDelete(
                    new List<DokuAuthUser>()
                    {
                        new DokuAuthUser() { Login = login }
                    }
                );
            }
            catch (Exception e)
            {
                this.DokuWikiAuthException(e);
            }
        }
        public void DokuWikiAuthSave()
        {
            if (this._cocDWAuth == null)
            {
                return;
            }
            try
            {
                this._cocDWAuth.UserSave();
            }
            catch (Exception e)
            {
                this.DokuWikiAuthException(e);
            }
        }
        private void DokuWikiAuthException(Exception e)
        {
            if (this.isLogEnable)
            {
                if (e.GetType() == typeof(RpcXmlException))
                {
                    RpcXmlException er = e as RpcXmlException;
                    this._ilog.LogError("[" + er.errcode + "] " + er.Message);
                }
                else
                {
                    this._ilog.LogError(e.Message);
                }
            }
        }
    }
}
