using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Web.Helpers;
using MyFirstProject;

namespace API_DiscoverAlgorithm
{
    public class aquireApiLoop
    {

        public List<MovieTitles> getYear(int year)
        {
            // Vi vil gerne holde styr på hvor ofte vi kalder APIen!
            int ApiCalls = 0;

            // Vi laver tegnsættet som skal søges med
            char[] alpha = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

            // Vi opretter filmliste objektet
            MovieTitlesList movieTitlesList = new MovieTitlesList();

            Console.WriteLine("Searching the year: " + year);

            // Vi starter loopet som skal gennemgå alle tegn
            foreach (char c in alpha)
            {
                Console.Write("Searching for: " + c + " >");

                // Søger på første tal/karakter
                Task<string> apiTitlesSearchResult = ApiOps.RunApiMovieSearch(year, c, 1);
                
                ApiCalls++; // Tæller Api kald!

                // Læg resultatet over i en string (json)
                string jsonResults = apiTitlesSearchResult.Result;
                dynamic jsonObj = JsonConvert.DeserializeObject(jsonResults);

                // Loop gennem alle sider:
                if ((int)jsonObj.total_pages > 1)
                {
                    // Loop through the rest of the pages:
                    for (var i = 0; i < (int)jsonObj.total_pages; i++)
                    {
                        
                        Console.Write(">");

                        // Kald Api m.:Årstal, Tegn, Side
                        Task<string> apiTitlesSearchResults = ApiOps.RunApiMovieSearch(year, c, i + 1);

                        // Træk data ud af json resultat, og tilføj til listen.
                        movieTitlesList.extractMovieTitlesData(apiTitlesSearchResults.Result);

                        ApiCalls++; // Tæller Api kald!
                    }
                    
                }
                Console.WriteLine("<DONE>");
            }

            List<MovieTitles> noDupesList = movieTitlesList.movieTitlesList.GroupBy(x => x._id).Select(x => x.First()).ToList();


            // Vi vil nu vise hvad vi har downloadet:
            foreach (MovieTitles movie in noDupesList)
            {
                //Console.WriteLine(movie._id + ". " + movie._title);
            }

            Console.WriteLine(" ");

            Console.WriteLine("Movies from 0-9, a-z from " + year);

            Console.WriteLine("Movies found total: " + movieTitlesList.movieTitlesList.Count + " In " + ApiCalls + " Api calls!");

            Console.WriteLine("Removing duplicates:");

            Console.WriteLine("Movies total: " + noDupesList.Count);

            string[] jsonToText = { JsonConvert.SerializeObject(noDupesList.OrderBy(x => x._title)) };

            File.WriteAllLines("../../../DataStore/Movies_From_"+year+".json", jsonToText);


            return noDupesList;
        }

        
    }
}
