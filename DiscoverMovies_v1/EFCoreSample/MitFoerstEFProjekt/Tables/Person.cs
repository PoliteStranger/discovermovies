using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitFoerstEFProjekt.Tables
{
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int personId { get; set; }
        public string _Personname { get; set; }
        public double _Personpopularity { get; set; }
        public List<Employment> employmentList { get; set; }

        public DateTime _dob { get; set; }

        public DateTime _dod { get; set; }
    }
}
