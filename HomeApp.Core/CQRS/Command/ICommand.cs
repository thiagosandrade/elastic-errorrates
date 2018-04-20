using System;

namespace ElasticSearch.Core.CQRS.Command
{
    public interface ICommand
    {
        bool IsCompleted { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
