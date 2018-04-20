using ElasticSearch.Core.CQRS.Query;

namespace ElasticSearch.CQRS.Query
{
    public class Query : IQuery
    {
        public bool IsCompleted { get; set; }

    }
}
