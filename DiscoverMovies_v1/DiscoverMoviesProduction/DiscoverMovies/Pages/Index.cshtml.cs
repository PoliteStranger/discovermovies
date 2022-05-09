using System.ComponentModel.DataAnnotations;
using ASP_Web_Bootstrap.Search.Init;
using ASP_Web_Bootstrap.Search.SearchResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_Web_Bootstrap.Pages
{
    public class IndexModel : PageModel
    {
        //Input klasse fra søgning
        [BindProperty]
        public InputMovie theinput { get; set; } = new InputMovie();

        // initializer for søgning
        Iinitializer initsoegning = new soegning();

        [BindProperty]
        //liste til dropdown menu til søgning af film, personer, eller produktionsselskaber.
        public List<string> Soegninger { get; set; } = new List<string>();

        [BindProperty]
        //liste af alle år.
        public List<int> Year { get; set; } = new List<int>();

        [BindProperty]
        //liste af alle Productions selskaber.
        public List<int> Productioncompany { get; set; } = new List<int>();

        [BindProperty]
        //liste til alle genres hentet fra DB.
        public List<Genres> TheOriginaleGenres { get; set; } = new List<Genres>();

        // Listen over film som skal vises på en enkelt side:
        private List<Movie> movieList = new List<Movie>();

        // Listen over film som skal vises men sorteres i før den bliver vist på en enkeltside:
        private List<Movie> templiste = new List<Movie>();

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
        //public void OnGet(int size = 50, int no = 4)

        public void OnGet()
        {
            //dropdown menuer til søgning af film, personer, år, genre.
            initsoegning.initSearchOption(Soegninger);
            initsoegning.initYear(Year);
            TheOriginaleGenres = initsoegning.initGenre();

            //loader med vores genres beskrevet i genres db
            using (var db = new MyDbContext())
            {
                //en counter på, da jeg ikke vil hente ALLE film (1500+!!!) + mere !!!!!!!
                int i = 0;
                // Vi gennemgår listen af film fra databasen
                foreach (Movie movie in db.Movies)
                {
                    // og lægger dem over i listen over film som skal vises:
                    movieList.Add(movie);
                    i++;
                    // Når den har hentet 100 film'ish, så stopper vi!
                    if (i == 201) break;
                }
            }
        }
        
        public IActionResult OnPost()
        {
            //dropdown menuer til søgning af film, personer, år, genre.
            initsoegning.initSearchOption(Soegninger);
            initsoegning.initYear(Year);
            TheOriginaleGenres = initsoegning.initGenre();

            if (theinput.Searchtype == "Movie")
            {
                ISearch filmsoegning = new MovieSearchoption();
                MovieList = filmsoegning.SearchInput(theinput.Name, theinput.GenreID, theinput.Year, theinput.Searchtype);
            }

            else if (theinput.Searchtype == "Person")
            {
                ISearch filmsoegning = new PersonSearchoption();
                MovieList = filmsoegning.SearchInput(theinput.Name, theinput.GenreID, theinput.Year, theinput.Searchtype);
            }

            else if (theinput.Searchtype == "")
            {
                ISearch filmsoegning = new NullSearchoption();
                MovieList = filmsoegning.SearchInput(theinput.Name, theinput.GenreID, theinput.Year, theinput.Searchtype);                
            }
            return Page();
        }

        public class InputMovie
        {
            [StringLength(100, ErrorMessage = "Maximum length is {1}")]
            //[Display(Name = "Searching Field")]
            public string Name { get; set; } = "";
            public string GenreID { get; set; } = "0";

            public string Searchtype { get; set; } = "";
            public string Year { get; set; } = "0";
            public string Productioncompany { get; set; } = "0";

        }
    }
}
