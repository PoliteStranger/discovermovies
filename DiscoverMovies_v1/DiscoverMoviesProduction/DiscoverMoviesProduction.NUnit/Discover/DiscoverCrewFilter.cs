using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverCrewFilter
    {

        // Test af input:
        List<Movie> NullListe;

        // GetCrew 
        List<Employment> inputEmployments = new List<Employment>();

        // Filmliste med 2x Film, med ialt 2 skuespillere og 4 Instruktøre/Producere
        List<Movie> inputMoviesSmallStub = new List<Movie>()
            {
                new Movie()
                {
                    movieId = 1,
                    _title = "Movie 1",
                    _employmentList = new List<Employment>()
                    {
                        new Employment()
                        {
                            employmentId = 1,
                            _job = "Acting",
                        },
                        new Employment()
                        {
                            employmentId = 2,
                            _job = "Director",
                        },
                        new Employment()
                        {
                            employmentId = 3,
                            _job = "Producer",
                        },
                    }
                },
                                new Movie()
                {
                    movieId = 2,
                    _title = "Movie 2",
                    _employmentList = new List<Employment>()
                    {
                        new Employment()
                        {
                            employmentId = 4,
                            _job = "Acting",
                        },
                        new Employment()
                        {
                            employmentId = 5,
                            _job = "Director",
                        },
                        new Employment()
                        {
                            employmentId = 6,
                            _job = "Producer",
                        },
                    }
                },
            };

        List<Employment> EmploymentsResults = new List<Employment>()
            {
                new Employment()
                {
                    employmentId = 1,
                    _job = "Acting",
                },
                new Employment()
                {
                    employmentId = 4,
                    _job = "Acting",
                },
            };

        // STUBS til at teste in
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
        public void TestCrewFilterInput()
        {
            // ARRANGE
            CrewFilter uut;

            // ACT, ASSERT - Vi send to null lister ind, nu skal den brokke sig!
            Assert.Throws<ArgumentNullException>(() => uut = new CrewFilter(NullListe, NullListe));
        }

        // TEST FOR OUTPUT, har filteret lavet noget?
        [Test]
        public void TestCastFilterOutputPresent()
        {
            // ARRANGE, ACT
            CrewFilter uut = new CrewFilter(inputMovies, Shortlist);

            // ASSERT, Vi ved at inputlisterne(STUBS) SKAL producere resultater, så DiscoverScore listen SKAL være større end 0
            Assert.That(uut.discoverScores.Count, Is.GreaterThan(0));
        }

        // TEST FOR OUTPUT, har filmene fået scores?
        [Test]
        public void TestCastFilterOutputScores()
        {
            // ARRANGE, ACT
            CrewFilter uut = new CrewFilter(inputMovies, Shortlist);

            // ASSERT, Vi ved at inputlisterne(STUBS) SKAL producere resultater, så DiscoverScore listen SKAL kun have scores over nul!
            Assert.That(uut.discoverScores.Find(x => x.Score == 0), Is.Null);
        }

        // TEST FOR OUTPUT, finder den det rigtige antal employments?
        [Test]
        public void TestGetCrew()
        {

            GetCrew uut = new GetCrew(inputMoviesSmallStub, inputEmployments);
            Assert.That(inputEmployments.Count, Is.EqualTo(4));

        }


    }
}
