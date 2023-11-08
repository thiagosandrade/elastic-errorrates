namespace ElasticErrorRates.Core.CQRS.Command
{
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatches a command to its handler
        /// </summary>
        /// <typeparam name="TRequest">Command Type</typeparam>
        /// <param name="command">The command to be passed to the handler</param>
        Task DispatchAsync(Func<Task> command);
        Task DispatchAsync<TRequest>(Func<TRequest, Task> command, TRequest request);
    }
}
