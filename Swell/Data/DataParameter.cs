using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swell.Data
{
    /// <summary> 参数
    /// </summary>
    [Serializable]
    public class DataParameter : IDataParameter
    {
        /// <summary> 初始化实例
        /// </summary>
        public DataParameter()
        {
            
        }

        /// <summary> 初始化实例
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <param name="sourceColumn"></param>
        public DataParameter(string parameterName, object value, string sourceColumn)
        {
            this.ParameterName = parameterName;
            this.Value = value;
            this.SourceColumn = sourceColumn;
        }

        /// <summary> 获取或设置参数的 System.Data.DbType
        /// </summary>
        public virtual DbType DbType
        {
            get;
            set;
        }

        /// <summary> 获取或设置一个值，该值指示参数是只可输入、只可输出、双向还是存储过程返回值参数
        /// </summary>
        public virtual ParameterDirection Direction
        {
            get;
            set;
        }

        /// <summary> 获取一个值，该值指示参数是否接受 null 值
        /// </summary>
        public virtual bool IsNullable
        {
            get { return true; }
        }

        /// <summary> 获取或设置 System.Data.IDataParameter 的名称
        /// </summary>
        public virtual string ParameterName
        {
            get;
            set;
        }

        /// <summary>  Value 值的状态
        /// </summary>
        public virtual DataRowVersion SourceVersion
        {
            get;
            set;
        }

        /// <summary> 获取或设置该参数的值
        /// </summary>
        public virtual object Value
        {
            get;
            set;
        }

        /// <summary> 获取或设置源列的名称 + 比较运算符
        /// </summary>
        public virtual string SourceColumn
        {
            get;
            set;
        }

        /// <summary> 转换为其它的 Parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T ConvertTo<T>() where T : class, IDataParameter, new()
        {
            T t = new T();
            return EDataParameter.Copy(t, this); 
        }

        /// <summary> 不区分大小写比较 ParameterName
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (Object.Equals(this, null))
                return false;
            else if (obj is IDataParameter)
                return String.Compare(this.ParameterName, (obj as IDataParameter).ParameterName, true) == 0;
            return false;
        }

        /// <summary> Base 的 Hash 算法
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
 	         return base.GetHashCode();
        }

        /// <summary> 返回 ParameterName
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
 	         return base.ToString();
        }

        /// <summary> 不区分大小写比较 ParameterName
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public static bool operator ==(DataParameter param1, object param2)
        {
            if (Object.Equals(param1, null))
                return false;
            return param1.Equals(param2);
        }

        /// <summary> 不区分大小写比较 ParameterName
        /// </summary>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <returns></returns>
        public static bool operator !=(DataParameter param1, object param2)
        {
            if (Object.Equals(param1, null))
                return false;
            return !param1.Equals(param2);
        }

     

    }
}
