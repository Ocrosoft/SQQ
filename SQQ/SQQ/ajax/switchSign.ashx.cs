using Newtonsoft.Json;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// switchSign 的摘要说明
    /// </summary>
    public class switchSign : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Sys.SignEnabled = !Sys.SignEnabled;
            Sys.Log((Sys.SignEnabled ? "开启" : "关闭") + "了配送人员登记");
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