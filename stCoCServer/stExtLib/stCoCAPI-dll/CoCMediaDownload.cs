using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stCore;
using stNet;
using System.IO;
using System.Data;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        private class CoCMediaDownload : IDisposable
        {
            private CoCAPI _parent = null;
            private string _basepath = null;
            private dynamic _ccl = null;

            public CoCMediaDownload(dynamic ccl, string basepath, CoCAPI parent)
            {
                this._ccl = ccl;
                this._parent = parent;
                this._basepath = basepath;
                if (string.IsNullOrWhiteSpace(this._basepath))
                {
                    throw new ArgumentNullException("Base Path");
                }
                if (this._ccl == null)
                {
                    throw new ArgumentNullException("Curl instance");
                }
                if (this._parent == null)
                {
                    throw new ArgumentNullException("Parent instance");
                }
            }
            ~CoCMediaDownload()
            {
                this.Dispose();
            }
            public void Dispose()
            {
                this._ccl = null;
            }

            public void Download(stCoCAPI.CoCAPI.CoCEnum.CoCFmtReq mediaid, DataTable dt)
            {
                CoCAPI.CoCMediaSetInfo cms = null;

                if (
                    (dt.Rows.Count == 0) ||
                    ((cms = CoCMediaSet.GetMediaSet(mediaid)) == null)
                   )
                {
                    return;
                }

                try
                {
                    this._CheckMediaDir(cms);

                    foreach (DataRow dr in dt.Rows)
                    {
                        if ((dr["ico"] == null) || (string.IsNullOrWhiteSpace((string)dr["ico"])))
                        {
                            continue;
                        }
                        foreach (int i in cms.msize)
                        {
                            string mediaPath = Path.Combine(
                                    this._basepath,
                                    cms.mpath[0],
                                    cms.mpath[1],
                                    cms.mdir,
                                    i.ToString(),
                                    (string)dr["ico"] + ".png"
                            );
                            if (File.Exists(mediaPath))
                            {
                                continue;
                            }
                            this._MediaDownload(
                                string.Format(
                                    @"{0}/{1}/{2}/{3}.png",
                                    Properties.Settings.Default.CoCMediaURL,
                                    cms.mdir,
                                    i.ToString(),
                                    (string)dr["ico"]
                                ),
                                mediaPath
                            );
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            private void _MediaDownload(string url, string path)
            {
                try
                {
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogInfo(
                            string.Format(
                                Properties.Resources.CoCMediaDownloadInfo,
                                url, ""
                            )
                        );
                    }
#if DEBUG
                    else
                    {
                        stConsole.WriteLine(" - HTTP Get: " + url);
                    }
#endif
                    this._ccl.GetFile(url, path);
                }
                catch (Exception e)
                {
                    if (!File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    if (this._parent.isLogEnable)
                    {
                        this._parent._ilog.LogError(
                            string.Format(
                                Properties.Resources.CoCMediaDownloadInfo,
                                "error - ", e.Message
                            )
                        );
                    }
#if DEBUG
                    else
                    {
                        stConsole.WriteLine(" - HTTP Get error: " + e.Message);
                    }
#endif
                }
            }
            private void _CheckMediaDir(CoCMediaSetInfo cms)
            {
                try
                {
                    foreach (int i in cms.msize)
                    {
                        string path = Path.Combine(
                                this._basepath,
                                cms.mpath[0],
                                cms.mpath[1],
                                cms.mdir,
                                i.ToString()
                        );
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

        }
    }
}
