using BlazorCardGame.Models;
using BlazorCardGame.Enums;

namespace BlazorCardGame.Services;

public class FoolGameService
{
    private Dictionary<string, bool> _fools = [];
    private ScoreTable scoreTable = new();
    public Deck Deck { get; private set; } = new Deck();
    public Table Table { get; private set; } = new Table();
    public GameState GameState { get; private set; } = GameState.Settings;
    public bool IsLoaded { get; private set; } = false;
    public bool CanPlay { get; private set; } = true;
    public bool CanDraw { get; private set; } = false;
    public bool CanEndTurn { get; private set; } = false;
    public string PlayButton { get; private set; } = "Походить";
    public string StartButton { get; private set; } = "Новая игра";
    public static int CountOfGames { get; private set; } = 0;
    public int DiscardCardCount { get; private set; } = 0;
    public bool FirstTurn { get; set; } = false;
    public Player Player { get; set; } = new Player();
    public AIPlayer Opponent { get; set; } = new AIPlayer();
    public List<Card> AttackingCards { get; set; } = [];
    public List<Card> DefendingCards { get; set; } = [];

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

        //set start turn numbers from the fool
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
        //set start turn number base on first trumps
        else
        {
            //checking minimal trump
            for (int i = 1; i < firstTrumpCards.Count; i++)
            {
                if (firstTrumpCards[i].ImageUrl == "" && firstTrumpCards[i - 1].ImageUrl != "")
                {
                    break;
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

            //set numbers
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

    //refill cards in hands
    public void RefillHands()
    {
        //deck card count <= 6
        if (Deck.CardsAmount <= 6 && Deck.CardsAmount != 0)
        {
            //if can draw card
            while (Deck.CardsAmount != 0 || Player.inHand.Count < 6 || Opponent.inHand.Count < 6)
            {
                //player's take first
                if (Player.IsAttack)
                {
                    //if can player's take
                    if (Player.inHand.Count < 6)
                    {
                        Player.inHand.Add(Deck.DrawCard());
                    }

                    //if cards exist and bot can bot's take
                    if (Deck.CardsAmount != 0 && Opponent.inHand.Count < 6)
                    {
                        Opponent.inHand.Add(Deck.DrawCard());
                    }

                    //if cards in hand enough or deck card end sort card in hands and exit
                    if (Player.inHand.Count >= 6 && Opponent.inHand.Count >= 6 || Deck.CardsAmount == 0)
                    {
                        Player.Sort();
                        Opponent.Sort();
                        break;
                    }
                }
                //bot's take first
                else
                {
                    //if cards exist and bot can bot's take
                    if (Opponent.inHand.Count < 6)
                    {
                        Opponent.inHand.Add(Deck.DrawCard());
                    }

                    //if can player's take
                    if (Deck.CardsAmount != 0 && Player.inHand.Count < 6)
                    {
                        Player.inHand.Add(Deck.DrawCard());
                    }

                    //if cards in hand enough or deck card end sort card in hands and exit
                    if ((Player.inHand.Count >= 6 && Opponent.inHand.Count >= 6) || Deck.CardsAmount == 0)
                    {
                        Player.Sort();
                        Opponent.Sort();
                        break;
                    }
                }
            }
        }
        //deck cards count > 6
        else
        {
            Player.RefillHand(Deck);
            Opponent.RefillHand(Deck);
        }
    }

    //reset game on logout
    public void ResetGame()
    {
        DiscardCardCount = 0;
        CountOfGames = 0;
        GameState = GameState.Loading;
        FirstTurn = false;
        DefendingCards.Clear();
        AttackingCards.Clear();
        IsLoaded = false;
        CanPlay = true;
        CanDraw = false;
        CanEndTurn = false;
        PlayButton = "Походить";
        StartButton = "Новая игра";
        _fools = [];
    }
    //load game
    public void LoadGame()
    {
        DiscardCardCount = 0;

        //first load
        if (CountOfGames == 0)
        {
            Deck = new Deck();
            Deck.Shuffle();
            Deck.Trump();

            Player = new Player();

            _fools.Add(Player.Name, Player.IsFool);

            Opponent = new AIPlayer();

            _fools.Add(Opponent.Name, Opponent.IsFool);
        }
        //repeat load
        else
        {
            Player.inHand.Clear();
            Opponent.inHand.Clear();

            if (_fools.ContainsKey(Player.Name))
            {
                _fools[Player.Name] = Player.IsFool;
            }

            if (_fools.ContainsKey(Opponent.Name))
            {
                _fools[Opponent.Name] = Opponent.IsFool;
            }

            Deck = new Deck();
            Deck.Shuffle();
            Deck.Trump();
        }

        RefillHands();

        List<Card> firstTrumps =
        [
            //remember first trump of each player
            GetFirstTrump(Player.inHand),
            GetFirstTrump(Opponent.inHand),
        ];

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

        if (GameState != GameState.JustStarted)
        {
            GameState = GameState.JustStarted;
            FirstTurn = true;
            CountOfGames++;
        }

        RefreshPlayButtonName();

        if (Player.IsAttack)
        {
            IsLoaded = true;
        }
        else
        {
            CanPlay = false;
        }
    }

    //refresh start button properties
    private void RefreshStartButton()
    {
        if (GameState == GameState.Finished || GameState == GameState.JustStarted)
        {
            IsLoaded = false;
            CanPlay = false;
            CanDraw = false;
            CanEndTurn = false;
        }
        else if (GameState == GameState.InProgress)
        {
            IsLoaded = true;
            CanPlay = true;
        }
    }

    //refresh play button's name
    private void RefreshPlayButtonName()
    {
        if (Player.IsAttack)
        {
            if (AttackingCards.Count == 0)
                PlayButton = "Походить";
            else
                PlayButton = "Подкинуть";
        }
        else
        {
            PlayButton = "Отбиться";
        }
    }

    //check non playable cards, set play button disable property if can't play
    private void CheckNonPlayable()
    {
        //checking state of cards for setting enable property for "Play" button
        var nonPlayable = 0;

        //count non playable cards
        foreach (var card in Player.inHand)
        {
            if (!card.IsPlayable)
            {
                nonPlayable++;
            }
        }

        //if all cards non playable set button disabled
        if (nonPlayable == Player.inHand.Count)
        {
            CanPlay = false;
        }
    }

    //start current turn
    public void StartCurrentTurn()
    {
        RefreshPlayButtonName();

        GameState = GameState.InProgress;

        RefreshStartButton();

        //start attack if first turn is bot's
        if (!Player.IsAttack)
        {
            Card attackingCard = Opponent.Attack(Table);

            if (attackingCard.ImageUrl != "")
            {
                AttackingCards.Add(attackingCard);
                CanDraw = true;
            }

            Player.RefreshPlayableForBeat(attackingCard, Table);

            CheckNonPlayable();
        }
        //refresh playable cards if first turn is player's
        else
        {
            Player.RefreshPlayableForAttack(Table);
        }
    }

    //turn logic
    public async Task Turn()
    {
        //refresh start properties for game
        Player.RefreshPlayableForAttack(Table);

        RefreshPlayButtonName();

        //player attack
        if (Player.IsAttack && Opponent.inHand.Count != 0)
        {
            //check first turn discards can't be more than ten
            if (FirstTurn && Table.Length() != 10)
            {
                //choose attacking card
                Card attackingCard = Player.Attack(Table);

                //if attacking card exists
                if (attackingCard.ImageUrl != "")
                {
                    //add to center table
                    AttackingCards.Add(attackingCard);

                    RefreshPlayButtonName();

                    //bot's defense
                    await Task.Delay(500);

                    Card defendingCard = Opponent.Defend(attackingCard, Table);

                    //if defending card exists
                    if (defendingCard.ImageUrl != "")
                    {
                        //add to center table
                        DefendingCards.Add(defendingCard);

                        //refresh enable property for "EndTurn" button
                        if (Table.Length() % 2 == 0)
                        {
                            CanEndTurn = true;
                            CanDraw = false;
                        }
                    }
                }

                Player.RefreshPlayableForAttack(Table);

                CheckNonPlayable();
            }
            else if (Table.Length() != 12)
            {
                //choose attacking card
                Card attackingCard = Player.Attack(Table);

                //if attacking card exists
                if (attackingCard.ImageUrl != "")
                {
                    //add to center table
                    AttackingCards.Add(attackingCard);

                    CanEndTurn = false;

                    RefreshPlayButtonName();

                    //bot's defense
                    await Task.Delay(500);

                    Card defendingCard = Opponent.Defend(attackingCard, Table);

                    //if defending card exists
                    if (defendingCard.ImageUrl != "")
                    {
                        //add to center table
                        DefendingCards.Add(defendingCard);

                        //refresh enable property for "EndTurn" button
                        if (Table.Length() % 2 == 0)
                        {
                            CanEndTurn = true;
                            CanDraw = false;
                        }
                    }
                }

                Player.RefreshPlayableForAttack(Table);

                CheckNonPlayable();
            }
        }
        //bot's attack
        else
        {
            //getting card for player defense if attacking card exists
            if (Table.Length() % 2 == 1)
            {
                Card attackingCard = AttackingCards.Last();

                if (attackingCard.ImageUrl != "")
                {
                    CanDraw = true;
                    CanPlay = true;
                    CanEndTurn = false;

                    Card defendingCard = Player.Defend(attackingCard, Table);

                    //defense if attacking card exists
                    if (defendingCard.ImageUrl != "")
                    {
                        DefendingCards.Add(defendingCard);

                        //refresh enable property for "EndTurn" button
                        if (Table.Length() % 2 == 0)
                        {
                            CanEndTurn = true;
                            CanDraw = false;
                            CanPlay = false;
                        }
                    }
                }

                Player.RefreshPlayableForBeat(attackingCard, Table);

                CheckNonPlayable();
            }

            //all the attacking card defend, not max count of cards on table, player had cards in hand, player not taken
            if (Table.Length() % 2 == 0 && Table.Length() != 12
                && Player.inHand.Count != 0 && !Player.Taken)
            {
                //bot's attack
                Card attackingCard = Opponent.Attack(Table);

                await Task.Delay(500);

                if (attackingCard.ImageUrl != "")
                {
                    //add attacking card to center table if exists
                    AttackingCards.Add(attackingCard);
                    CanDraw = true;
                    CanPlay = true;
                    CanEndTurn = false;
                }
                else
                {
                    //refresh enable properties for buttons "Play" and "DrawCards"
                    CanPlay = false;
                    CanDraw = false;
                }

                Player.RefreshPlayableForBeat(attackingCard, Table);

                CheckNonPlayable();
            }

            //player taken
            if (Player.Taken)
            {
                Opponent.IsAttack = true;
                Player.IsAttack = false;
            }
        }
    }

    //checking win lose condition and set winner
    private void WinLose()
    {
        //check deck ends
        if (Deck.CardsAmount == 0)
        {
            //set game state if game ends
            if (Player.inHand.Count == 0 || Opponent.inHand.Count == 0)
            {
                GameState = GameState.Finished;
                CanDraw = false;
                CanPlay = false;
            }

            //player win
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
            //bot win
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

    //end current turn, refresh game's properties for next turn
    public void EndCurrentTurn()
    {
        //set flag first turn
        if (FirstTurn)
        {
            FirstTurn = false;
        }

        //take cards if player taken
        if ((Player.Taken || Table.Length() % 2 == 1) && !Opponent.Taken)
        {
            Player.TakeCards(Table);
        }

        //refresh discard pile if nobody taken
        if (!Player.Taken && !Opponent.Taken)
        {
            DiscardCardCount += Table.Length();
        }

        WinLose();

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

        RefreshPlayButtonName();

        CanEndTurn = false;

        if (Player.IsAttack)
        {
            Player.RefreshPlayableForAttack(Table);
        }

        //continue refresh buttons state
        RefreshStartButton();

        if (GameState != GameState.Finished)
        {
            CanPlay = true;

            if (Player.IsAttack)
            {
                CanDraw = false;
            }
            else
            {
                CanDraw = true;
            }
        }
    }
}
