using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class Discover
    {

        // STUBS til at teste input
        List<Movie> inputMovies;

        List<Movie> Shortlist;

        [SetUp]
        public void Setup()
        {

            // Fylder listerne med relevant data
            inputMovies = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMovies");

            Shortlist = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMoviesReturn");

        }

        [Test]
        public void TestDiscoverInputMovies()
        {
            // ARRANGE, ACT
            Discover uut = new Discover();

            // ASSERT
            Assert.That(uut.inputMovies, Is.Null);

        }

        [Test]
        public void TestDiscoverShortList()
        {
            // ARRANGE, ACT
            Discover uut = new Discover();

            // ASSERT
            Assert.That(uut.Shortlist, Is.Null);

        }


    }
}
