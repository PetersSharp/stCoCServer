using System;

namespace stCoCServer.CoCAPI
{
    public static class CoCWebSrvHandleSettings
    {
        private const string constFileWebRquest = "FileWebRquest";
        private const string constTemplateWebRquest = "TemplateWebRquest";
        private const string constJsonWebRquest = "JsonWebRquest";
        private const string constServerSentEventWebRquest = "ServerSentEventWebRquest";

        public static Action<string, object, object> HttpHandleAction(stNet.WebHandleTypes htype)
        {
            switch (htype)
            {
                default:
                case stNet.WebHandleTypes.FileWebRquest:
                    {
                        return CoCWebSrv.FileWebRquest;
                    }
                case stNet.WebHandleTypes.TemplateWebRquest:
                    {
                        return CoCWebSrv.TemplateWebRquest;
                    }
                case stNet.WebHandleTypes.JsonWebRquest:
                    {
                        return CoCWebSrv.JsonWebRquest;
                    }
                case stNet.WebHandleTypes.ServerSentEventWebRquest:
                    {
                        return CoCWebSrv.SseWebRquest;
                    }
            }
        }
        public static Action<string, object, object> HttpHandleAction(string stype)
        {
            switch (stype)
            {
                default:
                case constFileWebRquest:
                    {
                        return CoCWebSrv.FileWebRquest;
                    }
                case constTemplateWebRquest:
                    {
                        return CoCWebSrv.TemplateWebRquest;
                    }
                case constJsonWebRquest:
                    {
                        return CoCWebSrv.JsonWebRquest;
                    }
                case constServerSentEventWebRquest:
                    {
                        return CoCWebSrv.SseWebRquest;
                    }
            }
        }
        public static bool HttpHandleBool(stNet.WebHandleTypes htype)
        {
            switch (htype)
            {
                default:
                case stNet.WebHandleTypes.FileWebRquest:
                    {
                        return true;
                    }
                case stNet.WebHandleTypes.TemplateWebRquest:
                    {
                        return false;
                    }
                case stNet.WebHandleTypes.JsonWebRquest:
                    {
                        return false;
                    }
                case stNet.WebHandleTypes.ServerSentEventWebRquest:
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
                case constFileWebRquest:
                    {
                        return true;
                    }
                case constTemplateWebRquest:
                    {
                        return false;
                    }
                case constJsonWebRquest:
                    {
                        return false;
                    }
                case constServerSentEventWebRquest:
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
                case constFileWebRquest:
                    {
                        return stNet.WebHandleTypes.FileWebRquest;
                    }
                case constTemplateWebRquest:
                    {
                        return stNet.WebHandleTypes.TemplateWebRquest;
                    }
                case constJsonWebRquest:
                    {
                        return stNet.WebHandleTypes.JsonWebRquest;
                    }
                case constServerSentEventWebRquest:
                    {
                        return stNet.WebHandleTypes.ServerSentEventWebRquest;
                    }
            }
        }
    }
}
