using Newtonsoft.Json;
using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// changeFloor 的摘要说明
    /// </summary>
    public class changeFloor : IHttpHandler
    {
        public class Ret
        {
            public string code;
            public string info;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string team_id = context.Request.Form["teamId"].ToString();
            string floor = context.Request.Form["floor"].ToString();
            string op = context.Request.Form["op"].ToString();

            if (op == "1")
            {
                try
                {
                    Database.DeleteFloor(team_id, floor);
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
            else if (op =="0")
            {
                try
                {
                    Database.AddFloor(team_id, floor);
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