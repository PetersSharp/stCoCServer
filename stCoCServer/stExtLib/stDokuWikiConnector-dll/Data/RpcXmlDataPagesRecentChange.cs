using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki getRecentChanges result ( XMLMethodRecentChanges )

    /// <summary>
    /// Return from function: DokuPagesChange(int TimeStamp || DateTime dateTime || string dateTime)
    /// Enum (XmlRpcRequest): wiki_getRecentChanges
    /// </summary>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getRecentChanges.xml"/>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getRecentChanges.xml"/>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPagesRecentChange
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPagesRecentChangeParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPagesRecentChangeValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "dateTime.iso8601")]
        public string DateTimeIso8601 { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
        [XmlElement(ElementName = "boolean")]
        public string Boolean { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLPagesRecentChangeMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLPagesRecentChangeValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLPagesRecentChangeStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLPagesRecentChangeMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPagesRecentChangeYvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLPagesRecentChangeStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XMLPagesRecentChangeData
    {
        [XmlElement(ElementName = "value")]
        public List<XMLPagesRecentChangeYvalue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLPagesRecentChangeArray
    {
        [XmlElement(ElementName = "data")]
        public XMLPagesRecentChangeData Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPagesRecentChangeXvalue
    {
        [XmlElement(ElementName = "array")]
        public XMLPagesRecentChangeArray Array { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPagesRecentChangeParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPagesRecentChangeXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPagesRecentChangeParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPagesRecentChangeParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
