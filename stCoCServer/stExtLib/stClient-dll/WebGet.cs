using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using stCore;
using System.Drawing;

namespace stClient
{
    public class WebGet
    {
        public static string GetJsonString(string url, IMessage iLog)
        {
            HttpWebResponse res = null;

            try
            {
                res = (HttpWebResponse)WebGet._WebGet(url, iLog);
                using (Stream ds = res.GetResponseStream())
                {
                    using (StreamReader rd = new StreamReader(ds))
                    {
                        return rd.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
        }
        public static Image GetImage(string url, IMessage iLog)
        {
            HttpWebResponse res = null;

            try
            {
                res = (HttpWebResponse)WebGet._WebGet(url, iLog);
                if (!res.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException(
                        string.Format(
                            Properties.Resources.WebGetErrorException2,
                            url,
                            ((int)res.StatusCode).ToString()
                        )
                    );
                }
                using (Stream ds = res.GetResponseStream())
                {
                    return (Image)Image.FromStream(ds);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
        }
        private static HttpWebResponse _WebGet(string url, IMessage iLog)
        {
            HttpWebResponse res = null;

            try
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    res = (HttpWebResponse)req.GetResponse();
                }
                catch (WebException e)
                {
                    res = (HttpWebResponse)e.Response;
                    if (res == null)
                    {
                        throw e;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                switch (res.StatusCode)
                {
                    case HttpStatusCode.OK:
                    case HttpStatusCode.Moved:
                    case HttpStatusCode.Redirect:
                        {
                            return res;
                        }
                    default:
                        {
                            throw new ArgumentException(
                                string.Format(
                                    Properties.Resources.WebGetErrorException1,
                                    url,
                                    ((int)res.StatusCode).ToString(),
                                    res.StatusCode.ToString()
                                )
                            );
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
