using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notify.DbCommon.Repositroies;
using Notify.Infrastructure.DomainBase;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Infrastructure.UnitOfWork;
using Notify.IRepository;
using Notify.Model.DB;

namespace Notify.Repository.Mysql
{
    /// <summary>
    /// 角色权限仓储
    /// </summary>
    public class RolePermissionsRepository : SqlRepositoryBase<Guid, MRolePermissions>, IRolePermissionsRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="name">数据链接</param>
        public RolePermissionsRepository(IPowerUnitOfWork unitOfWork, string name) : base(unitOfWork, name)
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>MRolePermissions</returns>
        public override MRolePermissions Query(Guid key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item">内容</param>
        public override void PersistNewItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="item">内容</param>
        public override void PersistUpdatedItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="item">删除内容</param>
        public override void PersistDeletedItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建数据工厂
        /// </summary>
        /// <returns>数据工厂</returns>
        protected override IEntityFactory<MRolePermissions> BuildEntityFactory()
        {
            return null;
        }

        /// <summary>
        /// 加载子对象
        /// </summary>
        /// <param name="childCallbacks">子对象委托</param>
        protected override void BuildChildCallbacks(Dictionary<string, AppendChildData> childCallbacks)
        {
        }

        /// <summary>
        /// 根据角色Id删除
        /// </summary>
        /// <param name="roleId">用户Id</param>
        public void RemoveByRoleId(Guid roleId)
        {
            if (this.UnitOfWork == null)
            {
                this.RemoveByRoleIdValue(roleId);
            }
            else
            {
                this.UnitOfWork.RegisterAction(roleId, this.RemoveByRoleIdValue);
            }
        }

        /// <summary>
        /// 查询可用菜单Id
        /// </summary>
        /// <returns>角色Id</returns>
        public IEnumerable<Guid> QueryMenuIds(Guid roleId)
        {
            this.ClearParameters();
            var result = new List<Guid>();
            const string sql = "SELECT MenuId FROM RolePermissions WHERE RoleId = @RoleId;";
            this.AddParameter("@RoleId", roleId);
            using (var reader = this.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    result.Add(reader.GetGuid(0));
                }
            }
            return result;
        }

        /// <summary>
        /// 根据角色Id删除
        /// </summary>
        /// <param name="roleId">用户Id</param>
        public void RemoveByRoleIdValue(object roleId)
        {
            this.ClearParameters();
            var entity = Guid.Parse(roleId.ToString());
            const string sql = "DELETE FROM RolePermissions WHERE RoleId = @RoleId;";
            this.AddParameter("@RoleId", entity);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entity">内容</param>
        public void Add(IEnumerable<MRolePermissions> entity)
        {
            if (this.UnitOfWork == null)
            {
                this.AddValue(entity);
            }
            else
            {
                this.UnitOfWork.RegisterAction(entity, this.AddValue);
            }
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entity">内容</param>
        public void AddValue(object entity)
        {
            this.ClearParameters();
            var items = entity as IEnumerable<MRolePermissions>;
            if (items == null)
            {
                return;
            }
            var mRolePermissionses = items as MRolePermissions[] ?? items.ToArray();
            if (!mRolePermissionses.Any())
            {
                return;
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO RolePermissions");
            sql.Append(" ( ");
            sql.Append("Id,");
            sql.Append("MenuId,");
            sql.Append("RoleId");
            sql.Append(" ) VALUES ");
            int index = 0;
            foreach (var item in mRolePermissionses)
            {
                sql.Append("(");
                sql.Append($"@Id{index},");
                sql.Append($"@MenuId{index},");
                sql.Append($"@RoleId{index}");
                sql.Append("),");

                this.AddParameter($"@Id{index}", item.Id);
                this.AddParameter($"@MenuId{index}", item.MenuId);
                this.AddParameter($"@RoleId{index}", item.RoleId);
                index++;
            }
            this.ExecuteNonQuery(sql.ToString().TrimEnd(','));
        }
    }
}