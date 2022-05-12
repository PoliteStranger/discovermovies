namespace ASP_Web_Bootstrap.Search.SearchResults
{
    public class NullSearchoption : ISearch
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
                var query = (from m in db.Movies
                             join gm in db.GenresAndMovies
                             on m.movieId equals gm._movieId
                             join g in db.Genres
                             on gm._genreId equals g._genreId
                             where (genreidattribute == "0" || gm._genreId == Int32.Parse(genreidattribute))
                              && (yearattribute == "0" || m._releaseDate.Value.Year == Int32.Parse(yearattribute))

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
                    tempmovie.movieId = item.movieid;
                    tempmovie._posterUrl = item.movieposter;
                    tempmovie._title = item.movietitel;
                    templiste.Add(tempmovie);
                }

                Console.WriteLine("Search Results: " + query.Count());
            }
            return templiste;
        }

    }
}
