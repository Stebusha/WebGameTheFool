namespace TheFool;
public class GameController
{
    private const int MAX_CARDS_TO_ATTACK = 6;
    private List<IPlayer> _players = new List<IPlayer>();
    private ScoreTable _scoreTable = new ScoreTable();
    private Table _gameTable = new Table();
    private Deck _deck = new Deck();
    private Dictionary<string, bool> _fools = new Dictionary<string, bool>();
    private bool _Finished { get; set; }
    private bool _TurnFinished { get; set; }
    private bool _FirtsTurn { get; set; }
    public int PlayerCount { get; set; }
    public int BotPlayerCount { get; set; }

    //turn logic
    private void Turn(int turn)
    {
        _TurnFinished = false;
        Card attackingCard = new Card();

        int attacking = turn % _players.Count;
        int defending = (turn + 1) % _players.Count;
        int nextAttacking = (turn + 2) % _players.Count;

        //reset taken properties
        foreach (var player in _players)
        {
            player.Taken = false;
        }

        while (!_TurnFinished)
        {
            if (_FirtsTurn)
            {
                Console.WriteLine("\nНачало партии\n");
                Console.WriteLine($"Козырная масть - {_deck.GetTrumpSuitName()}");

                for (int i = 0; i < MAX_CARDS_TO_ATTACK - 1; i++)
                {
                    //output cards on table
                    if (_gameTable.Length() != 0 && !_players[defending].Taken)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(_gameTable.ToString());
                        Console.ResetColor();
                    }

                    //Taken or no cards for attack
                    if (_players[defending].Taken || _players[attacking].GetCardsForAttack(_gameTable).Count == 0)
                    {
                        _TurnFinished = true;
                        _FirtsTurn = false;
                        break;
                    }

                    attackingCard = _players[attacking].Attack(_gameTable);
                    _players[defending].Defend(attackingCard, _gameTable);

                    //logic for more than 2 players
                    if (_players.Count > 2)
                    {
                        if (_players[defending].Taken
                                || _players[defending].GetCards().Count == 0
                                || _players[nextAttacking].GetCardsForAttack(_gameTable).Count == 0
                                || i == 4)
                        {
                            _TurnFinished = true;
                            _FirtsTurn = false;
                            break;
                        }

                        i++;
                        attackingCard = _players[nextAttacking].Attack(_gameTable);
                        _players[defending].Defend(attackingCard, _gameTable);
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
                //logic for more than 2 players
                if (_players.Count == 1)
                {
                    break;
                }

                Console.WriteLine($"\nНачало хода: ");

                for (int i = 0; i < MAX_CARDS_TO_ATTACK; i++)
                {
                    //output cards on table
                    if (_gameTable.Length() != 0 && !_players[defending].Taken)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(_gameTable.ToString());
                        Console.ResetColor();
                    }

                    //Taken, no cards to Defend, max card on table, no cards for attack
                    if (_players[defending].Taken
                            || _players[defending].GetCards().Count == 0
                            || _gameTable.Length() == 12
                            || _players[attacking].GetCardsForAttack(_gameTable).Count == 0)
                    {
                        _TurnFinished = true;
                        break;
                    }

                    attackingCard = _players[attacking].Attack(_gameTable);
                    _players[defending].Defend(attackingCard, _gameTable);

                    //logic for more than 2 players
                    if (_players.Count > 2)
                    {
                        if (_players[defending].Taken
                                || _players[defending].GetCards().Count == 0
                                || i == 5
                                || _players[nextAttacking].GetCardsForAttack(_gameTable).Count == 0)
                        {
                            _TurnFinished = true;
                            break;
                        }

                        i++;
                        attackingCard = _players[nextAttacking].Attack(_gameTable);
                        _players[defending].Defend(attackingCard, _gameTable);
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
            _players = new List<IPlayer>();

            //logic for 2 bot players
            if (playerCount + AIPlayerCount == 2 && playerCount == 0)
            {
                AIPlayer aIPlayer = new AIPlayer();
                aIPlayer.RefillHand(_deck);
                _players.Add(aIPlayer);
                _players[0].Name = "Бот 1";
                _fools.Add(_players[0].Name ?? "Бот 1", _players[0].IsFool);

                AIPlayer aIPlayer1 = new AIPlayer();
                aIPlayer1.RefillHand(_deck);
                _players.Add(aIPlayer1);
                _players[1].Name = "Бот 2";
                _fools.Add(_players[1].Name ?? "Бот 2", _players[1].IsFool);
            }

            //logic for 1 human and 1 bot players
            else if (playerCount + AIPlayerCount == 2 && playerCount == 1)
            {
                Player player = new Player();

                if (player.Name != null)
                {
                    if (_scoreTable.IsNameExistInScores(player.Name))
                    {
                        Console.WriteLine("Имя {0} уже существует, выбрать другое? (да/нет)", player.Name);
                        string? answer = Console.ReadLine();

                        if (answer != null && answer.ToLower() == "да")
                        {
                            string? tempName = Console.ReadLine();

                            while (tempName == null)
                            {
                                Console.WriteLine("Введите другое имя:");
                                tempName = Console.ReadLine();
                            }

                            player.Name = tempName;
                        }
                    }
                }

                player.RefillHand(_deck);
                _players.Add(player);

                if (player.Name != null)
                    _fools.Add(player.Name, player.IsFool);

                AIPlayer aIPlayer1 = new AIPlayer();
                aIPlayer1.RefillHand(_deck);
                _players.Add(aIPlayer1);
                _players[1].Name = "Бот 1";
                _fools.Add(_players[1].Name ?? "Бот 1", _players[1].IsFool);
            }

            //logic for all remaining variables of type players
            else
            {
                //set humans 
                for (int i = 0; i < playerCount; i++)
                {
                    Player player = new Player();

                    if (player.Name != null)
                    {
                        if (_scoreTable.IsNameExistInScores(player.Name))
                        {
                            Console.WriteLine("Имя {0} уже существует, выбрать другое? (да/нет)", player.Name);
                            string? answer = Console.ReadLine();

                            if (answer != null && answer.ToLower() == "да")
                            {
                                string? tempName = Console.ReadLine();

                                while (tempName == null)
                                {
                                    Console.WriteLine("Введите другое имя:");
                                    tempName = Console.ReadLine();
                                }

                                player.Name = tempName;
                            }
                        }
                    }

                    player.RefillHand(_deck);
                    _players.Add(player);

                    if (player.Name != null)
                    {
                        _fools.Add(player.Name, player.IsFool);
                    }
                }

                //set AIs
                for (int i = playerCount; i < playerCount + AIPlayerCount; i++)
                {
                    AIPlayer aIPlayer = new AIPlayer();
                    aIPlayer.RefillHand(_deck);
                    _players.Add(aIPlayer);
                    _players[i].Name = $"Бот {i + 1}";
                    _fools.Add(_players[i].Name ?? $"Бот {i + 1}", _players[i].IsFool);
                }
            }
        }

        //logic for reload game for same players 
        else
        {
            _players = new List<IPlayer>();

            //reload 2 bot players
            if (playerCount + AIPlayerCount == 2 && playerCount == 0)
            {
                foreach (var fool in _fools)
                {
                    AIPlayer aIPlayer = new AIPlayer(fool.Key, fool.Value);
                    aIPlayer.RefillHand(_deck);
                    _players.Add(aIPlayer);
                }
            }

            // reload for all remaining variables of type players
            else
            {
                foreach (var fool in _fools)
                {
                    if (fool.Key.Contains("Бот"))
                    {
                        AIPlayer aIPlayer = new AIPlayer(fool.Key, fool.Value);
                        aIPlayer.RefillHand(_deck);
                        _players.Add(aIPlayer);
                    }
                    else
                    {
                        Player player = new Player(fool.Key, fool.Value);
                        player.RefillHand(_deck);
                        _players.Add(player);
                    }
                }
            }
        }

        List<Card> firstTrumps = new List<Card>();

        //remember first trump of each player
        foreach (var player in _players)
        {
            firstTrumps.Add(GetFirstTrump(player.GetCards()));
        }

        //set first turns player
        int first = firstTurnNumbers(firstTrumps);
        Console.WriteLine($"\nПервым ходит игрок {_players[first].Name}\n");

        _players[first].TurnNumber = 1;

        //set turn numbers for 2 players
        if (_players.Count == 2)
        {
            foreach (var player in _players)
            {
                if (player.TurnNumber == 0)
                {
                    player.TurnNumber = 2;
                    break;
                }
            }
        }

        //set turn numbers for more than 2 players
        else
        {
            SetStartTurnNumbers(ref _players, first);
        }

        //reset fool properties
        foreach (var player in _players)
        {
            player.IsFool = false;

            if (_fools.ContainsKey(player.Name))
                _fools[player.Name] = false;
        }

        int turns = 0;

        while (!_Finished)
        {
            Console.WriteLine($"Козырная масть - {_deck.GetTrumpSuitName()}");
            Console.WriteLine($"Карт в колоде: {_deck.CardsAmount}");

            foreach (var player in _players)
            {
                Console.WriteLine($"Количество карт игрока {player.Name} : {player.GetCards().Count}");
            }

            Turn(turns);

            if (_deck.CardsAmount != 0)
            {
                _players[turns % _players.Count].RefillHand(_deck);
                _players[(turns + 1) % _players.Count].RefillHand(_deck);

                //for more than 2 players
                if (_players.Count > 2)
                {
                    _players[(turns + 2) % _players.Count].RefillHand(_deck);
                }

                Console.WriteLine("Игроки взяли карты");
            }

            //set next turnes player
            if (!_players[(turns + 1) % _players.Count].Taken)
            {
                turns++;
            }
            else
            {
                turns += 2;
            }

            //refresh turn numbers based on turns
            RefreshTurnNumbers(ref _players, turns);

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

    //set first turn number of players use checking min trumps in players' hand or choosing next player after fool
    private int firstTurnNumbers(List<Card> firstTrumpCards)
    {
        int number = 0;

        foreach (var player in _players)
        {
            number++;

            if (player.IsFool == true)
            {
                return number % _players.Count;
            }
            else if (number == _players.Count)
            {
                number = 0;

                for (int i = 1; i < firstTrumpCards.Count; i++)
                {
                    if (firstTrumpCards[i].Rank < firstTrumpCards[i - 1].Rank)
                    {
                        number++;
                    }
                }
            }
        }

        return number;
    }

    //check the winner, update and display score table
    private void Win()
    {
        if (_deck.CardsAmount == 0)
        {
            int lefts = 0;

            //check lefts players
            foreach (var player in _players)
            {
                if (player.GetCards().Count == 0)
                {
                    lefts++;
                }
            }

            //check draw condition
            if (lefts == _players.Count)
            {
                _Finished = true;
                Console.WriteLine($"Колода закончилась. Конец партии. Ничья.");
                if (BotPlayerCount + PlayerCount == 2)
                {
                    _scoreTable.DisplayScores();
                }
                else
                {
                    _scoreTable.DisplayFools();
                }
            }

            //logic for 2 players
            else if (BotPlayerCount + PlayerCount == 2)
            {
                if (_players[1].GetCards().Count == 0)
                {
                    _Finished = true;
                    Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {_players[1].Name}.");
                    _players[0].IsFool = true;

                    if (_players[0].Name != null)
                    {
                        if (_fools.ContainsKey(_players[0].Name))
                        {
                            _fools[_players[0].Name] = _players[0].IsFool;
                        }
                    }

                    int score = 1;

                    _scoreTable.AddScore(_players[1].Name, score);
                    _scoreTable.DisplayScores();
                }
                else if (_players[0].GetCards().Count == 0)
                {
                    Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {_players[0].Name}.");
                    _Finished = true;
                    _players[1].IsFool = true;
                    _fools[_players[1].Name] = _players[1].IsFool;
                    int score = 1;

                    _scoreTable.AddScore(_players[0].Name, score);
                    _scoreTable.DisplayScores();
                }
            }

            //logic for more than 2 players
            else
            {
                if (_players.Count > 1)
                {
                    for (int p = 0; p < _players.Count; p++)
                    {
                        if (_players[p].GetCards().Count == 0)
                        {
                            Console.WriteLine($"Карты закончились. Игрок {_players[p].Name} вышел.\n");
                            _players[p].IsFool = false;
                            _players.Remove(_players[p]);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Колода закончилась. Конец партии. Проиграл игрок {_players[0].Name}.");
                    _Finished = true;
                    int score = 1;
                    _players[0].IsFool = true;
                    _fools[_players[0].Name] = _players[0].IsFool;

                    _scoreTable.AddFool(_players[0].Name, score);
                    _scoreTable.DisplayFools();
                }
            }
        }
    }

    //set start turn numbers
    private void SetStartTurnNumbers(ref List<IPlayer> players, int first)
    {
        players[first].TurnNumber = 1;
        int counter = 0;

        for (int i = first + 1; i < players.Count; i++)
        {
            counter++;

            if (players[i].TurnNumber == 0)
            {
                players[i].TurnNumber = players[first].TurnNumber + counter;
            }
        }

        for (int i = 0; i < first; i++)
        {
            counter++;

            if (players[i].TurnNumber == 0)
            {
                players[i].TurnNumber = players[first].TurnNumber + counter;
            }
        }

        players = players.OrderBy(p => p.TurnNumber).ToList();
    }

    //rerfesh turn numbers for nest turn
    private void RefreshTurnNumbers(ref List<IPlayer> players, int next)
    {
        if (next == players.Count)
        {
            next = next % players.Count;
        }

        players[next].TurnNumber = 1;
        int counter = 0;

        for (int i = next + 1; i < players.Count; i++)
        {
            counter++;

            if (players[i].TurnNumber != 0)
            {
                players[i].TurnNumber = players[next].TurnNumber + counter;
            }
        }

        for (int i = 0; i < next; i++)
        {
            counter++;

            if (players[i].TurnNumber != 0)
            {
                players[i].TurnNumber = players[next].TurnNumber + counter;
            }
        }

        players = players.OrderBy(p => p.TurnNumber).ToList();
    }
}