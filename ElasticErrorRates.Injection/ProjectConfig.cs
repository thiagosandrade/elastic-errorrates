using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Persistence;
using ElasticErrorRates.Core.SignalR;
using ElasticErrorRates.CQRS.Command;
using ElasticErrorRates.CQRS.Query;
using ElasticErrorRates.Persistence.Context;
using ElasticErrorRates.Persistence.Manager;
using ElasticErrorRates.Persistence.Mappers;
using ElasticErrorRates.Persistence.Repository;
using ElasticErrorRates.SignalR.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticErrorRates.Injection
{
    public static class ProjectConfig
    {
        public static void AddElasticErrorRatesInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<DashboardContext>(options => options.UseSqlServer(configuration.GetConnectionString("DailyRatesConnection")));

            services.AddScoped(typeof(IQueryDispatcher), typeof(QueryDispatcher));
            services.AddScoped(typeof(ICommandDispatcher), typeof(CommandDispatcher));
            services.AddScoped(typeof(IQueryHandler), typeof(QueryHandler));
            services.AddScoped(typeof(ICommandHandler), typeof(CommandHandler));

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(ILogElasticRepository<>), typeof(LogElasticRepository<>));
            services.AddScoped(typeof(IDashboardElasticRepository<>), typeof(DashboardElasticRepository<>));
            services.AddScoped(typeof(IElasticContext), typeof(ElasticContext));
            services.AddScoped(typeof(ILogElasticMappers<>), typeof(LogElasticMappers<>));
            services.AddScoped(typeof(IDashboardElasticMappers<>), typeof(DashboardElasticMappers<>));
            services.AddScoped(typeof(ISignalRHub), typeof(SignalRHub));
        }
    }
}
