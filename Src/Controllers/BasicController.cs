using EastRiverCommune.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EastRiverCommune.Controllers
{
    public class BasicController : Controller
	{
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
		static BasicController()
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
			get { return BasicController._mongo_id.Generate(); }
		}
		#endregion

		private EastRiverEntities database_context;

		public virtual EastRiverEntities DatabaseContext
		{
			get
			{
				if (this.database_context == null)
					this.database_context = new EastRiverEntities();
				return this.database_context;
			}
		}

		private JavaScriptSerializer _java_script_serializer;

		public JavaScriptSerializer JavaScriptSerializer
		{
			get 
			{
				if (this._java_script_serializer == null)
					this._java_script_serializer = new JavaScriptSerializer();
				return this._java_script_serializer;
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
			{
				if (this.database_context != null)
					this.database_context.Dispose();
			}
		}

        /// <summary> 什么都不做
        /// </summary>
        /// <returns></returns>
        protected virtual ActionResult Empty()
        {
            return new EmptyResult();
        }

		/// <summary>
		/// 验证模式
		/// </summary>
		/// <returns>成功返回null</returns>
		[NonAction]
		protected ActionResult ValidateModel()
		{
			if (this.ModelState.IsValid)
				return null;
			return this.PartialView("~/Views/Shared/_ValidationSummary.cshtml");
		}

    }
}
