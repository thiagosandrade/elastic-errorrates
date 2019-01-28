using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElasticErrorRates.Core.SignalR;
using SearchCriteria = ElasticErrorRates.Core.Criteria.Dashboard.SearchCriteria;

namespace ElasticErrorRates.Hangfire.Tasks
{
    public class Jobs
    {
        private readonly IHubContextWrapper _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICommandDispatcher _commandDispatcher;

        public Jobs(IServiceProvider provider)
        {
            _hubContext = provider.GetRequiredService<IHubContextWrapper>();
            _unitOfWork = provider.GetRequiredService<IUnitOfWork>();
            _queryDispatcher = provider.GetRequiredService<IQueryDispatcher>();
            _commandDispatcher = provider.GetRequiredService<ICommandDispatcher>();
        }

        public async Task ImportYesterdayLogs()
        {
            var yesterdayDate = DateTime.Now.AddDays(-7);

            var startDate = new DateTime(yesterdayDate.Year,
                yesterdayDate.Month, yesterdayDate.Day, 6, 0, 0);

            var endDate = new DateTime(DateTime.Now.Year,
                DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);

            Expression<Func<Log, bool>> predicate = srv => srv.DateTimeLogged >= startDate && srv.DateTimeLogged <= endDate;

            //Get Logs data from database
            var logs = (await _queryDispatcher.DispatchAsync(_unitOfWork.GenericRepository<Log>()
                    .FindBy, predicate))
                ?.ToList();

            if (logs?.Count > 0)
            {
                //Push the logs into Elastic Search
                await _commandDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<Log>().Bulk, logs);
            }

            await _commandDispatcher.DispatchAsync(_hubContext.SendMessage, new SignalRMessage(){ Payload = "Completed Import Yesterday Logs" });
        }

        public async Task ImportYesterdayDailyRateLogs()
        {
            Expression<Func<DailyRate, bool>> predicate = null;

            //Get Logs data from database
            var dailyRates = (await _queryDispatcher.DispatchAsync(_unitOfWork.GenericRepository<DailyRate>().FindBy, predicate))?.ToList();

            if (dailyRates?.Count > 0)
            {
                //Push the logs into Elastic Search
                await _commandDispatcher.DispatchAsync(_unitOfWork.DashboardElasticRepository<DailyRate>().Bulk, dailyRates);
            }

            await _commandDispatcher.DispatchAsync(_hubContext.SendMessage, new SignalRMessage(){ Payload = "Completed Import Yesterday Daily Rate Logs"} );
        }

        public async Task FlushOldLogs()
        {
            var oldestDate = DateTime.Now.AddDays(7);
            var searchCriteria = new SearchCriteria
            {
                EndDate = oldestDate
            };

             //Clear the logs into Elastic Search
            await _commandDispatcher.DispatchAsync(_unitOfWork.LogElasticRepository<Log>().Delete, searchCriteria);
            await _commandDispatcher.DispatchAsync(_unitOfWork.DashboardElasticRepository<DailyRate>().Delete, searchCriteria);

            await _commandDispatcher.DispatchAsync(_hubContext.SendMessage, new SignalRMessage(){ Payload = "Completed Flush Old Logs" });
        }
    }
}
