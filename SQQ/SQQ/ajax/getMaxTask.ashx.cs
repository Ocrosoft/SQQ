using Newtonsoft.Json;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 获取最大任务数
    /// </summary>
    public class getMaxTask : IHttpHandler
    {
        public class Ret
        {
            public int maxTask;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            context.Response.Write(JsonConvert.SerializeObject(new Ret() { maxTask = Sys.MaxTask }));
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