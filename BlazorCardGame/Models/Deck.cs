namespace BlazorCardGame.Models
{
    public class Deck{
        private List<Card> cards;
        
        public static SuitType trumpSuit=SuitType.Clubs;
        public int CardsAmount{get; private set;}

        public SuitType GetTrumpSuit() => trumpSuit;

        public Deck(){
            CardsAmount = 36;
            cards = new List<Card>(CardsAmount);
            foreach(var suit in  (SuitType[])Enum.GetValues(typeof(SuitType))){
                foreach(var rank in (RankType[])Enum.GetValues(typeof(RankType))){
                    Card cardToCreate = new Card(suit,rank);
                    cards.Add(cardToCreate);
                }
            }
        }  
        
        //shuffle cards on the deck
        public void Shuffle(){
            Random _random = new Random();
            cards.Sort((a,b) => _random.Next(-2,2));
        }
        
        //return cards of the deck
        public List<Card> DrawCards(int count){
            List<Card> drawnCards = new List<Card>();
            if(CardsAmount!=0&&CardsAmount>=count){
                for(int i=0;i<count;i++){
                    drawnCards.Add(cards.First());
                    cards.RemoveAt(0);
                }
            }
            else if(CardsAmount!=0&&CardsAmount<count){
                for(int i=0;i<CardsAmount;i++){
                    drawnCards.Add(cards.First());
                    cards.RemoveAt(0);
                }
            }
            CardsAmount = cards.Count;
            return drawnCards;
        }
        
        //set trump - first card of deck, moves trump to the end of the deck
        public void Trump(){
            Card trumpCard;
            trumpCard = cards.First();
            cards.RemoveAt(0);
            cards.Add(trumpCard);
            trumpSuit = trumpCard.Suit;
        }
    }
}