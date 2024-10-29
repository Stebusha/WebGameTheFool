namespace TheFool;
public class ScoreTable
{
    private Dictionary<string, int> _scores = new Dictionary<string, int>();
    private Dictionary<string, int> _fools = new Dictionary<string, int>();
    private string _pathScores = "C:/Users/МиненковаНА/Projects/TheFool/Scores/scores.txt";
    private string _pathFools = "C:/Users/МиненковаНА/Projects/TheFool/Scores/fools.txt";

    public ScoreTable()
    {
        LoadDataFromFile(_pathScores, ref _scores);
        LoadDataFromFile(_pathFools, ref _fools);
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

    //display score table in the console
    public void DisplayScores()
    {
        Console.WriteLine("\nТаблица рекордов:");

        foreach (var pair in _scores)
        {
            Console.WriteLine("{0}: \t{1}", pair.Key, pair.Value);
        }
    }

    //display fool table in the console
    public void DisplayFools()
    {
        Console.WriteLine("\nТаблица дураков:");

        foreach (var pair in _fools)
        {
            Console.WriteLine("{0}: \t{1}", pair.Key, pair.Value);
        }
    }

    //check exist name in score table
    public bool IsNameExistInScores(string? name)
    {
        if (name != null)
        {
            return _scores.ContainsKey(name) ? true : false;
        }

        return false;
    }

    //check exist name in fool table
    public bool IsNameExistInFools(string? name)
    {
        if (name != null)
        {
            return _fools.ContainsKey(name) ? true : false;
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

                if (_scores.TryGetValue(name, out value))
                {
                    value++;
                    _scores[name] = value;
                }
            }
            else
            {
                _scores.Add(name, score);
            }

            SaveDataToFile(_pathScores, _scores);
        }

    }

    //add fool to fools table, if name exist - update score of existing fool
    public void AddFool(string? name, int score)
    {
        if (name != null)
        {
            if (IsNameExistInFools(name))
            {
                int value;

                if (_fools.TryGetValue(name, out value))
                {
                    value++;
                    _fools[name] = value;
                }
            }
            else
            {
                _fools.Add(name, score);
            }

            SaveDataToFile(_pathFools, _fools);
        }
    }
}