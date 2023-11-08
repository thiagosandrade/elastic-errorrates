using ElasticErrorRates.Hangfire.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ElasticErrorRates.Hangfire.Extensions
{
    public static class Setup
    {
        public static WebApplicationBuilder AddHangfireInMemory(this WebApplicationBuilder builder)
        {
            string? hangFireConnection = builder.Configuration.GetConnectionString("HangFireConnection");

            builder.Services.AddHangfire(cfg =>
                cfg.UseMemoryStorage()
                //cfg.UseSqlServerStorage(hangFireConnection)
            );

            builder.Services.AddHangfireServer();

            return builder;
        }

        public static IApplicationBuilder UseElasticErrorsRatesJobs(this IApplicationBuilder application)
        {
            var configuration = application.ApplicationServices.GetRequiredService<IConfiguration>();

            bool.TryParse(configuration["Jobs:IsOn"], out bool jobIsOn);

            if (!jobIsOn) return application;

            var jobs = new Jobs(application.ApplicationServices);

            //Cron UTC - IE 23:00 UTC -> 00:00 Lisbon
            //RecurringJob.AddOrUpdate(() => jobs.FlushOldLogs(), Cron.Daily(23, 01));
            //RecurringJob.AddOrUpdate(() => jobs.ImportYesterdayLogs(), Cron.Daily(23, 05));
            //RecurringJob.AddOrUpdate(() => jobs.ImportYesterdayDailyRateLogs(), Cron.Daily(23, 07));
            RecurringJob.AddOrUpdate("UpdateLogs", () => jobs.UpdateLogs(), Cron.Daily(23, 10));
            RecurringJob.AddOrUpdate("UpdateDailyRates", () => jobs.UpdateDailyRates(), Cron.Daily(23, 15));
            
            return application;
        }

    }
}
