using System.ComponentModel.DataAnnotations;
using BlazorCardGame.Enums;

namespace BlazorCardGame.DataMangerAPI.Entities;

public class ApplicationUser
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите логин")]
    [StringLength(20)]
    public string? Login { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(20)]
    public string? Password { get; set; }

    // [Display(Name = "Запомнить?")]
    // public bool RememberMe { get; set; }
    public DateTime LastEnterTime { get; set; } = DateTime.Now;

    [EnumDataType(typeof(PlayerType))]
    public PlayerType PlayerType { get; set; } 
    public bool IsAttack { get; set; }

    // public ApplicationUser(string _login, string _password)
    // {
    //     Login = _login;
    //     Password = _password;
    //     LastEnterTime = DateTime.Now;

    //     foreach (var name in Enum.GetNames(typeof(AINames)))
    //     {
    //         if (Login == name)
    //         {
    //             PlayerType = PlayerType.AI;
    //             break;
    //         }
    //     }

    //     if (PlayerType != PlayerType.AI)
    //     {
    //         PlayerType = PlayerType.Human;
    //     }
    // }
}