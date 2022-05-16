﻿using System.ComponentModel.DataAnnotations;
using AcquireDB_EFcore.Tables;
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

        public List<Genres> initGenre(List<Genres>liste)
        {
            using (var db = new MyDbContext())
            {
                liste = db.Genres.ToList();
            }
            return liste;
        }
    }
}
