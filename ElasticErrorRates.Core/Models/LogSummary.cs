using System;

namespace ElasticErrorRates.Persistence.Repository
{
    public class LogSummary
    {
        public long HttpUrlCount { get; set; }
        public string HttpUrl { get; set; }
        public string FirstOccurrence { get; set; }
        public string LastOccurrence { get; set; }
    }
}
