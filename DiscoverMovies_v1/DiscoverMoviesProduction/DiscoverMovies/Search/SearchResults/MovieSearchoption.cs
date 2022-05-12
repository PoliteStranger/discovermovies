namespace ASP_Web_Bootstrap.Search.SearchResults
{
    public class MovieSearchoption : ISearch
    {
        private List<Searchclass> templiste = new List<Searchclass>();

        public List<Searchclass> SearchInput(string theinputName, string theinputGenreID, string theinputYear, string theinputSearchtype)
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

                foreach (var searchitem in query)
                {
                    Searchclass tempmovie = new Searchclass();
                    tempmovie.movieid = searchitem.movieid;
                    tempmovie.movieposter = searchitem.movieposter;
                    tempmovie.movietitel = searchitem.movietitel; 
                    templiste.Add(tempmovie);
                }

                Console.WriteLine("Search Results: " + query.Count());
            }
            return templiste;

        }

    }
}
