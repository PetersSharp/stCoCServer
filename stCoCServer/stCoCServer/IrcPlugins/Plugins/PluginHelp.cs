using stNet;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD Help
        private void PluginHelp(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            string[] helplist = new string[] {
                (string)Properties.Resources.ResourceManager.GetString("fmtSpeakHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtTimeHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtUpTimeHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtUrlShortHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtClanHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtClanInfoHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtClanListHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtClanPlayerHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtClanDonationHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtWarLastHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtWarLogHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtSetCommandHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtNotifySetupHelp", this._ci),
                (string)Properties.Resources.ResourceManager.GetString("fmtLangSetupHelp", this._ci)
            };
            foreach (string hlp in helplist)
            {
                this._Send(
                    true, channel, nik, hlp.ColorText(IrcFormatText.Color.White, IrcFormatText.Color.DarkGreen)
                );
            }
        }
        #endregion
    }
}
