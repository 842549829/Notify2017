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

namespace Notify.Repository.SqlServer
{
    /// <summary>
    /// 用户角色关系仓储
    /// </summary>
    public class RoleUserRelationshipRepository : SqlRepositoryBase<Guid, MRoleUserRelationship>, IRoleUserRelationshipRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="name">数据链接</param>
        public RoleUserRelationshipRepository(IPowerUnitOfWork unitOfWork, string name) : base(unitOfWork, name)
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>MRoleUserRelationship</returns>
        public override MRoleUserRelationship Query(Guid key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item">添加内容</param>
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
            this.ClearParameters();
            var entity = (MRoleUserRelationship)item;
            const string sql = "DELETE FROM RoleUserRelationship WHERE AccountId = @AccountId;";
            this.AddParameter("@AccountId", entity.AccountId);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 创建数据工厂
        /// </summary>
        /// <returns>数据工厂</returns>
        protected override IEntityFactory<MRoleUserRelationship> BuildEntityFactory()
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
        /// 根据用户Id删除
        /// </summary>
        /// <param name="accountId">用户Id</param>
        public void RemoveByAccountId(Guid accountId)
        {
            if (this.UnitOfWork == null)
            {
                this.RemoveByAccountIdValue(accountId);
            }
            else
            {
                this.UnitOfWork.RegisterAction(accountId, this.RemoveByAccountIdValue);
            }
        }

        /// <summary>
        /// 根据用户Id删除
        /// </summary>
        /// <param name="accountId">用户Id</param>
        public void RemoveByAccountIdValue(object accountId)
        {
            this.ClearParameters();
            var entity = Guid.Parse(accountId.ToString());
            const string sql = "DELETE FROM RoleUserRelationship WHERE AccountId = @AccountId;";
            this.AddParameter("@AccountId", entity);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entity">实体对象</param>
        public void Add(IEnumerable<MRoleUserRelationship> entity)
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
        /// <param name="entity">实体对象</param>
        public void AddValue(object entity)
        {
            this.ClearParameters();
            var items = entity as IList<MRoleUserRelationship>;
            if (items == null || !items.Any())
            {
                return;
            }
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO RoleUserRelationship");
            sql.Append(" ( ");
            sql.Append("Id,");
            sql.Append("AccountId,");
            sql.Append("RoleId");
            sql.Append(" ) VALUES ");
            int index = 0;
            foreach (var item in items)
            {
                sql.Append("(");
                sql.Append($"@Id{index},");
                sql.Append($"@AccountId{index},");
                sql.Append($"@RoleId{index}");
                sql.Append("),");

                this.AddParameter($"@Id{index}", item.Id);
                this.AddParameter($"@AccountId{index}", item.AccountId);
                this.AddParameter($"@RoleId{index}", item.RoleId);
                index++;
            }
            this.ExecuteNonQuery(sql.ToString().TrimEnd(','));
        }
    }
}
