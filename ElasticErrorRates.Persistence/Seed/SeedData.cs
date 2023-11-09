using Bogus;
using ElasticErrorRates.Core.Enums;
using ElasticErrorRates.Core.Models;
using ElasticErrorRates.Persistence.Context;

namespace ElasticErrorRates.Persistence.Seed
{
    public class SeedData
    {
        public static void Seed(ApplicationDbContext applicationDbContext, DashboardContext dashboardContext)
        {
            if (applicationDbContext.Logs.Count().Equals(0))
            {
                var logData = GenerateLogSeed();

                foreach (var log in logData.Chunk(1000))
                {
                    applicationDbContext.Logs.AddRange(log);

                    applicationDbContext.SaveChanges(acceptAllChangesOnSuccess: false);
                }

                var dailyRateData = GenerateDailyRateSeed();

                foreach (var dailyRate in dailyRateData.Chunk(1000))
                {
                    dashboardContext.DailyRates.AddRange(dailyRate);

                    dashboardContext.SaveChanges(acceptAllChangesOnSuccess: false);
                }
            }
        }

        private static IEnumerable<DailyRate> GenerateDailyRateSeed()
        {
            var logs = new Faker<DailyRate>()
                .RuleFor(rate => rate.CountryId, faker => faker.PickRandom<Country>())
                .RuleFor(rate => rate.StartDate, faker => faker.Date.Past(1))
                .Rules((faker, rate) =>
                {
                    var date = faker.Date.Past(1);

                    var startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    var endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                    rate.StartDate = startDate;
                    rate.EndDate = endDate;

                })
                .RuleFor(rate => rate.ErrorCount, faker => faker.Random.Int(0, 50))
                .RuleFor(rate => rate.OrderCount, faker => faker.Random.Int(0, 1000))
                .RuleFor(rate => rate.OrderValue, faker => faker.Random.Double(0, 1000));

            return logs.Generate(10000);
        }

        private static IEnumerable<Log> GenerateLogSeed()
        {
            var logs = new Faker<Log>()
                .RuleFor(rate => rate.Level, faker => faker.PickRandom<LogLevel>().ToString())
                .RuleFor(rate => rate.Message, faker => faker.Lorem.Text())
                .RuleFor(rate => rate.Source, faker => faker.Lorem.Text())
                .RuleFor(rate => rate.Exception, faker => faker.Lorem.Text())
                .RuleFor(rate => rate.HttpUrl, faker => faker.Internet.Url())
                .RuleFor(rate => rate.DateTimeLogged, faker => faker.Date.Past())
                .RuleFor(rate => rate.CountryId, faker => faker.PickRandom<Country>());

            return logs.Generate(10000);
        }
    }
}
