using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.DataMangerAPI.Entities;

public class CardInfo
{
    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    [EnumDataType(typeof(CardType))]
    public CardType CardType { get; set; }
}