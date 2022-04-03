using System.Linq;
using System.Collections.Generic;

namespace ASP_Web_Bootstrap
{
    public class DiscoverScore
    {
        public Movie Movie { get; set; }

        public int Score { get; set; }

    }



    public class Discover
    {
        List<Movie> shortList = new List<Movie>();


        // 5 movies enter, one movie leaves: Mad Max rulez!
        public List<Movie> DiscoverMovies(List<Movie> inputMovies)
        {
            // Shortlist
            //      Alle film, som samtlige instruktører, producerer, og filmstjerner(pop 20+) har være med til at lave
            //

            using (var db = new MyDbContext())
            {
                // 8374, 1542, 603, 564, 3293

                inputMovies.Add(db.Movies.Find(8374));
                inputMovies.Add(db.Movies.Find(1542));
                inputMovies.Add(db.Movies.Find(603));
                inputMovies.Add(db.Movies.Find(564));
                inputMovies.Add(db.Movies.Find(3293));

                List<Person> directors = new List<Person>();

                

                foreach (var movie in inputMovies)
                {
                    Console.WriteLine("Title: " + movie._title);
                    // first.Concat(second).ToList();
                    foreach(var employment in movie._employmentList.Where(x => x._job == "Director").ToList())
                    {
                        directors.Add(employment.Person);
                    }

                }

                Console.WriteLine("Count: " + directors.Count);

                foreach (var person in directors)
                {
                    Console.WriteLine("Name: " + person._Personname);
                }


            }

            // Filters
            // 1. Genre
            // 2. Cast
            // 3. Crew
            // 4. Year
            // 5. Production Companies

            // Hvert filter laver en liste

            // Hvad der går igen i hver liste

            // Top resultatet er den forslåede film




            return inputMovies;
        }

        public List<Movie> GenreFilter(List<Movie> shortlist)
        {
            List<Movie> genreList = new List<Movie>();



            return genreList;
        }

        public List<Movie> CastFilter(List<Movie> shortlist)
        {
            List<Movie> castList = new List<Movie>();



            return castList;
        }

        public List<Movie> CrewFilter(List<Movie> shortlist)
        {
            List<Movie> crewList = new List<Movie>();



            return crewList;
        }

        public List<Movie> YearFilter(List<Movie> shortlist)
        {
            List<Movie> yearList = new List<Movie>();



            return yearList;
        }

        public List<Movie> ProdFilter(List<Movie> shortlist)
        {
            List<Movie> prodList = new List<Movie>();



            return prodList;
        }


    }
}
