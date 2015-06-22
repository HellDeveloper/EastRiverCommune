using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Extension
{
	public class RequiredAttribute : ValidationAttribute
	{

		public RequiredAttribute()
		{
			this.ErrorMessage = "{0}必填";
			this.AllowEmptyStrings = false;
		}

		public bool AllowEmptyStrings { get; set; }

		public override string FormatErrorMessage(string name)
		{
			return String.Format(this.ErrorMessage, name);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return false;
			if (!this.AllowEmptyStrings && value.ToString() == String.Empty)
				return false;
			return true;
		}

		public override Dictionary<string, object> GetValidateAttributes(ModelMetadata metadata, ControllerContext context)
		{
			Dictionary<string, object> attrs = new Dictionary<string, object>();
			attrs.Add("required", "required");
			attrs.Add("data-msg-required", this.FormatErrorMessage(metadata.DisplayName));
			return attrs;
		}
		

	}
}