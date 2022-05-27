using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverMoviesProduction
{
    public class Genres
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int _genreId { get; set; }

        public string _Genrename { get; set; }
    }

}