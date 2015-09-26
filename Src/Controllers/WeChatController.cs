using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
    public class WeChatController : BasicController
    {

        public ActionResult RedirectToWeChatOAuth(string id)
        {
		 
            return this.View(id);
        }

    }
}
