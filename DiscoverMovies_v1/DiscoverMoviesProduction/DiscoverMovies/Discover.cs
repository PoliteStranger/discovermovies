using System.Linq;
using System.Collections.Generic;
using Database.Tables;

namespace DiscoverMoviesProduction
{
    public class DiscoverScore
    {
        public DiscoverScore()
        {
            Movie = new Movie();
        }

        public DiscoverScore(Movie movie, double score)
        {
            Movie = movie;
            Score = score;
        }

        public Movie Movie { get; set; }

        public double Score { get; set; }

    }

    // Til at skabe lister over par
    public class PersonPair
	{
        public int personA { get; set; }
        public int personB { get; set; }

        public PersonPair(int persona, int personb)
		{
            personA = persona;
            personB = personb;
		}
	}

     

    public class Discover
    {
        List<Movie> shortList = new List<Movie>();
        List<Movie> inputMovies;

        // Settings:

        // Hvor populær skal en skuespiller være før vi vil bruge dem på shortlisten:
        private double actorPopularityMin = 20.0;

        // Hvor populær skal en film være før vi vil bruge den på shortlisten:
        private double filmPopularityMin = 20.0;
        

        // Time Logging:
        public DateTime Start { get; set; }
        private DateTime End { get; set; }


        // 5 movies enter, one movie leaves: Mad Max rulez!
        public List<Movie> DiscoverMovies(List<Movie> inputMovies)
        {
            // Log time:
            Start = DateTime.Now;

            this.inputMovies = inputMovies;

            // Shortlist
            //      Alle film, som samtlige instruktører, producerer, og filmstjerner(pop 20+) har være med til at lave
            //


            using (var db = new MyDbContext())
            {
                // 8374, 1542, 603, 564, 3293



                

                List<Person> people = new List<Person>();

                // Find alle instruktøre og producere, og skuespillere m. popularity 20+
                foreach (var movie in inputMovies)
                {

                    // db.Movies.Where(x => x._movieId == 3293).Include(x => x._Employment).First

                    foreach(var employment in db.Employments.Where(x => x._movieId == movie.movieId).ToList())
                    {
                        if(employment._job != "Acting")
                        {
                            people.Add(db.Persons.Find(employment._personId));
                        }

                        if(employment._job == "Acting" && db.Persons.Find(employment._personId)._Personpopularity > actorPopularityMin)
                        {
                            people.Add(db.Persons.Find(employment._personId));
                        }
                    }
                }

                // Find alle film som de har været med i:
                foreach (var person in people)
                {
                    foreach(var employment in db.Employments.Where(x => x._personId == person._personId).ToList())
                    {
                        if(!shortList.Any(x => x.movieId == employment._movieId))
                            shortList.Add(db.Movies.Find(employment._movieId));
                    }
                    
                }
                
                

                Console.WriteLine("Input Movies:");
                foreach(var movie in inputMovies.ToList())
                {
                    shortList.Remove(shortList.Find(x => x.movieId == movie.movieId));
                    Console.WriteLine(movie._title);
                }
                Console.WriteLine("Shortlist from input has count of: " + shortList.Count);
                
            }

            // spacer
            Console.WriteLine("");



            // Filters
            // 1. Genre
            // 2. Cast
            // 3. Crew
            // 4. Year
            // 5. Production Companies

            List<DiscoverScore> genreMovies = GenreFilter(shortList);
            List<DiscoverScore> castMovies = CastFilter(shortList);
            List<DiscoverScore> crewMovies = CrewFilter(shortList);
            List<Movie> yearMovies = YearFilter(shortList);
            List<Movie> ProdMovies = ProdFilter(shortList);

            // Samlede score liste
            List<DiscoverScore> finalScore = new List<DiscoverScore>();

            double GenreMaxScore = genreMovies.Max(x => x.Score);
            double CastMaxScore = castMovies.Max(x => x.Score);
            double CrewMaxScore = crewMovies.Max(x => x.Score);

            // Gennemgang af shortlist, og tælle points sammen:
            foreach (var movie in shortList)
            {
                // Hver film starter med en score på 0
                double score = 0;

                // HVIS filmen har fået en score
                if(genreMovies.Any(x => x.Movie == movie))
                    score += (1/GenreMaxScore) * genreMovies.Find(x => x.Movie == movie).Score; // Så hent dens points fra Genre Filteret
                // HVIS filmen har fået en score
                if (castMovies.Any(x => x.Movie == movie))
                    score += (1 / CastMaxScore) * castMovies.Find(x => x.Movie == movie).Score;// Så hent dens points fra Cast Filteret
                // HVIS filmen har fået en score
                if (crewMovies.Any(x => x.Movie == movie))
                    score += (1 / CrewMaxScore) * crewMovies.Find(x => x.Movie == movie).Score;// Så hent dens points fra Crew Filteret

                // Til filmen, samt summen af dens points:
                finalScore.Add(new DiscoverScore(movie, score));
            }

            // I rækkefølge af score:
            finalScore = finalScore.OrderByDescending(x => x.Score).ToList();

            // Print til consol
            Console.WriteLine("Final scores:");
            foreach(var score in finalScore)
            {
                Console.WriteLine(score.Movie._title + ": " + score.Score.ToString("0.00"));
            }

            // Vi timer kodeafviklingen, så vi kan se om det går hurtigt nok!
            // OBS Console print TAGER EKSTRA TID, de skal fjernes, så vi kan få den endelige tid!

            // Log time:
            End = DateTime.Now;

            Console.WriteLine("Discover took: " + (End - Start));
            return inputMovies;
        }

