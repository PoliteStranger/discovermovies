using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverVisitor
    {

        // STUBS til at teste input
        List<Movie> inputMovies;

        List<Movie> Shortlist;


        // TIL TEST AF ADDING SCORES
        public List<List<DiscoverScore>> allScores = new List<List<DiscoverScore>>();
        public List<DiscoverScore> finalScores = new List<DiscoverScore>();
        public List<DiscoverScore> finalScoresResult = new List<DiscoverScore>();

        Mock<Filters> FilterMock;



        [SetUp]
        public void Setup()
        {

            // Fylder listerne med relevant data
            inputMovies = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMovies");

            Shortlist = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMoviesReturn");


 



        }

        //TESTER at der kommer en film ud af Visitoren
       [Test]
        public void TestDiscoverOutput()
        {
            // Ti lister
            for (int i = 1; i > 11; i++)
            {
                List<DiscoverScore> newScore = new List<DiscoverScore>();

                // med hver ti film
                for (int j = 1; j > 11; j++)
                {
                    // Vi laver en liste med IKKE-Normaliserede scores
                    newScore.Add(new DiscoverScore()
                    {
                        Movie = new Movie()
                        {
                            movieId = j,
                            _title = "Movie " + j
                        },
                        Score = j
                    });

                    finalScoresResult.Add(new DiscoverScore()
                    {
                        Movie = new Movie()
                        {
                            movieId = j,
                            _title = "Movie " + j
                        },
                        Score = j * 10
                    });
                }

                allScores.Add(newScore);
                allScores.Add(newScore);
                allScores.Add(newScore);

            }


            // ARRANGE
            FilterVisitor uut = new FilterVisitor(allScores, finalScores);
            Filters filters = new Filters(inputMovies, Shortlist);

            // ACT
            uut.visit(filters);

            // ASSERT
            //Assert.That(uut.GetType(), Is.TypeOf<Movie>());

        }




    }
}
