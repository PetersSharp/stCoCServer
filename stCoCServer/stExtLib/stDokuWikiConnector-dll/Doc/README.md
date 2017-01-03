
# stDokuWikiConnector


## Class: stDokuWiki.AuthManager.DokuAuthManager

Manager DokuWiki Auth (file: conf/users.auth.php)


##### Code:

```csharp

using stDokuWiki;
            using stDokuWiki.Data;
            using stDokuWiki.AuthManager;
```

*stDokuWiki.RpcXmlException:* RpcXmlException thrown if error internal DokuAuthManager class

*System.Exception:* Exception thrown if other error


### Method: #ctor(path, group)

(class) DokuAuthManager Constructor


##### Code:

```csharp

try
            {
                DokuAuthManager dam = new DokuAuthManager("/path/to/dokuwiki/root/dir","mygroup");
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

| Name | Description |
| ---- | ----------- |
| path | *System.String*<br>path to DokuWiki root folder |
| group | *System.String*<br>default DokuWiki users group |

### Method: UserAdd(user)

(Doku Manager) Add users to auth table (input DokuAuthUser)


##### Code:

```csharp

dam.UserAdd(
              new DokuAuthUser() 
              { 
                 Login = "userLogin",
                 Password = "pwd1234",
                 Name = "Nikolas",
                 Email = "Nikolas@nomail.com",
                 Group = "personalGroup"
              }
            );
            dam.AuthSave();
```

| Name | Description |
| ---- | ----------- |
| user | *stDokuWiki.Data.DokuAuthUser*<br>Class DokuAuthUser |


> #### Returns
> Class DokuAuthUser


### Method: UserAdd(login, passwd, name, group, email)

(Doku Manager) Add user to auth table (input strings)


##### Code:

```csharp

dam.UserAdd(
              "userLogin",
              "pwd1234",
              "Nikolas",
              "personalGroup",
              "Nikolas@nomail.com"
            );
            dam.AuthSave();
```

| Name | Description |
| ---- | ----------- |
| login | *System.String*<br>string DokuWiki user Login |
| passwd | *System.String*<br>string DokuWiki user Password |
| name | *System.String*<br>string DokuWiki user Name |
| group | *System.String*<br>string DokuWiki user Group, null is default |
| email | *System.String*<br>string user E-Mail, null is default |


> #### Returns
> Class DokuAuthUser


### Method: UserDelete(userlst)

(Doku Manager) Delete user from auth table


##### Code:

```csharp

List<DokuAuthUser> udel = new List<DokuAuthUser>()
            {
                new DokuAuthUser() { Login = "userLogin" },
                new DokuAuthUser() { ... },
            };
            dam.UserDelete(udel);
            dam.AuthSave();
```

| Name | Description |
| ---- | ----------- |
| userlst | *System.Collections.Generic.List{stDokuWiki.Data.DokuAuthUser}*<br> |

### Method: UserFind(usermask)

(Doku Manager) Find users where Login or Name contains usermask


##### Code:

```csharp

List<DokuAuthUser> ldau = dam.UserFind("nikol");
            if (ldau != null)
            {
               foreach (DokuAuthUser dau in idau)
               {
                  Console.WriteLine(
                     dau.Login + " : " + dau.Name + " : " + dau.Email
                  );
               }
            }
```

| Name | Description |
| ---- | ----------- |
| usermask | *System.String*<br>contains user Login or Name |


> #### Returns
> List<DokuAuthUser>


### Method: UserGet(userlogin)

(Doku Manager) Get user from auth table


##### Code:

```csharp

DokuAuthUser dau = dam.UserGet("Nikolas");
            if (dau != null)
            {
               Console.WriteLine(
                  dau.Name + " : " + dau.Email
               );
            }
```

| Name | Description |
| ---- | ----------- |
| userlogin | *System.String*<br> |


> #### Returns
> class Data.DokuAuthUser


### Method: UserList

(Doku Manager) Get all users from auth table


##### Code:

```csharp

List<DokuAuthUser> ldau = dam.UserList();
            if (ldau != null)
            {
               foreach (DokuAuthUser dau in idau)
               {
                  Console.WriteLine(
                     dau.Login + " : " + dau.Name + " : " + dau.Email
                  );
               }
            }
```


> #### Returns
> List<DokuAuthUser>


### Method: UsersAdd(userlst)

(Doku Manager) Add users to auth table


##### Code:

```csharp

List<DokuAuthUser> uadd = new List<DokuAuthUser>()
            {
                new DokuAuthUser()
                { 
                  Login = "userLogin",
                  Password = "pwd1234",
                  Name = "Nikolas",
                  Email = "Nikolas@nomail.com",
                  Group = "personalGroup"
                },
                new DokuAuthUser() { ... },
            };
            dam.UsersAdd(uadd);
            dam.AuthSave();
```

| Name | Description |
| ---- | ----------- |
| userlst | *System.Collections.Generic.List{stDokuWiki.Data.DokuAuthUser}*<br> |

### Method: UserSave

(Doku Manager) Save user auth table


##### Code:

```csharp

dam.AuthSave();
```


## Class: stDokuWiki.Connector.RpcXml

Rpc-Xml DokuWiki connector class


##### Code:

```csharp

using stDokuWiki;
            using stDokuWiki.Data;
            using stDokuWiki.Connector;
```

DokuWiki Rpc-Xml connector



### Method: #ctor(url, login, passwd)

(class) Rpc-Xml Constructor


| Name | Description |
| ---- | ----------- |
| url | *System.String*<br>DokuWiki base Url |
| login | *System.String*<br>you DokuWiki login credentials |
| passwd | *System.String*<br>you DokuWiki password credentials |

### Method: DokuAttachmentGet(id)

Returns the object data of a media file.


| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>Attachment id (file name) |


> #### Returns
> the data of the file in byte[] format


### Method: DokuAttachmentInfo(id)

Returns information about a media file.


##### Example:

```
size = size in bytes
            lastModified = modification date as XML-RPC Date object
```

| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>Attachment id (file name) |


> #### Returns
> class Data.XMLMethodAttachmentInfo


### Method: DokuAttachmentList(System.String,System.String)

Returns a list of media files in a given namespace.


##### Example:

```
id = media id
            file = name of the file
            size = size in bytes
            mtime = upload date as a timestamp
            lastModified =  modification date as XML-RPC Date object
            isimg = true if file is an image, false otherwise
            writable = true if file is writable, false otherwise
            perms = permissions of file
