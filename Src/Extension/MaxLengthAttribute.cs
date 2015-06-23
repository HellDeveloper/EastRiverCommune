using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Extension
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class MaxLengthAttribute : ValidationAttribute
	{
		public MaxLengthAttribute(int minlength)
		{
			this.MaximumLength = minlength;
		}

		public int MaximumLength { get; set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (this.IsValid(value))
				return ValidationResult.Success;
			return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
		}

		public override string FormatErrorMessage(string name)
		{
			if (String.IsNullOrWhiteSpace(this.ErrorMessage))
				this.ErrorMessage = "{0}最多{1}个字符";
			return String.Format(this.ErrorMessage, name, this.MaximumLength);
		}

		public override bool IsValid(object value)
		{
			if (value == null)
				return true;
			string str = value.ToString();
			if (str.Length == 0 || str.Length >= this.MaximumLength)
				return true;
			return false;
		}

		public override Dictionary<string, object> GetValidateAttributes(ModelMetadata metadata, ControllerContext context)
		{
			Dictionary<string, object> attrs = new Dictionary<string, object>();
			attrs.Add("maxlength", this.MaximumLength);
			attrs.Add("data-msg-maxlength", this.FormatErrorMessage(metadata.DisplayName));
			return attrs;
		}
	}
}