﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult BaiduMap()
        {
            return View();
        }

		public ActionResult QRCode()
		{
			return View();
		}

        public ActionResult ShowParams()
        {
			
            return this.View();
        }


    }
}
