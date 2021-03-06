using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DiscoverMoviesProduction
{
  
    /// <summary>
    /// Normalisere scores i et MovieScore objekt
    /// </summary>
    public class NormalizingScores
    {
        public static void Normalize(List<DiscoverScore> inputScores)
        {
            // Hvis der er Scores:
            if (inputScores.Count() > 0)
            {
                // Find MAX og MIN Scores
                double MaxScore = inputScores.Max(x => x.Score);
                double MinScore = inputScores.Min(x => x.Score);

                // Normaliser alle Scores
                foreach (DiscoverScore movie in inputScores)
                {
                    movie.Score = (double)(1 / MaxScore) * inputScores.Find(x => x.Movie == movie.Movie).Score;
                }
            }
        }
    }

    /// <summary>
    /// Lægger alle filmscores sammen
    /// </summary>
    public class AddingScores
    {
        public static void AddScores(List<List<DiscoverScore>> allScores, List<DiscoverScore> finalScores)
        {
            foreach (var scoreList in allScores)
            {
                foreach (var movieScore in scoreList)
                {
                    // Tjek om filmen allerede findes på listen
                    if (finalScores.Any(x => x.Movie.movieId == movieScore.Movie.movieId))
                    {
                        //Console.WriteLine("Adding {0} to {1}", movieScore.Score, finalScores.Find(x => x.Movie.movieId == movieScore.Movie.movieId).Score);
                        // Brug nuværende film, og føj til score
                        finalScores.Find(x => x.Movie.movieId == movieScore.Movie.movieId).Score += movieScore.Score;
                    }
                    else
                    {
                        // Opret en ny film
                        finalScores.Add(new DiscoverScore(movieScore.Movie, movieScore.Score));
                    }
                }
            }
        }
    }

    public class PrintScores
    {
        public static void PrintTopTen(List<DiscoverScore> discoverScores)
        {
            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();

            int range = 10;
            if (discoverScores.Count() < 10) range = discoverScores.Count();
            foreach (var score in discoverScores.GetRange(0, range))
            {
                Console.WriteLine(score.Movie._title + " - " + score.Score);
            }
        }
    }

    public class GetCrew
    {
        public GetCrew(List<Movie> inputMovies, List<Employment> inputEmployments)
        {
            foreach (var movie in inputMovies.ToList())
            {
                foreach (var employment in movie._employmentList.ToList())
                {
                    if (employment._job != "Acting")
                    {
                        inputEmployments.Add(employment);
                    }
                }
            }
        }
    }

}