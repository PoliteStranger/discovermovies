using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcquireDB_EFcore.Tables
{
    public class Employment
    {
        [Key]
        public int employmentId { get; set; }

        [ForeignKey("Movies")]
        public int _movieId { get; set; }
        public Movie Movies { get; set; }

        [ForeignKey("Person")]
        public int _personId { get; set; }
        public Person Person { get; set; }

        public string _job { get; set; }
        public string? _character { get; set; }

    }
}
