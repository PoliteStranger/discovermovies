using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiscoverMoviesProduction.NUnit
{
    public class DiscoverCrewFilter
    {

        // GetCrew 
        List<Employment> inputEmployments = new List<Employment>();

        // Filmliste med 2x Film, med ialt 2 skuespillere og 4 Instruktøre/Producere
        List<Movie> inputMovies = new List<Movie>()
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

        [SetUp]
        public void Setup()
        {

        }


        [Test]
        public void TestGetCrew()
        {

            GetCrew uut = new GetCrew(inputMovies, inputEmployments);
            Assert.That(inputEmployments.Count, Is.EqualTo(4));

        }
    }
}
