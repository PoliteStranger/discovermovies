using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverFilterOperations
    {

        [SetUp]
        public void Setup()
        {



        }

        #region-NORMALZING SCORES

        // TIL TEST AF NORMALIZING
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void TestNormalizing(int idNum)
        {
            // ARRANGE

            // Scoreliste uden og med endelige resultater
            List<DiscoverScore> ScoresList = new List<DiscoverScore>();
            List<DiscoverScore> ScoresListResult = new List<DiscoverScore>();

            // originale scores
            ScoresList.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 1 });
            ScoresList.Add(new DiscoverScore() { Movie = new Movie() { movieId = 2, _title = "Movie " + 2 }, Score = 2 });
            ScoresList.Add(new DiscoverScore() { Movie = new Movie() { movieId = 3, _title = "Movie " + 3 }, Score = 3 });

            // endelige resultater
            ScoresListResult.Add(new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 0.5 });
            ScoresListResult.Add(new DiscoverScore() { Movie = new Movie() { movieId = 2, _title = "Movie " + 2 }, Score = 1 });
            ScoresListResult.Add(new DiscoverScore() { Movie = new Movie() { movieId = 3, _title = "Movie " + 3 }, Score = 1.5 });


            // ACT, Vi normalisere scores på den ikke-normaliserede
            NormalizingScores.Normalize(ScoresList);

            // ASSERT, sammenligner så værdierne i listerne:
            Assert.AreEqual(ScoresListResult.ToArray()[idNum].Score, ScoresList.ToArray()[idNum].Score);
        }

        #endregion

        #region-ADDING SCORES

        // TESTING OUTPUT, Addere den scores korrekt?
        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void TestAddingScores(int idNum)
        {
            // ARRANGE
            // TIL TEST AF ADDING SCORES
            List<List<DiscoverScore>> allScores = new List<List<DiscoverScore>>();
            List<DiscoverScore> finalScores = new List<DiscoverScore>();
            List<DiscoverScore> finalScoresResult;

            // originale scores, tilføjer 2 sæt scores
            allScores.Add(
                new List<DiscoverScore>() { 
                    new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 1 } ,
                    new DiscoverScore() { Movie = new Movie() { movieId = 2, _title = "Movie " + 2 }, Score = 2 } ,
                    new DiscoverScore() { Movie = new Movie() { movieId = 3, _title = "Movie " + 3 }, Score = 3 } ,
                });
            allScores.Add(
                new List<DiscoverScore>() {
                    new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 1 } ,
                    new DiscoverScore() { Movie = new Movie() { movieId = 2, _title = "Movie " + 2 }, Score = 2 } ,
                    new DiscoverScore() { Movie = new Movie() { movieId = 3, _title = "Movie " + 3 }, Score = 3 } ,
                });

            // Vi lægger de manuelt sammenlagte scores ind
            finalScoresResult = new List<DiscoverScore>() {
                    new DiscoverScore() { Movie = new Movie() { movieId = 1, _title = "Movie " + 1 }, Score = 2 } ,
                    new DiscoverScore() { Movie = new Movie() { movieId = 2, _title = "Movie " + 2 }, Score = 4 } ,
                    new DiscoverScore() { Movie = new Movie() { movieId = 3, _title = "Movie " + 3 }, Score = 6 } ,
                };

            // ACT
            // Vi tæller alle scores op fra allScores, og lægger resultatet ind i finalScores
            AddingScores.AddScores(allScores, finalScores);

            // ASSERT
            // og sammenligner så listerne.
            Assert.AreEqual(finalScoresResult.ToArray()[idNum].Score, finalScores.ToArray()[idNum].Score);
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
