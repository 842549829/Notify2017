using System;
using System.Collections.Generic;
using Notify.Infrastructure.RepositoryFramework;
using Notify.Model.DB;
using Notify.Model.Transfer;

namespace Notify.IRepository
{
    /// <summary>
    /// 用户仓储接口
    /// </summary>
    public interface IAccountesRepository : IRepository<Guid, MAccount>
    {
        /// <summary>
        /// 根据ID删除
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>影响结果</returns>
        void RemoveAccount(Guid id);

        /// <summary>
        /// 根据帐号查询用户
        /// </summary>
        /// <param name="accountNo">用户帐号</param>
        /// <returns>用户</returns>
        MAccount Query(string accountNo);

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="condition">查询条件</param>
        /// <returns>结果</returns>
        IEnumerable<MAccount> QueryAccountByPaging(TAccountCondition condition);
    }
}
