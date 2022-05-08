using static ASP_Web_Bootstrap.Pages.IndexModel;

namespace ASP_Web_Bootstrap.Models.SearchResults
{
    public interface Isearch
    {
        public List<Movie> SearchInput(string theinputName, string theinputGenreID, string theinputYear, string theinputSearchtype);
    }
}
