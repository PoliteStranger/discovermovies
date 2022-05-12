using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverCastFilter
    {
        //List<Movie> inputMovies;

        List<Movie> shortList;

        List<DiscoverScore> CastFilterReturn;

        [SetUp]
        public void Setup()
        {
            //// TestCastFilterOutput
            //inputMovies = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMovies");
            //foreach(Movie movie in inputMovies)
            //{
            //    //Console.WriteLine(movie._title);
            //}

            //shortList = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMoviesReturn");
            //CastFilterReturn = DiscoverFilterData.GetJsonScores("../../../JsonStubs/CastFilterReturn");


            // TestGetCrew


            


        }


        [Test]
        public void TestCastFilterOutput()
        {

            //CastFilter uut = new CastFilter(inputMovies, shortList);
            //Console.WriteLine("{0}\n{1}", CastFilterReturn.Count, uut.discoverScores.Count);
            //Assert.AreEqual(CastFilterReturn[0].Movie.movieId, uut.discoverScores[0].Movie.movieId);

        }

    }
}
