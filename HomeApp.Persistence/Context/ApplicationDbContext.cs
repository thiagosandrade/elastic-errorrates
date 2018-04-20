using System.Threading;
using System.Threading.Tasks;
using ElasticSearch.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ElasticSearch.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Log> Logs { get; set; }

        //public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        //{
        //    return base.Set<TEntity>();
        //}
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
