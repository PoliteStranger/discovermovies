using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASP_Web_Bootstrap;
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
                // 8374, 1542, 603, 564, 3293
                inputMovies.Add(db.Movies.Where(c => c.movieId == 8374).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 1542).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 603).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 564).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
                inputMovies.Add(db.Movies.Where(c => c.movieId == 3293).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
            }



            discover.DiscoverMovies(inputMovies);
        }
    }
}