using EastRiverCommune.Entities;
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
        /// <summary> 商品类型
        /// </summary>
        /// <returns></returns>
        public ActionResult Commodity()
        {
			var list = this.DatabaseContext.GetCategories(CategoryType.Commodity, true);
			return this.View(list.ToList());
        }

    }
}
