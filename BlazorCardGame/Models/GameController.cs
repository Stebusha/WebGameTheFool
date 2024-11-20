using BlazorCardGame.Components.Pages.Partials;

namespace BlazorCardGame.Models;

public class GameController
{
    private const int MAX_CARDS_TO_ATTACK = 6;
    private Player player = new Player();
    private AIPlayer opponent = new AIPlayer();
    private ScoreTable _scoreTable = new ScoreTable();
    private Table _gameTable = new Table();
    private Deck _deck = new Deck();
    private Dictionary<string, bool> _fools = new Dictionary<string, bool>();
    private bool _Finished { get; set; } = false;
    private bool _TurnFinished { get; set; } = false;
    private bool _FirtsTurn { get; set; } = true;
    public int PlayerCount { get; set; } = 1;
    public int BotPlayerCount { get; set; } = 1;

    private void Turn(int turn)
    {
        _TurnFinished = false;
        Card attackingCard = new Card();

        //reset taken properties
        player.Taken = false;
        opponent.Taken = false;

        while (!_TurnFinished)
        {
            if (_FirtsTurn)
            {
                Console.WriteLine("\nНачало партии\n");
                Console.WriteLine($"Козырная масть - {_deck.GetTrumpSuitName()}");

                for (int i = 0; i < MAX_CARDS_TO_ATTACK - 1; i++)
                {
                    if (player.IsAttack)
                    {
                        //output cards on table
                        if (_gameTable.Length() != 0 && !opponent.Taken)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(_gameTable.ToString());
                            Console.ResetColor();
                        }
                        //Taken or no cards for attack
                        if (opponent.Taken || player.GetCardsForAttack(_gameTable).Count == 0)
                        {
                            _TurnFinished = true;
                            _FirtsTurn = false;
                            break;
                        }

                        attackingCard = player.Attack(_gameTable);
                        opponent.Defend(attackingCard, _gameTable);
                    }
                    else
                    {
                        //output cards on table
                        if (_gameTable.Length() != 0 && !player.Taken)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(_gameTable.ToString());
                            Console.ResetColor();
                        }
                        //Taken or no cards for attack
                        if (player.Taken || opponent.GetCardsForAttack(_gameTable).Count == 0)
                        {
                            _TurnFinished = true;
                            _FirtsTurn = false;
                            break;
                        }

                        attackingCard = opponent.Attack(_gameTable);
                        opponent.Defend(attackingCard, _gameTable);
                    }
                }

                if (!_TurnFinished)
                {
                    _TurnFinished = true;
                    _FirtsTurn = false;
                }

                Console.WriteLine("\nКонец хода");
            }
            else
            {
                Console.WriteLine($"\nНачало хода: ");

                for (int i = 0; i < MAX_CARDS_TO_ATTACK; i++)
                {
                    if (player.IsAttack)
                    {
                        //output cards on table
                        if (_gameTable.Length() != 0 && !opponent.Taken)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(_gameTable.ToString());
                            Console.ResetColor();
                        }

                        //Taken, no cards to Defend, max card on table, no cards for attack
                        if (opponent.Taken
                                || opponent.inHand.Count == 0
                                || _gameTable.Length() == 12
                                || player.GetCardsForAttack(_gameTable).Count == 0)
                        {
                            _TurnFinished = true;
                            break;
                        }

                        attackingCard = player.Attack(_gameTable);
                        opponent.Defend(attackingCard, _gameTable);
                    }
                    else
                    {
                        //output cards on table
                        if (_gameTable.Length() != 0 && !player.Taken)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(_gameTable.ToString());
                            Console.ResetColor();
                        }

                        //Taken, no cards to Defend, max card on table, no cards for attack
                        if (player.Taken
                                || player.inHand.Count == 0
                                || _gameTable.Length() == 12
                                || opponent.GetCardsForAttack(_gameTable).Count == 0)
                        {
                            _TurnFinished = true;
                            break;
                        }

                        attackingCard = opponent.Attack(_gameTable);
                        player.Defend(attackingCard, _gameTable);
                    }

                }

                if (!_TurnFinished)
                {
                    _TurnFinished = true;
                }

                Console.WriteLine("\nКонец хода");
            }
        }

        _gameTable.ClearTable();
        Console.ReadLine();
        Console.Clear();
    }

    //launch game, set start info, set trump, player's turns, check the winning condition
    public void Game(int playerCount, int AIPlayerCount, in bool repeat)
    {
        PlayerCount = playerCount;
        BotPlayerCount = AIPlayerCount;

        _deck = new Deck();
        _FirtsTurn = true;
        _Finished = false;

        _deck.Shuffle();
        _deck.Trump();

        if (!repeat)
        {

            //logic for 1 human and 1 bot players
            if (playerCount + AIPlayerCount == 2 && playerCount == 1)
            {
                Player player = new Player();

                // if (player.Name != null)
                // {
                //     if (_scoreTable.IsNameExistInScores(player.Name))
                //     {
                //         Console.WriteLine("Имя {0} уже существует, выбрать другое? (да/нет)", player.Name);
                //         string? answer = Console.ReadLine();

                //         if (answer != null && answer.ToLower() == "да")
                //         {
                //             string? tempName = Console.ReadLine();

                //             while (tempName == null)
                //             {
                //                 Console.WriteLine("Введите другое имя:");
                //                 tempName = Console.ReadLine();
                //             }

                //             player.Name = tempName;
                //         }
                //     }
                // }
                player.Name = "Rat";
                player.RefillHand(_deck);
                _fools.Add(player.Name, player.IsFool);

                AIPlayer opponent = new AIPlayer();
                opponent.RefillHand(_deck);
                opponent.Name = "Бот 1";
                _fools.Add(opponent.Name ?? "Бот 1", opponent.IsFool);
            }
        }

        //logic for reload game for same players 
        else
        {

            //reload 2 bot players
            if (playerCount + AIPlayerCount == 2 && playerCount == 0)
            {
                foreach (var fool in _fools)
                {
                    AIPlayer opponent = new AIPlayer(fool.Key, fool.Value);
                    opponent.RefillHand(_deck);
                }
            }

            // reload for all remaining variables of type players
            else
            {
                foreach (var fool in _fools)
                {
                    if (fool.Key.Contains("Бот"))
                    {
                        AIPlayer opponent = new AIPlayer(fool.Key, fool.Value);
                        opponent.RefillHand(_deck);
                    }
                    else
                    {
                        Player player = new Player(fool.Key, fool.Value);
                        player.RefillHand(_deck);
                    }
                }
            }
        }

        List<Card> firstTrumps = new List<Card>();

        //remember first trump of each player
        firstTrumps.Add(GetFirstTrump(player.inHand));
        firstTrumps.Add(GetFirstTrump(opponent.inHand));


        //set first turns player
        SetStartTurnNumbers(firstTrumps);
        //set turn numbers for more than 2 players
        SetStartTurnQueue(ref player, ref opponent);

        if (player.IsAttack)
        {
            Console.WriteLine($"\nПервым ходит игрок {player.Name}\n");
        }
        else
        {
            Console.WriteLine($"\nПервым ходит игрок {opponent.Name}\n");
        }

        //reset fool properties

        player.IsFool = false;
        opponent.IsFool = false;

        if (_fools.ContainsKey(player.Name))
            _fools[player.Name] = false;


        int turns = 0;

        while (!_Finished)
        {
            Console.WriteLine($"Козырная масть - {_deck.GetTrumpSuitName()}");
            Console.WriteLine($"Карт в колоде: {_deck.CardsAmount}");
            Console.WriteLine($"Количество карт игрока {player.Name} : {player.inHand.Count}");


            Turn(turns);

            if (_deck.CardsAmount != 0)
            {
                player.RefillHand(_deck);
                opponent.RefillHand(_deck);

                Console.WriteLine("Игроки взяли карты");
            }

            //set next turnes player
            if (!opponent.Taken && !player.Taken)
            {
                RefreshTurnQueue(ref player, ref opponent);
            }

            //check winner condition
            Win();

            //reset turns
            turns = 0;
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

        if (player.IsFool == true)
        {
            opponent.TurnNumber = 1;
            player.TurnNumber = 2;
        }
        else if (opponent.IsFool == true)
        {
            player.TurnNumber = 1;
            opponent.TurnNumber = 2;
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
                player.TurnNumber = 1;
                opponent.TurnNumber = 2;
            }
            else
            {
                opponent.TurnNumber = 1;
                player.TurnNumber = 2;
            }
        }
    }




    //check the winner, update and display score table
    private void Win()
    {
        if (_deck.CardsAmount == 0)
        {

            //logic for 2 players
            if (BotPlayerCount + PlayerCount == 2)
            {
                if (player.inHand.Count == 0)
                {
                    _Finished = true;
                    Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {player.Name}.");
                    opponent.IsFool = true;

                    if (opponent.Name != null)
                    {
                        if (_fools.ContainsKey(opponent.Name))
                        {
                            _fools[opponent.Name] = opponent.IsFool;
                        }
                    }

                    int score = 1;

                    _scoreTable.AddScore(player.Name, score);
                }
                else if (opponent.inHand.Count == 0)
                {
                    Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {player.Name}.");
                    _Finished = true;
                    player.IsFool = true;
                    _fools[player.Name] = player.IsFool;
                    int score = 1;

                    _scoreTable.AddScore(opponent.Name, score);
                }
            }
        }
    }

    //set start turn queue
    private void SetStartTurnQueue(ref Player player, ref AIPlayer opponent)
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

    //rerfesh turn queue for nest turn
    private void RefreshTurnQueue(ref Player player, ref AIPlayer opponent)
    {
        player.IsAttack = !player.IsAttack;
        opponent.IsAttack = !opponent.IsAttack;
    }
}