using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task ImportYesterdayLogs()
        {
            var unitOfWork = _provider.GetRequiredService<IUnitOfWork>();
            var queryDispatcher = _provider.GetRequiredService<IQueryDispatcher>();
            var commandDispatcher = _provider.GetRequiredService<ICommandDispatcher>();

            var yesterdayDate = DateTime.Now.AddDays(-1);

            var startDate = new DateTime(yesterdayDate.Year,
                yesterdayDate.Month, yesterdayDate.Day, 0, 0, 0);

            var endDate = new DateTime(yesterdayDate.Year,
               yesterdayDate.Month, yesterdayDate.Day, 23, 59, 59);

            //Expression to determinate the specific range of date to query on database 
            Expression<Func<Log, bool>> predicate =
                srv => srv.DateTimeLogged >= startDate && srv.DateTimeLogged <= endDate;

            //Get Logs data from database
            var logs = (await queryDispatcher.DispatchAsync(unitOfWork.GenericRepository<Log>()
                    .FindBy, predicate))
                ?.ToList();

            if (logs?.Count > 0)
            {
                //Push the logs into Elastic Search
                await commandDispatcher.DispatchAsync(unitOfWork.LogElasticRepository<Log>().Bulk, logs);
            }
        }

        public async Task ImportYesterdayDailyRateLogs()
        {
            var unitOfWork = _provider.GetRequiredService<IUnitOfWork>();
            var queryDispatcher = _provider.GetRequiredService<IQueryDispatcher>();
            var commandDispatcher = _provider.GetRequiredService<ICommandDispatcher>();

            var yesterdayDate = DateTime.Now.AddDays(-1);

            var startDate = new DateTime(yesterdayDate.Year,
                yesterdayDate.Month, yesterdayDate.Day, 0, 0, 0);

            var endDate = new DateTime(yesterdayDate.Year,
                yesterdayDate.Month, yesterdayDate.Day, 23, 59, 59);

            //Expression to determinate the specific range of date to query on database 
            Expression<Func<DailyRate, bool>> predicate =
                srv => srv.StartDate >= startDate && srv.EndDate <= endDate;

            //Get Logs data from database
            var dailyRates = (await queryDispatcher.DispatchAsync(unitOfWork.GenericRepository<DailyRate>()
                    .FindBy, predicate))
                ?.ToList();

            if (dailyRates?.Count > 0)
            {
                //Push the logs into Elastic Search
                await commandDispatcher.DispatchAsync(unitOfWork.DashboardElasticRepository<DailyRate>().Bulk,
                    dailyRates);
            }
        }

    }
}
