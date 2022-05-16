using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace DiscoverMoviesProduction.NUnit
{
   



    public class Utilities
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestTimerStart()
        {
            // ARRANGE
            // Opret et Genres objekt
            LoadTimer uut = new LoadTimer();

            // Skaber en StringWriter, så vi kan "optage" konsollet
            var stringWriterB = new StringWriter();
            // Vi "optager" konsollet
            Console.SetOut(stringWriterB);


            // ACT
            uut.StartTimer();

            // Vi laver konsol output til et array, så vi kan tælle linjer.
            var outputLines = stringWriterB.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT
            Assert.That(outputLines[0], Contains.Substring("Starting time now: "));

        }

        [Test]
        public void TestTimerStop()
        {
            // ARRANGE
            // Opret et Genres objekt
            LoadTimer uut = new LoadTimer();

            // Skaber en StringWriter, så vi kan "optage" konsollet
            var stringWriterB = new StringWriter();
            // Vi "optager" konsollet
            Console.SetOut(stringWriterB);


            // ACT
            uut.StartTimer();
            uut.StopTimer();

            // Vi laver konsol output til et array, så vi kan tælle linjer.
            var outputLines = stringWriterB.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT
            Assert.That(outputLines[1], Contains.Substring("Time taken: "));

        }

        [Test]
        public void TestTimerWrongOrder()
        {
            // ARRANGE
            // Opret et Genres objekt
            LoadTimer uut = new LoadTimer();

            // Skaber en StringWriter, så vi kan "optage" konsollet
            var stringWriterB = new StringWriter();
            // Vi "optager" konsollet
            Console.SetOut(stringWriterB);


            // ACT
            uut.StopTimer();

            // Vi laver konsol output til et array, så vi kan tælle linjer.
            var outputLines = stringWriterB.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            // ASSERT
            Assert.That(outputLines[0], Contains.Substring("Must start timer, before stopping it!"));

        }
    }
}