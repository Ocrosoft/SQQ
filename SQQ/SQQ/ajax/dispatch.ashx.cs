using System;
using System.Collections.Generic;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// 调用即进行分配
    /// </summary>
    public class Dispatch : IHttpHandler
    {
        public class Ret
        {
            public int count;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            var ret = new Ret();

            if (!Sys.DispatchEnabled || Sys.Dispatching) // 分配未开启或正在进行分配，返回0
            {
                ret.count = 0;
            }
            else
            {
                try
                {
                    Sys.Dispatching = true; // 表示正在进行任务分配，避免其他其他线程同时进行分配
                    var result = ShellDispatch.execute(Sys.MaxTask);

                    if ((bool)result["error"]) // 错误
                    {
                        ret.count = -1;
                        Sys.Error("dispatch.ashx: Dispatch.execute() returned false.");
                    }
                    else // 成功
                    {
                        ret.count = ((List<string>)result["sendingList"]).Count;
                        if (ret.count > 0)
                        {
                            Sys.Log("分配了" + ret.count + "个任务");
                        }
                        foreach (var each in (List<string>)result["sendingList"])
                        {
                            Sys.Log(each);
                        }
                    }
                }
                catch(MySql.Data.MySqlClient.MySqlException ex)
                {
                    ret.count = -1;
                    Sys.Error("dispatch.ashx: SQL Error.[" + ex.Message + "]");
                }
                catch (Exception ex)
                {
                    Sys.Error("dispatch.ashx: Unexpected error.[" + ex.Message + "]");
                    ret.count = -1;
                }
                Sys.Dispatching = false; // 表示分配结束，其他进程可以继续分配

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