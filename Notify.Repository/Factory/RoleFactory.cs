using System;
using System.Data;
using Notify.Code.Extension;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Model.DB;

namespace Notify.Repository.Factory
{
    /// <summary>
    /// 角色仓储工作
    /// </summary>
    public class RoleFactory : IEntityFactory<MRole>
    {
        /// <summary>
        /// 创建角色信息
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <returns>角色信息</returns>
        public MRole BuildEntity(IDataReader reader)
        {
            var mRole = new MRole
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                RoleName = reader["RoleName"].ToString(),
                RoleDescription = reader["RoleDescription"].ToString()
            };
            return mRole;
        }

        /// <summary>
        /// 创建角色信息
        /// </summary>
        /// <param name="table">DataSet</param>
        /// <returns>角色信息</returns>
        public MRole BuildEntity(DataSet table)
        {
            return table.ToModel<MRole>();
        }
    }
}