(function ($) {
	$.fn.SerializeObject = function () {
		var object = {};
		var array = this.serializeArray();
		for (var i = 0; i < array.length; i++) {
			var item = array[i];
			object[item.name] = item.value;
		}
		return object;
	}
})(jQuery);