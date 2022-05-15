using System.Collections.Generic;
using System.Linq;
using DiscoverMoviesProduction;
using NSubstitute;
using NUnit.Framework;
using DiscoverMoviesProduction.Search.Init;
using DiscoverMoviesProduction.Search.SearchResults;
using DiscoverMoviesProduction.Search;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class MovieSearchoptionTests
    {
        private MovieSearchoption uut;


        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        [TestCase("", "", "", "Movie", "MovieSearch")]
        [TestCase("", "", "", "Person", "PersonSearch")]
        [TestCase("", "", "", "0", "NullSearch")]

        public void TestSearchInputsLogic(string Name, string GenreID, string Year, string Searchtype, string resultString)
        {
            // ARRANGE
            ResolveSearch uut = new ResolveSearch();

            var MockMovieSearch = new Mock<ISearch>();
            var MockPersonSearch = new Mock<ISearch>();
            var MockNullSearch = new Mock<ISearch>();

            // Kun til Movie Search
            MockMovieSearch.Setup(x => x.SearchInput()).Returns(new List<Movie>()
            {
                new Movie()
                {
                    movieId = 1,
                    _title = "MovieSearch",
                }
            });

            // Kun til Person Search
            MockPersonSearch.Setup(x => x.SearchInput()).Returns(new List<Movie>()
            {
                new Movie()
                {
                    movieId = 1,
                    _title = "PersonSearch",
                }
            });

            // Kun til null Search
            MockNullSearch.Setup(x => x.SearchInput()).Returns(new List<Movie>()
            {
                new Movie()
                {
                    movieId = 1,
                    _title = "NullSearch",
                }
            });

            List<Movie> returnMovieList = new List<Movie>();


            // ACT
            returnMovieList = uut.Resolve(Name, GenreID, Year, Searchtype, MockMovieSearch.Object, MockPersonSearch.Object, MockNullSearch.Object);

            // ASSERT
            Assert.That(returnMovieList.ToArray()[0]._title, Is.EqualTo(resultString));
        }

        


        //[Test]
        //public void TestCountWithAllSearchParameters()
        //{
        //    string theinputName = "the";
        //    string theinputGenreID = "12";//adventure
        //    string theinputYear = "2001";
        //    string theinputSearchtype = "Movie";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //    Assert.That(movieTESTliste.Count, Is.EqualTo(36));
        //}

        //    [Test]
        //    public void TestWithAllSearchParametersContainsSpecificMovie()
        //    {
        //        //searching for a movie
        //        string theinputName = "the";
        //        string theinputGenreID = "12";//adventure
        //        string theinputYear = "2001";
        //        string theinputSearchtype = "Movie";

        //        List<Movie> movieTESTliste = new List<Movie>();
        //        movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //        //testing with movie "Planet of the Apes" == movieID 869
        //        Assert.That(movieTESTliste.Any(i => i.movieId == 869), Is.True);
        //    }

        //    [Test]
        //    public void TestCountWithoutYearParameter()
        //    {
        //        string theinputName = "the";
        //        string theinputGenreID = "14";//fantasy
        //        string theinputYear = "0";
        //        string theinputSearchtype = "Movie";

        //        List<Movie> movieTESTliste = new List<Movie>();
        //        movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
        //        Assert.That(movieTESTliste.Count, Is.EqualTo(477));
        //    }

        //    [Test]
        //    public void TestWithoutYearParameterContainsSpecificMovie()
        //    {
        //        //searching for a movie
        //        string theinputName = "the";
        //        string theinputGenreID = "12";//adventure
        //        string theinputYear = "0";
        //        string theinputSearchtype = "Movie";

        //        List<Movie> movieTESTliste = new List<Movie>();
        //        movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //        //testing with movie "The Last Legion" == movieID 9703
        //        Assert.That(movieTESTliste.Any(i => i.movieId == 9703), Is.True);
        //    }

        //    [Test]
        //    public void TestCountWithoutYearAndGenreIDParameters()
        //    {
        //        string theinputName = "the";
        //        string theinputGenreID = "0";
        //        string theinputYear = "0";
        //        string theinputSearchtype = "Movie";

        //        List<Movie> movieTESTliste = new List<Movie>();
        //        movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
        //        Assert.That(movieTESTliste.Count, Is.EqualTo(6919));
        //    }

        //    [Test]
        //    public void TestWithoutYearAndGenreIDParametersContainsSpecificMovie()
        //    {
        //        //searching for a movie
        //        string theinputName = "the";
        //        string theinputGenreID = "0";
        //        string theinputYear = "0";
        //        string theinputSearchtype = "Movie";

        //        List<Movie> movieTESTliste = new List<Movie>();
        //        movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //        //testing with same movie "The Last Legion" == movieID 9703
        //        Assert.That(movieTESTliste.Any(i => i.movieId == 9703), Is.True);
        //    }

        //    [Test]
        //    public void TestCountWithoutNameAndSearchTypeParameters()
        //    {
        //        string theinputName = "";
        //        string theinputGenreID = "16"; //animation
        //        string theinputYear = "2000";
        //        string theinputSearchtype = "";

        //        List<Movie> movieTESTliste = new List<Movie>();
        //        movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
        //        Assert.That(movieTESTliste.Count, Is.EqualTo(133));
        //    }

        //    [Test]
        //    public void TestWithoutNameAndSearchTypeParametersContainsSpecificMovie()
        //    {
        //        //searching for a movie
        //        string theinputName = "";
        //        string theinputGenreID = "16"; //animation
        //        string theinputYear = "2000";
        //        string theinputSearchtype = "";

        //        List<Movie> movieTESTliste = new List<Movie>();
        //        movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //        //testing with same movie "Lupin the Third: Missed by a Dollar" == movieID 31053
        //        Assert.That(movieTESTliste.Any(i => i.movieId == 31053), Is.True);
        //    }
        //}
    }
}