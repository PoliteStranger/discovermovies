using System.Collections.Generic;
using System.Linq;
using ASP_Web_Bootstrap;
using NSubstitute;
using NUnit.Framework;
using ASP_Web_Bootstrap.Search.Init;
using ASP_Web_Bootstrap.Search.SearchResults;

namespace DiscoverMoviesProduction.NUnit
{
    public class MovieSearchoptionTests
    {
        private MovieSearchoption uut;

        private ISearch search;
        
        [SetUp]
        public void Setup()
        {
            uut = new MovieSearchoption();
        }
        
        [Test]
        public void TestCountWithAllSearchParameters()
        {
            string theinputName = "the";
            string theinputGenreID = "12";//adventure
            string theinputYear = "2001";
            string theinputSearchtype = "Movie";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
            Assert.That(movieTESTliste.Count, Is.EqualTo(36));
        }

        [Test]
        public void TestWithAllSearchParametersContainsSpecificMovie()
        {
            //searching for a movie
            string theinputName = "the";
            string theinputGenreID = "12";//adventure
            string theinputYear = "2001";
            string theinputSearchtype = "Movie";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

            //testing with movie "Planet of the Apes" == movieID 869
            Assert.That(movieTESTliste.Any(i => i.movieId == 869), Is.True);
        }

        [Test]
        public void TestCountWithoutYearParameter()
        {
            string theinputName = "the";
            string theinputGenreID = "14";//fantasy
            string theinputYear = "0";
            string theinputSearchtype = "Movie";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
            Assert.That(movieTESTliste.Count, Is.EqualTo(477));
        }

        [Test]
        public void TestWithoutYearParameterContainsSpecificMovie()
        {
            //searching for a movie
            string theinputName = "the";
            string theinputGenreID = "12";//adventure
            string theinputYear = "0";
            string theinputSearchtype = "Movie";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

            //testing with movie "The Last Legion" == movieID 9703
            Assert.That(movieTESTliste.Any(i => i.movieId == 9703), Is.True);
        }

        [Test]
        public void TestCountWithoutYearAndGenreIDParameters()
        {
            string theinputName = "the";
            string theinputGenreID = "0";
            string theinputYear = "0";
            string theinputSearchtype = "Movie";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
            Assert.That(movieTESTliste.Count, Is.EqualTo(6919));
        }

        [Test]
        public void TestWithoutYearAndGenreIDParametersContainsSpecificMovie()
        {
            //searching for a movie
            string theinputName = "the";
            string theinputGenreID = "0";
            string theinputYear = "0";
            string theinputSearchtype = "Movie";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

            //testing with same movie "The Last Legion" == movieID 9703
            Assert.That(movieTESTliste.Any(i => i.movieId == 9703), Is.True);
        }

        [Test]
        public void TestCountWithoutNameAndSearchTypeParameters()
        {
            string theinputName = "";
            string theinputGenreID = "16"; //animation
            string theinputYear = "2000";
            string theinputSearchtype = "";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
            Assert.That(movieTESTliste.Count, Is.EqualTo(133));
        }

        [Test]
        public void TestWithoutNameAndSearchTypeParametersContainsSpecificMovie()
        {
            //searching for a movie
            string theinputName = "";
            string theinputGenreID = "16"; //animation
            string theinputYear = "2000";
            string theinputSearchtype = "";

            List<Movie> movieTESTliste = new List<Movie>();
            movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

            //testing with same movie "Lupin the Third: Missed by a Dollar" == movieID 31053
            Assert.That(movieTESTliste.Any(i => i.movieId == 31053), Is.True);
        }
    }
}