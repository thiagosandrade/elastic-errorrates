using System;

namespace ElasticErrorRates.Core.CQRS.Command
{
    public interface ICommand
    {
        bool IsCompleted { get; set; }
        DateTime UpdatedOn { get; set; }
    }
}
