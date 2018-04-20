using System;
using System.Threading.Tasks;
using ElasticErrorRates.Core.Manager;

namespace ElasticErrorRates.Core.CQRS.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommandDispatcher(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DispatchAsync<TRequest>(Func<Task<TRequest>> command)
        {
            try
            {
                await _unitOfWork.GetInstance<ICommandHandler>().ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DispatchAsync<TRequest>(Func<TRequest, Task> command, TRequest request)
        {
            try
            {
                await _unitOfWork.GetInstance<ICommandHandler>().ExecuteAsync(command, request);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
