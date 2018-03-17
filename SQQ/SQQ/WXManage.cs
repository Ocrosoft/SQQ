using Ocrosoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Serialization;

namespace SQQ
{
    /// <summary>
    /// 微信接口管理
    /// </summary>
    public class WXManage
    {
        // 公众号微信号
        private static string gh = "";
        // 公众号appID
        public static string appID = "";
        // 公众号appsecret
        private static string appsecret = "";
        // access_token
        private static string access_token = string.Empty;
        // jsapi_ticket，一般与access_token同时获取和刷新
        private static string jsapi_ticket = string.Empty;
        // access_token下次要刷新的时间
        private static long timeStamp;
        // 公众号设置的token
        public static string token = "";

        /// <summary>
        /// 刷新access_token
        /// </summary>
        /// <returns></returns>
        public static string RefreshAccessToken()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential";
            var jsonObject = ORequest.RequestGet(url, new System.Collections.Generic.Dictionary<string, string>
            {
                { "appid",appID },
                {"secret",appsecret }
            });
            // 获取access_token，计算过期时间
            access_token = jsonObject["access_token"].ToString();
            timeStamp = OSecurity.DateTimeToTimeStamp(DateTime.Now) +
                Convert.ToInt64(jsonObject["expires_in"].ToString());
            RefreshJsapiTicket();
            return access_token;
        }
        /// <summary>
        /// 只能由RefreshAccessToken()调用
        /// </summary>
        /// <returns></returns>
        private static string RefreshJsapiTicket()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket";
            var jsonObject = ORequest.RequestGet(url, new System.Collections.Generic.Dictionary<string, string>
            {
                { "access_token",GetAccessToken() },
                {"type","jsapi" }
            });
            // 获取ticket
            jsapi_ticket = jsonObject["ticket"].ToString();
            return jsapi_ticket;
        }
        /// <summary>
        /// 获取access_token，每次调用接口都调用此函数，不要记录
        /// </summary>
        /// <returns></returns>
        public static String GetAccessToken()
        {
            if (access_token == String.Empty ||
                OSecurity.DateTimeToTimeStamp(DateTime.Now) > timeStamp)
            {
                return RefreshAccessToken();
            }
            return access_token;
        }
        /// <summary>
        /// 获取jsapi_ticket，每次调用接口都调用此函数，不要记录
        /// </summary>
        /// <returns></returns>
        public static string GetJsapiTicket()
        {
            if (access_token == String.Empty ||
                OSecurity.DateTimeToTimeStamp(DateTime.Now) > timeStamp)
            {
                RefreshAccessToken();
            }
            return jsapi_ticket;
        }
        /// <summary>
        /// 获取二维码地址
        /// </summary>
        /// <param name="content">编码二维码的内容，网址必须http(s)开头</param>
        /// <returns></returns>
        public static string QRCode(string content)
        {
            String api = "http://qr.liantu.com/api.php?text=";
            api += HttpUtility.UrlEncode(content);
            return api;
        }
        /// <summary>
        /// XML反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXML<T>(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(xml);
            T obj = (T)xs.Deserialize(sr);
            sr.Close();
            sr.Dispose();
            return obj;
        }
        /// <summary>
        /// XML序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ToXML<T>(T t)
        {
            XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
            xsn.Add(string.Empty, string.Empty);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringWriter sw = new StringWriter();
            xs.Serialize(sw, t, xsn);
            string str = sw.ToString();
            sw.Close();
            sw.Dispose();
            return str;
        }
        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="OPENID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool SendMessage(string OPENID, string content)
        {
            var jsonStr = "{\"touser\":\"" + OPENID + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + content + "\"}}";
            var errcode = ORequest.RequestPost("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + GetAccessToken(),
                jsonStr,
                "errcode");
            if (errcode == "0")
            {
                return true;
            }

            return false;
        }

        public static string WXJSSign(string nonce, Int64 timestamp, string url)
        {
            string str = "jsapi_ticket=" + GetJsapiTicket() +
                "&noncestr=" + nonce +
                "&timestamp=" + timestamp.ToString() +
                "&url=" + url;
            str = OSecurity.SHA1(str);
            return str;
        }
    }

    /// <summary>
    /// 消息基类
    /// </summary>
    [XmlRoot(ElementName = "xml")]
    public class XMLObject
    {
        public XMLObject()
        {
            CreateTime = OSecurity.DateTimeToTimeStamp(DateTime.Now);
        }
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public Int64 CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 转换为XML，子类必须重写(抄一遍实现，不然调用此方法报错，typeof会取到基类)
        /// </summary>
        /// <returns></returns>
        public virtual string ToXML()
        {
            CreateTime = OSecurity.DateTimeToTimeStamp(DateTime.Now);
            return WXManage.ToXML(this);
        }
    }
    /// <summary>
    /// 文本消息，MsgType=text
    /// </summary>
    [XmlRoot(ElementName = "xml")]
    public class TextMessageXMLObject : XMLObject
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 消息id
        /// </summary>
        public string MsgId { get; set; }
        public override string ToXML()
        {
            CreateTime = OSecurity.DateTimeToTimeStamp(DateTime.Now);
            return WXManage.ToXML(this);
        }
    }
    /// <summary>
    /// 订阅事件，MsgType=event
    /// </summary>
    [XmlRoot(ElementName = "xml")]
    public class SubscribeXMLObject : XMLObject
    {
        /// <summary>
        /// 事件类型，subscribe、unsubscribe
        /// </summary>
        public string Event { get; set; }
        public override string ToXML()
        {
            CreateTime = OSecurity.DateTimeToTimeStamp(DateTime.Now);
            return WXManage.ToXML(this);
        }
    }
    /// <summary>
    /// 图文消息（外链消息），MsgType=news
    /// </summary>
    [XmlRoot(ElementName = "xml")]
    public class ImageTextXMLObject : XMLObject
    {
        public int ArticleCount { get; set; }
        public List<item> Articles { get; set; }
        public override string ToXML()
        {
            CreateTime = OSecurity.DateTimeToTimeStamp(DateTime.Now);
            return WXManage.ToXML(this);
        }

        public class item
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string PicUrl { get; set; }
            public string Url { get; set; }
        }
    }
}