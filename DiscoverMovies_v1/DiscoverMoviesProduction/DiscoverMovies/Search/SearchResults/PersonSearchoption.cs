namespace ASP_Web_Bootstrap.Search.SearchResults
{
    public class PersonSearchoption : ISearch
    {
        private List<Movie> templiste = new List<Movie>();

        public List<Movie> SearchInput(string theinputName, string theinputGenreID, string theinputYear, string theinputSearchtype)
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

                             where (p._Personname.Contains(theinputName) || theinputName == "")
                             && (theinputGenreID == "0" || gm._genreId == Int32.Parse(theinputGenreID))
                             && (m._releaseDate.Value.Year == Int32.Parse(theinputYear) || theinputYear == "0")
                             && (theinputSearchtype == "Person" || theinputSearchtype == "")
                             select new
                             {
                                 movieid = m.movieId,
                                 movietitel = m._title,
                                 movieposter = m._posterUrl,
                             }
                             ).ToList().Distinct(); // til liste og fjerner samtidig duplikater.

                foreach (var item in query)
                {
                    Movie tempmovie = new Movie();
                    tempmovie._title = item.movietitel;
                    tempmovie._posterUrl = item.movieposter;
                    tempmovie.movieId = item.movieid;
                    templiste.Add(tempmovie);
                }
            }

            return templiste;
        }
    }
}
