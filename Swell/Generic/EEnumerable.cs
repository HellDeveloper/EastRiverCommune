using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swell.Core;

namespace Swell.Generic
{
	/// <summary>
	/// 
	/// </summary>
	public static partial class EEnumerable
	{
		/// <summary> 升序排序 IComparable接口 CompareTo
		/// </summary>
		/// <typeparam name="TSource">实现IComparable接口</typeparam>
		/// <param name="source">排序的数据源</param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source) where TSource : IComparable
		{
			return source.OrderBy(Assist.ReturnParameter, new Comparable<TSource>());
		}

		/// <summary> 升序排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source">排序的数据源</param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, int> callback)
		{
			return source.OrderBy(Assist.ReturnParameter, new Comparer<TSource>(callback));
		}

		/// <summary> 升序排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source">排序的数据源</param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
		{
			return source.OrderBy(Assist.ReturnParameter, comparer);
		}

		/// <summary> 升序排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source">排序的数据源</param>
		/// <param name="keySelector"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> callback)
		{
			return source.OrderBy(keySelector, new Comparer<TKey>(callback));
		}


		/// <summary> 降序排序 IComparable接口 CompareTo
		/// </summary>
		/// <typeparam name="TSource">实现IComparable接口</typeparam>
		/// <param name="source">排序的数据源</param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source) where TSource : IComparable
		{
			return source.OrderByDescending(Assist.ReturnParameter, new Comparable<TSource>());
		}

		/// <summary> 降序排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source">排序的数据源</param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, int> callback)
		{
			return source.OrderByDescending(Assist.ReturnParameter, new Comparer<TSource>(callback));
		}

		/// <summary> 降序排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source">排序的数据源</param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
		{
			return source.OrderByDescending(Assist.ReturnParameter, comparer);
		}

		/// <summary> 降序排序
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="source">排序的数据源</param>
		/// <param name="keySelector"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> callback)
		{
			return source.OrderByDescending(keySelector, new Comparer<TKey>(callback));
		}

		class BuilderFuncConvert<T, V>
		{
			public static readonly Func<int, T, V> Null = null;

			public BuilderFuncConvert(Func<T, V> func)
			{
				this._func = func;
			}

			private Func<T, V> _func;

			public V Func(int index, T t)
			{
				return this._func(t);
			}
		}

		private static StringBuilder BuilderAppend<T>(StringBuilder builder, T temp, string separator)
		{
			if (temp == null)
				return builder;
			else if (builder == null)
				builder = new StringBuilder();
			else
				builder.Append(separator);
			builder.Append(temp);
			return builder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="args"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static StringBuilder ToStringBuilder<T>(this IEnumerable<T> args, string separator = ", ")
		{
			return ToStringBuilder(args, BuilderFuncConvert<T, string>.Null, separator);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static StringBuilder ToStringBuilder<T, V>(this IEnumerable<T> args, Func<T, V> func, string separator = ", ")
		{
			if (func == null)
				return ToStringBuilder(args, BuilderFuncConvert<T, V>.Null, separator);
			BuilderFuncConvert<T, V> convert = new BuilderFuncConvert<T, V>(func);
			return ToStringBuilder(args, convert.Func, separator);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static StringBuilder ToStringBuilder<T, V>(this IEnumerable<T> args, Func<int, T, V> func, string separator = ", ")
		{
			StringBuilder builder = null;
			if (args == null)
				return builder;
			int index = -1;
			foreach (var item in args)
			{
				index++;
				if (item == null)
					continue;
				if (func == null)
					builder = BuilderAppend(builder, item.ToString(), separator);
				else
					builder = BuilderAppend(builder, func(index, item), separator);
			}
			return builder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="K"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="args"></param>
		/// <param name="func_key"></param>
		/// <param name="func_value"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static Tuple<StringBuilder, StringBuilder> ToStringBuilder<T, K, V>(IEnumerable<T> args, Func<T, K> func_key, Func<T, V> func_value, string separator = ", ")
		{
			if (args == null)
				return new Tuple<StringBuilder, StringBuilder>(new StringBuilder(), new StringBuilder());
			Tuple<StringBuilder, StringBuilder> tuple = null;
			foreach (var item in args)
			{
				if (item == null)
					continue;
				var fieldname = func_key(item);
				if (fieldname == null)
					continue;
				if (tuple == null)
				{
					tuple = new Tuple<StringBuilder, StringBuilder>(new StringBuilder(), new StringBuilder());
				}
				else
				{
					tuple.Item1.Append(separator);
					tuple.Item2.Append(separator);
				}
				tuple.Item1.Append(fieldname);
				tuple.Item2.Append(func_value(item));
			}
			if (tuple == null)
				return new Tuple<StringBuilder, StringBuilder>(new StringBuilder(), new StringBuilder());
			return new Tuple<StringBuilder, StringBuilder>(tuple.Item1, tuple.Item2);
		}


	}
}
