using System;
using Notify.Infrastructure.RepositoryFramework;
using Notify.Model.DB;

namespace Notify.IRepository
{
    public interface IVerificationCodeRepository : IRepository<Guid, MVerificationCode>, IDisposable
    {
    }
}