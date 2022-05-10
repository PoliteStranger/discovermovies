using System.Linq;
using System.Collections.Generic;
using Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace DiscoverMoviesProduction
{
    /// <summary>
    /// Datastrukturen til at holde points tildelt filene af filtrene
    /// </summary>
    public class DiscoverScore
    {
        public DiscoverScore()
        {
            Movie = new Movie();
        }

        public DiscoverScore(Movie movie, double score)
        {
            Movie = movie;
            Score = score;
        }

        public Movie Movie { get; set; }

        public double Score { get; set; }

    }

    /// <summary>
    /// Til at skabe lister over par, skal evt droppes!
    /// </summary>
    public class PersonPair
	{
        public int personA { get; set; }
        public int personB { get; set; }

        public PersonPair(int persona, int personb)
		{
            personA = persona;
            personB = personb;
		}
	}


    /// <summary>
    /// Interface til DiscoverIntsToMovies, som konvertere en liste af filmId'er til en movieliste med samtlige films data.
    /// </summary>
    public interface DiscoverInputMovies
    {
        List<Movie> GetInputMovies(List<int> InputMovieIds);
    }

    /// <summary>
    /// Konvertere en liste af filmId'er til en movieliste med samtlige films data.
    /// </summary>
    public class DiscoverIntsToMovies : DiscoverInputMovies
    {
        public List<Movie> GetInputMovies(List<int> InputMovieIds)
        {
            List<Movie> InputMovies = new List<Movie>();

            using (var db = new MyDbContext())
            {
                InputMovies = db.Movies.Where(c => InputMovieIds.Contains(c.movieId)).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).ToList();
            }

            return InputMovies;
        }
    }

    /// <summary>
    /// Normalisere scores i et MovieScore objekt
    /// </summary>
    public class NormalizingScores
    {
        public static void Normalize(List<DiscoverScore> inputScores)
        {
            if(inputScores.Count() > 0)
            {
                double MaxScore = inputScores.Max(x => x.Score);
                double MinScore = inputScores.Min(x => x.Score);

                foreach (DiscoverScore movie in inputScores)
                {
                    movie.Score = (double)(1 / (MaxScore - MinScore)) * inputScores.Find(x => x.Movie == movie.Movie).Score;
                }
            }
        }
    }



    /// <summary>
    /// Selve Discover algoritmen
    /// </summary>
    public class Discover
    {
        // Listen som holder alle film som kan vælges imellem.
        List<Movie> shortList = new List<Movie>();

        // De fem film valgt inde på Discover siden.
        List<Movie> inputMovies;

        DiscoverIntsToMovies IntsToMovies = new DiscoverIntsToMovies();

        


        // Settings:

        // Hvor populær skal en skuespiller være før vi vil bruge dem på shortlisten:
        private double actorPopularityMin = 20.0;

        // Vi holder styr på hvor lang tid at Discover tager at beregne/hente data!
        private LoadTimer timer = new LoadTimer();

        // Vi tager tid på hvor lang tid siden er om at beregne resultatet
        public int dbKald = 0;


        /// <summary>
        /// 5 movies enter, one movie leaves: Mad Max rulez!
        /// Giv den en liste over 5 Ints, og den sender et Movie objekt tilbage!
        /// </summary>
        /// <param name="inputMovieInts"></param>
        /// <returns></returns>
        public Movie DiscoverMovies(List<int> inputMovieInts)
        {
            // Log time:
            timer.StartTimer();

            // læg input movies over i den variabel som vi arbejder med.
            inputMovies = IntsToMovies.GetInputMovies(inputMovieInts);

            

            using (var db = new MyDbContext())
            {
                // Ny liste over personer, som skal bruges til at finde alle VIP fra de fem inputfilm
                List<Person> people = new List<Person>();

                // Find alle instruktøre og producere, og skuespillere m. popularity 20+
                foreach (var movie in inputMovies)
                {
                    // Vi gennemgår alle personer involveret i de fem udvalgte film
                    foreach(var employment in db.Employments.Where(x => x._movieId == movie.movieId).ToList())
                    {
                        dbKald++;
                        // Vi sætter alle Instruktøre og Producere til side:
                        if (employment._job == "Director" || employment._job == "Producer")
                        {
                            people.Add(db.Persons.Find(employment._personId));
                        }

                        // Vi sætter alle skuespillere med popularity over actorPopularityMin til side også...
                        if (employment._job == "Acting" && db.Persons.Find(employment._personId)._Personpopularity > actorPopularityMin)
                        {
                            dbKald++;
                            people.Add(db.Persons.Find(employment._personId));
                        }
                    }
                }

                // Find alle film som de har været med i:
                foreach (var person in people)
                {
                    foreach(var employment in db.Employments.Where(x => x._personId == person._personId).ToList())
                    {
                        dbKald++;
                        if (!shortList.Any(x => x.movieId == employment._movieId))
                        {
                            shortList.Add(db.Movies.Find(employment._movieId));
                            dbKald++;

                        }

                    }
                }
                
                
                // Vi fjerner duplikater blandt filmene. Inputfilmene vil nemlig også være på denne liste!
                Console.WriteLine("Input Movies:");
                foreach(var movie in inputMovies.ToList())
                {
                    shortList.Remove(shortList.Find(x => x.movieId == movie.movieId));
                    Console.WriteLine(movie._title);
                }
                Console.WriteLine("Shortlist from input has count of: " + shortList.Count);
                
                // Vi har nu vores endelige shortlist!

            }

            // spacer
            Console.WriteLine("");


            // Vi sender nu listen gennem samtlige filtre, og får deres MovieScores i retur:
            List<DiscoverScore> genreMovies = GenreFilter(shortList);
            List<DiscoverScore> castMovies = CastFilter(shortList);
            List<DiscoverScore> crewMovies = CrewFilter(shortList);
            List<DiscoverScore> yearMovies = YearFilter(shortList);
            List<DiscoverScore> ProdMovies = ProdFilter(shortList);

            // Samlede score liste
            List<DiscoverScore> finalScore = new List<DiscoverScore>();

            // Normalisering af alle scores i alle lister
            NormalizingScores.Normalize(genreMovies);
            NormalizingScores.Normalize(castMovies);
            NormalizingScores.Normalize(crewMovies);
            NormalizingScores.Normalize(yearMovies);
            NormalizingScores.Normalize(ProdMovies);

            // Gennemgang af shortlist, og tælle points sammen:
            foreach (var movie in shortList)
            {
                // Hver film starter med en score på 0
                double score = 0;
                if (genreMovies.Any(m => m.Movie.movieId == movie.movieId))
                    score += genreMovies.Find(m => m.Movie.movieId == movie.movieId).Score;

                if (castMovies.Any(m => m.Movie.movieId == movie.movieId))
                    score += castMovies.Find(m => m.Movie.movieId == movie.movieId).Score;

                if (crewMovies.Any(m => m.Movie.movieId == movie.movieId))
                    score += crewMovies.Find(m => m.Movie.movieId == movie.movieId).Score;

                if (yearMovies.Any(m => m.Movie.movieId == movie.movieId))
                    score += yearMovies.Find(m => m.Movie.movieId == movie.movieId).Score;

                if (ProdMovies.Any(m => m.Movie.movieId == movie.movieId))
                    score += ProdMovies.Find(m => m.Movie.movieId == movie.movieId).Score;

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

            // Log time:
            timer.StopTimer();

            Console.WriteLine("DB Kald: " + dbKald);

            return finalScore.FirstOrDefault().Movie;
        }





        public List<DiscoverScore> GenreFilter(List<Movie> shortlist)
        {
            List<Movie> genreList = new List<Movie>();
            List<int> genreCounter = new List<int>();
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: GENRE");
            Console.WriteLine("-------------------------------------------------");



            foreach (var movie in inputMovies)
            {

                foreach (var genre in movie._genreList)
                {
                    genreCounter.Add(genre._genreId);
                }
            }


            var genreCounted = genreCounter.GroupBy(x => x).Where(g => g.Count() > 0)// skal være  > 1
                .Select(y => new { Id = y.Key, Count = y.Count() }).ToList();

            foreach (var genre in genreCounted)
            {
                //Console.WriteLine("genre: " + genre);
            }

            foreach (var _Movie in shortlist)
            {
                int score = 0;
                using (var db = new MyDbContext())
                {
                    var genres = db.GenresAndMovies.Where(Genre => Genre._movieId == _Movie.movieId);
                    dbKald++;

                    foreach (var genre in genres)
                    {
                        if (genreCounted.Find(x => x.Id == genre._genreId) != null)
                        {
                            dbKald++;

                            score = score + genreCounted.Find(x => x.Id == genre._genreId).Count;
                        }
                    }
                    discoverScores.Add(new DiscoverScore(_Movie, score));
                }

            }
            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();
            
            foreach (var score in discoverScores)
            {
                Console.WriteLine(score.Movie._title + " has " + score.Score);
            }

            /*
            discoverScores.Sort(delegate (DiscoverScore x, DiscoverScore y) // denne sortering er vist ikke nødvendig hvis vi bare er efter point.
            {
                return y.Score.CompareTo(x.Score);
            });
            */

            return discoverScores;
        }

        public List<DiscoverScore> CastFilter(List<Movie> shortlist)
        {

            var db = new MyDbContext();


            // En liste over crew
            List<Movie> crewList = new List<Movie>();

            // Til at tildele points
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: CAST");
            Console.WriteLine("-------------------------------------------------");



            List<Employment> inputEmployments = new List<Employment>();


            foreach (var movie in inputMovies.ToList())
            {
                foreach (var employment in movie._employmentList.ToList())
                {
                   // Console.WriteLine("!" + employment._job);
                    if (employment._job == "Acting")
                    {
                        inputEmployments.Add(employment);
                    }

                }
            }
            Console.WriteLine("Found {0} cast in input", inputEmployments.Count);


            Console.WriteLine("\nSearching for matches in cast:");



            foreach (var movie in shortList.ToList())
            {
                //Console.Write(">");

                foreach (var employment in inputEmployments.ToList())
                {

                    if (movie._employmentList.Any(x => x._personId == employment._personId))
                    {
                        int Addscore = 1;


                        // If match, then pass out score:
                        if (discoverScores.Any(x => x.Movie == movie))
                        {
                            discoverScores.Find(x => x.Movie == movie).Score += Addscore;
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
                foreach (var employment in inputEmployments.ToList())
                {
                    if (employment._movieId == score.Movie.movieId)
                        Console.WriteLine(employment.Person._Personname + " was " + employment._job);
                }
            }



            return discoverScores;
        }

        public List<DiscoverScore> CrewFilter(List<Movie> shortlist)
        {

            // En liste over crew
            List<Movie> crewList = new List<Movie>();

            // Til at tildele points
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: CREW");
            Console.WriteLine("-------------------------------------------------");

            List<Employment> inputEmployments = new List<Employment>();


            foreach (var movie in inputMovies.ToList())
            {
                foreach (var employment in movie._employmentList.ToList())
                {
                    if(employment._job != "Acting")
                    {
                        inputEmployments.Add(employment);
                    }

                }
            }
            Console.WriteLine("Found {0} employments in input", inputEmployments.Count);


            Console.WriteLine("\nSearching for matches in crew:");

            

            foreach (var movie in shortList.ToList())
            {
                //Console.Write(">");

                foreach (var employment in inputEmployments.ToList())
                {
                    
                    if (movie._employmentList.Any(x => x._personId == employment._personId))
                    {
                        int AddScore = 1;

                        // Vi vægter instruktøre højere!
                        if (employment._job == "Director" || employment._job == "Producer")
                        {
                            //AddScore = 3;
                            //Console.WriteLine("Director/Producer spottet!");
                        }

                        // If match, then pass out score:
                        if (discoverScores.Any(x => x.Movie == movie))
                        {
                            discoverScores.Find(x => x.Movie == movie).Score += AddScore;
                            //Console.Write(" Match!");
                        }
                        else
                        {
                            discoverScores.Add(new DiscoverScore(movie, AddScore));
                            //Console.Write(" Match!");
                        }
                        //Console.WriteLine(discoverScores.Find(x => x.Movie == movie).Movie._title + " has " + discoverScores.Find(x => x.Movie == movie).Score + " points");

                    }
                }
               
            }

            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();
            

            foreach(var score in discoverScores)
            {
                Console.WriteLine(score.Movie._title + " has " + score.Score);
                foreach(var employment in inputEmployments.ToList())
                {
                    if(employment._movieId == score.Movie.movieId)
                    Console.WriteLine(employment.Person._Personname + " was " + employment._job);
                }
            }

            


            foreach (var person in inputEmployments.ToList())
            {
                //Console.WriteLine("Person: " + db.Persons.Find(person._personId)._Personname);
            }


            Console.WriteLine("-------------------------------------------------");

            


            // Director AND Producer team

            // Director AND Director of Photography team

            return discoverScores;
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
