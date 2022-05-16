using DiscoverMoviesProduction.Search.SearchResults;

namespace DiscoverMoviesProduction.Search
{
    public class ResolveSearch
    {
        List<Movie> tempMovieList;



        public List<Movie> Resolve(string Name, string GenreID, string Year, string Searchtype, ISearch film, ISearch person, ISearch nul)
        {

            if (Searchtype == "Movie")
            {
                //ISearch filmsoegning = film;
                //filmsoegning.Setattributes(Name, GenreID, Year, Searchtype);
                //tempMovieList = filmsoegning.SearchInput();
                film.Setattributes(Name, GenreID, Year, Searchtype);
                tempMovieList = film.SearchInput();
            }

            else if (Searchtype == "Person")
            {
                //ISearch personsøgning = person;
                //personsøgning.Setattributes(Name, GenreID, Year, Searchtype);
                //tempMovieList = personsøgning.SearchInput();

                person.Setattributes(Name, GenreID, Year, Searchtype);
                tempMovieList = person.SearchInput();

            }

            else if (Searchtype == "0")
            {
                //ISearch nullsøgning = nul;
                //nullsøgning.Setattributes(Name, GenreID, Year, Searchtype);
                //tempMovieList = nullsøgning.SearchInput();
                nul.Setattributes(Name, GenreID, Year, Searchtype);
                tempMovieList = nul.SearchInput();

            }

            return tempMovieList;
        }


    }
}
