using EastRiverCommune.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Api
{
    public class WeChat
    {

		/// <summary>
		/// {0}: appid
		/// {1}: redirect_uri
		/// {2}: response_type
		/// {3}: scope
		/// {4}: state
		/// </summary>
		public static string PublicOAuthUrlFormat
		{
			get { return "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type={2}&scope={3}&state={4}#wechat_redirect"; }
		}


		public static string OpenIDSessionKey
		{
			get { return "WeChatOpenID"; }
		}

		public static string UserIDSessionKey
		{
			get { return "WeChatUserID"; }
		}

		public WeChat(Controller controller)
        {
			this._controller = controller;
        }

		public WeChat(Controller controller, string appid)
        {
			this._controller = controller;
            this.AppID = appid;
        }

		public WeChat(Controller controller, string appid, string secret)
		{
			this._controller = controller;
			this.AppID = appid;
			this.Secret = secret;
		}

        private Controller _controller;

		public string AppID { get; set; }

		public string Secret { get; set; }

		/// <summary> 从 controller session 
		/// </summary>
		public string OpenID
		{
			get { return this._controller.Session[OpenIDSessionKey] as string; }
			set { this._controller.Session[OpenIDSessionKey] = value; }
		}


		public string GetPublicOAuthUrl(string redirect_uri, string response_type, string scope, string state)
		{
			redirect_uri = HttpUtility.UrlEncode(redirect_uri);
			return String.Format(PublicOAuthUrlFormat, this.AppID, redirect_uri, response_type, scope, state);
		}

		/// <summary> 简版 response_type=code, scope=snsapi_base
		/// </summary>
		/// <param name="redirect_uri"></param>
		/// <param name="state"></param>
		/// <returns></returns>
		public string GetPublicOAuthUrl(string redirect_uri, string state)
		{
			return this.GetPublicOAuthUrl(redirect_uri, "code", "snsapi_base", state);
		}

		public string GetOpenID(string code)
		{
			var result = String.Empty;
			string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
			url = String.Format(url, this.AppID, this.Secret, code);
			var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
			var response = (System.Net.HttpWebResponse)request.GetResponse();
			using (var stream = response.GetResponseStream())
			{
				var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
				result = reader.ReadToEnd();
			}
			System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\log\WeChat.txt");
			sw.Write(result);
			sw.Flush();
			sw.Close();
			var json = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
			object openid;
			if (json.TryGetValue("openid", out openid))
				return openid.ToString();
			return null;
		}

    }
}