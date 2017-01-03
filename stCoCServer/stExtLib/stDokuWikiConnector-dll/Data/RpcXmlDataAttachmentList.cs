using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki wiki.getAttachments result ( XMLMethodAttachmentList )

    /// <summary>
    /// Return from DokuAttachmentList(string NameSpace, string Options)
    /// Enum (XmlRpcRequest): wiki_getAttachments
    /// </summary>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAttachments.xml"/>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAttachments.xml"/>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodAttachmentList
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLAttachmentListParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLAttachmentListValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
        [XmlElement(ElementName = "boolean")]
        public string Boolean { get; set; }
        [XmlElement(ElementName = "dateTime.iso8601")]
        public string DateTimeIso8601 { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLAttachmentListMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLAttachmentListValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLAttachmentListStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLAttachmentListMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLAttachmentListYvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLAttachmentListStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class Data
    {
        [XmlElement(ElementName = "value")]
        public List<XMLAttachmentListYvalue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLAttachmentListArray
    {
        [XmlElement(ElementName = "data")]
        public Data Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLAttachmentListXvalue
    {
        [XmlElement(ElementName = "array")]
        public XMLAttachmentListArray Array { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLAttachmentListParam
    {
        [XmlElement(ElementName = "value")]
        public XMLAttachmentListXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLAttachmentListParams
    {
        [XmlElement(ElementName = "param")]
        public XMLAttachmentListParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
