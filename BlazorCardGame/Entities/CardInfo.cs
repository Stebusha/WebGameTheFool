using System.ComponentModel.DataAnnotations;
using BlazorCardGame.Enums;

namespace BlazorCardGame.DataMangerAPI.Entities;

public class CardInfo
{
    public int Id { get; set; }

    [Required]
    public string? ImageUrl { get; set; }

    [Required]
    [EnumDataType(typeof(CardType))]
    public CardType CardType { get; set; }
}