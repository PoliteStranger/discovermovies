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
        
        public int movieId { get; set; }
        public Movie movie { get; set; }

        public int personId { get; set; }
        public Person person { get; set; }

        public string _job { get; set; }
        public string _character { get; set; }

    }
}
