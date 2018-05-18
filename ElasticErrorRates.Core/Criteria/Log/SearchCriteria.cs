using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticErrorRates.Core.Criteria.Log
{
    public class SearchCriteria
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string HttpUrl { get; set; }
    }
}
