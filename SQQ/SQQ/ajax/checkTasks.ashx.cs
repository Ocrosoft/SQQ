using Newtonsoft.Json;
using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 验证任务是否10分钟未完成
    /// </summary>
    public class checkTasks : IHttpHandler
    {
        public class Ret
        {
            public string code;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (Sys.Checking)
            {
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
                return;
            }
            Sys.Checking = true;
            var tasks = Database.GetsProblemSending();
            // task's start time can't earlier than this.
            var forwardTime = DateTime.Now.AddMinutes(-10);
            foreach(var task in tasks)
            {
                if(task.timeSent<=forwardTime)
                {
                    try
                    {
                        if(Database.AddProblemSaved(task.team_id, task.num))
                        {
                            if(Database.DeleteProblemSending(task.team_id,task.num))
                            {
                                Sys.Log("回收了一个超时任务：" + task.team_id + " " + (char)(task.num + (int)'A') + "题，本应由 " +
                                    Database.GetSender(task.sender).name + " 配送");
                            }
                            else
                            {
                                Sys.Error("checkTasks.ashx: DeleteProblemSending() returned false.");
                                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                            }
                        }
                        else
                        {
                            Sys.Error("checkTasks.ashx: AddProblemSaved() returned false.");
                            context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                        }
                    }
                    catch(MySql.Data.MySqlClient.MySqlException ex)
                    {
                        Sys.Error("checkTasks.ashx: SQL Error:[" + ex.Message + "]");
                        context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                    }
                    catch(IndexOutOfRangeException)
                    {
                        Sys.Error("checkTasks.ashx: No such sender.");
                        context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                    }
                    catch(Exception ex)
                    {
                        Sys.Error("checkTasks.ashx: Unexpected error.[" + ex.Message + "]");
                        context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                    }
                }
            }
            Sys.Checking = false;
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