using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EastRiverCommune.Extension
{
    public class BaiduApi
    {

        public string ApiKey { get; set; }

        public System.Net.HttpWebRequest CreateHttpWebRequest(string url)
        {
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Headers.Add("apikey", this.ApiKey);
            return request;
        }

        public System.Net.HttpWebRequest CreateHttpWebRequest(string url, string method)
        {
            var request = this.CreateHttpWebRequest(url);
            request.Method = method;
            return request;
        }

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        public string SendHttpGetRequest(string url, string param)
        {
            url = url + '?' + param;
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", this.ApiKey);
            var response = (System.Net.HttpWebResponse)request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        public string Phone(string phone)
        {
            string url = "http://apis.baidu.com/apistore/mobilephoneservice/mobilephone";
            string param = "tel=" + phone;
            return this.SendHttpGetRequest(url, param);
        }

    }
}