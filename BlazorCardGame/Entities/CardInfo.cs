using System.ComponentModel.DataAnnotations;
using BlazorCardGame.Enums;

namespace BlazorCardGame.Entities;

public class CardInfo
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string ImageUrl { get; set; } = null!;

    [Required]
    [EnumDataType(typeof(CardType))]
    public CardType CardType { get; set; }
}