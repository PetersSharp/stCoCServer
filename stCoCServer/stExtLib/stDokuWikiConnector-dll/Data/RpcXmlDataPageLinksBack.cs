using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki Page BackLinks result ( XMLMethodPageLinksBack )

    /// <summary>
    /// Return from function: DokuPageLinksBack(string PageName)
    /// Enum (XmlRpcRequest): wiki_getBackLinks
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageLinksBack
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageLinksBackParams Params { get; set; }
    }


    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageLinksBackValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "data")]
    public class XMLPageLinksBackData
    {
        [XmlElement(ElementName = "value")]
        public List<XMLPageLinksBackValue> Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "array")]
    public class XMLPageLinksBackArray
    {
        [XmlElement(ElementName = "data")]
        public XMLPageLinksBackData Data { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageLinksBackParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageLinksBackValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageLinksBackParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageLinksBackParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
