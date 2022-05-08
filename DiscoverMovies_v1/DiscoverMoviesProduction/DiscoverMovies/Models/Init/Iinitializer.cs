namespace ASP_Web_Bootstrap.Models.Init
{
    public interface Iinitializer
    {
        public void initSearchOption(List<string> soegningsliste);
        public void initYear(List<int> yearliste);
        public List<Genres> initGenre(List<Genres> Genrelisten);
    }
}
