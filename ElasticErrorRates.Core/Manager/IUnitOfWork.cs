using ElasticErrorRates.Core.Persistence;

namespace ElasticErrorRates.Core.Manager
{
    public interface IUnitOfWork : IDisposable
    {
        ILogElasticRepository<T> LogElasticRepository<T>() where T : class;
        IDashboardElasticRepository<T> DashboardElasticRepository<T>() where T : class;
        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        void RollBack();
        T GetInstance<T>() where T : class;
        IGenericRepository<T> GenericRepository<T>() where T : class;
    }
}
