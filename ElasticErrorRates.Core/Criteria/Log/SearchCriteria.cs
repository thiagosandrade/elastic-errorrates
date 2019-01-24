namespace ElasticErrorRates.Core.Criteria.Log
{
    public class SearchCriteria : ISearchCriteria
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string HttpUrl { get; set; }
    }
}
