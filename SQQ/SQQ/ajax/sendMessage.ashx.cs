using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SQQ.ajax
{
    /// <summary>
    /// sendMessage 的摘要说明
    /// </summary>
    public class sendMessage : IHttpHandler
    {
        public class Ret
        {
            public string code;
            public string info;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var open_id = context.Request.Form["oid"];
            var message = context.Request.Form["msg"];

            if(open_id == "-1")
            {
                var senders = Database.GetsSender();
                int count = 0;
                foreach(var sender in senders)
                {
                    if(WXManage.SendMessage(sender.open_id, message))
                    {
                        count++;
                    }
                }
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret()
                {
                    code = count != senders.Count ? "err" : "success",
                    info = count + " message has send, " + (senders.Count - count) + " message send failed."
                }));
                return;
            }

            if (WXManage.SendMessage(open_id, message)) 
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret()
                {
                    code = "success",
                    info = "message sended."
                }));
            }
            else
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret()
                {
                    code = "err",
                    info = "can not send message."
                }));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}