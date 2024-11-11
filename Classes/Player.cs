namespace TheFool;
public class Player : IPlayer
{
    private const int REQUIRED_CARDS_COUNT = 6;
    PlayerHand playerHand = new PlayerHand();
    public int TurnNumber { get; set; }
    public bool Taken { get; set; }
    public string Name { get; set; }
    public bool IsFool { get; set; }

    public Player()
    {
        Console.WriteLine("Введите имя: ");
        string? temp = Console.ReadLine();

        while (temp == null)
        {
            Console.WriteLine("Введите имя: ");
        }

        Name = temp;
    }

    public Player(string _name, bool _fool)
    {
        Name = _name;
        IsFool = _fool;
    }

    //return cards in hand
    public List<Card> GetCards() => playerHand.cards;

    //draw cards from deck
    public void RefillHand(Deck deck)
    {
        if (playerHand.cards.Count == 0)
        {
            playerHand.cards = deck.DrawCards(REQUIRED_CARDS_COUNT);
            playerHand.Sort();
        }
        else if (playerHand.cards.Count < REQUIRED_CARDS_COUNT)
        {
            playerHand.cards.AddRange(deck.DrawCards(REQUIRED_CARDS_COUNT - playerHand.cards.Count));
            playerHand.Sort();
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

        if (CanBeAttacking(playerHand.cards, gameTable))
        {
            if (gameTable.Length() == 0)
            {
                return playerHand.cards;
            }

            foreach (var card in playerHand.cards)
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

    //attack chosen card
    public Card Attack(Table gameTable)
    {
        bool isAttacking = CanBeAttacking(playerHand.cards, gameTable);
        Card attackingCard = new Card();

        if (isAttacking)
        {
            List<Card> attackingCards = GetCardsForAttack(gameTable);

            if (attackingCards.Count != 0)
            {
                ToStringFor(playerHand.cards, attackingCards);
                Console.WriteLine("\nВыберите порядковый номер карты, которой хотите походить: ");

                bool settingNumber = false;

                while (!settingNumber)
                {
                    string? number = Console.ReadLine();

                    if (int.TryParse(number, out var index))
                    {
                        if ((index - 1) >= 0 && (index - 1) < attackingCards.Count)
                        {
                            settingNumber = true;
                            attackingCard = attackingCards[index - 1];

                            Console.WriteLine($"\nВы походили картой: {attackingCard}");

                            gameTable.AddCardToTable(attackingCard);
                            playerHand.RemoveCardFromHand(attackingCard);
                            attackingCards.Remove(attackingCard);
                        }
                        else
                        {
                            Console.WriteLine("Нет такого номера. Введите порядковый номер повторно: ");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ввод .Введите порядковый номер повторно: ");
                    }
                }
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

        foreach (var card in playerHand.cards)
        {
            if (card > attackingCard)
            {
                return true;
            }
        }

        return false;

    }

    //return cards for defense from attacking card
    private List<Card> GetCardsforDefense(Card attackingCard, Table gameTable)
    {
        List<Card> defenseCards = new List<Card>();

        if (gameTable.Length() != 0)
        {
            foreach (var card in playerHand.cards)
            {
                if (card > attackingCard)
                {
                    defenseCards.Add(card);
                }
            }
        }

        return defenseCards;
    }

    //return chosen card to defend
    private Card GetCardToDefend(Card attackingCard, Table gameTable)
    {
        Card cardToDefend = new Card();
        List<Card> defendingCards = GetCardsforDefense(attackingCard, gameTable);

        if (defendingCards.Count != 0)
        {
            ToStringFor(playerHand.cards, defendingCards);
            Console.WriteLine("\nВыберите порядковый номер карты, которой хотите отбиться: ");

            bool settingNumber = false;

            while (!settingNumber)
            {
                string? number = Console.ReadLine();

                if (int.TryParse(number, out var index))
                {
                    if ((index - 1) >= 0 && (index - 1) < defendingCards.Count)
                    {
                        settingNumber = true;
                        cardToDefend = defendingCards[index - 1];
                    }
                    else
                    {
                        Console.WriteLine("Нет такого номера. Введите порядковый номер повторно: ");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ввод .Введите порядковый номер повторно: ");
                }
            }
        }

        return cardToDefend;
    }

    //defend
    public void Defend(Card attackingCard, Table gameTable)
    {
        bool beaten = CanBeBeaten(attackingCard, gameTable);
        Card defendingCard = GetCardToDefend(attackingCard, gameTable);

        if (beaten)
        {
            Console.WriteLine($"\nВы отбились картой: {defendingCard}");
            gameTable.AddCardToTable(defendingCard);
            playerHand.RemoveCardFromHand(defendingCard);
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
        playerHand.cards.AddRange(onTableCards);
        playerHand.Sort();

        Console.WriteLine($"\nВы взяли карты :\n {ToString(playerHand.cards)}");
    }

    //output cards for console
    public string ToString(List<Card> cards)
    {
        string cardDrawnString = string.Empty;
        cardDrawnString = $"\nКарты игрока {Name}: \n";

        if (cards.Count > 6)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards.Count % 6 > 0)
                {
                    if (i != 0 && i % 6 == 0)
                    {
                        cardDrawnString += "\n\n";
                    }
                }

                Card tempCard = cards[i];
                cardDrawnString += $"[{i + 1}] - {tempCard}\t";
            }
        }
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card tempCard = cards[i];
                cardDrawnString += $"[{i + 1}] - {tempCard}\t";
            }
        }

        return cardDrawnString;
    }

    //output cards to console for defend or attack
    public void ToStringFor(List<Card> cards, List<Card> forSomethingCards)
    {
        Console.WriteLine($"\nКарты игрока {Name}: \n");

        if (cards.Count > 6)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards.Count % 6 > 0)
                {
                    if (i != 0 && i % 6 == 0)
                    {
                        Console.Write("\n\n");
                    }
                }

                Card tempCard = cards[i];

                int counter = 0;
                bool InConsole = false;

                foreach (var card in forSomethingCards)
                {
                    counter++;

                    if (card == tempCard)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"[{counter}] - {tempCard}\t");
                        Console.ResetColor();
                        InConsole = true;
                    }
                }

                if (!InConsole)
                {
                    Console.Write($"{tempCard}\t");
                }
            }
        }
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card tempCard = cards[i];

                int counter = 0;
                bool InConsole = false;

                foreach (var card in forSomethingCards)
                {
                    counter++;

                    if (card == tempCard)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"[{counter}] - {tempCard}\t");
                        InConsole = true;
                        Console.ResetColor();
                    }
                }

                if (!InConsole)
                {
                    Console.Write($"{tempCard}\t");
                }
            }
        }

        Console.WriteLine();
    }
}