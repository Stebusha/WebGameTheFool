using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.Entities;

public class FoolGame
{
    public int Id { get; set; }

    [Required]
    public List<ApplicationUser> Players { get; set; } = new();

    [Required]
    List<CardInfo> Cards { get; set; } = new();
    
    [Required]
    [Range(0, int.MaxValue)]
    public int CountOfGames { get; set; }
}