namespace TheFool;
public class PlayerHand
{
    public List<Card> cards = new List<Card>();

    //return card by index
    public Card GetCard(int index) => cards.ElementAt(index);

    //remove card from hand
    public void RemoveCardFromHand(Card card)
    {
        cards.Remove(card);
        Sort();
    }

    //sorted cards in hand by rank, trumps also sorted by rank in the end of hand
    public void Sort()
    {
        //sort all cards in hand
        cards = cards.OrderBy(c => c.Rank).ToList();

        List<Card> trumpCards = new List<Card>();

        //remember trump cards
        foreach (var card in cards)
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
                cards.Remove(trump);
            }

            //sort trumps
            trumpCards = trumpCards.OrderBy(t => t.Rank).ToList();

            //add sorted trumps to hand
            foreach (var trump in trumpCards)
            {
                cards.Add(trump);
            }
        }
    }
}

