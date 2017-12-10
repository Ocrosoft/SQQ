using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;

namespace SQQ
{
    public class WXRequest : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        /// <summary>
        /// 进行微信消息处理
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            String postString = String.Empty;
            // POST请求为消息处理
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }

                if (!string.IsNullOrEmpty(postString))
                {
                    Execute(postString);
                }
            }
            // GET请求为签名验证
            else
            {
                string token = WXManage.token;

                string echoString = HttpContext.Current.Request.QueryString["echoStr"];
                string signature = HttpContext.Current.Request.QueryString["signature"];
                string timestamp = HttpContext.Current.Request.QueryString["timestamp"];
                string nonce = HttpContext.Current.Request.QueryString["nonce"];

                if (CheckSignature(token, signature, timestamp, nonce))
                {
                    if (!string.IsNullOrEmpty(echoString))
                    {
                        HttpContext.Current.Response.Write(echoString);
                        HttpContext.Current.Response.End();
                    }
                }
            }
        }
        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="token"></param>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            string[] ArrTmp = { token, timestamp, nonce };

            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            if (tmpStr == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 详细消息处理
        /// </summary>
        /// <param name="postString"></param>
        public void Execute(string postString)
        {
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            // 获取ToUserName,FromUserName,CreateTime,MsgType
            var rec = WXManage.FromXML<XMLObject>(postString);
            if (rec.MsgType == "event")
            {
                // 事件类型处理
                // 仅处理关注
                var subEV = WXManage.FromXML<SubscribeXMLObject>(postString);
                if (subEV.Event == "subscribe")
                {
                    var res = new TextMessageXMLObject()
                    {
                        FromUserName = rec.ToUserName,
                        ToUserName = rec.FromUserName,
                        MsgType = "text",
                        Content = "欢迎来到送气球的大家庭中，回复\"sqq 姓名\"(不含引号，中间空格隔开)进行登记哦~"
                    };
                    HttpContext.Current.Response.Write(res.ToXML());
                }
            }
            else if (rec.MsgType == "text")
            {
                // 文本类型处理
                var textMSG = WXManage.FromXML<TextMessageXMLObject>(postString);
                if (!textMSG.Content.StartsWith("sqq"))
                {
                    TextMessageXMLObject res = new TextMessageXMLObject()
                    {
                        FromUserName = rec.ToUserName,
                        ToUserName = rec.FromUserName,
                        MsgType = "text",
                        Content = "有什么问题可以找技术支持周甄陶提问哦！"
                    };
                    HttpContext.Current.Response.Write(res.ToXML());
                }
                // 注册送气球
                else if (textMSG.Content.StartsWith("sqq"))
                {
                    var name = textMSG.Content.Split(' ')[1];
                    ImageTextXMLObject res = new ImageTextXMLObject()
                    {
                        FromUserName = rec.ToUserName,
                        ToUserName = rec.FromUserName,
                        MsgType = "news",
                        ArticleCount = 1,
                        Articles = new System.Collections.Generic.List<ImageTextXMLObject.item>()
                    };
                    TextMessageXMLObject tres = new TextMessageXMLObject()
                    {
                        FromUserName = rec.ToUserName,
                        ToUserName = rec.FromUserName,
                        MsgType = "text"
                    };
                    bool failed = false;
                    if (Sys.SignEnabled)
                    {
                        try
                        {
                            if (Database.AddSender(rec.FromUserName, name))
                            {
                                res.Articles.Add(new ImageTextXMLObject.item()
                                {
                                    Title = "登记成功，在这里查看和提交任务哦！",
                                    Description = "查看和提交任务遇到问题，请联系管理员。",
                                    PicUrl = "..",
                                    Url = "http://debug.ocrosoft.com/ReportBack.aspx?oid=" + rec.FromUserName
                                });
                                Sys.Log(name + "进行了登记");
                            }
                            else
                            {
                                failed = true;
                                tres.Content = "登记失败，如果已经登记则不能再次登记。";
                            }
                        }
                        catch(Exception)
                        {
                            failed = true;
                            tres.Content = "登记失败，如果已经登记则不能再次登记。";
                        }
                    }
                    else
                    {
                        failed = true;
                        tres.Content = "登记未开启，如错过登记请联系管理员。";
                    }
                    if (failed)
                    {
                        HttpContext.Current.Response.Write(tres.ToXML());
                    }
                    else
                    {
                        HttpContext.Current.Response.Write(res.ToXML());
                    }
                }
            }
        }
    }
}