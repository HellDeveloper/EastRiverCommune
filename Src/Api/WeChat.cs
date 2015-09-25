using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Api
{
    public class WeChat
    {
        public WeChat(Controller controller)
        {
            this._controller = controller;
        }

        public WeChat(Controller controller, string appid)
        {
            this._controller = controller;
            this.AppID = appid;
        }

        private Controller _controller;

        public string AppID
        {
            get;
            set;
        }

        /// <summary>
        /// {0}: appid
        /// {1}: redirect_uri
        /// {2}: state
        /// </summary>
        public static string OAuthFormat
        {
            get { return "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=SCOPE&state={2}#wechat_redirect";}
        }

        public void GetOAuthCode(string redirect_uri)
        {
            var uri = this._controller.Request.Url.Authority + this._controller.Url.Content("~/WeChat/RedirectToWeChatOAuth");
            redirect_uri = HttpUtility.UrlEncode(redirect_uri);
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=SCOPE&state={2}#wechat_redirect";
            url = String.Format(url, this.AppID, uri, redirect_uri);
            this._controller.Response.Redirect(uri + "/" + url);
            //var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
            //var response = (System.Net.HttpWebResponse)request.GetResponse();
            //using (var stream = response.GetResponseStream())
            //{
            //    var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8);
            //    return reader.ReadToEnd();
            //}
        }

        public static string WeChatOpenIDSessionKey
        {
            get { return "WeChatOpenID"; }
        }

        public static string WeChatUserIDSessionKey
        {
            get { return "WeChatUserID"; }
        }

        public string GetOpenID()
        {
            string open_id = this._controller.Session[WeChatOpenIDSessionKey] as string;
            if (String.IsNullOrWhiteSpace(open_id))
            {
                this.GetOAuthCode(this._controller.Request.RawUrl);
            }
            return open_id;
        }

        public string UserID
        {
            get
            {
                return String.Empty;
            }
        }

    }
}