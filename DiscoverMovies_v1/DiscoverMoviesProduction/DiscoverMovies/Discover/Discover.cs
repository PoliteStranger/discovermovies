using System.Linq;
using System.Collections.Generic;
using ASP_Web_Bootstrap;
using Microsoft.EntityFrameworkCore;
using AcquireDB_EFcore.Tables;
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
                InputMovies = db.Movies.Where(c => InputMovieIds.Contains(c.movieId)).Include(x => x._genreList).Include(y => y._prodCompanyList).Include(z => z._employmentList).ToList();
            }


            return InputMovies;
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

        public DiscoverIntsToMovies IntsToMovies = new DiscoverIntsToMovies();

        


        // Settings:

        // Hvor populær skal en skuespiller være før vi vil bruge dem på shortlisten:
        private double actorPopularityMin = 20.0;

        private double moviePopularityMin = 10.0;

        // Vi holder styr på hvor lang tid at Discover tager at beregne/hente data!
        private LoadTimer timer = new LoadTimer();

        // Vi tager tid på hvor lang tid siden er om at beregne resultatet
        public int dbKald = 0;


        /// <summary>
        /// 5 movies enter, one movie leaves: Mad Max rulez!
        /// Giv den en liste med 5 Ints, og den sender et Movie objekt tilbage!
        /// </summary>
        /// <param name="inputMovieInts"></param>
        /// <returns></returns>
        public Movie DiscoverMovies(List<int> inputMovieInts)
        {
            // Log time:
            timer.StartTimer();

            // læg input movies over i den variabel som vi arbejder med.
            inputMovies = IntsToMovies.GetInputMovies(inputMovieInts);

            // Finder personer, derefter alle film som de har med til at lave, og lægger dem i shortListen.
            using (var db = new MyDbContext())
            {
                Console.WriteLine("Henter alle skuespillere involveret:");
                // Ny liste over personer, som skal bruges til at finde alle VIP fra de fem inputfilm
                List<Person> people = (from pers in db.Persons.Where(x => x._Personpopularity > actorPopularityMin)
                                       join emp in db.Employments.Where(x => inputMovieInts.Contains(x._movieId))
                                       on pers._personId equals emp._personId
                                       where emp._job == "Acting"
                                       select pers).ToList();
                
                timer.StopTimer();
                Console.WriteLine("Henter alle instruktørere og Producere involveret:");
                people.AddRange((from pers in db.Persons
                                       join emp in db.Employments.Where(x => inputMovieInts.Contains(x._movieId))
                                       on pers._personId equals emp._personId
                                       where emp._job == "Director" || emp._job == "Producer"
                                       select pers).ToList());

                timer.StopTimer();
                Console.WriteLine("Persons found: " + people.Count);

                List<int> personIds = new List<int>();
                foreach (var person in people)
                {
                    personIds.Add(person._personId);
                }


                Console.WriteLine("Henter alle film som de har været involveret i");
                shortList = (from m in db.Movies.Include(z => z._employmentList).Include(y => y._genreList)
                             join e in db.Employments.Where(x => personIds.Contains(x._personId))
                             on m.movieId equals e._movieId
                             select m).ToList();

                timer.StopTimer();
                shortList = shortList.Distinct().ToList();




            }

            Console.WriteLine("Input Movies:");
            foreach (var movie in inputMovies)
            {
                shortList.Remove(shortList.Find(x => x.movieId == movie.movieId));
                Console.WriteLine(movie._title);
            }
            Console.WriteLine("Shortlist from input has count of: " + shortList.Count);











            // Kør filtering:
            Console.WriteLine("");
            Console.WriteLine("Running filters...");

            // Opret filtrene:
            Filters filters = new Filters(inputMovies, shortList);

            // Opret filter visitor, som vil gennemløbe alle filtre:
            FilterVisitor newFilterVisitor = new FilterVisitor();

            // Sæt processen igang:
            filters.accept(newFilterVisitor);



            // Log time:
            timer.StopTimer();


            // Send endelige valgte film tilbage:
            return newFilterVisitor.FinalResult;
        }




    }
}
