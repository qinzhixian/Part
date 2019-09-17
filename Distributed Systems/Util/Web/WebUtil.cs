using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Util.Web
{
    /// <summary>
    /// Http帮助类
    /// </summary>
    public static class WebUtil
    {
        public static string GetResponseParame(System.Web.HttpRequestBase request)
        {
            if (request == null)
                return string.Empty;

            string parame = string.Empty;

            string method = request.HttpMethod;

            if (method == "GET")
            {
                parame = request.Url.Query;
            }
            else if (method == "POST")
            {
                parame = request.Form.ToString();
            }

            return UrlDeCode(parame);
        }

        public static string GetRequestUrl(System.Web.HttpRequestBase request)
        {
            if (request == null)
                return string.Empty;

            return UrlDeCode(request.Url.ToString());
        }

        public static System.Web.HttpContextBase TransToBase(this System.Web.HttpContext context)
        {
            return new System.Web.HttpContextWrapper(context);
        }

        public static System.Web.HttpRequestBase TransToBase(this System.Web.HttpRequest context)
        {
            return new System.Web.HttpRequestWrapper(context);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        /// <returns></returns>
        public static string UrlDeCode(string str)
        {
            return UrlDeCode(str, Encoding.UTF8);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="str">要解码的字符串</param>
        /// <param name="encoding">解码的编码格式</param>
        /// <returns></returns>
        public static string UrlDeCode(string str, Encoding encoding)
        {
            return HttpUtility.UrlDecode(str, encoding);
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="str">要编码的Url字符串</param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            return UrlEncode(str, Encoding.UTF8);
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="str">要编码的Url字符串</param>
        /// <param name="encoding">编码的格式</param>
        /// <returns></returns>
        public static string UrlEncode(string str, Encoding encoding)
        {
            return HttpUtility.UrlEncode(str, encoding);
        }



        /// <summary>
        /// 获取网络数据(Method:Get)
        /// </summary>
        /// <param name="url">网络数据地址</param>
        /// <param name="encoding">数据编码</param>
        /// <param name="headers">请求标头</param>
        /// <returns></returns>
        public static string Get(string url, Encoding encoding = null, NameValueCollection headers = null)
        {
            using (WebClient client = new WebClient())
            {
                if (headers != null)
                {
                    client.Headers.Add(headers);
                }

                client.Encoding = encoding != null ? encoding : Encoding.UTF8;

                return client.Encoding?.GetString(client.DownloadData(url));
            }
        }

        /// <summary>
        /// 获取网络数据(Method:Post)
        /// </summary>
        /// <param name="url">网络数据地址</param>
        /// <param name="formdata">请求携带参数</param>
        /// <param name="encoding">数据编码</param>
        /// <param name="headers">请求标头</param>
        /// <returns></returns>
        public static string Post(string url, NameValueCollection formdata, Encoding encoding = null, NameValueCollection headers = null)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding != null ? encoding : Encoding.UTF8;

                if (headers != null)
                {
                    client.Headers.Add(headers);
                }
                return client.Encoding.GetString(client.UploadValues(url, "POST", formdata));
            }
        }

        /// <summary>
        /// 获取网络数据(Method:Post)
        /// </summary>
        /// <param name="url">网络数据地址</param>
        /// <param name="formdata">请求携带参数</param>
        /// <param name="encoding">数据编码</param>
        /// <param name="headers">请求标头</param>
        /// <returns></returns>
        public static string Post(string url, object data, Encoding encoding = null, NameValueCollection headers = null)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding != null ? encoding : Encoding.UTF8;

                if (headers != null)
                {
                    client.Headers.Add(headers);
                }

                NameValueCollection postData = new NameValueCollection();
                foreach (PropertyInfo pinfo in data.GetType().GetProperties())
                {
                    NameValueCollection item = new NameValueCollection();
                    item.Add(pinfo.Name, pinfo.GetValue(data, null).ToString());
                    postData.Add(item);
                }


                return client.Encoding.GetString(client.UploadValues(url, "POST", postData));
            }
        }

        public static string Send(string method, string url, object sendData)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                byte[] data = client.UploadData(url, method, client.Encoding.GetBytes(UrlEncode(Util.JsonUtil.Serialize(sendData))));
                return client.Encoding.GetString(data);
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">文件所在网络地址</param>
        /// <param name="saveFilePath">保存到本地磁盘地址</param>
        public static void DownloadFile(string url, string saveFilePath)
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, saveFilePath);
            }
        }

        /// <summary>
        /// 上传本地文件到指定的网络地址
        /// </summary>
        /// <param name="url">网络数据地址</param>
        /// <param name="fileFullName">本地文件地址</param>
        /// <param name="headers">请求标头</param>
        /// <param name="completed">上传完成</param>
        public static void UploadFile(string url, string fileFullName, NameValueCollection headers = null, Action<WebClient> completed = null)
        {
            using (WebClient client = new WebClient())
            {
                if (headers != null)
                {
                    client.Headers.Add(headers);
                }
                client.UploadFile(url, fileFullName);

                completed?.Invoke(client);
            }
        }

        /// <summary>
        /// 异步上传文件
        /// </summary>
        /// <param name="url">网络数据地址</param>
        /// <param name="fileFullName">本地文件地址</param>
        /// <param name="headers">请求标头</param>
        /// <param name="progressChanged">上传中</param>
        /// <param name="completed">上传完毕</param>
        public static void UploadFileAsync(string url, string fileFullName, NameValueCollection headers = null, UploadProgressChangedEventHandler progressChanged = null, UploadFileCompletedEventHandler completed = null)
        {
            using (WebClient client = new WebClient())
            {
                if (headers != null)
                {
                    client.Headers.Add(headers);
                }
                if (progressChanged != null)
                {
                    client.UploadProgressChanged += progressChanged;
                }
                if (completed != null)
                {
                    client.UploadFileCompleted += completed;
                }
                client.UploadFileAsync(new Uri(url), "Http", fileFullName);
            }
        }

    }
}
