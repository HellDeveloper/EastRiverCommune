using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Swell.Core;
using Swell.Data;

namespace Swell.WebForm
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControlTool
    {
        /// <summary>
        /// data-fieldname
        /// </summary>
        public const string DATA_FIELDNAME = "data-fieldname";

        /// <summary>
        /// 
        /// </summary>
        public const string DATA_DB_TYPE = "data-dbtype";

        /// <summary> 初始化参数
        /// </summary>
        /// <param name="param"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="sourceColumn"></param>
        /// <returns></returns>
        private static IDataParameter initialize_parameter(IDataParameter param, string parameterName, object value, string sourceColumn)
        {
            param.ParameterName = parameterName;
            param.SourceVersion = DataRowVersion.Original;
            param.SourceColumn = sourceColumn.Trim();
			if (String.IsNullOrWhiteSpace(param.GetComparisonOperator()))
				param.SourceColumn += " LIKE";
            param.Value = value;
            return param;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="temp"></param>
        private static void DbType<T>(Control control, T temp) where T : class, IDataParameter
        {
            string dbtype = (control as IAttributeAccessor).GetAttribute(DATA_DB_TYPE);
            if (String.Compare(dbtype, "date", true) == 0 && temp.GetComparisonOperator() == "<=")
                temp.Value += " 23:59:59";
        }

        /// <summary> 获取值
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private static object GetValue(this Control control)
        {
            if (control is ITextControl)
                return (control as ITextControl).Text;
            else if (control is ICheckBoxControl)
                return (control as ICheckBoxControl).Checked;
            else if (control is HtmlInputControl)
                return (control as HtmlInputControl).Value;
            else if (control is HtmlTextArea)
                return (control as HtmlTextArea).Value;
            return String.Empty;
        }

        /// <summary> 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="paramNamePrefix"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static T create_parameter<T>(Control control, char paramNamePrefix, Func<T> func) where T : class, IDataParameter
        {
            if (!(control is IAttributeAccessor))
                return null;
            string fieldname = ControlTool.GetDataFieldName(control);
            if (String.IsNullOrWhiteSpace(fieldname))
                return null;
            object value = GetValue(control);
            var temp = func();
            initialize_parameter(temp, paramNamePrefix + control.ID, value, fieldname);
            DbType(control, temp);
            return temp;
        }

        /// <summary> 创建Parameter
        /// </summary>
        /// <typeparam name="T">创建的类型</typeparam>
        /// <param name="control"></param>
        /// <param name="paramNamePrefix">参数名称的前缀</param>
        /// <param name="check_param">检查参数是否需要</param>
        /// <param name="maxLevel">最大深度</param>
        /// <returns></returns>
        public static List<T> CreateParameters<T>(this Control control, char paramNamePrefix = '@', Func<Control, T, T> check_param = null, int maxLevel = 2) where T : class, IDataParameter, new()
        {
            List<T> list = new List<T>();
            if (0 > maxLevel)
                return list;
            ControlTool.create_parameters<T>(control, paramNamePrefix, list, 1, maxLevel, Assist.CreateParameter<T>, check_param);
            return list;
        }

        /// <summary> 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="paramNamePrefix"></param>
        /// <param name="func"></param>
        /// <param name="maxLevel"></param>
        /// <returns></returns>
        public static List<DataParameter> CreateDataParameters(this Control control, char paramNamePrefix = '@', Func<Control, DataParameter, DataParameter> func = null, int maxLevel = 2) 
        {
            return CreateParameters(control, paramNamePrefix, func, maxLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="paramNamePrefix">参数名称的前缀</param>
        /// <param name="list"></param>
        /// <param name="currentLevel"></param>
        /// <param name="maxLevel"></param>
        /// <param name="create_param"></param>
        /// <param name="check_param"></param>
        private static void create_parameters<T>(Control control, char paramNamePrefix, List<T> list, int currentLevel, int maxLevel, Func<T> create_param, Func<Control, T, T> check_param = null) where T : class, IDataParameter, new()
        {
            T t = ControlTool.create_parameter<T>(control, paramNamePrefix, create_param);
            if (t != null)
			{
				if (check_param == null)
					t = DefaultCheckParameter(control, t);
				else
					t = check_param(control, t);
				if (t != null)
					list.Add(t);
			}
            if (currentLevel < maxLevel)
                foreach (Control ctrl in control.Controls)
                    create_parameters(ctrl, paramNamePrefix, list, currentLevel + 1, maxLevel, create_param, check_param);
        }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="control"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public static T DefaultCheckParameter<T>(Control control, T param) where T : class, IDataParameter
		{
			if (param.Value == null || param.Value.ToString() == String.Empty)
				return null;
			return param;
		}

        /// <summary>
        /// (control as IAttributeAccessor).GetAttribute(DATA_FIELDNAME)
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static string GetDataFieldName(this Control control)
        {
            if (!(control is IAttributeAccessor))
                return null;
            return (control as IAttributeAccessor).GetAttribute(DATA_FIELDNAME);
        }

        /// <summary> 获取字段名
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private static string get_field_name(Control control)
        {
            string fieldname = GetDataFieldName(control);
            return Assist.GetBeforeFirstWhiteSpace(fieldname);
        }

        /// <summary>
        /// attribute.SetAttribute(DATA_FIELDNAME, value);
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetDataFieldName(this Control control, string value)
        {
            IAttributeAccessor attribute = control as IAttributeAccessor;
            if (attribute == null)
                return false;
            attribute.SetAttribute(DATA_FIELDNAME, value);
            return true;
        }

        /// <summary> 填充数据（包括当前控件）
        /// </summary>
        /// <param name="control"></param>
        /// <param name="row">数据行</param>
        /// <param name="after_action">回调函数</param>
        /// <param name="maxLevel">递归的深度</param>
        public static void FillData(this Control control, DataRow row, Action<Control, string, object> after_action = null, int maxLevel = 3)
        {
            if (row == null || row.Table == null)
                return;
            fill_data(control, row, after_action, 1, maxLevel);
        }

        /// <summary> 填充数据（包括当前控件）
        /// </summary>
        /// <param name="control"></param>
        /// <param name="table">数据表</param>
        /// <param name="rowIndex">行索引</param>
        /// <param name="after_action">回调函数</param>
        /// <param name="maxLevel">递归的深度</param>
        public static void FillData(this Control control, DataTable table, int rowIndex = 0, Action<Control, string, object> after_action = null, int maxLevel = 3)
        {
            if (table != null && table.Rows.Count > rowIndex)
                fill_data(control, table.Rows[rowIndex], after_action, 1, maxLevel);
        }

        /// <summary> 填充数据（包括当前控件）
        /// </summary>
        /// <param name="control"></param>
        /// <param name="row"></param>
        /// <param name="after_action"></param>
        /// <param name="currentLevel"></param>
        /// <param name="maxLevel"></param>
        private static void fill_data(Control control, DataRow row, Action<Control, string, object> after_action, int currentLevel, int maxLevel)
        {
            string fieldname = get_field_name(control);
            if (!String.IsNullOrWhiteSpace(fieldname))
            {
                int index = row.Table.Columns.IndexOf(fieldname);
                if (index >= 0)
                {
					//if (row[index] is DateTime)
					//	fill_data(control, fieldname, ((DateTime)row[index]).ToString("yyyy-MM-dd HH:mm:ss"));
					//else
						fill_data(control, fieldname, row[index]);
                    if (after_action != null)
                        after_action.Invoke(control, fieldname, row[index]);
                }
            }
            if (currentLevel < maxLevel)
                foreach (Control ctrl in control.Controls)
                    fill_data(ctrl, row, after_action, currentLevel + 1, maxLevel);
        }

        /// <summary> 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void fill_data(Control control, string name, object value)
        {
            value = value ?? String.Empty;
            if (control is System.Web.UI.ITextControl)
                (control as System.Web.UI.ITextControl).Text = value.ToString();
            else if (control is System.Web.UI.ICheckBoxControl)
                (control as System.Web.UI.ICheckBoxControl).Checked = (value is bool) ? (bool)value : value.TryToString().TryToBool() ?? false;
            else if (control is System.Web.UI.HtmlControls.HtmlInputControl)
                (control as System.Web.UI.HtmlControls.HtmlInputControl).Value = value.ToString();
            else if (control is System.Web.UI.HtmlControls.HtmlContainerControl)
                (control as System.Web.UI.HtmlControls.HtmlContainerControl).InnerHtml = value.ToString();
            else if (control is System.Web.UI.IAttributeAccessor)
                (control as System.Web.UI.IAttributeAccessor).SetAttribute("value", value.ToString());
        }

    }
}
