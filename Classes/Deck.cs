namespace TheFool;
public class Deck
{
    private const int MAX_CARD_AMOUNT = 36;
    private List<Card> _cards;
    public static SuitType s_trumpSuit = SuitType.Clubs;
    public int CardsAmount { get; private set; }

    public Deck()
    {
        CardsAmount = MAX_CARD_AMOUNT;
        _cards = new List<Card>(CardsAmount);

        foreach (var suit in (SuitType[])Enum.GetValues(typeof(SuitType)))
        {
            foreach (var rank in (RankType[])Enum.GetValues(typeof(RankType)))
            {
                Card cardToCreate = new Card(suit, rank);
                _cards.Add(cardToCreate);
            }
        }
    }

    //return trump suit
    public SuitType GetTrumpSuit() => s_trumpSuit;

    //return trump suit unicode char string
    public string GetTrumpSuitName() => s_trumpSuit switch
    {
        SuitType.Clubs => "♣",
        SuitType.Hearts => "♥",
        SuitType.Spades => "♠",
        SuitType.Diams => "♦",
        _ => "Suit Not Found"
    };

    //shuffle cards on the deck
    public void Shuffle()
    {
        Random _random = new Random();
        _cards.Sort((a, b) => _random.Next(-2, 2));
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

    //set trump - first card of deck, moves trump to the end of the deck
    public void Trump()
    {
        Card trumpCard;
        trumpCard = _cards.First();
        _cards.RemoveAt(0);
        _cards.Add(trumpCard);
        s_trumpSuit = trumpCard.Suit;
    }
}