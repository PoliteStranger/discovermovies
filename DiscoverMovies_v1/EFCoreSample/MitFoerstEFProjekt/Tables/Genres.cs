using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcquireDB_EFcore.Tables;


public class Genres
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int _genreId { get; set; }

        public string _Genrename { get; set; }
    }

