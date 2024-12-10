using System.ComponentModel.DataAnnotations;
using BlazorCardGame.Enums;

namespace BlazorCardGame.DataMangerAPI.Entities;

public class ApplicationUser
{
    public int Id { get; set; }
    public int FoolGameId { get; set; }
    public FoolGame FoolGame { get; set; } = null!;

    [Required(ErrorMessage = "Введите логин")]
    [StringLength(20)]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(20)]
    public string Password { get; set; } = null!;

    // [Display(Name = "Запомнить?")]
    // public bool RememberMe { get; set; }
    public DateTime? LastEnterTime
    {
        get { return LastEnterTime; }
        set { LastEnterTime = DateTime.Now; }
    }

    [EnumDataType(typeof(PlayerType))]
    public PlayerType PlayerType
    {
        get { return PlayerType; }
        set
        {
            foreach (var name in Enum.GetNames(typeof(AINames)))
            {
                if (Login == name)
                {
                    PlayerType = PlayerType.AI;
                    break;
                }
            }

            if (PlayerType != PlayerType.AI)
            {
                PlayerType = PlayerType.Human;
            }
        }
    }
    public bool IsAttack { get; set; }
}