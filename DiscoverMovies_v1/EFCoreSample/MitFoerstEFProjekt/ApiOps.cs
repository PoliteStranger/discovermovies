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
using API_DiscoverAlgorithm;

 
namespace MyFirstProject
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

                //                                                      search/movie?api_key=4e00a65a5badd5659a449269be8fcd80&language=en-US&query=a&page=1&include_adult=false&primary_release_year=1991
                //Console.WriteLine("3/search/movie?api_key=4e00a65a5badd5659a449269be8fcd80&language=en-US&query=" + letter + "&page=" + page + "&include_adult=false&primary_release_year=" + year);
                HttpResponseMessage response = await client.GetAsync("3/search/movie?api_key=4e00a65a5badd5659a449269be8fcd80&language=en-US&query=" + letter + "&page=" + page + "&include_adult=false&primary_release_year=" + year);
                //Console.WriteLine("URL: " + client.BaseAddress + "3/search/movie" + movieId + "?api_key=4e00a65a5badd5659a449269be8fcd80");
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
                //Console.WriteLine("3/movie/" + movieId.ToString() + "?api_key=4e00a65a5badd5659a449269be8fcd80");
                //Console.WriteLine("URL: " + client.BaseAddress + "3/search/movie/" + movieId + "?api_key=4e00a65a5badd5659a449269be8fcd80");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Got Response!");
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

        public static async Task<string> RunApiMovieCredits(int movieId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("3/movie/" + movieId.ToString() + "/credits?api_key=4e00a65a5badd5659a449269be8fcd80");
                //Console.WriteLine("URL: " + client.BaseAddress + "3/search/movie/" + movieId + "?api_key=4e00a65a5badd5659a449269be8fcd80");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Got Response!");
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
    }
}
