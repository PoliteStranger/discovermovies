namespace DiscoverMoviesProduction.Search.SearchResults
{
    public class NullSearchoption : ISearch
    {
        private string _nameattribute;
        private string _genreidattribute;
        private string _yearattribute;
        private string _searchattribute;

        public void Setattributes(string theinputName, string theinputGenreID, string theinputYear,
            string theinputSearchtype)
        {
            if (theinputSearchtype=="0")
            {
                _nameattribute = "";
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
            set { _nameattribute = ""; }
        }

        public string Genreattribute
        {
            get { return _genreidattribute; }
            set { _genreidattribute = value; }
        }

        public string Yearattribute
        {
            get { return _yearattribute; }
            set { _yearattribute = value; }
        }
        public string Searchattribute
        {
            get { return _searchattribute; }
            set { _searchattribute = value; }
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
                             where (_genreidattribute == "0" || gm._genreId == Int32.Parse(_genreidattribute))
                              && (_yearattribute == "0" || m._releaseDate.Value.Year == Int32.Parse(_yearattribute))
                             select m
                    ).ToList().Distinct(); // til liste og fjerner samtidig duplikater.

                return query.ToList();
            }
        }
    }
}
