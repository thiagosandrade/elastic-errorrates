namespace ElasticErrorRates.Core.Models
{
    public class ErrorRate
    {
        public string Date { get; set; }
        public string OrderCount { get; set; }
        public string ErrorCount { get; set; }
        public string OrderValue { get; set; }
        public string ErrorPercentage { get; set; }
    }
}
