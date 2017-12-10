using Newtonsoft.Json;
using System;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 删除送气球人员
    /// </summary>
    public class deleteSender : IHttpHandler
    {
        public class Ret
        {
            public string code;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var open_id = context.Request.QueryString["oid"];
            if(open_id == null)
            {
                Sys.Log("deleteSender.ashx: 非正常访问(参数不正确)");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                return;
            }
            try
            {
                var sender = Database.GetSender(open_id);
                var sendings = Database.GetsProblemSending(open_id);
                foreach(var sending in sendings)
                {
                    if(Database.AddProblemSaved(sending.team_id,sending.num))
                    {
                        if(Database.DeleteProblemSending(sending.team_id,sending.num))
                        {
                            //
                        }
                        else
                        {
                            Database.DeleteProblemSaved(sending.team_id, sending.num);
                            Sys.Error("deleteSender.ashx: DeleteProblemSending() returned false.");
                            context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                        }
                    }
                    else
                    {
                        Sys.Error("deleteSender.ashx: AddProblemSaved() returned false.");
                        context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                    }
                }
                if(Database.DeleteSender(open_id))
                {
                    Sys.Log("删除了配送人员：" + sender.name);
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
                }
                else
                {
                    Sys.Error("deleteSender.ashx: DeleteSender() returned false.");
                    context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("deleteSender.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "ok" }));
            }
            catch(IndexOutOfRangeException)
            {
                Sys.Error("deleteSender.ashx: No such sender.");
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { code = "error" }));
            }
            catch (Exception ex)
            {
                Sys.Error("deleteSender.ashx: Unexpected error.[" + ex.Message + "]");
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