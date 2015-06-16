$(function () {
    var $gallery = $(".gallery");
    var $pictures = $(".gallery > ul.pictures");
    var search = "/Commodity/Detail/";
    var url = window.location.href;
    var id = url.substring(url.indexOf(search) + search.length, url.length);
    var create_li_picture = function (commodity) {
        var $li = $("<li></li>").addClass("picture").data("commodity", commodity.ID).css({
            "background-image": "url(" + window.API.ApplicationPath + "/Commodity/Image/" + commodity.ID + ")"
        }).width($gallery.width()).height($gallery.height());
        return $li;
    };

    $("#pay").on("click", function () {
		window.location.href = window.API.ApplicationPath + "/Commodity/Cart"
    }).children().text("去结算");

    var initialize = function () {
        $gallery.height($gallery.width() * window.API.CommodityPictureProportion);
        $pictures.height($gallery.width() * window.API.CommodityPictureProportion);
        var commodities = window.API.Commodities();
        var descriptions = {};
        var picture_move = function (event) {
            var $picture = $($pictures.children().get(event.data.index));
            var commodity = window.API.Commodities()[$picture.data("commodity")];
            var $information = $(".information");
            if (commodity.Unit == null)
            	commodity.Unit = "";
            $information.find(".name").text(commodity.Name);
            $information.find(".price").text("￥" + commodity.Price + "/" + commodity.Unit);
            var $count = $information.find(".count");
            $information.find(".decrement").unbind("click").bind("click", function () {
                var cart = window.API.Cart();
                var c = cart[commodity.ID];
                if (c) {
                    var count = parseInt($count.text());
                    c.Count = count = count - 1;
                    if (count < 0)
                        c.Count = count = 0;
                    $count.text(count);
                    if (count == 0) {
                        cart.Remove(commodity.ID);
                    }
                    window.API.Cart(cart);
                    cart.Set$Sum();
                }
            });
            $information.find(".increment").unbind("click").bind("click", function () {
                var cart = window.API.Cart();
                var c = cart[commodity.ID];
                var count = parseInt($count.text());
                count += 1;
                console.log("count:" + count)
                if (!c) {
                    cart.Add(commodity.ID, commodity.Name, commodity.Price, count, commodity.Unit);
                    c = cart[commodity.ID];
                }
                c.Count = count;
                $count.text(count);
                window.API.Cart(cart);
                cart.Set$Sum();
            });
            var cart = window.API.Cart();
            var c = cart[commodity.ID];
            if (c) {
                $count.text(c.Count);
            } else {
                $count.text(0);
            }
            cart.Set$Sum();
            var description = descriptions[commodity.ID];
            if (description) {
                console.log(description);
                $(".description").text(description);
            } else {
                $.ajax({
                    "url": window.API.ApplicationPath + "/Commodity/Description",
                    "type": "post",
                    "data": {
                        "id": commodity.ID
                    },
                    "success": function (data) {
                        var temp = "";
                        if (data.Code != "0") {
                            temp = "";
                        } else {
                            if (!data.Data)
                                data.Data = "";
                            temp = data.Data;
                        }
                        $(".description").text(temp);
                        descriptions[commodity.ID] = temp;
                    }
                });
            }
        };
        var gallery_option = {
            "index": 0,
            "onmove": picture_move
        };
        commodities.Each(function (index, commodity) {
            var $li = create_li_picture(commodity);
            $pictures.append($li);
            if (commodity.ID == id)
                gallery_option["index"] = index;
        });
        $gallery.parent().Gallery(gallery_option);
        picture_move({
            "data": {
                "index": gallery_option["index"]
            }
        });
    };
    initialize();
});