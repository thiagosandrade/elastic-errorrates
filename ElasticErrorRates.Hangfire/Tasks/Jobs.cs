using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ElasticErrorRates.Hangfire.Tasks
{
    public class Jobs 
    {
        private readonly IServiceProvider _provider;

        public Jobs(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task ImportLogs()
        {      
            var unitOfWork = _provider.GetRequiredService<IUnitOfWork>();
            var queryDispatcher = _provider.GetRequiredService<IQueryDispatcher>();
            var commandDispatcher = _provider.GetRequiredService<ICommandDispatcher>();

            //Get Logs data from database
            var extractedResult = await queryDispatcher.DispatchAsync(unitOfWork.GenericRepository<DailyRate>().GetAllAsync);

            //Push the logs into Elastic Search
            await commandDispatcher.DispatchAsync(unitOfWork.DashboardElasticRepository<DailyRate>().Bulk, extractedResult);
        }
        
    }
}
