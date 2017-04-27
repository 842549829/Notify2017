using System;
using System.Collections.Generic;
using System.Text;
using Notify.DbCommon.Repositroies;
using Notify.Infrastructure.DomainBase;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Infrastructure.UnitOfWork;
using Notify.IRepository;
using Notify.Model.DB;
using Notify.Repository.Factory;

namespace Notify.Repository.Access
{
    /// <summary>
    /// 注册验证码仓储
    /// </summary>
    public class VerificationCodeRepository : SqlRepositoryBase<Guid, MVerificationCode>, IVerificationCodeRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="name">数据库链接</param>
        public VerificationCodeRepository(IPowerUnitOfWork unitOfWork, string name) : base(unitOfWork, name)
        {
        }

        /// <summary>
        /// 根据主键查询注册验证码
        /// </summary>
        /// <param name="key">主键</param>
        /// <returns>注册验证码</returns>
        public override MVerificationCode Query(Guid key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加注册验证码
        /// </summary>
        /// <param name="item">注册验证码</param>
        public override void PersistNewItem(IEntity item)
        {
            var entity = (MVerificationCode)item;
            this.ClearParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append(" INSERT INTO VerificationCode ");
            sql.Append(" ( ");
            sql.Append("Id,");
            sql.Append("Type,");
            sql.Append("Code,");
            sql.Append("AccountId,");
            sql.Append("Contact,");
            sql.Append("CreateTime");
            sql.Append(" ) VALUES ( ");
            sql.Append("?Id,");
            sql.Append("?Type,");
            sql.Append("?Code,");
            sql.Append("?AccountId,");
            sql.Append("?Contact,");
            sql.Append("?CreateTime");
            sql.Append(" ); ");

            this.AddParameter("?Id", entity.Key);
            this.AddParameter("?Type", entity.Type);
            this.AddParameter("?Code", entity.Code);
            this.AddParameter("?AccountId", entity.AccountId);
            this.AddParameter("?Contact", entity.Contact);
            this.AddParameter("?CreateTime", entity.CreateTime);

            this.ExecuteNonQuery(sql.ToString());
        }

        /// <summary>
        /// 修改注册验证码
        /// </summary>
        /// <param name="item">注册验证码</param>
        public override void PersistUpdatedItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改注册验证码
        /// </summary>
        /// <param name="item">注册验证码</param>
        public override void PersistDeletedItem(IEntity item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建注册验证码仓储工厂
        /// </summary>
        /// <returns></returns>
        protected override IEntityFactory<MVerificationCode> BuildEntityFactory()
        {
            return new VerificationCodeFactory();
        }

        /// <summary>
        /// 加载子对象
        /// </summary>
        /// <param name="childCallbacks">子对象委托</param>
        protected override void BuildChildCallbacks(Dictionary<string, AppendChildData> childCallbacks)
        {
        }
    }
}
