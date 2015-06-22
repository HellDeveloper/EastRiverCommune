using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swell.Core;

namespace Swell.Data
{
	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection : IDbConnection
	{
		/*
		 *  静态成员
		 *  辅助作用
		 */

		/// <summary> 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="args"></param>
		/// <param name="param_type">Parameter 的实例类型</param>
		/// <param name="neq_inputs">parameter的方向不等于 ParameterDirection.Input</param>
		private static void add_parameter(IDbCommand cmd, Type param_type, IEnumerable<object> args, Dictionary<IDataParameter, IDataParameter> neq_inputs)
		{
			foreach (var item in args)
			{
				if (item == null)
					continue;
				else if (item is IEnumerable<object>)
					TDbConnection.add_parameter(cmd, param_type, item as IEnumerable<object>, neq_inputs);
				else if (item is IDataParameter)
					TDbConnection.add_parameter(cmd, param_type, item as IDataParameter, neq_inputs);
			}
		}

		/// <summary> 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="param"></param>
		/// <param name="param_type">Parameter 的实例类型</param>
		/// <param name="neq_inputs">parameter的方向不等于 ParameterDirection.Input</param>
		private static void add_parameter(IDbCommand cmd, Type param_type, IDataParameter param, Dictionary<IDataParameter, IDataParameter> neq_inputs)
		{
			if (String.IsNullOrWhiteSpace(param.ParameterName))
				return;
			param.Value = param.Value ?? DBNull.Value;
			IDataParameter arg = param;
			if (!param_type.IsInstanceOfType(arg))
			{
				arg = EDataParameter.Clone<IDataParameter>(cmd.CreateParameter(), param);
				if (arg.Direction != ParameterDirection.Input)
					neq_inputs.Add(param, arg);
			}
			if (arg.SourceVersion == DataRowVersion.Original)
			{
				if (arg.GetComparisonOperator().EndsWith(" like", StringComparison.OrdinalIgnoreCase))
					arg.Value = String.Format("%{0}%", arg.Value.TryToString());
				arg.SourceVersion = DataRowVersion.Current;
			}
			if (arg.Direction != ParameterDirection.Input)
				cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(arg);
		}

		/// <summary> 执行 ExecuteNonQuery
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		private static int non_query(System.Data.IDbCommand cmd, object o)
		{
			return cmd.ExecuteNonQuery();
		}

		/// <summary> 执行 ExecuteScalar()
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		private static object scalar(System.Data.IDbCommand cmd, object o)
		{
			return cmd.ExecuteScalar();
		}

		/// <summary> 执行 ExecuteReader(behavior)
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="behavior"></param>
		/// <returns></returns>
		private static IDataReader reader(System.Data.IDbCommand cmd, CommandBehavior behavior)
		{
			return cmd.ExecuteReader(behavior);
		}

		/// <summary> 清除
		/// </summary>
		/// <param name="neq_inputs">parameter的方向不等于 ParameterDirection.Input</param>
		private static void clear_neq_input(Dictionary<IDataParameter, IDataParameter> neq_inputs)
		{
			foreach (var item in neq_inputs)
				item.Key.Value = item.Value.Value;
			neq_inputs.Clear();
		}

		/// <summary> IDataReader 转 DataTable 
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static DataTable IDataReaderToDataTable(IDataReader reader)
		{
			DataTable table = new DataTable();
			List<int> list = new List<int>();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				string name = reader.GetName(i);
				if (table.Columns.Contains(name))
					continue;
				table.Columns.Add(name, reader.GetFieldType(i));
				list.Add(i);
			}
			while (reader.Read())
			{
				object[] array = new object[list.Count];
				int index = 0;
				foreach (var item in list)
					array[index++] = reader.GetValue(item) ?? DBNull.Value;
				table.Rows.Add(array);
			}
			if (!reader.IsClosed)
				reader.Close();
			return table;
		}
	}

	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection : IDbConnection
	{
		/// <summary> 关系型数据库连接对象
		/// </summary>
		private IDbConnection _db_connection;

		/// <summary> 构造函数
		/// </summary>
		/// <param name="conn">关系型数据库连接对象</param>
		protected TDbConnection(IDbConnection conn)
		{
			this._db_connection = conn;
		}

	}

	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection : IDbConnection
	{
		/// <summary> 开始数据库事务
		/// </summary>
		/// <param name="il">连接的事务锁定行为</param>
		/// <returns>新事务的对象</returns>
		public virtual IDbTransaction BeginTransaction(IsolationLevel il)
		{
			return this.BeginTransaction(il);
		}

		/// <summary> 开始数据库事务
		/// </summary>
		/// <returns>新事务的对象</returns>
		public virtual IDbTransaction BeginTransaction()
		{
			return this.BeginTransaction();
		}

		/// <summary> 更改当前数据库
		/// </summary>
		/// <param name="databaseName">要代替当前数据库进行使用的数据库的名称</param>
		public virtual void ChangeDatabase(string databaseName)
		{
			this._db_connection.ChangeDatabase(databaseName);
		}

		/// <summary> 关闭数据库的连接
		/// </summary>
		void IDbConnection.Close()
		{
			this.Close();
		}

		/// <summary> 关闭数据库的连接
		/// </summary>
		/// <returns>是否关闭状态</returns>
		public virtual bool Close()
		{
			if (this._db_connection.State == ConnectionState.Broken || this._db_connection.State == ConnectionState.Open)
				this._db_connection.Close();
			return this._db_connection.State == ConnectionState.Closed;
		}

		/// <summary> 获取或设置用于打开数据库的字符串
		/// </summary>
		public virtual string ConnectionString
		{
			get
			{
				return this._db_connection.ConnectionString;
			}
			set
			{
				if (String.IsNullOrWhiteSpace(value))
					return;
				if (value.IndexOf(Swell.Core.Assist.SEMICOLON) >= 0) // 如果有分号直接赋值
				{
					this._db_connection.ConnectionString = value;
				}
				else // 从配置文件 value是Name
				{
					ConnectionStringSettings conn_str_set = ConfigurationManager.ConnectionStrings[value];
					this._db_connection.ConnectionString = conn_str_set.ConnectionString;
				}
			}
		}

		/// <summary> 获取在尝试建立连接时终止尝试并生成错误之前所等待的时间
		/// </summary>
		public virtual int ConnectionTimeout
		{
			get { return this._db_connection.ConnectionTimeout; }
		}

		/// <summary> 创建并返回一个与该连接相关联的 Command 对象
		/// </summary>
		/// <returns>一个与该连接相关联的 Command 对象</returns>
		public virtual IDbCommand CreateCommand()
		{
			return this._db_connection.CreateCommand();
		}

		/// <summary> 创建并返回一个与该连接相关联的 Command 对象
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns>一个与该连接相关联的 Command 对象</returns>
		public virtual IDbCommand CreateCommand(string commandText)
		{
			IDbCommand cmd = this._db_connection.CreateCommand();
			cmd.CommandText = commandText;
			return cmd;
		}

		/// <summary> 获取当前数据库或连接打开后要使用的数据库的名称
		/// </summary>
		public virtual string Database
		{
			get { return this._db_connection.Database; }
		}

		/// <summary> 打开一个数据库连接，由 ConnectionString 属性指定
		/// </summary>
		void IDbConnection.Open()
		{
			this.Open();
		}

		/// <summary> 打开一个数据库连接，由 ConnectionString 属性指定
		/// </summary>
		/// <returns>是否打开状态</returns>
		public virtual bool Open()
		{
			if (this._db_connection.State == ConnectionState.Broken)
				this._db_connection.Close();
			if (this._db_connection.State == ConnectionState.Closed)
				this._db_connection.Open();
			return this._db_connection.State == ConnectionState.Open;
		}

		/// <summary> 获取连接的当前状态
		/// </summary>
		public virtual ConnectionState State
		{
			get { return this._db_connection.State; }
		}

		/// <summary> 执行与释放或重置非托管资源相关的应用程序定义的任务
		/// </summary>
		public virtual void Dispose()
		{
			this._db_connection.Dispose();
		}
	}

	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection : IDbConnection
	{
		/// <summary> 核心方法，主要针对 ExecuteNonQuery 和 ExecuteScalar
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <typeparam name="Data"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="init_command"></param>
		/// <param name="fun"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		private Result run<Result, Data>(string sql, IEnumerable<object> args, Action<IDbCommand, IEnumerable<object>, Dictionary<IDataParameter, IDataParameter>> init_command, Func<IDbCommand, Data, Result> fun, Data data)
		{
			Dictionary<IDataParameter, IDataParameter> neq_inputs = new Dictionary<IDataParameter, IDataParameter>();
			bool need_close = this._db_connection.State == ConnectionState.Closed;
			IDbCommand cmd = this.CreateCommand(sql);
			init_command(cmd, args, neq_inputs);
			this.Open();
			Result temp = fun.Invoke(cmd, data);
			if (need_close && !(temp is IDataReader))
				this.Close();
			TDbConnection.clear_neq_input(neq_inputs);
			cmd.Parameters.Clear();
			return temp;
		}

		/// <summary> 核心方法，主要针对 ExecuteReader
		/// </summary>
		/// <typeparam name="Result">返回结果类型</typeparam>
		/// <param name="sql">执行文本</param>
		/// <param name="args"></param>
		/// <param name="init_command"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		private Result run<Result>(string sql, IEnumerable<object> args, Action<IDbCommand, IEnumerable<object>, Dictionary<IDataParameter, IDataParameter>> init_command, Func<IDataReader, Result> func)
		{
			CommandBehavior behavior = this._db_connection.State == ConnectionState.Closed ? CommandBehavior.CloseConnection : CommandBehavior.Default;
			IDataReader reader = this.run(sql, args, init_command, TDbConnection.reader, behavior);
			Result temp = func(reader);
			if (!reader.IsClosed)
				reader.Close();
			return temp;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <typeparam name="Data"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		private Result execute<Result, Data>(string sql, IEnumerable<object> args, Func<IDbCommand, Data, Result> func, Data data)
		{
			return this.run(sql, args, this.init_execute_command, func, data);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		private Result execute<Result>(string sql, IEnumerable<object> args, Func<IDataReader, Result> func)
		{
			return this.run(sql, args, this.init_execute_command, func);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <typeparam name="Data"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		private Result number<Result, Data>(string sql, IEnumerable<object> args, Func<IDbCommand, Data, Result> func, Data data)
		{
			return this.run(sql, args, this.init_number_command, func, data);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		private Result number<Result>(string sql, IEnumerable<object> args, Func<IDataReader, Result> func)
		{
			return this.run(sql, args, this.init_number_command, func);
		}

		/// <summary> 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="args"></param>
		/// <param name="neq_inputs">parameter的方向不等于 ParameterDirection.Input</param>
		/// <returns></returns>
		private void init_execute_command(IDbCommand cmd, IEnumerable<object> args, Dictionary<IDataParameter, IDataParameter> neq_inputs)
		{
			var param_type = cmd.CreateParameter().GetType();
			cmd.Parameters.Clear();
			TDbConnection.add_parameter(cmd, param_type, args, neq_inputs);
			if (cmd.CommandType != CommandType.StoredProcedure && cmd.CommandText.IndexOf(Swell.Core.Assist.WHITE_SPACE, 0) == -1)
				cmd.CommandType = CommandType.StoredProcedure;
		}

		/// <summary> 
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="args"></param>
		/// <param name="neq_inputs">parameter的方向不等于 ParameterDirection.Input</param>
		/// <returns></returns>
		private void init_number_command(IDbCommand cmd, IEnumerable<object> args, Dictionary<IDataParameter, IDataParameter> neq_inputs)
		{
			int increment = 0;
			foreach (var item in args)
			{
				IDataParameter param = cmd.CreateParameter();
				param.ParameterName = this.ParameterNamePrefix + (increment++).ToString();
				param.Value = item;
				cmd.Parameters.Add(param);
			}
		}
	}

	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection : IDbConnection
	{
		/// <summary> 
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(string sql, IEnumerable<IDataParameter> args)
		{
			return this.execute(sql, args, TDbConnection.non_query, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(string sql, IEnumerable<IEnumerable<IDataParameter>> args)
		{
			return this.execute(sql, args, TDbConnection.non_query, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public int ExecuteNonQuery(string sql, params IDataParameter[] args)
		{
			return this.execute(sql, args, TDbConnection.non_query, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object ExecuteScalar(string sql, IEnumerable<IDataParameter> args)
		{
			return this.execute(sql, args, TDbConnection.scalar, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object ExecuteScalar(string sql, IEnumerable<IEnumerable<IDataParameter>> args)
		{
			return this.execute(sql, args, TDbConnection.scalar, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object ExecuteScalar(string sql, params IDataParameter[] args)
		{
			return this.execute(sql, args, TDbConnection.scalar, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="behavior"></param>
		/// <returns></returns>
		public IDataReader ExecuteReader(string sql, IEnumerable<IDataParameter> args, CommandBehavior behavior = CommandBehavior.CloseConnection)
		{
			return this.execute(sql, args, TDbConnection.reader, behavior);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="behavior"></param>
		/// <returns></returns>
		public IDataReader ExecuteReader(string sql, IEnumerable<IEnumerable<IDataParameter>> args, CommandBehavior behavior = CommandBehavior.CloseConnection)
		{
			return this.execute(sql, args, TDbConnection.reader, behavior);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="behavior"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public IDataReader ExecuteReader(string sql, CommandBehavior behavior = CommandBehavior.CloseConnection, params IDataParameter[] args)
		{
			return this.execute(sql, args, TDbConnection.reader, behavior);
		}

		/// <summary> 
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public Result ExecuteReader<Result>(string sql, IEnumerable<IDataParameter> args, Func<IDataReader, Result> func)
		{
			return this.execute(sql, args, func);
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public Result ExecuteReader<Result>(string sql, IEnumerable<IEnumerable<IDataParameter>> args, Func<IDataReader, Result> func)
		{
			return this.execute(sql, args, func);
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <param name="sql"></param>
		/// <param name="func"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public Result ExecuteReader<Result>(string sql, Func<IDataReader, Result> func, params IDataParameter[] args)
		{
			return this.execute(sql, args, func);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(string sql, IEnumerable<IDataParameter> args)
		{
			return this.execute(sql, args, TDbConnection.IDataReaderToDataTable);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(string sql, IEnumerable<IEnumerable<IDataParameter>> args)
		{
			return this.execute(sql, args, TDbConnection.IDataReaderToDataTable);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public DataTable ExecuteDataTable(string sql, params IDataParameter[] args)
		{
			return this.execute(sql, args, TDbConnection.IDataReaderToDataTable);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public int NumberNonQuery(string sql, IEnumerable<object> args)
		{
			return this.number(sql, args, TDbConnection.non_query, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public int NumberNonQuery(string sql, params object[] args)
		{
			return this.number(sql, args, TDbConnection.non_query, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object NumberScalar(string sql, IEnumerable<object> args)
		{
			return this.number(sql, args, TDbConnection.scalar, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public object NumberScalar(string sql, params object[] args)
		{
			return this.number(sql, args, TDbConnection.scalar, String.Empty);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="behavior"></param>
		/// <returns></returns>
		public IDataReader NumberReader(string sql, IEnumerable<object> args, CommandBehavior behavior = CommandBehavior.CloseConnection)
		{
			return this.number(sql, args, TDbConnection.reader, behavior);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="behavior"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public IDataReader NumberReader(string sql, CommandBehavior behavior = CommandBehavior.CloseConnection, params object[] args)
		{
			return this.number(sql, args, TDbConnection.reader, behavior);
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <param name="func"></param>
		/// <returns></returns>
		public Result NumberReader<Result>(string sql, IEnumerable<object> args, Func<IDataReader, Result> func)
		{
			return this.number(sql, args, func);
		}

		/// <summary>
		/// </summary>
		/// <typeparam name="Result"></typeparam>
		/// <param name="sql"></param>
		/// <param name="func"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public Result NumberReader<Result>(string sql, Func<IDataReader, Result> func, params object[] args)
		{
			return this.number(sql, args, func);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public DataTable NumberDataTable(string sql, IEnumerable<object> args)
		{
			return this.number(sql, args, TDbConnection.IDataReaderToDataTable);
		}

		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public DataTable NumberDataTable(string sql, params object[] args)
		{
			return this.number(sql, args, TDbConnection.IDataReaderToDataTable);
		}
	}

	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection : IDbConnection
	{
		/// <summary>
		/// 条件
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public string GetConditionSql(IDataParameter param)
		{
			return CommonSql.ConditionSql(param, CommonSql.FormatSqlValue);
		}

		/// <summary>
		/// 条件AND
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public string GetConditionSql(IEnumerable<IDataParameter> args)
		{
			return CommonSql.ConditionSql(args, CommonSql.GetConditionSql);
		}

		/// <summary>
		/// 条件OR
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public string GetConditionSql(IEnumerable<IEnumerable<IDataParameter>> args)
		{
			return CommonSql.ConditionSql(args, CommonSql.GetConditionSql);
		}

		/// <summary>
		/// 插入
		/// </summary>
		/// <param name="args"></param>
		/// <param name="table_name"></param>
		/// <returns></returns>
		public string GetInsertSql(string table_name, IEnumerable<IDataParameter> args)
		{
			return CommonSql.InsertSql(args, table_name, CommonSql.GetParameterValue);
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="sets">设置</param>
		/// <param name="where">条件</param>
		/// <param name="table_name">表名</param>
		/// <returns></returns>
		public string GetUpdateSql(string table_name, IEnumerable<IDataParameter> sets, IEnumerable<IDataParameter> where)
		{
			return CommonSql.UpdateSql(sets, where, table_name, CommonSql.GetSetSql, CommonSql.GetConditionSql);
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="where"></param>
		/// <param name="table_name"></param>
		/// <returns></returns>
		public string GetDeleteSql(string table_name, IEnumerable<IDataParameter> where)
		{
			return CommonSql.DeleteSql(where, table_name, CommonSql.GetConditionSql);
		}

		/// <summary>
		/// 构建查询SQL
		/// </summary>
		/// <param name="args">where 条件</param>
		/// <param name="table_naem">表名</param>
		/// <param name="fieldname">查询的字段名(如果是String.Empty || null 就是 *)</param>
		/// <returns>SELECT语句</returns>
		public string GetSelectSql(string table_naem, IEnumerable<IDataParameter> args, string fieldname = null)
		{
			return CommonSql.SelectSql(args, table_naem, CommonSql.GetConditionSql, fieldname);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public string BuildConditionSql(IDataParameter param)
		{
			return CommonSql.ConditionSql(param, CommonSql.GetParameterName);
		}

		/// <summary>
		/// AND
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public string BuildConditionSql(IEnumerable<IDataParameter> args)
		{
			return CommonSql.ConditionSql(args, CommonSql.BuildConditionSql);
		}

		/// <summary>
		/// OR
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public string BuildConditionSql(IEnumerable<IEnumerable<IDataParameter>> args)
		{
			return CommonSql.ConditionSql(args, CommonSql.BuildConditionSql);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="table_name"></param>
		/// <returns></returns>
		public string BuildInsertSql(string table_name, IEnumerable<IDataParameter> args)
		{
			return CommonSql.InsertSql(args, table_name, CommonSql.BuildParameterValue);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="table_name"></param>
		/// <param name="where"></param>
		/// <returns></returns>
		public string BuildUpdateSql(string table_name, IEnumerable<IDataParameter> args, IEnumerable<IDataParameter> where)
		{
			return CommonSql.UpdateSql(args, where, table_name, CommonSql.BuildSetSql, CommonSql.BuildConditionSql);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <param name="table_name"></param>
		/// <param name="where"></param>
		/// <returns></returns>
		public string BuildUpdateSql(string table_name, IEnumerable<IDataParameter> args, string pk_name)
		{
			var pks = args.Where(m => String.Compare(pk_name, m.GetColumnName(), true) == 0);
			var temp = args.Except(pks);
			return this.BuildUpdateSql(table_name, temp, pks);
		}

		/// <summary>
		/// 构建删除的SQL语句
		/// </summary>
		/// <param name="args"></param>
		/// <param name="table_name">表名</param>
		/// <returns>SQL语句</returns>
		public string BuildDeleteSql(string table_name, IEnumerable<IDataParameter> args)
		{
			return CommonSql.DeleteSql(args, table_name, CommonSql.BuildConditionSql);
		}

		/// <summary>
		/// 构建查询SQL
		/// </summary>
		/// <param name="args">where 条件</param>
		/// <param name="table_name">表名</param>
		/// <param name="fieldname">查询的字段名(如果是String.Empty || null 就是 *)</param>
		/// <returns>SELECT语句</returns>
		public string BuildSelectSql(string table_name, IEnumerable<IDataParameter> args, string fieldname = null)
		{
			return CommonSql.SelectSql(args, table_name, CommonSql.BuildConditionSql, fieldname);
		}
	}

	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection : IDbConnection
	{
		/// <summary> 参数名称的前缀
		/// </summary>
		public virtual char ParameterNamePrefix
		{
			get { return '@'; }
		}

		/// <summary> MongoDB 数据库生成唯一 ID 方式
		/// </summary>
		public virtual string MongoID
		{
			get { return Swell.Core.Assist.MongoID; }
		}

		/// <summary> 创建参数
		/// </summary>
		/// <returns></returns>
		public DataParameter CreateDataParameter()
		{
			return new DataParameter();
		}
	}

	/// <summary> 关系型数据库连接对象
	/// </summary>
	public partial class TDbConnection<T> : TDbConnection where T : IDbConnection, new()
	{
		/// <summary> 构造函数
		/// </summary>
		public TDbConnection()
			: base(new T())
		{

		}
		/// <summary> 
		/// </summary>
		/// <param name="connectionString"></param>

		public TDbConnection(string connectionString)
			: base(new T())
		{
			this.ConnectionString = connectionString;
		}

	}

}
