using System.Collections.Generic;
using NUnit.Framework;
using DiscoverMoviesProduction.Search.SearchResults;
using DiscoverMoviesProduction.Search;
using Moq;

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
        
    }
}