using System.Data;
using Notify.Code.Extension;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Model.DB;

namespace Notify.Repository.Factory
{
    /// <summary>
    /// 注册验证码仓储工厂
    /// </summary>
    public class VerificationCodeFactory : IEntityFactory<MVerificationCode>
    {
        /// <summary>
        /// 创建注册验证码
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <returns>注册验证码</returns>
        public MVerificationCode BuildEntity(IDataReader reader)
        {
            return reader.ToModel<MVerificationCode>();
        }

        /// <summary>
        /// 创建注册验证码
        /// </summary>
        /// <param name="table">DataSet</param>
        /// <returns>注册验证码</returns>
        public MVerificationCode BuildEntity(DataSet table)
        {
            return table.ToModel<MVerificationCode>();
        }
    }
}