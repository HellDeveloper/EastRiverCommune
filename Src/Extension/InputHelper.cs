using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Properties;
using System.Web.Routing;

namespace EastRiverCommune.Extension
{
	public static class InputHelper
	{
		#region Methods
		internal static string EvalString(HtmlHelper helper, string key)
		{
			return Convert.ToString(helper.ViewData.Eval(key), CultureInfo.CurrentCulture);
		}

		internal static string EvalString(HtmlHelper helper, string key, string format)
		{
			return Convert.ToString(helper.ViewData.Eval(key, format), CultureInfo.CurrentCulture);
		}

		internal static bool EvalBoolean(HtmlHelper helper, string key)
		{
			return Convert.ToBoolean(helper.ViewData.Eval(key), CultureInfo.InvariantCulture);
		}

		internal static object GetModelStateValue(HtmlHelper helper, string key, Type destinationType)
		{
			ModelState modelState;
			if (helper.ViewData.ModelState.TryGetValue(key, out modelState))
			{
				if (modelState.Value != null)
				{
					return modelState.Value.ConvertTo(destinationType, null /* culture */);
				}
			}
			return null;
		}

		private static RouteValueDictionary ToRouteValueDictionary(IDictionary<string, object> dictionary)
		{
			return dictionary == null ? new RouteValueDictionary() : new RouteValueDictionary(dictionary);
		}

		internal static IEnumerable<ModelClientValidationRule> ClientValidationRuleFactory(HtmlHelper helper, string name, ModelMetadata metadata)
		{
			return ModelValidatorProviders.Providers.GetValidators(metadata ?? ModelMetadata.FromStringExpression(name, helper.ViewData), helper.ViewContext).SelectMany(v => v.GetClientValidationRules());
		}
		#endregion

		#region VCheckBox
		public static MvcHtmlString VCheckBox(this HtmlHelper htmlHelper, string name)
		{
			return VCheckBox(htmlHelper, name, htmlAttributes: (object)null);
		}

		public static MvcHtmlString VCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked)
		{
			return VCheckBox(htmlHelper, name, isChecked, htmlAttributes: (object)null);
		}

