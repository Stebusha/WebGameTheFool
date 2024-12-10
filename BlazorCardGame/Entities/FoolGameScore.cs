using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.Entities;

public class FoolGameScore
{
    public int Id { get; set; }
    public string UserLogin {get; set;} = null!;
    public ApplicationUser User { get; set; } = null!;

    [Range(0, int.MaxValue)]
    public int? NumberOfWins { get; set; }

    [Range(0, int.MaxValue)]
    public int? NumberOfLosses { get; set; }

    [Range(0, int.MaxValue)]
    public int? NumerOfDraws { get; set; }

    [Range(0, int.MaxValue)]
    public int? CountOfGames { get; set; }
}