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

            discover.DiscoverMovies(inputMovies);
        }
    }
}