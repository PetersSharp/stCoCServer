using System.Reflection;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD version
        private void PluginVersion(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            this._Send(
                isPrivate, channel, nik,
                string.Format(
                    Properties.Settings.Default.setVersion,
                    ((isPrivate) ? "" : nik),
                    ((isPrivate) ? "" : ", "),
                    Assembly.GetEntryAssembly().GetName().Version.ToString()
                )
            );
        }
        #endregion
    }
}
