using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticErrorRates.Core.Criteria.Dashboard
{
    public class GraphCriteria : ISearchCriteria
    {
        public int CountryId { get; set; }
        public string TypeAggregation { get; set; }
        public int NumberOfResults { get; set; }
    }
}
