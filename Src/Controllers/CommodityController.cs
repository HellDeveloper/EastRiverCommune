using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Controllers
{
	/// <summary> 商品
	/// </summary>
    public class CommodityController : BasicController
    {
		public static string DIRECTORY_NAME = "Commodity";

        /// <summary> 商店
        /// </summary>
        /// <returns></returns>
		public ActionResult Store()
        {
            return View();
        }

		/// <summary> 列表
		/// </summary>
		/// <returns></returns>
		public ActionResult List()
		{
			var list = this.DatabaseContext.GetCommodities(true);
			return this.View(list.ToList());
		}

		/// <summary> 购物车
		/// </summary>
		/// <returns></returns>
        public ActionResult Defray()
		{
			return this.View();
		}

        /// <summary> 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            return this.View();
        }

		/// <summary> 获取商品id对应的图片
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        public ActionResult Image(string id)
        {
			if (String.IsNullOrWhiteSpace(id))
				return this.Empty();
			this.Response.ContentType = "image/png";
			string fullpath = this.DatabaseContext.GetDirectoryPath(DIRECTORY_NAME) + id;
			if (System.IO.File.Exists(fullpath))
			{
				return this.File(fullpath, "image/png");
			}
            return this.Empty();
        }

		/// <summary> 同步数据
		/// </summary>
		/// <param name="datetime">上次更新的时间</param>
		/// <returns></returns>
		public ActionResult Sync(DateTime? datetime)
		{
			var commodities = this.DatabaseContext.GetCommodities(datetime);
			var list = from commodity in commodities select new 
			{
				ID = commodity.ID,
				Name = commodity.Name,
				Unit = commodity.Unit,
				Price = commodity.Price,
				Category = commodity.CategoryID,
				Enable = commodity.Enable
			};
			var result = EastRiverCommune.Models.Result.Create(list);
			return this.Json(result, JsonRequestBehavior.AllowGet);
		}

        /// <summary> 获取商品的描述
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Description(string id)
        {
            var desctiption = this.DatabaseContext.GetCommodityDescription(id);
            var result = EastRiverCommune.Models.Result.Create(desctiption);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
