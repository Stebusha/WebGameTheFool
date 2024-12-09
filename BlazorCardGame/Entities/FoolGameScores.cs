using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.DataMangerAPI.Entities;

public class FoolGameScores
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string? UserLogin { get; set; }
    public int? NumberOfWins { get; set; }
    public int? NumberOfLosses { get; set; }
    public int? NumerOfDraws { get; set; }
    public int? CountOfGames { get; set; }
}