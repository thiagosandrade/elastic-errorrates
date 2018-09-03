using ElasticErrorRates.Core.Enums;
using System;

namespace ElasticErrorRates.Core.Criteria.Dashboard
{
    public class SearchCriteria : ISearchCriteria
    {
        public Country CountryId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
