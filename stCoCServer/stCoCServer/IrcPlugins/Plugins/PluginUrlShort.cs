using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using stNet;
using stNet.stWebServerUtil;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD URL short

        private static readonly List<dynamic> PluginUrlShort_ShortApiService = new List<dynamic>() {
            new string [] { "rlu", "http://rlu.ru/index.sema?a=api&preview=0&link="},
            new string [] { "tinyurl", "http://tinyurl.com/api-create.php?url=" }
        };
        private static readonly string PluginUrlShort_regExp = @"http(s?)://\S+";

        private void PluginUrlShort(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            if (cmd.Length < 2)
            {
                this.Conf.Irc.SendNotice(nik, (string)Properties.Resources.ResourceManager.GetString("fmtUrlShortHelp", this._ci));
                return;
            }
            if (
                (!string.IsNullOrWhiteSpace(cmd[1])) &&
                (Regex.IsMatch(cmd[1], PluginUrlShort_regExp, RegexOptions.IgnoreCase | RegexOptions.Multiline))
               )
            {
                stWebClient wcl = null;
                List<string> UrlList = new List<string>();
                List<string> ShortList = new List<string>();
                string [] ApiService = null;

                ApiService = ((cmd.Length > 2) ?
                    IrcCommand.PluginUrlShort_ShortApiService.Find(o =>
                        {
                            return (o[0].Equals(cmd[2]));
                        }
                    ) :
                    IrcCommand.PluginUrlShort_ShortApiService[0]
                );
                ApiService = ((ApiService == null) ? IrcCommand.PluginUrlShort_ShortApiService[0] : ApiService);

                foreach (var match in new Regex(PluginUrlShort_regExp).Matches(cmd[1]))
                {
                    UrlList.Add(match.ToString());
                }
                try
                {
                    wcl = new stWebClient();
                    wcl.wUserAgent = HttpUtil.GetHttpUA(Properties.Settings.Default.setCoCHttpClient);

                    foreach (string URL in UrlList)
                    {
                        ShortList.Add(wcl.DownloadString(ApiService[1] + URL));
                    }
                }
                catch (Exception e)
                {
                    this._Send(
                        true, channel, nik,
                            (
                                string.Format(
                                    (string)Properties.Resources.ResourceManager.GetString("cmdUrlShortError", this._ci),
                                    cmd[1],
                                    e.Message
                                )
                             ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Red)
                    );
                    return;
                }
                finally
                {
                    if (wcl != null)
                    {
                        wcl.Dispose();
                    }
                }
                this._Send(
                     isPrivate, channel, nik,
                         string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("cmdUrlShortOK", this._ci),
                            nik,
                            cmd[1],
                            string.Join(", ", ShortList)
                         )
                );
            }
            else
            {
                this._Send(
                    true, channel, nik,
                        (
                            string.Format(
                                (string)Properties.Resources.ResourceManager.GetString("cmdUrlShortError", this._ci),
                                cmd[1],
                                "not valid URL"
                            )
                         ).ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Red)
                );
            }
        }
        #endregion
    }
}
