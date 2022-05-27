using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverFilters
    {
        // Til TestCastFilterInput()
        List<Movie> NullListe;

        List<Movie> inputMovies;

        List<Movie> Shortlist;


        [SetUp]
        public void Setup()
        {
            

            inputMovies = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMovies");

            Shortlist = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMoviesReturn");

        }


        // TEST FOR INPUT, er listerne Null?
        [Test]
        public void TestMAinFilterInput()
        {
            // ARRANGE
            Filters uut;

            // ACT, ASSERT - Vi send to null lister ind, nu skal den brokke sig!
            Assert.Throws<ArgumentNullException>(() => uut = new Filters(NullListe, NullListe));
        }

        [Test]
        public void TestFiltersDoTheyExist()
        {

            // ARRANGE, ACT - Vi kalder Filters 
            Filters uut = new Filters(inputMovies, Shortlist);

            // ASSERT - Vi tjekker om Filtrene eksistere
            Assert.That(uut.DiscoverFilters.Count, Is.EqualTo(5));

        }

        






    }
}
