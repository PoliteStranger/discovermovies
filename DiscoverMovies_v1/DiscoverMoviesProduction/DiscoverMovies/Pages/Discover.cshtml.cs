using System.Collections.Specialized;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscoverMoviesProduction;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ASP_Web_Bootstrap.Pages
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

        private readonly ILogger<IndexModel> _logger;

        public DiscoverModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet(
            [FromRoute] int? movieParam1 = null,
            [FromRoute] int? movieParam2 = null,
            [FromRoute] int? movieParam3 = null,
            [FromRoute] int? movieParam4 = null,
            [FromRoute] int? movieParam5 = null
        )
        {
            using (var db = new MyDbContext())
            {
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
                                    MovieList.Add(db.Movies.FirstOrDefault(i => i.movieId == movieParam5));
                                    //use algorithm
                                    displayAddMoveForm = false;
                                }
                            }
                        }
                    }
                }

            }
            //check for duplicate values in movie list - return error to client if they've inputted duplicates
            if (MovieList.Count() != MovieList.Distinct().Count())
                return Content("ERROR: duplicate movies were added to list");

             return Page();
        }

        public IActionResult OnPost()
        {
            //brug Request.RouteValues til at få en dictionary af key-value pairs... lav det om til en liste??
            Console.WriteLine($"look at this {Request.RouteValues.Values.ToString()}");

            Console.WriteLine($"Size of movie list: {MovieList.Count}");



            if (MovieList.Count < 5)
            {
                using (var db = new MyDbContext())
                {
                    if (theinput.Name != "")
                    {
                        var movieSearchResult = db.Movies.FirstOrDefault(i => i._title == theinput.Name);
                        if (movieSearchResult != null)
                        {
                            string redirectUrl = Request.Path;
                            if (!redirectUrl.EndsWith('/'))
                                redirectUrl += '/';
                            redirectUrl += movieSearchResult.movieId.ToString();

                            return Redirect(redirectUrl);
                        }
                    }
                }
            }

            return Page();
        }

        public IActionResult OnPostClear()
        {
            return Redirect("/discover");
        }
    }
}
