namespace ElasticSearch.Core.CQRS.Query
{
    public interface IQuery
    {
        bool IsCompleted { get; set; }
    }
}
