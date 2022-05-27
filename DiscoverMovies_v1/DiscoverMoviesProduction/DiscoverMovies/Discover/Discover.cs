using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DiscoverMoviesProduction
{


    /// <summary>
    /// Interface til DiscoverIntsToMovies, som konvertere en liste af filmId'er til en movieliste med samtlige films data.
    /// </summary>
    public interface IDiscoverInputMovies
    {
        List<Movie> GetInputMovies(List<int> InputMovieIds);
    }

    /// <summary>
    /// Konvertere en liste af filmId'er til en movieliste med samtlige films data.
    /// </summary>
    public class DiscoverIntsToMovies : IDiscoverInputMovies
    {
        public List<Movie> GetInputMovies(List<int> InputMovieIds)
        {
            List<Movie> InputMovies = new List<Movie>();

            using (var db = new MyDbContext())
            {
                InputMovies = db.Movies.Where(c => InputMovieIds.Contains(c.movieId))
                                        .Include(x => x._genreList)
                                        .Include(y => y._prodCompanyList)
                                        .Include(z => z._employmentList)
                                        .ToList();
            }


            return InputMovies;
        }
    }


    /// <summary>
    /// Interface til Discover DB, formålet er nemmere tests.
    /// </summary>
    public interface IDiscoverDB
    {
        public List<Person> GetPeople(List<int> inputMovieInts);
        public List<Movie> GetMovies(List<int> personIds);
    }

    /// <summary>
    /// Til at fortage DB kald på vejende af Discover
    /// </summary>
    public class DiscoverDB : IDiscoverDB
    {
        public List<Person> GetPeople(List<int> inputMovieInts)
        {
            List<Person> people;

            using (var db = new MyDbContext())
            {
                Console.WriteLine("Henter alle skuespillere involveret:");
                // Ny liste over personer, som skal bruges til at finde alle VIP fra de fem inputfilm
                people = (from pers in db.Persons.Where(x => x._Personpopularity > 20)
                          join emp in db.Employments.Where(x => inputMovieInts.Contains(x._movieId))
                          on pers._personId equals emp._personId
                          where emp._job == "Acting"
                          select pers).ToList();

                Console.WriteLine("Henter alle instruktørere og Producere involveret:");
                people.AddRange((from pers in db.Persons
                                 join emp in db.Employments.Where(x => inputMovieInts.Contains(x._movieId))
                                 on pers._personId equals emp._personId
                                 where emp._job == "Director" || emp._job == "Producer"
                                 select pers).ToList());
            }
            return people;
        }

        public List<Movie> GetMovies(List<int> personIds)
        {
            List<Movie> shortList;

            using (var db = new MyDbContext())
            {
                shortList = (from m in db.Movies.Include(z => z._employmentList).Include(y => y._genreList)
                             join e in db.Employments.Where(x => personIds.Contains(x._personId))
                             on m.movieId equals e._movieId
                             select m).ToList();


                shortList = shortList.Distinct().ToList();
            }

            return shortList;
        }
    }



    /// <summary>
    /// Selve Discover algoritmen
    /// </summary>
    public class Discover
    {
        // Listen som holder alle film som kan vælges imellem.
        public List<Movie> shortList = new List<Movie>();

        // De fem film valgt inde på Discover siden.
        public List<Movie> inputMovies;



        // Vi holder styr på hvor lang tid at Discover tager at beregne/hente data!
        private LoadTimer timer = new LoadTimer();



        /// <summary>
        /// 5 movies enter, one movie leaves: Mad Max rulez!
        /// Giv den en liste med 5 Ints, og den sender et Movie objekt tilbage!
        /// </summary>
        /// <param name="inputMovieInts"></param>
        /// <returns></returns>
        public Movie DiscoverMovies(List<int> inputMovieInts, IDiscoverDB getDB, IDiscoverInputMovies InputMoviesObj)
        {
            IDiscoverDB db = getDB;

            // Log time:
            timer.StartTimer();

            // læg input movies over i den variabel som vi arbejder med.
            IDiscoverInputMovies obj = InputMoviesObj;
            
            List<Movie> inputMovies = obj.GetInputMovies(inputMovieInts);


            Console.WriteLine("Henter alle skuespillere involveret:");
            // Ny liste over personer, som skal bruges til at finde alle VIP fra de fem inputfilm
            List<Person> people = db.GetPeople(inputMovieInts);

            // TEMP, lægger personer i json fil
            //ObjectToJson json = new ObjectToJson(people, "people");


            // Tager en mellemtid!
            timer.StopTimer();


            Console.WriteLine("Persons found: " + people.Count);
            List<int> personIds = new List<int>();
            foreach (var person in people)
            {
                personIds.Add(person._personId);
            }


            Console.WriteLine("Henter alle film som de har været involveret i");
            shortList = db.GetMovies(personIds);

            Console.WriteLine("Shortlist from input has count of: " + shortList.Count);

            // Tager en mellemtid!
            timer.StopTimer();


            Console.WriteLine("Input Movies:");
            foreach (var movie in inputMovies)
            {
                Console.WriteLine(movie._title);
                if(shortList.Any(x => x.movieId == movie.movieId))
                {
                    shortList.Remove(shortList.Find(x => x.movieId == movie.movieId));
                }
            }
            Console.WriteLine("Shortlist from input has count of: " + shortList.Count);



            // Kør filtering:
            Console.WriteLine("");
            Console.WriteLine("Running filters...");

            // Opret filtrene:
            Filters filters = new Filters(inputMovies, shortList);

            // Opret filter visitor, som vil gennemløbe alle filtre:
            FilterVisitor newFilterVisitor = new FilterVisitor(new List<List<DiscoverScore>>(), new List<DiscoverScore>());

            // Sæt processen igang:
            filters.accept(newFilterVisitor);



            // Log time:
            timer.StopTimer();


            // Send endelige valgte film tilbage:
            return newFilterVisitor.FinalResult;
        }




    }
}
