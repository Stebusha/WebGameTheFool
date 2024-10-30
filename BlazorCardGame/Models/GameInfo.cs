namespace BlazorCardGame.Models;
public class GameInfo
{
    public string Name { get; set; } = string.Empty;
    public int PlayerCount { get; set; } = 1;
    public int BotPlayerCount { get; set; } = 1;
    public List<Player> players = new List<Player>();
    public Table gameTable = new Table();
    public Deck deck = new Deck();
    public GameState gameState { get; set; }
}
