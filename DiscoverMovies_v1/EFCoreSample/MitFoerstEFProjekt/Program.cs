// See https://aka.ms/new-console-template for more information
using AcquireDB_EFcore;

Console.WriteLine("Ripping TMDB movies database");

// Opretter et AcquireApiLoop objekt, det gennemgår alle film i et givet årstal, og laver en liste over alle deres IDer
acquireApiLoop tis = new acquireApiLoop();

// Åres: 1999 vælges, og listen laves
List<int> moviesList = tis.getYear(1997);


// 27205, 329, 553, 271110, 862

using (var db = new MyDbContext())
{


    


    // Gemmer alle genre i databasen
    MakeGenresList allGenres = new MakeGenresList(db);

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
    foreach(int movieId in moviesList)
    {
        // Vi henter KUN en film, hvis den ikke allerede ER i databasen!
        if(!db.Movies.Any(x => x.movieId == movieId))
        {
            // Vi tæller film tælleren en op
            

            dlMovieCount = moviesList.IndexOf(movieId)+1;
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