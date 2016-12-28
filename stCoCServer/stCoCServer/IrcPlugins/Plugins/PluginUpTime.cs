using System;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD uptime
        private void PluginUpTime(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            this._Send(
                isPrivate, channel, nik,
                string.Format(
                    (string)Properties.Resources.ResourceManager.GetString("fmtUpTime", this._ci),
                    ((isPrivate) ? "" : nik),
                    ((isPrivate) ? "" : ", "),
                    (TimeSpan)(DateTime.Now - this.Conf.StatTime)
                )
            );
        }
        #endregion
    }
}
