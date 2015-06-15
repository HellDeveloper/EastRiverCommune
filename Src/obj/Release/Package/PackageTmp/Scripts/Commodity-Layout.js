$(function () {
	//
	$("#menu").click(function () {
		$("#menu-items").parent().removeClass("mask-fade-out").addClass("mask-fade-in");
		$("#menu-items").removeClass("aside-fade-out").addClass("aside-fade-in");
	});

	// 隐藏
	$(document).delegate(".mask", "click", function (event) {
		if (event.target != this)
			return;
		var $that = $(this);
		$that.children().removeClass("aside-fade-in").addClass("aside-fade-out");
		$that.removeClass("mask-fade-in").addClass("mask-fade-out");
		//$that.hide();
	});
});