/*
 *
<div class="gallery">
    <ul class="pictures"></ul>
    <span class="previous icomoon">&#xf053;</span>
    <span class="next icomoon">&#xf054;</span>
</div>
 */
(function ($) {
	$.fn.Gallery = function (option) {
		this.each(function (index, element) {
			var opt = {
			    index: 0,
                onmove: null
			};
			$.extend(opt, option);
			var $that = $(element);
			var $gallery = $that.find(".gallery");
			var $pictures = $gallery.find(".pictures");
			var $previous = $gallery.find(".previous");
			var $next = $gallery.find(".next");

			if ($gallery.length == 0) {
				$gallery = $("<div></div>").addClass("gallery");
				$that.append($gallery);
			}
			if ($pictures.length == 0) {
				$pictures = $("<ul></ul>").addClass("pictures");
				$gallery.append($pictures);
			}

			var pictures = $pictures.get(0);
			var obj = {
				"begin_x": 0,
				"end_x": 0,
				"index": 0
			};
			var initialize = function () {
				obj.index = opt["index"];
				$pictures.children().each(function (index, element) {
					$pictures.width($pictures.width() + $(element).width())
				});
				move();
			}
			var move = function (event) {
			    if (obj.end_x == null)
			        return null;
			    var offset_x = obj.end_x - obj.begin_x;

			    //console.log("begin_x" + obj.begin_x);
			    //console.log("end_x" + obj.end_x);
				//console.log("offset x:" + offset_x);
				if (Math.abs(offset_x) > $pictures.parent().width() / 2.1) {
					if (offset_x > 0)
					    obj.index--;
					else if (offset_x < 0)
						obj.index++;
				}
				if (obj.index < 0)
				    obj.index = 0;
				else if (obj.index > $pictures.children().length - 1)
				    obj.index = $pictures.children().length - 1;
				if (opt.onmove && event) {
				    if (!event.data)
				        event.data = {};
				    event.data.index = obj.index;
				    opt.onmove(event);
				}
				var translate_x = (obj.index * $pictures.parent().width());
				$pictures.css({
					"transform": "translateX(" + (0 - translate_x) + "px)"
				}).css({
					"-webkit-transform": "translateX(" + (0 - translate_x) + "px)"
				});
			};
			var touch_start = function (event) {
				//console.log("touch start");
				event.preventDefault();
				var touch = event.touches[0];
				obj.begin_x = touch.pageX;
				obj.end_x = null;
			};
			var touch_move = function (event) {
				//console.log("touch move");
				event.preventDefault();
				var touch = event.touches[0];
				obj.end_x = touch.pageX;
				var translate_x = (touch.pageX - obj.begin_x) + (0 - obj["index"] * $pictures.parent().width());
				$pictures.css({
					"transform": "translateX(" + translate_x + "px)"
				}).css({
					"-webkit-transform": "translateX(" + translate_x + "px)"
				});
			};
			var touch_end = function (event) {
				//console.log("touch end");
				event.preventDefault();
				move(event);
				obj.begin_x = 0;
				obj.end_x = 0;
			};
			pictures.addEventListener("touchstart", touch_start, false);
			pictures.addEventListener("touchmove", touch_move, false);
			pictures.addEventListener("touchend", touch_end, false);
			$previous.on("click", function (event) {
			    event.preventDefault();
			    obj.index--;
			    move(event);
			    //console.log("previous:" + obj.index);
			});

			$next.on("click", function (event) {
			    console.log("next");
			    event.preventDefault();
			    if (event.target != this)
			        return;
			    obj.index++;
			    move(event);
			    //console.log("next:" + obj.index);
			});

			initialize();
		});
	}
})(jQuery);