```


> #### Returns
> class Data.XMLMethodAttachmentList


### Method: DokuAttachmentPut(path, owr)

Uploads a file as a given media id, source as full file path.


| Name | Description |
| ---- | ----------- |
| path | *System.String*<br>full file path |
| owr | *System.Boolean*<br> |


> #### Returns
> Bolean


### Method: DokuAttachmentPut(id, obj, owr)

Uploads a file as a given media id, source as byte array.


| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>media id |
| obj | *System.Byte[]*<br>byte array |
| owr | *System.Boolean*<br> |


> #### Returns
> 


### Method: DokuAttachmentPut(id, base64, owr)

Uploads a file as a given media id, source as Base64 encoded.


| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>Attachment id (file name) |
| base64 | *System.String*<br>source string - Base64 encoded |
| owr | *System.Boolean*<br> |


> #### Returns
> 


### Method: DokuAttachmentRemove(id)

Deletes a file. Fails if the file is still referenced from any page in the wiki.


| Name | Description |
| ---- | ----------- |
| id | *System.String*<br>Attachment id (file name) |


> #### Returns
> Bolean


### Method: DokuAuth(login, passwd)

Uses the provided credentials to execute a login and will set cookies. This can be used to make authenticated requests afterwards.


| Name | Description |
| ---- | ----------- |
| login | *System.String*<br> |
| passwd | *System.String*<br> |


> #### Returns
> Boolean


### Method: DokuGetDateTime

Returns the current time at the remote wiki server as DateTime.



> #### Returns
> DateTime


### Method: DokuGetTimeStamp

Returns the current time at the remote wiki server as Unix timestamp.



> #### Returns
> Int32


### Method: DokuMediaChange(date)

Returns a list of recent changes since given DateTime.


| Name | Description |
| ---- | ----------- |
| date | *System.DateTime*<br>DateTime format timestamp |


> #### Returns
> class Data.XMLMethodMediaRecentChange


### Method: DokuMediaChange(timestamp)

Returns a list of recent changes since given timestamp.
As stated: Only the most recent change for each page is listed,
regardless of how many times that page was changed.


##### Example:

```
Return (array) each array item holds the following data:
            name = page id
            lastModified = modification date as UTC timestamp
            author = author
            version = page version as timestamp
            perms = media permissions
            size = media size in bytes
```

| Name | Description |
| ---- | ----------- |
| timestamp | *System.Int32*<br>Unix timestamp (int) |


> #### Returns
> class Data.XMLMethodMediaRecentChange


### Method: DokuMediaChange(date)

Returns a list of recent changes since given Date and Time string.


| Name | Description |
| ---- | ----------- |
| date | *System.String*<br>string format Date and Time |


> #### Returns
> class Data.XMLMethodMediaRecentChange


### Method: DokuPageAclCheck(pagename)

Returns the permission of the given wikipage.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |


> #### Returns
> Int32


### Method: DokuPageAppend(pagename, rawtxt, isHtml, sum, minor)

Append text to Wiki Page.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| rawtxt | *System.String*<br>wiki markup/html Wiki text |
| isHtml | *System.Boolean*<br>wiki markup/html selector |
| sum | *System.String*<br>change summary |
| minor | *System.Boolean*<br>minor |


> #### Returns
> bool


### Method: DokuPageClear(pagename)

Clear all text from Wiki Page.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |


> #### Returns
> bool


### Method: DokuPageDelete(pagename)

Delete Wiki Page.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |


> #### Returns
> bool


### Method: DokuPageGet(pagename, isHtml)

Get DokuWiki raw/html Wiki text for a page.



> #### Remarks
>  isHtml = false : Returns the raw Wiki text for a page isHtml = true : Returns the rendered XHTML body of a Wiki page 

| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| isHtml | *System.Boolean*<br>wiki markup/html selector |


> #### Returns
> String


### Method: DokuPageGetVersion(pagename, date, isHtml)

Returns the raw/html Wiki text for a page with DateTime.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| date | *System.DateTime*<br>DateTime format timestamp |
| isHtml | *System.Boolean*<br>return Html content |


> #### Returns
> String


### Method: DokuPageGetVersion(pagename, timestamp, isHtml)

Returns the raw/html Wiki text for a page with timestamp.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| timestamp | *System.Int32*<br>Unix timestamp (int) |
| isHtml | *System.Boolean*<br>return Html content |


> #### Returns
> String


### Method: DokuPageGetVersion(pagename, date, isHtml)

Returns the raw/html Wiki text for a page with Date and Time string.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| date | *System.String*<br>string format Date and Time |
| isHtml | *System.Boolean*<br>return Html content |


> #### Returns
> String


### Method: DokuPageGetVersions(pagename, offset)

Returns the available versions of a Wiki page.
The number of pages in the result is controlled via the recent configuration setting.
The offset can be used to list earlier versions in the history.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| offset | *System.Int32*<br>offset page (int) |


> #### Returns
> class Data.XMLMethodPageGetVersions


### Method: DokuPageInfo(pagename)

Returns information about a Wiki page.


##### Example:

```
Returns (array) an array containing the following data: 
            name = page name.
            lastModified = modification date as IXR_Date Object.
            author = author of the Wiki page.
            version = page version as timestamp.
```

| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br> |


> #### Returns
> class Data.XMLMethodPageInfo


### Method: DokuPageInfoVersion(pagename, date)

Returns information about a Wiki page with DateTime.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| date | *System.DateTime*<br>DateTime format timestamp |


> #### Returns
> class Data.XMLMethodPageInfo


### Method: DokuPageInfoVersion(pagename, timestamp)

Returns information about a Wiki page with timestamp.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| timestamp | *System.Int32*<br>Unix timestamp (int) |


> #### Returns
> class Data.XMLMethodPageInfo


### Method: DokuPageInfoVersion(pagename, date)

Returns information about a Wiki page with Date and Time string.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| date | *System.String*<br>string format Date and Time |


> #### Returns
> class Data.XMLMethodPageInfo


### Method: DokuPageInsert(pagename, rawtxt, isHtml, sum, minor)

Insert text to Wiki Page.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| rawtxt | *System.String*<br>wiki markup/html Wiki text |
| isHtml | *System.Boolean*<br>wiki markup/html selector |
| sum | *System.String*<br>change summary |
| minor | *System.Boolean*<br>minor |


> #### Returns
> bool


### Method: DokuPageLinks(pagename)

Returns a list of all links contained in a Wiki page.


##### Example:

```
Return (array) each array item holds the following data:
            type = local/extern
            page = the wiki page (or the complete URL if extern)
            href = the complete URL
```

| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |


> #### Returns
> class Data.XMLMethodPageLinks


### Method: DokuPageLinksBack(pagename)

Returns a list of backlinks of a Wiki page.
See: https://www.dokuwiki.org/backlinks


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |


> #### Returns
> class Data.XMLMethodPageLinksBack


### Method: DokuPageList(namesspace, opt)

Lists all pages within a given namespace.


##### Example:

```
example foreach all page in namespace "wiki:*"
```

##### Code:

```csharp

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

| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br> |
| opt | *System.String*<br> |


> #### Returns
> class Data.XMLMethodPageList


### Method: DokuPageListAll

Returns a list of all Wiki pages in the remote Wiki.


##### Example:

```
Return (array) One item for each page, each item containing the following data:
            id = id of the page.
            perms = integer denoting the permissions on the page.
            size = size in bytes.
            lastModified = dateTime object of last modification date.
```


> #### Returns
> class Data.XMLMethodPageListAll


### Method: DokuPagePut(pagename, rawtxt, sum, minor)

Create or update Wiki Page.


| Name | Description |
| ---- | ----------- |
| pagename | *System.String*<br>page name |
| rawtxt | *System.String*<br>raw Wiki text |
| sum | *System.String*<br>change summary |
| minor | *System.Boolean*<br>minor |


> #### Returns
> bool


### Method: DokuPagesChange(date)

Returns a list of recent changes since given DateTime.


