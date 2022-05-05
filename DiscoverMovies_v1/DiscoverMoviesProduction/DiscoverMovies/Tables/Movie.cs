using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AcquireDB_EFcore;
using AcquireDB_EFcore.Tables;



public class Movie 
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Required]
    public int movieId { get; set; }
    public string _title { get; set; }

    public string? _description { get; set; }
    public string? _posterUrl { get; set; }



    public List<Employment> _employmentList { get; set; } = new();

    public List<Genre> _genreList { get; set; } = new();

    public List<ProdCompany> _prodCompanyList { get; set; } = new();

    public DateTime? _releaseDate { get; set; }
    public int? _budget { get; set; }
    public int? _revenue { get; set; }
    public double? _popularity { get; set; }
    public int? _runtime { get; set; }

    public Movie()
    { }

}

