using Nest;

namespace ElasticErrorRates.Core.Persistence
{
    public interface IElasticContext
    {
        void SetupIndex<T>(string defaultIndex) where T : class;
        IElasticClient ElasticClient { get; }
    }
}
