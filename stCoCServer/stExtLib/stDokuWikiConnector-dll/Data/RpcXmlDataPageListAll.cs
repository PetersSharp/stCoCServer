using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki All Page List result ( XMLMethodPageListAll )

    /// <summary>
    /// Return from function: DokuPageListAll()
    /// Enum (XmlRpcRequest): wiki_getAllPages
    /// </summary>
    /// <examle>
    /// Warning! Big Data return!
    /// </examle>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAllPages.xml"/>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAllPages.xml"/>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageListAll
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageListAllParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageListAllValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
        [XmlElement(ElementName = "dateTime.iso8601")]
        public string DateTimeIso8601 { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLPageListAllMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLPageListAllValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLPageListAllStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLPageListAllMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageListAllYvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLPageListAllStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XMLPageListAllData
    {
        [XmlElement(ElementName = "value")]
        public List<XMLPageListAllYvalue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLPageListAllArray
    {
        [XmlElement(ElementName = "data")]
        public XMLPageListAllData Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageListAllXvalue
    {
        [XmlElement(ElementName = "array")]
        public XMLPageListAllArray Array { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageListAllParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageListAllXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageListAllParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageListAllParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
