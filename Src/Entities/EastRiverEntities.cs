﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EastRiverCommune.Entities
{
	public partial class EastRiverEntities
	{
		

		/// <summary> 获取分类列表
		/// </summary>
        /// <param name="category_type">分类类型</param>
		/// <param name="enable">是否启用</param>
		/// <returns></returns>
		public IEnumerable<Category> GetCategories(CategoryType? category_type, bool? enable = true)
		{
			var query = from category in this.Categories select category;
            if (category_type.HasValue)
            {
                query = from category in query
                        where category.Type == (int)category_type.Value
                        select category;
            }
			if (enable.HasValue)
			{
				query = from category in query
						where category.Enable == enable.Value
						select category;
			}
			return query;
		}

		/// <summary> 获取商品列表
		/// </summary>
		/// <param name="enable">是否启用</param>
		/// <returns></returns>
		public IEnumerable<Commodity> GetCommodities(bool? enable = true)
		{
			var query = from commodity in this.Commodities select commodity;
			if (enable.HasValue)
			{
				query = from commodity in query
						where commodity.Enable == enable.Value
						select commodity;
			}
			return query;
		}

		/// <summary> 获取商品列表
		/// </summary>
		/// <param name="when_modify">大于</param>
		/// <returns></returns>
		public IEnumerable<Commodity> GetCommodities(DateTime? when_modify)
		{
			var query = from commodity in this.Commodities select commodity;
			if (when_modify.HasValue)
			{
				query = from commodity in query
						where commodity.WhenModify > when_modify.Value
						select commodity;
			}
			return query;
		}

		/// <summary> 获取商品的描述
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public string GetCommodityDescription(string id)
		{
			string sql = "SELECT [Description] FROM Commodity WHERE [ID] = @p0";
			using (var cmd = this.Database.Connection.CreateCommand())
			{
				cmd.CommandText = sql;
				var arg = cmd.CreateParameter();
				arg.ParameterName = "@p0";
				arg.Value = id;
				cmd.Parameters.Add(arg);
				if (cmd.Connection.State == System.Data.ConnectionState.Broken)
					cmd.Connection.Close();
				if (cmd.Connection.State == System.Data.ConnectionState.Closed)
					cmd.Connection.Open();
				var temp = cmd.ExecuteScalar() as string;
				cmd.Connection.Close();
				return temp;
			}
		}

		/// <summary> 获取目录路径
		/// </summary>
		/// <param name="enable"></param>
		/// <returns></returns>
		public string GetDirectoryPath(string name, bool? enable = true)
		{
			IEnumerable<string> temp = null;
			if (enable.HasValue)
			{
				string sql = "SELECT [Path] FROM Directory WHERE [Name] = @p0 AND [Enable] = @p1";
				temp = this.Database.SqlQuery<string>(sql, name, enable);
			}
			else
			{
				string sql = "SELECT [Path] FROM Directory WHERE [Name] = @p0";
				temp = this.Database.SqlQuery<string>(sql, name);
			}
			foreach (var item in temp)
				return item;
			return String.Empty;
		}

		/// <summary> 删除 禁用的商品
		/// </summary>
		/// <param name="orderListID"></param>
		public int Reinitialize(string orderListID)
		{
			string sql =
@"DELETE FROM OrderItem WHERE OrderListID = @p0
AND EXISTS (SELECT * FROM Commodity WHERE ID = CommodityID AND [Enable] = 0) ";
			int deleted = this.Database.ExecuteSqlCommand(sql, orderListID);
			sql =
@"UPDATE OrderItem SET 
Price = (SELECT Price FROM Commodity WHERE Commodity.ID = CommodityID)
WHERE OrderListID = @p0";
			this.Database.ExecuteSqlCommand(sql, orderListID);
			sql =
@"UPDATE OrderItem SET 
Total = Price * Count
WHERE OrderListID = @p0";
			this.Database.ExecuteSqlCommand(sql, orderListID);
			return deleted;
		}

		/// <summary>
		/// 核实、核对 OrderItem
		/// </summary>
		/// <param name="merchandises"></param>
		/// <returns></returns>
		public List<OrderItem> Verify(IEnumerable<OrderItem> merchandises)
		{
			string sql = "SELECT ID as CommodityID, Name as CommodityName, Enable, Price, Unit, ";
			string cases = "(CASE ID ";
			string where = " ELSE 1 END) AS Count FROM Commodity WHERE 1<>1";
			var args = new List<string>();
			foreach (var item in merchandises)
			{
				cases += " WHEN @p" + args.Count + " THEN " + item.Count;
				where += " OR ID = @p" + args.Count;
				args.Add(item.CommodityID);
			}
			sql += cases + where;
			return this.ExecuteReader(this.Verify, sql, args);
		}

		public List<T> ExecuteReader<T>(Func<IDataReader, T> read, string sql, IEnumerable<object> args)
		{
			List<T> list = new List<T>();
			var cmd = this.Database.Connection.CreateCommand();
			cmd.CommandText = sql;
			this.AddParameters(cmd, args);
			this.OpenConnection();
			var reader = cmd.ExecuteReader();
			while (reader.Read())
				list.Add(read(reader));
			reader.Close();
			this.CloseConnection();
			return list;
		}

		protected OrderItem Verify(IDataReader reader)
		{
			OrderItem order_item = new OrderItem()
			{
				CommodityID = reader["CommodityID"] as string,
				//Price = (decimal)reader["Price"],
				Count = (int)reader["Count"]
			};

			order_item.Commodity = new Commodity()
			{
				Name = reader["CommodityName"] as string,
				Price = (decimal)reader["Price"],
				Unit = reader["Unit"] as string,
				Enable = (bool)reader["Enable"]
			};

			return order_item;
		}

		protected IDbCommand AddParameters(IDbCommand cmd, IEnumerable<object> args)
		{
			if (args == null)
				return cmd;
			int i = 0;
			foreach (var arg in args)
			{
				var param = cmd.CreateParameter();
				param.ParameterName = "@p" + i++;
				param.Value = arg;
				cmd.Parameters.Add(param);
			}
			return cmd;
		}

		protected bool OpenConnection()
		{
			if (this.Database.Connection.State == ConnectionState.Broken)
				this.Database.Connection.Close();
			if (this.Database.Connection.State == ConnectionState.Closed)
				this.Database.Connection.Open();
			return this.Database.Connection.State == ConnectionState.Open;
		}

		protected bool CloseConnection()
		{
			if (this.Database.Connection.State == ConnectionState.Broken)
				this.Database.Connection.Close();
			else if (this.Database.Connection.State == ConnectionState.Open)
				this.Database.Connection.Close();
			return this.Database.Connection.State == ConnectionState.Closed;
		}

		#region GenerateID
		/// <summary>
		/// MongoDB 生成 唯一 的 ID
		/// </summary>
		class MongoID
		{
			static MongoID()
			{
				Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			}

			/// <summary> 1970-01-01 00:00:00 U
			/// </summary>
			private static DateTime Epoch { get; set; }

			/// <summary> 本地计算机的主机名 hash.
			/// </summary>
			/// <returns></returns>
			private static byte[] HostNameHash()
			{
				var md5 = System.Security.Cryptography.MD5.Create();
				var host = System.Net.Dns.GetHostName();
				return md5.ComputeHash(System.Text.Encoding.Default.GetBytes(host));
			}

			private readonly object _inclock = new object();
			private int _inc;
			private byte[] _machineHash;
			private byte[] _procId;

			/// <summary>
			///   Initializes a new instance of the <see cref = "MongoID" /> class.
			/// </summary>
			public MongoID()
			{
				GenerateConstants();
			}

			/// <summary>
			///   Generates this instance.
			/// </summary>
			/// <returns></returns>
			private byte[] generate()
			{
				//FIXME Endian issues with this code.  
				//.Net runs in native endian mode which is usually little endian.  
				//Big endian machines don't need the reversing (Linux+PPC, XNA on XBox)
				var oid = new byte[12];
				var copyidx = 0;

				var time = BitConverter.GetBytes(GenerateTime());
				Array.Reverse(time);
				Array.Copy(time, 0, oid, copyidx, 4);
				copyidx += 4;

				Array.Copy(_machineHash, 0, oid, copyidx, 3);
				copyidx += 3;

				Array.Copy(_procId, 2, oid, copyidx, 2);
				copyidx += 2;

				var inc = BitConverter.GetBytes(GenerateInc());
				Array.Reverse(inc);
				Array.Copy(inc, 1, oid, copyidx, 3);

				return oid;
			}

			/// <summary>
			/// 生成
			/// </summary>
			/// <returns></returns>
			public string Generate()
			{
				var _bytes = new byte[12];
				Array.Copy(generate(), _bytes, 12);
				return BitConverter.ToString(_bytes).Replace("-", "").ToLower();
			}

			/// <summary>
			///   Generates the time.
			/// </summary>
			/// <returns></returns>
			private uint GenerateTime()
			{
				var now = DateTime.UtcNow;
				//DateTime nowtime = new DateTime(epoch.Year, epoch.Month, epoch.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
				var diff = now - MongoID.Epoch;
				return (uint)Math.Floor(diff.TotalSeconds);
			}

			/// <summary>
			///   Generates the inc.
			/// </summary>
			/// <returns></returns>
			private int GenerateInc()
			{
				lock (_inclock)
				{
					return ++_inc;
				}
			}

			/// <summary> Generates the constants.
			/// </summary>
			private void GenerateConstants()
			{
				_machineHash = MongoID.HostNameHash();
				_procId = BitConverter.GetBytes(GenerateProcId());
				Array.Reverse(_procId);
			}

			/// <summary>
			///   Generates the proc id.
			/// </summary>
			/// <returns></returns>
			private int GenerateProcId()
			{
				var proc = System.Diagnostics.Process.GetCurrentProcess();
				return proc.Id;
			}
		}

		/// <summary>
		/// </summary>
		static EastRiverEntities()
		{
			_mongo_id = new MongoID();
		}

		/// <summary>
		/// </summary>
		private static MongoID _mongo_id;

		/// <summary> 获取唯一的ID
		/// </summary>
		public virtual string GenerateID
		{
			get { return EastRiverEntities._mongo_id.Generate(); }
		}
		#endregion

	}
}