        public List<DiscoverScore> GenreFilter(List<Movie> shortlist)
        {
            List<Movie> genreList = new List<Movie>();
            List<int> genreCounter = new List<int>();
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: GENRE");
            Console.WriteLine("-------------------------------------------------");



            foreach (var movie in inputMovies)
            {

                foreach (var genre in movie._genreList)
                {
                    genreCounter.Add(genre._genreId);
                }
            }


            var genreCounted = genreCounter.GroupBy(x => x).Where(g => g.Count() > 0)// skal være  > 1
                .Select(y => new { Id = y.Key, Count = y.Count() }).ToList();

            foreach (var genre in genreCounted)
            {
                //Console.WriteLine("genre: " + genre);
            }

            foreach (var _Movie in shortlist)
            {
                int score = 0;
                using (var db = new MyDbContext())
                {
                    var genres = db.GenresAndMovies.Where(Genre => Genre._movieId == _Movie.movieId);

                    foreach (var genre in genres)
                    {
                        if (genreCounted.Find(x => x.Id == genre._genreId) != null)
                        {
                            score = score + genreCounted.Find(x => x.Id == genre._genreId).Count;
                        }
                    }
                    discoverScores.Add(new DiscoverScore(_Movie, score));
                }

            }
            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();
            foreach (var score in discoverScores)
            {
                Console.WriteLine(score.Movie._title + " has " + score.Score);
            }

            /*
            discoverScores.Sort(delegate (DiscoverScore x, DiscoverScore y) // denne sortering er vist ikke nødvendig hvis vi bare er efter point.
            {
                return y.Score.CompareTo(x.Score);
            });
            */

            return discoverScores;
        }

