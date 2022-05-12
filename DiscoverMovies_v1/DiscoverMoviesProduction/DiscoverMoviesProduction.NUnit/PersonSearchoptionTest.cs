using System;
using System.Collections.Generic;
using System.Linq;
using ASP_Web_Bootstrap;
using NSubstitute;
using NUnit.Framework;
using ASP_Web_Bootstrap.Search.Init;
using ASP_Web_Bootstrap.Search.SearchResults;

namespace DiscoverMoviesProduction.NUnit
{
    public class PersonSearchoptionTests
    {
        //private PersonSearchoption uut;

        //private ISearch search;
        
        //[SetUp]
        //public void Setup()
        //{
        //    uut = new PersonSearchoption();
        //}
        
        //[Test]
        //public void PersonTest_Count_With_All_Search_Parameters()
        //{
        //    string theinputName = "charles";
        //    string theinputGenreID = "35";//Comedy
        //    string theinputYear = "2000";
        //    string theinputSearchtype = "Person";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
        //    Assert.That(movieTESTliste.Count, Is.EqualTo(23));
        //}

        //[Test]
        //public void PersonTest_With_All_Search_Parameters_Contains_Specific_Movie()
        //{
        //    //searching for a movie
        //    string theinputName = "charles";
        //    string theinputGenreID = "36";//History
        //    string theinputYear = "2000";
        //    string theinputSearchtype = "Person";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //    //testing with movie "The Patriot" == movieID 2024
        //    Assert.That(movieTESTliste.Any(i => i.movieId == 2024), Is.True);
        //}

        //[Test]
        //public void PersonTest_Count_Without_Year_Parameter()
        //{
        //    string theinputName = "steven";
        //    string theinputGenreID = "80";//Crime
        //    string theinputYear = "0";
        //    string theinputSearchtype = "Person";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
        //    Assert.That(movieTESTliste.Count, Is.EqualTo(147));
        //}

        //[Test]
        //public void PersonTest_Without_Year_Parameter_Contains_Specific_Movie()
        //{
        //    //searching for a movie
        //    string theinputName = "steven";
        //    string theinputGenreID = "37";//Western
        //    string theinputYear = "0";
        //    string theinputSearchtype = "Person";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //    //testing with movie "Last Stand at Saber River" == movieID 55576
        //    Assert.That(movieTESTliste.Any(i => i.movieId == 55576), Is.True);
        //}

        //[Test]
        //public void PersonTest_Count_Without_Year_And_GenreID_Parameter()
        //{
        //    string theinputName = "robert downey";
        //    string theinputGenreID = "0";
        //    string theinputYear = "0";
        //    string theinputSearchtype = "Person";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);
        //    Assert.That(movieTESTliste.Count, Is.EqualTo(32));
        //}

        //[Test]
        //public void PersonTest_Without_Year_And_GenreID_Parameter_Contains_Specific_Movie()
        //{
        //    //searching for a movie
        //    string theinputName = "downey";
        //    string theinputGenreID = "0";
        //    string theinputYear = "0";
        //    string theinputSearchtype = "Person";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //    //testing with movie "Bowfinger" == movieID 11353
        //    Assert.That(movieTESTliste.Any(i => i.movieId == 11353), Is.True);
        //}

        //[Test]
        //public void PersonTest_Does_Person_Do_Something_In_Specific_Movie()
        //{
        //    //searching for a movie
        //    string theinputName = "downey";
        //    string theinputGenreID = "0";
        //    string theinputYear = "0";
        //    string theinputSearchtype = "Person";

        //    List<Movie> movieTESTliste = new List<Movie>();
        //    movieTESTliste = uut.SearchInput(theinputName, theinputGenreID, theinputYear, theinputSearchtype);

        //    //testing with movie "Bowfinger" == movieID 11353
        //    Movie film = movieTESTliste.Find(i => i.movieId == 11353);

            

        //    List<Person> personliste = new List<Person>();

        //    //using (var db = new MyDbContext())
        //    //{
        //    //    var query = (from p in db.Persons
        //    //        join e in db.Employments
        //    //        on p._personId equals e._personId
                        
        //    //        where p._Personname.Contains(theinputName);


        //    //}

        //    Assert.That(film._employmentList.Count, Is.GreaterThan(1));


        //    //Assert.That(film._employmentList.Any(i => i.Person._Personname.Contains("downey")), Is.True);
        //}
        
    }
}