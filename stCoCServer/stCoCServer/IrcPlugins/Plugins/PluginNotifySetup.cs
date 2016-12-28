using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD set Notify setup in channel
        private void PluginNotifySetup(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            if (!this._AuthorizeCmd(isPrivate, channel, nik, cmd))
            {
                return;
            }
            if ((cmd.Length < 3) || (string.IsNullOrWhiteSpace(cmd[2])))
            {
                this._Send(true, channel, nik, (string)Properties.Resources.ResourceManager.GetString("fmtNotifySetupHelp", this._ci));
                return;
            }
            switch (cmd[2])
            {
                case "start":
                    {
                        this.IRCPluginLoopClanNotifyStart(channel);
                        this._Send(true, channel, nik,
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtNotifySetupHelp", this._ci),
                                (string)Properties.Resources.ResourceManager.GetString("prnYes", this._ci)
                            )
                        );
                        break;
                    }
                case "stop":
                    {
                        this.IRCPluginLoopClanNotifyStop();
                        this._Send(true, channel, nik,
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("fmtNotifySetupHelp", this._ci),
                                (string)Properties.Resources.ResourceManager.GetString("prnNo", this._ci)
                            )
                        );
                        break;
                    }
                case "period":
                    {
                        if (cmd.Length > 3)
                        {
                            int ps = 0;
                            if (Int32.TryParse(cmd[3], out ps))
                            {
                                this.Conf.Opt.IRCPluginLoopClanNotifyPeriod.num = ps;
                                this._Send(true, channel, nik,
                                    string.Format(
                                        (string)Properties.Resources.ResourceManager.GetString("fmtNotifySetupPeriod", this._ci),
                                        ps
                                    )
                                );
                            }
                        }
                        break;
                    }
                default:
                    {
                        this._Send(true, channel, nik, (string)Properties.Resources.ResourceManager.GetString("fmtNotifySetupHelp", this._ci));
                        break;
                    }
            }
        }
        #endregion
    }
}
