using System.Collections;

namespace ASP_Web_Bootstrap.Search.SearchResults
{
    public class MovieSearchoption : ISearch
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
            Console.WriteLine(nameattribute);
            Console.WriteLine(genreidattribute);
            Console.WriteLine(yearattribute);
            Console.WriteLine(searchattribute);

            using (var db = new MyDbContext())
            {
                var query = (from m in db.Movies
                             join gm in db.GenresAndMovies
                             on m.movieId equals gm._movieId
                             join g in db.Genres
                             on gm._genreId equals g._genreId
                             where (m._title.Contains(nameattribute) || nameattribute == "")
                             && (genreidattribute == "0" || gm._genreId == Int32.Parse(genreidattribute))
                             && (m._releaseDate.Value.Year == Int32.Parse(yearattribute) || yearattribute == "0")
                             && (searchattribute == "Movie" || searchattribute == "")
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
                    tempmovie.movieId = item.movieid;
                    tempmovie._posterUrl = item.movieposter;
                    tempmovie._title = item.movietitel;
                    templiste.Add(tempmovie);
                }
            }
            return templiste;

        }

    }
}
