using System.Linq;
using System.Collections.Generic;
using ASP_Web_Bootstrap;
using Microsoft.EntityFrameworkCore;
using AcquireDB_EFcore.Tables;

namespace DiscoverMoviesProduction
{


    /// <summary>
    /// Visitor Filter Interface
    /// </summary>
    public interface IFilterVisitor
    {
        public void visit(Filters filter);
        public void visit(GenreFilter genre);
        public void visit(CastFilter cast);
        public void visit(CrewFilter crew);
        public void visit(YearFilter year);
        public void visit(ProdFilter prod);
        public void visit(BudgetRevenueFilter BuRe);
    }
    /// <summary>
    /// Gennemgang af alle filtre
    /// </summary>
    public class FilterVisitor : IFilterVisitor
    {

        List<List<DiscoverScore>> AllScores = new List<List<DiscoverScore>>();
        List<DiscoverScore> FinalScores = new List<DiscoverScore>();

        public Movie FinalResult { get; set; }


        public void visit(Filters filters)
        {

            // Læg scores sammen:
            AddingScores.AddScores(AllScores, FinalScores);


            // Udskriv en liste over final scores:
            Console.WriteLine("");
            Console.WriteLine("Final scores:");
            Console.WriteLine("-------------------------------------------------");

            List<DiscoverScore> finalScore = FinalScores.OrderByDescending(x => x.Score).ToList().GetRange(0, 10).ToList();
            foreach (var score in finalScore)
            {
                Console.WriteLine(score.Movie._title + ": " + score.Score.ToString("0.00") + " - " + score.Movie._popularity);
            }

            // Sorter efter Score, og returner så den med højeste score:
            FinalResult = FinalScores.OrderByDescending(x => x.Score).FirstOrDefault().Movie;


            // Generate Report of result!

            //...
        }

        public void visit(GenreFilter filter)
        {
            // Do stuff with filter!

            // Get scores, normalize

            NormalizingScores.Normalize(filter.discoverScores);
            // Add to scores
            AllScores.Add(filter.discoverScores);

        }

        public void visit(CastFilter filter)
        {
            // Do stuff with filter!

            // Get scores, normalize
            NormalizingScores.Normalize(filter.discoverScores);

            // Add to scores
            AllScores.Add(filter.discoverScores);

        }

        public void visit(CrewFilter filter)
        {
            // Do stuff with filter!

            // Get scores, normalize
            NormalizingScores.Normalize(filter.discoverScores);

            // Add to scores
            AllScores.Add(filter.discoverScores);

        }

        public void visit(YearFilter filter)
        {
            // Do stuff with filter!

            // Get scores, normalize
            NormalizingScores.Normalize(filter.discoverScores);

            // Add to scores
            AllScores.Add(filter.discoverScores);

        }

        public void visit(ProdFilter filter)
        {
            // Do stuff with filter!

            // Get scores, normalize
            NormalizingScores.Normalize(filter.discoverScores);

            // Add to scores
            AllScores.Add(filter.discoverScores);

        }

        public void visit(BudgetRevenueFilter filter)
        {
            // Do stuff with filter!

            // Get scores, normalize
            NormalizingScores.Normalize(filter.discoverScores);

            // Add to scores
            AllScores.Add(filter.discoverScores);

        }

    }

}
