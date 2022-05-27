using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DiscoverMoviesProduction.NUnit
{
    public class Discover
    {

        // STUBS til at teste input
        List<Movie> inputMovies;

        List<Movie> Shortlist;

        List<Person> persons;

        [SetUp]
        public void Setup()
        {
            // Fylder listerne med relevant data
            inputMovies = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMovies");

            Shortlist = DiscoverFilterData.GetJsonMovies("../../../JsonStubs/5InputMoviesReturn");

            persons = DiscoverFilterData.GetJsonPeople("../../../JsonStubs/people");
        }

        [Test]
        public void TestDiscoverInputMovies()
        {
            // ARRANGE, ACT
            Discover uut = new Discover();

            // ASSERT
            Assert.That(uut.inputMovies, Is.Null);

        }

        [Test]
        public void TestDiscoverShortList()
        {
            // ARRANGE, ACT
            Discover uut = new Discover();

            // ASSERT
            Assert.That(uut.Shortlist, Is.Null);

        }

        [Test]
        public void TestDiscoverOutput()
        {
            // ARRANGE
            DiscoverMoviesProduction.Discover uut = new DiscoverMoviesProduction.Discover();
            var MockDiscoverDB = new Mock<IDiscoverDB>();
            var MockDiscoverIntsToMovies = new Mock<IDiscoverInputMovies>();

            Console.WriteLine("Person count: " + persons.Count);
            Console.WriteLine("ShortList count: " + Shortlist.Count);
            List<int> movieInts = new List<int> { 199, 1571, 1639, 1893, 2787 };

            // Mock setup m. DB stubs
            MockDiscoverDB.Setup(x => x.GetPeople(It.IsAny<List<int>>())).Returns(persons);
            MockDiscoverDB.Setup(z => z.GetMovies(It.IsAny<List<int>>())).Returns(Shortlist);

            MockDiscoverIntsToMovies.Setup(y => y.GetInputMovies(movieInts)).Returns(inputMovies);

            // ACT
            Movie newMovie = uut.DiscoverMovies(movieInts, MockDiscoverDB.Object, MockDiscoverIntsToMovies.Object);

            // ASSERT - Den skal vende tilbage med en film!
            Assert.That(newMovie, Is.Not.Null);
        }


    }
}
