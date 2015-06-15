$(function () {
	var $order_item_tbody = $("#order-item > tbody");
	var initialize = function () {
		var cart = window.API.Cart();
		$order_item_tbody.empty();
		cart.Each(function (i, commodity) {
			var $tr = {};
			var $count = $("<div></div>").addClass("count").text(commodity["Count"]);
			$tr = $("<tr></tr>").attr("id", commodity["ID"]);
			$tr.append($("<td></td>")
					.append($("<div></div>").text(commodity["Name"]).addClass("name"))
					.append($("<div></div>").text("￥" + commodity["Price"] + "/" + commodity["Unit"]).addClass("price")));
			$tr.append($("<td></td>").css({
				"text-align": "center",
				"vertical-align": "middle"
			}).append($count));
			$tr.append($("<td></td>")
					.append($("<span></span>").text(cart.CalculateSum(commodity["ID"])).addClass("total")));
			$tr.append($("<td></td>")
					.append($("<span>&#xf014;</span>").addClass("icomoon").addClass("delete")));
			$order_item_tbody.append($tr);

			// 分隔行
			$tr = $("<tr></tr>");
			$tr.append($("<td><span class='vertical-center'><span></span><span></td>").attr("colspan", "4"));
			$order_item_tbody.append($tr);

			$count.data("commodity-id", commodity["ID"]);
			$count.SelectNumber({
				"min": 1,
				"current": $count.text(),
				"max": 100,
				"onok": function (event) {
					var cart = window.API.Cart();
					var $that = event.Option["$that"];
					var item = cart[$that.data("commodity-id")];
					$that.text(item.Count = event.Option.current);
					$that.closest("tr").find(".total").text(cart.CalculateSum(item["ID"]));
					window.API.Cart(cart);
					cart.Set$Sum();
				}
			});
		});
		$("#pay > span").text("确定支付");
		$("#pay").on("click", function () {
			var cart = window.API.Cart();
			var $form = cart.CreateForm(window.API.ApplicationPath + "/Order/Verify");
			$("#order-list").find("[type=text], [type=tel], select, textarea").clone().appendTo($form);
			console.log($form);
			$form.appendTo($("body")).submit();
			//var form = $("#order-list").SerializeObject();
			//form["OrderItems"] = [];
			//$("#order-item").find("tbody tr").each(function (index, tr) {
			//	var $count = $(tr).find(".count");
			//	var commodity = $count.data("commodity-id");
			//	if (!commodity)
			//		return;
			//	form["OrderItems"].push({
			//		"CommodityID": commodity,
			//		"Count": $count.text()
			//	});
			//});
			//console.log(JSON.stringify(form));
			//$.ajax({
			//	"url": window.API.ApplicationPath + "/Order/Verify",
			//	"type": "POST",
			//	"contentType": 'application/json; charset=utf-8',
			//	"data": JSON.stringify(form)
			//});
		});
		DeliveryManner_on_input();
		cart.Set$Sum();
	};

	$order_item_tbody.delegate(".delete", "click", function () {
		var $that = $(this);
		var $tr = $that.closest("tr");
		var id = $tr.attr("id");
		var cart = window.API.Cart();
		cart.Remove(id);
		window.API.Cart(cart);
		cart.Set$Sum();
		$tr.next().remove();
		$tr.remove();
	});

	var DeliveryManner_on_input = function () {
		if ($("#DeliveryManner").val() == "快递送货") {
			$("#Address").attr("required", "required").parent().css({
				"display": "-moz-box"
			}).css({
				"display": "-ms-flexbox"
			}).css({
				"display": "-webkit-box"
			}).css({
				"display": "-webkit-flex"
			}).css({
				"display": "flex"
			});
		} else {
			$("#Address").removeAttr("required").parent().css({
				"display": "none"
			});
		}
	};

	$("#DeliveryManner").on("change", function () {
		DeliveryManner_on_input();
	});

	$(window).on("popstate", function () {
		initialize();
	});

	initialize();
});

$(function () {
	var now = new Date();
	$("#DeliveryDate").mobiscroll({
		"preset": "date",
		"theme": 'android-ics light', //皮肤样式
		"display": 'modal', //显示方式 
		"mode": 'scroller', //日期选择模式
		"dateFormat": 'yyyy-mm-dd',
		"lang": 'zh',
		"showNow": true,
		"nowText": "今天",
		"startYear": now.getFullYear(), //开始年份
		"endYear": now.getFullYear() + 10 //结束年份
	});
});