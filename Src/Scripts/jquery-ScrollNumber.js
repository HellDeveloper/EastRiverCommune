/*
 *
<div class="gallery">
    <ul class="pictures"></ul>
    <span class="previous icomoon">&#xf053;</span>
    <span class="next icomoon">&#xf054;</span>
</div>
 */
(function ($) {
	var identity = 0;
	$.fn.ScrollNumber = function (option) {
		var padding_left = function (str, length, character) {
			if (!character)
				character = "0";
			str = str + "";
			for (var i = str.length; i < length; i++) {
				str = character + str;
			}
			return str;
		}
		this.each(function (index, element) {
			var opt = {
				"min": 0,
				"max": 100,
				"index": 0
			};
			$.extend(opt, option);
			if (!opt.index)
				opt.index = opt.min;
			var obj = {
				"begin_y": 0,
				"end_y": 0,
				"index": opt.index
			};
			var $that = $(element);
			var $scroll_number = {};
			if ($that.data("scroll-number")) {
				var $scroll_number = $("#" + $that.data("scroll-number"));
			} else {
				$scroll_number.length = 0;
			}
			if ($scroll_number.length == 0) {
				var id = "__scroll_number__" + identity;
				$scroll_number = $("<div></div>").addClass("scroll-number").attr("id", id);
				$that.data("scroll-number", id);
				$("body").append($scroll_number);
			}
			var $wrapper = $scroll_number.find(".wrapper");
			if ($wrapper.length == 0) {
				$wrapper = $("<div></div>").addClass("wrapper");
				$scroll_number.append($wrapper);
			}
			$scroll_number.show();
			$wrapper.empty();
			var $box = $("<div></div>").addClass("box").appendTo($wrapper);
			var $numbers = $("<ul></ul>").addClass("numbers").appendTo($box);
			var numbers = $numbers.get(0);
			for (var i = opt.min; i < opt.max; i++) {
				var text = padding_left(i, (opt.max + "").length);
				$("<li></li>").appendTo($numbers).addClass("number").text(text);
			}
			var touch_start = function (event) {
				//console.log("touch start");
				event.preventDefault();
				var touch = event.touches[0];
				obj.begin_y = touch.pageY;
			};
			var touch_move = function (event) {
				//console.log("touch move");
				event.preventDefault();
				var touch = event.touches[0];
				obj.end_y = touch.pageY;
				var translate_y = (touch.pageY - obj.begin_y) + (0 - obj["index"] * $numbers.children().height());
				//console.log("touch.pageY:" + touch.pageY);
				//console.log("obj.begin_y:" + obj.begin_y);
				//console.log("translate_y:" + translate_y);
				$numbers.css({
					"transform": "translateY(" + translate_y + "px)"
				}).css({
					"-webkit-transform": "translateY(" + translate_y + "px)"
				});
			};
			var touch_end = function (event) {
				//console.log("touch end");
				event.preventDefault();
				var index = (obj.end_y - obj.begin_y) / $numbers.children().height();
				//index = Math.abs(index);
				console.log(index);
				if (Math.abs(index) > parseInt(Math.abs(index)) + 0.5)
					obj.index += 0 - (parseInt(index) + 1);
				else
					obj.index += 0- parseInt(index);

				if (obj.index < 0)
					obj.index = 0;
				else if (obj.index >= $numbers.children().length)
					obj.index = $numbers.children().length - 1;

				console.log("obj.index:" + obj.index)
				var translate_y = (0 - obj["index"] * $numbers.children().height());
				$numbers.css({
					"transform": "translateY(" + translate_y + "px)"
				}).css({
					"-webkit-transform": "translateY(" + translate_y + "px)"
				});
				obj.begin_y = 0;
				obj.end_y = 0;
			};
			numbers.addEventListener("touchstart", touch_start, false);
			numbers.addEventListener("touchmove", touch_move, false);
			numbers.addEventListener("touchend", touch_end, false);
			var $buttons = $("<div></div>").addClass("buttons").appendTo($wrapper);
			var $cancel = $("<input />").attr("type", "button").addClass("cancel").val("取消").appendTo($buttons);
			var $ok = $("<input />").attr("type", "button").addClass("ok").val("确定").appendTo($buttons);
		});
	}
})(jQuery);