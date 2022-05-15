using static DiscoverMoviesProduction.Pages.IndexModel;

namespace DiscoverMoviesProduction.Search.SearchResults
{
    public interface ISearch
    {
        public bool Setattributes(string theinputName, string theinputGenreID, string theinputYear, string theinputSearchtype);
        public List <Movie> SearchInput();
    }
}
