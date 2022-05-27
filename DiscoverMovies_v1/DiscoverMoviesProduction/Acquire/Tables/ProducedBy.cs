using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcquireDB_EFcore.Tables
{
    public class ProducedBy
    {
        [Key]
        public int producedById { get; set; }

        [ForeignKey("Movies")]
        public int _movieId { get; set; }
        public Movie Movies { get; set; }

        [ForeignKey("ProdCompanies")]
        public int prodCompanyId { get; set; }
        public ProdCompany ProdCompanies { get; set; }

    }
}
