using System;
using System.Threading.Tasks;
using ElasticErrorRates.Core.CQRS.Command;

namespace ElasticErrorRates.CQRS.Command
{
    public class CommandHandler : ICommandHandler 
    {
        public async Task ExecuteAsync(Func<Task> command)
        {
            try
            {
                await Task.Run(async () => { await command.Invoke(); });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task ExecuteAsync<TRequest>(Func<TRequest, Task> command, TRequest request)
        {
            try
            {
                await Task.Run(async () => { await command.Invoke(request); });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
