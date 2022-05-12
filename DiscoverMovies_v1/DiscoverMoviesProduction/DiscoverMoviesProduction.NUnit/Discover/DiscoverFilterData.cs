using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverFilterData
    {

        public static List<Movie> GetJsonMovies(string file)
        {
            if (System.IO.File.Exists(file + ".json") == true)
            {
                Console.WriteLine("Loading file: " + file + ".json");

                return JsonConvert.DeserializeObject<List<Movie>>(File.ReadAllText(file + ".json"));
            }
            else
            {
                Console.WriteLine("Error: File " + file + ".json, does not exist!");

                return new List<Movie>();
            }
        }

        public static List<DiscoverScore> GetJsonScores(string file)
        {
            if (System.IO.File.Exists(file + ".json") == true)
            {
                Console.WriteLine("Loading file: " + file + ".json");

                return JsonConvert.DeserializeObject<List<DiscoverScore>>(File.ReadAllText(file + ".json"));
            }
            else
            {
                Console.WriteLine("Error: File " + file + ".json, does not exist!");

                return new List<DiscoverScore>();
            }
        }

    }
}
