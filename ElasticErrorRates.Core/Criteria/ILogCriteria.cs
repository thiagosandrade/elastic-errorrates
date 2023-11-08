namespace ElasticErrorRates.Core.Criteria
{
    public abstract class LogCriteria
    {
        public DateTime StartDateTimeLogged { get; set; }
        public DateTime EndDateTimeLogged { get; set; }
    }
}