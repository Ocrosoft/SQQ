using System;

namespace SQQ
{
    public partial class ShowSender : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Database.GetsProblemSolved();
        }
    }
}