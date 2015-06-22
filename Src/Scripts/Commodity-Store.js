$(function () {
	var $sum = $("#sum");
	var $commodities = $("#commodities");
	$(window).on("resize", function () {
		var commodity_size = calculate_commodity_size();
		$commodities.children().each(function (index, commodity) {
			$(commodity).css({
				"width": commodity_size.width + "px",
				"height": commodity_size.height + "px",
				"margin": commodity_size.margin + "px"
			});
		});
	});

	var calculate_commodity_size = function () {
		var width = 190;
		var margin = 5;
		var count = Math.ceil(($commodities.width()) / (width + margin * 2));
		var commodity_width = Math.floor($commodities.width() / count) - margin * 2 - Math.ceil($commodities.scrollLeft() / count);
		console.log(commodity_width)
		return { "width": commodity_width, "height": commodity_width * window.API.CommodityPictureProportion, "margin": margin };
	}

	var initialize = function () {
		var commodity_size = calculate_commodity_size();
		var commodities = window.API.Commodities();
		var array = [];
		var appendToCommodities = function () {
			for (var i = 0; i < array.length; i++) {
				$commodities.append(array[i]);
			}
			array = [];
		}
		commodities.Each(function (index, commodity) {
		    console.log(commodity);
		    var $commodity = $("<section></section>").addClass("commodity").attr("data-commodity-id", commodity.ID).css({
				"background-image": "url(" + window.API.ApplicationPath + "/Commodity/Image/" + commodity.ID + ")",
				"width": commodity_size.width + "px",
				"height": commodity_size.height + "px",
				"margin": commodity_size.margin + "px"
		    }).attr("data-category-id", commodity.CategoryID);
			var $checkbox = $("<div></div>").addClass("checkbox").appendTo($commodity);
			$("<input />").attr("id", commodity.ID).attr("name", "commodities").attr("type", "checkbox").val(commodity.ID).appendTo($checkbox);
			$("<label></label>").addClass("icomoon").attr("for", commodity.ID).html("&#xe602;").appendTo($checkbox);
			var $info = $("<div></div>").addClass("info").appendTo($commodity);
			$("<span></span>").addClass("name").text(commodity.Name).appendTo($info);
			$("<span></span>").addClass("price").data("price", commodity.Price).data("unit", commodity.Unit).text("￥" + commodity.Price + "/" + commodity.Unit).appendTo($info);
			array.push($commodity);
			if (array.length > 10)
				appendToCommodities();
		});
		appendToCommodities();
		upgrade();
		$commodities.css("height", "auto");
	};

	var upgrade = function () {
		var cart = window.API.Cart();
		//$("#commodities .commodity").height($("#commodities .commodity").width() * window.API.CommodityPictureProportion);
		$("#commodities .commodity input[type=checkbox]").each(function (index, checkbox) {
			if (cart[checkbox.value])
				checkbox.checked = true;
			else
				checkbox.checked = false;
		});
		cart.Set$Sum();
		//$sum.find("span").text("总金额：￥" + cart.CalculateSum());
	};

	$("#categories").delegate(".category", "click", function (event) {
		$(".category").show().width(70);
		var $that = $(this).hide().width(0);
		var category_id = $that.data("category-id");
		if (category_id) {
			$("#commodities .commodity").each(function (index, commodity) {
				var $commodity = $(commodity);
				if ($commodity.data("category-id") == category_id) {
					$commodity.show();
				} else {
					$commodity.hide();
				}
				console.log($commodity.data("category-id"))
			});
		} else {
			$("#commodities .commodity").show();
		}
		$("#categories .show").text($that.text());
	});

	$("#category-count").click(function (event) {
		var categories_div1 = document.querySelector("#categories > div:nth-child(1)");
		var $categories = $(categories_div1).parent();
		var height = $(categories_div1).children().first().height();
		if ($categories.height() == height)
			$categories.height(categories_div1.scrollHeight);
		else
			$categories.height(height);
	});

	$commodities.delegate(".commodity input[type=checkbox]", "click", function (event) {
		event.stopPropagation();
    	var $that = $(this);
    	var $commodity = $that.closest(".commodity");
    	var id = $commodity.data("commodity-id");
    	var cart = window.API.Cart();
    	if (this.checked) {
    		cart.Add(id, $commodity.find(".name").text(), $commodity.find(".price").data("price"), 1, $commodity.find(".price").data("unit"));
    	} else {
    		cart.Remove(id);
    	}
    	window.API.Cart(cart);
    	$sum.find("span").text("总金额：￥" + cart.CalculateSum());
    });

	$commodities.delegate(".commodity", "click", function (event) {
		if (event.target == this)
			window.location.href = window.API.ApplicationPath + "/Commodity/Detail/" + $(this).data("commodity-id");
	});

    $("#pay").click(function () {
    	window.location.href = window.API.ApplicationPath + "/Commodity/ShoppingList";
    }).find("span").text("去结算");

    // Adding popstate event listener to handle browser back button
    $(window).on("popstate", function () {
    	upgrade();
    });

	window.API.Commodities().Sync(initialize);
});