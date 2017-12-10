using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// problemsSavedCount 的摘要说明
    /// </summary>
    public class problemsSavedCount : IHttpHandler
    {
        public class Ret
        {
            public int count;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { count = Database.GetsProblemSaved().Count }));
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("problemsSavedCount.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { count = -1 }));
            }
            catch (Exception ex)
            {
                Sys.Error("problemsSavedCount.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { count = -1 }));
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