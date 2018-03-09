using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// getColor 的摘要说明
    /// </summary>
    public class getColor : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                var senders = Database.GetsColor();
                context.Response.Write(JsonConvert.SerializeObject(senders));
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("getFloor.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new List<string>()));
            }
            catch (Exception ex)
            {
                Sys.Error("getFloor.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new List<string>()));
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