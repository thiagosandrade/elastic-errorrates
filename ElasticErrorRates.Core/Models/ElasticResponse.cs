using System.Collections.Generic;

namespace ElasticErrorRates.Core.Models
{
    public class ElasticResponse<T> where T : class
    {
        public long TotalRecords { get; set; }
        public IEnumerable<T> Records;
    }
}
