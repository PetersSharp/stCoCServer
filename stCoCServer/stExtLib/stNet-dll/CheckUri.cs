using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if STCLIENTBUILD
namespace stClient

#else

namespace stNet
#endif
{
    public static class stCheckUri
    {
        public static bool CheckURL(this string url)
        {
            Uri outUri;
            try
            {
                if (
                    (Uri.TryCreate(url, UriKind.Absolute, out outUri)) &&
                    ((outUri.Scheme == Uri.UriSchemeHttp) || (outUri.Scheme == Uri.UriSchemeHttps))
                   )
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static string HeaderURL(this string url)
        {
            Uri outUri;
            try
            {
                if (
                    (Uri.TryCreate(url, UriKind.Absolute, out outUri)) &&
                    ((outUri.Scheme == Uri.UriSchemeHttp) || (outUri.Scheme == Uri.UriSchemeHttps))
                   )
                {
                    return outUri.DnsSafeHost;
                }
                return url.Substring(0, 15);
            }
            catch (Exception)
            {
                return url.Substring(0, 15);
            }
        }
    }
}
