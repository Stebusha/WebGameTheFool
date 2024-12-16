using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCardGame.Entities;

public class FoolGameScore
{
    public int Id { get; set; }
    public string PlayerInfoName { get; set; } = null!;
    public PlayerInfo? PlayerInfo { get; set; } = null!;

    [Range(0, int.MaxValue)]
    [DefaultValue(0)]
    public int? NumberOfWins { get; set; }

    [Range(0, int.MaxValue)]
    [DefaultValue(0)]
    public int? NumberOfLosses { get; set; }

    [Range(0, int.MaxValue)]
    [DefaultValue(0)]
    public int? NumberOfDraws { get; set; }

    [Range(0, int.MaxValue)]
    [DefaultValue(0)]
    public int? CountOfGames { get; set; }
}