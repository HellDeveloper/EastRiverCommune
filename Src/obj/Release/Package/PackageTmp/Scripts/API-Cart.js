(function () {
	var constant_cart = window.API.Constant.Cart = "Cart";

	/*
     * 计算总金额
     */
	var calculate_sum = function (cart) {
		var total = 0;
		for (var id in cart) {
			var commodity = cart[id];
			if (typeof (commodity) == "function")
				continue;
			total += calculate_total(commodity);
		}
		return total;
	}

	var calculate_total = function (commodity) {
		if (typeof (commodity) != "object")
			return 0;
		var price = parseFloat(commodity["Price"]);
		var count = parseFloat(commodity["Count"]);
		if (!price || !count)
			return 0;
		return price * count;
	}

	var add_commodity = function (id, name, price, count, unit) {
		return {
			"ID": id,
			"Name": name,
			"Price": price,
			"Count": count,
			"Unit": unit
		};
	}

	var get_cart = function () {
		var cart = window.API.LocalStorage.GetJson(constant_cart);
		cart.Remove = function (id) {
			delete this[id];
		}
		cart.Add = function (id, name, price, count, unit) {
			this[id] = add_commodity(id, name, price, count, unit);
		}
		cart.CalculateSum = function (id) {
			if (id) {
				var commodity = {};
				if (commodity = this[id])
					return calculate_total(commodity);
				else
					return 0;
			}
			return calculate_sum(this);
		};
		cart.Each = window.API.EachProperty;
		cart.Decrement = function (id) {
			var commodity = {};
			if (id && (commodity = this[id])) {
				commodity["Count"] = commodity["Count"] - 1;
				return commodity["Count"];
			}
			return null;
		};
		cart.Increment = function (id) {
			var commodity = {};
			if (id && (commodity = this[id])) {
				commodity["Count"] = commodity["Count"] - 0 + 1;
				return commodity["Count"];
			}
			return null;
		};
		cart.Set$Sum = function () {
			$("#sum > span").text("总金额：￥" + this.CalculateSum());
		};
		cart.CreateForm = function (action) {
			var $form = $("<form></form>").attr("method", "post").attr("action", action);
			this.Each(function (index, commodity) {
				$("<input />").attr("type", "hidden").attr("name", "OrderItems[" + index + "].Count").addClass("Cart-CreateForm").val(commodity.Count).appendTo($form);
				$("<input />").attr("type", "hidden").attr("name", "OrderItems[" + index + "].CommodityID").addClass("Cart-CreateForm").val(commodity.ID).appendTo($form);
			});
			return $form;
		};
		return cart;
	}

	var set_cart = function (cart) {
		window.API.LocalStorage.SetJson(constant_cart, cart);
	}

	window.API.Cart = function (cart) {
		/// <summary>购物车</summary>
		if (arguments.length == 0) {
			return get_cart();
		} else {
			set_cart(cart);
		}
	}
})();