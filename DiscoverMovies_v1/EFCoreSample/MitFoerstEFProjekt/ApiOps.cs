using System.Net.Http.Headers;


namespace AcquireDB_EFcore
{
    public class ApiOps
    {
        public static async Task<string> RunApiMovieSearch(int year, char letter, int page)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(
                    "3/search/movie?api_key=4e00a65a5badd5659a449269be8fcd80&language=en-US&query=" + 
                    letter + "&page=" + 
                    page + "&include_adult=false&primary_release_year=" + 
                    year
                    );

                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Got Response!");
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("Status Code:");
                    Console.WriteLine(response.StatusCode.ToString());
                    return "Json: Nothing.";
                }
            }
        }

        // Til at slå film op med via ID:
        public static async Task<string> RunApiMovieId(int movieId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("3/movie/" + movieId.ToString() + "?api_key=4e00a65a5badd5659a449269be8fcd80");
                
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Got Response!");
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("Movie Details API CALL Status Code:");
                    Console.WriteLine(response.StatusCode.ToString());
                    return "Json: Nothing.";
                }
            }
        }

        public static async Task<string> RunApiMovieCredits(int movieId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("3/movie/" + movieId.ToString() + "/credits?api_key=4e00a65a5badd5659a449269be8fcd80");
                
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Got Response!");
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("Movie Credits API CALL Status Code:");
                    Console.WriteLine(response.StatusCode.ToString());
                    return "Json: Nothing.";
                }
            }
        }


        public static async Task<string> RunApiPerson(int personId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("3/person/" + personId.ToString() + "?api_key=4e00a65a5badd5659a449269be8fcd80");
                
                if (response.IsSuccessStatusCode)
                {
                    //Console.WriteLine("Got Response!");
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("Person API CALL Status Code:");
                    Console.WriteLine(response.StatusCode.ToString());
                    return "Json: Nothing.";
                }
            }
        }
    }
}
