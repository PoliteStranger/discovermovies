using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverGenreFilter
    {

        // Til TestCastFilterInput()
        List<Movie> NullListe;

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



        // TEST FOR INPUT, er listerne Null?
        [Test]
        public void TestGenreFilterInput()
        {
            // ARRANGE
            GenreFilter uut;

            // ACT, ASSERT - Vi send to null lister ind, nu skal den brokke sig!
            Assert.Throws<ArgumentNullException>(() => uut = new GenreFilter(NullListe, NullListe));
        }

        // TEST FOR OUTPUT, har filteret lavet noget?
        [Test]
        public void TestGenreFilterOutputPresent()
        {
            // ARRANGE, ACT
            GenreFilter uut = new GenreFilter(inputMovies, Shortlist);

            // ASSERT, Vi ved at inputlisterne(STUBS) SKAL producere resultater, så DiscoverScore listen SKAL være større end 0
            Assert.That(uut.discoverScores.Count, Is.GreaterThan(0));
        }

        // TEST FOR OUTPUT, har filmene fået scores?
        [Test]
        public void TestCastFilterOutputScores()
        {
            // ARRANGE, ACT
            GenreFilter uut = new GenreFilter(inputMovies, Shortlist);

            // ASSERT, Vi ved at inputlisterne(STUBS) SKAL producere resultater, så DiscoverScore listen SKAL kun have scores over nul!
            Assert.That(uut.discoverScores.Find(x => x.Score == 0), Is.Null);
        }

    }
}
