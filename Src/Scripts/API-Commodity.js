(function () {
	var constant_commodities = window.API.Constant.Commodities = "Commodities";

	var add_commodity = function (id, name, price, unit, description, category_id) {
		return {
			"ID": id,
			"Name": name,
			"Price": price,
			"Unit": unit,
			"Description": description,
			"CategoryID": category_id
		};
	}

	var sync = function (success) {
		var that = this;
		$.ajax({
			"url": window.API.ApplicationPath + "/Commodity/Sync",
			"type": "POST",
			"data": {
				"datetime": window.API.WhenUpdate(constant_commodities)
			},
			"success": function (result) {
				if (result["Code"] != "0") {
					return;
				}
				//console.log(window.API.WhenUpdate(constant_commodities))
				var date = new Date(parseInt(result["DateTime"].replace("/Date(", "").replace(")/", ""), 10));
				window.API.WhenUpdate(constant_commodities, date);
				var commodities = that;
				var cart = window.API.Cart();
				var array = result["Data"];
				for (var i = 0; i < array.length; i++) {
					var obj = array[i];
					if (obj["Enable"]) {
						commodities[obj.ID] = obj;
						if (cart[obj.ID]) {
							var count = cart[obj.ID]["Count"] > 0 ? cart[obj.ID]["Count"] : 1;
							cart.Add(obj.ID, obj.Name, obj.Price, count, obj.Unit)
						}
					} else {
						commodities.Remove(obj.ID);
						cart.Remove(obj.ID);
					}
				}
                console.log(commodities)
				window.API.Commodities(commodities);
				window.API.Cart(cart);
				success(result);
			}
		});
	}

	var get_commodities = function () {
		var commodities = window.API.LocalStorage.GetJson(constant_commodities);
		commodities.Add = function (id, name, price, unit, description) {
			this[id] = add_commodity(id, name, price, unit, description);
		}
		commodities.Remove = function (id) {
			delete this[id];
		}
		commodities.Each = window.API.EachProperty;
		commodities.Sync = sync;
		return commodities;
	}

	var set_commodities = function (commodities) {
		window.API.LocalStorage.SetJson(constant_commodities, commodities);
	}

	window.API.Commodities = function (commodities) {
		/// <summary>商品</summary>
		if (arguments.length == 0) {
			return get_commodities();
		} else {
			set_commodities(commodities);
		}
	}
})();