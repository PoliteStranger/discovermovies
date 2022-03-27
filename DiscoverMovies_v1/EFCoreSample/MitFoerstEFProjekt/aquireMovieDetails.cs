using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MitFoerstEFProjekt.Tables;

namespace MyFirstProject
{
    public class aquireMovieDetails
    {

        public void getMovieDetails(int movieId, MyDbContext db)
        {
            Movie newMovie = new Movie();

            Console.WriteLine("Downloading movie info for: " + movieId);

            // Henter overordnede film detaljer fra TMDB, som json
            Task<string> apiResult = ApiOps.RunApiMovieId(movieId);

            // Læg resultatet over i en string (json)
            string jsonMovie = apiResult.Result;

            //Console.WriteLine(jsonMovie);

            // json svar til dynamisk objekt
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonMovie);

            newMovie._title = (string)jsonObj.title;
            newMovie.movieId = (int)jsonObj.id;
            newMovie._popularity = (double)jsonObj.popularity;
            newMovie._revenue = (int)jsonObj.revenue;
            newMovie._budget = (int)jsonObj.budget;
            newMovie._runtime = (int)jsonObj.runtime;

            // Konvertering af dato fra json yyyy-MM-DD til DateTime
            DateTime theDate;
            DateTime.TryParseExact(
                (string)jsonObj.release_date,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out theDate);

            newMovie._releaseDate = theDate;

            Console.WriteLine("Saving movie genres.");

            // Gemmer samtlige genre fra filmen i dens liste over genre
            foreach(var genre in jsonObj.genres)
            {
                newMovie._genreList.Add(new Genre() { _genreId = genre.id, _movieId = movieId});
            }

            Console.WriteLine("Downloading moviecredits info for: " + movieId);

            // Henter overordnede film detaljer fra TMDB, som json
            Task<string> apiResultCredits = ApiOps.RunApiMovieCredits(movieId);

            // Læg resultatet over i en string (json)
            string jsonMovieCredits = apiResultCredits.Result;

            // json svar til dynamisk objekt
            dynamic jsonObjCredits = JsonConvert.DeserializeObject(jsonMovieCredits);

            // Lav listen over employments - Kun dem med Popularity +X

            // Lav listen over personer - Fjern duplicates, og kun dem



            Console.WriteLine("Saving movie in DB!");
            db.Movies.Add(newMovie);
            db.SaveChanges();
        }
    }
}
