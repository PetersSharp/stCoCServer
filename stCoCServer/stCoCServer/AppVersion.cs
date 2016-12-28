
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace stApp
{
	public class AppInformation
	{
        public static string GetAppName()
        {
            return Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
        }
        public static string GetAppVersion()
        {
            FileVersionInfo ver = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return string.Format(
                "{0} v.{1}.{2}.{3}.{4} build: 2016-12-28",
                ver.ProductName,
                ver.ProductMajorPart,
                ver.ProductMinorPart,
                ver.ProductBuildPart,
                ver.ProductPrivatePart
            );
        }
        public static void PrnBanner(string [] strs)
        {
                stCore.stConsole.Write(
					new string [] { 
						Environment.NewLine,
						"\t",
						stApp.AppInformation.GetAppVersion(),
						Environment.NewLine
					}
				);
                if ((strs != null) && (strs.Length > 0))
                {
                    foreach (string str in strs)
                    {
                        stCore.stConsole.WriteLine("\t" + str);
                    }
                }
				stCore.stConsole.Write(
					new string [] {
						Environment.NewLine,
						Environment.NewLine
					}
				);
        }
	}
}