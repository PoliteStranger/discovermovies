namespace DiscoverMoviesProduction.Search.SearchResults
{
    public class PersonSearchoption : ISearch
    {
        private List<Movie> templiste = new List<Movie>();

        private string _nameattribute;
        private string _genreidattribute;
        private string _yearattribute;
        private string _searchattribute;

        public void Setattributes(string theinputName, string theinputGenreID, string theinputYear,
            string theinputSearchtype)
        {
            _nameattribute = theinputName;
            _genreidattribute = theinputGenreID;
            _yearattribute = theinputYear;
            _searchattribute = theinputSearchtype;

            if (theinputSearchtype=="Person")
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
                var query = (from p in db.Persons
                             join e in db.Employments
                             on p._personId equals e._personId
                             join m in db.Movies
                             on e._movieId equals m.movieId
                             join gm in db.GenresAndMovies
                             on m.movieId equals gm._movieId
                             join g in db.Genres
                             on gm._genreId equals g._genreId

                             where (_nameattribute == ""  || p._Personname.Contains(_nameattribute))
                             && (_genreidattribute == "0" || gm._genreId == Int32.Parse(_genreidattribute))
                             && (m._releaseDate.Value.Year == Int32.Parse(_yearattribute) || _yearattribute == "0")
                             && (_searchattribute == "Person" || _searchattribute == "")
                             select m
                            ).ToList().Distinct(); // til liste og fjerner samtidig duplikater.
                return query.ToList();
            }
        }
    }
}
