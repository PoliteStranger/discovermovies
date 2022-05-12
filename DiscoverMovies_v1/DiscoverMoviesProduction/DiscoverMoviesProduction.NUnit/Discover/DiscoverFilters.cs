using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverFilters
    {

        List<Movie> inputMovies;

        List<Movie> Shortlist;


        [SetUp]
        public void Setup()
        {
            

            inputMovies = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMovies");

            Shortlist = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMoviesReturn");

        }


        [Test]
        public void TestFiltersDoTheyExist()
        {
            // ACT - Vi kalder Filters 
            Filters uut = new Filters(inputMovies, Shortlist);

            // ASSERT - Vi tjekker om Filtrene eksistere
            Assert.That(uut.DiscoverFilters.Count, Is.EqualTo(5));

        }






    }
}