| Name | Description |
| ---- | ----------- |
| date | *System.DateTime*<br>DateTime format timestamp |


> #### Returns
> class Data.XMLMethodPagesRecentChange


### Method: DokuPagesChange(timestamp)

Returns a list of recent changes since given timestamp.
As stated: Only the most recent change for each page is listed,
regardless of how many times that page was changed.


##### Example:

```
Return (array) each array item holds the following data:
            name = page id
            lastModified = modification date as UTC timestamp
            author = author
            version = page version as timestamp
```

| Name | Description |
| ---- | ----------- |
| timestamp | *System.Int32*<br>Unix timestamp (int) |


> #### Returns
> class Data.XMLMethodPagesRecentChange


### Method: DokuPagesChange(date)

Returns a list of recent changes since given Date andTime string.


| Name | Description |
| ---- | ----------- |
| date | *System.String*<br>string format Date and Time |


> #### Returns
> class Data.XMLMethodPagesRecentChange


### Method: DokuRPCVersionSupported

Returns 2 with the supported RPC API version.



> #### Returns
> Int32


### Method: DokuSearch(query)

Search return associative array with matching pages similar to what is returned by dokuwiki.getPagelist, snippets are provided for the first 15 results.


| Name | Description |
| ---- | ----------- |
| query | *System.String*<br> |


> #### Returns
> class Data.XMLMethodPageList


### Method: DokuTitle

Returns the title of the wiki.



> #### Returns
> String


### Method: DokuVersion

Returns the DokuWiki version of the remote Wiki.



> #### Returns
> String


### Method: DokuXMLRPCAPIVersion

Returns the XML RPC interface version of the remote Wiki.
This is DokuWiki implementation specific and independent of the supported
standard API version returned by wiki.getRPCVersionSupported



> #### Returns
> Int32


### Method: Get``1(type, n)

Execute DokuWiki command


| Name | Description |
| ---- | ----------- |
| type | *stDokuWiki.Connector.RpcXml.XmlRpcRequest*<br> |
| n | *System.Int32*<br> |


> #### Returns
> 


### Method: Get``1(type, val, n)

Execute DokuWiki command


| Name | Description |
| ---- | ----------- |
| type | *stDokuWiki.Connector.RpcXml.XmlRpcRequest*<br> |
| val | *System.String*<br> |
| n | *System.Int32*<br> |


> #### Returns
> 


### Method: Get``1(type, val)

Execute DokuWiki command


| Name | Description |
| ---- | ----------- |
| type | *stDokuWiki.Connector.RpcXml.XmlRpcRequest*<br> |
| val | *System.String[]*<br> |


> #### Returns
> 


### Method: Get``1(xmlstr)

Execute DokuWiki command


| Name | Description |
| ---- | ----------- |
| xmlstr | *System.String*<br> |


> #### Returns
> 


## Class: stDokuWiki.Connector.RpcXml.XmlRpcPageAction

Page Action: Insert, Update, Delete, None



## Class: stDokuWiki.Connector.RpcXml.XmlRpcRequest

See API https://www.dokuwiki.org/devel:xmlrpc



### F:dokuwiki_appendPage

Appends text to a Wiki page.



### F:dokuwiki_getPagelist

Lists all pages within a given namespace.



### F:dokuwiki_getTime

Returns the current time at the remote wiki server as Unix timestamp.



### F:dokuwiki_getTitle

Returns the title of the wiki.



### F:dokuwiki_getVersion

Returns the DokuWiki version of the remote Wiki.



### F:dokuwiki_getXMLRPCAPIVersion

Returns the XML RPC interface version of the remote Wiki.



### F:dokuwiki_login

Uses the provided credentials to execute a login and will set cookies.



### F:dokuwiki_search

Performs a fulltext search.



### F:dokuwiki_setLocks

Allows you to lock or unlock a whole bunch of pages at once.



### F:plugin_acl_addAcl

Add an ACL rule. Use @groupname instead of user to add an ACL rule for a group.



### F:plugin_acl_delAcl

Delete any ACL rule matching the given scope and user.



### F:wiki_aclCheck

Returns the permission of the given Wiki page.



### F:wiki_deleteAttachment

Deletes a file. Fails if the file is still referenced from any page in the wiki.



### F:wiki_getAllPages

Returns a list of all Wiki pages in the remote Wiki.



### F:wiki_getAttachment

Returns the binary data of a media attachments file.



### F:wiki_getAttachmentInfo

Returns information about a media attachments file.



### F:wiki_getAttachments

Returns a list of media files in a given namespace.



### F:wiki_getBackLinks

Returns a list of backlinks of a Wiki page.



### F:wiki_getPage

Returns the raw Wiki text for a page.



### F:wiki_getPageHTML

Returns the rendered XHTML body of a Wiki page.



### F:wiki_getPageHTMLVersion

Returns the rendered HTML of a specific version of a Wiki page.



### F:wiki_getPageInfo

Returns information about a Wiki page.



### F:wiki_getPageInfoVersion

Returns information about a specific version of a Wiki page.



### F:wiki_getPageVersion

Returns the raw Wiki text for a specific revision of a Wiki page.



### F:wiki_getPageVersions

Returns the available versions of a Wiki page.



### F:wiki_getRecentChanges

Returns a list of recent changes since given timestamp.



### F:wiki_getRecentMediaChanges

Returns a list of recent changed media since given timestamp.



### F:wiki_getRPCVersionSupported

Returns number with the supported RPC API version.



### F:wiki_listLinks

Returns a list of all links contained in a Wiki page.



### F:wiki_putAttachment

Uploads a file as a given media id.



### F:wiki_putPage

Saves a Wiki page.



## Class: stDokuWiki.Crypt.CryptUtils

(class) Safe password hashing, compatible PHP crypt() function



## Class: stDokuWiki.Data.DokuAuthUser

class DokuWiki Auth user ( use from class DokuAuthManager )



## Class: stDokuWiki.Data.RpcXmlRequestComposite

Use in request composite String and Int DokuWiki Data
Enum (XmlRpcRequest): none



## Class: stDokuWiki.Data.RpcXmlRequestInt

Use in request Int DokuWiki Data
Enum (XmlRpcRequest): none



## Class: stDokuWiki.Data.RpcXmlRequestString

Use in request String DokuWiki Data
Enum (XmlRpcRequest): none



## Class: stDokuWiki.Data.XMLMethodAttachmentGet

Use in DokuAttachmentGet(string id)
Enum (XmlRpcRequest): wiki_getAttachment


> See also: [request-wiki_getAttachment](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAttachment.xml)

> See also: [response-wiki_getAttachment](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAttachment.xml)


## Class: stDokuWiki.Data.XMLMethodAttachmentInfo

Return from DokuAttachmentInfo(string id)
Enum (XmlRpcRequest): wiki_getAttachmentInfo


> See also: [request-wiki_getAttachmentInfo](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAttachmentInfo.xml)

> See also: [response-wiki_getAttachmentInfo](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAttachmentInfo.xml)


## Class: stDokuWiki.Data.XMLMethodAttachmentList

Return from DokuAttachmentList(string NameSpace, string Options)
Enum (XmlRpcRequest): wiki_getAttachments


