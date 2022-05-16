using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcquireDB_EFcore.Tables;
using AcquireDB_EFcore;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_Bootstrap.Pages
{
    public class MoviePageModel : PageModel
    {

        private List<Movie> inputMovies = new List<Movie>();
        private Movie specificMovie = new Movie();
        private MyDbContext dbContext = new MyDbContext();

        [BindProperty]
        public List<Movie> InputMovies
        {
            get { return inputMovies; }
            set { inputMovies = value; }
        }

        [BindProperty]
        public Movie SpecificMovie
        {
            get { return specificMovie; }
            set { specificMovie = value; }
        }

        [BindProperty]
        public MyDbContext DbContext
        {
            get { return dbContext;}
            set { dbContext = value; }
        }

        public void OnGet([FromRoute] int? movieId = null)
        {
            if (movieId != null)
            {
                // 8374, 1542, 603, 564, 3293
                inputMovies.Add(dbContext.Movies.Where(c => c.movieId == movieId)
                                                .Include(x => x._genreList).Include(y => y._prodCompanyList)
                                                .Include(z => z._employmentList)
                                                .FirstOrDefault());
                specificMovie = inputMovies.ElementAt(0);
            }




        }
    }
}