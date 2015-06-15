using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
	public class MainController : BasicController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cache()
        {
            this.Response.AddHeader("MIME-type", "text/cache-manifest");
            return this.View();
        }

    }
}
