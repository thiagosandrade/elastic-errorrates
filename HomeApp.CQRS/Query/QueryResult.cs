using System.Collections.Generic;
using ElasticSearch.Core.CQRS.Query;

namespace ElasticSearch.CQRS.Query
{
    public class QueryResult<T> : IQueryResult<T> where T : class
    {
        public IEnumerable<T> ResultList { get; set; }
    }
}
