using System;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD time
        private void PluginTime(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            this._Send(
                isPrivate, channel, nik,
                string.Format(
                    (string)Properties.Resources.ResourceManager.GetString("fmtTime", this._ci),
                    ((isPrivate) ? "" : nik),
                    ((isPrivate) ? "" : ", "),
                    DateTime.Now.ToString()
                )
            );
        }
        #endregion
    }
}
