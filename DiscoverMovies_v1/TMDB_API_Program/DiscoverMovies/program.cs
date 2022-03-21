using System;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Web.Helpers;


namespace API_DiscoverAlgorithm
{

    // Liste over samtlige film
    public class Movies
    {
        public List<Movie> moviesList;

        public Movies()
        {
            moviesList = new List<Movie>();
        }

        public void ExtractMovieDetails(string jsonMovie)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonMovie);

            List<int> genreIds = new List<int>();

            foreach (var item in jsonObj.genres)
            {
                genreIds.Add((int)item.id);

            }

            List<int> prodCompaniesId = new List<int>();

            foreach (var item in jsonObj.production_companies)
            {
                prodCompaniesId.Add((int)item.id);

            }

            moviesList.Add(new Movie((int)jsonObj.id, (string)jsonObj.title, (string)jsonObj.overview, (string)jsonObj.release_date, genreIds, (string)jsonObj.original_laguage, (string)jsonObj.poster_path, (int)jsonObj.runtime, (double)jsonObj.popularity, (int)jsonObj.budget, (int)jsonObj.revenue, prodCompaniesId));


        }
    }

    // Film objektet
    public class Movie
    {
        public int _id;
        private string _title;
        private string _description;
        private string _releaseDate;
        private List<int> _genreIds;
        private string _originalLanguage;
        private string _poster;
        private int _length;
        private double _popularity;
        private int _budget;
        private int _revenue;
        private List<int> _prodCompanies;


        public Movie(
            int id,
            string title,
            string description,
            string releaseDate,
            List<int> genreIds,
            string originalLanguage,
            string poster,
            int length,
            double popularity,
            int budget,
            int revenue,
            List<int> prodCompanies)
        {
            _id = id;
            _title = title;
            _description = description;
            _releaseDate = releaseDate;
            _genreIds = genreIds;
            _originalLanguage = originalLanguage;
            _poster = poster;
            _length = length;
            _popularity = popularity;
            _budget = budget;
            _revenue = revenue;
            _prodCompanies = prodCompanies;
        }

        public Movie() { }

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

        public double Popularity
        {
            get { return this._popularity; }
            set { this._popularity = value; }
        }

        public int Budget
        {
            get { return this._budget; }
            set { this._budget = value; }
        }

        public int Revenue
        {
            get { return this._revenue; }
            set { this._revenue = value; }
        }

        public List<int> ProdCompanies
        {
            get { return this._prodCompanies; }
            set { this._prodCompanies = value; }
        }
    }

    // Produktionsselvskaber relateret til film de har lavet
    public class ProdCompare
    {
        public int ProdId;
        public int MovieId;

        public ProdCompare(int prodId, int movieId)
        {
            this.MovieId = movieId;
            this.ProdId = prodId;
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

        public void extractProdCompaniesFromMovieDetails(string json, List<ProdCompare> prodCompareList)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.production_companies)
            {
                prodCompanyList.Add(new ProdCompany((int)item.id,(string)item.name, (string)item.origin_country));
                prodCompareList.Add(new ProdCompare((int)item.id, (int)jsonObj.id));
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

    // Liste over alle personer
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

    // En enkelt person
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

    // Worked On / Employed, liste over hvem har arbejdet på hvilke film
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

    // En employment, en person har arbejdet på hvilken film
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

    // Liste over simple filmdetailjer
    public class MovieTitlesList
    {
        public List<MovieTitles> movieTitlesList;

        public MovieTitlesList()
        {
            movieTitlesList = new List<MovieTitles>();
        }

        public void extractMovieTitlesData(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.results)
            {
                movieTitlesList.Add(new MovieTitles((int)item.id, (string)item.title, (double)item.popularity, (string)item.release_date, (string)item.original_language));
            }
        }

    }

    // En enkelt film med simple detailjer - beregnet til at lave film søgelister
    public class MovieTitles
    {
        public int _id;
        public string _title;
        public double _popularity;
        public string _releaseDate;
        public string _originalLanguage;

        public MovieTitles(int id, string title, double popularity, string releaseDate, string originalLanguage)
        {
            this._id = id;
            this._title = title;
            this._releaseDate = releaseDate;
            this._originalLanguage = originalLanguage; 
            this._popularity = popularity;
        }
    }


    public class Genres
    {
        public List<Genre> genres;

        public Genres()
        {
            genres = new List<Genre>();

            // Loader json filen ind i et dynamisk objekt
            dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText("../../../DataStore/APIcalls/movieGenres.json"));

            foreach(var item in jsonObj.genres)
            {
                genres.Add(new Genre((int)item.id, (string)item.name));
            }
        }
    }

    public class Genre
    {
        public int Id;
        public string GenreName;

        public Genre(int id, string genreName)
        {
            Id = id;
            GenreName = genreName;
        }
    }

    public class GenreCompare
    {
        public int GenreId;
        public int MovieId; 

        public GenreCompare(int genreId, int movieId)
        {
            GenreId = genreId;
            MovieId = movieId;
        }
    }


    // Programmet
    class Program
    {

        static void Main()
        {

            List<int> compareMovies = new List<int>() { 329, 857, 424, 879, 85, 578 };

            // Opret et film objekt
            Movies moviesList = new Movies();
            // Opret en WorkedOn/Employed liste objekt
            WorkedOnList workedOnList = new WorkedOnList();
            // Opret en Persons liste over alle personer
            Persons personResults = new Persons();
            // Opret en ProdCompanies, liste over samtlige produktionsselvskaber
            ProdCompanies prodResult = new ProdCompanies();

            List<ProdCompare> prodCompareList = new List<ProdCompare>();

            // Opret et Aquire Movie Details objekt, som afvikler algoritmen
            aquireMovieDetails getMovies = new aquireMovieDetails();

            foreach (var movie in compareMovies)
            {
                // Afvikler API kald, for at få film detailjerne:
                getMovies.getMovieDetails(movie, moviesList, workedOnList, personResults, prodResult, prodCompareList);
            }

            List<Person> noDupesPersonList = personResults.personList.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            // Vi har nu
            Console.WriteLine(".-------------------------------------------------------------.");
            Console.WriteLine("Movies: " + moviesList.moviesList.Count);
            Console.WriteLine("Employments: " + workedOnList.workedOnList.Count);
            Console.WriteLine("Persons: " + noDupesPersonList.Count);
            Console.WriteLine("Production Companies: " + prodResult.prodCompanyList.Count);
            Console.WriteLine("");


            // Hvor mange personer går igen:
            Console.WriteLine("Person duplicates:");

            // Find duplikater i listen
            var moviesCommon = workedOnList.workedOnList.GroupBy(test => test.PersonId)
                                                    .Where(group => group.Skip(1).Any());
            // Vis resultaterne
            foreach (var employment in moviesCommon)
            {
                Console.WriteLine(personResults.personList[personResults.personList.FindIndex(x => x.Id == employment.Key)].Name + ": " + employment.Count());

            }

            Console.WriteLine("");

            Console.WriteLine("Genre duplicates:");
            List<GenreCompare> genreCompares = new List<GenreCompare>();
            // 
            foreach (var genres in moviesList.moviesList)
            {
                foreach(var genre in genres.GenreIds)
                {
                    genreCompares.Add(new GenreCompare(genre, genres._id));
                }
                
            }

            Genres genresList = new Genres();

            var commonGenres = genreCompares.GroupBy(test => test.GenreId)
                                        .Where(group => group.Skip(1).Any());
            // Vis resultaterne
            foreach (var genres in commonGenres)
            {
                Console.WriteLine(genresList.genres[genresList.genres.FindIndex(x => x.Id == genres.Key)].GenreName + ": " + genres.Count());

            }

            // Vis duplikater blandt produktions selvskaber
            Console.WriteLine("");

            Console.WriteLine("Production Company duplicates:");
            var commonProdCompanies = prodCompareList.GroupBy(test => test.ProdId)
                                        .Where(group => group.Skip(1).Any());
            // Vis resultaterne
            foreach (var prods in commonProdCompanies)
            {
                Console.WriteLine(prodResult.prodCompanyList[prodResult.prodCompanyList.FindIndex(x => x.Id == prods.Key)].Name + ": " + prods.Count());

            }


            // Vis årstal interval

            int? minYear = null;
            int? maxYear = null;

            foreach (var movie in moviesList.moviesList)
            {
                string[] date = movie.ReleaseDate.Split('-');
                if (minYear == null) minYear = int.Parse(date[0]);
                if (maxYear == null) maxYear = int.Parse(date[0]);
                if (int.Parse(date[0]) < minYear)
                {
                    minYear = int.Parse(date[0]);
                }
                if (int.Parse(date[0]) > maxYear)
                {
                    maxYear = int.Parse(date[0]);
                }

            }

            Console.WriteLine("");
            Console.WriteLine("Release year range: " + minYear + " - " + maxYear);



            // Discover
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Recommendations:");



            List<WorkedOn> workedOnResults = new List<WorkedOn>();

            foreach (var employment in moviesCommon)
            {
                Task<string> apiResultb = ApiOps.RunApiPersonCredits(personResults.personList.FindIndex(x => x.Id == employment.Key));


                //Console.WriteLine(apiResultb.Result);

                var newJson = apiResultb.Result;

                if (newJson != "NA")
                {
                    // Læg resultatet over i en string (json)
                    dynamic jsonObjb = JsonConvert.DeserializeObject(newJson);

                    foreach (var item in jsonObjb.cast)
                    {
                        workedOnResults.Add(new WorkedOn((int)employment.Key, (int)item.id, "actor", (string)item.character));
                    }
                    foreach (var item in jsonObjb.crew)
                    {
                        workedOnResults.Add(new WorkedOn((int)employment.Key, (int)item.id, (string)item.job, "N/A"));
                    }
                }



            }

            Console.WriteLine("");

            Console.WriteLine("Result duplicates:");
            var workedOnResultsFinal = workedOnResults.GroupBy(test => test.MovieId)
                                        .Where(group => group.Skip(1).Any());
            // Vis resultaterne
            foreach (var prods in workedOnResultsFinal)
            {
                Console.WriteLine(prods.Key + ": " + prods.Count());

            }


            // Overfør til SQL DB

        }
    }
}
