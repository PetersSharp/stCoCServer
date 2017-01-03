using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace stDokuWiki.Data
{
    #region DkuWiki wiki.wiki_getAttachment result ( XMLMethodAttachmentGet )

    /// <summary>
    /// Use in DokuAttachmentGet(string id)
    /// Enum (XmlRpcRequest): wiki_getAttachment
    /// </summary>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/request-wiki_getAttachment.xml"/>
    /// <seealso cref="!:https://github.com/PetersSharp/stCoCServer/tree/master/stCoCServer/stExtLib/stDokuWikiConnector-dll/Doc/XMLTable/response-wiki_getAttachment.xml"/>
    [Serializable]
    [XmlRoot(ElementName = "methodResponse")]
    public class XMLMethodAttachmentGet
    {
        #pragma warning disable 1591
        [XmlElement(ElementName = "params")]
        public XMLAttachmentGetParams Params { get; set; }
    }
    
    [Serializable]
    [XmlRoot(ElementName = "value")]
    public class XMLAttachmentGetValue
    {
        [XmlElement(ElementName = "base64")]
        public string Base64 { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "param")]
    public class XMLAttachmentGetParam
    {
        [XmlElement(ElementName = "value")]
        public XMLAttachmentGetValue Value { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "params")]
    public class XMLAttachmentGetParams
    {
        [XmlElement(ElementName = "param")]
        public XMLAttachmentGetParam Param { get; set; }
    }

    #pragma warning restore 1591
    #endregion
}

