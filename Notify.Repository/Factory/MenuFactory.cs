using System;
using System.Data;
using Notify.Code.Extension;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Model.DB;

namespace Notify.Repository.Factory
{
    /// <summary>
    /// 菜单仓储工厂
    /// </summary>
    public class MenuFactory : IEntityFactory<MMenu>
    {
        /// <summary>
        /// 创建菜单实体
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <returns>菜单实体</returns>
        public MMenu BuildEntity(IDataReader reader)
        {
            return new MMenu
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                ParentId = Guid.Parse(reader["ParentId"].ToString()),
                Sort = Convert.ToInt32(reader["Sort"].ToString()),
                Title = reader["Title"].ToString(),
                Description = reader["Description"].ToString(),
                Url = reader["Url"].ToString(),
                Icon = reader["Icon"].ToString()
            };
        }

        /// <summary>
        /// 创建菜单实体
        /// </summary>
        /// <param name="table">IDataReader</param>
        /// <returns>菜单实体</returns>
        public MMenu BuildEntity(DataSet table)
        {
            return table.ToModel<MMenu>();
        }
    }
}