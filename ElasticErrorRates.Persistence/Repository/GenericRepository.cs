﻿using System.Linq.Expressions;
using ElasticErrorRates.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ElasticErrorRates.Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected DbContext Context;

        public GenericRepository(DbContext context)
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
            return await Context.Set<T>()
                .ToListAsync();
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

        public virtual async Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> wherePredicate)
        {
            return await Task.Factory.StartNew(() =>
            {
                var query = Context.Set<T>().AsNoTracking<T>();

                query = (wherePredicate == null) ? query : query.Where(wherePredicate);

                return query;
            });
        }
    }
}
