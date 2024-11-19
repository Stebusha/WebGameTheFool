namespace BlazorCardGame.Models;

using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required(ErrorMessage = "Введите логин")]
    public string Login { get; set; } = string.Empty;
    [Required(ErrorMessage = "Введите пароль")]
    public string Password { get; set; } = string.Empty;

    public LoginModel(string _login, string _password)
    {
        Login = _login;
        Password = _password;
    }
}