using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch.Core.Models
{
    public class ElasticResponse<T> where T : class
    {
        public int TotalRecords { get; set; }
        public IEnumerable<T> Records;
    }
}
