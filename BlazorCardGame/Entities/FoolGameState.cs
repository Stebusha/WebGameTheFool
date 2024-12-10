using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.DataMangerAPI.Entities;

public class FoolGameState
{
    public int Id { get; set; }

    // [Required]
    // [StringLength(20)]
    // public string? UserLogin { get; set; }

    // [Required]
    // [StringLength(20)]
    // public string? OpponentLogin { get; set; }

    [Required]
    public List<ApplicationUser>? Players { get; set; }

    [Required]
    List<CardInfo>? Cards { get; set; }

    [Required]
    [Range(0,int.MaxValue)]
    public int CountOfGames { get; set; }

    // [Required]
    // public bool IsPlayerAttack { get; set; }
}