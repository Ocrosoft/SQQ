using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 获取题目的保存数量、派送中数量、已送数量
    /// </summary>
    public class problemsCountInfo : IHttpHandler
    {
        public class Info
        {
            public int savedCount;
            public int sendingCount;
            public int sentCount;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                var info = new Info()
                {
                    savedCount = Database.GetsProblemSaved().Count,
                    sendingCount = Database.GetsProblemSending().Count,
                    sentCount = Database.GetsProblemSent().Count()
                };
                context.Response.Write(JsonConvert.SerializeObject(info));
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("problemsCountInfo.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new Info() { savedCount = -1, sendingCount = -1, sentCount = -1 }));
            }
            catch (Exception ex)
            {
                Sys.Error("problemsCountInfo.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new Info() { savedCount = -1, sendingCount = -1, sentCount = -1 }));
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