namespace ElasticErrorRates.Core.Models
{
    public class LogSummary
    {
        public long HttpUrlCount { get; set; }
        public string HttpUrl { get; set; }
        public string FirstOccurrence { get; set; }
        public string LastOccurrence { get; set; }
        public string Highlight { get; set; }
    }
}
