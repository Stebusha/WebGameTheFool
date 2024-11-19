namespace BlazorCardGame.Models;

public class AIPlayer{
    private const int REQUIRED_CARDS_COUNT = 6;
    public List<Card> inHand { get; set; } = new List<Card>();
    public PlayerType playerType { get; set; } = PlayerType.AI;
    public string Name { get; set; } = string.Empty;
    public int TurnNumber { get; set; }
    public bool Taken { get; set; }
    public bool IsFool { get; set; }
    public bool IsAttack { get; set; } = true;

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

        foreach (var card in inHand)
        {
            if (!card.IsPlayable)
            {
                card.IsPlayable = true;
            }

            card.IsSelected = false;
        }
    }

    private bool CanBeAttacking(List<Card> cards, Table gameTable)
    {
        if (gameTable.Length() == 0)
        {
            return true;
        }

        foreach (var card in cards)
        {
            for (int i = 0; i < gameTable.Length(); i++)
            {
                if (card.Rank == gameTable.GetCard(i).Rank)
                {
                    return true;
                }
            }
        }

        return false;

    }

    //return cards for attack
    public List<Card> GetCardsForAttack(Table gameTable)
    {
        List<Card> cardsForAttack = new List<Card>();

        if (CanBeAttacking(inHand, gameTable))
        {
            if (gameTable.Length() == 0)
            {
                return inHand;
            }

            foreach (var card in inHand)
            {
                for (int i = 0; i < gameTable.Length(); i++)
                {
                    if (card.Rank == gameTable.GetCard(i).Rank)
                    {
                        cardsForAttack.Add(card);
                    }
                }
            }
        }

        cardsForAttack = cardsForAttack.Distinct().ToList();

        return cardsForAttack;
    }

    //attack card based on decision
    public Card Attack(Table gameTable)
    {
        bool Attacking = CanBeAttacking(inHand, gameTable);
        Card attackingCard = new Card();

        if (Attacking)
        {
            List<Card> attackingCards = GetCardsForAttack(gameTable);

            if (attackingCards.Count != 0)
            {
                int index = MakeDecision();
                attackingCard = attackingCards[index];
                Console.WriteLine($"\n{Name} походил картой: {attackingCard}");
                gameTable.AddCardToTable(attackingCard);
                //fixed
                //first card delete before defend, comparison with next card -> bug defend 
                inHand.Remove(attackingCard);
            }
        }

        return attackingCard;
    }

    //check attacking card can be beaten
    private bool CanBeBeaten(Card attackingCard, Table gameTable)
    {
        if (gameTable.Length() == 0)
        {
            return false;
        }

        foreach (var card in inHand)
        {
            if (card > attackingCard)
            {
                return true;
            }
        }

        return false;
    }

    //return  card to defend based on decision
    private Card GetCardToDefend(Card attackingCard)
    {
        Card cardToDefend = new Card();

        foreach (var card in inHand)
        {
            if (card > attackingCard)
            {
                cardToDefend = card;
                break;
            }
        }

        return cardToDefend;
    }

    //defend
    public void Defend(Card attackingCard, Table gameTable)
    {
        bool beaten = CanBeBeaten(attackingCard, gameTable);
        Card defendingCard = GetCardToDefend(attackingCard);

        if (beaten)
        {
            Console.WriteLine($"{Name} отбился картой: {defendingCard}");
            gameTable.AddCardToTable(defendingCard);
            inHand.Remove(defendingCard);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nНечем отбиться");
            Console.ResetColor();
            TakeAllCards(gameTable);
        }
    }

    //taken all cards from the game table, set property Taken
    public void TakeAllCards(Table gameTable)
    {
        Taken = true;
        List<Card> onTableCards = gameTable.TakeCardsFromTable();
        inHand.AddRange(onTableCards);
        Sort();

        if (inHand.Count != 0)
        {
            Console.WriteLine($"\n{Name} взял карты");
        }
    }

    //output cards for console
    public string ToString(List<Card> cards)
    {
        string cardDrawnString = string.Empty;
        cardDrawnString = $"\nКарты игрока {Name}\n";

        for (int i = 0; i < cards.Count; i++)
        {
            Card tempCard = cards[i];
            cardDrawnString += $"{tempCard}\t";
        }

        return cardDrawnString;
    }

    //return min card
    protected virtual int MakeDecision() => 0;
}