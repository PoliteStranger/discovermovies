using static ASP_Web_Bootstrap.Pages.IndexModel;

namespace ASP_Web_Bootstrap.Search.SearchResults
{
    public interface ISearch
    {
        public bool Setattributes(string theinputName, string theinputGenreID, string theinputYear, string theinputSearchtype);
        public List <Movie> SearchInput();
    }
}
