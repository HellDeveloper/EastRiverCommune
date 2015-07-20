using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EastRiverCommune.Extension
{
	/// <summary> 
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
	public class LessThanAttribute : ValidationAttribute
	{
		public LessThanAttribute(string member, bool equal = true)
		{
			this.MemberName = member;
			this.CanEqual = equal;
		}

		public string MemberName { get; set; }

		public bool CanEqual { get; set; }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (this.IsValid(value))
				return ValidationResult.Success;
			return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
		}

		public override string FormatErrorMessage(string name)
		{
			if (String.IsNullOrWhiteSpace(this.ErrorMessage))
				this.ErrorMessage = "{0}大于{1}";
			return String.Format(this.ErrorMessage, name, this.MemberName);
		}

		public override bool IsValid(object value)
		{
			//if (value == null)
			//	return true;
			//string str = value.ToString();
			//if (str.Length == 0 || str.Length >= this.MaximumLength)
			//	return true;
			return false;
		}

		public override Dictionary<string, object> GetValidateAttributes(ModelMetadata metadata, ControllerContext context)
		{
			Dictionary<string, object> attrs = new Dictionary<string, object>();
			attrs.Add("GreaterThan", this.MemberName);
			attrs.Add("data-msg-GreaterThan", this.FormatErrorMessage(metadata.DisplayName));
			return attrs;
		}
	}
}