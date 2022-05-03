using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscoverMoviesProduction;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_Bootstrap.Pages
{
    public class DiscoverModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DiscoverModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            List<Movie> inputMovies = new List<Movie>();
            
            Discover discover = new Discover();

            using (var db = new MyDbContext())
            {
                // 5 random film som er i db!
                // 8374, 1542, 603, 564, 3293
                // Til 27205, 329, 553, 271110, 862
                inputMovies.Add(db.Movies.Where(c => c.movieId == 11398).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 955).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 1493).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 2787).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 107).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
            }
            
            discover.DiscoverMovies(inputMovies);
        }
    }
}