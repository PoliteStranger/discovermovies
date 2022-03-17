using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitFoerstEFProjekt.Tables
{
    public class Employment
    {
        [Key]
        public int employmentId { get; set; }
        public Movie movieId { get; set; }
        public Person personId { get; set; }
        public string _job { get; set; }
        public string _character { get; set; }

    }
}
