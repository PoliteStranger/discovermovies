namespace ASP_Web_Bootstrap.Search.SearchResults
{
    public class PersonSearchoption : ISearch
    {
        private List<Movie> templiste = new List<Movie>();

        private string nameattribute;
        private string genreidattribute;
        private string yearattribute;
        private string searchattribute;

        public bool Setattributes(string theinputName, string theinputGenreID, string theinputYear,
            string theinputSearchtype)
        {
            nameattribute = theinputName;
            genreidattribute = theinputGenreID;
            yearattribute = theinputYear;
            searchattribute = theinputSearchtype;

            return true;
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

                             where (p._Personname.Contains(nameattribute) || nameattribute == "")
                             && (genreidattribute == "0" || gm._genreId == Int32.Parse(genreidattribute))
                             && (m._releaseDate.Value.Year == Int32.Parse(yearattribute) || yearattribute == "0")
                             && (searchattribute == "Person" || searchattribute == "")
                             select m
                            ).ToList().Distinct(); // til liste og fjerner samtidig duplikater.

                return query.ToList();
            }
        }
    }
}
