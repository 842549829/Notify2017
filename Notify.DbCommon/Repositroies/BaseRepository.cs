using System;
using System.Data;
using Notify.DbCommon.UnitOfWork;
using Notify.Infrastructure.UnitOfWork;

namespace Notify.DbCommon.Repositroies
{
    /// <summary>
    /// 数据库持久化基类
    /// </summary>
    public abstract class BaseRepository : IDisposable
    {
        /// <summary>
        /// 数据库链接
        /// </summary>
        private IDbConnection connection;

        /// <summary>
        /// Command对象
        /// </summary>
        private IDbCommand cmd;

        /// <summary>
        /// 工作单元接口
        /// </summary>
        public IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// 数据库链接
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository"/> class.
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        /// <param name="name">数据库链接</param>
        protected BaseRepository(IUnitOfWork unit, string name)
        {
            this.Name = name;
            this.UnitOfWork = unit;
            if (unit != null)
            {
                this.cmd = unit.Command;
            }
            else
            {
                if (this.connection == null)
                {
                    this.connection = DbFactories.GetConnection(name);
                }

                if (this.connection.State != ConnectionState.Open)
                {
                    this.connection.Open();
                }

                this.cmd = this.connection.CreateCommand();
            }
        }

        #region Parameter
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>参数</returns>
        protected IDbDataParameter AddParameter(string name)
        {
            IDbDataParameter param = this.CreateParameter(name);
            this.cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        protected IDbDataParameter AddParameter(string name, object value)
        {
            IDbDataParameter param = this.CreateParameter(name, value);
            this.cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <returns>参数</returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type)
        {
            IDbDataParameter param = this.CreateParameter(name, value, type);
            this.cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数输出类型</param>
        /// <returns>参数</returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type, ParameterDirection direction)
        {
            IDbDataParameter param = this.CreateParameter(name, value, type, direction);
            this.cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数输出类型</param>
        /// <param name="size">参数大小</param>
        /// <returns>参数</returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type, ParameterDirection direction, int size)
        {
            IDbDataParameter param = this.CreateParameter(name, value, type, direction, size);
            this.cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数输出类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="scale">显示数字参数的规模(精确到小数点后几位)</param>
        /// <returns>参数</returns>
        protected IDbDataParameter AddParameter(string name, object value, DbType type, ParameterDirection direction, int size, byte scale)
        {
            IDbDataParameter param = this.CreateParameter(name, value, type, direction, size, scale);
            this.cmd.Parameters.Add(param);
            return param;
        }

        /// <summary>
        /// 清除参数
        /// </summary>
        protected void ClearParameters()
        {
            this.cmd.Parameters.Clear();
        }
        #endregion

        #region  ExecuteReader

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql, CommandType type, CommandBehavior behavior, int timeout)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql");
            }

            this.cmd.CommandText = sql;
            this.cmd.CommandType = type;
            this.cmd.CommandTimeout = timeout;
            return this.cmd.ExecuteReader(behavior);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql, CommandType type, CommandBehavior behavior)
        {
            return this.ExecuteReader(sql, type, behavior, 0);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql, CommandType type, int timeout)
        {
            return this.ExecuteReader(sql, type, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql, CommandType type)
        {
            return this.ExecuteReader(sql, type, CommandBehavior.Default, 0);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql, CommandBehavior behavior, int timeout)
        {
            return this.ExecuteReader(sql, CommandType.Text, behavior, timeout);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql, CommandBehavior behavior)
        {
            return this.ExecuteReader(sql, CommandType.Text, behavior, 0);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql, int timeout)
        {
            return this.ExecuteReader(sql, CommandType.Text, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>IDataReader</returns>
        protected IDataReader ExecuteReader(string sql)
        {
            return this.ExecuteReader(sql, CommandType.Text, CommandBehavior.Default, 0);
        }

        #endregion

        #region ExecuteTable

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql, CommandType type, CommandBehavior behavior, int timeout)
        {
            using (IDataReader dr = this.ExecuteReader(sql, type, behavior, timeout))
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql, CommandType type, CommandBehavior behavior)
        {
            return this.ExecuteTable(sql, type, behavior, 0);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql, CommandType type, int timeout)
        {
            return this.ExecuteTable(sql, type, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型(sql文本,存储过程,表)</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql, CommandType type)
        {
            return this.ExecuteTable(sql, type, CommandBehavior.Default, 0);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql, CommandBehavior behavior, int timeout)
        {
            return this.ExecuteTable(sql, CommandType.Text, behavior, timeout);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="behavior">提供了一个查询的结果的描述和对数据库的影响</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql, CommandBehavior behavior)
        {
            return this.ExecuteTable(sql, CommandType.Text, behavior, 0);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql, int timeout)
        {
            return this.ExecuteTable(sql, CommandType.Text, CommandBehavior.Default, timeout);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        protected DataTable ExecuteTable(string sql)
        {
            return this.ExecuteTable(sql, CommandType.Text, CommandBehavior.Default, 0);
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="tableName">表名</param>
        /// <returns>DataTable</returns>
        protected DataSet ExecuteDataSet(string sql, params string[] tableName)
        {
            return this.ExecuteDataSet(sql, CommandType.Text, tableName);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">读取类型</param>
        /// <param name="tableName">表名</param>
        /// <returns>DataTable</returns>
        protected DataSet ExecuteDataSet(string sql, CommandType type, params string[] tableName)
        {
            using (IDataReader dr = this.ExecuteReader(sql, type, CommandBehavior.Default, 0))
            {
                DataSet ds = new DataSet();
                ds.Load(dr, LoadOption.Upsert, tableName);
                return ds;
            }
        }

        #endregion

        #region ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">执行类型</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>结果</returns>
        protected object ExecuteScalar(string sql, CommandType type, int timeout)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql");
            }

            this.cmd.CommandText = sql;
            this.cmd.CommandType = type;
            this.cmd.CommandTimeout = timeout;
            object result = this.cmd.ExecuteScalar();
            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>结果</returns>
        protected object ExecuteScalar(string sql)
        {
            return this.ExecuteScalar(sql, CommandType.Text, 0);
        }
        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">执行类型</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>结果</returns>
        protected int ExecuteNonQuery(string sql, CommandType type, int timeout)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException("sql");
            }

            this.cmd.CommandText = sql;
            this.cmd.CommandType = type;
            this.cmd.CommandTimeout = timeout;
            return this.cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="type">执行类型</param>
        /// <returns>结果</returns>
        protected int ExecuteNonQuery(string sql, CommandType type)
        {
            return this.ExecuteNonQuery(sql, type, 0);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>结果</returns>
        protected int ExecuteNonQuery(string sql, int timeout)
        {
            return this.ExecuteNonQuery(sql, CommandType.Text, timeout);
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>结果</returns>
        protected int ExecuteNonQuery(string sql)
        {
            return this.ExecuteNonQuery(sql, CommandType.Text, 0);
        }

        #endregion

        #region CreateParameter
        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <returns>参数</returns>
        private IDbDataParameter CreateParameter(string name)
        {
            IDbDataParameter param = this.cmd.CreateParameter();
            param.ParameterName = name;
            return param;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns>参数</returns>
        private IDbDataParameter CreateParameter(string name, object value)
        {
            IDbDataParameter param = this.CreateParameter(name);
            param.Value = value ?? DBNull.Value;
            return param;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <returns>参数</returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type)
        {
            IDbDataParameter param = this.CreateParameter(name, value);
            param.DbType = type;
            return param;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数输出类型</param>
        /// <returns>参数</returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction)
        {
            IDbDataParameter param = this.CreateParameter(name, value, type);
            param.Direction = direction;
            return param;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数输出类型</param>
        /// <param name="size">参数大小</param>
        /// <returns>参数</returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction, int size)
        {
            IDbDataParameter param = this.CreateParameter(name, value, type, direction);
            param.Size = size;
            return param;
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="type">参数类型</param>
        /// <param name="direction">参数输出类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="scale">显示数字参数的规模(精确到小数点后几位)</param>
        /// <returns>参数</returns>
        private IDbDataParameter CreateParameter(string name, object value, DbType type, ParameterDirection direction, int size, byte scale)
        {
            IDbDataParameter param = this.CreateParameter(name, value, type, direction, size);
            param.Scale = scale;
            return param;
        }
        #endregion

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (this.cmd != null)
            {
                this.cmd.Dispose();
                this.cmd = null;
            }

            if (this.connection == null)
            {
                return;
            }

            if (this.connection.State == ConnectionState.Open)
            {
                this.connection.Close();
            }

            this.connection.Dispose();
            this.connection = null;
        }
    }
}