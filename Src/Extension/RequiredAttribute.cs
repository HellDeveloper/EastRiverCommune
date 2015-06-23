using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Extension
{
	public class RequiredAttribute : ValidationAttribute
	{

		public RequiredAttribute()
		{
			this.AllowEmptyStrings = false;
		}

		public bool AllowEmptyStrings { get; set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (this.IsValid(value))
				return ValidationResult.Success;
			return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
		}

		public override string FormatErrorMessage(string name)
		{
			if (String.IsNullOrWhiteSpace(this.ErrorMessage))
				this.ErrorMessage = "{0}必填";
			return String.Format(this.ErrorMessage, name);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return false;
			if (value is System.Collections.IEnumerable)
			{ 
				if (this.AllowEmptyStrings)
					return true;
				else if (Core.Has(value as System.Collections.IEnumerable))
					return true;
				else
					return false;
			}
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