using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AcquireDB_EFcore.Tables;

namespace AcquireDB_EFcore
{
    public class acquireMovieDetails
    {

        public void getMovieDetails(int movieId, MyDbContext db)
        {

            // *********************************************************************
            //  SUMMERY
            //  1. Download stamdata for filmen:
            // 
            // *********************************************************************


            // Opretter en ny film:
            Movie newMovie = new Movie();

            Console.WriteLine("----------------------------");


            // Henter overordnede film detaljer fra TMDB, som json
            Task<string> apiResult = ApiOps.RunApiMovieId(movieId);

            // Læg resultatet over i en string (json)
            string jsonMovie = apiResult.Result;

            // json svar til dynamisk objekt
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonMovie);

            Console.WriteLine("Getting Movie:   " + movieId + ": " + (string)jsonObj.title + " ("+ (double)jsonObj.popularity + ") ");

            // Stamdata sættes:
            newMovie._title = (string)jsonObj.title;
            newMovie.movieId = (int)jsonObj.id;
            newMovie._popularity = (double)jsonObj.popularity;
            newMovie._revenue = (long)jsonObj.revenue;
            Console.WriteLine("Revenue: " + newMovie._revenue + "   API: " + jsonObj.revenue);
            newMovie._budget = (int)jsonObj.budget;
            // Hvis en runtime er NULL, giver det bøvl, så vi sætter alle runtime som er NULL til 0
            if(jsonObj.runtime == null)
            {
                newMovie._runtime = 0;
            }
            else
            {
                newMovie._runtime = (int)jsonObj.runtime;
            }
            
            newMovie._releaseDate = dateFromJson((string)jsonObj.release_date);
            newMovie._description = (string)jsonObj.overview;
            newMovie._posterUrl = (string)jsonObj.poster_path;

            // *********************************************************************
            //  SUMMERY
            //  2. Alle filmens genre bliver lagt ind i filmens objekt
            // 
            // *********************************************************************

            //Console.Write("API-Genres: ");

            // Gemmer samtlige genre fra filmen i dens liste over genre
            foreach(var genre in jsonObj.genres)
            {
                //Console.Write(db.Genres.Find((int)genre.id)._Genrename + ", ");
                newMovie._genreList.Add(new Genre() { _genreId = (int)genre.id, _movieId = movieId});
            }
            


            // *********************************************************************
            //  SUMMERY
            //  3. Alle produktionsselvskaberne bliver lagt ind i filmens objekt
            // 
            // *********************************************************************

            //Console.Write("API-ProdComp: ");

            // Tællere til nye objekter!
            int statsPers = 0;
            int statsEmployments = 0;
            int statsProds = 0;


            // Gemmer samtlige Produktions Firmaer
            foreach (var company in jsonObj.production_companies)
            {
                //Console.Write((string)company.name + ", ");

                // Hvis firmaet ikke findes i databasen, så kan vi både tilføje det og lægge det i filmens firma liste
                if (db.ProdCompanies.Find((int)company.id) == null)
                {
                    ProdCompany newComp = new ProdCompany() 
                    { 
                        ProdCompanyId = (int)company.id, 
                        _ProdCompanyname = (string)company.name, 
                        _ProdCompanycountry = (string)company.origin_country 
                    };

                    db.ProdCompanies.Add(newComp);  
                    newMovie._ProdCompaniesList.Add(new ProducedBy() { prodCompanyId = (int)company.id, _movieId = movieId });
                    statsProds++;
                }
                else
                {
                    // Den findes allerede, så vi bruger referencen fra den som er der.
                    newMovie._ProdCompaniesList.Add(new ProducedBy() { prodCompanyId = (int)company.id, _movieId = movieId });
                }
            }
            


            // <--------------------------------------------------------------- Film Credits info!

            // Henter overordnede film detaljer fra TMDB, som json
            Task<string> apiResultCredits = ApiOps.RunApiMovieCredits(movieId);

            // Læg resultatet over i en string (json)
            string jsonMovieCredits = apiResultCredits.Result;

            // json svar til dynamisk objekt
            dynamic jsonObjCredits = JsonConvert.DeserializeObject(jsonMovieCredits);

            // <---------------------------------------------------------------Film Crew!

            foreach (var credit in jsonObjCredits.crew)
            {
                // Vi gemmer IKKE alle Crew: Kun dem i følgende filter:
                if (   credit.job == "Director" ||
                       credit.job == "Producer" ||
                       credit.job == "Co - Producer" ||
                       credit.job == "Screenplay" ||
                       credit.job == "Editor" ||
                       credit.job == "Director of Photography" ||
                       credit.job == "Executive Producer" ||
                       credit.job == "Assistant Director" ||
                       credit.job == "Production Design" ||
                       credit.job == "Costume Design" ||
                       credit.job == "Original Music Composer" ||
                       credit.job == "Art Direction"
                   )
                {
                    // Hvis personen IKKE findes, så gemmer vi den person:
                    if (db.Persons.Find((int)credit.id) == null)
                    {
                        
                        Person newPerson = GetPerson((int)credit.id);
                        db.Persons.Add(newPerson);
                        statsPers++;
                    }

                    // Personens job tilføjes:
                    newMovie._employmentList.Add(new Employment()
                    { 
                        _personId = (int)credit.id, 
                        _movieId = movieId, 
                        _job = (string)credit.job, 
                        _character = "N/A"
                    });
                    statsEmployments++;
                }
            }

            // <---------------------------------------------------------------Film Cast!
            foreach (var credit in jsonObjCredits.cast)
            {
                // Hvis personen IKKE findes, så gemmer vi den person:
                if (db.Persons.Find((int)credit.id) == null)
                {
                    Person newPerson = GetPerson((int)credit.id);
                    
                    db.Persons.Add(newPerson);
                    statsPers++;
                }

                newMovie._employmentList.Add(new Employment()
                { 
                    _personId = (int)credit.id, 
                    _movieId = movieId, 
                    _job = "Acting", 
                    _character =  (string)credit.character }
                );
                statsEmployments++;
            }


            // Vi gemmer allerede nu! Fordi at vi skal have alle personerne ind,
            // så deres foreign keys kan referes til!
            db.SaveChanges();
            Console.WriteLine("");
            Console.WriteLine("New Persons:     " + statsPers);
            Console.WriteLine("New Employments: " + statsEmployments);
            Console.WriteLine("New Companies:   " + statsProds);

            db.Movies.Add(newMovie);
            db.SaveChanges();





        }

        public Person GetPerson(int personId)
        {
            string personJson;

            // Henter overordnede film detaljer fra TMDB, som json
            Task<string> personApi = ApiOps.RunApiPerson(personId);

            // Læg resultatet over i en string (json)
            personJson = personApi.Result;

            // json svar til dynamisk objekt
            dynamic p = JsonConvert.DeserializeObject(personJson);

            Person newPerson = new Person() 
            {   
                _personId = (int)p.id, 
                _Personname = (string)p.name, 
                _dob = dateFromJson((string)p.birthday),
                _dod = dateFromJson((string)p.deathday), 
                _biography = (string)p.biography, 
                _gender = (int)p.gender, 
                _imageUrl = (string)p.profile_path, 
                _Personpopularity = (double)p.popularity 
            };

            return newPerson;
        }

        public DateTime dateFromJson(string json)
        {
            DateTime theDate;
            DateTime.TryParseExact(
                (string)json,
                    "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out theDate);

            return theDate;
        }

        

    }
}
