using System.Linq;
using System.Collections.Generic;
using ASP_Web_Bootstrap;
using Microsoft.EntityFrameworkCore;
using AcquireDB_EFcore.Tables;

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
    /// Interface til DiscoverIntsToMovies, som konvertere en liste af filmId'er til en movieliste med samtlige films data.
    /// </summary>
    public interface IDiscoverInputMovies
    {
        List<Movie> GetInputMovies(List<int> InputMovieIds);
    }

    /// <summary>
    /// Konvertere en liste af filmId'er til en movieliste med samtlige films data.
    /// </summary>
    public class DiscoverIntsToMovies : IDiscoverInputMovies
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

        private double moviePopularityMin = 10.0;

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
                dbKald++;
                // Ny liste over personer, som skal bruges til at finde alle VIP fra de fem inputfilm
                List<Person> people = (from pers in db.Persons.Where(x => x._Personpopularity > actorPopularityMin)
                                       join emp in db.Employments.Where(x => inputMovieInts.Contains(x._movieId))
                                       on pers._personId equals emp._personId
                                       where emp._job == "Acting"
                                       select pers).ToList();

                timer.StopTimer();
                dbKald++;
                people.AddRange((from pers in db.Persons
                                       join emp in db.Employments.Where(x => inputMovieInts.Contains(x._movieId))
                                       on pers._personId equals emp._personId
                                       where emp._job == "Director" || emp._job == "Producer"
                                       select pers).ToList());

                timer.StopTimer();
                Console.WriteLine("Persons found: " + people.Count);

                List<int> personIds = new List<int>();
                foreach (var person in people)
                {
                    personIds.Add(person._personId);
                }

                //people.Select(x => x._personId).ToList()



                dbKald++;

                shortList = (from m in db.Movies.Include(z => z._employmentList).Include(y => y._genreList)
                             join e in db.Employments.Where(x => personIds.Contains(x._personId))
                             on m.movieId equals e._movieId
                             select m).ToList();
                timer.StopTimer();
                shortList = shortList.Distinct().ToList();

                //foreach(var personId in personIds)
                //{
                //    dbKald++;

                //    Console.WriteLine("Getting person no.: " + personId);

                //    shortList.Concat((from m in db.Movies.Include(z => z._employmentList).Include(y => y._genreList)
                //                 join e in db.Employments
                //                 on m.movieId equals e._movieId
                //                 where e._personId == personId && m._popularity > moviePopularityMin
                //                 select m).ToList());


                //    timer.StopTimer();
                //    shortList = shortList.Distinct().ToList();
                //}




                Console.WriteLine("Input Movies:");
                foreach (var movie in inputMovies)
                {
                    shortList.Remove(shortList.Find(x => x.movieId == movie.movieId));
                    Console.WriteLine(movie._title);
                }
                Console.WriteLine("Shortlist from input has count of: " + shortList.Count);

            }



            // spacer
            Console.WriteLine("");

            Console.WriteLine("");
            Console.WriteLine("Running filters...");
            // Vi sender nu listen gennem samtlige filtre, og får deres MovieScores i retur:
            List<DiscoverScore> genreMovies = GenreFilter(shortList);
            List<DiscoverScore> castMovies = CastFilter(shortList);
            List<DiscoverScore> crewMovies = CrewFilter(shortList);
            List<DiscoverScore> yearMovies = YearFilter(shortList);
            Console.WriteLine("DB Kald: " + dbKald);
            List<DiscoverScore> ProdMovies = ProdFilter(shortList);
            Console.WriteLine("DB Kald: " + dbKald);
            List<DiscoverScore> BudgetRevenueMovies = BudgetRevenueFilter(shortList);

            timer.StopTimer();

            // Samlede score liste
            List<DiscoverScore> finalScore = new List<DiscoverScore>();


            Console.WriteLine("");
            Console.WriteLine("Tallying scores...");

            // Normalisering af alle scores i alle lister
            NormalizingScores.Normalize(genreMovies);
            NormalizingScores.Normalize(castMovies);
            NormalizingScores.Normalize(crewMovies);
            NormalizingScores.Normalize(yearMovies);
            NormalizingScores.Normalize(ProdMovies);
            NormalizingScores.Normalize(BudgetRevenueMovies);

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

                if (BudgetRevenueMovies.Any(m => m.Movie.movieId == movie.movieId))
                    score += BudgetRevenueMovies.Find(m => m.Movie.movieId == movie.movieId).Score;

                // Til filmen, samt summen af dens points:
                finalScore.Add(new DiscoverScore(movie, score));


            }

            // I rækkefølge af score:
            finalScore = finalScore.OrderByDescending(x => x.Score).ToList();



            // Print til consol
            Console.WriteLine("");
            Console.WriteLine("Final scores:");
            Console.WriteLine("-------------------------------------------------");

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
                //using (var db = new MyDbContext())
                //{
                    //var genres = db.GenresAndMovies.Where(Genre => Genre._movieId == _Movie.movieId);
                    //dbKald++;

                    foreach (var genre in _Movie._genreList)
                    {
                        if (genreCounted.Find(x => x.Id == genre._genreId) != null)
                        {
                            //dbKald++;

                            score = score + genreCounted.Find(x => x.Id == genre._genreId).Count;
                        }
                    }
                    discoverScores.Add(new DiscoverScore(_Movie, score));
                //}

            }
            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();


            int range = 10;
            if (discoverScores.Count() < 10) range = discoverScores.Count();
            foreach (var score in discoverScores.GetRange(0, range))
            {
                Console.WriteLine(score.Movie._title + " - " + score.Score);
            }


            return discoverScores;
        }

        public List<DiscoverScore> CastFilter(List<Movie> shortlist)
        {

            

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
               // Console.WriteLine("movie: " + movie._title + " C: " + movie._employmentList.Count());

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
                //Console.WriteLine("movie: " + movie._title + " Emps: " + movie._employmentList.Count());


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

            int range = 10;
            if (discoverScores.Count() < 10) range = discoverScores.Count();
            foreach (var score in discoverScores.GetRange(0, range))
            {
                Console.WriteLine(score.Movie._title + " - " + score.Score);
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


            int rangeb = 10;
            if (discoverScores.Count() < 10) rangeb = discoverScores.Count();
            foreach (var score in discoverScores.GetRange(0, rangeb))
            {
                Console.WriteLine(score.Movie._title + " - " + score.Score);
            }



            Console.WriteLine("-------------------------------------------------");


            return discoverScores;
        }


        public List<DiscoverScore> YearFilter(List<Movie> shortlist)
        {
            //var db = new MyDbContext();

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

            int rangeb = 10;
            if (discoverScores.Count() < 10) rangeb = discoverScores.Count();
            foreach (var score in discoverScores.GetRange(0, rangeb))
            {
                Console.WriteLine(score.Movie._title + " - " + score.Score);

            }

            return discoverScores;
        }


        public List<DiscoverScore> ProdFilter(List<Movie> shortlist)
        {
            //var db = new MyDbContext();

            //List<Movie> prodList = new List<Movie>();

            // Til at tildele points
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: PRODUCTION");
            Console.WriteLine("-------------------------------------------------");


            List<ProducedBy> inputProds;

            List<ProducedBy> prods;

            using (var db = new MyDbContext())
            {
                // Find ProdCompanies 
                inputProds = db.ProducedBy.Where(x => inputMovies.Select(c => c.movieId).ToList().Contains(x._movieId)).ToList();
                dbKald++;

                //prods = db.ProducedBy.Where(p => shortList.Select(x => x.movieId).ToList().Contains(p._movieId)).ToList();

                //dbKald++;
            }


            //foreach (var movie in inputMovies)
            //{
            //    Console.WriteLine("Input M P Count: " + movie._prodCompanyList.Count());
            //    foreach (var prod in db.ProducedBy)
            //    {
            //        if (prod._movieId == movie.movieId)
            //        {
            //            inputProds.Add(prod);
            //            //Console.WriteLine("!" + prod.prodCompanyId);
            //        }

            //    }
            //}

            Console.WriteLine("Found {0} prod in input", inputProds.Count);

            Console.WriteLine("\nSearching for matches in Production:");


            // db.ProducedBy.Where(c => InputMovieIds.Contains(c.movieId))


            foreach (var movie in shortList.ToList())
            {
                //Console.WriteLine("Shortlist M P Count: " + movie._prodCompanyList.Count());

                using (var db = new MyDbContext())
                {
                    prods = db.ProducedBy.Where(p => p._movieId == movie.movieId).ToList();
                    dbKald++;
                }
                    
                foreach (var p in prods)
                {
                    //If match, then pass out score:
                    if (discoverScores.Any(x => x.Movie == movie))
                    {
                        discoverScores.Find(x => x.Movie == movie).Score++;
                    }
                    else
                    {
                        discoverScores.Add(new DiscoverScore(movie, 1));
                    }
                }
            }

            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();

            int range = 10;
            if (discoverScores.Count() < 10) range = discoverScores.Count();
            foreach (var score in discoverScores.GetRange(0, range))
            {
                Console.WriteLine(score.Movie._title + " - " + score.Score);

            }

            return discoverScores;


        }

        public List<DiscoverScore> BudgetRevenueFilter(List<Movie> shortlist)
        {
            List<Movie> budgetRevenueList = new List<Movie>();
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: BUDGET vs REVENUE");
            Console.WriteLine("-------------------------------------------------");

            int averageBudget = 0;
            int totalBudget = 0;
            int profitableCount = 0;
            int unProfitableCount = 0;
            int firstQuartile = 5200000, secondQuartile = 18100000, thirdQuartile = 41000000;
            int firstQuartileCount = 0, secondQuartileCount = 0, thirdQuartileCount = 0, fourthQuartileCount = 0;


            foreach (var movie in inputMovies)
            {
                if (movie._budget != 0 && movie._revenue != 0 && movie._budget != null && movie._revenue != null)
                {
                    if (movie._budget > movie._revenue)
                    {
                        unProfitableCount++;
                    }
                    else
                    {
                        profitableCount++;
                    }
                    totalBudget += movie._budget.GetValueOrDefault();
                }
                //https://stephenfollows.com/how-much-does-the-average-movie-cost-to-make/
                //Ovenstående link fremsætter kvartilerne for filmbudget. Disse bliver brugt til vurdering.
                if (movie._budget <= firstQuartile)
                {
                    firstQuartileCount++;
                }
                else if (movie._budget <= secondQuartile && movie._budget > firstQuartile)
                {
                    secondQuartileCount++;
                }
                else if (movie._budget <= thirdQuartile && movie._budget > secondQuartile)
                {
                    thirdQuartileCount++;
                }
                else if (movie._budget > thirdQuartile)
                {
                    fourthQuartileCount++;
                }
            }
            averageBudget = totalBudget / inputMovies.Count;

            foreach (var movie in shortList)
            {
                int score = 0;
                //if (movie._budget <= firstQuartile)
                //{
                //    score += firstQuartileCount;
                //}
                //else if (movie._budget <= secondQuartile && movie._budget > firstQuartile)
                //{
                //    score += secondQuartileCount;
                //}
                //else if (movie._budget <= thirdQuartile && movie._budget > secondQuartile)
                //{
                //    score += thirdQuartileCount;
                //}
                //else if (movie._budget > thirdQuartile)
                //{
                //    score += fourthQuartileCount;
                //}

                //if (movie._budget > averageBudget - averageBudget * 0.1 && movie._budget < averageBudget + averageBudget * 0.1)
                //{
                //    score += 3;
                //}

                if (movie._budget > movie._revenue)
                {
                    score += unProfitableCount;
                }
                else
                {
                    score += profitableCount;
                }

                discoverScores.Add(new DiscoverScore(movie, score));
            }

            discoverScores.Sort(delegate (DiscoverScore x, DiscoverScore y) // denne sortering er vist ikke nødvendig hvis vi bare er efter point.
            {
                return y.Score.CompareTo(x.Score);
            });

            int range = 10;
            if (discoverScores.Count() < 10) range = discoverScores.Count();
            foreach (var score in discoverScores.GetRange(0, range))
            {
                Console.WriteLine(score.Movie._title + " - " + score.Score);
            }

            return discoverScores;
        }
    }
}
