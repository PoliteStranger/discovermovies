using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscoverMoviesProduction;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_Bootstrap.Pages
{
    public class DiscoverModel : PageModel
    {
        [BindProperty]
        public IndexModel.InputMovie theinput { get; set; } = new IndexModel.InputMovie();

        // Listen over film som skal vises på en enkelt side:
        private List<Movie> movieList = new List<Movie>();

        // Den bindes, så vi kan tilgå den inde fra vores Razor page
        [BindProperty]
        public List<Movie> MovieList
        {
            get { return movieList; }
            set { movieList = value; }
        }

        private readonly ILogger<IndexModel> _logger;
        public DiscoverModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            //            List<Movie> inputMovies = new List<Movie>();
            //            
            //            Discover discover = new Discover();
            //
            //            using (var db = new MyDbContext())
            //            {
            //                // 5 random film som er i db!
            //                // 8374, 1542, 603, 564, 3293
            //                // Til 27205, 329, 553, 271110, 862
            //                inputMovies.Add(db.Movies.Where(c => c.movieId == 11398).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
            //                inputMovies.Add(db.Movies.Where(c => c.movieId == 955).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
            //                inputMovies.Add(db.Movies.Where(c => c.movieId == 180).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
            //                inputMovies.Add(db.Movies.Where(c => c.movieId == 2787).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
            //                inputMovies.Add(db.Movies.Where(c => c.movieId == 107).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).FirstOrDefault());
            //            }
            //
            //
            //            discover.DiscoverMovies(inputMovies);
        }

        public IActionResult OnPost()
        {
            using (var db = new MyDbContext())
            {
                if (theinput.Name != "")
                {
                    var movieSearchResult = db.Movies.FirstOrDefault(i => i._title == theinput.Name);
                    if (movieSearchResult != null)
                    {
                        MovieList.Add(movieSearchResult);
                    }
                }

//                var theMovies = db.Movies.ToList();
//                //hver film i theMoviesByGenres listen bliver slået op i movie db
//                //film på begge lister bliver tilføjet til temp listen som sættes til Movielisten
//                foreach (var item in theMovies)
//                {
//                    templiste.Add(item);
//                }

                return Page();
            }
        }
    }
}