using System;
using System.Collections.Generic;
using System.Linq;
using ASP_Web_Bootstrap;
using NSubstitute;
using NUnit.Framework;
using ASP_Web_Bootstrap.Search.Init;
using DiscoverMoviesProduction;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverFilterOperations
    {

        // Original score liste
        public List<DiscoverScore> ScoresList = new List<DiscoverScore>();
        public List<DiscoverScore> ScoresListResult = new List<DiscoverScore>();

        // TIL TEST AF ADDING SCORES
        public List<List<DiscoverScore>> allScores = new List<List<DiscoverScore>>();
        public List<DiscoverScore> finalScores = new List<DiscoverScore>();
        public List<DiscoverScore> finalScoresResult = new List<DiscoverScore>();


        [SetUp]
        public void Setup()
        {
            // TIL TEST AF NORMALIZING

            // Lav STUBs til Normaliseringsfunktionen
            for (int i = 1; i > 11; i++)
            {
                // Vi laver en liste med IKKE-Normaliserede scores
                ScoresList.Add(new DiscoverScore()
                {
                    Movie = new Movie()
                    {
                        movieId = i,
                        _title = "Movie " + i
                    },
                    Score = i
                });

                // Vi laver en tilsvarende liste MED normaliserede scores
                ScoresListResult.Add(new DiscoverScore()
                {
                    Movie = new Movie()
                    {
                        movieId = i,
                        _title = "Movie " + i
                    },
                    Score = 10/i
                });
            }

            // Til test af Adding Scores

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
                        Score = j*10
                    });
                }

                allScores.Add(newScore);

            }
            
        }

        [Test]
        public void TestNormalizing()
        {
            // Vi normalisere scores på den ikke-normaliserede
            NormalizingScores.Normalize(ScoresList);

            // og sammenligner så de normaliserede lister
            Assert.AreEqual(ScoresList, ScoresListResult);
        }

        [Test]
        public void TestAddingScores()
        {
            // Vi tæller alle scores op fra allScores, og lægger resultatet ind i finalScores
            AddingScores.AddScores(allScores, finalScores);
            
            // og sammenligner så listerne med en korrekt Stub.
            Assert.AreEqual(finalScores, finalScoresResult);
        }




    }
}
