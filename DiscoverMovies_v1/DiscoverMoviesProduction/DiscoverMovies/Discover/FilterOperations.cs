using System.Linq;
using System.Collections.Generic;
using ASP_Web_Bootstrap;
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
                    movie.Score = (double)(1 / (MaxScore - MinScore)) * inputScores.Find(x => x.Movie == movie.Movie).Score;
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
                foreach(var movieScore in scoreList)
                {
                    // Tjek om filmen allerede findes på listen
                    if (finalScores.Any(x => x.Movie == movieScore.Movie))
                    {
                        // Brug nuværende film, og føj til score
                        finalScores.Find(x => x.Movie == movieScore.Movie).Score += movieScore.Score;
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


}