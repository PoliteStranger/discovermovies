using AcquireDB_EFcore.Tables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcquireDB_EFcore
{
    public class MakeGenresList
    {
        // Alle genre
        string jsonGenres = "{\"genres\":[{\"id\":28,\"name\":\"Action\"},{\"id\":12,\"name\":\"Adventure\"},{\"id\":16,\"name\":\"Animation\"},{\"id\":35,\"name\":\"Comedy\"},{\"id\":80,\"name\":\"Crime\"},{\"id\":99,\"name\":\"Documentary\"},{\"id\":18,\"name\":\"Drama\"},{\"id\":10751,\"name\":\"Family\"},{\"id\":14,\"name\":\"Fantasy\"},{\"id\":36,\"name\":\"History\"},{\"id\":27,\"name\":\"Horror\"},{\"id\":10402,\"name\":\"Music\"},{\"id\":9648,\"name\":\"Mystery\"},{\"id\":10749,\"name\":\"Romance\"},{\"id\":878,\"name\":\"Science Fiction\"},{\"id\":10770,\"name\":\"TV Movie\"},{\"id\":53,\"name\":\"Thriller\"},{\"id\":10752,\"name\":\"War\"},{\"id\":37,\"name\":\"Western\"}]}";

        public MakeGenresList(MyDbContext db)
        {

            Console.WriteLine("Getting genres:");
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonGenres);

            foreach(var genre in jsonObj.genres)
            {
                // Hvis den allerede er i DB så springer vi over!
                if (db.Genres.Find((int)genre.id) == null)
                {
                    Console.WriteLine("Getting: " + (string)genre.name);
                    db.Genres.Add(new Genres() { _genreId = (int)genre.id, _Genrename = (string)genre.name });
                    db.SaveChanges();

                }
                else
                {
                    Console.WriteLine("Got it!");
                }

            }
            Console.WriteLine("Saving to DB");
            
        }
    }
}
