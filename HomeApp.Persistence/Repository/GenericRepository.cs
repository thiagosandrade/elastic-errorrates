using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElasticSearch.Core.Persistence;
using ElasticSearch.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ElasticSearch.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext Context;

        public GenericRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<T> FindAsync(int id)
        {
            return await await Task.Factory.StartNew(
                () => Context.Set<T>().FindAsync(id));
        }

        public virtual async Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> wherePredicate, params string[] includePredicate)
        {
            return await Task.Factory.StartNew(() =>
            {
                var query = Context.Set<T>().AsNoTracking<T>();

                if (includePredicate != null)
                {
                    foreach (string include in includePredicate)
                    {
                        query = query.Include(include);
                    }
                };

                query = (wherePredicate == null) ? query : query.Where(wherePredicate);

                return query;
            });
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await Context.Set<T>().Take(1000).ToListAsync();
        }

        public async Task<T> SaveAsync(T entity)
        {
            return await Task.Factory.StartNew(() =>
            {
                //TODO
                if (entity != null)
                {
                    Context.Set<T>().AddAsync(entity);
                    Context.Entry(entity).State = EntityState.Added;
                    
                }
                else
                {
                    Context.Set<T>().Update(entity);
                }

                return entity;
            });
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await FindAsync(id);
            await Task.Factory.StartNew(() => 
            {
                Context.Set<T>().Attach(entity);
                Context.Set<T>().Remove(entity);
            });
            
        }
    }
}
