using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcquireDB_EFcore.Tables
{
    public class ProdCompany
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int ProdCompanyId { get; set; }

        public string _ProdCompanyname { get; set; }
        public string? _ProdCompanycountry { get; set; }
    }
}
