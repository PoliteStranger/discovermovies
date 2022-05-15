using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverMoviesProduction
{
    public class Genre
    {
        [Key]
        public int genreKey { get; set; }

        [ForeignKey("Genres")]
        public int _genreId { get; set; }
        public Genres Genres { get; set; }

        [ForeignKey("Movies")]
        public int _movieId { get; set; }
        public Movie Movies { get; set; }
    }
}
