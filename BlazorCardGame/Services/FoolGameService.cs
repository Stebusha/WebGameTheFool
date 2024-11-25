using BlazorCardGame.Models;

namespace BlazorCardGame.Services;

public class FoolGameService
{
    private Dictionary<string, bool> _fools = new Dictionary<string, bool>();
    public Player Player { get; set; } = new Player();
    public AIPlayer Opponent { get; set; } = new AIPlayer();
    public Deck Deck { get; set; } = new Deck();
    public Table Table { get; set; } = new Table();
    public List<Card> AttackingCards { get; set; } = new List<Card>();
    public List<Card> DefendingCards { get; set; } = new List<Card>();
    public bool Repeat { get; set; } = false;
    public GameState gameState { get; set; } = GameState.Loading;
    public int CountOfGames { get; set; } = 0;
    public bool FirstTurn { get; set; } = false;
    public bool TurnFinished { get; set; } = false;

    public void BotTurn()
    {
        TurnFinished = false;

        Card attackingCard = new Card();
        Card defendingCard = new Card();

        while (!TurnFinished)
        {
            if (FirstTurn)
            {
                for (int i = 0; i < Constants.MAX_CARDS_TO_ATTACK - 1; i++)
                {
                    Console.WriteLine("\nНачало партии\n");
                    Console.WriteLine($"Козырная масть - {Deck.GetTrumpSuitName()}");

                    if (AttackingCards.Count == Constants.MAX_CARDS_TO_ATTACK - 1)
                    {
                        TurnFinished = true;
                        FirstTurn = false;
                        Console.WriteLine("\nКонец хода");
                        break;
                    }

                    //Taken or no cards for attack
                    if (Player.Taken || Opponent.GetCardsForAttack(Table).Count == 0)
                    {
                        TurnFinished = true;
                        FirstTurn = false;
                        break;
                    }
                    attackingCard = Opponent.Attack(Table);

                    if (attackingCard.ImageUrl != "")
                    {
                        AttackingCards.Add(attackingCard);
                    }

                    Player.RefreshPlayableForBeat(attackingCard);

                    if (Table.Length() == 0 || Table.Length() % 2 == 1)
                    {
                        // attackingCard = AttackingCards.Last();

                        if (attackingCard.ImageUrl != "")
                        {
                            defendingCard = Player.Defend(attackingCard, Table);

                            if (defendingCard.ImageUrl != "")
                            {
                                DefendingCards.Add(defendingCard);
                            }
                        }

                        Player.RefreshPlayableForBeat(attackingCard);
                    }
                }

            }
            else
            {
                Console.WriteLine($"\nНачало хода: ");

                for (int i = 0; i < Constants.MAX_CARDS_TO_ATTACK; i++)
                {
                    if (AttackingCards.Count == Constants.MAX_CARDS_TO_ATTACK)
                    {
                        TurnFinished = true;
                        Console.WriteLine("\nКонец хода");
                        break;
                    }

                    //Taken, no cards to Defend, max card on table, no cards for attack
                    if (Player.Taken
                            || Player.inHand.Count == 0
                            || Table.Length() == 12
                            || Opponent.GetCardsForAttack(Table).Count == 0)
                    {
                        TurnFinished = true;
                        break;
                    }

                    attackingCard = Opponent.Attack(Table);

                    if (attackingCard.ImageUrl != "")
                    {
                        AttackingCards.Add(attackingCard);
                    }

                    Player.RefreshPlayableForBeat(attackingCard);

                    if (Table.Length() == 0 || Table.Length() % 2 == 1)
                    {
                        // attackingCard = AttackingCards.Last();

                        if (attackingCard.ImageUrl != "")
                        {
                            defendingCard = Player.Defend(attackingCard, Table);

                            if (defendingCard.ImageUrl != "")
                            {
                                DefendingCards.Add(defendingCard);
                            }
                        }

                        Player.RefreshPlayableForBeat(attackingCard);
                    }
                }

                Console.WriteLine("\nКонец хода");
            }
        }

        Table.ClearTable();
    }

