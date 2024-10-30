using BlazorCardGame.Models;

namespace BlazorCardGame.Services;

public class ScoresService
{
    private Dictionary<string, int> scores = new Dictionary<string, int>();
    private Dictionary<string, int> fools = new Dictionary<string, int>();
}
