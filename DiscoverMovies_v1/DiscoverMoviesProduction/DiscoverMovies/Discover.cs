using System.Linq;
using System.Collections.Generic;
using AcquireDB_EFcore.Tables;

namespace ASP_Web_Bootstrap
{
    public class DiscoverScore
    {
        public DiscoverScore()
        {
            Movie = new Movie();
        }

        public DiscoverScore(Movie movie, int score)
        {
            Movie = movie;
            Score = score;
        }

        public Movie Movie { get; set; }

        public int Score { get; set; }

    }



    public class Discover
    {
        List<Movie> shortList = new List<Movie>();
        List<Movie> inputMovies;

        // Settings:

        // Hvor populær skal en skuespiller være før vi vil bruge dem på shortlisten:
        private double actorPopularityMin = 20.0;

        // Hvor populær skal en film være før vi vil bruge den på shortlisten:
        private double filmPopularityMin = 20.0;



        // 5 movies enter, one movie leaves: Mad Max rulez!
        public List<Movie> DiscoverMovies(List<Movie> inputMovies)
        {
            this.inputMovies = inputMovies;

            // Shortlist
            //      Alle film, som samtlige instruktører, producerer, og filmstjerner(pop 20+) har være med til at lave
            //

            foreach(var genres in inputMovies.First()._employmentList.ToList())
            {
                Console.WriteLine("Person: " + genres._personId);
            }

            using (var db = new MyDbContext())
            {
                // 8374, 1542, 603, 564, 3293



                

                List<Person> people = new List<Person>();

                // Find alle instruktøre og producere, og skuespillere m. popularity 20+
                foreach (var movie in inputMovies)
                {

                    // db.Movies.Where(x => x._movieId == 3293).Include(x => x._Employment).First

                    foreach(var employment in db.Employments.Where(x => x._movieId == movie.movieId).ToList())
                    {
                        if(employment._job == "Director" || employment._job == "Producer")
                        {
                            people.Add(db.Persons.Find(employment._personId));
                        }

                        if(employment._job == "Actor" && db.Persons.Find(employment._personId)._Personpopularity > actorPopularityMin)
                        {
                            people.Add(db.Persons.Find(employment._personId));
                        }
                    }
                }

                // Find alle film som de har været med i, som 
                foreach (var person in people)
                {
                    foreach(var employment in db.Employments.Where(x => x._personId == person._personId).ToList())
                    {
                        shortList.Add(db.Movies.Find(employment._movieId));
                    }
                    
                }

                Console.WriteLine("Shortlist count: " + shortList.Count);

            }

            // Fjern Dupes:




            // Filters
            // 1. Genre
            // 2. Cast
            // 3. Crew
            // 4. Year
            // 5. Production Companies

            List<Movie> genreMovies = GenreFilter(shortList);
            List<Movie> castMovies = CastFilter(shortList);
            List<Movie> crewMovies = CrewFilter(shortList);
            List<Movie> yearMovies = YearFilter(shortList);
            List<Movie> ProdMovies = ProdFilter(shortList);

            // Hvert filter laver en liste

            // Hvad der går igen i hver liste

            // Top resultatet er den forslåede film




            return inputMovies;
        }

        public List<Movie> GenreFilter(List<Movie> shortlist)
        {
            List<Movie> genreList = new List<Movie>();
            List<int> genreCounter = new List<int>();
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            foreach (var movie in inputMovies)
            {

                foreach (var genre in movie._genreList)
                {
                    genreCounter.Add(genre._genreId);
                }
            }


            var genreCounted = genreCounter.GroupBy(x => x).Where(g => g.Count() > 1)
                .Select(y => new { Id = y.Key, Count = y.Count() }).ToList();

            foreach (var genre in genreCounted)
            {
                Console.WriteLine("genre: " + genre);
            }

            foreach (var Movie in shortlist)
            {
                discoverScores.Add(new DiscoverScore());





            }


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

            // 2001
            // +/- intervallet   5 - 1999 - 2002 +5

            // +/- 5 år
            // +/- 10 år

            return yearList;
        }

        public List<Movie> ProdFilter(List<Movie> shortlist)
        {
            List<Movie> prodList = new List<Movie>();




            return prodList;
        }


    }
}
