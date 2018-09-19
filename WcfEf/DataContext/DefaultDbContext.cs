using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using WcfEf.Model;

namespace WcfEf.DataContext
{
    public class DefaultDbContext : DbContext
    {
        public DefaultDbContext() : base("DefaultDbContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DefaultDbContext>());
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasKey(e => e.Id)
                .Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            base.OnModelCreating(modelBuilder);
        }
    }
}
