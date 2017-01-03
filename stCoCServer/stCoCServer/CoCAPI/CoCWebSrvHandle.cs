using System;

namespace stCoCServer.CoCAPI
{
    public static class CoCWebSrvHandleSettings
    {
        private const string constFileWebRequest = "FileWebRequest";
        private const string constTemplateWebRequest = "TemplateWebRequest";
        private const string constJsonWebRequest = "JsonWebRequest";
        private const string constServerSentEventWebRequest = "ServerSentEventWebRequest";
        private const string constInformerWebRequest = "InformerWebRequest";

        public static Action<string, object, object> HttpHandleAction(stNet.WebHandleTypes htype)
        {
            switch (htype)
            {
                default:
                case stNet.WebHandleTypes.FileWebRequest:
                    {
                        return CoCWebSrv.FileWebRequest;
                    }
                case stNet.WebHandleTypes.TemplateWebRequest:
                    {
                        return CoCWebSrv.TemplateWebRequest;
                    }
                case stNet.WebHandleTypes.JsonWebRequest:
                    {
                        return CoCWebSrv.JsonWebRequest;
                    }
                case stNet.WebHandleTypes.ServerSentEventWebRequest:
                    {
                        return CoCWebSrv.SseWebRequest;
                    }
                case stNet.WebHandleTypes.InformerWebRequest:
                    {
                        return CoCWebSrv.InformerWebRequest;
                    }
            }
        }
        public static Action<string, object, object> HttpHandleAction(string stype)
        {
            switch (stype)
            {
                default:
                case constFileWebRequest:
                    {
                        return CoCWebSrv.FileWebRequest;
                    }
                case constTemplateWebRequest:
                    {
                        return CoCWebSrv.TemplateWebRequest;
                    }
                case constJsonWebRequest:
                    {
                        return CoCWebSrv.JsonWebRequest;
                    }
                case constServerSentEventWebRequest:
                    {
                        return CoCWebSrv.SseWebRequest;
                    }
                case constInformerWebRequest:
                    {
                        return CoCWebSrv.InformerWebRequest;
                    }
            }
        }
        public static bool HttpHandleBool(stNet.WebHandleTypes htype)
        {
            switch (htype)
            {
                default:
                case stNet.WebHandleTypes.FileWebRequest:
                    {
                        return true;
                    }
                case stNet.WebHandleTypes.TemplateWebRequest:
                    {
                        return false;
                    }
                case stNet.WebHandleTypes.JsonWebRequest:
                    {
                        return false;
                    }
                case stNet.WebHandleTypes.ServerSentEventWebRequest:
                    {
                        return false;
                    }
                case stNet.WebHandleTypes.InformerWebRequest:
                    {
                        return false;
                    }
            }
        }
        public static bool HttpHandleBool(string stype)
        {
            switch (stype)
            {
                default:
                case constFileWebRequest:
                    {
                        return true;
                    }
                case constTemplateWebRequest:
                    {
                        return false;
                    }
                case constJsonWebRequest:
                    {
                        return false;
                    }
                case constServerSentEventWebRequest:
                    {
                        return false;
                    }
                case constInformerWebRequest:
                    {
                        return false;
                    }
            }
        }
        public static stNet.WebHandleTypes HttpHandleType(string stype)
        {
            switch (stype)
            {
                default:
                case constFileWebRequest:
                    {
                        return stNet.WebHandleTypes.FileWebRequest;
                    }
                case constTemplateWebRequest:
                    {
                        return stNet.WebHandleTypes.TemplateWebRequest;
                    }
                case constJsonWebRequest:
                    {
                        return stNet.WebHandleTypes.JsonWebRequest;
                    }
                case constServerSentEventWebRequest:
                    {
                        return stNet.WebHandleTypes.ServerSentEventWebRequest;
                    }
                case constInformerWebRequest:
                    {
                        return stNet.WebHandleTypes.InformerWebRequest;
                    }
            }
        }
    }
}