        public List<DiscoverScore> CastFilter(List<Movie> shortlist)
        {

            var db = new MyDbContext();


            // En liste over crew
            List<Movie> crewList = new List<Movie>();

            // Til at tildele points
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: CAST");
            Console.WriteLine("-------------------------------------------------");



            List<Employment> inputEmployments = new List<Employment>();


            foreach (var movie in inputMovies.ToList())
            {
                foreach (var employment in movie._employmentList.ToList())
                {
                   // Console.WriteLine("!" + employment._job);
                    if (employment._job == "Acting")
                    {
                        inputEmployments.Add(employment);
                    }

                }
            }
            Console.WriteLine("Found {0} cast in input", inputEmployments.Count);


            Console.WriteLine("\nSearching for matches in cast:");



            foreach (var movie in shortList.ToList())
            {
                //Console.Write(">");

                foreach (var employment in inputEmployments.ToList())
                {

                    if (movie._employmentList.Any(x => x._personId == employment._personId))
                    {

                        // If match, then pass out score:
                        if (discoverScores.Any(x => x.Movie == movie))
                        {
                            discoverScores.Find(x => x.Movie == movie).Score++;
                            //Console.Write(" Match!");
                        }
                        else
                        {
                            discoverScores.Add(new DiscoverScore(movie, 1));
                            //Console.Write(" Match!");
                        }
                        //Console.WriteLine(discoverScores.Find(x => x.Movie == movie).Movie._title + " has " + discoverScores.Find(x => x.Movie == movie).Score + " points");

                    }
                }

            }

            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();
            foreach (var score in discoverScores)
            {
                Console.WriteLine(score.Movie._title + " has " + score.Score);
                foreach (var employment in inputEmployments.ToList())
                {
                    if (employment._movieId == score.Movie.movieId)
                        Console.WriteLine(employment.Person._Personname + " was " + employment._job);
                }
            }

            return discoverScores;
        }

