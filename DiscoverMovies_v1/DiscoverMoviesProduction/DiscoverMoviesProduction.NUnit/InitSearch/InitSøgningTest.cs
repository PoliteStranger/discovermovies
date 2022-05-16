using System;
using System.Collections.Generic;
using DiscoverMoviesProduction.Search.Init;
using NUnit.Framework;
using DiscoverMoviesProduction.Search.SearchResults;



namespace DiscoverMoviesProduction.NUnit
{
    [TestFixture]
        public class InitSøgningTest
        {
            private soegning søge;

            [SetUp]
            public void Setup()
            {
                søge = new soegning();
            }

            [Test]
            public void TestInitYearCount()
            {
                List<int> aarsliste = new List<int>();
                søge.initYear(aarsliste);
                Assert.That(aarsliste.Count == 41);
            }

            [Test]
            public void TestInitYeareveryYear()
            {
                List<int> aarsliste = new List<int>();
                søge.initYear(aarsliste);
                int aar = 1980;
                
                for (int i=0; i<41; i++)
                {
                    Assert.AreEqual(aar, aarsliste[i]);
                    aar++;
                }
            }

            [Test]
            public void TestInitSearchOptionCount()
            {
                List<string> soegningsliste = new List<string>();
                søge.initSearchOption(soegningsliste);
                Assert.That(soegningsliste.Count == 2);
            }

            [Test]
            public void TestInitSearchOption()
            {
                List<string> soegningsliste = new List<string>();
                søge.initSearchOption(soegningsliste);

                Assert.AreEqual(soegningsliste[0], "Movie");
                Assert.AreEqual(soegningsliste[1], "Person");
            }

            //[Test]
            //public void TestInitGenres()
            //{
            //    List<Genres> soegningsliste = new List<Genres>();

            //    soegningsliste = søge.initGenre(soegningsliste);
                
                
            //    Assert.That(soegningsliste.Count == 19);
            //}
        }
}


