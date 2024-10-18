namespace BlazorWebApp.Models
{
    public class ScoreTable{
        private Dictionary<string,int> scores = new Dictionary<string, int>();
        private Dictionary<string,int> fools = new Dictionary<string, int>();
        private string pathScores = "C:/Users/МиненковаНА/Projects/WebAppFool/Scores/scores.txt";
        private string pathFools = "C:/Users/МиненковаНА/Projects/WebAppFool/Scores/fools.txt";
    }
}