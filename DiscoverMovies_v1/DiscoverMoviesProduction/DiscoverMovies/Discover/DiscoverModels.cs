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






}

