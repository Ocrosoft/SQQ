using Newtonsoft.Json;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 获取分配开关状态
    /// </summary>
    public class getDispatch : IHttpHandler
    {
        public class Status
        {
            public bool status;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new Status() { status = Sys.DispatchEnabled }));
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