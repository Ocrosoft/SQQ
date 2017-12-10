using Newtonsoft.Json;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// getLogs 的摘要说明
    /// </summary>
    public class getLogs : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var logs = Sys.GetLog();
            context.Response.Write(JsonConvert.SerializeObject(logs));
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