using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscoverMoviesProduction;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_Bootstrap.Pages
{
    public class MovieReturn
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string PosterURL { get; set; }
    }



    public class DiscoverModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DiscoverModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public MovieReturn DiscoverResult { get; set; }

        [BindProperty]
        public string TheMovie { get; set; }

        public void OnGet()
        {
            List<int> inputMovies = new List<int>();
            inputMovies.Add(11398);
            inputMovies.Add(955);
            inputMovies.Add(180);
            inputMovies.Add(2787);
            inputMovies.Add(107);

            Discover discover = new Discover();

            Movie ReturnMovie = discover.DiscoverMovies(inputMovies);

            TheMovie = ReturnMovie._title;
        }
    }
}