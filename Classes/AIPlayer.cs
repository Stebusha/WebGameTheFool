namespace TheFool;
public class AIPlayer : IPlayer
{
    private const int REQUIRED_CARDS_COUNT = 6;
    PlayerHand _playerHand = new PlayerHand();
    public string Name { get; set; } = "Бот";
    public int TurnNumber { get; set; }
    public bool Taken { get; set; }
    public bool IsFool { get; set; }

    public AIPlayer() { }
    public AIPlayer(string _name, bool _fool)
    {
        Name = _name;
        IsFool = _fool;
    }

    //return cards in hand
    public List<Card> GetCards() => _playerHand.cards;

    //draw cards from deck
    public void RefillHand(Deck deck)
    {
        if (_playerHand.cards.Count == 0)
        {
            _playerHand.cards = deck.DrawCards(REQUIRED_CARDS_COUNT);
            _playerHand.Sort();
        }
        else if (_playerHand.cards.Count < REQUIRED_CARDS_COUNT)
        {
            _playerHand.cards.AddRange(deck.DrawCards(REQUIRED_CARDS_COUNT - _playerHand.cards.Count));
            _playerHand.Sort();
        }
    }

    //check exist cards for attack
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

        if (CanBeAttacking(_playerHand.cards, gameTable))
        {
            if (gameTable.Length() == 0)
            {
                return _playerHand.cards;
            }

            foreach (var card in _playerHand.cards)
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
        bool Attacking = CanBeAttacking(_playerHand.cards, gameTable);
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
                _playerHand.RemoveCardFromHand(attackingCard);
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

        foreach (var card in _playerHand.cards)
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

        foreach (var card in _playerHand.cards)
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
            _playerHand.RemoveCardFromHand(defendingCard);
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
        _playerHand.cards.AddRange(onTableCards);
        _playerHand.Sort();

        if (_playerHand.cards.Count != 0)
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