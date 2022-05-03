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

        public void ExtractFromJson(string jsonMovie)
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

            moviesList.Add(new Movie((int)jsonObj.id, (string)jsonObj.title, (string)jsonObj.overview, (string)jsonObj.release_date, (string)jsonObj.original_laguage, (string)jsonObj.poster_path, (int)jsonObj.runtime, (double)jsonObj.popularity, (int)jsonObj.budget, (int)jsonObj.revenue));


        }
    }

    // Film objektet
    public class Movie
    {
        public int _id { get ; set; }
        public string _title { get; set; }
        private string _description { get; set; }
        private string _releaseDate { get; set; }
        private string _originalLanguage { get; set; }
        private string _poster { get; set; }
        private int _length { get; set; }
        private double _popularity { get; set; }
        private int _budget { get; set; }
        private int _revenue { get; set; }


        public Movie(
            int id,
            string title,
            string description,
            string releaseDate,
            string originalLanguage,
            string poster,
            int length,
            double popularity,
            int budget,
            int revenue)
        {
            _id = id;
            _title = title;
            _description = description;
            _releaseDate = releaseDate;
            _originalLanguage = originalLanguage;
            _poster = poster;
            _length = length;
            _popularity = popularity;
            _budget = budget;
            _revenue = revenue;
        }

        public Movie() { }


    }

    public class ProdCompareList
    {
        public List<ProdCompare> prodCompareList;

        public ProdCompareList()
        {
            prodCompareList = new List<ProdCompare>();
        }

        public void ExtractFromJson(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.production_companies)
            {
                prodCompareList.Add(new ProdCompare((int)item.id, (int)jsonObj.id));
            }

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
        public void ExtractFromJson(string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.production_companies)
            {
                prodCompanyList.Add(new ProdCompany((int)item.id, (string)item.name, (string)item.origin_country));
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

        public void ExtractPersonFromCredits(EmployedList employedList, Movie movie, string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.cast)
            {
                personList.Add(new Person((int)item.id, (string)item.name, (int)item.gender, (double)item.popularity));
                employedList.employedList.Add(new Employment((int)item.id, (int)movie._id, "Acting", (string)item.character));
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
        public double Popularity { get { return _popularity; } }

    }

    // Worked On / Employed, liste over hvem har arbejdet på hvilke film
    public class EmployedList
    {
        public List<Employment> employedList;

        public EmployedList()
        {
            employedList = new List<Employment>();
        }

        public void ExtractFromJson(Persons persons, Movie movie, string json)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(json);

            foreach (var item in jsonObj.crew)
            {
                if (item.job == "Director" ||               // Vælger de jobs til som vi vil bruge.
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
                    employedList.Add(new Employment((int)item.id, (int)movie._id, (string)item.job, "N/A"));
                }
            }
            foreach (var item in jsonObj.cast)
            {
                // Se navnene, til tjek/feedback.
                //Console.WriteLine((string)item.name);

                //
                persons.personList.Add(new Person((int)item.id, (string)item.name, (int)item.gender, (double)item.popularity));
                employedList.Add(new Employment((int)item.id, (int)movie._id, "Actor", (string)item.character));
                
            }

        }

    }

    // En employment, en person har arbejdet på hvilken film
    public class Employment
    {
        private int _personId;
        private int _movieId;
        private string _job;
        private string _character;

        public Employment(int personId, int movieId, string job, string character)
        {
            _personId = personId;
            _movieId = movieId;
            _job = job;
            _character = character;
        }

        public int PersonId { get { return _personId; } }
        public int MovieId { get { return _movieId; } }
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

    // Alle genre som liste:
    public class Genres
    {
        public List<Genre> genres;

        public Genres()
        {
            genres = new List<Genre>();

            // Loader json filen ind i et dynamisk objekt
            dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText("../../../DataStore/APIcalls/movieGenres.json"));

            foreach (var item in jsonObj.genres)
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

    public class GenreCompareList
    {
        public List<GenreCompare> genresCompare;

        public GenreCompareList()
        {
            genresCompare = new List<GenreCompare>();
        }

        public void ExtractFromJson(string jsonMovie)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonMovie);

            List<int> genreIds = new List<int>();

            foreach (var item in jsonObj.genres)
            {
                genresCompare.Add(new GenreCompare((int)item.id, (int)jsonObj.id));

            }
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


    public class ShortListResult
    {
        public int MovieId { get { return MovieId; } }

        public int FromPersonId { get { return FromPersonId; } }

        public string FromFilter { get { return FromFilter; } }


    }
}
