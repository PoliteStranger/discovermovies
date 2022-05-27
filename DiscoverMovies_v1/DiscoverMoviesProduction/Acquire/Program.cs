// See https://aka.ms/new-console-template for more information
using AcquireDB_EFcore;
using AcquireDB_EFcore.Tables;
using Newtonsoft.Json;

Console.WriteLine("Ripping TMDB movies database");

// Opretter et AcquireApiLoop objekt, det gennemgår alle film i et givet årstal, og laver en liste over alle deres IDer
acquireApiLoop loop = new acquireApiLoop();

// Åres: 1999 vælges, og listen laves
List<int> moviesList = loop.getYear(1994);


// 27205, 329, 553, 271110, 862

using (var db = new MyDbContext())
{

    // Gemmer alle genre i databasen
    //MakeGenresList allGenres = new MakeGenresList(db);

    // Der oprettes et AcquireMovieDetails objekt, som henter alle film detaljer ud fra et film ID
    acquireMovieDetails n = new acquireMovieDetails();


    // Download movie data to database:

    // Midlertidig liste til at teste med:
    //List<int> moviesList = new List<int>() { 9836, 508947 , 566525, 438695, 476669, 329, 857 };

    // Vi begynder at downloade film
    Console.WriteLine("Downloading {0} movies", moviesList.Count);
    // Vi tæller hvor langt vi er nået:
    int dlMovieCount = 0;
    // Download loopet begynder:
    foreach (int movieId in moviesList)
    {
        // Vi henter KUN en film, hvis den ikke allerede ER i databasen!
        if (!db.Movies.Any(x => x.movieId == movieId))
        {
            // Vi tæller film tælleren en op


            dlMovieCount = moviesList.IndexOf(movieId) + 1;
            // Vi skriver hvilken film vi er nået til
            Console.WriteLine("\n\nDownloading {0}/{1}", dlMovieCount, moviesList.Count);
            // Vi downloader filmens info:
            n.getMovieDetails(movieId, db);

        }
        else
        {
            Console.Write(".");

        }
    }
}

//using (var db = new MyDbContext())
//{
//    acquireMovieDetails film = new acquireMovieDetails();

//    film.getMovieDetails(19995, db);
//}

////19995

/*
// Fikse PROD COMPANY listen i DB
using (var db = new MyDbContext())
{
    int i = 0;
    foreach(var movie in db.Movies.ToList())
    {
        i++;
        Console.WriteLine("{0}/{1} Getting Prod Companies for movie: " + movie._title + ": " + movie.movieId, i, db.Movies.Count());



        if(db.ProducedBy.Any(x => x._movieId == movie.movieId) == false)
        {
            Console.WriteLine("Missing from ProducedBy, looking for companies!");
            // Henter overordnede film detaljer fra TMDB, som json
            Task<string> apiResult = ApiOps.RunApiMovieId(movie.movieId);

            // Læg resultatet over i en string (json)
            string jsonMovie = apiResult.Result;

            // json svar til dynamisk objekt
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonMovie);


            // Gemmer samtlige Produktions Firmaer
            foreach (var company in jsonObj.production_companies)
            {
                Console.WriteLine("Found some!");
                //Console.WriteLine("Getting: "+ (string)company.name);

                // Hvis firmaet ikke findes i databasen, så kan vi både tilføje det og lægge det i filmens firma liste
                if (db.ProdCompanies.Find((int)company.id) == null)
                {
                    //Console.WriteLine("New company, adding it to Movie ProdCompany List!");
                    ProdCompany newComp = new ProdCompany() { ProdCompanyId = (int)company.id, _ProdCompanyname = (string)company.name, _ProdCompanycountry = (string)company.origin_country };
                    db.ProdCompanies.Add(newComp);
                    movie._ProdCompaniesList.Add(new ProducedBy() { prodCompanyId = (int)company.id, _movieId = movie.movieId });

                }
                else
                {
                    //Console.WriteLine("Already have this company, adding it to Movie ProdCompany List!");
                    // Den findes allerede, så vi bruger referencen fra den som er der.
                    movie._ProdCompaniesList.Add(new ProducedBy() { prodCompanyId = (int)company.id, _movieId = movie.movieId });
                }


            }
            Console.WriteLine("Saving...");
            db.SaveChanges();
        }
        else
        {
            Console.WriteLine("Skipping this one...");
        }


    }


}

*/