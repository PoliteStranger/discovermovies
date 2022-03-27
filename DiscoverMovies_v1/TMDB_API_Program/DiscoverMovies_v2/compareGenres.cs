using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Generator;
using API_DiscoverAlgorithm;

namespace DiscoverMovies_v2
{
    public class compareGenres
    {
        // Output en liste
        // Adgang til listen over oprindelige film
        // Adgang til movieTitleShortList

        //Constructor
        compareGenres(List<MovieTitles> movieList)
        {
           

        }

        private List<MovieTitles> movieList = new List<MovieTitles>();

        public List<MovieTitles> SortMethod(List<MovieTitles> movieList)
        {
            List<Movie> SortedList = new List<Movie>();
            List<GenreCounter> genreCounterList = new List<GenreCounter>();


            foreach (var movie in movieList)
            {
                GenreCompareList genreList = new GenreCompareList();
                // Henter overordnede film detaljer fra TMDB
                Task<string> apiResult = ApiOps.RunApiMovieId(movie._id);
                // Læg resultatet over i en string (json)
                string jsonMovie = apiResult.Result;

                genreList.ExtractFromJson(jsonMovie);

                foreach (var genre in genreList.genresCompare)
                {
                    if (!genreCounterList.Contains(new GenreCounter(genre.GenreId))) //Skal eftercheckes
                    {
                        genreCounterList.Add(new GenreCounter(genre.GenreId));
                        genreCounterList.Find(g => g.GetGenreId() == genre.GenreId).AddCount();//Mulig exception men burde ikke være der da der er tjekket ovenfor
                    }
                    else
                    {
                        genreCounterList.Find(g => g.GetGenreId() == genre.GenreId).AddCount(); //Mulig exception men burde ikke være der da der er tjekket ovenfor
                    }
                }
            }

            //ikke nødvendig?
            genreCounterList.Sort(delegate (GenreCounter x, GenreCounter y) //sorterer satanen
            {
                return x.GetCount().CompareTo(y.GetCount());
            });

            List<MovieGenreScore> genreScores = new List<MovieGenreScore>();

            foreach (var movie in movieList)
            {
                genreScores.Add(new MovieGenreScore(movie._id));
            }

            foreach (var movie in genreScores)
            {
                GenreCompareList genreList = new GenreCompareList();
                // Henter overordnede film detaljer fra TMDB
                Task<string> apiResult = ApiOps.RunApiMovieId(movie.MovieId);
                // Læg resultatet over i en string (json)
                string jsonMovie = apiResult.Result;

                genreList.ExtractFromJson(jsonMovie);

                foreach (var genre in genreList.genresCompare)
                {
                    movie.AddToScore(genreCounterList.Find(x => x.GenreId.Equals(genre.GenreId)).GetCount());
                }

                
            }

            genreScores.Sort(delegate (MovieGenreScore x, MovieGenreScore y) // denne sortering er vist ikke nødvendig
            {
                return x.GetScore().CompareTo(y.GetScore());
            });

            movieList.Sort(delegate (MovieTitles x, MovieTitles y)
            {
                return genreScores.Find(g => g.MovieId.Equals(x._id)).GetScore().CompareTo(genreScores.Find(g => g.MovieId.Equals(y._id)).GetScore());
            });




            return movieList;
        }

        // laver en liste over samtlige genre fra alle 5 film

        // tælle duplikaterne: Fks: Action x5, drama x2 ..... => Action, drama, thriller

        // Sortere movieShortList efter disse genre, popularity 

        // outputter vi den nye liste:

        
    }

    public class GenreCounter
    {
        public int GenreId;
        private int _count;

        public GenreCounter(int id)
        {
            GenreId = id;
            _count = 0;
        }

        public void AddCount()
        {
            _count++;
        }

        public int GetCount()
        {
            return _count;
        }

        //public int GetGenreId()
        //{
        //    return GenreId;
        //}
    }

    public class MovieGenreScore
    {
        public int MovieId;
        private int _score;

        public MovieGenreScore(int movieId)
        {
            MovieId = movieId;
            _score = 0;
        }

        public int GetScore()
        {
            return _score;
        }

        public void AddToScore(int val)
        {
            _score += val;
        }

    }

}
