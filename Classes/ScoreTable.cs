using System;
using System.IO;
using System.Collections.Generic;

namespace TheFool
{
    public class ScoreTable{
        
        private Dictionary<string,int> scores = new Dictionary<string, int>();
        private Dictionary<string,int> fools = new Dictionary<string, int>();
        private string pathScores = "C:/Users/МиненковаНА/Projects/TheFool/Scores/scores.txt";
        private string pathFools = "C:/Users/МиненковаНА/Projects/TheFool/Scores/fools.txt";
        
        //load score table from files
        private void LoadDataFromFile(string path, ref Dictionary<string,int> dataDictionary){
            if(File.Exists(path)){
                using(StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while((line = sr.ReadLine())!=null){
                        string[] parts = line.Split(',');
                        dataDictionary.Add(parts[0],int.Parse(parts[1]));
                    }
                    dataDictionary = dataDictionary.OrderByDescending(pair=>pair.Value).ToDictionary();
                }
            }
        }
        public ScoreTable(){
            LoadDataFromFile(pathScores,ref scores);
            LoadDataFromFile(pathFools,ref fools);
        }
        
        //save data to file
        private void SaveDataToFile(string path, Dictionary<string,int> dataDictionary){
            dataDictionary = dataDictionary.OrderByDescending(pair=>pair.Value).ToDictionary();
            using(StreamWriter sw = new StreamWriter(path))
            {
                foreach(var pair in dataDictionary)
                {
                    sw.WriteLine("{0},{1}",pair.Key,pair.Value);
                }
            }
        }

        //display score table in the console
        public void DisplayScores(){
            Console.WriteLine("\nТаблица рекордов:");
            foreach(var pair in scores){
                Console.WriteLine("{0}: \t{1}", pair.Key,pair.Value);
            }
        }

        //display fool table in the console
        public void DisplayFools(){
            Console.WriteLine("\nТаблица дураков:");
            foreach(var pair in fools){
                Console.WriteLine("{0}: \t{1}", pair.Key,pair.Value);
            }
        }
        
        //check exist name in score table
        public bool IsNameExistInScores(string name){
            if(scores.ContainsKey(name)){
                return true;
            }
            else{
                return false;
            }
        }
        //check exist name in fool table
        public bool IsNameExistInFools(string name){
            if(fools.ContainsKey(name)){
                return true;
            }
            else{
                return false;
            }
        }
        
        //add winner to score table, if name exist - update score of existing winner
        public void AddScore(string name, int score){
            if(IsNameExistInScores(name)){
                int value;
                if(scores.TryGetValue(name, out value)){
                    value++;
                    scores[name]=value;
                }
            }
            else{
                scores.Add(name,score);
            }
            SaveDataToFile(pathScores,scores);
        }

        //add fool to fools table, if name exist - update score of existing fool
        public void AddFool(string name, int score){
            if(IsNameExistInFools(name)){
                int value;
                if(fools.TryGetValue(name, out value)){
                    value++;
                    fools[name]=value;
                }
            }
            else{
                fools.Add(name,score);
            }
            SaveDataToFile(pathFools,fools);
        }
    }
}