> See also: [request-wiki_getAttachments](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAttachments.xml)

> See also: [response-wiki_getAttachments](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAttachments.xml)


## Class: stDokuWiki.Data.XMLMethodBool

Use in return bool function: DokuAuth,
Enum (XmlRpcRequest): none



## Class: stDokuWiki.Data.XMLMethodErrorResponse

Use to check wiki error response
Enum (XmlRpcRequest): none



## Class: stDokuWiki.Data.XMLMethodGetInt

Use in return int functions: DokuXMLRPCAPIVersion, DokuRPCVersionSupported, DokuDokuGetDateTime
Enum (XmlRpcRequest): wiki_getXMLRPCAPIVersion, wiki_getRPCVersionSupported, wiki_getTime



## Class: stDokuWiki.Data.XMLMethodGetString

Use in return String functions
Enum (XmlRpcRequest):



## Class: stDokuWiki.Data.XMLMethodMediaRecentChange

Return from function: DokuRecentMediaChanges
Enum (XmlRpcRequest): RecentMediaChanges



## Class: stDokuWiki.Data.XMLMethodPageAclCheck

Return from function: DokuPageAclCheck
Enum (XmlRpcRequest): wiki_aclCheck



## Class: stDokuWiki.Data.XMLMethodPageGet

Use in function: DokuPageGet
Enum (XmlRpcRequest): wiki_getPageHTML, wiki_getPage



## Class: stDokuWiki.Data.XMLMethodPageGetVersions

Use in function: DokuPageGetVersions(string PageName, Int32 StartOffset)
Enum (XmlRpcRequest): wiki_getPageVersions



## Class: stDokuWiki.Data.XMLMethodPageInfo

Use in function: DokuPageInfo(string PageName), DokuPageInfoVerion(string PageName, ...)
Enum (XmlRpcRequest): wiki_getPageInfo


> See also: [request-wiki_getPageInfoVersion](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getPageInfoVersion.xml)

> See also: [response-wiki_getPageInfoVersion](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getPageInfoVersion.xml)


## Class: stDokuWiki.Data.XMLMethodPageLinks

Return from function: DokuPageLinks(string PageName)
Enum (XmlRpcRequest): wiki_listLinks



## Class: stDokuWiki.Data.XMLMethodPageLinksBack

Return from function: DokuPageLinksBack(string PageName)
Enum (XmlRpcRequest): wiki_getBackLinks



## Class: stDokuWiki.Data.XMLMethodPageList

Return from function: DokuPageList(string NamesSpace, string Options)
Enum (XmlRpcRequest): dokuwiki_getPagelist


> See also: [request-dokuwiki_getPagelist](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-dokuwiki_getPagelist.xml)

> See also: [response-dokuwiki_getPagelist](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-dokuwiki_getPagelist.xml)


## Class: stDokuWiki.Data.XMLMethodPageListAll

Return from function: DokuPageListAll()
Enum (XmlRpcRequest): wiki_getAllPages


> See also: [request-wiki_getAllPages](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAllPages.xml)

> See also: [response-wiki_getAllPages](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAllPages.xml)


## Class: stDokuWiki.Data.XMLMethodPagesRecentChange

Return from function: DokuPagesChange(int TimeStamp || DateTime dateTime || string dateTime)
Enum (XmlRpcRequest): wiki_getRecentChanges


> See also: [request-wiki_getRecentChanges](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getRecentChanges.xml)

> See also: [response-wiki_getRecentChanges](https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getRecentChanges.xml)


## Class: stDokuWiki.Properties.ResourceCodeError

Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.



## Class: stDokuWiki.Properties.ResourceWikiEngine

Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.



## Class: stDokuWiki.RpcXmlException

RPC-XML/AuthManager error Exception


##### Code:

```csharp

using stDokuWiki;
```


### Method: #ctor(info, context, ecode)

Serialization method RpcXmlException


| Name | Description |
| ---- | ----------- |
| info | *System.Runtime.Serialization.SerializationInfo*<br>Exception error SerializationInfo |
| context | *System.Runtime.Serialization.StreamingContext*<br>Exception error StreamingContext |
| ecode | *System.Int32*<br>Exception error code |

### Method: #ctor(message, ecode)

Base RpcXmlException


| Name | Description |
| ---- | ----------- |
| message | *System.String*<br>Exception error message |
| ecode | *System.Int32*<br>Exception error code |

### Method: #ctor(message, ecode, inner)

Custom Exception RpcXmlException


| Name | Description |
| ---- | ----------- |
| message | *System.String*<br>Exception error message |
| ecode | *System.Int32*<br>Exception error code |
| inner | *System.Exception*<br>Exception inner Exception |

### F:errcode

Exception internal error code



## Class: stDokuWiki.Util.DokuUtil

Internal utilites Class



### Method: AtticCompressFile(ppath, apath)

Wiki attic history change create page backup


| Name | Description |
| ---- | ----------- |
| ppath | *System.String*<br>input page path |
| apath | *System.String*<br>output attic path |

### Method: AtticDeCompressFile(apath)

Wiki attic history change read page backup


| Name | Description |
| ---- | ----------- |
| apath | *System.String*<br>input attic path |


> #### Returns
> decompressed body as byte []


### Method: GetDateTimeFromUnixTimeStamp(timestamp)

Convert Unix TimeStamp to DateTime


| Name | Description |
| ---- | ----------- |
| timestamp | *System.Int32*<br>Unix timestamp in int format |


> #### Returns
> DateTime


### Method: GetDateTimeFromUnixTimeStampString(stime)

Convert Unix TimeStamp string to DateTime


| Name | Description |
| ---- | ----------- |
| stime | *System.String*<br>Unix timestamp in string format |


> #### Returns
> DateTime


### Method: GetUnixTimeStamp(time)

Convert DateTime to Unix TimeStamp


| Name | Description |
| ---- | ----------- |
| time | *System.DateTime*<br>Time and Date in DateTime format |


> #### Returns
> DateTime format


### Method: GetUnixTimeStamp(stime)

Convert String date and time to Unix TimeStamp


| Name | Description |
| ---- | ----------- |
| stime | *System.String*<br>Time and Date in string format |


> #### Returns
> Int32 format


### Method: WikiFileExtToString(src)

normalize file extension string


| Name | Description |
| ---- | ----------- |
| src | *System.String*<br>String file extension |


> #### Returns
> String


### Method: WikiFileMetaDataMerge(wfm, isCreate)

Merge meta data to WikiEngine.WikiMetaChanges


| Name | Description |
| ---- | ----------- |
| wfm | *stDokuWiki.WikiEngine.WikiFileMeta*<br>WikiEngine.WikiFileMeta |
| isCreate | *System.Boolean*<br>Bolean create or update file |


> #### Returns
> WikiEngine.WikiMetaChanges


### Method: WikiFilePathRewrite(wft, path, replace, append)

Rewrite file path, change type


| Name | Description |
| ---- | ----------- |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br>WikiFileType |
| path | *System.String*<br>file path |
| replace | *System.String*<br>replace string |
| append | *System.String*<br>append string to end |


> #### Returns
> 


### Method: WikiFileStringToDefaultType(src, rw)

map string full Namespace to type, enum WikiEngine.WikiFileType


