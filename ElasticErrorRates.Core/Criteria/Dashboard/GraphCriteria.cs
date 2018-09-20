using System;
using System.Collections.Generic;
using System.Text;
using ElasticErrorRates.Core.Enums;

namespace ElasticErrorRates.Core.Criteria.Dashboard
{
    public class GraphCriteria : ISearchCriteria
    {
        public Country CountryId { get; set; }
        public string TypeAggregation { get; set; }
        public int NumberOfResults { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
