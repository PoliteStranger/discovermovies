using NUnit.Framework;
using System.Diagnostics;

namespace DiscoverMoviesProduction.NUnit
{
   



    public class TDbModel
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Genres()
        {
            // ARRANGE, ACT
            // Opret et Genres objekt
            Genres uut = new Genres() { _genreId = 1, _Genrename = "Ny Genre"};

            // ASSERT
            Assert.That(uut._Genrename, Is.Not.Null);
            Assert.That(uut._genreId, Is.Not.Null);
        }


        [Test]
        public void ProducedBy()
        {
            // ARRANGE, ACT
            // Opret et Genres objekt
            ProducedBy uut = new ProducedBy() { prodCompanyId = 1, _movieId = 1};


            // ASSERT
            Assert.That(uut.prodCompanyId, Is.Not.Null);
            Assert.That(uut._movieId, Is.Not.Null);
        }
    }
}