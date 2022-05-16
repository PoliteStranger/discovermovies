using System.ComponentModel.DataAnnotations;
using AcquireDB_EFcore.Tables;
using ASP_Web_Bootstrap.Search.Init;
using ASP_Web_Bootstrap.Search.SearchResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_Web_Bootstrap.Pages
{
    public class IndexModel : PageModel
    {
        //Input klasse fra søgning
        [BindProperty] public InputMovie theinput { get; set; } = new InputMovie();

        // initializer
        Iinitializer initsoegning = new soegning();

        [BindProperty]
        //liste til dropdown menu til søgning af film, eller personer.
        public List<string> Soegninger { get; set; } = new List<string>();

        [BindProperty]
        //liste til dropdown af år.
        public List<int> Year { get; set; } = new List<int>();

        [BindProperty]
        //liste til dropdown af alle genres hentet fra DB.
        public List<Genres> TheOriginaleGenres { get; set; } = new List<Genres>();

        // Listen over film som skal vises på en enkelt side:
        private List<Movie> movieList = new List<Movie>();

        // Listen over film som skal vises men sorteres i før den bliver vist på en enkeltside:
        private List<Movie> templiste = new List<Movie>();

        public int CurrentPageNumber;

        //number of movies loaded on each page
        private int moviesPerPage = 36;

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

        public void OnGet(int? pageNum)
        {
            //dropdown menuer til søgning af film, personer, år, genre.
            initsoegning.initSearchOption(Soegninger);
            initsoegning.initYear(Year);
            TheOriginaleGenres = initsoegning.initGenre();
            if (pageNum != null)
            {
                CurrentPageNumber = pageNum.GetValueOrDefault();
            }
            else
            {
                CurrentPageNumber = 0;
            }



            //loader med vores genres beskrevet i genres db
            using (var db = new MyDbContext())
            {
                //en counter på, da jeg ikke vil hente ALLE film (1500+!!!) + mere !!!!!!!
                int i = 0;
                // Vi gennemgår listen af film fra databasen
                foreach (Movie movie in db.Movies.Skip(moviesPerPage * CurrentPageNumber).Take(moviesPerPage).ToList())
                {
                    // og lægger dem over i listen over film som skal vises:
                    movieList.Add(movie);
                    //i++;
                    // Når den har hentet 100 film'ish, så stopper vi!
                    //if (i == 201) break;
                }
            }
        }
        
        public IActionResult OnPost(int? pageNum)
        {
            //dropdown menuer til søgning af film, personer, år, genre.
            initsoegning.initSearchOption(Soegninger);
            initsoegning.initYear(Year);
            TheOriginaleGenres = initsoegning.initGenre();
            pageNum = 0;
            if (pageNum != null)
            {
                CurrentPageNumber = pageNum.GetValueOrDefault();
            }
            else
            {
                CurrentPageNumber = 0;
            }
            
            var db = new MyDbContext();
            
            if (theinput.Searchtype == "Movie")
            {
                ISearch filmsoegning = new MovieSearchoption();
                MovieList = filmsoegning.SearchInput(theinput.Name, theinput.GenreID, theinput.Year, theinput.Searchtype).Skip(CurrentPageNumber*moviesPerPage).Take(moviesPerPage).ToList();
            }

            else if (theinput.Searchtype == "Person")
            {
                ISearch filmsoegning = new PersonSearchoption();
                MovieList = filmsoegning.SearchInput(theinput.Name, theinput.GenreID, theinput.Year, theinput.Searchtype).Skip(CurrentPageNumber * moviesPerPage).Take(moviesPerPage).ToList();
            }

            else if (theinput.Searchtype == "0")
            {
                Console.WriteLine("tjek");
                ISearch filmsoegning = new NullSearchoption();
                MovieList = filmsoegning.SearchInput(theinput.Name, theinput.GenreID, theinput.Year, theinput.Searchtype).Skip(CurrentPageNumber * moviesPerPage).Take(moviesPerPage).ToList();
                Console.WriteLine("tjek2");

            }

            
            return Page();
        }

        public class InputMovie
        {
            public string Name { get; set; } = "";
            public string GenreID { get; set; } = "0";
            public string Searchtype { get; set; } = "0";
            public string Year { get; set; } = "0";

        }
    }
}
