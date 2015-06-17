using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
	/// <summary>
	/// 活动
	/// </summary>
    public class ActivityController : BasicController
    {
        
        public ActionResult Index()
        {
            return View();
        }

    }
}
