﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticErrorRates.Core.Criteria.Log
{
    public class FindCriteria : IFindCriteria
    {
        public string ColumnField { get; set; }
        public string HttpUrl { get; set; }
        public string Term { get; set; }
    }
}
