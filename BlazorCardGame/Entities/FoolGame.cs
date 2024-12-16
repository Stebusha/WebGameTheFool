using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.Entities;

public class FoolGame
{
    [Required]
    public string PlayerInfoName { get; set; } = null!;

    [Required]
    public string OpponentInfoName { get; set; } = null!;

    [Required]
    public List<CardInfo> Cards { get; set; } = new();

    [Required]
    public int DiscardCardsCount { get; set; } = 0;

    [Required]
    [Range(0, int.MaxValue)]
    public int CountOfGames { get; set; }
}