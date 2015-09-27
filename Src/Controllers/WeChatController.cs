using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace EastRiverCommune.Controllers
{
    public class WeChatController : BasicController
    {
		[ValidateInput(false)]
		public ActionResult OAuth(string redirect_uri, string code, string state)
		{
			this.WeChat.OpenID = this.WeChat.GetOpenID(code);
			return this.Redirect(HttpUtility.UrlDecode(redirect_uri));
		}

		[ValidateInput(false)]
		public ActionResult CommodityConfirmPay(string id, string code, string state)
		{
			this.WeChat.OpenID = this.WeChat.GetOpenID(code);
			//return this.PartialView("~/Views/Test/ShowParams.cshtml");
			return this.Redirect("~/Commodity/ConfirmPay?order_list_id=" + id ?? String.Empty);
		}

		

		

    }
}
