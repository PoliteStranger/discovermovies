using Newtonsoft.Json;
using AcquireDB_EFcore;

namespace AcquireDB_EFcore
{
    public class acquireApiLoop
    {
        // Vi opretter filmliste objektet
        List<int> movieTitlesList = new List<int>();

        public List<int> getYear(int year)
        {
            if (File.Exists("../../../DataStore/MovieLists/Movies_From_" + year + ".json") == true)
            {
                // Dette år findes:
                Console.WriteLine("Loading jsonfile movielist for " + year);

                // Loader json filen ind i listeobjektet
                movieTitlesList = JsonConvert.DeserializeObject<List<int>>(File.ReadAllText("../../../DataStore/MovieLists/Movies_From_"+year+".json"));
            }
            else
            {
                // Vi vil gerne holde styr på hvor ofte vi kalder APIen!
                int ApiCalls = 0;

                // Vi laver tegnsættet som skal søges med
                char[] alpha = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

                Console.WriteLine("Downloading movielist for " + year);

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

                            dynamic SearchResultsObj = JsonConvert.DeserializeObject(apiTitlesSearchResults.Result);

                            // Træk data ud af json resultat, og tilføj til listen.
                            foreach (var movieResults in SearchResultsObj.results)
                            {

                                //Console.WriteLine("T: " + movieResults.title);
                                // Findes den allerede?
                                if(!movieTitlesList.Contains((int)movieResults.id))
                                    movieTitlesList.Add((int)movieResults.id);
                                

                            }

                            ApiCalls++; // Tæller Api kald!
                        }

                    }

                    Console.WriteLine("<DONE> Got: " + (int)jsonObj.total_results + " Movies");
                }

                // Fjern kopier
                //List<int> noDupesList = movieTitlesList.GroupBy(x => x).Select(x => x.First()).ToList();

                string[] jsonToText = { JsonConvert.SerializeObject(movieTitlesList) };

                File.WriteAllLines("../../../DataStore/MovieLists/Movies_From_" + year + ".json", jsonToText);

                Console.WriteLine("API calls: " + ApiCalls);
                Console.WriteLine("Movies found: " + movieTitlesList.Count);
                //Console.WriteLine("Movies selected: " + noDupesList.Count);

            }



            return movieTitlesList;
        }
        
        
    }
}
