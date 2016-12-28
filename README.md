# stCoCServer
[![Release](https://img.shields.io/github/release/PetersSharp/stCoCServer.svg?style=flat)](https://github.com/PetersSharp/stCoCServer/releases/latest)
[![Issues](https://img.shields.io/github/issues/PetersSharp/stCoCServer.svg?style=flat)](https://github.com/PetersSharp/stCoCServer/issues)

CoCServer - statistic server for games Clash of Clans, Web frontend,  IRC bot and more..

 
 Release stCoCserver is 32bit binary, Mono 3.0.0 or higher compatible,
 found in this repository: [Distributive][]

* Archive: [stCoCServer-Release-x32.zip][]
* Win32 Installation wizard: [stCoCServer-Release-x32-xxxxxxxx.exe][]

 MONO required:
	- libcurl
	- libsqlite
	- [libgdiplus][] the master branch from Github, or [mono-project][]

## Linux & Mono questions

Use Nginx as front-end
=========
  For using Nginx as front-end, edit /etc/hosts and add you actuale hostname:
````
127.0.0.1   localhost localhost.localdomain localhost4 localhost4.localdomain4 cocserver.you.server.dns.name
::1         localhost localhost.localdomain localhost6 localhost6.localdomain6 cocserver.you.server.dns.name
````
  see example [nginx-front-end.conf][] for more details..

Alias configuration libgdiplus
=========
  Edit /etc/mono/config and add next line in configuration section:
````
<dllmap dll="gdiplus" target="/path/to/lib/libgdiplus.so" />
<dllmap dll="gdiplus.dll" target="/path/to/lib/libgdiplus.so" />
````

 oops.. more info is later..

[Distributive]: https://github.com/PetersSharp/stCoCServer/tree/master/Dist/
[stCoCServer-Release-x32.zip]: https://github.com/PetersSharp/stCoCServer/raw/master/Dist/stCoCServer-Release-x32.zip
[stCoCServer-Release-x32-xxxxxxxx.exe]: https://github.com/PetersSharp/stCoCServer/tree/master/Dist
[libgdiplus]: https://github.com/mono/libgdiplus/archive/master.tar.gz
[mono-project]: http://download.mono-project.com/sources/libgdiplus/
[nginx-front-end.conf]: https://github.com/PetersSharp/stCoCServer/raw/master/stCoCServer/linux-mono/nginx-front-end.conf