		public static MvcHtmlString VCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, object htmlAttributes)
		{
			return VCheckBox(htmlHelper, name, isChecked, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VCheckBox(this HtmlHelper htmlHelper, string name, object htmlAttributes)
		{
			return VCheckBox(htmlHelper, name, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VCheckBox(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
		{
			return _VCheckBoxHelper(htmlHelper, metadata: null, name: name, isChecked: null, htmlAttributes: htmlAttributes);
		}

		public static MvcHtmlString VCheckBox(this HtmlHelper htmlHelper, string name, bool isChecked, IDictionary<string, object> htmlAttributes)
		{
			return _VCheckBoxHelper(htmlHelper, metadata: null, name: name, isChecked: isChecked, htmlAttributes: htmlAttributes);
		}

		//[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
		{
			return VCheckBoxFor(htmlHelper, expression, htmlAttributes: null);
		}

		//[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes)
		{
			return VCheckBoxFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, IDictionary<string, object> htmlAttributes)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			bool? isChecked = null;
			if (metadata.Model != null)
			{
				bool modelChecked;
				if (Boolean.TryParse(metadata.Model.ToString(), out modelChecked))
				{
					isChecked = modelChecked;
				}
			}

			return _VCheckBoxHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), isChecked, htmlAttributes);
		}

		private static MvcHtmlString _VCheckBoxHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, bool? isChecked, IDictionary<string, object> htmlAttributes)
		{
			RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);

			bool explicitValue = isChecked.HasValue;
			if (explicitValue)
			{
				attributes.Remove("checked"); // Explicit value must override dictionary
			}

			return _InputHelper(htmlHelper,
							   InputType.CheckBox,
							   metadata,
							   name,
							   value: "true",
							   useViewData: !explicitValue,
							   isChecked: isChecked ?? false,
							   setId: true,
							   isExplicitValue: false,
							   format: null,
							   htmlAttributes: attributes);
		}
		#endregion

		#region VHidden
		public static MvcHtmlString VHidden(this HtmlHelper htmlHelper, string name)
		{
			return VHidden(htmlHelper, name, value: null, htmlAttributes: null);
		}

		public static MvcHtmlString VHidden(this HtmlHelper htmlHelper, string name, object value)
		{
			return VHidden(htmlHelper, name, value, htmlAttributes: null);
		}

		public static MvcHtmlString VHidden(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			return VHidden(htmlHelper, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VHidden(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
		{
			return _VHiddenHelper(htmlHelper,
								metadata: null,
								value: value,
								useViewData: value == null,
								expression: name,
								htmlAttributes: htmlAttributes);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return VHiddenFor(htmlHelper, expression, (IDictionary<string, object>)null);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return VHiddenFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _VHiddenHelper(htmlHelper,
								metadata,
								metadata.Model,
								false,
								ExpressionHelper.GetExpressionText(expression),
								htmlAttributes);
		}

		private static MvcHtmlString _VHiddenHelper(HtmlHelper htmlHelper, ModelMetadata metadata, object value, bool useViewData, string expression, IDictionary<string, object> htmlAttributes)
		{
			Binary binaryValue = value as Binary;
			if (binaryValue != null)
			{
				value = binaryValue.ToArray();
			}

			byte[] byteArrayValue = value as byte[];
			if (byteArrayValue != null)
			{
				value = Convert.ToBase64String(byteArrayValue);
			}

			return _InputHelper(htmlHelper,
							   InputType.Hidden,
							   metadata,
							   expression,
							   value,
							   useViewData,
							   isChecked: false,
							   setId: true,
							   isExplicitValue: true,
							   format: null,
							   htmlAttributes: htmlAttributes);
		}
		#endregion

		#region VPassword
		// VPassword
		public static MvcHtmlString VPassword(this HtmlHelper htmlHelper, string name)
		{
			return VPassword(htmlHelper, name, value: null);
		}

		public static MvcHtmlString VPassword(this HtmlHelper htmlHelper, string name, object value)
		{
			return VPassword(htmlHelper, name, value, htmlAttributes: null);
		}

		public static MvcHtmlString VPassword(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			return VPassword(htmlHelper, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VPassword(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
		{
			return _VPasswordHelper(htmlHelper, metadata: null, name: name, value: value, htmlAttributes: htmlAttributes);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return VPasswordFor(htmlHelper, expression, htmlAttributes: null);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return VPasswordFor(htmlHelper, expression, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		[SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Users cannot use anonymous methods with the LambdaExpression type")]
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			return _VPasswordHelper(htmlHelper,
								  ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData),
								  ExpressionHelper.GetExpressionText(expression),
								  value: null,
								  htmlAttributes: htmlAttributes);
		}

		private static MvcHtmlString _VPasswordHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, object value, IDictionary<string, object> htmlAttributes)
		{
			return _InputHelper(htmlHelper,
							   InputType.Password,
							   metadata,
							   name,
							   value,
							   useViewData: false,
							   isChecked: false,
							   setId: true,
							   isExplicitValue: true,
							   format: null,
							   htmlAttributes: htmlAttributes);
		}
		#endregion

		#region VRadioButton

		public static MvcHtmlString VRadioButton(this HtmlHelper htmlHelper, string name, object value)
		{
			return VRadioButton(htmlHelper, name, value, htmlAttributes: (object)null);
		}

		public static MvcHtmlString VRadioButton(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			return VRadioButton(htmlHelper, name, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VRadioButton(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
		{
			// Determine whether or not to render the checked attribute based on the contents of ViewData.
			string valueString = Convert.ToString(value, CultureInfo.CurrentCulture);
			bool isChecked = (!String.IsNullOrEmpty(name)) && (String.Equals(EvalString(htmlHelper, name), valueString, StringComparison.OrdinalIgnoreCase));
			// checked attributes is implicit, so we need to ensure that the dictionary takes precedence.
			RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);
			if (attributes.ContainsKey("checked"))
			{
				return _InputHelper(htmlHelper,
								   InputType.Radio,
								   metadata: null,
								   name: name,
								   value: value,
								   useViewData: false,
								   isChecked: false,
								   setId: true,
								   isExplicitValue: true,
								   format: null,
								   htmlAttributes: attributes);
			}

			return VRadioButton(htmlHelper, name, value, isChecked, htmlAttributes);
		}

		public static MvcHtmlString VRadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked)
		{
			return VRadioButton(htmlHelper, name, value, isChecked, htmlAttributes: (object)null);
		}

		public static MvcHtmlString VRadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, object htmlAttributes)
		{
			return VRadioButton(htmlHelper, name, value, isChecked, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VRadioButton(this HtmlHelper htmlHelper, string name, object value, bool isChecked, IDictionary<string, object> htmlAttributes)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			// checked attribute is an explicit parameter so it takes precedence.
			RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);
			attributes.Remove("checked");
			return _InputHelper(htmlHelper,
							   InputType.Radio,
							   metadata: null,
							   name: name,
							   value: value,
							   useViewData: false,
							   isChecked: isChecked,
							   setId: true,
							   isExplicitValue: true,
							   format: null,
							   htmlAttributes: attributes);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value)
		{
			return VRadioButtonFor(htmlHelper, expression, value, htmlAttributes: null);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes)
		{
			return VRadioButtonFor(htmlHelper, expression, value, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _VRadioButtonHelper(htmlHelper,
									 metadata,
									 metadata.Model,
									 ExpressionHelper.GetExpressionText(expression),
									 value,
									 null /* isChecked */,
									 htmlAttributes);
		}

		private static MvcHtmlString _VRadioButtonHelper(HtmlHelper htmlHelper, ModelMetadata metadata, object model, string name, object value, bool? isChecked, IDictionary<string, object> htmlAttributes)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			RouteValueDictionary attributes = ToRouteValueDictionary(htmlAttributes);

			bool explicitValue = isChecked.HasValue;
			if (explicitValue)
			{
				attributes.Remove("checked"); // Explicit value must override dictionary
			}
			else
			{
				string valueString = Convert.ToString(value, CultureInfo.CurrentCulture);
				isChecked = model != null &&
							!String.IsNullOrEmpty(name) &&
							String.Equals(model.ToString(), valueString, StringComparison.OrdinalIgnoreCase);
			}

			return _InputHelper(htmlHelper,
							   InputType.Radio,
							   metadata,
							   name,
							   value,
							   useViewData: false,
							   isChecked: isChecked ?? false,
							   setId: true,
							   isExplicitValue: true,
							   format: null,
							   htmlAttributes: attributes);
		}
		#endregion

		#region VTextBox
		public static MvcHtmlString VTextBox(this HtmlHelper htmlHelper, string name)
		{
			return VTextBox(htmlHelper, name, value: null);
		}

		public static MvcHtmlString VTextBox(this HtmlHelper htmlHelper, string name, object value)
		{
			return VTextBox(htmlHelper, name, value, format: null);
		}

		public static MvcHtmlString VTextBox(this HtmlHelper htmlHelper, string name, object value, string format)
		{
			return VTextBox(htmlHelper, name, value, format, htmlAttributes: (object)null);
		}

		public static MvcHtmlString VTextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes)
		{
			return VTextBox(htmlHelper, name, value, format: null, htmlAttributes: htmlAttributes);
		}

		public static MvcHtmlString VTextBox(this HtmlHelper htmlHelper, string name, object value, string format, object htmlAttributes)
		{
			return VTextBox(htmlHelper, name, value, format, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		public static MvcHtmlString VTextBox(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes)
		{
			return VTextBox(htmlHelper, name, value, format: null, htmlAttributes: htmlAttributes);
		}

		public static MvcHtmlString VTextBox(this HtmlHelper htmlHelper, string name, object value, string format, IDictionary<string, object> htmlAttributes)
		{
			return _InputHelper(htmlHelper,
							   InputType.Text,
							   metadata: null,
							   name: name,
							   value: value,
							   useViewData: (value == null),
							   isChecked: false,
							   setId: true,
							   isExplicitValue: true,
							   format: format,
							   htmlAttributes: htmlAttributes);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return htmlHelper.VTextBoxFor(expression, format: null);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
		{
			return htmlHelper.VTextBoxFor(expression, format, (IDictionary<string, object>)null);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			return htmlHelper.VTextBoxFor(expression, format: null, htmlAttributes: htmlAttributes);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes)
		{
			return htmlHelper.VTextBoxFor(expression, format: format, htmlAttributes: HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			return htmlHelper.VTextBoxFor(expression, format: null, htmlAttributes: htmlAttributes);
		}

		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an appropriate nesting of generic types")]
		public static MvcHtmlString VTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _VTextBoxHelper(htmlHelper,
								 metadata,
								 metadata.Model,
								 ExpressionHelper.GetExpressionText(expression),
								 format,
								 htmlAttributes);
		}

		private static MvcHtmlString _VTextBoxHelper(this HtmlHelper htmlHelper, ModelMetadata metadata, object model, string expression, string format, IDictionary<string, object> htmlAttributes)
		{
			return _InputHelper(htmlHelper,
							   InputType.Text,
							   metadata,
							   expression,
							   model,
							   useViewData: false,
							   isChecked: false,
							   setId: true,
							   isExplicitValue: true,
							   format: format,
							   htmlAttributes: htmlAttributes);
		}
		#endregion

		// Helper methods

		private static MvcHtmlString _InputHelper(HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string format, IDictionary<string, object> htmlAttributes)
		{
			string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			if (String.IsNullOrEmpty(fullName))
			{
				throw new ArgumentException("error", "name");
			}

			TagBuilder tagBuilder = new TagBuilder("input");
			tagBuilder.MergeAttributes(htmlAttributes);
			tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
			tagBuilder.MergeAttribute("name", fullName, true);

			string valueParameter = htmlHelper.FormatValue(value, format);
			bool usedModelState = false;

			switch (inputType)
			{
				case InputType.CheckBox:
					bool? modelStateWasChecked = GetModelStateValue(htmlHelper, fullName, typeof(bool)) as bool?;
					if (modelStateWasChecked.HasValue)
					{
						isChecked = modelStateWasChecked.Value;
						usedModelState = true;
					}
					goto case InputType.Radio;
				case InputType.Radio:
					if (!usedModelState)
					{
						string modelStateValue = GetModelStateValue(htmlHelper, fullName, typeof(string)) as string;
						if (modelStateValue != null)
						{
							isChecked = String.Equals(modelStateValue, valueParameter, StringComparison.Ordinal);
							usedModelState = true;
						}
					}
					if (!usedModelState && useViewData)
					{
						isChecked = EvalBoolean(htmlHelper, fullName);
					}
					if (isChecked)
					{
						tagBuilder.MergeAttribute("checked", "checked");
					}
					tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
					break;
				case InputType.Password:
					if (value != null)
					{
						tagBuilder.MergeAttribute("value", valueParameter, isExplicitValue);
					}
					break;
				default:
					string attemptedValue = (string)GetModelStateValue(htmlHelper, fullName, typeof(string));
					tagBuilder.MergeAttribute("value", attemptedValue ?? ((useViewData) ? EvalString(htmlHelper, fullName, format) : valueParameter), isExplicitValue);
					break;
			}

			if (setId)
			{
				tagBuilder.GenerateId(fullName);
			}

			// If there are any errors for a named field, we add the css attribute.
			ModelState modelState;
			if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
			{
				if (modelState.Errors.Count > 0)
				{
					tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
				}
			}

			tagBuilder.MergeAttributes(GetValidationAttributes(htmlHelper, name, metadata));

			if (inputType == InputType.CheckBox)
			{
				// Render an additional <input type="VHidden".../> for VCheckBoxes. This
				// addresses scenarios where unchecked VCheckBoxes are not sent in the request.
				// Sending a VHidden input makes it possible to know that the VCheckBox was present
				// on the page when the request was submitted.
				StringBuilder inputItemBuilder = new StringBuilder();
				inputItemBuilder.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));

				TagBuilder VHiddenInput = new TagBuilder("input");
				VHiddenInput.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
				VHiddenInput.MergeAttribute("name", fullName);
				VHiddenInput.MergeAttribute("value", "false");
				inputItemBuilder.Append(VHiddenInput.ToString(TagRenderMode.SelfClosing));
				return MvcHtmlString.Create(inputItemBuilder.ToString());
			}

			return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
		}


		public static IDictionary<string, object> GetValidationAttributes(this HtmlHelper html_helper, string name, ModelMetadata metadata)
		{
			Dictionary<string, object> results = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

			// The ordering of these 3 checks (and the early exits) is for performance reasons.
			//if (!ViewContext.UnobtrusiveJavaScriptEnabled)
			//{
			//	return results;
			//}

			FormContext formContext = html_helper.ViewContext.ClientValidationEnabled ? html_helper.ViewContext.FormContext : null;
			if (formContext == null)
			{
				return results;
			}

			string fullName = html_helper.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			if (formContext.RenderedField(fullName))
			{
				return results;
			}

			formContext.RenderedField(fullName, true);

			//IEnumerable<ModelClientValidationRule> clientRules = ClientValidationRuleFactory(html_helper, name, metadata);
			//ValidationAttributesGenerator.GetValidationAttributes(clientRules, results);

			AddValidateAttributes(results, metadata.ContainerType, metadata, html_helper.ViewContext);
			foreach (var type in metadata.ContainerType.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MetadataTypeAttribute), true))
			{
				AddValidateAttributes(results, (type as System.ComponentModel.DataAnnotations.MetadataTypeAttribute).MetadataClassType, metadata, html_helper.ViewContext);
			}

			//foreach (var item in metadata.ContainerType.)
			//{

			//}

			return results;
		}

		public static void AddValidateAttributes(Dictionary<string, object> dictionary, Type type, ModelMetadata metadata, ControllerContext controller_context)
		{
			var property = type.GetProperty(metadata.PropertyName);
			if (property == null)
				return;
			foreach (var item in property.GetCustomAttributes(typeof(ValidationAttribute), true))
			{
				foreach (var t in (item as ValidationAttribute).GetValidateAttributes(metadata, controller_context))
					dictionary[t.Key] = t.Value;
			}
		}

		public static MvcHtmlString InputTextAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, format, htmlAttributes);
		}

		public static MvcHtmlString InputTextAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, null, htmlAttributes);
		}

		public static MvcHtmlString InputTextAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, format, null);
		}

		public static MvcHtmlString InputTextAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, null, null);
		}

		private static MvcHtmlString _InputAttributesHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, object value, string format, IDictionary<string, object> htmlAttributes)
		{
			string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			if (String.IsNullOrEmpty(fullName))
			{
				throw new ArgumentException("error", "name");
			}
			IDictionary<string, object> attributes = GetValidationAttributes(htmlHelper, name, metadata);
			attributes["name"] = fullName;
			attributes["value"] = htmlHelper.FormatValue(value, format);
			attributes["id"] = TagBuilder.CreateSanitizedId(fullName);

			// If there are any errors for a named field, we add the css attribute.
			//ModelState modelState;
			//if (htmlHelper.ViewData.ModelState.TryGetValue(fullName, out modelState))
			//{
			//	if (modelState.Errors.Count > 0)
			//	{
			//		attributes["class"] =  HtmlHelper.ValidationInputCssClassName;
			//	}
			//}

			//tagBuilder.MergeAttributes(GetValidationAttributes(htmlHelper, name, metadata));
			var builder = FormatAttributes(null, attributes);
			builder = FormatAttributes(builder, htmlAttributes);
			return new MvcHtmlString(builder.ToString());
		}

		public static StringBuilder FormatAttributes(StringBuilder builder, IDictionary<string, object> attributes)
		{
			if (builder == null)
				builder = new StringBuilder().Append(" ");
			if (attributes == null)
				return builder;
			string format = @"{0}=""{1}"" ";
			foreach (var attr in attributes)
			{
				if (attr.Value is string)
					builder.AppendFormat(format, attr.Key, HttpUtility.HtmlAttributeEncode(attr.Value as string));
				else
					builder.AppendFormat(format, attr.Key, attr.Value);
			}
			return builder;
		}

		public static string HtmlAttributeEncode(object o)
		{
			return "";
		}

	}
}