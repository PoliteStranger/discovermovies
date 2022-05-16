using AcquireDB_EFcore.Tables;

namespace ASP_Web_Bootstrap.Search.Init
{
    public interface Iinitializer
    {
        public void initSearchOption(List<string> soegningsliste);
        public void initYear(List<int> yearliste);
        public List<Genres> initGenre(List<Genres> liste);
    }
}
