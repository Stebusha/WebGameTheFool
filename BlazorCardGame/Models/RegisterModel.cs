namespace BlazorCardGame.Models;

using System.ComponentModel.DataAnnotations;

public class RegisterModel
{
    [Required(ErrorMessage = "Введите логин")]
    [Length(3, 15, ErrorMessage = "Логин должен содержать от 3 до 15 символов")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Введите пароль")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!#%&*?])[A-Za-z\\d@$!#%&*?]{5,15}$",
    ErrorMessage = "Пароль должен содержать латинские заглавные и строчные буквы, цифры и специальные знаки @$!#%&*?")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Подтвердите пароль")]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = string.Empty;
}