| Name | Description |
| ---- | ----------- |
| src | *System.String*<br>String: pages, media, attic, meta |
| rw | *System.Boolean*<br>Bolean, read = false, write = true |


> #### Returns
> WikiEngine.WikiFileType


### Method: WikiFileStringToMethod(src)

map string to action, enum WikiRequestType


| Name | Description |
| ---- | ----------- |
| src | *System.String*<br>String: get, put, del, list, find |


> #### Returns
> WikiEngine.WikiRequestType


### Method: WikiFileStringToType(type, src)

map string to type, enum WikiEngine.WikiFileType


| Name | Description |
| ---- | ----------- |
| type | *stDokuWiki.WikiEngine.WikiRequestType*<br>WikiEngine.WikiRequestType |
| src | *System.String*<br>String: pages, media, attic, meta |


> #### Returns
> WikiEngine.WikiFileType


### Method: WikiFileTypeToString(type)

map string to type, enum WikiEngine.WikiFileType


| Name | Description |
| ---- | ----------- |
| type | *stDokuWiki.WikiEngine.WikiFileType*<br>WikiEngine.WikiFileType |


> #### Returns
> String


## Class: stDokuWiki.WikiEngine.WikiData

Wiki request file class



### Method: MergeWFI(wfi)

Merge data WikiFileInfo to WikiData


| Name | Description |
| ---- | ----------- |
| wfi | *stDokuWiki.WikiEngine.WikiFileInfo*<br>WikiFileInfo |

### Method: MergeWFT(wftr, path)

Merge data WikiFile.WikiFileTransfer to WikiData


| Name | Description |
| ---- | ----------- |
| wftr | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse*<br>internal WikiFile.WikiFileTransfer |
| path | *System.String*<br>root path |

## Class: stDokuWiki.WikiEngine.WikiErrorEventArgs

Event Arguments Wiki Error



### Method: #ctor(ex)

Constructor


| Name | Description |
| ---- | ----------- |
| ex | *System.Exception*<br>Exception  |

## Class: stDokuWiki.WikiEngine.WikiFile

DokuWiki compatible file engine.
Get/Put/Delete/List page and media files.



### Method: #ctor(rootPath)

Init DokuWiki compatible file engine.


##### Code:

```csharp

using stDokuWiki.WikiEngine;
            using stDokuWiki.WikiEngine.Exceptions;
            
            try
            {
               WikiFile wf = new WikiFile("data");
               wf.OnProcessError += (o, e) =>
               {
                  Console.WriteLine(e.ex.GetType().Name + ": " + e.ex.Message);
               };
               wf.OnWikiFSChange += (o, e) =>
               {
                  Console.WriteLine("Root namspace count: " + e.Count + ":" + e.WikiFile.CountRootNamspace);
               };
            }
            catch (Exception e)
            {
               Console.WriteLine(e.GetType().Name + ": " + e.Message);
            }
```

| Name | Description |
| ---- | ----------- |
| rootPath | *System.String*<br>Root path to wiki folder |

### Method: __WikiFileGetFileInfoSelect(wfp)

part from _WikiFileGetFileInfo(WikiFileParse) internal method


| Name | Description |
| ---- | ----------- |
| wfp | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse*<br>internal class WikiEngine.WikiFile.WikiFileParse |


> #### Returns
> WikiEngine.WikiFileInfo


### Method: __WikiFilesFindFiles(wfp)

internal Wiki find files


| Name | Description |
| ---- | ----------- |
| wfp | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse*<br>WikiFile.WikiFileParse data |


> #### Returns
> WikiFolderInfo data


### Method: __WikiFilesFindNamespace(wfp)

internal find Namespace


| Name | Description |
| ---- | ----------- |
| wfp | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse*<br>WikiFile.WikiFileParse data |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: __WikiFilesFindNamespaceFiles(wfo, wfi, key)

internal find Namespace folder, iteration recursive


| Name | Description |
| ---- | ----------- |
| wfo | *stDokuWiki.WikiEngine.WikiFolderInfo@*<br>ref WikiEngine.WikiFolderInfo, out data |
| wfi | *stDokuWiki.WikiEngine.WikiFolderInfo*<br>WikiEngine.WikiFolderInfo, input data |
| key | *System.String*<br>Start key to search from Dictionary, its directory name |


> #### Returns
> Bolean


### Method: __WikiFilesFindNamespaceFolder(wfi, search)

internal find Namespace folder, iteration recursive


| Name | Description |
| ---- | ----------- |
| wfi | *stDokuWiki.WikiEngine.WikiFolderInfo*<br>WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo input |
| search | *System.String*<br>serch patern string |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: __WikiFilesParse(wfp)

internal Uri/Namespace parse


| Name | Description |
| ---- | ----------- |
| wfp | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse*<br> |


> #### Returns
> WikiFile.WikiFileParse


### Method: __WikiFilesParseAtticExtension(wfp, checkNs, checkFn, app)

Attic append path to specific locations
part of __WikiFilesParse


| Name | Description |
| ---- | ----------- |
| wfp | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse@*<br> |
| checkNs | *System.Boolean*<br> |
| checkFn | *System.Boolean*<br> |
| app | *System.String*<br> |

### Method: __WikiFilesParseMetaExtension(checkNs, checkFn, wfp, app)

Meta append path to specific locations
part of __WikiFilesParse


| Name | Description |
| ---- | ----------- |
| checkNs | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse@*<br> |
| checkFn | *System.Boolean*<br> |
| wfp | *System.Boolean*<br> |
| app | *System.String*<br> |

### Method: _FilesAction_SetFileLock(wfa)

Call Back to _SetFileLock


| Name | Description |
| ---- | ----------- |
| wfa | *stDokuWiki.WikiEngine.WikiFileAction*<br>WikiFileAction |

### Method: _OnWikiFSChangeWatch(source, ev)

CallBack from FileSystemWatcher


| Name | Description |
| ---- | ----------- |
| source | *System.Object*<br>object, not used |
| ev | *System.IO.FileSystemEventArgs*<br>FileSystemEventArgs |

### Method: _WikiFileActionRecursive(wfi, wfa, act)

Recursive Action, use in:


| Name | Description |
| ---- | ----------- |
| wfi | *stDokuWiki.WikiEngine.WikiFolderInfo*<br> |
| wfa | *stDokuWiki.WikiEngine.WikiFileAction*<br> |
| act | *System.Action{stDokuWiki.WikiEngine.WikiFileAction}*<br>Action<WikiEngine.WikiFileAction> |


> #### Returns
> Bolean


### Method: _WikiFileCacheClear

Clear Cache



### Method: _WikiFileCacheFilePath(cacheid)

Create Cache path


| Name | Description |
| ---- | ----------- |
| cacheid | *System.String*<br>string Cache ID |


> #### Returns
> 


### Method: _WikiFileCacheRead(cacheid)

Read Cache


| Name | Description |
| ---- | ----------- |
| cacheid | *System.String*<br>string Cache ID |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: _WikiFileCacheWrite(wfi, cacheid)

Write Cache


| Name | Description |
| ---- | ----------- |
| wfi | *stDokuWiki.WikiEngine.WikiFolderInfo*<br>WikiEngine.WikiFolderInfo input |
| cacheid | *System.String*<br>string Cache ID |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: _WikiFileGetFileInfo(wfp)

