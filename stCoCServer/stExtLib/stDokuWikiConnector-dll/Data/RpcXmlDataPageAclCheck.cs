using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki PageAclCheck result ( XMLMethodPageAclCheck )

    /// <summary>
    /// Return from function: DokuPageAclCheck
    /// Enum (XmlRpcRequest): wiki_aclCheck
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodPageAclCheck
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLPageAclCheckParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLPageAclCheckParams
    {
        [XmlElement(ElementName = "param")]
        public XMLPageAclCheckParam Param { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLPageAclCheckParam
    {
        [XmlElement(ElementName = "value")]
        public XMLPageAclCheckValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLPageAclCheckValue
    {
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
