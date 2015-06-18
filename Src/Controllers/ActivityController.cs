using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
	/// <summary> 活动
	/// </summary>
    public class ActivityController : BasicController
    {
        /// <summary> 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            return View();
        }

    }
}