Get file information from FS from WikiEngine.WikiFileParse internal class


| Name | Description |
| ---- | ----------- |
| wfp | *stDokuWiki.WikiEngine.WikiFile.WikiFileParse*<br>internal class WikiEngine.WikiFile.WikiFileParse |


> #### Returns
> WikiEngine.WikiFileInfo


### Method: _WikiFileGetFileInfo(wft, namesspace, file)

Get file information from FS from string


| Name | Description |
| ---- | ----------- |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br>type enum WikiEngine.WikiFileType |
| namesspace | *System.String*<br>wiki namespace include ':' |
| file | *System.String*<br>page/media name (serch patern string) |


> #### Returns
> WikiEngine.WikiFileInfo


### Method: _WikiFilesParse(stDokuWiki.WikiEngine.WikiFileType,System.String,System.String,System.String,System.Boolean)

internal Uri/Namespace parse


| Name | Description |
| ---- | ----------- |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br>type enum WikiEngine.WikiFileType |
| namesspace | *System.String*<br>wiki namespace include ':' |
| file | *System.String*<br>page/media name (serch patern string) |


> #### Returns
> WikiEngine.WikiFile.WikiFileParse


### Method: _WikiFindList(stDokuWiki.WikiEngine.WikiFileType,System.String)

find pages list



### Method: _WikiGetFile(stDokuWiki.WikiEngine.WikiFileType,System.String)

get page/media/attic/meta



### Method: _WikiGetHome(System.String)

get Home location, Exception to load Home page default (html template)



### Method: _WikiGetList(stDokuWiki.WikiEngine.WikiFileType,System.String)

get page list default ?



### Method: _WikiPutFile(stDokuWiki.WikiEngine.WikiFileType,System.String,stDokuWiki.WikiEngine.WikiFileMeta)

put page/media



### F:atticExtension

Default attic extensions



### F:binExtension

Default media extensions



### Method: Finalize

Destructor class WikiFile



### Method: FindFiles(type, search, namesspace, strong)

Find wiki files, return data:
class WikiFolderInfo
class WikiFileInfo
for file/page name mask


##### Code:

```csharp

WikiFolderInfo wfi = wf.FindFiles(WikiEngine.WikiFileType.FileReadMd,"myfilename","mynamespace:");
              if (wfi == null)
              {
                 return;
              }
              Console.WriteLine("find pattern: " + wfi.SearchPatern);
              Console.WriteLine("start name space: " + wfi.NamespacePath);
              foreach (var items in (Dictionary<string, WikiFolderInfo>)wfi.Dirs)
              {
                 Console.WriteLine(" name space: " + items.Key);
                 foreach (WikiFileInfo item in ((WikiFolderInfo)items.Value).Files)
                 {
                    Console.WriteLine("   page/media name: " + item.FileName);
                 }
              }
```

| Name | Description |
| ---- | ----------- |
| type | *stDokuWiki.WikiEngine.WikiFileType*<br>object type: enum WikiEngine.WikiFileType |
| search | *System.String*<br>String search pattern |
| namesspace | *System.String*<br>String Namespace (optionale) |
| strong | *System.Boolean*<br>Bolean, is true search stop is one matching (optionale) |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: FindFilesAttic(search, strong)

Get wiki attic file  iformation


##### Example:

```
Example view code: WikiEngine.WikiFile.FindFiles()
```

| Name | Description |
| ---- | ----------- |
| search | *System.String*<br>Wiki archive/file name mask |
| strong | *System.Boolean*<br>Bolean, strong search if true |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: FindFilesMedia(search, strong)

Get wiki media file  media


##### Example:

```
Example view code: WikiEngine.WikiFile.FindFiles()
```

| Name | Description |
| ---- | ----------- |
| search | *System.String*<br>Wiki media/file name mask |
| strong | *System.Boolean*<br>Bolean, strong search if true |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: FindFilesPage(search, strong)

Find wiki page files  for file name mask


##### Example:

```
Example view code: WikiEngine.WikiFile.FindFiles()
```

| Name | Description |
| ---- | ----------- |
| search | *System.String*<br>Wiki page/file name mask |
| strong | *System.Boolean*<br>Bolean, strong search if true |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: GetFile(namesspace, wft)

Get file content as byte [] from wiki namespace


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br>type of request WikiEngine.WikiFileType |


> #### Returns
> WikiEngine.WikiData


### Method: GetFileFromAttic(namesspace, dt)

See  WikiFile.GetFileFromAttic(string, string)


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| dt | *System.DateTime*<br>DateTime format |


> #### Returns
> 


### Method: GetFileFromAttic(namesspace, uts)

See  WikiFile.GetFileFromAttic(string, string)


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| uts | *System.Int32*<br>Unix Unix TimeStamp, Int32 |


> #### Returns
> 


### Method: GetFileFromAttic(namesspace, stime)

Get file content from Attic backup (wiki namespace required)


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| stime | *System.String*<br>string date and time |


> #### Returns
> WikiEngine.WikiData


### Method: GetFileInfo(wft, namesspace)

Get wiki file  iformation


##### Code:

```csharp

WikiEngine.WikiFileInfo wfi = wf.GetFileInfo(WikiEngine.WikiFileType.FileReadMd,"testns:filepage1");
              if (wfi != null)
              {
                 Console.WriteLine(
                    " File: " + wfi.FileName +
                    " LastAccessTime: " + wfi.LastAccessTime +
                    " LastWriteTime: " + wfi.LastWriteTime
                 );
              }
```

| Name | Description |
| ---- | ----------- |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br>object type: enum WikiEngine.WikiFileType |
| namesspace | *System.String*<br>Wiki name space include page name |


> #### Returns
> WikiEngine.WikiFileInfo


### Method: GetFileInfoAttic(namesspace)

Get wiki attic file  iformation


##### Example:

```
Example view code: WikiEngine.WikiFile.GetFileInfo()
```

| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>Wiki name space include page name |


> #### Returns
> WikiEngine.WikiFileInfo


### Method: GetFileInfoMedia(namesspace)

Get wiki media file  iformation


##### Example:

```
Example view code: WikiEngine.WikiFile.GetFileInfo()
```

| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>Wiki name space include page name |


> #### Returns
> WikiEngine.WikiFileInfo


### Method: GetFileInfoPage(namesspace)

Get wiki page file  iformation


##### Example:

```
Example view code: WikiEngine.WikiFile.GetFileInfo()
```

| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>Wiki name space include page name |


> #### Returns
> WikiEngine.WikiFileInfo


### Method: GetFileList(wft, namesspace)

Get file list


##### Example:

```

```

##### Code:

```csharp

WikiFolderInfo wfi = wf.GetFileList(WikiEngine.WikiFileType.FileReadMd,"testns:");
              if (wfi == null)
              {
                Console.WriteLine("return empty..");
                return;
              }
              Console.WriteLine(
                "total directories: " + wfi.Dirs.Count + Environment.NewLine +
                "total files: " + wfi.Files.Count
              );
              foreach (var items in wfi.Files)
              {
                 Console.WriteLine(" name space: " + items.Key);
                 foreach (var item in items.Value)
                 {
                    Console.WriteLine("   page/file: " + item.FileName);
                 }
              }
```

