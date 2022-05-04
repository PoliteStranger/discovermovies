using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_DiscoverAlgorithm
{
    internal class aquireMovieDetails
    {

        public void getMovieDetails(int movieId, Movies moviesList, WorkedOnList workedOnList, Persons personResults, ProdCompanies prodResult, List<ProdCompare> prodCompareList)
        {
            Task<string> apiResult = ApiOps.RunApiMovieId(movieId);

            // Læg resultatet over i en string (json)
            string jsonMovie = apiResult.Result;

            //Console.WriteLine(jsonMovie);

            // Læg filmdetailjerne over i objektet
            moviesList.ExtractMovieDetails(jsonMovie);

            // Se resultaterne
            Console.WriteLine("Getting Movie:");
            Console.WriteLine("ID: " + moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)].Id);
            Console.WriteLine("Title: " + moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)].Title);
            //Console.WriteLine("Vote Avg: " + movieResult.Rating);
            //Console.WriteLine("Budget: " + movieResult.Budget);
            Console.WriteLine("Release Date: " + moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)].ReleaseDate);
            //Console.WriteLine("Revenue: " + movieResult.Revenue);
            Console.WriteLine("Runtime: " + moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)].Length + " Min");
            //Console.WriteLine("Original Language: " + movieResult.OriginalLanguage);
            //Console.WriteLine("Poster URL: " + movieResult.Poster);
            //Console.WriteLine("Description: " + movieResult.Description);


            // Loop gennem samtlige genre:
            Console.WriteLine("Genres: " + moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)].GenreIds.Count);
            foreach (var item in moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)].GenreIds)
            {
                //Console.WriteLine(item);
            }



            // Resultater for produktionsfirmaer
            Console.WriteLine("Getting production Companies...");

           

            // Læg resultaterne fra samme film over i listen
            prodResult.extractProdCompaniesFromMovieDetails(jsonMovie, prodCompareList);

            /*
            // Vis listen:
            foreach (var item in prodResult.prodCompanyList)
            {
                Console.WriteLine(item.Id + ". " + item.Name + " - " + item.Country);
            }
            */

            // Søg rulletekster for filmen: Zoolander
            Task<string> apiCreditsResult = ApiOps.RunApiMovieCredits(movieId);

            // Læg resultatet over i en string (json)
            string jsonCredits = apiCreditsResult.Result;

            Console.WriteLine("Getting cast...");
            personResults.extractPersonFromCredits(workedOnList, moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)], jsonCredits);

            Console.WriteLine("Getting crew...");
            workedOnList.extractWorkedOnData(personResults, moviesList.moviesList[moviesList.moviesList.FindIndex(p => p.Id == movieId)], jsonCredits);

            Console.WriteLine("");
            /*
            // Vis listen:
            foreach (var item in personResults.personList)
            {
                Console.WriteLine(item.Id + ". " + item.Name + " - Pop: " + item.Popularity + " - " + item.Gender);
            }
            */

            /*
            // Vis listen:
            foreach (var item in workedOnList.workedOnList)
            {
                Console.WriteLine(personResults.personList[personResults.personList.FindIndex(p => p.Id == item.PersonId)].Name + " was in " + movieResult.Title + " Job: " + item.Job + " - Character: " + item.Character);
            }
            */
        }
    }
}
