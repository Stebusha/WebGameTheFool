using BlazorCardGame.Models;

namespace BlazorCardGame.Services;

public class FoolGameService
{
    private Dictionary<string, bool> _fools = new Dictionary<string, bool>();
    public ScoreTable scoreTable = new ScoreTable();
    public Player Player { get; set; } = new Player();
    public AIPlayer Opponent { get; set; } = new AIPlayer();
    public Deck Deck { get; set; } = new Deck();
    public Table Table { get; set; } = new Table();
    public List<Card> AttackingCards { get; set; } = new List<Card>();
    public List<Card> DefendingCards { get; set; } = new List<Card>();
    public bool Repeat { get; set; } = false;
    public GameState gameState { get; set; } = GameState.Loading;
    public static int CountOfGames { get; set; } = 0;
    public bool FirstTurn { get; set; } = false;
    public bool TurnFinished { get; set; } = false;
    public int discardCardCount { get; set; } = 0;

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

    // public async Task Turn()
    // {
    //     TurnFinished = false;

    //     Player.RefreshPlayableForAttack(Table);

    //     if (Player.IsAttack && Opponent.inHand.Count != 0)
    //     {
    //         if (FirstTurn && Table.Length() == 10)
    //         {
    //             FirstTurn = false;
    //             TurnFinished = true;
    //         }

    //         Card attackingCard = Player.Attack(Table);

    //         if (attackingCard.ImageUrl != "")
    //         {
    //             AttackingCards.Add(attackingCard);

    //             await Task.Delay(500);

    //             Card defendingCard = Opponent.Defend(attackingCard, Table);

    //             if (defendingCard.ImageUrl != "")
    //             {
    //                 DefendingCards.Add(defendingCard);
    //             }
    //         }

    //         Player.RefreshPlayableForAttack(Table);

    //         if (Opponent.Taken)
    //         {
    //             TurnFinished = true;
    //             Opponent.IsAttack = false;
    //             Player.IsAttack = true;
    //         }
    //     }
    //     else
    //     {
    //         if (Table.Length() == 0 || Table.Length() % 2 == 1)
    //         {
    //             Card attackingCard = AttackingCards.Last();

    //             if (attackingCard.ImageUrl != "")
    //             {
    //                 Card defendingCard = Player.Defend(attackingCard, Table);

    //                 if (defendingCard.ImageUrl != "")
    //                 {
    //                     DefendingCards.Add(defendingCard);
    //                 }
    //             }

    //             Player.RefreshPlayableForBeat(attackingCard, Table);
    //         }

    //         if (FirstTurn && Table.Length() == 10)
    //         {
    //             FirstTurn = false;
    //             TurnFinished = true;
    //         }

    //         if (Table.Length() % 2 == 0 && Table.Length() != 12
    //             && Player.inHand.Count != 0 && !Player.Taken)
    //         {
    //             Card attackingCard = Opponent.Attack(Table);

    //             await Task.Delay(500);

    //             if (attackingCard.ImageUrl != "")
    //             {
    //                 AttackingCards.Add(attackingCard);
    //             }

    //             Player.RefreshPlayableForBeat(attackingCard, Table);
    //         }

    //         if (Player.Taken)
    //         {
    //             Opponent.IsAttack = true;
    //             Player.IsAttack = false;
    //         }
    //     }
    // }

    public void EndCurrentTurn()
    {
        WinLose();

        RefreshTurnQueue();

        AttackingCards.Clear();
        DefendingCards.Clear();

        Player.Taken = false;
        Opponent.Taken = false;

        TurnFinished = false;

        foreach (var card in Player.inHand)
        {
            card.IsSelected = false;
        }

        Table.ClearTable();

        RefillHands();
    }

    private void WinLose()
    {
        if (Deck.CardsAmount == 0)
        {

            if (Player.inHand.Count == 0 || Opponent.inHand.Count == 0)
            {
                gameState = GameState.Finished;
            }

            if (Player.inHand.Count == 0 && Opponent.inHand.Count != 0)
            {
                Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {Player.Name}.");

                Opponent.IsFool = true;

                if (Opponent.Name != null)
                {
                    if (_fools.ContainsKey(Opponent.Name))
                    {
                        _fools[Opponent.Name] = Opponent.IsFool;
                    }
                }

                int score = 1;

                scoreTable.AddScore(Player.Name, score);
            }
            else if (Player.inHand.Count != 0 && Opponent.inHand.Count == 0)
            {
                Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {Opponent.Name}.");

                Player.IsFool = true;
                _fools[Player.Name] = Player.IsFool;

                int score = 1;

                scoreTable.AddScore(Opponent.Name, score);
            }
        }
    }
    public void LoadGame()
    {
        discardCardCount = 0;

        Deck = new Deck();
        Deck.Shuffle();
        Deck.Trump();

        Player = new Player();
        Player.Name = "Admin";

        Opponent = new AIPlayer();
        Opponent.Name = "Бот 1";

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

        if (gameState != GameState.JustStarted)
        {
            gameState = GameState.JustStarted;
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
                if (firstTrumpCards[i].ImageUrl == "" && firstTrumpCards[i - 1].ImageUrl != "")
                {
                    number = 0;
                }
                else if (firstTrumpCards[i - 1].ImageUrl == "" && firstTrumpCards[i].ImageUrl != "")
                {
                    number++;
                }
                else if (firstTrumpCards[i].Rank < firstTrumpCards[i - 1].Rank)
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
