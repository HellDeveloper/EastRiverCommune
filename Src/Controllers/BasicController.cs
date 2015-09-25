using EastRiverCommune.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EastRiverCommune.Controllers
{
    public class BasicController : Controller
	{
		public BasicController()
		{
			string language = "zh";
			var culture = System.Globalization.CultureInfo.CreateSpecificCulture(language);
			var uiculture = new System.Globalization.CultureInfo(language);
			culture.DateTimeFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss";
			culture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
			culture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
			uiculture.DateTimeFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss";
			uiculture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
			uiculture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
			System.Threading.Thread.CurrentThread.CurrentCulture = culture;
			System.Threading.Thread.CurrentThread.CurrentUICulture = uiculture;
		}

        #region DatabaseContext
        private EastRiverEntities database_context;

		public virtual EastRiverEntities DatabaseContext
		{
			get
			{
				if (this.database_context == null)
					this.database_context = new EastRiverEntities();
				return this.database_context;
			}
		}
        #endregion

        #region JsonSerializer
        private Newtonsoft.Json.JsonSerializer _json_serializer;

		public Newtonsoft.Json.JsonSerializer JsonSerializer
		{
			get 
			{
				if (this._json_serializer == null)
					this._json_serializer = new Newtonsoft.Json.JsonSerializer();
				return this._json_serializer;
			}
		}
        #endregion

        #region WeChat
        private EastRiverCommune.Api.WeChat _we_chat;

        public EastRiverCommune.Api.WeChat WeChat
        {
            get
            {
                if (this._we_chat == null)
                    this._we_chat = new Api.WeChat(this, "wxa8599d5d1d3568c9");
                return this._we_chat;
            }
        }
        #endregion



        protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
			{
				if (this.database_context != null)
					this.database_context.Dispose();
			}
		}

        /// <summary> 什么都不做
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Empty()
        {
            return new EmptyResult();
        }

		/// <summary>
		/// 验证模式
		/// </summary>
		/// <returns>成功返回null</returns>
		[NonAction]
		protected ActionResult ValidateModel()
		{
			if (this.ModelState.IsValid)
				return null;
			return this.PartialView("~/Views/Shared/_ValidationSummary.cshtml");
		}


        
    }
}
