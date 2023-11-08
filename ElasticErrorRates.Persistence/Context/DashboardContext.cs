using ElasticErrorRates.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ElasticErrorRates.Persistence.Context
{
    public class DashboardContext : DbContext
    {
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options) { }

        public DbSet<DailyRate> DailyRates { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
