$(function () {
    var $gallery = $(".gallery");
    var $pictures = $(".gallery > ul.pictures");
    var create_li_picture = function (commodity) {
        var $li = $("<li></li>").addClass("picture").css({
            "background-image": "url(" + window.API.ApplicationPath + "/Commodity/Image/" + commodity.ID + ")"
        }).width($gallery.width()).height($gallery.height());
        li_picture_touch($li, commodity);
        return $li;
    };
    var li_picture_touch = function ($li, commodity) {
        var li = $li.get(0);
        var obj = {
            begin_x: 0,
            end_x: 0
        };
        var li_picture_touch_start = function (event) {
            event.preventDefault();
            console.log("touch_start");
            var touch = event.touches[0];
            obj.begin_x = touch.pageX;
        };
        var li_picture_touch_move = function (event) {
            event.preventDefault();
            console.log("touch_move");
            var touch = event.touches[0];
            console.log(touch.pageX - obj.begin_x);
            obj.end_x = touch.pageX;

            $li.prev ().css({
                "transform": "translateX(" + (touch.pageX - obj.begin_x) + "px)"
            }).css({
                "-webkit-transform": "translateX(" + (touch.pageX - obj.begin_x) + "px)"
            });

            $li.css({
                "transform": "translateX(" + (touch.pageX - obj.begin_x) + "px)"
            }).css({
                "-webkit-transform": "translateX(" + (touch.pageX - obj.begin_x) + "px)"
            });

            $li.next().css({
                "transform": "translateX(" + (touch.pageX - obj.begin_x) + "px)"
            }).css({
                "-webkit-transform": "translateX(" + (touch.pageX - obj.begin_x) + "px)"
            });
        };
        var li_picture_touch_end = function (event) {
            event.preventDefault();
            var touch = event.touches[0];
            console.log("touch_end")
        };
        li.addEventListener("touchstart", li_picture_touch_start, false);
        li.addEventListener("touchmove", li_picture_touch_move, false);
        li.addEventListener("touchend", li_picture_touch_end, false);
        //$li.bind("touchstart", li_picture_touch_start);
        //$li.bind("touchmove", li_picture_touch_move);
        //$li.bind("touchend", li_picture_touch_end);
    };
    var initialize = function () {
        $gallery.height($gallery.width());
        $pictures.height($gallery.height());
        var commodities = window.API.Commodities();
        commodities.Each(function (index, commodity) {
            var $li = create_li_picture(commodity);
            $pictures.append($li);
        });
    };
    initialize();
});