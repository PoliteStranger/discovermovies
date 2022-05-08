using System.Linq;
using System.Collections.Generic;
using Database.Tables;

namespace DiscoverMoviesProduction
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

            List<DiscoverScore> genreMovies = GenreFilter(shortList);
            List<DiscoverScore> castMovies = CastFilter(shortList);

            // Hvert filter laver en liste

                // HVIS filmen har fået en score
                if (castMovies.Any(x => x.Movie == movie))
                    score += (double)(1 / (CastMaxScore - CastMinScore)) * castMovies.Find(x => x.Movie == movie).Score;// Så hent dens points fra Cast Filteret
                // HVIS filmen har fået en score
                if (crewMovies.Any(x => x.Movie == movie))
                    score += (double)(1 / (CrewMaxScore - CrewMinScore)) * crewMovies.Find(x => x.Movie == movie).Score;// Så hent dens points fra Crew Filteret

                // HVIS filmen har fået en score
                if (yearMovies.Any(x => x.Movie == movie))
                    score += (double)(1 / (YearMaxScore - YearMinScore)) * yearMovies.Find(x => x.Movie == movie).Score;// Så hent dens points fra Crew Filteret


                // HVIS filmen har fået en score
                //if (ProdMovies.Any(x => x.Movie == movie))
                //    score += (double)(1 / (ProdMaxScore - ProdMinScore)) * ProdMovies.Find(x => x.Movie == movie).Score;// Så hent dens points fra Crew Filteret



                // Til filmen, samt summen af dens points:
                finalScore.Add(new DiscoverScore(movie, score));
            }

            // I rækkefølge af score:
            finalScore = finalScore.OrderByDescending(x => x.Score).ToList();

            // Print til consol
            Console.WriteLine("Final scores:");
            finalScore = finalScore.GetRange(0, 10).ToList();
            foreach (var score in finalScore)
            {
                Console.WriteLine(score.Movie._title + ": " + score.Score.ToString("0.00") + " - " + score.Movie._popularity);
            }

            // Vi timer kodeafviklingen, så vi kan se om det går hurtigt nok!
            // OBS Console print TAGER EKSTRA TID, de skal fjernes, så vi kan få den endelige tid!


            Console.WriteLine("");
            Console.WriteLine("Final scores: (Adjusted with popularity)");
            // Et forsøg med at forstærke score ved at gange film popularity med dens score:

            foreach (var score in finalScore)
            {
                score.Score += (double)score.Score * (double)score.Movie._popularity;
            }
            finalScore = finalScore.OrderByDescending(x => x.Score).ToList();
            foreach (var score in finalScore)
            {
                Console.WriteLine(score.Movie._title + ": " + score.Score.ToString("0.00"));
            }








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

        public List<DiscoverScore> YearFilter(List<Movie> shortlist)
        {
            var db = new MyDbContext();

            List<Movie> yearList = new List<Movie>();

            // Til at tildele points
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: ReleaseDate");
            Console.WriteLine("-------------------------------------------------");

            List<Movie> inputYear = new List<Movie>();

            int maxYear = 0;
            int minYear = 2022;
            int range;


            Console.WriteLine("Max year: " + shortlist.Max(x => x._releaseDate.Value.Year));
            Console.WriteLine("Min year: " + shortlist.Min(x => x._releaseDate.Value.Year));

            foreach (var movie in inputMovies)
            {

                // Extract Year from Datetime

                if (movie._releaseDate.Value.Year < minYear)
                {
                    minYear = movie._releaseDate.Value.Year;
                }
                else if (movie._releaseDate.Value.Year > maxYear)
                {
                    maxYear = movie._releaseDate.Value.Year;
                }


            }
            minYear = inputMovies.Min(x => x._releaseDate.Value.Year);
            maxYear = inputMovies.Max(x => x._releaseDate.Value.Year);

            Console.WriteLine("Found releaseyear in range from {0} to {1} in input", minYear, maxYear);

            Console.WriteLine("\nSearching for matches by Release Year:");

            range = maxYear - minYear;

            Console.WriteLine("Number of Movies: {0}", shortList.Count());

            foreach (var movie in shortList.ToList())
            {

                if (movie._releaseDate.Value.Year >= minYear && movie._releaseDate.Value.Year <= maxYear)
                {

                    // If match, then pass out score:
                    if (discoverScores.Any(x => x.Movie == movie))
                    {
                        discoverScores.Find(x => x.Movie == movie).Score += 3;
                    }
                    else
                    {
                        discoverScores.Add(new DiscoverScore(movie, 3));
                    }

                }

                else if (movie._releaseDate.Value.Year >= (minYear - 0.5 * range) && movie._releaseDate.Value.Year <= (maxYear + 0.5 * range))
                {
                    // If match, then pass out score:
                    if (discoverScores.Any(x => x.Movie == movie))
                    {
                        discoverScores.Find(x => x.Movie == movie).Score += 2;
                    }
                    else
                    {
                        discoverScores.Add(new DiscoverScore(movie, 2));
                    }
                }

                else if (movie._releaseDate.Value.Year >= (minYear - range) && movie._releaseDate.Value.Year <= (maxYear + range))
                {
                    // If match, then pass out score:
                    if (discoverScores.Any(x => x.Movie == movie))
                    {
                        discoverScores.Find(x => x.Movie == movie).Score += 1;
                    }
                    else
                    {
                        discoverScores.Add(new DiscoverScore(movie, 1));
                    }
                }





            }

            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();
            foreach (var score in discoverScores)
            {
                Console.WriteLine(score.Movie._title + " has " + score.Score);

            }

            return discoverScores;
        }



        public List<DiscoverScore> ProdFilter(List<Movie> shortlist)
        {
            var db = new MyDbContext();


            // En liste over crew
            List<Movie> prodList = new List<Movie>();

            // Til at tildele points
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: PRODUCTION");
            Console.WriteLine("-------------------------------------------------");



            List<ProdCompany> inputProds = new List<ProdCompany>();


            foreach (var movie in inputMovies)
            {
                //Console.WriteLine(movie._prodCompanyList.Count());
                foreach (var prod in movie._prodCompanyList.ToList())
                {
                    // Console.WriteLine("!" + employment._job);
                    inputProds.Add(prod);

                }
            }
            Console.WriteLine("Found {0} prod in input", inputProds.Count);


            Console.WriteLine("\nSearching for matches in Production:");



            foreach (var movie in shortList.ToList())
            {
                //Console.Write(">");

                foreach (var prod in inputProds.ToList())
                {

                    if (movie._prodCompanyList.Any(x => x.prodCompanyId == prod.prodCompanyId))
                    {

                        // If match, then pass out score:
                        if (discoverScores.Any(x => x.Movie == movie))
                        {
                            discoverScores.Find(x => x.Movie == movie).Score++;
                            //Console.Write(" Match!");
                        }
                        else
                        {
                            discoverScores.Add(new DiscoverScore(movie, 1));
                            //Console.Write(" Match!");
                        }
                        //Console.WriteLine(discoverScores.Find(x => x.Movie == movie).Movie._title + " has " + discoverScores.Find(x => x.Movie == movie).Score + " points");

                    }
                }

            }

            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();
            foreach (var score in discoverScores)
            {
                Console.WriteLine(score.Movie._title + " has " + score.Score);

            }

            return discoverScores;


        }


    }
}