    public void RefillHands()
    {
        if (Deck.CardsAmount <= 6 && Deck.CardsAmount != 0)
        {
            while (Deck.CardsAmount != 0 || Player.inHand.Count < 6 || Opponent.inHand.Count < 6)
            {
                if (Player.IsAttack)
                {

                    if (Player.inHand.Count < 6)
                    {
                        Player.inHand.Add(Deck.DrawCard());
                    }


                    if (Deck.CardsAmount != 0 && Opponent.inHand.Count < 6)
                    {
                        Opponent.inHand.Add(Deck.DrawCard());
                    }

                    if (Player.inHand.Count >= 6 && Opponent.inHand.Count >= 6 || Deck.CardsAmount == 0)
                    {
                        Player.Sort();
                        Opponent.Sort();
                        break;
                    }

                }
                else
                {

                    if (Opponent.inHand.Count < 6)
                    {
                        Opponent.inHand.Add(Deck.DrawCard());
                    }

                    if (Deck.CardsAmount != 0 && Player.inHand.Count < 6)
                    {
                        Player.inHand.Add(Deck.DrawCard());
                    }

                    if ((Player.inHand.Count >= 6 && Opponent.inHand.Count >= 6) || Deck.CardsAmount == 0)
                    {
                        Player.Sort();
                        Opponent.Sort();
                        break;
                    }
                }

            }
        }
        else
        {
            Player.RefillHand(Deck);
            Opponent.RefillHand(Deck);
        }
    }

    public void EndCurrentTurn()
    {
        RefreshTurnQueue();

        AttackingCards.Clear();
        DefendingCards.Clear();

        Player.Taken = false;
        Opponent.Taken = false;

        foreach (var card in Player.inHand)
        {
            card.IsSelected = false;
        }

        Table.ClearTable();

        RefillHands();

        if (Player.inHand.Count == 0 || Opponent.inHand.Count == 0)
        {
            gameState = GameState.Finished;
        }
    }

    public void LoadGame()
    {
        Deck = new Deck();
        Deck.Shuffle();
        Deck.Trump();

        Player = new Player();
        Player.Name = "Rat";

        Opponent = new AIPlayer();
        Opponent.Name = "Бот";

        RefillHands();

        List<Card> firstTrumps = new List<Card>();

        //remember first trump of each player
        firstTrumps.Add(GetFirstTrump(Player.inHand));
        firstTrumps.Add(GetFirstTrump(Opponent.inHand));

        //set first turns player
        SetStartTurnNumbers(firstTrumps);
        //set turn numbers for more than 2 players
        SetStartTurnQueue(Player, Opponent);

        if (Player.IsAttack)
        {
            Console.WriteLine($"\nПервым ходит игрок {Player.Name}\n");
        }
        else
        {
            Console.WriteLine($"\nПервым ходит игрок {Opponent.Name}\n");
        }

        //reset fool properties
        Player.IsFool = false;
        Opponent.IsFool = false;

        if (_fools.ContainsKey(Player.Name))
            _fools[Player.Name] = false;

        if (gameState != GameState.Loading)
        {
            gameState = GameState.JustStarted;
        }
        else
        {
            FirstTurn = true;
            CountOfGames++;
        }
    }

    //set start turn queue
    private void SetStartTurnQueue(Player player, AIPlayer opponent)
    {
        if (player.TurnNumber == 1)
        {
            player.IsAttack = true;
            opponent.IsAttack = false;
        }
        else
        {
            player.IsAttack = false;
            opponent.IsAttack = true;
        }
    }

    //rerfesh turn queue for next turn
    public void RefreshTurnQueue()
    {
        if (!Player.Taken && !Opponent.Taken)
        {
            Player.IsAttack = !Player.IsAttack;
            Opponent.IsAttack = !Opponent.IsAttack;
        }
    }
    //get first trump card in player's hand
    private Card GetFirstTrump(List<Card> cards)
    {
        Card firstTrumpCard = new Card();

        foreach (var card in cards)
        {
            if (card.Suit == Deck.s_trumpSuit)
            {
                firstTrumpCard = card;
                break;
            }
        }

        return firstTrumpCard;
    }

    //set start turn number of players use checking min trumps in players' hand or choosing next player after fool
    private void SetStartTurnNumbers(List<Card> firstTrumpCards)
    {
        int number = 1;

        if (Player.IsFool == true)
        {
            Opponent.TurnNumber = 1;
            Player.TurnNumber = 2;
        }
        else if (Opponent.IsFool == true)
        {
            Player.TurnNumber = 1;
            Opponent.TurnNumber = 2;
        }

        else
        {
            for (int i = 1; i < firstTrumpCards.Count; i++)
            {
                if (firstTrumpCards[i].Rank < firstTrumpCards[i - 1].Rank)
                {
                    number++;
                }
            }

            if (number == 1)
            {
                Player.TurnNumber = 1;
                Opponent.TurnNumber = 2;
            }
            else
            {
                Opponent.TurnNumber = 1;
                Player.TurnNumber = 2;
            }
        }
    }

}
