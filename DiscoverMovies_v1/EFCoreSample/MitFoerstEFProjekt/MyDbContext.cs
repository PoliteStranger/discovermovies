using Microsoft.EntityFrameworkCore;
using AcquireDB_EFcore.Tables;

namespace AcquireDB_EFcore
{
    public class MyDbContext : DbContext {
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(
                @"Data Source=127.0.0.1,1433;Database=MovieDB;User ID=SA;Password=ZyurnYNmhXBP37Jdpr7a;"
                );

        }


        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<ProdCompany> ProdCompany { get; set; }
    }

}