﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria.Log;
using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.Core.Persistence
{
    public interface ILogElasticRepository<T> where T : class
    {
        Task<ElasticResponse<T>> Search(SearchCriteria criteria);
        Task<ElasticResponse<T>> SearchAggregate();
        Task<ElasticResponse<T>> Find(FindCriteria criteria);
        Task Create(T product);
        Task Delete(T product);
        Task Bulk(IEnumerable<Log> records);

        
    }
}
