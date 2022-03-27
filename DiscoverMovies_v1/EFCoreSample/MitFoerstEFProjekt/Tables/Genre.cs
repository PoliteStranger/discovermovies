using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitFoerstEFProjekt.Tables
{
    public class Genre
    {
        [Key]
        public int genreKey { get; set; }

        [ForeignKey("_genreId")]
        public int _genreId { get; set; }
        public Genres genre { get; set; }

        [ForeignKey("_movieId")]
        public int _movieId { get; set; }
        public Movie movie { get; set; }
    }
}
