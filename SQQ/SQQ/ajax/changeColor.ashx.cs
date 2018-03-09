using Newtonsoft.Json;
using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// changeColor 的摘要说明
    /// </summary>
    public class changeColor : IHttpHandler
    {
        public class Ret
        {
            public string code;
            public string info;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string numChar = context.Request.Form["num"].ToString();
            if (!('A' <= numChar[0] && numChar[0] <= 'Z')) 
            {
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error", info = "要求nun是大写字母" }));
                return;
            }
            int num = numChar[0] - 'A';
            string color = context.Request.Form["color"].ToString();
            string op = context.Request.Form["op"].ToString();

            if (op == "1")
            {
                try
                {
                    Database.DeleteColor(num);
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    Sys.Error("changeFloor.ashx: SQL Error.[" + ex.Message + "]");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                }
                catch (Exception ex)
                {
                    Sys.Error("changeFloor.ashx: Unexpected error.[" + ex.Message + "]");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                }
            }
            else if (op == "0")
            {
                try
                {
                    Database.AddColor(num, color);
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    Sys.Error("changeFloor.ashx: SQL Error.[" + ex.Message + "]");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                }
                catch (Exception ex)
                {
                    Sys.Error("changeFloor.ashx: Unexpected error.[" + ex.Message + "]");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                }
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