using System;
using System.Threading.Tasks;

namespace ElasticSearch.Core.Manager
{
    public interface IUnitOfWork : IDisposable
    {
        T GetInstance<T>() where T : class;
        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        void RollBack();
    }
}
