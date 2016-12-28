using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using stNet;
using stNet.stWebServerUtil;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD Context HTTP Title
        private static readonly string PluginContextUrlTitle_regExp = @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>";

        private void PluginContextUrlTitle(bool isPrivate, string channel, string nik, string[] cmd, string cmds)
        {
            if (
                (string.IsNullOrWhiteSpace(cmds)) ||
                (!Regex.IsMatch(cmds, PluginUrlShort_regExp, RegexOptions.IgnoreCase | RegexOptions.Multiline))
               )
            {
                return;
            }

            stWebClient wcl = null;
            List<string> UrlList = new List<string>();

            foreach (var match in new Regex(PluginUrlShort_regExp).Matches(cmds))
            {
                UrlList.Add(match.ToString());
            }
            try
            {
                wcl = new stWebClient();
                wcl.wUserAgent = HttpUtil.GetHttpUA(Properties.Settings.Default.setCoCHttpClient);

                foreach (string URL in UrlList)
                {
                    string src = wcl.DownloadString(URL);
                    if (string.IsNullOrWhiteSpace(src))
                    {
                        continue;
                    }
                    Match titleMatch = Regex.Match(src, PluginContextUrlTitle_regExp, RegexOptions.IgnoreCase);
                    if (string.IsNullOrWhiteSpace(titleMatch.Groups["Title"].Value))
                    {
                        continue;
                    }
                    this._Send(
                        isPrivate, channel, nik,
                        string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("fmtFetchTitle", this._ci),
                            nik,
                            URL,
                            titleMatch.Groups["Title"].Value
                        )
                    );
                }

            }
            catch (Exception e)
            {
                this.Conf.ILog.LogError(
                    string.Format(
                        (string)Properties.Resources.ResourceManager.GetString("cmdFetchTitleError", this._ci),
                        IrcCommand.thisClass,
                        e.Message
                    )
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
        }
        #endregion
    }
}