using Microsoft.EntityFrameworkCore;

namespace Examples.Db
{
    public class ExampleContext : DbContext
    {
        public ExampleContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<ExampleEntity> Examples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=exampleDb;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
