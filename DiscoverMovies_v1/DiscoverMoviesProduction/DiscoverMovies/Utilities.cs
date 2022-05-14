namespace DiscoverMoviesProduction
{


    /// <summary>
    /// Timer til at tage tid på de lage DB kald!
    /// Skab et objekt, og kør Start, så kør Stop, når tiden skal måles. Den kan stoppes flere gange.
    /// </summary>
    public class LoadTimer
    {
        private DateTime Start { get; set; }

        private DateTime End { get; set; }

        public void StartTimer()
        {
            Start = DateTime.Now;
        }

        public void StopTimer()
        {
            End = DateTime.Now;
            Console.WriteLine("Time taken: " + (End - Start));
        }

    }


    public class Guard
    {
        public static void CheckDiscoverLists(List<Movie> inputMovies, List<Movie> shortList, string Msg)
        {
            // Check if value is null.
            if (inputMovies == null || shortList == null)
                throw new System.ArgumentNullException("Null List Error: " + Msg);

        }


    }
}
