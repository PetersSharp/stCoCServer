using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD set Language setup in channel
        private void PluginLangSetup(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            if (!this._AuthorizeCmd(isPrivate, channel, nik, cmd))
            {
                return;
            }
            if ((cmd.Length < 3) || (string.IsNullOrWhiteSpace(cmd[2])))
            {
                this._Send(true, channel, nik, (string)Properties.Resources.ResourceManager.GetString("fmtLangSetupHelp", this._ci));
                return;
            }
            try
            {
                CultureInfo ci = CultureInfo.GetCultureInfo(cmd[2]);
                this._ci = ci;
            }
            catch (Exception)
            {
                this._Send(true, channel, nik,
                    string.Format(
                        (string)Properties.Resources.ResourceManager.GetString("fmtLangSetupError", this._ci),
                        cmd[2]
                    )
                );
            }
            
            this.Conf.Opt.WEBLANGDefault.value = cmd[2];

            if (this.Conf.Api != null)
            {
                this.Conf.Api.DefaultLang = this.Conf.Opt.WEBLANGDefault.value;
            }
            if (this.Conf.HttpSrv != null)
            {
                this.Conf.HttpSrv.DefaultLang = this.Conf.Opt.WEBLANGDefault.value;
            }
            this._Send(true, channel, nik,
                string.Format(
                    (string)Properties.Resources.ResourceManager.GetString("fmtLangSetupOk", this._ci),
                    cmd[2],
                    (string)Properties.Resources.ResourceManager.GetString("prnYes", this._ci)
                )
            );
        }
        #endregion
    }
}

