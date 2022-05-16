using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscoverMoviesProduction
{
    public class ProdCompany
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int prodCompanyId { get; set; }

        public string _ProdCompanyname { get; set; }
        public string? _ProdCompanycountry { get; set; }
    }
}
