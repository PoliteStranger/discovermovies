using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Web.Helpers;


namespace ApiClient
{

    // Film objektet
    public class Movie
    {
        private int _id;
        private string _title;
        private string _description;
        private string _releaseDate;
        private List<int> _genreIds;
        private string _originalLanguage;
        private string _poster;
        private int _length;
        private double _rating;
        private double _budget;
        private double _revenue;


        public Movie(
            int id,
            string title,
            string description,
            string releaseDate,
            List<int> genreIds,
            string originalLanguage,
            string poster,
            int length,
            double rating,
            double budget,
            double revenue)
        {
            _id = id;
            _title = title;
            _description = description;
            _releaseDate = releaseDate;
            _genreIds = genreIds;
            _originalLanguage = originalLanguage;
            _poster = poster;
            _length = length;
            _rating = rating;
            _budget = budget;
            _revenue = revenue;
        }

        public Movie() { }

        public void ConvertMovieDetails(string jsonMovie)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonMovie);

            _id = jsonObj.id;
            _title = jsonObj.title;
            _description = jsonObj.overview;
            _rating = jsonObj.vote_average;
            _budget = jsonObj.budget;
            _releaseDate = jsonObj.release_date;
            _revenue = jsonObj.revenue;
            _length = jsonObj.runtime;
            _poster = jsonObj.poster_path;
            _originalLanguage = jsonObj.original_language;

            _genreIds = new List<int>();

