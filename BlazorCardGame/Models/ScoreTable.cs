namespace BlazorCardGame.Models;

public class ScoreTable
{
    public Dictionary<string, int> scores = new Dictionary<string, int>();
    private string _pathScores = "scores.txt";

    public ScoreTable()
    {
        LoadDataFromFile(_pathScores, ref scores);
    }

    //load score table from files
    private void LoadDataFromFile(string path, ref Dictionary<string, int> dataDictionary)
    {
        if (File.Exists(path))
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string? line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    dataDictionary.Add(parts[0], int.Parse(parts[1]));
                }

                dataDictionary = dataDictionary.OrderByDescending(pair => pair.Value).ToDictionary();
            }
        }
    }

    //save data to file
    private void SaveDataToFile(string path, Dictionary<string, int> dataDictionary)
    {
        dataDictionary = dataDictionary.OrderByDescending(pair => pair.Value).ToDictionary();

        using (StreamWriter sw = new StreamWriter(path))
        {
            foreach (var pair in dataDictionary)
            {
                sw.WriteLine("{0},{1}", pair.Key, pair.Value);
            }
        }
    }

    //check exist name in score table
    public bool IsNameExistInScores(string? name)
    {
        if (name != null)
        {
            return scores.ContainsKey(name) ? true : false;
        }

        return false;
    }

    //add winner to score table, if name exist - update score of existing winner
    public void AddScore(string? name, int score)
    {
        if (name != null)
        {
            if (IsNameExistInScores(name))
            {
                int value;

                if (scores.TryGetValue(name, out value))
                {
                    value++;
                    scores[name] = value;
                }
            }
            else
            {
                scores.Add(name, score);
            }

            SaveDataToFile(_pathScores, scores);
        }

    }
}