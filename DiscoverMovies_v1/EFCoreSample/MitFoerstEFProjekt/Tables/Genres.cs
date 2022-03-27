using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitFoerstEFProjekt.Tables
{
    public class Genres
    {
        [Key]
        public int genreId { get; set; }

        public int _Genrename { get; set; }
    }
}
