using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace stCore
{
    public static class IOBaseAssembly
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string BaseDir()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string BaseName()
        {
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        }
        public static string BaseName(System.Reflection.Assembly asm)
        {
            return Path.GetFileNameWithoutExtension(asm.Location);
        }
        public static string BaseDescription(System.Reflection.Assembly asm)
        {
            return ((AssemblyDescriptionAttribute)
                    asm.GetCustomAttributes(
                        typeof(System.Reflection.AssemblyDescriptionAttribute), false)[0]
                    )
                    .Description;
        }
        public static string BaseVersion(System.Reflection.Assembly asm)
        {
            return ((AssemblyDescriptionAttribute)
                    asm.GetCustomAttributes(
                        typeof(System.Reflection.AssemblyVersionAttribute), false)[0]
                    )
                    .Description;
        }
        public static string BaseDataDir(string path = null)
        {
            string dataDir = String.Empty;

            if (!string.IsNullOrWhiteSpace(path))
            {
                if (!path.EndsWith("data"))
                {
                    path = Path.Combine(path, "data");
                }
            }
            else
            {
                path = Path.Combine(
                    stCore.IOBaseAssembly.BaseDir(),
                    "data"
                );
            }
            stCore.IOBaseAssembly._BaseDirCheck(path);
            return path;
        }
        private static bool _BaseDirCheck(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
