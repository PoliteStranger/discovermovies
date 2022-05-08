using System.ComponentModel.DataAnnotations;
using ASP_Web_Bootstrap.Search.Init;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP_Web_Bootstrap.Search.Init
{
    public class soegning : Iinitializer
    {
        public void initSearchOption(List<string> soegningsliste)
        {
            soegningsliste.Add("Movie");
            soegningsliste.Add("Person");
        }

        public void initYear(List<int> yearliste)
        {
            //dropdown menu til år
            for (int i = 1980; i<2021; i++)
            {
                yearliste.Add(i);
            }
        }

        public List<Genres> initGenre()
        {
            List <Genres> tempgenres = new List<Genres>();
            //henter genres ned til dropdownmenu Genre
            using (var db = new MyDbContext())
            {
                tempgenres = db.Genres.ToList();
            }
            return tempgenres;
        }
    }
}
