(function ($) {
	$.fn.SelectNumber = function (option) {
		var find_not_append = function ($context, _class) {
			var temp = $context.find("." + _class);
			if (temp.length == 0) {
				temp = $("<div></div>").addClass(_class).appendTo($context);
			}
			return temp;
		};
		var select_number_id = "jquery-select-number";
		var $SelectNumber = $("#" + select_number_id);
		if ($SelectNumber.length == 0)
			$SelectNumber = $("<div></div>").addClass("masker").attr("id", select_number_id).appendTo("body");
		var $wrapper = find_not_append($SelectNumber, "wrapper");
		var $content = find_not_append($wrapper, "content");
		var $buttons = find_not_append($wrapper, "buttons");
		var $decrement = find_not_append($content, "decrement").html("&#xe60b;").addClass("icomoon");
		var $current = find_not_append($content, "current");
		var $increment = find_not_append($content, "increment").html("&#xe609;").addClass("icomoon");
		var $cancel = $buttons.find(".cancel");
		var $ok = $buttons.find(".ok");
		if ($cancel.length == 0)
			$cancel = $("<input />").addClass("cancel").attr("type", "button").val("取消").appendTo($buttons);
		var $cancel_I_ok = find_not_append($buttons, "separator");
		if ($ok.length == 0)
			$ok = $("<input />").addClass("ok").attr("type", "button").val("确定").appendTo($buttons);
		var on_click = function () {
			var $that = $(this);
			var opt = {
				"min": $that.attr("min"),
				"max": $that.attr("max"),
				"current": 0,
				"$that": $that
			};
			$.extend(opt, option);
			$SelectNumber.show();
			$cancel.unbind("click").bind("click", function (event) {
				$SelectNumber.hide();
				event.Option = opt;
				if (opt.oncancel && typeof (opt.oncancel) == "function")
					opt.oncancel(event);
			});
			$ok.unbind("click").bind("click", function (event) {
				$SelectNumber.hide();
				event.Option = opt;
				if (opt.onok && typeof (opt.onok) == "function")
					opt.onok(event);
			});
			$increment.unbind("click").bind("click", function (event) {
				opt.current++;
				opt.current = opt.current <= opt.max ? opt.current : opt.max;
				event.Option = opt;
				if (opt.onincrement && typeof (opt.onincrement) == "function")
					opt.onincrement(event);
				$current.val(opt.current).text(opt.current);
			});
			$decrement.unbind("click").bind("click", function (event) {
				opt.current--;
				opt.current = opt.current >= opt.min ? opt.current : opt.min;
				event.Option = opt;
				if (opt.ondecrement && typeof (opt.ondecrement) == "function")
					opt.ondecrement(event);
				$current.val(opt.current).text(opt.current);
			});
			opt.min = parseInt(opt.min ? opt.min : Number.MIN_VALUE);
			opt.max = parseInt(opt.max ? opt.max : Number.MAX_VALUE);
			opt.current = parseInt($that.is("input") ? $that.val() : $that.text());
			$current.text(opt.current = opt.current ? opt.current : opt.min);
		}
		this.unbind("click", on_click).bind("click", on_click);
		return this;
	};
})(jQuery);