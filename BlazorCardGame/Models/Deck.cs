using BlazorCardGame.Enums;

namespace BlazorCardGame.Models;
public class Deck
{
    private List<Card> _cards;
    public static SuitType s_trumpSuit = SuitType.Clubs;
    public static DeckStyle s_style = DeckStyle.Fantasy;
    public static string s_back = "Images/back_cyan.png";
    public int CardsAmount { get; private set; }
    public static bool UseExtra = false;
    public static string TrumpSuitImageURL
    {
        get => GetTrumpSuitURL();
    }

    public static Card s_trumpCard = new Card(SuitType.Clubs, RankType.Six);

    public Deck()
    {
        //cards amount 54
        if (UseExtra)
        {
            CardsAmount = Constants.MAX_CARD_AMOUNT_EXTENDED;
            _cards = new List<Card>(CardsAmount);

            for (int s = 0; s < 4; s++)
            {
                for (int r = 0; r < 13; r++)
                {
                    Card cardToCreate = new Card((SuitType)s, (RankType)r);
                    _cards.Add(cardToCreate);
                }
            }

            List<Card> jokers = [new Card(SuitType.Black, RankType.Joker), new Card(SuitType.Red, RankType.Joker)];
            _cards.AddRange(jokers);
        }
        //cards amount 36
        else
        {
            CardsAmount = Constants.MAX_CARD_AMOUNT_STANDARD;
            _cards = new List<Card>(CardsAmount);

            for (int s = 0; s < 4; s++)
            {
                for (int r = 4; r < 13; r++)
                {
                    Card cardToCreate = new Card((SuitType)s, (RankType)r);
                    _cards.Add(cardToCreate);
                }
            }
        }

    }

    //return trump suit
    public SuitType GetTrumpSuit() => s_trumpSuit;

    public Card GetTrumpCard() => s_trumpCard;

    //return trump suit unicode char string
    public string GetTrumpSuitName() => s_trumpSuit switch
    {
        SuitType.Clubs => "♣",
        SuitType.Hearts => "♥",
        SuitType.Spades => "♠",
        SuitType.Diams => "♦",
        _ => "Suit Not Found"
    };
    public void ChangeStyle(string selectedStyle)
    {
        foreach (var style in Enum.GetNames(typeof(DeckStyle)))
        {
            if (style == selectedStyle)
            {
                s_style = (DeckStyle)Enum.Parse(typeof(DeckStyle), style);
            }
        }
    }

    public List<Card> GetDeckCards() => _cards;
    private static string GetTrumpSuitURL() => s_trumpSuit switch
    {
        SuitType.Clubs => "club.png",
        SuitType.Hearts => "heart.png",
        SuitType.Spades => "spade.png",
        SuitType.Diams => "diam.png",
        _ => "Suit Not Found"
    };

    private bool CheckRetake()
    {
        Dictionary<SuitType, int> pairsPlayer = new();
        Dictionary<SuitType, int> pairsOpponent = new();

        for (int i = 1; i < 7; i++)
        {
            if (!pairsPlayer.ContainsKey(_cards.ElementAt(i).Suit))
            {
                pairsPlayer.Add(_cards.ElementAt(i).Suit, 1);
            }
            else
            {
                pairsPlayer[_cards.ElementAt(i).Suit]++;
            }
        }

        for (int i = 7; i < 13; i++)
        {
            if (!pairsOpponent.ContainsKey(_cards.ElementAt(i).Suit))
            {
                pairsOpponent.Add(_cards.ElementAt(i).Suit, 1);
            }
            else
            {
                pairsOpponent[_cards.ElementAt(i).Suit]++;
            }
        }

        foreach (var pair in pairsOpponent)
        {
            if (pair.Value >= 5)
            {
                return true;
            }
        }

        foreach (var pair in pairsPlayer)
        {
            if (pair.Value >= 5)
            {
                return true;
            }
        }

        return false;
    }

    //shuffle cards on the deck
    public void Shuffle()
    {
        Random _random = new Random();
        _cards.Sort((a, b) => _random.Next(-2, 2));

        while (CheckRetake())
        {
            _cards.Sort((a, b) => _random.Next(-2, 2));
        }
    }

    //return cards of the deck
    public List<Card> DrawCards(int count)
    {
        List<Card> drawnCards = new List<Card>();

        if (CardsAmount != 0 && CardsAmount >= count)
        {
            for (int i = 0; i < count; i++)
            {
                drawnCards.Add(_cards.First());
                _cards.RemoveAt(0);
            }
        }
        else if (CardsAmount != 0 && CardsAmount < count)
        {
            for (int i = 0; i < CardsAmount; i++)
            {
                drawnCards.Add(_cards.First());
                _cards.RemoveAt(0);
            }
        }

        CardsAmount = _cards.Count;

        return drawnCards;
    }

    public Card DrawCard()
    {
        Card drawnCard = new();

        if (CardsAmount != 0)
        {

            drawnCard = _cards.First();
            _cards.RemoveAt(0);

        }

        CardsAmount = _cards.Count;

        return drawnCard;
    }

    //set trump - first card of deck, moves trump to the end of the deck
    public void Trump()
    {
        Card trumpCard;
        trumpCard = _cards.First();

        if (UseExtra)
        {
            while (trumpCard.Rank == RankType.Joker || trumpCard.Rank == RankType.Ace)
            {
                _cards.RemoveAt(0);
                _cards.Add(trumpCard);
                trumpCard = _cards.First();
            }
        }
        else
        {
            while (trumpCard.Rank == RankType.Ace)
            {
                _cards.RemoveAt(0);
                _cards.Add(trumpCard);
                trumpCard = _cards.First();
            }
        }

        s_trumpCard = trumpCard;
        _cards.RemoveAt(0);
        _cards.Add(trumpCard);
        s_trumpSuit = trumpCard.Suit;
    }
}
