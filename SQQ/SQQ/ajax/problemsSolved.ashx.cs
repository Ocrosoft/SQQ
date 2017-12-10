using System;
using System.Collections.Generic;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// problemsSolved 的摘要说明
    /// </summary>
    public class problemsSolved : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Database.GetsProblemSolved()));
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("problemsSolved.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new List<string>()));
            }
            catch (Exception ex)
            {
                Sys.Error("initation.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new List<string>()));
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