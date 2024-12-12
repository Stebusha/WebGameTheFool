using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorCardGame.Entities;

public class ApplicationUser
{
    [Required(ErrorMessage = "Введите логин")]
    [StringLength(20)]
    [Key]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(20)]
    public string Password { get; set; } = null!;
    public DateTime? LastEnteredTime { get; set; }

    [NotMapped]
    [Display(Name = "Запомнить?")]
    public bool RememberMe { get; set; }
    public PlayerInfo? playerInfo { get; set; }
}