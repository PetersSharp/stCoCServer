
namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD set user mode in channel
        private void PluginMode(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            if (!this._AuthorizeCmd(isPrivate, channel, nik, cmd))
            {
                return;
            }

            string mode = string.Join(" ", cmd, 2, (cmd.Length - 2));

            this._SetUserMode(channel, mode);
            this._Send(
                true, channel, nik,
                string.Format(
                    (string)Properties.Resources.ResourceManager.GetString("cmdSetUserModeOk", this._ci),
                    nik,
                    channel,
                    mode
                )
            ); 
        }
        #endregion
    }
}

