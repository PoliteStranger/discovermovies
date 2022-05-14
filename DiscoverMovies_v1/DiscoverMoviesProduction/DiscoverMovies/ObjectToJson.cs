using DiscoverMoviesProduction;
using Newtonsoft.Json;

namespace ASP_Web_Bootstrap
{
    public class ObjectToJson
    {


        public ObjectToJson(List<Movie> inputMovies, string filename)
        {
            Console.WriteLine("SAVING JSON!!!!!");

            string json = JsonConvert.SerializeObject(inputMovies, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });


            string[] jsonToText = { json };

            System.IO.File.WriteAllLines("../DiscoverMoviesProduction.NUnit/JsonStubs/"+filename+".json", jsonToText);


            Console.WriteLine("DONE!!!");
        }

        public ObjectToJson(List<DiscoverScore> inputMovies, string filename)
        {
            Console.WriteLine("SAVING JSON!!!!!");

            string json = JsonConvert.SerializeObject(inputMovies, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });


            string[] jsonToText = { json };

            System.IO.File.WriteAllLines("../DiscoverMoviesProduction.NUnit/JsonStubs/" + filename + ".json", jsonToText);


            Console.WriteLine("DONE!!!");
        }

        public ObjectToJson(List<Employment> inputMovies, string filename)
        {
            Console.WriteLine("SAVING JSON!!!!!");

            string json = JsonConvert.SerializeObject(inputMovies, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });


            string[] jsonToText = { json };

            System.IO.File.WriteAllLines("../DiscoverMoviesProduction.NUnit/JsonStubs/" + filename + ".json", jsonToText);


            Console.WriteLine("DONE!!!");
        }
    }
}
