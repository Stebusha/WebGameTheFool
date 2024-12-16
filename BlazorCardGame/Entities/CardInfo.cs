// using System.ComponentModel.DataAnnotations;
// using BlazorCardGame.Enums;

// namespace BlazorCardGame.Entities;

// public class CardInfo
// {
//     public int Id { get; set; }

//     [Required]
//     public string ImageUrl { get; set; } = null!;

//     [Required]
//     [EnumDataType(typeof(CardType))]
//     public CardType CardType { get; set; }
//     public int FoolGameId { get; set; }
//     public FoolGame FoolGame { get; set; } = null!;
// }