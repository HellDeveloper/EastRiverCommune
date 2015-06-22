using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Extension
{
	public abstract class ValidationAttribute : System.ComponentModel.DataAnnotations.ValidationAttribute
	{
		public abstract Dictionary<string, object> GetValidateAttributes(ModelMetadata metadata, ControllerContext context);

		/// <summary> 
		/// </summary>
		public Func<object, bool> ValidCallback { get; set; }

		public override bool IsValid(object value)
		{
			if (this.ValidCallback == null)
				return base.IsValid(value);
			return this.ValidCallback(value);
		}

	}
}