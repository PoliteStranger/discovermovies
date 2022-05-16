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


        //public void initSearchOption(List<string> soegningsliste)
        //{
        //    soegningsliste.Add("Movie");
        //    soegningsliste.Add("Person");
        //}

        //public void initYear(List<int> yearliste)
        //{
        //    //dropdown menu til år
        //    for (int i = 1980; i<2021; i++)
        //    {
        //        yearliste.Add(i);
        //    }
        //}

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
        //[TestCase("", "16", "1999", "0")]
        //[TestCase("t", "14", "2000", "0")]
        //[TestCase("t", "14", "2000", "Person")]
        //public void TestMovieSearchOptionAttributesThrowsException(string Name, string GenreID, string Year, string Searchtype)
        //{
        //    MovieSearchoption _uut = new MovieSearchoption();
        //    Assert.Throws<NullReferenceException>(() => _uut.Setattributes(Name, GenreID, Year, Searchtype));
        //}
    }
    }


