namespace ElasticErrorRates.Core.Criteria.Log
{
    public class LogSearchCriteria : LogCriteria
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string HttpUrl { get; set; }
        public string ColumnField { get; set; }
        public string Term { get; set; }
    }
}
