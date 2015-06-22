using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Swell.Core;

namespace Swell.WebForm
{
    /// <summary>
    /// 
    /// </summary>
    public static class EListItemCollection
    {
		/// <summary> 
		/// </summary>
		/// <param name="lic"></param>
		/// <param name="arg"></param>
		public static void Add(this ListItemCollection lic, object arg)
		{
			var str = arg.TryToString() ?? String.Empty;
			lic.Add(new ListItem() { Text = str, Value = str });
		}

        /// <summary>
        /// 添加ListItem ----- new ListItem() { Text = text, Value = value }
        /// </summary>
        /// <param name="lic"></param>
        /// <param name="text"></param>
        /// <param name="value"></param>
        public static void Add(this ListItemCollection lic, object text, object value)
        {
			lic.Add(new ListItem() { Text = text.TryToString() ?? String.Empty, Value = value.TryToString() ?? String.Empty });
        }

        /// <summary>
        /// item是Text也是Value
        /// </summary>
        /// <param name="lic"></param>
        /// <param name="args"></param>
        public static void AddRange<T>(this ListItemCollection lic, IEnumerable<T> args) 
        {
            foreach (var item in args)
            {
                EListItemCollection.Add(lic, item);
            }
        }

        /// <summary>
        /// Key=Text, Value=Value
        /// </summary>
        /// <typeparam name="Key"></typeparam>
        /// <typeparam name="Value"></typeparam>
        /// <param name="lic"></param>
        /// <param name="dic"></param>
        public static void AddRange<Key, Value>(this ListItemCollection lic, Dictionary<Key, Value> dic)
        {
            foreach (var item in dic)
                EListItemCollection.Add(lic, item.Key, item.Value); 
        }

    }
}
