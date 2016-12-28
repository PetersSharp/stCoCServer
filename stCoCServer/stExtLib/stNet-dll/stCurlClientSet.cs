using System.Collections.Generic;

namespace stNet
{
    public static class stCurlClientSet
    {
        public const string httpEncoding = "deflate, gzip";
        public const string urlBearerShare = "Authorization: Bearer {0}";
        public const string urlBearerBinary = " -H \"Authorization: Bearer {0}\"";
        public const string outPathBinary = " -o \"{0}\" -L ";
        public static readonly string[] urlMethodBinary = new string[] {
            " -X GET ",
            " -X POST "
        };
        public static readonly string[] exeDefaultBinary = new string[] {
            "curl",
            "curl.exe",
        };
        public static readonly Dictionary<string, string> jsonHeaderArgs = new Dictionary<string, string>() {
            {@"Content-Type",    @"application/json" }, // @"application/x-www-form-urlencoded"
            {@"Accept",          @"application/json" }
        };

    }
}
