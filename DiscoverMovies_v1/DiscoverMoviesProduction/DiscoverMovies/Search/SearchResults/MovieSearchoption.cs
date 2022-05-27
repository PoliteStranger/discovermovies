using System.Collections;
using DiscoverMoviesProduction;

namespace DiscoverMoviesProduction.Search.SearchResults
{
    public class MovieSearchoption : ISearch
    {
        private string _nameattribute;
        private string _genreidattribute;
        private string _yearattribute;
        private string _searchattribute;

        public void Setattributes(string theinputName, string theinputGenreID, string theinputYear,
            string theinputSearchtype)
        {

            if (theinputSearchtype=="Movie")
            {
                _nameattribute = theinputName;
                _genreidattribute = theinputGenreID;
                _yearattribute = theinputYear;
                _searchattribute = theinputSearchtype;
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public string Nameattribute
        {
            get { return _nameattribute; }
        }

        public string Genreattribute
        {
            get { return _genreidattribute; }
        }
        public string Yearattribute
        {
            get { return _yearattribute; }
        }
        public string Searchattribute
        {
            get { return _searchattribute; }
        }

        public List<Movie> SearchInput()
        {
            using (var db = new MyDbContext())
            {
                var query = (from m in db.Movies
                             join gm in db.GenresAndMovies
                             on m.movieId equals gm._movieId
                             join g in db.Genres
                             on gm._genreId equals g._genreId
                             where _nameattribute == "" || (m._title.Contains(_nameattribute))
                                   && (_genreidattribute == "0" || gm._genreId == Int32.Parse(_genreidattribute))
                                   && (m._releaseDate.Value.Year == Int32.Parse(_yearattribute) || _yearattribute == "0")
                                   && (_searchattribute == "Movie" || _searchattribute == "")
                             select m
                    ).ToList().Distinct();

                return query.ToList();
            }
        }
    }
}
