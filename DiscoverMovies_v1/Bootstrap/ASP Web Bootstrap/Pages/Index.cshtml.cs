using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_Web_Bootstrap.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public InputMovie theinput { get; set; } = new InputMovie();

        // Listen over film som skal vises på en enkelt side:
        private List<Movie> movieList = new List<Movie>();

        private List<Movie> tings = new List<Movie>();


        // Den bindes, så vi kan tilgå den inde fra vores Razor page
        [BindProperty]
        public List<Movie> MovieList
        {
            get { return movieList; }
            set { movieList = value; }
        }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // Når vi besøger forsiden får vi følgende:
        public void OnGet()
        {
            // Skaber og bruger vores database objekt:
            using (var db = new MyDbContext())
            {
                // Jeg har en counter på, da jeg ikke vil hente ALLE film (1500+!!!) + mere !!!!!!!
                int i = 0;
                // Vi gennemgår listen af film fra databasen
                foreach(Movie movie in db.Movies)
                {
                    // og lægger dem over i listen over film som skal vises:
                    movieList.Add(movie);
                    i++;
                    // Når den har hentet 100 film'ish, så stopper vi!
                    if (i == 101) break;
                }
            }
        }

        public IActionResult OnPost()
        {
            Console.WriteLine("Itemname: {0}", theinput.Name);

            using (var db = new MyDbContext())
            {
                if (theinput.IsMovie == true)
                {
                    MovieList = db.Movies.Where(i => i._title.Contains(theinput.Name)).ToList();

                    foreach (var item in tings)
                    {
                        Console.WriteLine(tings.Count);
                    }
                }

                if (theinput.IsGenre == true)
                {
                    var thegenrelist = db.Genres.Where(i => i._Genrename == theinput.Name).ToList();

                    //vi tager det første OG ENESTE element i listen
                    Genres thegenreID = thegenrelist.First();
                    var theMoviesOfGenres = db.GenresAndMovies.Where(i=>i._genreId == thegenreID._genreId).ToList();

                    foreach (var item in theMoviesOfGenres)
                    {
                        Console.WriteLine(theMoviesOfGenres.Count);
                        Console.WriteLine(item.Movies._title);
                    }
                    //Console.WriteLine(thegenreID._Genrename);
                    //Console.WriteLine(thegenreID._genreId);

                    //var thegenre =db.Movies.Where()



                    //var thegenre = db.Genre.Where(i => i._Genrename == theinput.Name).ToList();


                    //Pga. Genreid, kan vi nu slå filmene op



                    //var thegenre123 = db.GenresAndMovies.Where(i => i._genreId == thegenreID._genreId).ToList();

                    //MovieList = db.Movies.Where(i => i._genreList.Contains(theinput.Name)).ToList();

                    //foreach (var item in thegenre123)
                    //{
                    //    Console.WriteLine(tings.Count);
                    //    Console.WriteLine(item.Movies);
                    //}
                }
                else
                {
                    return Page();
                }
            }
            return Page();
        }

        public class InputMovie
        {
            [Required]
            [StringLength(100, ErrorMessage = "Maximum length is {1}")]
            [Display(Name = "Moviename")]   
            public string Name { get; set; } = "";
            public bool IsMovie { get; set; }
            public bool IsGenre { get; set; }

        }
    }
}