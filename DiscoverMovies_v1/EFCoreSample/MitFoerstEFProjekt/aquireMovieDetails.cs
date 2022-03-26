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

            Console.WriteLine("Downloading film: " + movieId);

            // Henter overordnede film detaljer fra TMDB
            Task<string> apiResult = ApiOps.RunApiMovieId(movieId);

            // Læg resultatet over i en string (json)
            string jsonMovie = apiResult.Result;

            //Console.WriteLine(jsonMovie);

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonMovie);

            newMovie._title = (string)jsonObj.title;
            newMovie.movieId = (int)jsonObj.id;
            newMovie._popularity = (double)jsonObj.popularity;
            newMovie._revenue = (int)jsonObj.revenue;
            newMovie._budget = (int)jsonObj.budget;
            newMovie._runtime = (int)jsonObj.runtime;

            DateTime theDate;
            DateTime.TryParseExact(
                (string)jsonObj.release_date,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out theDate);

            newMovie._releaseDate = theDate;

            List<Genre> genreList = new List<Genre>();

            foreach(var genre in jsonObj.genres)
            {
                Genre newMovieGenre = new Genre();
                newMovieGenre.genreId = (int)genre.id;
                newMovieGenre._Genrename = (string)genre.name;


                genreList.Add();
            }


            Console.WriteLine("Saving film in DB!");
            db.Movies.Add(newMovie);
            db.SaveChanges();
        }
    }
}
