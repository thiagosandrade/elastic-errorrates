using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Hangfire.Extensions;
using ElasticErrorRates.Injection;
using ElasticErrorRates.Persistence.Context;
using ElasticErrorRates.Persistence.Seed;
using ElasticErrorRates.SignalR.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace ElasticErrorRates.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{builder.Environment.EnvironmentName}.json",
                    optional: true);


            builder
                .AddElasticErrorRatesInjections()
                .AddHangfireInMemory()
                .AddSignalRInjection();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed((host) => true)
                            .AllowCredentials();
                    });
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "Elastic Error Rates Core Web API"
                });
            });

            ConfigureJwt(builder);

            builder.Services.AddControllers();

            var app = builder.Build();

            var applicationDbContext = app.Services.GetRequiredService<ApplicationDbContext>();
            var dashboardContext = app.Services.GetRequiredService<DashboardContext>();

            SeedData.Seed(applicationDbContext, dashboardContext);

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var options = new DashboardOptions()
            {
                AppPath = "http://localhost:4200"
            };

            app
                .UseHangfireDashboard("/hangfire", options)
                .UseElasticErrorsRatesJobs();


            app.UseCors("AllowAll");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.WebSocketsConfig();

            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureJwt(WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}
