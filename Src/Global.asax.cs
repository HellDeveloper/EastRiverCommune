using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace EastRiverCommune
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

        
	}

	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}

	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
			);
		}
	}

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}

	/// <summary>
	/// jQuery ajax 提交的数组
	/// </summary>
	public class BetterModelBinder : DefaultModelBinder
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			//bool is_enum = false;
			//var type = bindingContext.
			if (bindingContext.Model is System.Collections.IEnumerable)
			{
				var key = bindingContext.ModelName + "[]";
				var valueResult = bindingContext.ValueProvider.GetValue(key);
				if (valueResult != null && !string.IsNullOrEmpty(valueResult.AttemptedValue))
				{
					bindingContext.ModelName = key;
				}
			}
			return base.BindModel(controllerContext, bindingContext);
		}
	}

}