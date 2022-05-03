using System.ComponentModel.DataAnnotations;
using Database.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscoverMoviesProduction;

namespace ASP_Web_Bootstrap.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public InputMovie theinput { get; set; } = new InputMovie();
        public List<Person> Persons { get; set; } = new List<Person>();

        [BindProperty]
        //liste til dropdown menu til søgning af film, personer, eller produktionsselskaber.
        public List<string> Soegninger { get; set; } = new List<string>();

        [BindProperty]
        //liste af alle år.
        public List<int> Year { get; set; } = new List<int>();

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
            //dropdown menu til søgning af film eller personer
            Soegninger.Add("Movie");
            Soegninger.Add("Person");

            //dropdown menu til år
            for (int j = 1990; j<2020; j++)
            {
                Year.Add(j);
            }

            //henter genres ned til dropdownmenu Genre
            using (var db = new MyDbContext())
            {
                TheOriginaleGenres = db.Genres.ToList();
            }

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
            //dropdown menu til søgning af film eller personer
            Soegninger.Add("Movie");
            Soegninger.Add("Person");
            templiste.Clear();

            Console.WriteLine(theinput.Name);
            //dropdown menu til år
            for (int j = 1995; j<2020; j++)
            {
                Year.Add(j);
            }
            
            //henter genres ned til dropdownmenu Genre
            using (var db = new MyDbContext())
            {
                TheOriginaleGenres = db.Genres.ToList();
            }

            if (theinput.Searchtype == "Movie")
            {
                using (var db = new MyDbContext())
                {
                    var query = (from m in db.Movies
                                 join gm in db.GenresAndMovies
                                 on m.movieId equals gm._movieId
                                 join g in db.Genres
                                 on gm._genreId equals g._genreId
                                 where (m._title.Contains(theinput.Name) || theinput.Name == "")
                                 && (theinput.GenreID == "0" || gm._genreId == Int32.Parse(theinput.GenreID))
                                 && (m._releaseDate.Value.Year == Int32.Parse(theinput.Year) || theinput.Year == "0")
                                 && theinput.Searchtype == "Movie" || theinput.Searchtype == null

                                 select new
                                 {
                                     movieid = m.movieId,
                                     movietitel = m._title,
                                     movieposter = m._posterUrl,
                                 }
                                 ).ToList().Distinct();

                    foreach (var item in query)
                    {
                        Movie tempmovie = new Movie();
                        tempmovie._title = item.movietitel;
                        tempmovie._posterUrl = item.movieposter;
                        tempmovie.movieId = item.movieid;
                        templiste.Add(tempmovie);
                    }
                }
                MovieList = templiste;
            }

            else if (theinput.Searchtype == "Person")
            {
                using (var db = new MyDbContext())
                {
                    var query = (from p in db.Persons
                                 join e in db.Employments
                                 on p._personId equals e._personId
                                 join m in db.Movies
                                 on e._movieId equals m.movieId
                                 join gm in db.GenresAndMovies
                                 on m.movieId equals gm._movieId
                                 join g in db.Genres
                                 on gm._genreId equals g._genreId

                                 where (p._Personname.Contains(theinput.Name) || theinput.Name == "")
                                 && (theinput.GenreID == "0" || gm._genreId == Int32.Parse(theinput.GenreID))
                                 && (m._releaseDate.Value.Year == Int32.Parse(theinput.Year) || theinput.Year == "0")
                                 && theinput.Searchtype == "Person" || theinput.Searchtype == null
                                 select new
                                 {
                                     movieid = m.movieId,
                                     movietitel = m._title,
                                     movieposter = m._posterUrl,
                                 }
                                 ).ToList().Distinct(); // til liste og fjerner samtidig duplikater.
                   

                    foreach (var item in query)
                    {
                        Movie tempmovie = new Movie();
                        Console.WriteLine(item.movietitel);
                        tempmovie._title = item.movietitel;
                        tempmovie._posterUrl = item.movieposter;
                        tempmovie.movieId = item.movieid;
                        templiste.Add(tempmovie);
                    }
                }
                MovieList = templiste;
            }

            else if (theinput.Searchtype == "")
            {
                using (var db = new MyDbContext())
                {
                    var query = (from m in db.Movies
                                 join gm in db.GenresAndMovies
                                 on m.movieId equals gm._movieId
                                 join g in db.Genres
                                 on gm._genreId equals g._genreId
                                 where (theinput.GenreID == "0" || gm._genreId == Int32.Parse(theinput.GenreID))
                                  && (m._releaseDate.Value.Year == Int32.Parse(theinput.Year) || theinput.Year == "0")

                                 select new
                                 {
                                     movieid = m.movieId,
                                     movietitel = m._title,
                                     movieposter = m._posterUrl,
                                 }
                                 ).ToList().Distinct(); // til liste og fjerner samtidig duplikater.

                    foreach (var item in query)
                    {
                        Movie tempmovie = new Movie();
                        tempmovie._title = item.movietitel;
                        tempmovie._posterUrl = item.movieposter;
                        tempmovie.movieId = item.movieid;
                        templiste.Add(tempmovie);
                    }
                }
                MovieList = templiste;

            }
            return Page();
        }

        public class InputMovie
        {
            [StringLength(100, ErrorMessage = "Maximum length is {1}")]
            [Display(Name = "Searching Field")]
            public string Name { get; set; } = "";
            public string GenreID { get; set; } = "0";
            public string Searchtype { get; set; } = "";
            public string Year { get; set; } = "0";
        }
    }
}