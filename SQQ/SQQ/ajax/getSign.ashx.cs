using Newtonsoft.Json;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 获取登记的开关状态
    /// </summary>
    public class getSign : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            context.Response.Write(JsonConvert.SerializeObject(new getDispatch.Status() { status = Sys.SignEnabled }));
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