# DokuWikiConnector
[![Release](https://img.shields.io/github/release/PetersSharp/DokuWikiConnector.svg?style=flat)](https://github.com/PetersSharp/DokuWikiConnector/releases/latest)
[![Issues](https://img.shields.io/github/issues/PetersSharp/stCoCServer.svg?style=flat)](https://github.com/PetersSharp/stCoCServer/issues)
[![License](http://img.shields.io/:license-mit-blue.svg)](https://github.com/PetersSharp/stCoCServer/blob/master/LICENSE)

####.NET RPC-XML DokuWiki Connector API &amp; Auth manager

 this part of stCoCServer, see [stCoCServer](https://github.com/PetersSharp/stCoCServer)

####Features:

* API .NET library allows to XML-RPC connect to remote DokuWiki.
* Support all DokuWiki API operations.
* Local DokuWiki file credential authenticate managements.

* DokuWikiConnector [source](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll)
* DokuWikiConnector [Test suite](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stTest/TestDokuWikiConnector)
* DokuWikiConnector [Method Documentation](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc)
* Plugins publish   [dokuwiki.org](https://www.dokuwiki.org/plugins:stdokuwikiconnector?s[]=dokuwikiconnector)

####Example use stDokuWiki.Connector:

```csharp

using stDokuWiki.Connector;
using stDokuWiki.Data;

    RpcXml xml = new RpcXml("http://you-dokuwiki-url.org/", "userquest", "userquest");
    XMLMethodPageList dokuList = xml.DokuPageList("wiki:") as XMLMethodPageList;
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

```

####Example use stDokuWiki.AuthManager:

```csharp

using stDokuWiki.AuthManager
using stDokuWiki.Data;

    DokuAuthManager dam;
    try
    {
       dam = new DokuAuthManager("/path/to/dokuwiki/root/dir","mygroup");

       List<DokuAuthUser> uadd = new List<DokuAuthUser>()
       {
          new DokuAuthUser() { Login = "userLogin1", Password = "pwd1234", Name = "Nikolas", Email = "Nikolas@nomail.com", Group = "personalGroup"},
          new DokuAuthUser() { ... },
       };
       List<DokuAuthUser> udel = new List<DokuAuthUser>()
       {
          new DokuAuthUser() { Login = "userLogin2" },
          new DokuAuthUser() { ... },
       };

       dam.UserAdd(uadd);
       dam.UserDelete(udel);
       dam.AuthSave();

       DokuAuthUser dau = dam.UserGet("Nikolas");
       if (dau != null)
       {
          Console.WriteLine(
             dau.Name + " : " + dau.Email
          );
       }
    }
    catch (RpcXmlException e)
    {
        Console.WriteLine("[" + e.errcode + "] " +e.Message); return;
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message); return;
    }

```
