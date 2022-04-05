using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcquireDB_EFcore.Tables
{
    public class ProdCompanies
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int ProdCompaniesId { get; set; }

        public string _ProdCompaniesname { get; set; }
        public string? _ProdCompaniescountry { get; set; }
    }
}
