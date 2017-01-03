using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region request string Data ( RpcXmlRequestString )

    /// <summary>
    /// Use in request String DokuWiki Data
    /// Enum (XmlRpcRequest): none
    /// </summary>
    [Serializable]
    [XmlRoot("methodCall")]
    public class RpcXmlRequestString
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "methodName")]
        public string MethodName { get; set; }
        [XmlElement(ElementName = "params", IsNullable = true)]
        public RpcXmlStringParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class RpcXmlStringParams
    {
        [XmlElement(ElementName = "param")]
        public RpcXmlStringValue[] Param { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class RpcXmlStringValue
    {
        [XmlElement(ElementName = "value")]
        public RpcXmlStringString Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class RpcXmlStringString
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
    }

    #pragma warning restore 1591
    #endregion

    #region request int Data ( RpcXmlRequestInt )

    /// <summary>
    /// Use in request Int DokuWiki Data
    /// Enum (XmlRpcRequest): none
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodCall")]
    public class RpcXmlRequestInt
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "methodName")]
        public string MethodName { get; set; }
        [XmlElement(ElementName = "params")]
        public RpcXmlIntParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class RpcXmlIntParams
    {
        [XmlElement(ElementName = "param")]
        public RpcXmlIntParam Param { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class RpcXmlIntParam
    {
        [XmlElement(ElementName = "value")]
        public RpcXmlIntValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class RpcXmlIntValue
    {
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
    }

    #pragma warning restore 1591
    #endregion

    #region request string & int Data ( RpcXmlRequestComposite )

    /// <summary>
    /// Use in request composite String and Int DokuWiki Data
    /// Enum (XmlRpcRequest): none
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "methodCall")]
    public class RpcXmlRequestComposite
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "methodName")]
        public string MethodName { get; set; }
        [XmlElement(ElementName = "params")]
        public RpcXmlCompositeParams Params { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class RpcXmlCompositeParams
    {
        [XmlElement(ElementName = "param")]
        public List<RpcXmlCompositeParam> Param { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class RpcXmlCompositeParam
    {
        [XmlElement(ElementName = "value")]
        public RpcXmlCompositeValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class RpcXmlCompositeValue
    {
        [XmlElement(ElementName = "string")]
        public string String { get; set; }
        [XmlElement(ElementName = "int")]
        public string Int { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}
