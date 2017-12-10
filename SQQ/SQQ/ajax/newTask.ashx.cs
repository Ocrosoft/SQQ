using System;
using System.Collections.Generic;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 配送人员获取新任务请求页
    /// </summary>
    public class newTask : IHttpHandler
    {
        public class Ret
        {
            public string name;
            public List<Database.ProblemSolved> problems;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                var open_id = context.Request.QueryString["oid"];
                var task = Database.GetsProblemSending(open_id);
                var name = Database.GetSender(open_id).name;
                var ret = new Ret()
                {
                    name = name,
                    problems = task
                };

                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ret));
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("newTask.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { name = "-1", problems = new List<Database.ProblemSolved>() }));
            }
            catch(IndexOutOfRangeException)
            {
                Sys.Log("newTask.ashx: 非正常访问，不存在该配送人员");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { name = "-1", problems = new List<Database.ProblemSolved>() }));
            }
            catch (Exception ex)
            {
                Sys.Error("newTask.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { name = "-1", problems = new List<Database.ProblemSolved>() }));
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