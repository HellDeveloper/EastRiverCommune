using EastRiverCommune.Entities;
using EastRiverCommune.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EastRiverCommune.Controllers
{
    public class OrderController : BasicController
    {
        //
        // GET: /Order/

		public ActionResult Verify(OrderList order_list)
		{
			if (order_list.OrderItems == null || order_list.OrderItems.Count == 0)
			{
				this.ModelState.AddModelError("OrderItems", "没有购买的商品");
			}
			var validate_result = this.ValidateModel();
			if (validate_result != null)
				return validate_result;
			order_list.OrderItems = this.DatabaseContext.Verify(order_list.OrderItems);
			return PartialView("~/Views/Order/_ShoppingList.cshtml", order_list);
		}

    }
}
