namespace BlazorCardGame.Models;

public class HistoModel
{
    public string? Name { get; set; }
    public int? Wins { get; set; }
    public int? Losses { get; set; }
    public int? Draws { get; set; }
    public int? Total { get; set; }

    public HistoModel(string _name, int? _wins, int? _losses, int? _draws, int? _total)
    {
        Name = _name;
        Wins = _wins;
        Draws = _draws;
        Losses = _losses;
        Total = _total;
    }
}