using System;
using ElasticSearch.Core.CQRS.Command;

namespace ElasticSearch.CQRS.Command
{
    public class Command : ICommand
    {
        public bool IsCompleted { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
