using Microsoft.EntityFrameworkCore;
using MyFirstProject;

namespace MyFirstProject {
    public class MyDbContext : DbContext {
        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(
                @"Data Source=127.0.0.1,1433;Database=MovieDB;User ID=SA;Password=ZyurnYNmhXBP37Jdpr7a;"
                );

        }


        public DbSet<Movie> Movies { get; set; }
    }

}