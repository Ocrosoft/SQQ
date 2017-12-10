using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// sents 的摘要说明
    /// </summary>
    public class sents : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            try
            {
                var sents = Database.GetsProblemSent();
                for (int i = 0; i < sents.Count; i++)
                {
                    // change open_id to name
                    if (string.IsNullOrEmpty(sents[i].sender))
                    {
                        sents[i].sender = "已开除";
                    }
                    else
                    {
                        sents[i].sender = Database.GetSender(sents[i].sender).name;
                    }
                }
                context.Response.Write(JsonConvert.SerializeObject(sents));
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("sents.ashx: SQL Error.[" + ex.Message + "]");
                context.Response.Write(JsonConvert.SerializeObject(new List<string>()));
            }
            catch(IndexOutOfRangeException)
            {
                Sys.Error("sents.ashx: No such sender.");
                context.Response.Write(JsonConvert.SerializeObject(new List<string>()));
            }
            catch (Exception ex)
            {
                Sys.Error("sents.ashx: Unexpected error.[" + ex.Message + "]");
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