using System.Xml.Serialization;
using System;

namespace stDokuWiki.Data
{
    #region DkuWiki Page Get ( XMLMethodPageGet )

    /// <summary>
    /// Use in function: DokuPageGet
    /// Enum (XmlRpcRequest): wiki_getPageHTML, wiki_getPage
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageGet
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageGetParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageGetParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageGetParam Param { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageGetParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageGetValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageGetValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
