using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SQQ
{
    /// <summary>
    /// processProblems 的摘要说明
    /// </summary>
    public class processProblems : IHttpHandler
    {
        public class Ret
        {
            public bool error;
            public List<Database.ProblemSolved> newSolved;
        }
        public class Tmp
        {
            public string team_id;
            public string num;
        }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if(Sys.ContestID == 0 || Sys.ProcessingProblems) // 未设置比赛 或 已经在处理
            {
                context.Response.Write(JsonConvert.SerializeObject(new Ret() { error = false, newSolved = new List<Database.ProblemSolved>() }));
                return;
            }

            Sys.ProcessingProblems = true; // 表示正在处理
            var json = GetUrltoHtml("http://.../JudgeOnline/contestsolved_ajax.php?cid=" + Sys.ContestID);
            json = json.Replace("[\"", "[\"team_id\":\"").Replace(",\"", ",\"num\":\"").Replace("[\"", "{\"").Replace("\"]", "\"}");
            var tmp = JsonConvert.DeserializeObject<List<Tmp>>(json);
            List<Database.ProblemSolved> problems = new List<Database.ProblemSolved>();
            var ret = new Ret()
            {
                error = false,
                newSolved = null
            };
            try
            {
                foreach (var each in tmp)
                {
                    problems.Add(new Database.ProblemSolved()
                    {
                        team_id = each.team_id,
                        num = int.Parse(each.num)
                    });
                }

                var problemsSolved = Database.GetsProblemSolved();
                var newSolved = problems.Except(problemsSolved, new Database.ProblemSolved()).ToList(); // 差集
                ret.newSolved = newSolved;
                foreach (var solved in newSolved)
                {
                    if (!Database.AddProblemSaved(solved.team_id, solved.num))
                    {
                        ret.error = true;
                        ret.newSolved = null;
                    }
                }

                if (newSolved.Count > 0)
                {
                    Sys.Log("获取到" + newSolved.Count + "道新过题");
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("processProblems.ashx: SQL Error.[" + ex.Message + "]");
                ret.error = true;
                ret.newSolved = null;
            }
            catch (Exception ex)
            {
                Sys.Error("processProblems.ashx: Unexpected error.[" + ex.Message + "]");
                ret.error = true;
                ret.newSolved = null;
            }

            Sys.ProcessingProblems = false; // 表示处理完成
            context.Response.Write(JsonConvert.SerializeObject(ret));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public static string GetUrltoHtml(string Url)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                //
            }
            return "";
        }
    }
}