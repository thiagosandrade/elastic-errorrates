using ElasticErrorRates.Core.Enums;

namespace ElasticErrorRates.Core.Criteria.Dashboard
{
    public class GraphCriteria : LogCriteria
    {
        public Country CountryId { get; set; }
        public string TypeAggregation { get; set; }
        public int NumberOfResults { get; set; }
    }
}
