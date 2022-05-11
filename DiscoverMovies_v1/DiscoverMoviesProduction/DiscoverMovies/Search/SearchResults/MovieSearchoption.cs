namespace ASP_Web_Bootstrap.Search.SearchResults
{
    public class MovieSearchoption : ISearch
    {
        private List<Movie> templiste = new List<Movie>();

        public List<Movie> SearchInput(string theinputName, string theinputGenreID, string theinputYear, string theinputSearchtype)
        {
            Console.WriteLine(theinputName);
            Console.WriteLine(theinputGenreID);
            Console.WriteLine(theinputYear);
            Console.WriteLine(theinputSearchtype);

            using (var db = new MyDbContext())
            {
                var query = (from m in db.Movies
                             join gm in db.GenresAndMovies
                             on m.movieId equals gm._movieId
                             join g in db.Genres
                             on gm._genreId equals g._genreId
                             where (m._title.Contains(theinputName) || theinputName == "")
                             && (theinputGenreID == "0" || gm._genreId == Int32.Parse(theinputGenreID))
                             && (m._releaseDate.Value.Year == Int32.Parse(theinputYear) || theinputYear == "0")
                             && (theinputSearchtype == "Movie" || theinputSearchtype == "")

                             select new
                             {
                                 movieid = m.movieId,
                                 movietitel = m._title,
                                 movieposter = m._posterUrl,
                             }
                             ).ToList().Distinct();

                foreach (var item in query)
                {
                    Movie tempmovie = new Movie();
                    tempmovie._title = item.movietitel;
                    tempmovie._posterUrl = item.movieposter;
                    tempmovie.movieId = item.movieid;
                    templiste.Add(tempmovie);
                }

                Console.WriteLine("Search Results: " + query.Count());
            }

            

            return templiste;
        }

    }
}
