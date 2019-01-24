using ElasticErrorRates.Hangfire.Extensions;
using ElasticErrorRates.Injection;
using ElasticErrorRates.SignalR.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticErrorRates.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.AddElasticErrorRatesInjections(Configuration);

            services.AddHangfireInMemory();

            services.AddSignalRInjection();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer()
                .UseElasticErrorsRatesJobs(Configuration)
                .UseHangfireDashboard();

            app.UseCors("AllowAll");

            app.WebSocketsConfig();

            app.UseMvc(
            //routes => { routes.MapRoute("elastic", "api/{controller=Elastic}/{action=Find}/{term?}/{sort?}/{match?}"); }
            );
        }

        
    }
}
