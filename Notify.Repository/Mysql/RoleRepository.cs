using System;
using System.Collections.Generic;
using System.Text;
using Notify.DbCommon.Repositroies;
using Notify.Infrastructure.DomainBase;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Infrastructure.UnitOfWork;
using Notify.IRepository;
using Notify.Model.DB;
using Notify.Model.Transfer;
using Notify.Repository.Factory;

namespace Notify.Repository.Mysql
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class RoleRepository : SqlRepositoryBase<Guid, MRole>, IRoleRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="name">数据连接</param>
        public RoleRepository(IPowerUnitOfWork unitOfWork, string name)
            : base(unitOfWork, name)
        {
        }

        /// <summary>
        /// 根据Id查询角色
        /// </summary>
        /// <param name="key">主键Id</param>
        /// <returns>角色</returns>
        public override MRole Query(Guid key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 角色分页查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>结果</returns>
        public IEnumerable<MRole> QueryRolesByPaging(TRoleCondition condition)
        {
            this.ClearParameters();
            StringBuilder sqlCondition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(condition.RoleName))
            {
                sqlCondition.Append(" AND RoleName = @RoleName ");
                this.AddParameter("@RoleName", condition.RoleName);
            }
            if (!string.IsNullOrWhiteSpace(condition.RoleDescription))
            {
                sqlCondition.Append(" AND RoleDescription = @RoleDescription ");
                this.AddParameter("@RoleDescription", condition.RoleDescription);
            }

            if (condition.GetRowsCount)
            {
                string sqlCount = "SELECT COUNT(0) FROM Role WHERE 1 = 1 " + sqlCondition + ";";
                object obj = this.ExecuteScalar(sqlCount);
                condition.RowsCount = obj == null ? 0 : Convert.ToInt32(obj);
            }

            string sqlData = "SELECT * FROM Role WHERE 1 = 1 " + sqlCondition + " ORDER BY Id DESC LIMIT @StratRows, @PageSize;";
            this.AddParameter("@StratRows", condition.StratRows);
            this.AddParameter("@PageSize", condition.PageSize);
            return this.BuildEntitiesFromSql(sqlData);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="item">角色</param>
        public override void PersistNewItem(IEntity item)
        {
            this.ClearParameters();
            var role = (MRole)item;
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Role ");
            sql.Append(" ( ");
            sql.Append(" Id,");
            sql.Append(" RoleName,");
            sql.Append(" RoleDescription");
            sql.Append(" ) VALUES ( ");
            sql.Append("@Id,");
            sql.Append("@RoleName,");
            sql.Append("@RoleDescription");
            sql.Append(");");
            this.AddParameter("@Id", role.Id);
            this.AddParameter("@RoleName", role.RoleName);
            this.AddParameter("@RoleDescription", role.RoleDescription);
            this.ExecuteNonQuery(sql.ToString());
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="item">角色</param>
        public override void PersistUpdatedItem(IEntity item)
        {
            this.ClearParameters();
            var role = (MRole)item;
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Role SET");
            sql.Append(" RoleName = @RoleName,");
            sql.Append(" RoleDescription = @RoleDescription");
            sql.Append(" WHERE Id = @Id;");
            this.AddParameter("@Id", role.Id);
            this.AddParameter("@RoleName", role.RoleName);
            this.AddParameter("@RoleDescription", role.RoleDescription);
            this.ExecuteNonQuery(sql.ToString());
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="item">角色</param>
        public override void PersistDeletedItem(IEntity item)
        {
            this.ClearParameters();
            var role = (MRole)item;
            const string sql = "DELETE FROM Role WHERE Id = @Id;";
            this.AddParameter("@Id", role.Id);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 创建角色仓储工厂
        /// </summary>
        /// <returns>角色仓储工厂</returns>
        protected override IEntityFactory<MRole> BuildEntityFactory()
        {
            return new RoleFactory();
        }

        /// <summary>
        /// 加载角色子对象
        /// </summary>
        /// <param name="childCallbacks">子对象委托</param>
        protected override void BuildChildCallbacks(Dictionary<string, AppendChildData> childCallbacks)
        {
        }
    }
}
