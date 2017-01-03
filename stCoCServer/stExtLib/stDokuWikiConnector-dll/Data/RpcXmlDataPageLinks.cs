using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki Links result ( XMLMethodPageLinks )

    /// <summary>
    /// Return from function: DokuPageLinks(string PageName)
    /// Enum (XmlRpcRequest): wiki_listLinks
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageLinks
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageLinksParams Params { get; set; }
    }


    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageLinksValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "struct")]
        public XMLPageLinksStruct Struct { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "member")]
    public class XMLPageLinksMember
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public XMLPageLinksValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "struct")]
    public class XMLPageLinksStruct
    {
        [XmlElement(ElementName = "member")]
        public List<XMLPageLinksMember> Member { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XMLPageLinksData
    {
        [XmlElement(ElementName = "value")]
        public List<XMLPageLinksValue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLPageLinksArray
    {
        [XmlElement(ElementName = "data")]
        public XMLPageLinksData Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageLinksParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageLinksValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageLinksParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageLinksParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}