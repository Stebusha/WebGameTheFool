namespace BlazorCardGame.Models;
using BlazorCardGame.Enums;
using Microsoft.EntityFrameworkCore.Storage;

public class Card
{
    public SuitType Suit { get; set; }
    public RankType Rank { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPlayable { get; set; } = false;
    public bool IsSelected { get; set; } = false;
    public Card() { }
    public Card(SuitType _suit, RankType _rank)
    {
        Suit = _suit;
        Rank = _rank;
        ImageUrl = $"/Images//Cards/{Deck.s_style}/{GetRankName(_rank)}{GetSuitName(_suit)}.png";
        IsPlayable = true;
    }

    //return rank string for card output based on RankType
    private string GetRankName(RankType rank) => rank switch
    {
        RankType.Two => "Extra/2",
        RankType.Three => "Extra/3",
        RankType.Four => "Extra/4",
        RankType.Five => "Extra/5",
        RankType.Six => "6",
        RankType.Seven => "7",
        RankType.Eight => "8",
        RankType.Nine => "9",
        RankType.Ten => "10",
        RankType.Jack => "J",
        RankType.Queen => "Q",
        RankType.King => "K",
        RankType.Ace => "A",
        RankType.Joker => "Extra/joker_",
        _ => "Rank Not Found"
    };

    //return suit unicode char string for card output based on SuitType 
    private string GetSuitName(SuitType suit) => suit switch
    {
        SuitType.Clubs => "C",
        SuitType.Hearts => "H",
        SuitType.Spades => "S",
        SuitType.Diams => "D",
        SuitType.Black => "male",
        SuitType.Red => "lady",
        _ => "Suit Not Found"
    };

    //card output
    public override string ToString() => $"{GetRankName(Rank)}{GetSuitName(Suit)}";

    //ovveride operators
    public static bool operator ==(Card card1, Card card2)
    {
        return card1.Suit == card2.Suit && card1.Rank == card2.Rank;
    }

    public static bool operator !=(Card card1, Card card2) => !(card1 == card2);

    public static bool operator >(Card card1, Card card2)
    {
        //same suit
        if (card1.Suit == card2.Suit)
        {
            return card1.Rank > card2.Rank;
        }
        //first joker
        else if (card1.Rank == RankType.Joker && card2.Rank != RankType.Joker)
        {
            return true;
        }
        //second joker
        else if (card1.Rank != RankType.Joker && card2.Rank == RankType.Joker)
        {
            return false;
        }
        // first red, second black
        else if (card1.Suit == SuitType.Red && card2.Suit == SuitType.Black)
        {
            return true;
        }
        // first black, second red
        else if (card1.Suit == SuitType.Black && card2.Suit == SuitType.Red)
        {
            return false;
        }
        //first trump
        else if (card1.Suit == Deck.s_trumpSuit && card2.Suit != Deck.s_trumpSuit)
        {
            return true;
        }
        //second trump
        else if (card1.Suit != Deck.s_trumpSuit && card2.Suit == Deck.s_trumpSuit)
        {
            return false;
        }
        //all another cases
        else
        {
            return false;
        }
    }

    public static bool operator <(Card card1, Card card2)
    {
        //same suit
        if (card1.Suit == card2.Suit)
        {
            return card1.Rank < card2.Rank;
        }
        //first joker
        else if (card1.Rank == RankType.Joker && card2.Rank != RankType.Joker)
        {
            return false;
        }
        //second joker
        else if (card1.Rank != RankType.Joker && card2.Rank == RankType.Joker)
        {
            return true;
        }
        // first red, second black
        else if (card1.Suit == SuitType.Red && card2.Suit == SuitType.Black)
        {
            return false;
        }
        // first black, second red
        else if (card1.Suit == SuitType.Black && card2.Suit == SuitType.Red)
        {
            return true;
        }
        //first trump
        else if (card1.Suit == Deck.s_trumpSuit && card2.Suit != Deck.s_trumpSuit)
        {
            return false;
        }
        //second trump
        else if (card1.Suit != Deck.s_trumpSuit && card2.Suit == Deck.s_trumpSuit)
        {
            return true;
        }
        //all another cases
        else
        {
            return false;
        }
    }

    public static bool operator >=(Card card1, Card card2)
    {
        //same rank, suit
        if (card1.Suit == card2.Suit && card1.Rank == card2.Rank)
        {
            return true;
        }
        //first joker
        else if (card1.Rank == RankType.Joker && card2.Rank != RankType.Joker)
        {
            return true;
        }
        //second joker
        else if (card1.Rank != RankType.Joker && card2.Rank == RankType.Joker)
        {
            return false;
        }
        // first red, second black
        else if (card1.Suit == SuitType.Red && card2.Suit == SuitType.Black)
        {
            return true;
        }
        // first black, second red
        else if (card1.Suit == SuitType.Black && card2.Suit == SuitType.Red)
        {
            return false;
        }
        //first trump
        else if (card1.Suit == Deck.s_trumpSuit && card2.Suit != Deck.s_trumpSuit)
        {
            return true;
        }
        //second trump
        else if (card2.Suit == Deck.s_trumpSuit && card1.Suit != Deck.s_trumpSuit)
        {
            return false;
        }
        //not the same suit, not trump
        else if (card1.Suit != card2.Suit)
        {
            return false;
        }
        //same suit for trumps
        else if (card1.Suit == card2.Suit && card1.Suit == Deck.s_trumpSuit)
        {
            return card1.Rank > card2.Rank;
        }
        //all another cases
        else
        {
            return card1.Rank > card2.Rank;
        }
    }

    public static bool operator <=(Card card1, Card card2)
    {
        //same rank, suit
        if (card1.Suit == card2.Suit && card1.Rank == card2.Rank)
        {
            return true;
        }
        //first joker
        else if (card1.Rank == RankType.Joker && card2.Rank != RankType.Joker)
        {
            return false;
        }
        //second joker
        else if (card1.Rank != RankType.Joker && card2.Rank == RankType.Joker)
        {
            return true;
        }
        // first red, second black
        else if (card1.Suit == SuitType.Red && card2.Suit == SuitType.Black)
        {
            return false;
        }
        // first black, second red
        else if (card1.Suit == SuitType.Black && card2.Suit == SuitType.Red)
        {
            return true;
        }
        //first trump
        else if (card1.Suit == Deck.s_trumpSuit && card2.Suit != Deck.s_trumpSuit)
        {
            return false;
        }
        //second trump
        else if (card2.Suit == Deck.s_trumpSuit && card1.Suit != Deck.s_trumpSuit)
        {
            return true;
        }
        //not the same suit
        else if (card1.Suit != card2.Suit)
        {
            return false;
        }
        //same suit for trumps
        else if (card1.Suit == card2.Suit && card1.Suit == Deck.s_trumpSuit)
        {
            return card1.Rank < card2.Rank;
        }
        //all another cases
        else
        {
            return card1.Rank < card2.Rank;
        }
    }

    // override object.Equals
    public override bool Equals(object? card)
    {
        var c = card ?? throw new ArgumentNullException(nameof(card));
        return this == (Card)card;
    }
    // override object.GetHashCode

    public override int GetHashCode() => 9 * (int)Suit + (int)Rank;
}
