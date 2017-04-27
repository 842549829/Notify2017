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

namespace Notify.Repository.Access
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class AccountesRepository : SqlRepositoryBase<Guid, MAccount>, IAccountesRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="name">数据连接</param>
        public AccountesRepository(IPowerUnitOfWork unitOfWork, string name)
            : base(unitOfWork, name)
        {
        }

        /// <summary>
        /// 创建用户仓储工厂
        /// </summary>
        /// <returns>用户仓储工厂</returns>
        protected override IEntityFactory<MAccount> BuildEntityFactory()
        {
            return new AccountesFactory();
        }

        /// <summary>
        /// 加载子对象
        /// </summary>
        /// <param name="childCallbacks">子对象委托</param>
        protected override void BuildChildCallbacks(Dictionary<string, AppendChildData> childCallbacks)
        {
            childCallbacks.Add(
               "Id",
               (account, obj) =>
               {
                   //using (AddressRepository repository = new AddressRepository(this.UnitOfWork, this.Name))
                   //{
                   //}
                   //string sql = "SELECT * FROM Address WHERE AccountId = @AccountId";
                   //this.ClearParameters();
                   //this.AddParameter("@AccountId", a.Id);
                   //a.Address = this.ExecuteReader(sql).ToModel<Address>();
               });
        }

        /// <summary>
        /// 根据Id查询用户
        /// </summary>
        /// <param name="key">用户Id</param>
        /// <returns>用户</returns>
        public override MAccount Query(Guid key)
        {
            string sql = "SELECT * FROM Account WHERE Id = @Id";
            this.ClearParameters();
            this.AddParameter("@Id", key);
            return this.BuildEntityFromSql(sql);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="item">用户</param>
        public override void PersistNewItem(IEntity item)
        {
            var entity = (MAccount)item;
            this.ClearParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO Account ");
            sql.Append(" ( ");
            sql.Append("Id,");
            sql.Append("AccountNO,");
            sql.Append("AccountName,");
            sql.Append("Mail,");
            sql.Append("Mobile,");
            sql.Append("Password,");
            sql.Append("PayPassword,");
            sql.Append("CreateTime,");
            sql.Append("IsAdmin,");
            sql.Append("Status");
            sql.Append(" ) VALUES ( ");
            sql.Append("@Id,");
            sql.Append("@AccountNO,");
            sql.Append("@AccountName,");
            sql.Append("@Mail,");
            sql.Append("@Mobile,");
            sql.Append("@Password,");
            sql.Append("@PayPassword,");
            sql.Append("@CreateTime,");
            sql.Append("@IsAdmin,");
            sql.Append("@Status");
            sql.Append(" ); ");

            this.AddParameter("@Id", entity.Key);
            this.AddParameter("@AccountNO", entity.AccountNo);
            this.AddParameter("@AccountName", entity.AccountName);
            this.AddParameter("@Mail", entity.Mail);
            this.AddParameter("@Mobile", entity.Mobile);
            this.AddParameter("@Password", entity.Password);
            this.AddParameter("@PayPassword", entity.PayPassword);
            this.AddParameter("@CreateTime", entity.CreateTime);
            this.AddParameter("@IsAdmin", entity.IsAdmin);
            this.AddParameter("@Status", entity.Status);

            this.ExecuteNonQuery(sql.ToString());
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="item">用户</param>
        public override void PersistUpdatedItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="item">用户</param>
        public override void PersistDeletedItem(IEntity item)
        {
            this.ClearParameters();
            var account = (MAccount)item;
            const string sql = "DELETE FROM Account WHERE Id = @Id;";
            this.AddParameter("@Id", account.Id);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        public void RemoveAccount(Guid id)
        {
            if (this.UnitOfWork == null)
            {
                this.RemoveAccountValue(id);
            }
            else
            {
                this.UnitOfWork.RegisterAction(id, this.RemoveAccountValue);
            }
        }

        /// <summary>
        /// 根据帐号查询用户
        /// </summary>
        /// <param name="accountNo">用户帐号</param>
        /// <returns>用户</returns>
        public MAccount Query(string accountNo)
        {
            const string sql = "SELECT * FROM Account WHERE AccountNo = @AccountNo";
            this.ClearParameters();
            this.AddParameter("@AccountNo", accountNo);
            return this.BuildEntityFromSql(sql);
        }

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>结果</returns>
        public IEnumerable<MAccount> QueryAccountByPaging(TAccountCondition condition)
        {
            this.ClearParameters();
            StringBuilder sqlCondition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(condition.AccountNo))
            {
                sqlCondition.Append(" AND AccountNo = @AccountNo ");
                this.AddParameter("@AccountNo", condition.AccountNo);
            }
            if (!string.IsNullOrWhiteSpace(condition.AccountName))
            {
                sqlCondition.Append(" AND AccountName = @AccountName ");
                this.AddParameter("@AccountName", condition.AccountName);
            }

            if (condition.GetRowsCount)
            {
                string sqlCount = "SELECT COUNT(0) FROM Account WHERE 1 = 1 " + sqlCondition + ";";
                object obj = this.ExecuteScalar(sqlCount);
                condition.RowsCount = obj == null ? 0 : Convert.ToInt32(obj);
            }

            string sqlData = string.Format("SELECT TOP {0} * FROM Account WHERE Id NOT IN( SELECT TOP {1} Id  FROM Account);", condition.PageSize, condition.StratRows);
            return this.BuildEntitiesFromSql(sqlData);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        private void RemoveAccountValue(object id)
        {
            this.ClearParameters();
            const string sql = "DELETE FROM Account WHERE Id = ?Id;";
            this.AddParameter("?Id", id);
            this.ExecuteNonQuery(sql);
        }
    }
}
