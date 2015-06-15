(function () {
	if (!window.API)
		window.API = {};

	window.API.Constant = {};

	window.API.EachProperty = function (callback) {
		var i = 0;
		for (var id in this) {
			var obj = this[id];
			if (obj && typeof (obj) != "function") {
				callback(i++, obj);
			}
		}
	};

	window.API.LocalStorage = {};

	window.API.LocalStorage.GetJson = function (name) {
		var temp = window.localStorage.getItem(name);
		var result = {};
		if (temp) {
			try {
				result = JSON.parse(temp);
			} catch (e) {
				console.log("从本地获取Json失败");
			}
		}
		return result;
	};

	window.API.LocalStorage.SetJson = function (name, json) {
		if (typeof (json) == "object") {
			window.localStorage.setItem(name, JSON.stringify(json));
		} else {
			window.localStorage.setItem(name, json);
		}
	};

})();

//WhenUpdate
(function () {
	var const_when_update = window.API.Constant.WhenUpdate = "__WhenUpdate__";

	window.API.WhenUpdate = function (name, date) {
		var obj = window.API.LocalStorage.GetJson(const_when_update);
		if (date) {
			obj[name] = date;
			window.API.LocalStorage.SetJson(const_when_update, obj);
		}
		return obj[name];
	}
})();