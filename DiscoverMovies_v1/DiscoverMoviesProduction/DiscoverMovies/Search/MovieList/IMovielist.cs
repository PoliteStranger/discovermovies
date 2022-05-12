using ASP_Web_Bootstrap.Search.SearchResults;

namespace ASP_Web_Bootstrap.Search.MovieList
{
    public interface IMovielist
    {
        public List<Movie> ListOfMovies(List<Searchclass> sqlliste);
    }
}
