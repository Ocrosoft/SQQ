using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// senders 的摘要说明
    /// </summary>
    public class senders : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                var senders = Database.GetsSender();
                context.Response.Write(JsonConvert.SerializeObject(senders));
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("senders.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new List<string>()));
            }
            catch (Exception ex)
            {
                Sys.Error("senders.ashx: Unexpected error.[" + ex.Message + "]");
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