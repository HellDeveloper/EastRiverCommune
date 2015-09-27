using EastRiverCommune.Entities;
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

		/// <summary> 购物清单
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ShoppingList()
		{
			return this.View(new OrderList() { DeliveryDate = DateTime.Now });
		}

		/// <summary> 购物清单
		/// </summary>
		/// <param name="order_list"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult ShoppingList(OrderList order_list)
		{
			if (!ValidateShoppingList(order_list))
				return this.Content("提交表单错误");
			order_list.ID = this.DatabaseContext.GenerateID;
			order_list.WhenModify = System.DateTime.Now;
			order_list.Enable = true;
			foreach (var item in order_list.OrderItems)
			{
				item.ID = this.DatabaseContext.GenerateID;
				item.OrderListID = order_list.ID;
				item.WhenModify = order_list.WhenModify;
				item.Enable = order_list.Enable;
			}
			this.DatabaseContext.OrderLists.Add(order_list);
			this.DatabaseContext.SaveChanges();
			this.DatabaseContext.Reinitialize(order_list.ID);

			return this.ConfirmPay(order_list.ID);
		}

		private bool ValidateShoppingList(OrderList order_list)
		{
			if (order_list == null || order_list.OrderItems == null || order_list.OrderItems.Count == 0)
				return false;
			if (!this.ModelState.IsValid)
				return false;
			if (System.DateTime.Now.Date > order_list.DeliveryDate.Date)
				return false;
			return true;
		}

		/// <summary> 确认支付
		/// </summary>
		/// <param name="order_list_id"></param>
		/// <returns></returns>
		[HttpGet]
		public ActionResult ConfirmPay(string order_list_id)
		{
			if (String.IsNullOrWhiteSpace(this.WeChat.OpenID))
			{
				string redirect_uri = "http://" + this.Request.Url.Authority + this.Url.Content("~/WeChat/CommodityConfirmPay?order_list_id=" + order_list_id);
				//redirect_uri = redirect_uri + "?redirect_uri=" + HttpUtility.UrlEncode(this.Url.Content("~/Commodity/ConfirmPay") + "?order_list_id=" + order_list_id);
				string state = this.Session.SessionID; //;
				string url = this.WeChat.GetPublicOAuthUrl(redirect_uri, state);
				return this.PartialView("~/Views/Shared/_WindowOpen.cshtml", url);
			}
			return this.PartialView("~/Views/Commodity/ConfirmPay.cshtml");
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
			var list = from commodity in commodities
					   select new
						   {
							   ID = commodity.ID,
							   Name = commodity.Name,
							   Unit = commodity.Unit,
							   Price = commodity.Price,
							   CategoryID = commodity.CategoryID,
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
