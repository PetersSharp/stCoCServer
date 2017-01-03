using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki Page Info ( XMLMethodPageInfo )

    /// <summary>
    /// Use in function: DokuPageInfo(string PageName), DokuPageInfoVerion(string PageName, ...)
    /// Enum (XmlRpcRequest): wiki_getPageInfo
    /// </summary>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getPageInfoVersion.xml"/>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getPageInfoVersion.xml"/>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageInfo
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageInfoParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageInfoValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "dateTime.iso8601")]
        public string DateTimeIso8601 { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLPageInfoMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLPageInfoValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLPageInfoStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLPageInfoMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageInfoXvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLPageInfoStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageInfoParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageInfoXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageInfoParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageInfoParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