| Name | Description |
| ---- | ----------- |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br>object type: enum WikiEngine.WikiFileType |
| namesspace | *System.String*<br>Wiki name space |


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: GetFileListAttic

Get Root attic file list


##### Example:

```
Example view code: WikiEngine.GetFileList()
```


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: GetFileListMedia

Get Root media file list


##### Example:

```
Example view code: WikiEngine.GetFileList()
```


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: GetFileListPages

Get Root page file list


##### Example:

```
Example view code: WikiEngine.GetFileList()
```


> #### Returns
> WikiEngine.WikiFolderInfo


### Method: GetFileMeta(namesspace)

Get Meta data from file, return List WikiEngine.WikiMetaChanges


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |


> #### Returns
> List<WikiEngine.WikiMetaChanges>


### Method: GetFileToString(namesspace, wft)

Get file content as string from wiki namespace


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br>WikiEngine.WikiFileType, return in namespace, is found |


> #### Returns
> WikiEngine.WikiData


### Method: LockFile(type, namesspace, fname, status)

Lock/Unlock file for writing (pages/media)


| Name | Description |
| ---- | ----------- |
| type | *stDokuWiki.WikiEngine.WikiFileType*<br>enum WikiFileType |
| namesspace | *System.String*<br>wiki namespace include ':' |
| fname | *System.String*<br>file/page/media name or null |
| status | *System.Boolean*<br>Lock = true, Unlock = false |


> #### Returns
> Bolean, true is succesful


### Method: LockFileMedia(namesspace, status)

Lock/Unlock media file
See  WikiFile.LockFile(WikiFileType,string,string,bool)


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include media file name |
| status | *System.Boolean*<br>Lock = true, Unlock = false |


> #### Returns
> Bolean, true is succesful


### Method: LockFilePage(namesspace, status)

Lock/Unlock page file
See  WikiFile.LockFile(WikiFileType,string,string,bool)


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include page name |
| status | *System.Boolean*<br>Lock = true, Unlock = false |


> #### Returns
> Bolean, true is succesful


### F:mdExtension

Default pages extensions



### F:metaExtension

Default meta extensions



### Method: MetaListToMdString(lwmc, sb)

ToString method from List<WikiMetaChanges> to MarkDown


| Name | Description |
| ---- | ----------- |
| lwmc | *System.Collections.Generic.List{stDokuWiki.WikiEngine.WikiMetaChanges}*<br>Data in List<WikiFileAttic> format |
| sb | *System.Text.StringBuilder*<br>StringBuilder instance or null |


> #### Returns
> String or String.Empty


### E:OnProcessError

Event Wiki Error, return WikiEngine.WikiErrorEventArgs



### E:OnWikiFSChange

Event Wiki File System change WikiEngine.WikiFSChangeEventArgs



### Method: PageToMdString(wd, isPageInfo, sb)

ToString method from WikiEngine.WikiData to MarkDown


| Name | Description |
| ---- | ----------- |
| wd | *stDokuWiki.WikiEngine.WikiData*<br>Data in WikiEngine.WikiData format |
| isPageInfo | *System.Boolean*<br>Bolean print Page information before |
| sb | *System.Text.StringBuilder*<br>StringBuilder instance or null |


> #### Returns
> String or String.Empty


### Method: PutFile(namesspace, data, wfm)

Put file content to wiki namespace


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| data | *System.Byte[]*<br>byte [] data to wiki file |
| wfm | *stDokuWiki.WikiEngine.WikiFileMeta*<br>Meta data WikiEngine.WikiFileMeta |


> #### Returns
> WikiEngine.WikiData


### Method: PutFileHtmlDoc(namesspace, data, wfm)

Put file content to wiki namespace,
input source is completed HTML document, output is Markdown


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| data | *System.Byte[]*<br>byte [] data to wiki file |
| wfm | *stDokuWiki.WikiEngine.WikiFileMeta*<br>Meta data WikiEngine.WikiFileMeta |


> #### Returns
> WikiEngine.WikiData


### Method: PutFileHtmlText(namesspace, data, wfm)

Put file content to wiki namespace,
input source is HTML fragment, output is Markdown


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>wiki namespace include ':' |
| data | *System.Byte[]*<br>byte [] data to wiki file |
| wfm | *stDokuWiki.WikiEngine.WikiFileMeta*<br>Meta data WikiEngine.WikiFileMeta |


> #### Returns
> WikiEngine.WikiData


### Method: ResourceListToMdString(wfi, sb, type)

ToString method from WikiEngine.WikiFolderInfo to MarkDown
Page/Media/Attic List


| Name | Description |
| ---- | ----------- |
| wfi | *stDokuWiki.WikiEngine.WikiFolderInfo*<br>WikiEngine.WikiFolderInfo input |
| sb | *System.Text.StringBuilder*<br>StringBuilder instance or null |
| type | *stDokuWiki.WikiEngine.WikiFileType*<br>WikiEngine.WikiFileType, if this equals WikiFileType.FileReadAttic, return only date from attic |


> #### Returns
> String or String.Empty


### Method: RouteTree(url, wfm)

Wiki Route tree (Web)
return requested resource from wiki file system.


##### Example:

```
Uri: /wiki/get/pages|attic|media|meta/clan:test1
             Uri: /wiki/put/pages|attic|media/clan:test1
             Uri: /wiki/del/pages|attic|media/clan:test1
             Uri: /wiki/list/pages|attic|media/clan:
             Uri: /wiki/find/part-of-serch-page-name
             enum WikiEngine.WikiRequestType: get|put|del|list|find
             enum WikiEngine.WikiFileType: pages|attic|media|meta
```


> #### Remarks
>  Example routing Uri: "/wiki/get/pages/clan:test1" OK "/wiki/get/clan:test1" OK "/wiki/clan:test1" -> HomeException "/wiki/clan:" -> HomeException "/wiki/" -> HomeException (redirect) "/wiki/list/pages/clan:" OK "/wiki/list/clan:" OK (default pages) "/wiki/list/" OK (default pages and default namespace) "/wiki/list/media/clan:" OK "/wiki/list/media/" OK (default namespace) "/wiki/find/pages/tes" OK "/wiki/find/media/tes" OK "/wiki/find/attic/tes" OK "/wiki/find/tes" OK (default pages and default namespace) 

*stDokuWiki.WikiEngine.Exceptions.WikiEngineAuthException:* WikiEngineAuthException

*stDokuWiki.WikiEngine.Exceptions.WikiEngineSearchException:* WikiEngineSearchException

*stDokuWiki.WikiEngine.Exceptions.WikiEngineHomePageException:* WikiEngineHomePageException

*stDokuWiki.WikiEngine.Exceptions.WikiEngineErrorPageException:* WikiEngineErrorPageException

*stDokuWiki.WikiEngine.Exceptions.WikiEngineNotImplementPageException:* WikiEngineNotImplementPageException

| Name | Description |
| ---- | ----------- |
| url | *System.String*<br>given raw url requested |
| wfm | *stDokuWiki.WikiEngine.WikiFileMeta*<br>WikiEngine.WikiFileMeta |


