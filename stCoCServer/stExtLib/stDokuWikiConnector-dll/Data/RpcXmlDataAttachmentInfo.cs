using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki wiki.getAttachmentInfo result ( XMLMethodAttachmentInfo )

    /// <summary>
    /// Return from DokuAttachmentInfo(string id)
    /// Enum (XmlRpcRequest): wiki_getAttachmentInfo
    /// </summary>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAttachmentInfo.xml"/>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAttachmentInfo.xml"/>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodAttachmentInfo
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLAttachmentInfoParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLAttachmentInfoValue
    {
        [XmlElement(ElementName = "dateTime.iso8601")]
        public string DateTimeIso8601 { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLAttachmentInfoMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLAttachmentInfoValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLAttachmentInfoStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLAttachmentInfoMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLAttachmentInfoXvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLAttachmentInfoStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLAttachmentInfoParam
    {
        [XmlElement(ElementName = "value")]
        public XMLAttachmentInfoXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLAttachmentInfoParams
    {
        [XmlElement(ElementName = "param")]
        public XMLAttachmentInfoParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
