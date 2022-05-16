using System.Linq;
using System.Collections.Generic;
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






}

