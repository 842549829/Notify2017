using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

namespace Notify.DbCommon.UnitOfWork
{
    /// <summary>
    /// 创建数据链接工厂
    /// </summary>
    public static class DbFactories
    {
        /// <summary>
        /// 创建数据链接
        /// </summary>
        /// <param name="name">
        /// 链接字符名称
        /// </param>
        /// <returns>
        /// The IDbConnection<see cref="IDbConnection"/>.
        /// </returns>
        public static IDbConnection GetConnection(string name)
        {
            ConnectionStringSettings connectionSetting = ConfigurationManager.ConnectionStrings[name];
            DbConnection conn;
            switch (connectionSetting.ProviderName.ToLower())
            {
                case "mysql.data.mysqlclient":
                    conn = new MySqlConnection(connectionSetting.ConnectionString);
                    break;
                case "oracle.data.oracleclient":
                    conn = new OracleConnection(connectionSetting.ConnectionString);
                    break;
                case "access.data.accessclient":
                    conn = new OleDbConnection(connectionSetting.ConnectionString);
                    break;
                default:
                    conn = new SqlConnection(connectionSetting.ConnectionString);
                    break;
            }
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }
    }

    /// <summary>
    /// 数据链接字符串
    /// </summary>
    public static class ConnectionString
    {
        /// <summary>
        /// 数据链接
        /// </summary>
        public static string SqlDb => "SqlDB";

        /// <summary>
        /// 数据链接
        /// </summary>
        public static string MySqlDb => "MySqlDB";
    }
}
