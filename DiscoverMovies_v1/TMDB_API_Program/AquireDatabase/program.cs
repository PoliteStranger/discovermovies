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

            moviesList.Add(new Movie((int)jsonObj.id, (string)jsonObj.title, (string)jsonObj.overview, (string)jsonObj.release_date, genreIds, (string)jsonObj.original_laguage, (string)jsonObj.poster_path, (int)jsonObj.runtime, (double)jsonObj.popularity, (int)jsonObj.budget, (int)jsonObj.revenue));


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
            int revenue)
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

    // En enkelt film med simple detailjer
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

    // Programmet
    class Program
    {

        static void Main()
        {

            // SUMMERY: Aquire Movie Database - v.1
            //
            // Denne følgende kodedel downloader samtlige film titler (KUN titler m. id) fra TMDB
            // med formålet at få en liste over samtlige film som skal lægges i vores database.

            // Indstillinger:

            int fromYear = 1987;
            int toYear = 2015;

            // For ikke at lave alt for mange API kald, så er alle film fra 1990'erne blevet cached i nogle json filer,
            // så der tjekkes nu om disse filer kan findes, og hvis de kan det, så bliver de loadet i stedet for at kalde APIen i stedet for!
            // Bliver de ikke fundet, så laves der nye. Ligger i mappen: DataStore/

            // Skab et objekt af ApiLoop klassen (Crazy loop!)
            aquireApiLoop downloadMovieTitles = new aquireApiLoop();

            // Opret en liste, som filmtitelerne skal lægges i
            List<MovieTitles> movieTitlesList = new List<MovieTitles>();

            // Vi looper nu gennem den årrække, som blev valgt foroven:
            for (var year = fromYear; year <= toYear; year++)
            {
                // Vi ser nu om json filerne allerede findes:
                if (File.Exists("../../../DataStore/Movies_From_" + year + ".json") == true)
                {
                    // Dette år findes:
                    Console.WriteLine("File: Movies_From_" + year + ".json Exists!");

                    // Loader json filen ind i et dynamisk objekt
                    dynamic jsonObj = JsonConvert.DeserializeObject(File.ReadAllText("../../../DataStore/Movies_From_" + year + ".json"));


                    // Flytter json dataene ind i listen som vi lavede tidligere:
                    foreach(var movieTitle in jsonObj)
                    {
                        // De bliver lagt i movieTitlesList
                        movieTitlesList.Add(new MovieTitles((int)movieTitle._id, (string)movieTitle._title, (double)movieTitle._popularity, (string)movieTitle._releaseDate, (string)movieTitle._originalLanguage));
                    }
                }
                else
                {
                    // En json fil kunne ikke findes, så vi downloader dette år:

                    // vi bruger et objekt af klassen: aquireApiLoop, og downloader det manglende års værd af film titeler:
                    foreach (var movieTitle in downloadMovieTitles.getYear(year))
                    {
                        // De bliver lagt i movieTitlesList
                        movieTitlesList.Add(new MovieTitles((int)movieTitle._id, (string)movieTitle._title, (double)movieTitle._popularity, (string)movieTitle._releaseDate, (string)movieTitle._originalLanguage));
                    }
                }
                
            }
            // Hvor mange film titeler blev samlet:
            Console.WriteLine("Movie titles collected: " + movieTitlesList.Count);



            // SUMMERY: Aquire Movie Details v.0.9 - Mangler DoB og DoD for personer!
            // 
            // Denne udkommenterede kode downloader alle detailer for en enkelt film.
            // Sæt movieId lig med et eksisterende film id.


            
            // Film id som skal søges på
            int movieId = 634649;
            // Opret et film objekt
            Movies moviesList = new Movies();
            // Opret en WorkedOn/Employed liste objekt
            WorkedOnList workedOnList = new WorkedOnList();
            // Opret en Persons liste over alle personer
            Persons personResults = new Persons();
            // Opret en ProdCompanies, liste over samtlige produktionsselvskaber
            ProdCompanies prodResult = new ProdCompanies();

            // Opret et Aquire Movie Details objekt, som afvikler algoritmen
            aquireMovieDetails getMovies = new aquireMovieDetails();

            // Afvikler selve algoritmen:
            //getMovies.getMovieDetails(99, moviesList, workedOnList, personResults, prodResult);

            // Filmen, el filmene, kan nu findes i objekterne moviesList, og resten kan findes i hhv. personResults, og prodResults.
            // Algorithmen: getMovies.getMovieDetails() kan bruges gentagende gange, og lægger bare flere film, personer, Employed, og produktionsselvskaber i objekterne.

            // Film Id'et kan også bare skrives direkte ind i funktionen:
            // getMovies.getMovieDetails(568124, moviesList, workedOnList, personResults, prodResult);

            



            // Overfør til SQL DB

        }
    }
}
