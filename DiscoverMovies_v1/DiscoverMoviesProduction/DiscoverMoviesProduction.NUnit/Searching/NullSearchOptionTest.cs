using System;
using NUnit.Framework;
using DiscoverMoviesProduction.Search.SearchResults;

namespace DiscoverMoviesProduction.NUnit
{
    [TestFixture]
    public class NullSearchOptionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("", "14", "0", "0")]
        [TestCase("", "0", "0", "0")]
        [TestCase("", "12", "2001", "0")]
        [TestCase("", "0", "2002", "0")]
        [TestCase("Charles", "0", "2001", "0")]

        public void TestMovieSearchOptionAttributes(string Name, string GenreID, string Year, string Searchtype)
        {
            NullSearchoption _uut = new NullSearchoption();
            _uut.Setattributes(Name, GenreID, Year, Searchtype);
            Assert.AreEqual(_uut.Nameattribute, "");
            Assert.AreEqual(_uut.Genreattribute, GenreID);
            Assert.AreEqual(_uut.Yearattribute, Year);
            Assert.AreEqual(_uut.Searchattribute, Searchtype);
        }

        [Test]
        [TestCase("t", "14", "2000", "Person")]
        [TestCase("t", "14", "2000", "Movie")]
        public void TestMovieSearchOptionAttributesThrowsException(string Name, string GenreID, string Year, string Searchtype)
        {
            NullSearchoption _uut = new NullSearchoption();
            Assert.Throws<NullReferenceException>(() => _uut.Setattributes(Name, GenreID, Year, Searchtype));
        }
    }
}