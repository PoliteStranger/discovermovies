using Microsoft.EntityFrameworkCore;
using Database.Tables;

namespace DiscoverMoviesProduction
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
<<<<<<<< HEAD:DiscoverMovies_v1/Bootstrap/ASP Web Bootstrap/MyDbContext.cs
        public DbSet<ProdCompany> ProdCompany { get; set; }
        public DbSet<Employment> Employments { get; set; }   

========
        public DbSet<Genre> GenresAndMovies { get; set; }
        public DbSet<ProdCompany> ProdCompanies { get; set; }
        public DbSet<Employment> Employments { get; set; }   
>>>>>>>> PatricksBranch:DiscoverMovies_v1/DiscoverMoviesProduction/DiscoverMovies/MyDbContext.cs

    }

}