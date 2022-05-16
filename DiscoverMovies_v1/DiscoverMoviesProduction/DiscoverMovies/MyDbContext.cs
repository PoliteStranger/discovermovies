using Microsoft.EntityFrameworkCore;
using AcquireDB_EFcore.Tables;

namespace ASP_Web_Bootstrap
{
    public class MyDbContext : DbContext {
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(
                @"Data Source=discovermoviesserver.database.windows.net;Database=DiscoverMoviesDB;User ID=DiscoverMovies;Password=Discover123;"
                );
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<Genre> GenresAndMovies { get; set; }
        public DbSet<ProdCompany> ProdCompanies { get; set; }
        public DbSet<Employment> Employments { get; set; }
        public DbSet<ProducedBy> ProducedBy { get; set; }
    }

}