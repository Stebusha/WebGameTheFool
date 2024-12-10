namespace BlazorCardGame.Models;
using BlazorCardGame.Enums;
public class GameInfo
{
    public string Name { get; set; } = string.Empty;
    public int PlayerCount { get; set; } = 1;
    public int BotPlayerCount { get; set; } = 1;
    public Player player = new Player();
    public AIPlayer opponent = new AIPlayer();
    public Table gameTable = new Table();
    public Deck deck = new Deck();
    public GameState gameState { get; set; } = GameState.JustStarted;
}
