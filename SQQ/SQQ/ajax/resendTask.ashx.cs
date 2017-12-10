using Newtonsoft.Json;
using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 管理页重派
    /// </summary>
    public class resendTask : IHttpHandler
    {
        public class Ret
        {
            public string code;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var team_id = context.Request.Form["team_id"];
            Object num = context.Request.Form["num"];

            if (team_id == null || num == null)
            {
                Sys.Log("resendTask.ashx: unusual request(null para).");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                return;
            }
            num = int.Parse(num.ToString());

            try
            {
                var task = Database.GetProblemSent(team_id, (int)num);
                if (Database.AddProblemSaved(task.team_id, task.num))
                {
                    if (Database.DeleteProblemSent(task.team_id, task.num))
                    {
                        Sys.Log("管理员要求重新配送 " + task.team_id + " " + (char)((int)num + (int)'A') + "题");
                        context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
                    }
                    else
                    {
                        Database.DeleteProblemSaved(task.team_id, task.num);
                        Sys.Error("resendTask.ashx: AddProblemSaved() returned false.");
                        context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                    }
                }
                else
                {
                    Sys.Error("resendTask.ashx: AddProblemSaved() returned false.");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("resendTask.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
            }
            catch (Exception ex)
            {
                Sys.Error("resendTask.ashx: Unexpected error.[" + ex.Message + "]");
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