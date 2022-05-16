using System;
using NUnit.Framework;
using DiscoverMoviesProduction.Search.SearchResults;

namespace DiscoverMoviesProduction.NUnit
{
    [TestFixture]
    public class PersonSearchOptionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase("Charles", "0", "2001", "Person")]
        [TestCase("Steven", "14", "0", "Person")]
        [TestCase("Bond", "0", "0", "Person")]
        [TestCase("", "12", "2001", "Person")]

        public void TestMovieSearchOptionAttributes(string Name, string GenreID, string Year, string Searchtype)
        {
            PersonSearchoption _uut = new PersonSearchoption();
            _uut.Setattributes(Name, GenreID, Year, Searchtype);
            Assert.AreEqual(_uut.Nameattribute, Name);
            Assert.AreEqual(_uut.Genreattribute, GenreID);
            Assert.AreEqual(_uut.Yearattribute, Year);
            Assert.AreEqual(_uut.Searchattribute, Searchtype);
        }

        [Test]
        [TestCase("", "16", "1999", "0")]
        [TestCase("t", "14", "2000", "0")]
        [TestCase("t", "14", "2000", "Movie")]
        public void TestMovieSearchOptionAttributesThrowsException(string Name, string GenreID, string Year, string Searchtype)
        {
            PersonSearchoption _uut = new PersonSearchoption();
            Assert.Throws<NullReferenceException>(() => _uut.Setattributes(Name, GenreID, Year, Searchtype));
        }
    }
}