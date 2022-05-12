using ASP_Web_Bootstrap.Search.SearchResults;

namespace ASP_Web_Bootstrap.Search.MovieList
{
    public class MovieListFromDB : IMovielist
    {
        private List<Movie> tempmovies = new List<Movie>();

        public List<Movie> ListOfMovies(List<Searchclass> sqlliste)
        {
            foreach (var item in sqlliste)
            {
                Movie tempmovie = new Movie();
                tempmovie._title = item.movietitel;
                tempmovie._posterUrl = item.movieposter;
                tempmovie.movieId = item.movieid;
                tempmovies.Add(tempmovie);
            }

            Console.WriteLine("Search Results: " + tempmovies.Count());
            return tempmovies;
        }

    }
    }
