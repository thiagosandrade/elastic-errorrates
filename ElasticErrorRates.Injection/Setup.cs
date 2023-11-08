using ElasticErrorRates.API.Services;
using ElasticErrorRates.Core.Cache;
using ElasticErrorRates.Core.CQRS.Command;
using ElasticErrorRates.Core.CQRS.Query;
using ElasticErrorRates.Core.Manager;
using ElasticErrorRates.Core.Persistence;
using ElasticErrorRates.Core.Services;
using ElasticErrorRates.Core.SignalR;
using ElasticErrorRates.CQRS.Command;
using ElasticErrorRates.CQRS.Query;
using ElasticErrorRates.Persistence.Context;
using ElasticErrorRates.Persistence.Manager;
using ElasticErrorRates.Persistence.Mappers;
using ElasticErrorRates.Persistence.Repository;
using ElasticErrorRates.SignalR.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticErrorRates.Injection
{
    public static class Setup
    {
        public static WebApplicationBuilder AddElasticErrorRatesInjections(this WebApplicationBuilder builder)
        {
            string? defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
            string? dailyRatesConnection = builder.Configuration.GetConnectionString("DailyRatesConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(defaultConnection));
            builder.Services.AddDbContext<DashboardContext>(options => options.UseSqlServer(dailyRatesConnection));

            builder.Services.AddScoped(typeof(IQueryDispatcher), typeof(QueryDispatcher));
            builder.Services.AddScoped(typeof(ICommandDispatcher), typeof(CommandDispatcher));
            builder.Services.AddScoped(typeof(IQueryHandler), typeof(QueryHandler));
            builder.Services.AddScoped(typeof(ICommandHandler), typeof(CommandHandler));

            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(ILogElasticRepository<>), typeof(LogElasticRepository<>));
            builder.Services.AddScoped(typeof(IDashboardElasticRepository<>), typeof(DashboardElasticRepository<>));
            builder.Services.AddScoped(typeof(IElasticContext), typeof(ElasticContext));
            builder.Services.AddScoped(typeof(ILogElasticMappers<>), typeof(LogElasticMappers<>));
            builder.Services.AddScoped(typeof(IDashboardElasticMappers<>), typeof(DashboardElasticMappers<>));
            builder.Services.AddScoped(typeof(IHubContextWrapper), typeof(HubContextWrapper));

            builder.Services.AddSingleton(typeof(ICacheService), typeof(CacheService));
            builder.Services.AddSingleton(typeof(IUserService), typeof(UserService));

            builder.Services.AddMemoryCache();

            return builder;
        }
    }
}
