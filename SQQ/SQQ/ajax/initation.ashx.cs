using Newtonsoft.Json;
using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 初始化系统
    /// </summary>
    public class initation : IHttpHandler
    {
        public class Ret
        {
            public string code;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                Sys.ClearLogs();
                Sys.Reset();
                if (Database.Init())
                {
                    Sys.Log("系统初始化成功！");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
                }
                else
                {
                    Sys.Error("initation.ashx: System init error.");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("initation.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
            }
            catch (Exception ex)
            {
                Sys.Error("initation.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
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