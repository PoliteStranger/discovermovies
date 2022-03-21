using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_DiscoverAlgorithm;

namespace DiscoverMovies_v2
{
    public class compareGenres
    {
        // Output en liste
        // Adgang til listen over oprindelige film
        // Adgang til movieTitleShortList

        //Constructor
        compareGenres(List<MovieTitles> movieList)
        {
           

        }

        private List<MovieTitles> movieList = new List<MovieTitles>();

        public List<MovieTitles> SortMethod(List<MovieTitles> movieList)
        {
            foreach (var movie in movieList)
            {
                movie.


            }


            return movieList;
        }

        // laver en liste over samtlige genre fra alle 5 film

        // tælle duplikaterne: Fks: Action x5, drama x2 ..... => Action, drama, thriller

        // Sortere movieShortList efter disse genre, popularity 

        // outputter vi den nye liste:






    }
}
