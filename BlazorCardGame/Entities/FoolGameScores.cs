using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.DataMangerAPI.Entities;

public class FoolGameScores
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public int? ApplicationUserID { get; set; }

    [Range(0,int.MaxValue)]
    public int? NumberOfWins { get; set; }

    [Range(0,int.MaxValue)]
    public int? NumberOfLosses { get; set; }

    [Range(0,int.MaxValue)]
    public int? NumerOfDraws { get; set; }

    [Range(0,int.MaxValue)]
    public int? CountOfGames { get; set; }
}