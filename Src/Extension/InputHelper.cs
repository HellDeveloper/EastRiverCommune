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
		private static MvcHtmlString _InputAttributesHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, object value, Func<object, string> format, IDictionary<string, object> htmlAttributes)
		{
			string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
			if (String.IsNullOrEmpty(fullName))
			{
				throw new ArgumentException("error", "name");
			}
			IDictionary<string, object> attributes = GetValidationAttributes(htmlHelper, name, metadata);
			attributes["name"] = fullName;
			if (format == null)
			{
				attributes["value"] = value;
			}
			else
			{
				attributes["value"] = format(value);
			}
			 //htmlHelper.FormatValue(value, format);
			attributes["id"] = TagBuilder.CreateSanitizedId(fullName);
			var builder = ToHtmlAttributes(null, attributes);
			builder = ToHtmlAttributes(builder, htmlAttributes);
			return new MvcHtmlString(builder.ToString());
		}

		private static MvcHtmlString _SelectOptionsHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, object value, System.Collections.IEnumerable options)
		{
			StringBuilder builder =new StringBuilder();
			foreach (var item in _CreateSelectOptions(options, value))
			{
				builder.Append(item.ToString()).AppendLine();
			}
			return MvcHtmlString.Create(builder.ToString());
		}

		private static TagBuilder _CreateSelectOption(string text, object value, bool is_selected)
		{
			TagBuilder tagBuilder = new TagBuilder("option");
			tagBuilder.Attributes["value"] = value == null ? String.Empty : value.ToString();
			if (is_selected)
				tagBuilder.Attributes["selected"] = "selected";
			tagBuilder.SetInnerText(text);
			return tagBuilder;
		}

		private static List<TagBuilder> _CreateSelectOptions(System.Collections.IEnumerable options, object selected_value)
		{
			List<TagBuilder> list = new List<TagBuilder>();
			if (options is IEnumerable<KeyValuePair<string, object>>)
			{
				foreach (var item in (options as IEnumerable<KeyValuePair<string, object>>))
					list.Add(_CreateSelectOption(item.Key, item.Value, Object.Equals(selected_value, item)));
			}
			else 
			{
				foreach (var item in options)
					list.Add(_CreateSelectOption(item == null ? String.Empty : item.ToString(), item, Object.Equals(selected_value, item)));
			}
			return list;
		}

		public static StringBuilder ToHtmlAttributes(StringBuilder builder, IDictionary<string, object> attributes)
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

		internal static object GetModelStateValue(HtmlHelper html_helper, string key, Type destinationType)
		{
			ModelState modelState;
			if (html_helper.ViewData.ModelState.TryGetValue(key, out modelState) && modelState.Value != null)
				return modelState.Value.ConvertTo(destinationType, null);
			return null;
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

		public static IDictionary<string, object> GetValidationAttributes(this HtmlHelper html_helper, string name, ModelMetadata metadata)
		{
			Dictionary<string, object> results = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
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

			AddValidateAttributes(results, metadata.ContainerType, metadata, html_helper.ViewContext);
			foreach (var type in metadata.ContainerType.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MetadataTypeAttribute), true))
			{
				AddValidateAttributes(results, (type as System.ComponentModel.DataAnnotations.MetadataTypeAttribute).MetadataClassType, metadata, html_helper.ViewContext);
			}

			return results;
		}

		/// <summary> id name value Validate
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression"></param>
		/// <param name="format"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public static MvcHtmlString InputAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Func<object, string> format, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, format, htmlAttributes);
		}

		/// <summary> id name value Validate
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression"></param>
		/// <param name="format"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public static MvcHtmlString InputAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, null, htmlAttributes);
		}

		/// <summary> id name value Validate
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression"></param>
		/// <param name="format"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public static MvcHtmlString InputAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, Func<object, string> format)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, format, null);
		}

		/// <summary> id name value Validate
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression"></param>
		/// <param name="format"></param>
		/// <param name="htmlAttributes"></param>
		/// <returns></returns>
		public static MvcHtmlString InputAttributesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _InputAttributesHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, null, null);
		}

		/// <summary> option
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="htmlHelper"></param>
		/// <param name="expression"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static MvcHtmlString SelectOptionsFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, System.Collections.IEnumerable options)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			return _SelectOptionsHelper(htmlHelper, metadata, ExpressionHelper.GetExpressionText(expression), metadata.Model, options);
		}

	}
}