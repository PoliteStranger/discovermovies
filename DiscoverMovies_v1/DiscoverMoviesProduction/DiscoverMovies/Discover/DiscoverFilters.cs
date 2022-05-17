using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DiscoverMoviesProduction
{


    /// <summary>
    /// Filter interface
    /// </summary>
    public interface IFilter
    {
        public void accept(IFilterVisitor filterVisitor);
    }



    /// <summary>
    /// Grundfilteret, som sætter det hele igang.
    /// </summary>
    public class Filters : IFilter
    {

        // Kunne have filter vægtning her...

        public List<IFilter> DiscoverFilters;


        /// <summary>
        /// Grundfilteret, som skal have input Movies, og shortlist Movies, og deler det så ud til de andre filtre.
        /// </summary>
        /// <param name="inputMovies">Input filmene</param>
        /// <param name="shortlist">Shortlisten</param>
        public Filters(List<Movie> inputMovies, List<Movie> shortlist)
        {
            // Er inputtet ok?
            Guard.CheckDiscoverLists(inputMovies, shortlist, "Main Filter");

            // Skab samtlige filtre
            DiscoverFilters = new List<IFilter>();
            DiscoverFilters.Add(new GenreFilter(inputMovies, shortlist));
            DiscoverFilters.Add(new CrewFilter(inputMovies, shortlist));
            DiscoverFilters.Add(new CastFilter(inputMovies, shortlist));
            DiscoverFilters.Add(new YearFilter(inputMovies, shortlist));
            //DiscoverFilters.Add(new ProdFilter(inputMovies, shortlist));
            DiscoverFilters.Add(new BudgetRevenueFilter(inputMovies, shortlist));

        }

        public void accept(IFilterVisitor filterVisitor)
        {
            // Gennemgang af hvert filter:
            foreach (var filter in DiscoverFilters)
            {
                filter.accept(filterVisitor);
            }

            // Vi runder af med dette "filter", kunne også springes over...
            filterVisitor.visit(this);
        }

    }



    public class GenreFilter : IFilter
    {
        List<int> genreCounter = new List<int>();

        public List<DiscoverScore> discoverScores = new List<DiscoverScore>();

        public GenreFilter(List<Movie> inputMovies, List<Movie> shortlist)
        {
            // Er inputtet ok?
            Guard.CheckDiscoverLists(inputMovies, shortlist, "Genre Filter");

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


            foreach (var _Movie in shortlist)
            {
                int score = 0;

                foreach (var genre in _Movie._genreList)
                {
                    if (genreCounted.Find(x => x.Id == genre._genreId) != null)
                    {
                        score = score + genreCounted.Find(x => x.Id == genre._genreId).Count;
                    }
                }
                if(score != 0) // Vi vil kun se film som faktisk HAR fået en score.
                {
                    discoverScores.Add(new DiscoverScore(_Movie, score));

                }
            }

            // Sortere og printer scores:
            PrintScores.PrintTopTen(discoverScores);
        }

        public void accept(IFilterVisitor FilterVisitor)
        {
            FilterVisitor.visit(this);
        }

    }

    public class CastFilter : IFilter
    {
        // Til at tildele points
        public List<DiscoverScore> discoverScores = new List<DiscoverScore>();

        public List<Employment> inputEmployments = new List<Employment>();

        public CastFilter(List<Movie> inputMovies, List<Movie> shortList)
        {
            // Er inputtet ok?
            Guard.CheckDiscoverLists(inputMovies, shortList, "Cast Filter");

            Console.WriteLine("");
            Console.WriteLine("FILTER: CAST");
            Console.WriteLine("-------------------------------------------------");

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
                foreach (var employment in inputEmployments.ToList())
                {
                    if (movie._employmentList.Any(x => x._personId == employment._personId))
                    {
                        int Addscore = 1;

                        // If match, then pass out score:
                        if (discoverScores.Any(x => x.Movie == movie))
                        {
                            discoverScores.Find(x => x.Movie == movie).Score += Addscore;
                        }
                        else
                        {
                            discoverScores.Add(new DiscoverScore(movie, 1));
                        }
                    }
                }
            }

            // Sortere og printer scores:
            PrintScores.PrintTopTen(discoverScores);
        }

        public void accept(IFilterVisitor FilterVisitor)
        {
            FilterVisitor.visit(this);
        }
    }

    public class CrewFilter : IFilter
    {
        // Til at tildele points
        public List<DiscoverScore> discoverScores = new List<DiscoverScore>();

        List<Employment> inputEmployments = new List<Employment>();


        // Filtrer Crew
        public CrewFilter(List<Movie> inputMovies, List<Movie> shortList)
        {
            // Er inputtet ok?
            Guard.CheckDiscoverLists(inputMovies, shortList, "Crew Filter");

            Console.WriteLine("");
            Console.WriteLine("FILTER: CREW");
            Console.WriteLine("-------------------------------------------------");
            
            GetCrew getCrew = new GetCrew(inputMovies,inputEmployments);

            Console.WriteLine("Found {0} employments in input", inputEmployments.Count);
            Console.WriteLine("\nSearching Shortlist for matches in crew:");

            // For hver film på Shortlisten, 
            foreach (var movie in shortList.ToList())
            {
                // Ser vi gennem alle jobs, og
                foreach (var employment in inputEmployments.ToList())
                {
                    // ser om de matcher en person
                    if (movie._employmentList.Any(x => x._personId == employment._personId))
                    {
                        int AddScore = 1;

                        // If match, then pass out score:
                        if (discoverScores.Any(x => x.Movie == movie))
                        {
                            discoverScores.Find(x => x.Movie == movie).Score += AddScore;
                        }
                        else
                        {
                            discoverScores.Add(new DiscoverScore(movie, AddScore));
                        }
                    }
                }
            }


            //Console.WriteLine("SAVING JSON!!!!!");

            //string json = JsonConvert.SerializeObject(discoverScores, Formatting.None,
            //            new JsonSerializerSettings()
            //            {
            //                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //            });


            //string[] jsonToText = { json };

            //System.IO.File.WriteAllLines("../DiscoverMoviesProduction.NUnit/JsonStubs/CastFilterReturn.json", jsonToText);


            //Console.WriteLine("DONE!!!");



            // Sortere og printer scores:
            PrintScores.PrintTopTen(discoverScores);
        }


        public void accept(IFilterVisitor FilterVisitor)
        {
            FilterVisitor.visit(this);
        }
    }

    public class YearFilter : IFilter
    {
        // Til at tildele points
        public List<DiscoverScore> discoverScores = new List<DiscoverScore>();

        public YearFilter(List<Movie> inputMovies, List<Movie> shortlist)
        {
            // Er inputtet ok?
            Guard.CheckDiscoverLists(inputMovies, shortlist, "Year Filter");


            Console.WriteLine("");
            Console.WriteLine("FILTER: ReleaseDate");
            Console.WriteLine("-------------------------------------------------");

            int maxYear = 0;
            int minYear = 2022;
            int range;


            Console.WriteLine("Shortlist Max year: " + shortlist.Max(x => x._releaseDate.Value.Year) + 
                             " Min year: " + shortlist.Min(x => x._releaseDate.Value.Year));

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

            Console.WriteLine("Number of Movies: {0}", shortlist.Count());

            foreach (var movie in shortlist.ToList())
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


            // Sortere og printer scores:
            PrintScores.PrintTopTen(discoverScores);
        }

        public void accept(IFilterVisitor FilterVisitor)
        {
            FilterVisitor.visit(this);
        }

    }

    public class ProdFilter : IFilter
    {
        // Til at tildele points
        public List<DiscoverScore> discoverScores = new List<DiscoverScore>();

        List<ProducedBy> inputProds;

        List<ProducedBy> prods;

        public ProdFilter(List<Movie> inputMovies, List<Movie> shortlist)
        {

            // Er inputtet ok?
            Guard.CheckDiscoverLists(inputMovies, shortlist, "Prod Filter");

            Console.WriteLine("");
            Console.WriteLine("FILTER: PRODUCTION");
            Console.WriteLine("-------------------------------------------------");

            using (var db = new MyDbContext())
            {
                // Find ProdCompanies 
                inputProds = db.ProducedBy.Where(x => inputMovies.Select(c => c.movieId).ToList().Contains(x._movieId)).ToList();

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


            foreach (var movie in shortlist.ToList())
            {
                //Console.WriteLine("Shortlist M P Count: " + movie._prodCompanyList.Count());

                using (var db = new MyDbContext())
                {
                    prods = db.ProducedBy.Where(p => p._movieId == movie.movieId).ToList();
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

            // Sortere og printer scores:
            PrintScores.PrintTopTen(discoverScores);
        }

        public void accept(IFilterVisitor FilterVisitor)
        {
            FilterVisitor.visit(this);
        }
    }
    
    public class BudgetRevenueFilter : IFilter
    {

        List<Movie> budgetRevenueList = new List<Movie>();
        public List<DiscoverScore> discoverScores = new List<DiscoverScore>();

        public BudgetRevenueFilter(List<Movie> inputMovies, List<Movie> shortList)
        {
            // Er inputtet ok?
            Guard.CheckDiscoverLists(inputMovies, shortList, "BudgetRevenue Filter");

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
                if (movie._budget <= firstQuartile)
                {
                    score += firstQuartileCount;
                }
                else if (movie._budget <= secondQuartile && movie._budget > firstQuartile)
                {
                    score += secondQuartileCount;
                }
                else if (movie._budget <= thirdQuartile && movie._budget > secondQuartile)
                {
                    score += thirdQuartileCount;
                }
                else if (movie._budget > thirdQuartile)
                {
                    score += fourthQuartileCount;
                }

                if (movie._budget > averageBudget - averageBudget * 0.1 && movie._budget < averageBudget + averageBudget * 0.1)
                {
                    score += 3;
                }

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

            // Sortere og printer scores:
            PrintScores.PrintTopTen(discoverScores);
        }

        public void accept(IFilterVisitor FilterVisitor)
        {
            FilterVisitor.visit(this);
        }
    }
    
}
