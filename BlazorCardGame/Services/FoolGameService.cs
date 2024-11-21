using BlazorCardGame.Models;

namespace BlazorCardGame.Services;

public class FoolGameService
{
    private const int MAX_CARDS_TO_ATTACK = 6;
    private Dictionary<string, bool> _fools = new Dictionary<string, bool>();
    public Player Player { get; set; } = new Player();
    public AIPlayer Opponent { get; set; } = new AIPlayer();
    public Deck Deck { get; set; } = new Deck();
    public Table Table { get; set; } = new Table();
    public List<Card> AttackingCards { get; set; } = new List<Card>();
    public List<Card> DefendingCards { get; set; } = new List<Card>();
    public bool Repeat { get; set; } = false;
    public GameState gameState { get; set; } = GameState.JustStarted;
    public int CountOfGames { get; set; } = 0;
    public bool FirstTurn { get; private set; } = false;
    public bool TurnFinished { get; set; } = false;

    public void Turn()
    {
        TurnFinished = false;
        Card attackingCard = new Card();
        Card defendingCard = new Card();

        //reset taken properties
        Player.Taken = false;
        Opponent.Taken = false;

        while (!TurnFinished)
        {
            if (FirstTurn)
            {
                Console.WriteLine("\nНачало партии\n");
                Console.WriteLine($"Козырная масть - {Deck.GetTrumpSuitName()}");

                if (AttackingCards.Count == MAX_CARDS_TO_ATTACK - 1)
                {
                    TurnFinished = true;
                    FirstTurn = false;
                    Console.WriteLine("\nКонец хода");
                }

                if (Player.IsAttack)
                {
                    //Taken or no cards for attack
                    if (Opponent.Taken || Player.GetCardsForAttack(Table).Count == 0)
                    {
                        TurnFinished = true;
                        FirstTurn = false;
                        break;
                    }

                    attackingCard = Player.Attack(Table);
                    defendingCard = Opponent.Defend(attackingCard, Table);
                    AttackingCards.Add(attackingCard);
                    DefendingCards.Add(defendingCard);
                }
                else
                {
                    //Taken or no cards for attack
                    if (Player.Taken || Opponent.GetCardsForAttack(Table).Count == 0)
                    {
                        TurnFinished = true;
                        FirstTurn = false;
                        break;
                    }

                    attackingCard = Opponent.Attack(Table);
                    defendingCard = Player.Defend(attackingCard, Table);
                    AttackingCards.Add(attackingCard);
                    DefendingCards.Add(defendingCard);
                }
            }
            else
            {
                Console.WriteLine($"\nНачало хода: ");

                if (AttackingCards.Count == MAX_CARDS_TO_ATTACK - 1)
                {
                    TurnFinished = true;
                    FirstTurn = false;
                    Console.WriteLine("\nКонец хода");
                }

                if (Player.IsAttack)
                {
                    //Taken, no cards to Defend, max card on table, no cards for attack
                    if (Opponent.Taken
                            || Opponent.inHand.Count == 0
                            || Table.Length() == 12
                            || Player.GetCardsForAttack(Table).Count == 0)
                    {
                        TurnFinished = true;
                        break;
                    }

                    attackingCard = Player.Attack(Table);
                    defendingCard = Opponent.Defend(attackingCard, Table);
                    AttackingCards.Add(attackingCard);
                    DefendingCards.Add(defendingCard);
                }
                else
                {
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
                    defendingCard = Player.Defend(attackingCard, Table);
                    AttackingCards.Add(attackingCard);
                    DefendingCards.Add(defendingCard);
                }

                Console.WriteLine("\nКонец хода");
            }
        }

        Table.ClearTable();
    }



    public void LoadGame()
    {
        if (gameState != GameState.JustStarted)
        {
            gameState = GameState.JustStarted;
        }
        else
        {
            FirstTurn = true;
            CountOfGames++;
        }

        Deck = new Deck();
        Deck.Shuffle();
        Deck.Trump();

        Player = new Player();
        Player.Name = "Rat";
        Player.RefillHand(Deck);

        Opponent = new AIPlayer();
        Opponent.Name = "Бот";
        Opponent.RefillHand(Deck);

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
        Player.IsAttack = !Player.IsAttack;
        Opponent.IsAttack = !Opponent.IsAttack;
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
