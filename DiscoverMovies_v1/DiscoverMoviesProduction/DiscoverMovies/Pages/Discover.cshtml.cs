using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscoverMoviesProduction;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASP_Web_Bootstrap.Pages
{

    // Mit forslag til et objekt til at holde de nævvendige film data til discover resultats siden:
    public class MovieReturn
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string PosterURL { get; set; }

        public MovieReturn(Movie inputMovie)
        {
            MovieId = inputMovie.movieId;
            Title = inputMovie._title;
            PosterURL = inputMovie._posterUrl;
        }
    }



    public class DiscoverModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DiscoverModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Jeg ville gerne se det på siden, kan bare slettes/laves om!
        [BindProperty]
        public MovieReturn movieReturn { get; set; }


        public void OnGet()
        {
            // Liste over Film IDs:
            List<int> inputMovies = new List<int>()
            {
                11398, // Snatch
                955,   // Minority Report
                180,   // Mission: Impossible II
                2787,  // Pitch Black
                107    // The Art of War
            };

            // Skaber et Discover objekt:
            Discover discover = new Discover();

            // Bruger MovieReturn typen til at tage i mod en film fra discover, med Film IDs som input:
            movieReturn = new MovieReturn(discover.DiscoverMovies(inputMovies));
        }
    }
}