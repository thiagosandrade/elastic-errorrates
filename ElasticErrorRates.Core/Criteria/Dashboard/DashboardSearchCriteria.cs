using ElasticErrorRates.Core.Enums;
using System;

namespace ElasticErrorRates.Core.Criteria.Dashboard
{
    public class DashboardSearchCriteria : LogCriteria
    {
        public Country CountryId { get; set; }
    }
}
