using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki PageGetVersions result ( XMLMethodPageGetVersions )

    /// <summary>
    /// Use in function: DokuPageGetVersions(string PageName, Int32 StartOffset)
    /// Enum (XmlRpcRequest): wiki_getPageVersions
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageGetVersions
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageGetVersionsParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageGetVersionsValue
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
    public class XMLPageGetVersionsMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLPageGetVersionsValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLPageGetVersionsStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLPageGetVersionsMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageGetVersionsYvalue
    {
        [XmlElement(ElementName = "struct")]
        public XMLPageGetVersionsStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XMLPageGetVersionsData
    {
        [XmlElement(ElementName = "value")]
        public List<XMLPageGetVersionsYvalue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLPageGetVersionsArray
    {
        [XmlElement(ElementName = "data")]
        public XMLPageGetVersionsData Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageGetVersionsXvalue
    {
        [XmlElement(ElementName = "array")]
        public XMLPageGetVersionsArray Array { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageGetVersionsParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageGetVersionsXvalue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageGetVersionsParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageGetVersionsParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
