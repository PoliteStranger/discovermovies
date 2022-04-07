using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcquireDB_EFcore.Tables;
using AcquireDB_EFcore;

namespace HTML_pages_framework.Pages
{
    public class MovieModel : PageModel
    {

        [BindProperty]
        public int? movieId { get; set; }

        [BindProperty]
        public string title { get; set; }

        [BindProperty]
        public string description { get; set; }

        [BindProperty]
        public int? movieId { get; set; }

        public void OnGet([FromRoute] int? movieId = null)
        {

            movieId = movieId;

            var db = new MyDbContext();




        }
    }
}
