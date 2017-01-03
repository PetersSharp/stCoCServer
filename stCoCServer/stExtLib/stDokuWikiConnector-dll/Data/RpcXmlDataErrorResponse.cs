using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki ErrorResponse result ( XMLMethodErrorResponse )

    /// <summary>
    /// Use to check wiki error response
    /// Enum (XmlRpcRequest): none
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodErrorResponse
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "fault")]
        public XMLErrorResponseFault Fault { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "fault")]
    public class XMLErrorResponseFault
    {
        [XmlElement(ElementName = "value")]
        public XMLErrorResponseXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLErrorResponseXvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLErrorResponseStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLErrorResponseStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLErrorResponseMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLErrorResponseMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLErrorResponseValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLErrorResponseValue
    {
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
