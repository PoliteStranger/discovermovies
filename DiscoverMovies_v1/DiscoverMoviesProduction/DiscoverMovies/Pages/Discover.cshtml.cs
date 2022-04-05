using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASP_Web_Bootstrap;

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
                inputMovies.Add(db.Movies.Find(8374));
                inputMovies.Add(db.Movies.Find(1542));
                inputMovies.Add(db.Movies.Find(603));
                inputMovies.Add(db.Movies.Find(564));
                inputMovies.Add(db.Movies.Find(3293));
            }



            discover.DiscoverMovies(inputMovies);
        }
    }
}