using Hahn.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Hahn.Data.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Asset> Assets { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }


        #region Methods
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Configure BugFeatureRequest stocks table
            builder.Entity<Asset>().ToTable("Assets");
        }
        #endregion Methods
    }
}