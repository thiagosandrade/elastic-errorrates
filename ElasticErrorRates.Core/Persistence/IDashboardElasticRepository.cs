﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Criteria;
using ElasticErrorRates.Core.Criteria.Dashboard;
using ElasticErrorRates.Core.Models;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IDashboardElasticRepository<T> where T : class
    {
        Task<ElasticResponse<T>> Search(DashboardSearchCriteria criteria);
        Task<ElasticResponse<T>> SearchAggregate(GraphCriteria criteria);
        Task Bulk(IEnumerable<DailyRate> records);
        Task Delete(LogCriteria criteria);
        
    }
}
