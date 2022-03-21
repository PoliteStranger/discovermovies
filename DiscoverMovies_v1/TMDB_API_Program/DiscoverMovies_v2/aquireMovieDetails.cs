using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DiscoverAlgorithm
{
    internal class aquireMovieDetails
    {

        public void getMovieDetails(int movieId, Movies moviesList, EmployedList employedList, Persons personResults, ProdCompanies prodCompaniesList, ProdCompareList prodCompareList, GenreCompareList genreCompareList)
        {
            // Henter overordnede film detaljer fra TMDB
            Task<string> apiResult = ApiOps.RunApiMovieId(movieId);

            // Læg resultatet over i en string (json)
            string jsonMovie = apiResult.Result;


            // Læg filmdetailjerne over i film objektet
            moviesList.ExtractFromJson(jsonMovie);

            // Læg filmens genre over i genre listen:
            genreCompareList.ExtractFromJson(jsonMovie);

            // Læg filmens prod selvskaber, associationer over i prodCompare listen:
            prodCompareList.ExtractFromJson(jsonMovie);

            // Læg filmens prod selvskaber over i ProdListen
            prodCompaniesList.ExtractFromJson(jsonMovie);


            // Hent rulleteksterne for en film
            Task<string> apiCreditsResult = ApiOps.RunApiMovieCredits(movieId);

            // Læg resultatet over i en string (json)
            string jsonCredits = apiCreditsResult.Result;

            // Læg person credits ind i personer
            //personResults.ExtractPersonFromCredits(employedList, moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)], jsonCredits);

            // læg
            employedList.ExtractFromJson(personResults, moviesList.moviesList[moviesList.moviesList.FindIndex(p => p._id == movieId)], jsonCredits);

        }
    }
}
