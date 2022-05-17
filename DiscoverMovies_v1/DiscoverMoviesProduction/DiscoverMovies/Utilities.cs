using Newtonsoft.Json;

namespace DiscoverMoviesProduction
{


    /// <summary>
    /// Timer til at tage tid på de lage DB kald!
    /// Skab et objekt, og kør Start, så kør Stop, når tiden skal måles. Den kan stoppes flere gange.
    /// </summary>
    public class LoadTimer
    {
        private DateTime Start { get; set; }

        private DateTime End { get; set; }

        public void StartTimer()
        {
            Start = DateTime.Now;
            Console.WriteLine("Starting time now: " + Start);
        }

        public void StopTimer()
        {
            if(Start != default)
            {
                End = DateTime.Now;
                Console.WriteLine("Time taken: " + (End - Start));
            }
            else
            {
                Console.WriteLine("Must start timer, before stopping it!");
            }

        }

    }

    /// <summary>
    /// Til at Tjekke NULL Exceptions
    /// </summary>
    public class Guard
    {
        public static void CheckDiscoverLists(List<Movie> inputMovies, List<Movie> shortList, string Msg)
        {
            // Check if value is null.
            if (inputMovies == null || shortList == null)
                throw new System.ArgumentNullException("Null List Error: " + Msg);

        }


    }


    /// <summary>
    /// Til at konvertere objekter til json filer, til STUB brug i NUnit
    /// </summary>
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

            System.IO.File.WriteAllLines("../DiscoverMoviesProduction.NUnit/JsonStubs/" + filename + ".json", jsonToText);

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

        public ObjectToJson(List<Person> inputMovies, string filename)
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
