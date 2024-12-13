using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.Entities;

public class FoolGame
{
    public int Id { get; set; }

    [Required]
    public List<PlayerInfo> Players { get; set; } = new();

    [Required]
    public List<CardInfo> Cards { get; set; } = new();

    [Required]
    public int DicardsCardCount { get; set; } = 0;

    [Required]
    [Range(0, int.MaxValue)]
    public int CountOfGames { get; set; }
}