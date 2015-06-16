

$(function () {
	var $sum = $("#sum");
	var initialize = function () {
	    var commodities = window.API.Commodities();
	    commodities.Sync(initialize);
		var cart = window.API.Cart();
		$("#commodities .commodity").height($("#commodities .commodity").width() * window.API.CommodityPictureProportion);
	    $("#commodities .commodity input[type=checkbox]").each(function (index, checkbox) {
	        if (cart[checkbox.value])
	            checkbox.checked = true;
	        else
	            checkbox.checked = false;
	    });
	    $sum.find("span").text("总金额：￥" + cart.CalculateSum());
	}

	$(document).delegate("#categories .category", "click", function (event) {
		$(".category").show();
		var $that = $(this).hide();
		var category_id = $that.data("category-id");
		if (category_id) {
			$("#commodities .commodity").each(function (index, commodity) {
				var $commodity = $(commodity);
				if ($commodity.data("category-id") == category_id) {
					$commodity.show();
				} else {
					$commodity.hide();
				}
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

	$("#commodities .commodity input[type=checkbox]").on("click", function (event) {
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

	$("#commodities .commodity").on("click", function (event) {
		if (event.target == this)
			window.location.href = window.API.ApplicationPath + "/Commodity/Detail/" + $(this).data("commodity-id");
	});

    $("#pay").click(function () {
    	window.location.href = window.API.ApplicationPath + "/Commodity/Cart";
    }).find("span").text("去结算");

    // Adding popstate event listener to handle browser back button
    $(window).on("popstate", function () {
    	initialize();
    });

   
    //initialize();
});