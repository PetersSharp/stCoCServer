using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki getXMLRPCAPIVersion/getRPCVersionSupported/getTime result ( XMLMethodGetInt )

    /// <summary>
    /// Use in return int functions: DokuXMLRPCAPIVersion, DokuRPCVersionSupported, DokuDokuGetDateTime
    /// Enum (XmlRpcRequest): wiki_getXMLRPCAPIVersion, wiki_getRPCVersionSupported, wiki_getTime
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodGetInt
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLMethodGetIntParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLMethodGetIntParams
    {
        [XmlElement(ElementName = "param")]
        public XMLMethodGetIntParam Param { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLMethodGetIntParam
    {
        [XmlElement(ElementName = "value")]
        public XMLMethodGetIntValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLMethodGetIntValue
    {
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}