namespace BlazorWebApp.Models
{
    public class Card{
        public SuitType Suit{get; set;} 
        public RankType Rank{get;set;}

        //public Guid Id{get; private set;}
        public string ImageUrl{get; set;}
        public bool IsPlayable{get; set;}
        public Card(){}
        public Card (SuitType _suit, RankType _rank){
            Suit = _suit;
            Rank = _rank;
            ImageUrl=$"Images/Cards/{CreateCardName(_suit,_rank)}";
            IsPlayable = false;
        }

        //created name for card image url
        private string CreateCardName(SuitType suit,RankType rank){
            string name = string.Empty;
            switch (rank){
                case RankType.Six:
                    name+="6";
                    break;
                case RankType.Seven:
                    name+="7";
                    break;
                case RankType.Eight:
                    name+="8";
                    break;
                case RankType.Nine:
                    name+="9";
                    break;
                case RankType.Ten:
                    name+="10";
                    break;
                case RankType.Jack:
                    name+="J";
                    break;
                case RankType.Queen:
                    name+="Q";
                    break;
                case RankType.King:
                    name+="K";
                    break;
                case RankType.Ace:
                    name+="A";
                    break;
            }
            switch (suit){
                case SuitType.Clubs: 
                    name+="C";
                    break;
                case SuitType.Hearts: 
                    name+="H";
                    break;
                case SuitType.Diams: 
                    name+="D";
                    break;
                case SuitType.Spades: 
                    name+="S";
                    break;
            }
            return name+".png";
        }
        //card output
        //public override string ToString() => $"{Rank} of {Suit}";
    
        //ovveride operators
        public static bool operator ==(Card card1, Card card2) => card1.Suit==card2.Suit &&card1.Rank==card2.Rank;

        public static bool operator !=(Card card1, Card card2) => !(card1==card2);

        public static bool operator>(Card card1, Card card2){
            if(card1.Suit==card2.Suit){
                return card1.Rank>card2.Rank;
            }
            else if(card1.Suit==Deck.trumpSuit&&card2.Suit!=Deck.trumpSuit){
                return true;
            }
            else if(card1.Suit!=Deck.trumpSuit&&card2.Suit==Deck.trumpSuit){
                return false;
            }
            else{
                return false;
            }
        
        }

        public static bool operator <(Card card1, Card card2){
            if(card1.Suit==card2.Suit){
                return card1.Rank<card2.Rank;
            }
            else if(card1.Suit==Deck.trumpSuit&&card2.Suit!=Deck.trumpSuit){
                return false;
            }
            else if(card1.Suit!=Deck.trumpSuit&&card2.Suit==Deck.trumpSuit){
                return true;
            }
            else{
                return false;
            }
        }

        public static bool operator>=(Card card1, Card card2){
            if(card1.Suit==card2.Suit&&card1.Rank==card2.Rank){
                return true;
            }
            else if(card1.Suit==Deck.trumpSuit&&card2.Suit!=Deck.trumpSuit){
                return true;
            }
            else if(card2.Suit==Deck.trumpSuit&&card1.Suit!=Deck.trumpSuit){
                return false;
            }
            else if(card1.Suit!=card2.Suit){
                return false;
            }
            else if (card1.Suit==card2.Suit&&card1.Suit==Deck.trumpSuit){
                return card1.Rank>card2.Rank;
            }
            else{
                return card1.Rank>card2.Rank;
            }
        }

        public static bool operator <=(Card card1,Card card2){
            if(card1.Suit==card2.Suit&&card1.Rank==card2.Rank){
                return true;
            }
        
            else if(card1.Suit==Deck.trumpSuit&&card2.Suit!=Deck.trumpSuit){
                return false;
            }
            else if(card2.Suit==Deck.trumpSuit&&card1.Suit!=Deck.trumpSuit){
                return true;
            }
            else if(card1.Suit!=card2.Suit){
                return false;
            }
            else if (card1.Suit==card2.Suit&&card1.Suit==Deck.trumpSuit){
                return card1.Rank<card2.Rank;
            }
            else{
                return card1.Rank<card2.Rank;
            }
        }

        // override object.Equals
        public override bool Equals(object card) => this == (Card)card;
        // override object.GetHashCode
        public override int GetHashCode() => 9 *(int)Suit+(int)Rank;

    }
}