
namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD say
        private void PluginSay(bool isPrivate, string channel, string nik, string [] cmd, string cmds)
        {
            string cmda = cmds.Substring(cmd[0].Length);
            if (string.IsNullOrWhiteSpace(cmda))
            {
                this.Conf.Irc.SendNotice(nik, (string)Properties.Resources.ResourceManager.GetString("fmtSpeakHelp", this._ci));
            }
            else
            {
                this._Send(
                    isPrivate, channel, nik,
                    string.Format(
                        (string)Properties.Resources.ResourceManager.GetString("fmtSpeak", this._ci),
                        ((isPrivate) ? "" : nik),
                        ((isPrivate) ? "" : ", "),
                        cmda
                    )
                );
            }
        }
        #endregion
    }
}
