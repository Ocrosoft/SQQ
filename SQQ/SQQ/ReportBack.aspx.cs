using System;

namespace SQQ
{
    public partial class ReportBack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var open_id = Request.QueryString["oid"];

                var taskSender = Database.GetSender(open_id);
                var tasks = Database.GetsProblemSending(open_id);
                var sents = Database.GetsProblemSent(open_id);
                int rowIndex = 0;

                for (int i = 0; i < tasks.Count; i++)
                {
                    tbody.InnerHtml += "<tr align=\"center\" class=\"" + (rowIndex++ % 2 == 0 ? "evenrow" : "oddrow") + "\">";
                    tbody.InnerHtml += "<td>" + taskSender.name + "</td>";
                    tbody.InnerHtml += "<td>" + tasks[i].team_id + " - " + (char)(tasks[i].num + 'A') + "</td>";
                    tbody.InnerHtml += "<td><input type=\"button\" class=\"form-control\" value=\"完成\" data-detail=\"" + tasks[i].team_id + "#" + tasks[i].num + "\"/></td>";
                    tbody.InnerHtml += "</tr>";
                }
                for (int i = 0; i < sents.Count; i++)
                {
                    tbody.InnerHtml += "<tr align=\"center\" class=\"" + (rowIndex++ % 2 == 0 ? "evenrow" : "oddrow") + "\">";
                    tbody.InnerHtml += "<td>" + taskSender.name + "</td>";
                    tbody.InnerHtml += "<td>" + sents[i].team_id + " - " + (char)(sents[i].num + 'A') + "</td>";
                    tbody.InnerHtml += "<td>" + sents[i].time.ToString("HH:mm:ss") + "</td>";
                    tbody.InnerHtml += "</tr>";
                }
            }
            catch(MySql.Data.MySqlClient.MySqlException ex)
            {
                Sys.Error("ReportBack.aspx: SQL Error.[" + ex.Message + "]");
                Response.Clear();
                Response.Write("出现未知错误，请联系管理员。");
            }
            catch(IndexOutOfRangeException)
            {
                Sys.Log("ReportBack.aspx: 非正常访问，不存在该配送人员");
                Response.Redirect("https://www.baidu.com");
            }
        }
    }
}