using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swell.Data
{
	/// <summary> System.Data.DataTable, System.Data.DataColumnCollection 的扩展
	/// </summary>
	public static class EDataTable
	{
		/// <summary> 尝试添加新列名，如果当前列集合包含此名，就不添加返回false
		/// </summary>
		/// <param name="columns">列集合</param>
		/// <param name="name">要添加的列名</param>
		/// <param name="type">类型为null是typeof(string)</param>
		/// <returns>添加是否成功</returns>
		public static bool TryAdd(this DataColumnCollection columns, string name, Type type = null)
		{
			if (columns.Contains(name))
				return false;
			if (type == null)
				columns.Add(name, typeof(string));
			else
				columns.Add(name, type);
			return true;
		}


	}
}
