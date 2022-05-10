namespace DiscoverMoviesProduction
{
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
}
