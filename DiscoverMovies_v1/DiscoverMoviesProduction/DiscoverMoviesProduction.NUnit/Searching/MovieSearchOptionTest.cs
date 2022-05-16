using System;
using System.Collections.Generic;
using System.Linq;
using DiscoverMoviesProduction;
using NSubstitute;
using NUnit.Framework;
using DiscoverMoviesProduction.Search.Init;
using DiscoverMoviesProduction.Search.SearchResults;

namespace DiscoverMoviesProduction.NUnit
{
    [TestFixture]
    public class MovieSearchOptionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("the", "12", "2001", "Movie")]
        [TestCase("t", "14", "2000", "Movie")]
        [TestCase("Bond", "16", "1999", "Movie")]
        [TestCase("", "12", "2001", "Movie")]

        public void TestMovieSearchOptionAttributes(string Name, string GenreID, string Year, string Searchtype)
        {
            MovieSearchoption _uut = new MovieSearchoption();
            _uut.Setattributes(Name, GenreID, Year, Searchtype);
            Assert.AreEqual(_uut.Nameattribute, Name);
            Assert.AreEqual(_uut.Genreattribute, GenreID);
            Assert.AreEqual(_uut.Yearattribute, Year);
            Assert.AreEqual(_uut.Searchattribute, Searchtype);
        }

        [Test]
        [TestCase("", "16", "1999", "0")]
        [TestCase("t", "14", "2000", "0")]
        [TestCase("t", "14", "2000", "Person")]
        public void TestMovieSearchOptionAttributesThrowsException(string Name, string GenreID, string Year, string Searchtype)
        {
            MovieSearchoption _uut = new MovieSearchoption();
            Assert.Throws<NullReferenceException>(() => _uut.Setattributes(Name, GenreID, Year, Searchtype));
        }
    }
}