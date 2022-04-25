﻿using System.ComponentModel.DataAnnotations;
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

                for (int j = 1990; j<2020; j++)
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
            //dropdown menu skal sættes til at være genrelisten
            Soegninger.Add("Movie");
            Soegninger.Add("Person");

            for (int j = 1990; j<2020; j++)
            {
                Year.Add(j);
            }

            using (var db = new MyDbContext())
            {
                TheOriginaleGenres = db.Genres.ToList();

                if (theinput.Name != "")
                {
                    //tjekker om det er en person som er sat
                    if (theinput.Searchtype == "Person")
                    {
                        //kigger efter personens navn i person db
                        var query = (from p in db.Persons
                                     join e in db.Employments
                                     on p._personId equals e._personId
                                     join m in db.Movies
                                     on e._movieId equals m.movieId
                                     where p._Personname.Contains(theinput.Name)
                                     select new
                                     {
                                         movieid = m.movieId,
                                         movietitel = m._title,
                                         movieposter = m._posterUrl,
                                     }
                                     ).ToList();

                        foreach(var item in query)
                        {
                            Movie tempmovie = new Movie();

                            tempmovie._title = item.movietitel;
                            tempmovie._posterUrl = item.movieposter;
                            tempmovie.movieId = item.movieid;
                            templiste.Add(tempmovie);
                        }

                        MovieList = templiste;
                        //Persons = db.Persons.Where(i => i._Personname.Contains(theinput.Name)).ToList();

                        //foreach (var person in Persons)
                        //{
                        //    //kigger efter hver person i employments for at finde movie id'er
                        //    var liste = db.Employments.Where(i=>i._personId==person._personId).ToList();
                        //    foreach (var movie in liste)
                        //    {
                        //        var liste1 = db.Movies.Where(i => i.movieId==movie._movieId).ToList();
                        //        foreach (var film in liste1)
                        //        {
                        //            templiste.Add(film);
                        //        }
                        //        MovieList = templiste;
                        //    }
                        //}
                    }
                    //Så ved vi det er en film som søges efter
                    else if (theinput.Searchtype == "Movie")
                    {
                        templiste = db.Movies.Where(i => i._title.Contains(theinput.Name)).ToList();
                        MovieList = templiste;
                    }
                }

                if (theinput.GenreID != "0")
                {
                    //cast theinput.GenreID til int
                    int genreid = Int32.Parse(theinput.GenreID);

                    if (templiste.Count >= 1)
                    {
                        //.ToList sikrer, at vi caster til en ny liste.
                      //  var query  = (from gm in db.GenresAndMovies
                      //                join m in db.Movies
                      //                on gm._movieId equals m.movieId
                      //                where   
                            
                      //               )
                            


                      //var result = from o in someObj 
                      //   where 
                      //   (Name == null  o.Name == Name)
                      //   && 
                      //   (City == null  o.City == City)
                      //   && 
                      //   (State == null || o.State == State)
                      //   select o;

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

                //if (theinput.Year != "0")
                //{
                //    int genreid = Int32.Parse(theinput.GenreID);

                //    if (templiste.Count >= 1)
                //    {
                //        //.ToList sikrer, at vi caster til en ny liste.
                //        foreach (var item in templiste.ToList())
                //        {
                //            //Stemmer både movieID og GenreID?

                //            var tjek = db.GenresAndMovies
                //                .Any(i => i._movieId == item.movieId && i._genreId == genreid);

                //            if (tjek == false)
                //            {
                //                templiste.Remove(item);
                //            }

                //            var tjek2 = templiste.Any(i => i._releaseDate.Value.Year==Int32.Parse(theinput.Year));

                //            if (tjek2 == false)
                //            {
                //                templiste.Remove(item);
                //            }

                //            Console.WriteLine(item._title);
                //        }
                //        //MovieList = templiste;

                //    }

                //    else
                //    {
                //        var templiste = db.Movies.Where(i => i._releaseDate.Value.Year==Int32.Parse(theinput.Year)).ToList();

                //        MovieList = templiste;
                //    }
                   
                //}
            }
            return Page();
        }

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