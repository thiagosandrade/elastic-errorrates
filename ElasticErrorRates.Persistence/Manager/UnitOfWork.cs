using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Core.Persistence;
using ElasticErrorRates.Persistence.Context;
using ElasticErrorRates.Persistence.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticErrorRates.Persistence.Manager
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DashboardContext _dsContext;
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceprovider;
        private bool _disposed;
        private IDbContextTransaction _transaction;

        public UnitOfWork(DashboardContext dsContext, ApplicationDbContext context, IServiceProvider serviceprovider)
        {
            _dsContext = dsContext;
            _context = context;
            _serviceprovider = serviceprovider;
        }

        public T GetInstance<T>() where T : class
        {
            return _serviceprovider.GetRequiredService<T>();
        }

        public ILogElasticRepository<T> LogElasticRepository<T>() where T : class 
        {
            return _serviceprovider.GetRequiredService<ILogElasticRepository<T>>();
        }

        public IDashboardElasticRepository<T> DashboardElasticRepository<T>() where T : class 
        {
            return _serviceprovider.GetRequiredService<IDashboardElasticRepository<T>>();
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            if (typeof(T) == typeof(Log))
            {
                return new GenericRepository<T>(_context);
            }

            if (typeof(T) == typeof(DailyRate))
            {
                return new GenericRepository<T>(_dsContext);
            }

            return null;

        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            using (_transaction = _context.Database.BeginTransactionAsync().Result)
            {
                try
                {
                    if (_context != null)
                        await Task.Factory.StartNew(() => _context.SaveChangesAsync());

                    _transaction.Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
                    throw new Exception("Commit", ex);
                }
            }
        }

        public void RollBack()
        {
            try
            {
                _transaction?.Rollback();
            }
            catch (Exception ex)
            {
                throw new Exception("RollBack", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_context != null && !_disposed && disposing)
                _context.Dispose();

            _disposed = true;
        }
    }
}
