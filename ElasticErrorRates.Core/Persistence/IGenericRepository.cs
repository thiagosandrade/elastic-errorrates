﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> FindAsync(int id);
        Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> wherePredicate, params string[] includePredicate);
        Task<IQueryable<T>> FindBy(Expression<Func<T, bool>> wherePredicate);
        Task<List<T>> GetAllAsync();
        Task<T> SaveAsync(T entity);
        Task RemoveAsync(int id);
    }
}
