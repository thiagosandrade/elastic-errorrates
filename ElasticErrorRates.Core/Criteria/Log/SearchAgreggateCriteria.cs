using System;

namespace ElasticErrorRates.Core.Criteria.Log
{
    public class SearchAgreggateCriteria : ISearchCriteria
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
