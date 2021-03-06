using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiscoverMoviesProduction.Controllers
{
    // Autocomplete til søgningssiden
    [Route("api/autocomplete/")]
    [ApiController]
    public class AutocompleteController : ControllerBase
	{
        // Post http requestet til at bruge autocomplete
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] string value)
        {
            // Skaber og bruger vores database objekt:
            using (var db = new MyDbContext())
            {
                // Opretter en ny søgning
                var query = (from m in db.Movies

                             // Den søgte værdi SKAL matche enten starten af filmtitelen, ELLER starten af et nyt ord i filmtitelen!
                             where m._title.StartsWith(value) || m._title.Contains(" " + value)
                             //             Skal starte med ^   Nyt ord, så et space ind foran ^

                             // Vi laver den nye sammensluttede tabel
                             select new
                             {
                                 label = m._title,      // De hedder label og value, fordi at jQuery autocomplete bruger disse!!!
                                 value = m._title
                             }).ToList();

                if(query.Count > 10)
                    query = query.GetRange(0, 10);

                // Der burde sættes noget filtrering ind her, som sikre at der ikke kommer alt for mange forslag. Alt over 20 er for meget!
                // Autocomplete UI elementet bliver mega langsomt med alt over 20 elementer!

                // Feedback, så vi kan holde styr på om det virker!
                Console.WriteLine("Autocomplete fandt " + query.Count + " matches!");

                // Send som serialiseret json:
                return JsonConvert.SerializeObject(query);
            }


            
        }
	}
}