            foreach (var item in jsonObj.genres)
            {
                _genreIds.Add((int)item.id);

            }
        }

        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }
        public string Description
        {
            get { return this._description; }
            set { this._description = value; }
        }
        public string ReleaseDate
        {
            get { return this._releaseDate; }
            set { this._releaseDate = value; }
        }
        public List<int> GenreIds
        {
            get { return this._genreIds; }
            set { this._genreIds = value; }
        }
        public string OriginalLanguage
        {
            get { return this._originalLanguage; }
            set { string oldValue = this._originalLanguage; }
        }

        public string Poster
        {
            get { return this._poster; }
            set { this._poster = value; }
        }

        public int Length
        {
            get { return this._length; }
            set { this._length = value; }
        }

        public double Rating
        {
            get { return this._rating; }
            set { this._rating = value; }
        }

        public double Budget
        {
            get { return this._budget; }
            set { this._budget = value; }
        }

        public double Revenue
        {
            get { return this._revenue; }
            set { this._revenue = value; }
        }
    }

    // Liste over samtlige firmaer
    public class ProdCompanies
    {
        public List<ProdCompany> prodCompanyList;

        public ProdCompanies()
        {
            prodCompanyList = new List<ProdCompany>();
        }

        public void extractProdCompaniesFromMovieDetails(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.production_companies)
            {
                prodCompanyList.Add(new ProdCompany((int)item.id,(string)item.name, (string)item.origin_country));
            }

        }
    }
   
    // Til individuelle firmaer
    public class ProdCompany
    {
        private int _id;
        private string _name;
        private string _country;

        public ProdCompany(int id, string name, string country)
        {
            _id = id;
            _name = name;
            _country = country;
        }

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        public string Country { get { return _country; } }


    }

    public class Persons
    {
        public List<Person> personList;

        public Persons()
        {
            personList = new List<Person>();
        }

        public void extractPersonFromCredits(WorkedOnList workedOnList, Movie movie, string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.cast)
            {
                personList.Add(new Person((int)item.id, (string)item.name, (int)item.gender, (double)item.popularity));
                workedOnList.workedOnList.Add(new WorkedOn((int)item.id, (int)movie.Id, "Acting", (string)item.character));
            }
        }
    }

    public class Person
    {
        private int _id;
        private string _name;
        //private string _dob;
        //private string _dod;
        private int _gender;
        private double _popularity;

        public Person(int id, string name, int gender, double popularity) 
        {
            this._id = id;
            this._name = name;
            this._gender = gender;
            this._popularity = popularity;
        }

        public int Id { get { return _id; } }
        public int Gender { get { return _gender; } }
        public string Name { get { return _name; } }
        public double Popularity { get { return _popularity;} }

    }

    public class WorkedOnList
    {
        public List<WorkedOn> workedOnList;

        public WorkedOnList()
        {
            workedOnList = new List<WorkedOn>();
        }

        public void extractWorkedOnData(Persons persons, Movie movie, string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.crew)
            {
                if(item.job == "Director" ||
                   item.job == "Producer" ||
                   item.job == "Co - Producer" ||
                   item.job == "Screenplay" ||
                   item.job == "Editor" ||
                   item.job == "Director of Photography" ||
                   item.job == "Executive Producer" ||
                   item.job == "Assistant Director" ||
                   item.job == "Production Design" ||
                   item.job == "Costume Design" ||
                   item.job == "Original Music Composer" ||
                   item.job == "Art Direction")
                {
                    persons.personList.Add(new Person((int)item.id, (string)item.name, (int)item.gender, (double)item.popularity));
                    workedOnList.Add(new WorkedOn((int)item.id, (int)movie.Id, (string)item.job, "N/A"));
                }

                    
            }
        }

    }

    public class WorkedOn
    {
        private int _personId;
        private int _movieId;
        private string _job;
        private string _character;

        public WorkedOn(int personId, int movieId, string job, string character)
        {
            _personId = personId;
            _movieId = movieId;
            _job = job;
            _character = character;
        }

        public int PersonId { get { return _personId;} }
        public int MovieId { get { return _movieId;} }
        public string Job { get { return _job; } }  
        public string Character { get { return _character; } }
    }


    class Program
    {


        // Variables



        static void Main()
        {
            // Søg filmen: Zoolander
            Task<string> apiResult = RunApiMovieId("9398");

            // Læg resultatet over i en string (json)
            string jsonMovie = apiResult.Result;

            // Lav et nyt movie object
            Movie movieResult = new Movie();

            // Læg filmdetailjerne over i objektet
            movieResult.ConvertMovieDetails(jsonMovie);

            // Se resultaterne
            Console.WriteLine("ID: " + movieResult.Id);
            Console.WriteLine("Title: " + movieResult.Title);
            Console.WriteLine("Vote Avg: " + movieResult.Rating);
            Console.WriteLine("Budget: " + movieResult.Budget);
            Console.WriteLine("Release Date: " + movieResult.ReleaseDate);
            Console.WriteLine("Revenue: " + movieResult.Revenue);
            Console.WriteLine("Runtime: " + movieResult.Length + " Min");
            Console.WriteLine("Original Language: " + movieResult.OriginalLanguage);
            Console.WriteLine("Poster URL: " + movieResult.Poster);
            Console.WriteLine("Description: " + movieResult.Description);

            // Loop gennem samtlige genre:
            Console.WriteLine("Genres: ");
            foreach (var item in movieResult.GenreIds)
            {
                Console.WriteLine(item);
            }



            // Spacer, for læsbarheden
            Console.WriteLine(" - ");

            // Resultater for produktionsfirmaer
            Console.WriteLine("Production Companies:");

            // Nyt produktionsfirma liste objekt
            ProdCompanies prodResult = new ProdCompanies();

            // Læg resultaterne fra samme film over i listen
            prodResult.extractProdCompaniesFromMovieDetails(jsonMovie);

            // Vis listen:
            foreach (var item in prodResult.prodCompanyList)
            {
                Console.WriteLine(item.Id + ". " + item.Name);
            }

            // Søg rulletekster for filmen: Zoolander
            Task<string> apiCreditsResult = RunApiMovieCredits("9398");

            // Læg resultatet over i en string (json)
            string jsonCredits = apiCreditsResult.Result;

            Persons personResults = new Persons();

            WorkedOnList workedOnList = new WorkedOnList();

            personResults.extractPersonFromCredits(workedOnList, movieResult, jsonCredits);

            workedOnList.extractWorkedOnData(personResults, movieResult, jsonCredits);




            // Spacer, for læsbarheden
            Console.WriteLine(" - ");

            Console.WriteLine("Cast Credits:");

            // Vis listen:
            foreach (var item in personResults.personList)
            {
                Console.WriteLine(item.Id + ". " + item.Name + " - Pop: " + item.Popularity);
            }


            // Spacer, for læsbarheden
            Console.WriteLine(" - ");

            Console.WriteLine("Cast Credits:");

            // Vis listen:
            foreach (var item in workedOnList.workedOnList)
            {
                Console.WriteLine(personResults.personList[personResults.personList.FindIndex(p => p.Id == item.PersonId)].Name  + " was in " + movieResult.Title + " Job: " + item.Job + " - Character: " + item.Character);
            }


        }




        static async Task<string> RunApiMovieId(string movieId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("3/movie/" + movieId + "?api_key=4e00a65a5badd5659a449269be8fcd80");
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

        static async Task<string> RunApiMovieCredits(string movieId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("3/movie/" + movieId + "/credits?api_key=4e00a65a5badd5659a449269be8fcd80");
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
