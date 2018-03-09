using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 送气球人员反馈配送完成
    /// </summary>
    public class confirmTask : IHttpHandler
    {
        public class Ret
        {
            public string code;
            public string info;
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var team_id = context.Request.Form["tid"];
            Object num = context.Request.Form["num"];
            var open_id = context.Request.Form["oid"];
            // 参数检查
            if (team_id == null || num == null || open_id == null) 
            {
                Sys.Log("confirmTask.ashx: 非正常访问(参数不正确).");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret()
                {
                    code = "err",
                    info = "Infomation not complete."
                }));
                return;
            }
            num = int.Parse(num.ToString());
            Database.Sender sender = null;
            try
            {
                sender = Database.GetSender(open_id);
                if (sender == null)
                {
                    Sys.Log("confirmTask.ashx: 非正常访问(无此配送人员).");
                    context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret()
                    {
                        code = "err",
                        info = "No such sender."
                    }));
                    return;
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("confirmTask.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret()
                {
                    code = "err",
                    info = "SQL Error."
                }));
                return;
            }
            catch(IndexOutOfRangeException)
            {
                Sys.Error("confirmTask.ashx: No such sender.");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret()
                {
                    code = "err",
                    info = "No such sender."
                }));
                return;
            }
            catch (Exception ex)
            {
                Sys.Error("confirmTask.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { code = "error" }));
            }

            var ret = new Ret()
            {
                code = "",
                info = ""
            };

            try
            {
                var problemSending = Database.GetProblemSending(team_id, (int)num);
                if (Database.AddProblemSent(team_id, (int)num, open_id, problemSending.timeStart))
                {
                    if (Database.DeleteProblemSending(team_id, (int)num)) // 成功
                    {
                        sender.timeSpent += (DateTime.Now - problemSending.timeSent).TotalSeconds;
                        ret.code = "ok";
                        ret.info = DateTime.Now.ToString("HH:mm:ss");
                        Sys.Log(team_id + " " + (char)((int)num + 'A') + "题 已经由 " + sender.name + " 配送完成");
                    }
                    else // 删除在送失败
                    {
                        Database.DeleteProblemSent(team_id, (int)num);
                        ret.code = "err";
                        ret.info = "Delete Error.";
                        Sys.Error("confirmTask.ashx: AddProblemSent() success, but DeleteProblemSending() failed(return false)." + "[" + team_id + "," + num + "," + open_id + "]");
                    }
                }
                else // 加入已送失败
                {
                    ret.code = "err";
                    ret.info = "Add Error.";
                    Sys.Error("confirmTask.ashx: AddProblemSent() faild(return false)." + "[" + team_id + "," + num + "," + open_id + "]");
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException ex) // 数据库错误
            {
                ret.code = "err";
                ret.info = "SQL Error.";
                Sys.Error("confirmTask.ashx: SQL error.[" + ex.Message + "]");
            }
            catch(IndexOutOfRangeException)
            {
                ret.code = "err";
                ret.info = "SQL Error.";
                Sys.Error("confirmTask.ashx: No such sender.");
            }
            catch (Exception ex)
            {
                Sys.Error("confirmTask.ashx: Unexpected error.[" + ex.Message + "]");
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new Ret() { code = "error" }));
            }

            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ret));
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