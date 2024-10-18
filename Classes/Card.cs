using System;

namespace TheFool;
public class Card{
    public SuitType Suit{get; set;} 
    public RankType Rank{get;set;}       
    public Card(){}
    public Card (SuitType _suit, RankType _rank){
        Suit = _suit;
        Rank = _rank;
    }
    
    //card output
    public override string ToString() => $"{Rank} of {Suit}";
    
    //ovveride operators
    public static bool operator ==(Card card1, Card card2){
        return (card1.Suit==card2.Suit &&card1.Rank==card2.Rank);
    }
    public static bool operator !=(Card card1, Card card2){
        return !(card1==card2);
    }
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
    public override bool Equals(object card)
    {
        return this == (Card)card;
    }
    
        // override object.GetHashCode
    public override int GetHashCode()
    {
        return 9 *(int)Suit+(int)Rank;
    }
    
}
