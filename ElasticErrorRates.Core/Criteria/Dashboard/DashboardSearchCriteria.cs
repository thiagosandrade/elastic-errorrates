using ElasticErrorRates.Core.Enums;

namespace ElasticErrorRates.Core.Criteria.Dashboard
{
    public class DashboardSearchCriteria : LogCriteria
    {
        public Country CountryId { get; set; }
    }
}
