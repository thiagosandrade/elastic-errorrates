using System;
using System.Threading.Tasks;

namespace ElasticErrorRates.Core.CQRS.Command
{
    // <summary>
    /// Base interface for command handlers
    /// 
    public interface ICommandHandler
    {
        /// <summary>
        /// Executes a command handler
        /// </summary>
        /// <param name="command">The command to be used</param>
        Task ExecuteAsync(Func<Task> command);

        Task ExecuteAsync<TRequest>(Func<TRequest, Task> command, TRequest request);
    }
}
