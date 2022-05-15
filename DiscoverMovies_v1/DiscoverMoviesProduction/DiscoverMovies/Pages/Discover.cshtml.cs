using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscoverMoviesProduction;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DiscoverMoviesProduction.Pages
{
    public class DiscoverModel : PageModel
    {
        [BindProperty]
        public IndexModel.InputMovie theinput { get; set; } = new IndexModel.InputMovie();

        [BindProperty] public bool displayAddMoveForm { get; set; } = true;
        // Listen over film som skal vises på en enkelt side:
        private List<Movie> movieList = new List<Movie>();

        // Den bindes, så vi kan tilgå den inde fra vores Razor page
        [BindProperty]
        public List<Movie> MovieList
        {
            get { return movieList; }
            set { movieList = value; }
        }

        private bool performAlgorithm = false;
        [BindProperty] public Movie AlgorithmMovieResult { get; set; } = null;

        private readonly ILogger<IndexModel> _logger;

        public DiscoverModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        /*
         * OnGet kaldes hver gang siden renderes. Heri læser den alle parametre i Url, og søger/sætter satte parameter (som er filmId) til
         * deres tilknyttet film. OnGet sørger også for at gemme/vise elementer ved at ændre på binded properties. OnGet eksekverer også Discover
         * algoritmen, ved brug af en hjælper funktion.
         */
        public IActionResult OnGet(
            //movie id parametre fra url
            [FromRoute] int? movieParam1 = null,
            [FromRoute] int? movieParam2 = null,
            [FromRoute] int? movieParam3 = null,
            [FromRoute] int? movieParam4 = null,
            [FromRoute] int? movieParam5 = null
        )
        {
            using (var db = new MyDbContext())
            {
                //nested if sætninger, som sekventielt tjekker for hvorvidt filmparametre er sat og søger dernæst efter dem i db
                if (movieParam1 != null)
                {

                    MovieList.Add(db.Movies.FirstOrDefault(i => i.movieId == movieParam1));

                    if (movieParam2 != null)
                    {
                        MovieList.Add(db.Movies.FirstOrDefault(i => i.movieId == movieParam2));

                        if (movieParam3 != null)
                        {
                            MovieList.Add(db.Movies.FirstOrDefault(i => i.movieId == movieParam3));

                            if (movieParam4 != null)
                            {
                                MovieList.Add(db.Movies.FirstOrDefault(i => i.movieId == movieParam4));

                                if (movieParam5 != null)
                                {
                                    //hvis 5 film er sat i url, skal discover algoritmen eksekveres
                                    MovieList.Add(db.Movies.FirstOrDefault(i => i.movieId == movieParam5));
                                    //disable add movie form
                                    displayAddMoveForm = false;
                                    //use algorithm
                                    performAlgorithm = true;
                                }
                            }
                        }
                    }
                }
            }
            //check for duplicate values in movie list - return error to client if they've inputted duplicates
            if (MovieList.Count() != MovieList.Distinct().Count())
                return Content("ERROR: duplicate movies were added to list");

            if (performAlgorithm)
            {
                //AlgorithmMovieResult er en binded property, som vil blive vist på client-side hvis den ikke er null 
                AlgorithmMovieResult = GetDiscoverdMovie(MovieList);
            }

            return Page();
        }

        //perform algorithm helper function
        private Movie GetDiscoverdMovie(List<Movie> MovieParamList)
        {
            if (MovieParamList.Count != 5) return null;
            //create list of movie ids for algorithm
            List<int> idList = new List<int>();
            foreach (var movie in MovieList)
            {
                idList.Add(movie.movieId);
            }
            //create discover instance
            Discover discover = new Discover();

            //perform algorithm
            return discover.DiscoverMovies(idList);

        }

        /*
         * Post metode kalde for tilføjelse af film. Denne funktion søger efter filmen skrevet ind i søgefeltet, og finder det første
         * resultat i databasen. Hvis filmen ikke findes vil den intet gøre.
         * Hvis filmen findes, tilføjer den database id'en på filmen til dens nuværende url, hvorefter den laver et redirect til dette url.
         * Dette betyder at den netop tilføjet film vil være en ny parameter i get metoden, når siden renderes igen.
         */
        public IActionResult OnPost()
        {
            //variable til film i søgefunktion (sættes kun hvis filmen findes i db)
            Movie movieSearchResult = null;

            //søg kun i db hvis der er blevet skrevet i søgefeltet 
            if (theinput.Name != "")
            {
                using (var db = new MyDbContext())
                {
                    //find den første film med samme navn som navnet i inputfeltet
                    movieSearchResult = db.Movies.FirstOrDefault(i => i._title == theinput.Name);

                }
            }

            //hvis filmen blev fundet bliver film id'et på denne sammenkædet med det nuværende id, hvorefter der laves et redirect på dét nye url
            if (movieSearchResult != null)
            {
                string redirectUrl = Request.Path;
                if (!redirectUrl.EndsWith('/'))
                    redirectUrl += '/';
                redirectUrl += movieSearchResult.movieId.ToString();

                return Redirect(redirectUrl);
            }



            //returner blot nuværende side hvis filmen ikke blev fundet i db
            return Page();
        }

        //ryd tilføjet film ved blot at redirect til siden uden parametre
        public IActionResult OnPostClear()
        {
            return Redirect("/discover");
        }
    }
}
