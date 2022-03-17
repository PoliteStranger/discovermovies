using System;

using MitFoerstEFProjekt;
using MitFoerstEFProjekt.Tables;

public class Movie
{
    public int movieId { get; set; }
    public string _title { get; set; }
    public List<Employment> _employmentList { get; set; } = new();
    public List<Genre> _genreList { get; set; } = new();
    public List<ProdCompany> _prodCompanyList { get; set; } = new();
    public DateTime _releaseDate { get; set; }
    public int _budget { get; set; }
    public int _revenue { get; set; }
    public double _popularity { get; set; }

    public Movie()
    {}
    
}