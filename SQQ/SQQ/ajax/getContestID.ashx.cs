using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SQQ
{
    /// <summary>
    /// 获取系统要处理的比赛的ID
    /// </summary>
    public class getContestID : IHttpHandler
    {
        public class Ret
        {
            public int contest_id;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            context.Response.Write(JsonConvert.SerializeObject(new Ret() { contest_id = Sys.ContestID }));
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