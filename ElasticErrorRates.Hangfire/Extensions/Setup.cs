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

        public static IApplicationBuilder UseElasticErrorsRatesJobs([NotNull] this IApplicationBuilder appBuilder, IConfiguration configuration)
        {
            bool.TryParse(configuration["Jobs:IsOn"], out bool jobIsOn);

            if (jobIsOn)
            {
                var jobs = new Jobs(appBuilder.ApplicationServices);

                RecurringJob.AddOrUpdate(() => jobs.ImportLogs(), Cron.MinuteInterval(15));
            }

            return appBuilder;
        }

    }
}
