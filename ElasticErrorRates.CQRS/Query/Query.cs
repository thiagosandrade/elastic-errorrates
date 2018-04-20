using ElasticErrorRates.Core.CQRS.Query;

namespace ElasticErrorRates.CQRS.Query
{
    public class Query : IQuery
    {
        public bool IsCompleted { get; set; }

    }
}
