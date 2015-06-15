using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EastRiverCommune.Models
{

	public class Result<T>
	{
		public Result()
		{
			this.Code = 0;
		}

		/// <summary> 代号
		/// </summary>
		public int Code { get; set; }

		/// <summary> 数据
		/// </summary>
		public virtual T Data { get; set; }

		public DateTime DateTime { get; set; }
	}

	/// <summary>
	/// </summary>
	public class Result : Result<object>
	{

		public static Result<T> Create<T>(T data, int code = 0)
		{
			return new Result<T>()
			{
				Code = code,
				Data = data,
				DateTime = DateTime.Now.AddSeconds(3)
			};
		}
	}

}