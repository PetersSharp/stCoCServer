using System.Collections.Generic;
using System;

namespace stGeo
{
    public static partial class MaxMindUtil
    {
        public static string getCountryName(this int idx)
        {
            stGeoCountry geo = geoCountrys.Find(x => (x.Id == idx));
            return (string)((geo != null) ? geo.Name : geoCountrys[0].Name);
        }
        public static string getCountryName(this string needle)
        {
            stGeoCountry geo = null;
            if (needle.Length == 2)
            {
                geo = geoCountrys.Find(x => (x.Tag.Equals(needle.ToUpperInvariant())));
            }
            else
            {
                geo = geoCountrys.Find(x => (x.Name.Contains(needle)));
            }
            return (string)((geo != null) ? geo.Name : geoCountrys[0].Name);
        }
        public static string getCountryTagById(this int idx)
        {
            stGeoCountry geo = geoCountrys.Find(x => (x.Id == idx));
            return (string)((geo != null) ? geo.Tag : geoCountrys[0].Tag);
        }
        public static int getCountryIdByTag(this string tag)
        {
            stGeoCountry geo = geoCountrys.Find(x => (x.Tag.Equals(tag.ToUpperInvariant())));
            return (int)((geo != null) ? geo.Id : 0);
        }
        public static int getCountryIdByTagQuoted(this string tag)
        {
            int start = ((tag.StartsWith("\"")) ? 1 : 0),
                end = ((tag.EndsWith("\"")) ? (tag.Length - 2) : (tag.Length - 1));

            stGeoCountry geo = geoCountrys.Find(x => (x.Tag.Equals(tag.Substring(start, end).Trim().ToUpperInvariant() )));
            return (int)((geo != null) ? geo.Id : 0);
        }
        public static int getCountryIdByName(this string name)
        {
            stGeoCountry geo = geoCountrys.Find(x => (x.Name.Contains(name)));
            return (int)((geo != null) ? geo.Id : 0);
        }
    }
}