> #### Returns
> byte[] from requested source


### F:wikiDefaultEmptyNS

Default Namespace to unknown write namespace



### F:wikiDefaultSeparator

Default separator from runing system



### Method: WikiFileParse.#ctor(wft, strong, namesspace, search, atticid, iswrite)

Constructor


| Name | Description |
| ---- | ----------- |
| wft | *stDokuWiki.WikiEngine.WikiFileType*<br> |
| strong | *System.Boolean*<br> |
| namesspace | *System.String*<br> |
| search | *System.String*<br> |
| atticid | *System.String*<br> |
| iswrite | *System.Boolean*<br> |

### F:wikiLocalAttic

Default Wiki root/attic folder



### F:wikiLocalCache

Default Wiki root/cache folder



### F:wikiLocalCacheFind

Default Wiki root/cache/find folder, store and read find request



### F:wikiLocalMedia

Default Wiki root/media folder



### F:wikiLocalMeta

Default Wiki root/meta folder



### F:wikiLocalPage

Default Wiki root/pages folder



### F:wikiLocalPath

Default Wiki root folder



## Class: stDokuWiki.WikiEngine.WikiFileAction

Wiki file Action class



### Method: #ctor(wd, status)

Constructor WikiFileAction


| Name | Description |
| ---- | ----------- |
| wd | *stDokuWiki.WikiEngine.WikiData*<br>class WikiData |
| status | *System.Boolean*<br>set status |

### Method: #ctor(wd, status, sb)

Constructor WikiFileAction


| Name | Description |
| ---- | ----------- |
| wd | *stDokuWiki.WikiEngine.WikiData*<br>class WikiData |
| status | *System.Boolean*<br>set status |
| sb | *System.Text.StringBuilder*<br>String Builder instance |

### Method: #ctor(namesspace, search, isallfiles, status, sb)

Constructor WikiFileAction


| Name | Description |
| ---- | ----------- |
| namesspace | *System.String*<br>full Namespace path |
| search | *System.String*<br>search string |
| isallfiles | *System.Boolean*<br>process all files |
| status | *System.Boolean*<br>set status |
| sb | *System.Text.StringBuilder*<br>String Builder instance |

### Method: #ctor(sb)

Constructor WikiFileAction


| Name | Description |
| ---- | ----------- |
| sb | *System.Text.StringBuilder*<br>String Builder instance |

## Class: stDokuWiki.WikiEngine.WikiFileAttic

Wiki file Attic class



## Class: stDokuWiki.WikiEngine.WikiFileInfo

Wiki file Info class



### Method: Clone

Clone this



> #### Returns
> (object) WikiFileInfo


## Class: stDokuWiki.WikiEngine.WikiFileMeta

Wiki file Meta class



### Method: #ctor(author, authorip)

Constructor WikiFileMeta class


| Name | Description |
| ---- | ----------- |
| author | *System.String*<br>Author name |
| authorip | *System.String*<br>Author IP |

## Class: stDokuWiki.WikiEngine.WikiFileType

enum Wiki FS file type



### F:FileReadAttic

Read Attic recent changes file



### F:FileReadBinary

Read Media or binary file



### F:FileReadMd

Read Page MarkDown file



### F:FileReadMeta

Read meta file



### F:FileUnknown

File type Unknown



### F:FileWriteAttic

Write Attic recent changes file



### F:FileWriteBinary

Write Media or binary file



### F:FileWriteMd

Write Page MarkDown file



### F:FileWriteMeta

Write meta file



### F:NameSpace

File type is NameSpace



### F:None

None - not set, default



## Class: stDokuWiki.WikiEngine.WikiFolderInfo

Wiki file system folder



### Method: #ctor

Constructor WikiFolderInfo



### Method: Clone

Clone this



> #### Returns
> (object) WikiFolderInfo


## Class: stDokuWiki.WikiEngine.WikiFSChangeEventArgs

Event Wiki File System change



### Method: #ctor(wf, count, st)

Constructor


| Name | Description |
| ---- | ----------- |
| wf | *stDokuWiki.WikiEngine.WikiFile*<br>class WikiFile |
| count | *System.Int32*<br>File system total root name spaces |
| st | *System.Int32*<br>Scaning total time msec. |

## Class: stDokuWiki.WikiEngine.WikiMetaChanges

Wiki file Meta .changes class



### F:UnixTimeStamp

Unix Time stamp



## Class: stDokuWiki.WikiEngine.WikiRequestType

Type Wiki Web Reguest



### F:Del

Del page/file



### F:Find

Find page/file



### F:Get

Get page/file



### F:List

List page/file



### F:None

None - default



### F:Put

Put page/file



## Class: stDokuWiki.WikiEngine.WikiSyntaxErrorEventArgs

Event Arguments WikiSyntax Error



### Method: #ctor(ex)

Constructor


| Name | Description |
| ---- | ----------- |
| ex | *System.Exception*<br>Exception  |

## Class: stDokuWiki.WikiSyntax.DokuWikiFormat

Convert DokuWiki format and syntax


##### Example:

```
using stDokuWiki.WikiSyntax;
              string htmldoc = "<html><head></head><body>...</body></html>";
              string htmltxt = "<h3>...</h3><br/><i>...</i><br/>";
              string mdtxt   = "== test header ==\n **bold** text and //italic// and __underline__\n";
              DokuWikiFormat dwf = new DokuWikiFormat();
              dwf.OnProcessError += (o, e) =>
              {
                 Console.WriteLine(e.ex.GetType().Name + ": " + e.ex.Message);
              };
              Console.WriteLine(
                 dwf.HtmlDocToDokuWiki(htmldoc)
              );
              Console.WriteLine(
                 dwf.HtmlTextToDokuWiki(htmltxt)
              );
              Console.WriteLine(
                 dwf.DokuWikiToHtml(mdtxt)
              );
```


### Method: #ctor(isToMd, isToHtml)

Init DokuWikiFormat constructor


| Name | Description |
| ---- | ----------- |
| isToMd | *System.Boolean*<br>Bolean - enable/disable convert Html to DokuWiki format |
| isToHtml | *System.Boolean*<br>Bolean - enable/disable convert DokuWiki to Html format |

### Method: DokuWikiToHtml(mdtext)

DokuWiki markdown text to Html format


| Name | Description |
| ---- | ----------- |
| mdtext | *System.String*<br>DokuWiki markdown text source |


> #### Returns
> String Html


### Method: HtmlDocToDokuWiki(html)

Html Document to DokuWiki markdown format


| Name | Description |
| ---- | ----------- |
| html | *System.String*<br>Html dokument source |


> #### Returns
> String markdown DokuWiki


### Method: HtmlTextToDokuWiki(html)

Html text to DokuWiki markdown format


| Name | Description |
| ---- | ----------- |
| html | *System.String*<br>Html text source |


> #### Returns
> String markdown DokuWiki


### E:OnProcessError

Event DokuWikiFormat Error, return WikiEngine.WikiSyntaxErrorEventArgs



### Method: XHtmlToDokuWiki(xhtml)

XHtml to DokuWiki markdown format


| Name | Description |
| ---- | ----------- |
| xhtml | *System.String*<br>XHtml complette source |


> #### Returns
> String markdown DokuWiki

