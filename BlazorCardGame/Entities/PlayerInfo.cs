using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BlazorCardGame.Enums;

namespace BlazorCardGame.Entities;

public class PlayerInfo
{
    public int Id { get; set; }
    public string? UserLogin { get; set; }
    public ApplicationUser? User { get; set; }

    [DefaultValue(PlayerType.AI)]
    [EnumDataType(typeof(PlayerType))]
    public PlayerType PlayerType { get; set; }
    public bool IsAttack { get; set; }
    // public int FoolGameId { get; set; }
    // FoolGame? FoolGame { get; set; }
}