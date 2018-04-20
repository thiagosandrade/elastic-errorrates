using System;
using ElasticErrorRates.Core.CQRS.Command;

namespace ElasticErrorRates.CQRS.Command
{
    public class Command : ICommand
    {
        public bool IsCompleted { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
