using System.ComponentModel.DataAnnotations;
using AcquireDB_EFcore.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_Web_Bootstrap.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public InputMovie theinput { get; set; } = new InputMovie();
        public List<Person> Persons { get; set; } = new List<Person>();

        [BindProperty]
        //liste over muligheder for søgninger.
        public List<string> Soegninger { get; set; } = new List<string>();

        [BindProperty]
        //liste over alle genres hentet fra DB i context.
        public List<int> Year { get; set; } = new List<int>();

        [BindProperty]
        //liste over alle genres hentet fra DB i context.
        public List<Genres> TheOriginaleGenres { get; set; } = new List<Genres>();
        
        // Listen over film som skal vises på en enkelt side:
        private List<Movie> movieList = new List<Movie>();
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
            // Skaber og bruger vores database objekt:
            using (var db = new MyDbContext())
            {
                Soegninger.Add("Movie");
                Soegninger.Add("Person");

                for (int j = 1980; j<2000; j++)
                {
                    Year.Add(j);
                }
                //loader med vores genres beskrevet i genres db
                TheOriginaleGenres = db.Genres.ToList();

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
            using (var db = new MyDbContext())
            {
                //dropdown menu skal sættes til at være genrelisten
                Soegninger.Add("Movie");
                Soegninger.Add("Person");

                //var query = (from m in inputMovies.ToList()
                //             join e in db.Employments.ToList()
                //             on m.movieId equals e._movieId
                //             join eb in db.Employments.ToList()
                //             on m.movieId equals eb._movieId
                //             where e._personId == personPair.personA
                //             where eb._personId == personPair.personB
                //             select new
                //             {
                //                 MovieId = e._movieId,
                //                 MovieTitle = m._title,
                //                 Director = personPair.personA,
                //                 Producer = personPair.personB
                //             }).ToList();

                for (int j = 1980; j<2000; j++)
                {
                    Year.Add(j);
                }
                
                TheOriginaleGenres = db.Genres.ToList();

                if (theinput.Name != "")
                {
                    //tjekker om det er en person som er sat
                    if (theinput.Searchtype == "Person")
                    {
                        //kigger efter personens navn i person db

                        Persons = db.Persons.Where(i => i._Personname.Contains(theinput.Name)).ToList();
                        foreach (var person in Persons)
                        {
                            //kigger efter hver person i employments for at finde movie id'er
                            var liste = db.Employments.Where(i=>i._personId==person._personId).ToList();
                            foreach (var movie in liste)
                            {
                                var liste1 = db.Movies.Where(i => i.movieId==movie._movieId).ToList();
                                foreach (var film in liste1)
                                {
                                    templiste.Add(film);
                                }
                                MovieList = templiste;
                            }
                        }
                    }
                    else
                    {
                        templiste = db.Movies.Where(i => i._title.Contains(theinput.Name)).ToList();
                        MovieList = templiste;
                    }
                }

                //Tjekkes om genreid er sat.
                if (theinput.GenreID != "0")
                {
                    //cast theinput.GenreID til int
                    int genreid = Int32.Parse(theinput.GenreID);

                    if (templiste.Count >= 1)
                    {
                        //.ToList sikrer, at vi caster til en ny liste.
                        foreach (var item in templiste.ToList())
                        {
                            var tjek = db.GenresAndMovies
                                .Any(i => i._movieId == item.movieId && i._genreId == genreid);

                            if (tjek == false)
                            {
                                templiste.Remove(item);
                            }
                        }
                        MovieList = templiste;
                    }

                    else
                    {
                        var theMoviesByGenres = db.GenresAndMovies.Where(i => i._genreId == genreid).ToList();
                        //hver film i theMoviesByGenres listen bliver slået op i movie db
                        //film på begge lister bliver tilføjet til temp listen som sættes til Movielisten
                        foreach (var item in theMoviesByGenres)
                        {
                            var film = db.Movies.Find(item._movieId);
                            if (film!=null)
                            {
                                templiste.Add(film);
                            }
                        }

                        MovieList = templiste;
                    }
                }

                if (theinput.Year != "0")
                {
                    var templisten = db.Movies.ToList();
                    int aar = Int32.Parse(theinput.Year);
                    templiste.Clear();
                    foreach (var item in templisten)
                    {
                        var ting = item._releaseDate.ToString();
                        
                        Console.WriteLine(ting);
                    }

                    //var query = (from m in inputMovies.ToList()
                    //             join e in db.Employments.ToList()
                    //             on m.movieId equals e._movieId
                    //             join eb in db.Employments.ToList()
                    //             on m.movieId equals eb._movieId
                    //             where e._personId == personPair.personA
                    //             where eb._personId == personPair.personB
                    //             select new
                    //             {
                    //                 MovieId = e._movieId,
                    //                 MovieTitle = m._title,
                    //                 Director = personPair.personA,
                    //                 Producer = personPair.personB
                    //             }).ToList();



                }


            }
            return Page();
        }


        // JUST IN CASE !
        /*
        public IActionResult OnPost()
        {
            Console.WriteLine("Itemname: {0}", theinput.Name);
            Console.WriteLine("{0}",theinput.GenreID);
            using (var db = new MyDbContext())
            {
                if (theinput.IsMovie == true)
                {
                    MovieList = db.Movies.Where(i => i._title.Contains(theinput.Name)).ToList();
                    foreach (var item in tings)
                    {
                        Console.WriteLine(tings.Count);
                        Console.WriteLine(theinput.GenreID);
                    }
                }
                if (theinput.IsGenre == true)
                {
                    var thegenrelist = db.Genres.Where(i => i._Genrename == theinput.Name).ToList();
                    Genres thefirst=thegenrelist.First();
                    var theMoviesByGenres = db.GenresAndMovies.Where(i => i._genreId == thefirst._genreId).ToList();
                    foreach (var item in theMoviesByGenres)
                    {
                        var film = db.Movies.Find(item._movieId);
                        tings.Add(film);
                    }
                    MovieList = tings;
                }
                else
                {
                    return Page();
                }
            }
            return Page();
        }
        */

        public class InputMovie
        {
            [StringLength(100, ErrorMessage = "Maximum length is {1}")]
            [Display(Name = "Searching Field")]
            public string Name { get; set; } = "";

            [StringLength(100, ErrorMessage = "Maximum length is {1}")]
            public string GenreID { get; set; } = "0";

            public string Searchtype { get; set; }

            public string Year { get; set; }

            //public bool IsPerson { get; set; }
            //public bool IsProdCompany { get; set; }

            //public bool IsGenre { get; set; }

        }
    }
}