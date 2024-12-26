using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BlazorCardGame.Enums;

namespace BlazorCardGame.Entities;

public class PlayerInfo
{
    [Required]
    [Key]
    public string Name { get; set; } = null!;
    public string? UserLogin { get; set; }
    public ApplicationUser? User { get; set; }

    [DefaultValue(PlayerType.AI)]
    [EnumDataType(typeof(PlayerType))]
    public PlayerType PlayerType { get; set; }

    [DefaultValue(false)]
    public bool IsAttack { get; set; }

    public FoolGameScore? Score { get; set; }

}