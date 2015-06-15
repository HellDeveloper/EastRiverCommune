using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
	/// <summary> 类别
	/// </summary>
	public class CategoryController : BasicController
    {
        /// <summary> 列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
			var list = this.DatabaseContext.GetCategories(true);
			return this.View(list.ToList());
        }

    }
}
