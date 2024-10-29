namespace TheFool;
public class Card
{
    public SuitType Suit { get; set; }
    public RankType Rank { get; set; }

    public Card() { }
    public Card(SuitType _suit, RankType _rank)
    {
        Suit = _suit;
        Rank = _rank;
    }

    //return rank string for card output based on RankType
    private string GetRankName(RankType rank) => rank switch
    {
        RankType.Six   => "6",
        RankType.Seven => "7",
        RankType.Eight => "8",
        RankType.Nine  => "9",
        RankType.Ten   => "10",
        RankType.Jack  => "J",
        RankType.Queen => "Q",
        RankType.King  => "K",
        RankType.Ace   => "A",
        _ => "Rank Not Found"
    };

    //return suit unicode char string for card output based on SuitType 
    private string GetSuitName(SuitType suit) => suit switch
    {
        SuitType.Clubs  => "♣",
        SuitType.Hearts => "♥",
        SuitType.Spades => "♠",
        SuitType.Diams  => "♦",
        _ => "Suit Not Found"
    };

    //card output
    public override string ToString() => $"{GetRankName(Rank)}{GetSuitName(Suit)}";

    //ovveride operators
    public static bool operator ==(Card card1, Card card2)
    {
        return card1.Suit == card2.Suit && card1.Rank == card2.Rank;
    }

    public static bool operator !=(Card card1, Card card2)
    {
        return !(card1 == card2);
    }

    public static bool operator >(Card card1, Card card2)
    {
        //same suit
        if (card1.Suit == card2.Suit)
        {
            return card1.Rank > card2.Rank;
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