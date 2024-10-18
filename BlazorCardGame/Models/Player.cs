namespace BlazorWebApp.Models
{
    public class Player{
        public List<Card>? inHand{get;set;}
        public PlayerType playerType{get;set;}
        public string Name{get; set;} = string.Empty;
        public int TurnNumber{get;set;}
        public bool Taken{get;set;}
        public bool IsFool {get;set;}
    }
}