        public List<DiscoverScore> CrewFilter(List<Movie> shortlist)
        {
            var db = new MyDbContext();


            // En liste over crew
            List<Movie> crewList = new List<Movie>();

            // Til at tildele points
            List<DiscoverScore> discoverScores = new List<DiscoverScore>();

            Console.WriteLine("");
            Console.WriteLine("FILTER: CREW");
            Console.WriteLine("-------------------------------------------------");



            // Par filtering
            /*
            // Par søgninger

            Console.WriteLine("Discover: Looking for Pairs in Crews:");

            // Director + Cinematographer pairs
            Console.WriteLine("Discover: Looking for Director + Director of Photography pairs");
            List<PersonPair> DirectorDOP = new List<PersonPair>();
            foreach (var movie in inputMovies.ToList())
            {
                //Console.WriteLine("Søger Instruktør+Fotograf par i filmen " + movie._title);

                int? tempDirector = null;
                int? tempDOP = null;

                foreach (var employment in movie._employmentList.ToList())
                {


                    if (employment._job == "Director of Photography")
                    {
                        tempDOP = employment._personId;
                    }
                    if (employment._job == "Director")
                    {
                        tempDirector = employment._personId;
                    }

                }
                // HVIS de begge har fået afstat deres poster, så kan vi danne et par:
                if (tempDirector != null && tempDOP != null)
                {
                    Console.WriteLine("{0} - D: {1}, DoP: {2}", movie._title, db.Persons.Find(tempDirector)._Personname, db.Persons.Find(tempDOP)._Personname);
                    DirectorDOP.Add(new PersonPair((int)tempDirector, (int)tempDOP));

                    tempDOP = null;
                    tempDirector = null;
                }
            }



            // Director + Producer pairs
            Console.WriteLine("Discover: Looking for Director + Producer pairs");
            List<PersonPair> DirectorProducer = new List<PersonPair>();
            foreach (var movie in inputMovies.ToList())
            {

                int? tempDirector = null;
                int? tempProducer = null;
                //Console.WriteLine("Søger Instruktør+Producer par i filmen " + movie._title);
                foreach (var employment in movie._employmentList.ToList())
                {


                    if (employment._job == "Producer")
                    {
                        tempProducer = employment._personId;
                    }
                    if (employment._job == "Director")
                    {
                        tempDirector = employment._personId;
                    }

                }
                // HVIS de begge har fået afstat deres poster, så kan vi danne et par:
                if (tempDirector != null && tempProducer != null)
                {
                    Console.WriteLine("{0} - D: {1}, P: {2}", movie._title, db.Persons.Find(tempDirector)._Personname, db.Persons.Find(tempProducer)._Personname);
                    DirectorProducer.Add(new PersonPair((int)tempDirector, (int)tempProducer));
                    tempDirector = null;
                    tempProducer = null;
                }
            }



            Console.WriteLine("InputMovies: " + inputMovies.Count);
            Console.WriteLine("DB Employments: " + (int)db.Employments.Count());
            Console.WriteLine("DirectorProducer Pairs: " + DirectorProducer.Count);
            

            foreach (PersonPair personPair in DirectorProducer)
            {
                Console.WriteLine("Pair: " + personPair.personA + ", " + personPair.personB);

                var query = (from m in inputMovies.ToList()
                                join e in db.Employments.ToList()
                                on m.movieId equals e._movieId
                                join eb in db.Employments.ToList()
                                on m.movieId equals eb._movieId
                                where e._personId == personPair.personA
                                where eb._personId == personPair.personB
                                select new
                                {
                                    MovieId = e._movieId,
                                    MovieTitle = m._title,
                                    Director = personPair.personA,
                                    Producer = personPair.personB
                                }).ToList();

                Console.WriteLine("{0} + {1}", db.Persons.Find(personPair.personA)._Personname, db.Persons.Find(personPair.personB)._Personname);
                foreach (var q in query)
				{
                    Console.WriteLine(q.MovieTitle);
                }
                Console.WriteLine(" ");

            }
            */



            List<Employment> inputEmployments = new List<Employment>();


            foreach (var movie in inputMovies.ToList())
            {
                foreach (var employment in movie._employmentList.ToList())
                {
                    if(employment._job != "Acting")
                    {
                        inputEmployments.Add(employment);
                    }

                }
            }
            Console.WriteLine("Found {0} employments in input", inputEmployments.Count);


            Console.WriteLine("\nSearching for matches in crew:");

            

            foreach (var movie in shortList.ToList())
            {
                //Console.Write(">");

                foreach (var employment in inputEmployments.ToList())
                {
                    
                    if (movie._employmentList.Any(x => x._personId == employment._personId))
                    {
                        int AddScore = 1;

                        // Vi vægter instruktøre højere!
                        if (employment._job == "Director" || employment._job == "Producer")
                        {
                            AddScore = 2;
                            //Console.WriteLine("Director/Producer spottet!");
                        }

                        // If match, then pass out score:
                        if (discoverScores.Any(x => x.Movie == movie))
                        {
                            discoverScores.Find(x => x.Movie == movie).Score += AddScore;
                            //Console.Write(" Match!");
                        }
                        else
                        {
                            discoverScores.Add(new DiscoverScore(movie, AddScore));
                            //Console.Write(" Match!");
                        }
                        //Console.WriteLine(discoverScores.Find(x => x.Movie == movie).Movie._title + " has " + discoverScores.Find(x => x.Movie == movie).Score + " points");

                    }
                }
               
            }

            discoverScores = discoverScores.OrderByDescending(x => x.Score).ToList();
            foreach(var score in discoverScores)
            {
                Console.WriteLine(score.Movie._title + " has " + score.Score);
                foreach(var employment in inputEmployments.ToList())
                {
                    if(employment._movieId == score.Movie.movieId)
                    Console.WriteLine(employment.Person._Personname + " was " + employment._job);
                }
            }

            


            foreach (var person in inputEmployments.ToList())
            {
                //Console.WriteLine("Person: " + db.Persons.Find(person._personId)._Personname);
            }


            Console.WriteLine("-------------------------------------------------");


            // Director AND Producer team

            // Director AND Director of Photography team

            return discoverScores;
        }

        public List<Movie> YearFilter(List<Movie> shortlist)
        {
            List<Movie> yearList = new List<Movie>();

            // 2001
            // +/- intervallet   5 - 1999 - 2002 +5

            // +/- 5 år
            // +/- 10 år

            return yearList;
        }

        public List<Movie> ProdFilter(List<Movie> shortlist)
        {
            List<Movie> prodList = new List<Movie>();




            return prodList;
        }


    }
}
