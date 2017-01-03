using System;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki Bolean result ( XMLMethodBool )

    /// <summary>
    /// Use in return bool function: DokuAuth, 
    /// Enum (XmlRpcRequest): none
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodBool
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLBoolParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLBoolValue
    {
        [XmlElement(ElementName = "boolean")]
        public string Boolean { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLBoolParam
    {
        [XmlElement(ElementName = "value")]
        public XMLBoolValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLBoolParams
    {
        [XmlElement(ElementName = "param")]
        public XMLBoolParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
