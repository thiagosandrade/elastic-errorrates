using ElasticErrorRates.Hangfire.Extensions;
using ElasticErrorRates.Injection;
using ElasticErrorRates.SignalR.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

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
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithOrigins("http://localhost:4200")                            
                            .AllowCredentials();
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "Elastic Error Rates Core Web API",
                    TermsOfService = "None"
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

            var options = new DashboardOptions()
            {
                AppPath = "http://localhost:4200"
            };

            app.UseHangfireServer()
                .UseElasticErrorsRatesJobs(Configuration)
                .UseHangfireDashboard("/hangfire", options);

            app.UseCors("AllowAll");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.WebSocketsConfig();

            app.UseMvc(
            //routes => { routes.MapRoute("elastic", "api/{controller=Elastic}/{action=Find}/{term?}/{sort?}/{match?}"); }
            );
        }

        
    }
}
