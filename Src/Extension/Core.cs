using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EastRiverCommune.Extension
{
	public class Core
	{
		/// <summary> 调用value.ToString(format)
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public static object FormatToString(object value, string format)
		{
			if (value == null || String.IsNullOrWhiteSpace(format))
				return value;
			Type[] param_types = {typeof(string)};
			object[] args = { format }; 
			var method = value.GetType().GetMethod("ToString", param_types);
			if (method != null && method.ReturnType != null)
				return method.Invoke(value, args);
			return value;
		}

		public static bool Has(System.Collections.IEnumerable enumerable)
		{
			if (enumerable == null)
				return false;
			foreach (var item in enumerable)
				return true;
			return false;
		}

	}
}