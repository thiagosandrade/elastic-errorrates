using System.Collections.Generic;

namespace ElasticErrorRates.Core.CQRS.Query
{
    public interface IQueryResult<T> where T : class
    {
        IEnumerable<T> ResultList { get; set; }
    }
}
