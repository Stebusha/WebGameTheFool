using System.ComponentModel.DataAnnotations;

namespace BlazorCardGame.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Введите логин")]
    [StringLength(15, ErrorMessage = "Логин должен быть не меньше {2} символов", MinimumLength = 5)]
    public string Login { get; set; } = string.Empty;
    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(20, ErrorMessage = "Пароль должен быть не меньше {2} символов", MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = string.Empty;
}