using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using stDokuWiki;
using stDokuWiki.Connector;
using stDokuWiki.AuthManager;
using stDokuWiki.Data;  // data table classes
using stDokuWiki.Crypt; // not needed

namespace Test.DokuWikiConnector
{
    class TestProgram
    {
        public static string dkwUrl = "http://you-dokuwiki-url.org/";
        public static Int32 ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (Int32)Math.Floor(diff.TotalSeconds);
        }
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                TestProgram.dkwUrl = args[0];
            }
            
            Console.WriteLine("DokuWiki URL: " + TestProgram.dkwUrl);
            RpcXml xml = null;

            try
            {
                xml = new RpcXml(TestProgram.dkwUrl, "clanquest", "clanquest");
            }
            catch (RpcXmlException e)
            {
                Console.WriteLine(e.Message + " [" + e.errcode + "]");
                Console.ReadLine();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("RpcXml Exception: " + e.Message);
                Console.ReadLine();
                return;
            }

            DateTime dt = DateTime.Parse("2016-08-14 21:05:25"); // Date modify "clanwizard:rules"
            int unixTime = ConvertToUnixTimestamp(dt);
            Console.WriteLine(dt.ToString() + " : " + unixTime.ToString());

            try
            {
                xml.DokuVersion();
                xml.DokuGetTimeStamp();
                xml.DokuXMLRPCAPIVersion();
                xml.DokuRPCVersionSupported();
                xml.DokuMediaChange(unixTime);
                xml.DokuPagesChange(unixTime);
                xml.DokuPagePut("playground:test008", "hey!! open you main new");
                xml.DokuPageGet("playground:test008");

                xml.DokuPageAclCheck("clanwizard:rules");
                xml.DokuPageGetVersions("clanwizard:rules", 1);
                xml.DokuPageGetVersion("clanwizard:rules", dt);
                xml.DokuPageInfoVersion("clanwizard:rules", dt);

                xml.DokuPageInfoVersion("clanwizard:rules", unixTime);
                xml.DokuPageInfoVersion("clanwizard:rules", "2016-08-14T21:05:25+0000");

                Console.WriteLine("DokuWiki page in selected namespace:");

                XMLMethodPageList dokuList = xml.DokuPageList("clan:") as XMLMethodPageList;
                foreach (var items in dokuList.Params.Param.Value.Array.Data.Value)
                {
                    foreach (var item in items.Struct.Member)
                    {
                        Console.WriteLine(
                            item.Name +
                            ((string.IsNullOrWhiteSpace(item.Value.Int)) ? " " : " [" + item.Value.Int + "] ") +
                            ((string.IsNullOrWhiteSpace(item.Value.String)) ? "" : item.Value.String)
                        );
                    }
                }

                Console.WriteLine("DokuWiki All page:");

                XMLMethodPageListAll dokuListAll = xml.DokuPageListAll() as XMLMethodPageListAll;
                foreach (var items in dokuListAll.Params.Param.Value.Array.Data.Value)
                {
                    foreach (var item in items.Struct.Member)
                    {
                        Console.WriteLine(
                            item.Name +
                            ((string.IsNullOrWhiteSpace(item.Value.Int)) ? " " : " [" + item.Value.Int + "] ") +
                            ((string.IsNullOrWhiteSpace(item.Value.String)) ? "" : item.Value.String)
                        );
                    }
                }

                /*
                var lstn = xml.DokuPageList("clan:") as XMLMethodPageList;
                var lsta = xml.DokuPageListAll() as XMLMethodPageListAll;
                 */

                xml.DokuAttachmentList("playground:");
                xml.DokuAttachmentInfo("playground:coc00.png");
                xml.DokuAttachmentGet("playground:coc00.png");
                xml.DokuAttachmentRemove("playground:coc00.png");
            }
            catch (RpcXmlException e)
            {
                Console.WriteLine(e.Message + " [" + e.errcode + "]");
                Console.ReadLine();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("RpcXml Exception: " + e.Message);
                Console.ReadLine();
                return;
            }

            Console.ReadLine();
        }
    }
}
