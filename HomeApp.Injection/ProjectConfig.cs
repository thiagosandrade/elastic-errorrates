using ElasticSearch.Core.CQRS.Command;
using ElasticSearch.Core.CQRS.Query;
using ElasticSearch.Core.Manager;
using ElasticSearch.Core.Persistence;
using ElasticSearch.CQRS.Command;
using ElasticSearch.CQRS.Query;
using ElasticSearch.Persistence.Context;
using ElasticSearch.Persistence.Manager;
using ElasticSearch.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticSearch.Injection
{
    public class ProjectConfig
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public ProjectConfig(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void Setup()
        {
            _services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            _services.AddScoped(typeof(IQueryDispatcher), typeof(QueryDispatcher));
            _services.AddScoped(typeof(ICommandDispatcher), typeof(CommandDispatcher));
            _services.AddScoped(typeof(IQueryHandler), typeof(QueryHandler));
            _services.AddScoped(typeof(ICommandHandler), typeof(CommandHandler));

            _services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            _services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            _services.AddScoped(typeof(ILogElasticRepository<>), typeof(LogElasticRepository<>));
            _services.AddScoped(typeof(IElasticRepository<>), typeof(ElasticRepository<>));
        }

        public void Autentication()
        {
            
           
        }
    }
}
