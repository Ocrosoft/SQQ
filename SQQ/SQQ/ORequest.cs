using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Ocrosoft
{
    /// <summary>
    /// 接口请求功能类
    /// </summary>
    public class ORequest
    {
        /// <summary>
        /// 发起GET请求，返回响应文本
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="queryString">GET参数</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static string RequestGetRaw(string url, Dictionary<string, string> queryString = null, WebHeaderCollection headers = null)
        {
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;

            if (queryString != null)
            {
                foreach (var para in queryString)
                {
                    if (url.IndexOf('?') == -1)
                    {
                        url += "?" + para.Key + "=" + para.Value;
                    }
                    else
                    {
                        url += "&" + para.Key + "=" + para.Value;
                    }
                }
            }
            request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("Authorization", "Basic YWRtaW46YWRtaW4=");
            if (headers != null)
            {
                request.Headers = headers;
            }

            response = request.GetResponse() as HttpWebResponse;
            instream = response.GetResponseStream();
            sr = new StreamReader(instream, Encoding.UTF8);
            string content = sr.ReadToEnd();

            return content;
        }
        /// <summary>
        /// 发起GET请求，返回JSON反序列化对象
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="queryString">GET参数</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static JObject RequestGet(string url, Dictionary<string, string> queryString = null, WebHeaderCollection headers = null)
        {
            var content = RequestGetRaw(url, queryString, headers);
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(content);
            return jsonObject;
        }
        // <summary>
        /// 发起GET请求，返回JSON对象的某一键值。（多用于返回错误代码）
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="queryString">GET参数</param>
        /// <param name="key">要返回值的JSON键</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static string RequestGet(string url, string key, Dictionary<string, string> queryString = null, WebHeaderCollection headers = null)
        {
            JObject jsonObject = RequestGet(url, queryString, headers);
            return jsonObject[key].ToString();
        }
        /// <summary>
        /// 发起POST请求
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="postData">POST内容</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static string RequestPostRaw(string url, string postData, WebHeaderCollection headers = null)
        {
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;

            request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.Headers.Add("Authorization", "Basic YWRtaW46YWRtaW4=");
            if (headers != null) request.Headers = headers;
            var data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            response = request.GetResponse() as HttpWebResponse;
            instream = response.GetResponseStream();
            sr = new StreamReader(instream, Encoding.UTF8);
            string content = sr.ReadToEnd();

            return content;
        }
        /// <summary>
        /// 发起POST请求，返回JSON反序列化对象
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="postData">POST内容</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static JObject RequestPost(string url, string postData, WebHeaderCollection headers = null)
        {
            var content = RequestPostRaw(url, postData, headers);
            JObject jsonObject = (JObject)JsonConvert.DeserializeObject(content);
            return jsonObject;
        }
        /// <summary>
        /// 发起POST请求，返回JSON对象的某一键值。（多用于返回错误代码）
        /// </summary>
        /// <param name="url">请求链接</param>
        /// <param name="postData">POST内容</param>
        /// <param name="key">要返回值的JSON键</param>
        /// <param name="headers">请求头信息</param>
        /// <returns></returns>
        public static string RequestPost(string url, string postData, string key, WebHeaderCollection headers = null)
        {
            JObject jsonObject = RequestPost(url, postData, headers);
            return jsonObject[key].ToString();
        }
    }
}