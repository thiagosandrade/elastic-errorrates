using ElasticErrorRates.Core.CQRS.Query;

namespace ElasticErrorRates.CQRS.Query
{
    public class QueryResult<T> : IQueryResult<T> where T : class
    {
        public IEnumerable<T> ResultList { get; set; }
    }
}
