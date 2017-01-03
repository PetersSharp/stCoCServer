using System;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki getTitle/getVersion result ( XMLMethodGetString )

    /// <summary>
    /// Use in return String functions
    /// Enum (XmlRpcRequest):
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodGetString
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLGetStringParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLGetStringValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLGetStringParam
    {
        [XmlElement(ElementName = "value")]
        public XMLGetStringValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLGetStringParams
    {
        [XmlElement(ElementName = "param")]
        public XMLGetStringParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
