using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

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

            // Til Console Print



        }

        #region-NORMALZING SCORES

        [Test]
        public void TestNormalizing()
        {
            // Vi normalisere scores på den ikke-normaliserede
            NormalizingScores.Normalize(ScoresList);

            // og sammenligner så de normaliserede lister
            Assert.AreEqual(ScoresList, ScoresListResult);
        }

        #endregion

        #region-ADDING SCORES

        // TESTING OUTPUT, Addere den scores korrekt?
        [Test]
        public void TestAddingScores()
        {
            // ARRANGE, ACT
            // Vi tæller alle scores op fra allScores, og lægger resultatet ind i finalScores
            AddingScores.AddScores(allScores, finalScores);
            
            // ASSERT
            // og sammenligner så listerne med en korrekt Stub.
            Assert.AreEqual(finalScores, finalScoresResult);
        }



        #endregion

        #region-PRINTING TOP TEN

        // TESTING OUTPUT, når input kun er 10, er output også kun 10?
        [Test]
        public void TestPrintingTopTenExact()
        {
            // ARRANGE - Vi sætter 10 elementer ind, der skal KUN printes 10 til konsol!
            List<DiscoverScore> ScoresForConsolePrint = new List<DiscoverScore>();
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 1 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 2 }, Score = 2 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 3 }, Score = 3 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 4 }, Score = 4 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 5 }, Score = 5 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 6 }, Score = 6 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 7 }, Score = 7 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 8 }, Score = 8 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 9 }, Score = 9 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 10 }, Score = 10 });

            // Skriver lige count fra listen, at den faktisk HAR 11 ting
            TestContext.Out.WriteLine("List item count: " + ScoresForConsolePrint.Count);


            // Skaber en StringWriter, så vi kan "optage" konsollet
            var stringWriterB = new StringWriter();
            // Vi "optager" konsollet
            Console.SetOut(stringWriterB);

            // ACT
            PrintScores.PrintTopTen(ScoresForConsolePrint);

            // Vi laver konsol output til et array, så vi kan tælle linjer.
            var outputLines = stringWriterB.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT
            Assert.That(outputLines.Length, Is.EqualTo(10));
        }

        // TESTING OUTPUT, når input kun er 2, er output også kun 2? Burde slås sammen med testen ovenover!
        [Test]
        public void TestPrintingOnlyTwo()
        {

            // ARRANGE - Vi sætter 2 elementer ind, der skal KUN printes op til 10, men kan også printe mindre...
            List<DiscoverScore> ScoresForConsolePrint = new List<DiscoverScore>();
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 1 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 2 }, Score = 2 });

            // Skriver lige count fra listen, at den faktisk HAR 11 ting
            TestContext.Out.WriteLine("List item count: " + ScoresForConsolePrint.Count);


            // Skaber en StringWriter, så vi kan "optage" konsollet
            var stringWriterB = new StringWriter();
            // Vi "optager" konsollet
            Console.SetOut(stringWriterB);

            // ACT
            PrintScores.PrintTopTen(ScoresForConsolePrint);

            // Vi laver konsol output til et array, så vi kan tælle linjer.
            var outputLines = stringWriterB.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT
            Assert.That(outputLines.Length, Is.EqualTo(2));


        }

        // TESTING OUTPUT, trods mere end 10 ting på listen, viser den KUN 10 ting i print?
        [Test]
        public void TestPrintingOnlyTen()
        {

            // ARRANGE - Vi sætter 11 elementer ind, der skal KUN printes 10 til konsol!
            List<DiscoverScore> ScoresForConsolePrint = new List<DiscoverScore>();
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 1 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 2 }, Score = 2 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 3 }, Score = 3 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 4 }, Score = 4 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 5 }, Score = 5 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 6 }, Score = 6 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 7 }, Score = 7 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 8 }, Score = 8 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 9 }, Score = 9 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 10 }, Score = 10 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 11 }, Score = 11 });

            // Skriver lige count fra listen, at den faktisk HAR 11 ting
            TestContext.Out.WriteLine("List item count: " + ScoresForConsolePrint.Count);


            // Skaber en StringWriter, så vi kan "optage" konsollet
            var stringWriterB = new StringWriter();
            // Vi "optager" konsollet
            Console.SetOut(stringWriterB);

            // ACT
            PrintScores.PrintTopTen(ScoresForConsolePrint);

            // Vi laver konsol output til et array, så vi kan tælle linjer.
            var outputLines = stringWriterB.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT
            Assert.That(outputLines.Length, Is.EqualTo(10));


        }

        // TESTING OUTPUT, ændre den rækkefølgen korrekt?
        [Test]
        public void TestPrintingCheckOrder()
        {

            // ARRANGE - Vi sætter 10 elementer ind, De har scores fra 1-10, så funktionen burde sætte dem i omvendt rækkefølge!
            List<DiscoverScore> ScoresForConsolePrint = new List<DiscoverScore>();
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 1 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 2 }, Score = 2 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 3 }, Score = 3 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 4 }, Score = 4 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 5 }, Score = 5 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 6 }, Score = 6 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 7 }, Score = 7 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 8 }, Score = 8 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 9 }, Score = 9 });
            ScoresForConsolePrint.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 10 }, Score = 10 });


            // Skaber en StringWriter, så vi kan "optage" konsollet
            var stringWriterB = new StringWriter();
            // Vi "optager" konsollet
            Console.SetOut(stringWriterB);

            // ACT
            PrintScores.PrintTopTen(ScoresForConsolePrint);

            // Vi laver konsol output til et array, så vi kan tælle linjer.
            var outputLines = stringWriterB.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT - At den med højste score(10) nu er øverst!
            Assert.AreEqual("Movie 10 - 10", outputLines[0].ToString());


        }

        #endregion


    }
}
