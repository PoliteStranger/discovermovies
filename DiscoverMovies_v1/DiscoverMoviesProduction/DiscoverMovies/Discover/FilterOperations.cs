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
            if (inputScores.Count() > 0)
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



}