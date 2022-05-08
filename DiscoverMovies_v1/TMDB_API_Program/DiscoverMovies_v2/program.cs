using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Web.Helpers;
using DiscoverMovies_v2;


namespace API_DiscoverAlgorithm
{

    // Programmet
    class Program
    {

        static void Main()
        {

            // Vi starter med 5 film!
            List<int> compareMovies = new List<int>() { 512195, 744, 954, 75612, 155 };

            // Steven Spielberg: 329, 857, 424, 879, 85

            // Opret et filmliste objekt
            Movies moviesList = new Movies();

            // Opret en Employed liste objekt
            EmployedList employedList = new EmployedList();

            // Opret en Persons liste over alle personer
            Persons personResults = new Persons();


            // Liste over alle produktions selvskaber som har arbejdet på hvilke film
            ProdCompareList prodCompareList = new ProdCompareList();  
            
            ProdCompanies prodCompaniesList = new ProdCompanies();

            // Liste over alle film, og hvilke genre som de har
            GenreCompareList genreCompareList = new GenreCompareList();


            // Opret et Aquire Movie Details objekt, som afvikler algoritmen
            aquireMovieDetails getMovies = new aquireMovieDetails();

            // Der loopes gennem de 5 valgte film:
            foreach (var movie in compareMovies)
            {
                // Afvikler API kald, for at få film detailjerne:
                getMovies.getMovieDetails(movie, moviesList, employedList, personResults, prodCompaniesList, prodCompareList, genreCompareList);

                Console.WriteLine("Getting: " + moviesList.moviesList.Last()._title);
            }

            List<Person> noDupesPersonList = personResults.personList.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            // Vi har nu
            Console.WriteLine(".-------------------------------------------------------------.");
            Console.WriteLine("Movies: " + moviesList.moviesList.Count);
            Console.WriteLine("Employments: " + employedList.employedList.Count);
            Console.WriteLine("Persons: " + noDupesPersonList.Count);
            Console.WriteLine("Production Companies: " + prodCompaniesList.prodCompanyList.Count);
            Console.WriteLine("");




            // Find alle instruktøre og producere
            Console.WriteLine("Find: Directors and Producers of selected movies:");

            // Vi laver vores Movies Shortlist
            MovieTitlesList movieTitlesShortList = new MovieTitlesList();

            Console.Write("Downloading their credits >");

            // Vi fylder intruktøre og producers i listen:
            foreach(var person in employedList.employedList)
            {
                if (person.Job == "Director" ||
                    person.Job == "Producer")
                {
                    // Hent rulleteksterne for en film
                    Task<string> apiCreditsResult = ApiOps.RunApiPersonCredits(person.PersonId);

                    Console.Write(">");

                    // Læg resultatet over i en string (json)
                    string jsonPersonCredits = apiCreditsResult.Result;

                    dynamic jsonObj = JsonConvert.DeserializeObject(jsonPersonCredits);

                    foreach (var movie in jsonObj.crew) // crew, cast
                    {
                        movieTitlesShortList.movieTitlesList.Add(new MovieTitles((int)movie.id, (string)movie.title, (double)movie.popularity, (string)movie.release_date, (string)movie.original_language));
                    }
                }
            }
            Console.WriteLine("<DONE>");





            // Find alle top stjerner
            Console.WriteLine("Find: Movie stars of selected movies: (with popularity > 20.0)");


            Console.Write("Downloading their credits >");

            // Vi fylder intruktøre og producers i listen:
            foreach (var person in employedList.employedList)
            {
                if (person.Job == "Actor" && personResults.personList[personResults.personList.FindIndex(x => x.Id == person.PersonId)].Popularity > 20.0 )
                {
                    //Console.WriteLine(personResults.personList[personResults.personList.FindIndex(x => x.Id == person.PersonId)].Name + " Pop: " + personResults.personList[personResults.personList.FindIndex(x => x.Id == person.PersonId)].Popularity);
                    // Hent rulleteksterne for en film
                    Task<string> apiCreditsResult = ApiOps.RunApiPersonCredits(person.PersonId);

                    Console.Write(">");

                    // Læg resultatet over i en string (json)
                    string jsonPersonCredits = apiCreditsResult.Result;

                    dynamic jsonObj = JsonConvert.DeserializeObject(jsonPersonCredits);

                    foreach (var movie in jsonObj.cast)
                    {
                        movieTitlesShortList.movieTitlesList.Add(new MovieTitles((int)movie.id, (string)movie.title, (double)movie.popularity, (string)movie.release_date, (string)movie.original_language));
                    }
                }
            }
            Console.WriteLine("<DONE>");





            Console.WriteLine("Shortlist Count: " + movieTitlesShortList.movieTitlesList.Count);
            Console.WriteLine("Removing duplicates!");




            // Fjern alle som går igen: duplikater:
            List<MovieTitles> movieTitlesShortListNoDupes = movieTitlesShortList.movieTitlesList.GroupBy(x => x._id).Select(x => x.First()).ToList();

            // Fjern de oprindelige film, som blev søgt med:

            //Console.WriteLine("Shortlist Count: " + movieTitlesShortListNoDupes.Count);
            //Console.WriteLine("Removing original selection!");
            foreach (var movieId in compareMovies)
            {
                movieTitlesShortListNoDupes.RemoveAt(movieTitlesShortListNoDupes.FindIndex(x => x._id == movieId));
            }

            Console.WriteLine("Shortlist Count: " + movieTitlesShortListNoDupes.Count);
            Console.WriteLine("Popularity Filter ->");
            movieTitlesShortListNoDupes.RemoveAll(x => x._popularity < 20.0);




            Console.WriteLine("Shortlist Count: " + movieTitlesShortListNoDupes.Count);


            List<MovieTitles> movieTitlesShortListNoDupessdfsdf = movieTitlesShortListNoDupes.OrderByDescending(x => x._popularity).ToList();


            Console.WriteLine("\n \nAll:");

            int iterator = 1;

            foreach(var movie in movieTitlesShortListNoDupessdfsdf)
            {
                Console.WriteLine(iterator + ". " + movie._title + "   " + movie._popularity);
                iterator++;
            }

            // MADS's Tests
            List<MovieTitles> Genresorted = new List<MovieTitles>();

            Genresorted = compareGenres.SortMethod(movieTitlesShortListNoDupes, moviesList);

            foreach (var movie in Genresorted)
            {
                Console.WriteLine(movie._title + " Genresorted");
            }
            Console.WriteLine("###################################");
            Console.WriteLine("###################################");
            Console.WriteLine("###################################");
            Console.WriteLine("###################################");
            foreach (var movie in movieTitlesShortListNoDupes)
            {
                Console.WriteLine(movie._title + " Notsorted");
            }



        }
    }
}
