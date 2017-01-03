using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki Page List ( XMLMethodPageList )

    /// <summary>
    /// Return from function: DokuPageList(string NamesSpace, string Options)
    /// Enum (XmlRpcRequest): dokuwiki_getPagelist
    /// </summary>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-dokuwiki_getPagelist.xml"/>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-dokuwiki_getPagelist.xml"/>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageList
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageListParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageListParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageListParam Param { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageListParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageListXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageListXvalue
    {
        [XmlElement(ElementName = "array")]
        public XMLPageListArray Array { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLPageListArray
    {
        [XmlElement(ElementName = "data")]
        public XMLPageListData Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XMLPageListData
    {
        [XmlElement(ElementName = "value")]
        public List<XMLPageListYvalue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageListYvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLPageListStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLPageListStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLPageListMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLPageListMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLPageListPvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageListPvalue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
