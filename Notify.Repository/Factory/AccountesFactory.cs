using System;
using System.Data;
using Notify.Code.Extension;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Model.DB;

namespace Notify.Repository.Factory
{
    /// <summary>
    /// 用户仓储工厂
    /// </summary>
    public class AccountesFactory : IEntityFactory<MAccount>
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <returns>用户</returns>
        public MAccount BuildEntity(IDataReader reader)
        {
            return new MAccount
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                AccountNo = reader["AccountNo"].ToString(),
                AccountName = reader["AccountName"].ToString(),
                CreateTime = Convert.ToDateTime(reader["CreateTime"]),
                IsAdmin = Convert.ToBoolean(reader["IsAdmin"]),
                Mail = reader["Mail"].ToString(),
                Mobile = reader["Mobile"].ToString(),
                Password = reader["Password"].ToString(),
                PayPassword = reader["PayPassword"].ToString(),
                Status = (Model.AccountStatus)(Convert.ToUInt32(reader["Status"]))
            };

        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="table">DataSet</param>
        /// <returns>用户</returns>
        public MAccount BuildEntity(DataSet table)
        {
            return table.ToModel<MAccount>();
        }
    }
}