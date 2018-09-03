using ElasticErrorRates.Hangfire.Tasks;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticErrorRates.Hangfire.Extensions
{
    public static class Setup
    {
        public static IServiceCollection AddHangfireInMemory(this IServiceCollection services)
        {
            services.AddHangfire(cfg => cfg.UseMemoryStorage());

            return services;
        }

        public static IApplicationBuilder UseElasticErrorsRatesJobs([NotNull] this IApplicationBuilder appBuilder,
            IConfiguration configuration)
        {
            bool.TryParse(configuration["Jobs:IsOn"], out var jobIsOn);

            if (!jobIsOn) return appBuilder;

            var jobs = new Jobs(appBuilder.ApplicationServices);

            //Cron UTC - IE 23:00 UTC -> 00:00 Lisbon
            RecurringJob.AddOrUpdate(() => jobs.ImportYesterdayLogs(), Cron.Daily(23, 05));
            RecurringJob.AddOrUpdate(() => jobs.ImportYesterdayDailyRateLogs(), Cron.Daily(23, 07));

            return appBuilder;
        }

    }
}
