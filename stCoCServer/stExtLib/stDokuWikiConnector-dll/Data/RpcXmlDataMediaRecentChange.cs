using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki getRecentMediaChanges result ( XMLMethodMediaRecentChange )

    /// <summary>
    /// Return from function: DokuRecentMediaChanges
    /// Enum (XmlRpcRequest): RecentMediaChanges
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodMediaRecentChange
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLMediaRecentChangeParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLMediaRecentChangeValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "dateTime.iso8601")]
        public string DateTimeIso8601 { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
        [XmlElement(ElementName = "boolean")]
        public string Boolean { get; set; }
        [XmlElement(ElementName = "struct")]
        public XMLMediaRecentChangeStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLMediaRecentChangeMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLMediaRecentChangeValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLMediaRecentChangeStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLMediaRecentChangeMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XMLMediaRecentChangeData
    {
        [XmlElement(ElementName = "value")]
        public List<XMLMediaRecentChangeValue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLMediaRecentChangeArray
    {
        [XmlElement(ElementName = "data")]
        public XMLMediaRecentChangeData Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLMediaRecentChangeParam
    {
        [XmlElement(ElementName = "value")]
        public XMLMediaRecentChangeValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLMediaRecentChangeParams
    {
        [XmlElement(ElementName = "param")]
        public XMLMediaRecentChangeParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}