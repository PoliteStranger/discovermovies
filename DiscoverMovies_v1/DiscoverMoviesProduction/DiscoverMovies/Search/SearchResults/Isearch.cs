using static DiscoverMoviesProduction.Pages.IndexModel;

namespace DiscoverMoviesProduction.Search.SearchResults
{
    public interface ISearch
    {
        public void Setattributes(string theinputName, string theinputGenreID, string theinputYear, string theinputSearchtype);
        public List <Movie> SearchInput();
    }
}
