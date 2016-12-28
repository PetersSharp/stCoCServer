
namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD set channel Topic
        private void PluginTopic(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            if (!this._AuthorizeCmd(isPrivate, channel, nik, cmd))
            {
                return;
            }

            string topic = string.Join(" ", cmd, 2, (cmd.Length - 2));

            this._SendTopic(channel, topic);
            this._Send(
                true, channel, nik,
                string.Format(
                    (string)Properties.Resources.ResourceManager.GetString("cmdSetTopicOk", this._ci),
                    nik,
                    channel,
                    topic
                )
            ); 
        }
        #endregion

    }
}
