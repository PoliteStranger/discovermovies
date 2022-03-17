using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitFoerstEFProjekt.Tables
{
    public class Person
    {
        public int personId { get; set; }
        public string _Personname { get; set; }
        public double _Personpopularity { get; set; }
        public List<Employment> employmentList { get; set; }
    }
}
