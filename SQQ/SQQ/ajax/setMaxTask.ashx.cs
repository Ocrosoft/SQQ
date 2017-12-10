using Newtonsoft.Json;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 设置最大任务数
    /// </summary>
    public class setMaxTask : IHttpHandler
    {
        public class Ret
        {
            public string code;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var maxTask = context.Request.QueryString["maxTask"];
            int mtask = 0;
            if(maxTask == null || !int.TryParse(maxTask,out mtask))
            {
                Sys.Log("setMaxTask.ashx: 非正常访问(参数不正确)");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                return;
            }

            Sys.MaxTask = mtask;
            Sys.Log("设置最大同时配送数量为 " + mtask);
            context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
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