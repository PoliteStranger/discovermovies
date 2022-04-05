using System.Linq;
using System.Collections.Generic;
using Database.Tables;

namespace DiscoverMoviesProduction
{
    public class DiscoverScore
    {
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
        

        // Time Logging:
        public DateTime Start { get; set; }
        private DateTime End { get; set; }


        // 5 movies enter, one movie leaves: Mad Max rulez!
        public List<Movie> DiscoverMovies(List<Movie> inputMovies)
        {
            // Log time:
            Start = DateTime.Now;

            this.inputMovies = inputMovies;

            // Shortlist
            //      Alle film, som samtlige instruktører, producerer, og filmstjerner(pop 20+) har være med til at lave
            //


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



            // Log time:
            End = DateTime.Now;

            Console.WriteLine("Discover took: " + (End - Start));
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

            

            List<Employment> inputEmployments = new List<Employment>();

            foreach (var movie in inputMovies.ToList())
            {
                foreach (var employment in movie._employmentList.ToList())
                {
                    inputEmployments.Add(employment);
                }
            }



            // Director AND Producer team

            // Director AND Director of Photography team

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
