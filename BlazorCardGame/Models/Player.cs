namespace BlazorCardGame.Models;
public class Player
{
    private const int REQUIRED_CARDS_COUNT = 6;
    public List<Card> inHand { get; set; } = new List<Card>();
    public PlayerType playerType { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TurnNumber { get; set; }
    public bool Taken { get; set; }
    public bool IsFool { get; set; }


    public void Sort()
    {
        //sort all cards in hand
        inHand = inHand.OrderBy(c => c.Rank).ToList();

        List<Card> trumpCards = new List<Card>();

        //remember trump cards
        foreach (var card in inHand)
        {
            if (card.Suit == Deck.s_trumpSuit)
            {
                trumpCards.Add(card);
            }
        }

        if (trumpCards != null)
        {
            //remove trump cards from hands
            foreach (var trump in trumpCards)
            {
                inHand.Remove(trump);
            }

            //sort trumps
            trumpCards = trumpCards.OrderBy(t => t.Rank).ToList();

            //add sorted trumps to hand
            foreach (var trump in trumpCards)
            {
                inHand.Add(trump);
            }
        }
    }
    public void RefillHand(Deck deck)
    {
        if (inHand.Count == 0)
        {
            inHand = deck.DrawCards(REQUIRED_CARDS_COUNT);
            Sort();
        }
        else if (inHand.Count < REQUIRED_CARDS_COUNT)
        {
            inHand.AddRange(deck.DrawCards(REQUIRED_CARDS_COUNT - inHand.Count));
            Sort();
        }
    }
}
