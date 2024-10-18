using System;
using System.Collections;
using System.Collections.Generic;

namespace TheFool
{
    public class GameController{
        private List<IPlayer> players = new List<IPlayer>();
        private ScoreTable scoreTable = new ScoreTable();
        private Table gameTable = new Table();
        private const int MAX_CARDS_TO_ATTACK = 6;
        private Deck deck = new Deck();
        private Dictionary<string,bool> fools = new Dictionary<string, bool>();
        private bool Finished {get; set;}
        private bool TurnFinished {get; set;}
        private bool FirtsTurn {get; set;}
        public int PlayerCount{get; set;}
        public int BotPlayerCount{get; set;}
        
        //turn logic
        private void Turn(int turn){
            TurnFinished = false;
            Card attackingCard = new Card();
            int attacking = turn%players.Count;
            int defending = (turn+1)%players.Count;
            int nextAttacking = (turn+2)%players.Count; 
            foreach (var p in players){
                p.Taken = false;
            }
            while(!TurnFinished){
                if(FirtsTurn){
                    Console.WriteLine("\nНачало партии\n");
                    Console.WriteLine($"Козырная масть - {Deck.trumpSuit}");
                    for(int i=0;i<MAX_CARDS_TO_ATTACK-1;i++){
                        if(players[defending].Taken){
                            TurnFinished=true;
                            FirtsTurn=false;
                            break;       
                        }
                        if(players[attacking].GetCardsForAttack(gameTable).Count!=0){
                            attackingCard = players[attacking].Attack(gameTable);
                            players[defending].Defend(attackingCard,gameTable);
                            if(!players[defending].Taken&&players[defending].GetCards().Count!=0){
                                if(players.Count>2&&players[nextAttacking].GetCardsForAttack(gameTable).Count!=0&&i!=4){
                                    i++;
                                    attackingCard = players[nextAttacking].Attack(gameTable);
                                    players[defending].Defend(attackingCard,gameTable);
                                }
                            }
                            else{
                                TurnFinished = true;
                                FirtsTurn=false;
                                break;
                            }
                        }
                        else{
                            TurnFinished = true;
                            FirtsTurn=false;
                            break;
                        }
                    }
                    if(!TurnFinished){                      
                        TurnFinished = true;
                        FirtsTurn=false;
                    }
                    Console.WriteLine("\nКонец хода");
                }
                else{
                    if(players.Count==1){
                        break;
                    }
                    Console.WriteLine($"\nНачало хода: ");
                    for(int i=0;i<MAX_CARDS_TO_ATTACK;i++){
                        if(players[defending].Taken||players[defending].GetCards().Count==0||gameTable.Length()==12){
                            TurnFinished=true;
                            break;       
                        }
                        if(players[attacking].GetCardsForAttack(gameTable).Count!=0){
                            attackingCard = players[attacking].Attack(gameTable);
                            players[defending].Defend(attackingCard,gameTable);
                            if(!players[defending].Taken&&players[defending].GetCards().Count!=0&&i!=5){
                                if(players.Count>2&&players[nextAttacking].GetCardsForAttack(gameTable).Count!=0){
                                    i++;
                                    attackingCard = players[nextAttacking].Attack(gameTable);
                                    players[defending].Defend(attackingCard,gameTable);
                                }
                            }
                            else{
                                TurnFinished=true;
                                break;
                            }
                        }
                        else if(gameTable.Length()!=0&&i!=6){
                            if(!players[defending].Taken){
                                if(players.Count>2&&players[nextAttacking].GetCardsForAttack(gameTable).Count!=0){
                                    i++;
                                    attackingCard = players[nextAttacking].Attack(gameTable);
                                    players[defending].Defend(attackingCard,gameTable);
                                }
                            }
                            else{
                                TurnFinished=true;
                                break;
                            }
                        }
                        // else if(gameTable.Length()==12){
                        //     TurnFinished = true;
                        //     break;
                        // }
                        if(!TurnFinished){
                            TurnFinished = true;
                        }
                    } 
                    Console.WriteLine("\nКонец хода");
                }   
            }
            gameTable.ClearTable();
            Console.ReadLine();
            Console.Clear();            
        }
        
        //launch game, set start info, set trump, player's turns, check the winning condition
        public void Game(int playerCount,int AIPlayerCount, in bool repeat){
            PlayerCount=playerCount;
            BotPlayerCount=AIPlayerCount;
            deck = new Deck();
            FirtsTurn = true;
            Finished = false;
            deck.Shuffle();
            deck.Trump();
            if(!repeat){
                players = new List<IPlayer>();
                if(playerCount+AIPlayerCount==2&&playerCount==0){
                    AIPlayer aIPlayer = new AIPlayer();
                    aIPlayer.RefillHand(deck);
                    players.Add(aIPlayer);
                    players[0].Name = "Бот 1";
                    fools.Add(players[0].Name, players[0].IsFool);
                    AIPlayer aIPlayer1 = new AIPlayer();
                    aIPlayer1.RefillHand(deck);
                    players.Add(aIPlayer1);    
                    players[1].Name = "Бот 2";   
                    fools.Add(players[1].Name, players[1].IsFool);        
                }
                else if(playerCount+AIPlayerCount==2&&playerCount==1){
                    Player player = new Player();
                    if(scoreTable.IsNameExistInScores(player.Name)){
                        Console.WriteLine("Имя {0} уже существует, выбрать другое? (да/нет)",player.Name);
                        if(Console.ReadLine().ToLower()=="да"){
                            player.Name = Console.ReadLine();
                        }
                    }   
                    player.RefillHand(deck);
                    players.Add(player);
                    fools.Add(player.Name, player.IsFool);
                    AIPlayer aIPlayer1 = new AIPlayer();
                    aIPlayer1.RefillHand(deck);
                    players.Add(aIPlayer1);    
                    players[1].Name = "Бот 1";
                    fools.Add(players[1].Name, players[1].IsFool);
                }
                else{
                    for(int i = 0;i<playerCount; i++){
                        Player player = new Player();
                        if(scoreTable.IsNameExistInScores(player.Name)){
                            Console.WriteLine("Имя {0} уже существует, выбрать другое? (да/нет)",player.Name);
                            if(Console.ReadLine().ToLower()=="да"){
                                player.Name = Console.ReadLine();
                            }
                        }
                        player.RefillHand(deck);
                        players.Add(player);
                        fools.Add(player.Name, player.IsFool);
                    }
                    for(int i = playerCount;i<playerCount+AIPlayerCount; i++){
                        AIPlayer aIPlayer = new AIPlayer();
                        aIPlayer.RefillHand(deck);
                        players.Add(aIPlayer);
                        players[i].Name = $"Бот {i+1}";
                        fools.Add(players[i].Name, players[i].IsFool);
                    }   
                }
            }
            else{
                players = new List<IPlayer>();
                if(playerCount+AIPlayerCount==2&&playerCount==0){
                    foreach(var f in fools){
                        AIPlayer aIPlayer = new AIPlayer(f.Key,f.Value);
                        aIPlayer.RefillHand(deck);
                        players.Add(aIPlayer);
                    }     
                }
                else{
                    foreach(var f in fools){
                        if(f.Key.Contains("Бот")){
                            AIPlayer aIPlayer = new AIPlayer(f.Key,f.Value);
                            aIPlayer.RefillHand(deck);
                            players.Add(aIPlayer);
                        }
                        else{
                            Player player = new Player(f.Key,f.Value);
                            player.RefillHand(deck);
                            players.Add(player);
                        }
                    }
                }
                // foreach(var p in players){
                //     Console.WriteLine($"{p.Name} - fool - {p.IsFool}");
                // }
            }
            List<Card> firstTrumps = new List<Card>();
            foreach(var p in players){
               firstTrumps.Add(GetFirstTrump(p.GetCards()));
            }
            int first = firstTurnNumbers(firstTrumps);
            Console.WriteLine($"\nПервым ходит игрок {players[first].Name}\n");
            players[first].TurnNumber=1;
            if(players.Count==2){
                foreach(var p in players){
                    if(p.TurnNumber==0){
                        p.TurnNumber=2;
                        break;
                    }
                }
            }
            else{
                SetStartTurnNumbers(ref players,first);
            }
            foreach(var p in players){
                p.IsFool=false;
                fools[p.Name]=false;
            }
            int turns = 0;
            while(!Finished){
                // int nextTurn=1;
                Console.WriteLine($"Козырная масть - {Deck.trumpSuit}");
                Console.WriteLine($"Карт в колоде: {deck.CardsAmount}");
                foreach(var p in players){
                    Console.WriteLine($"Количество карт игрока {p.Name} : {p.GetCards().Count}");
                }
                Turn(turns);
                if(deck.CardsAmount!=0){
                    players[turns%players.Count].RefillHand(deck);
                    players[(turns+1)%players.Count].RefillHand(deck);
                    if(players.Count>2){
                        players[(turns+2)%players.Count].RefillHand(deck);
                    }
                    Console.WriteLine("Игроки взяли карты");
                }
                if(!players[(turns+1)%players.Count].Taken){
                    turns++;   
                }
                else{
                    turns+=2;
                }
                RefreshTurnNumbers(ref players,turns);
                Win();
                turns=0;
            }   
        }
        
        //get first trump card in player's hand
        private Card GetFirstTrump(List<Card> cards){
            Card firstTrumpCard = new Card();
            foreach (var c in cards){
                if(c.Suit==Deck.trumpSuit){
                    firstTrumpCard = c;
                    break;
                }
            }
            return firstTrumpCard;
        }
        
        //set first turn number of players use checking min trumps in players' hand or choosing next player after fool
        private int firstTurnNumbers(List<Card> firstTrumpCards){
            int number = 0;
            foreach(var player in players){
                number++;
                if(player.IsFool==true){
                    return number%players.Count;
                }
                else if(number==players.Count){
                    number=0;
                    for(int i=1;i<firstTrumpCards.Count;i++){
                        if(firstTrumpCards[i].Rank<firstTrumpCards[i-1].Rank){
                            number++;
                        }
                    }   
                }
            }
            return number;
        }
        
        //check the winner, update and display score table
        private void Win(){
            if(deck.CardsAmount==0){
                int lefts = 0;
                foreach(var player in players){
                    if(player.GetCards().Count==0){
                        lefts++;
                    }
                    else{
                        break;
                    }
                }
                if(lefts==players.Count){
                    Finished = true;
                    Console.WriteLine($"Колода закончилась. Конец партии. Ничья.");
                    scoreTable.DisplayScores();
                }
                else if(BotPlayerCount+PlayerCount==2){
                    if (players[1].GetCards().Count==0){
                    Finished = true;
                    Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {players[1].Name}."); 
                    players[0].IsFool = true;
                    fools[players[0].Name] = true;
                    int score = 1;
                    scoreTable.AddScore(players[1].Name,score);
                    scoreTable.DisplayScores();
                    }
                    else if(players[0].GetCards().Count==0){
                        Console.WriteLine($"Колода закончилась. Конец партии. Победил игрок {players[0].Name}.");
                        Finished = true;
                        players[1].IsFool = true;
                        fools[players[1].Name] = true;
                        int score = 1;
                        scoreTable.AddScore(players[0].Name,score);
                        scoreTable.DisplayScores();
                    }
                }
                else{
                    if(players.Count > 1){
                        for(int p=0; p<players.Count;p++){
                            if (players[p].GetCards().Count==0){
                                Console.WriteLine($"Карты закончились. Игрок {players[p].Name} вышел.\n");
                                players[p].IsFool = false;
                                players.Remove(players[p]);
                            }
                        }
                    }
                    else{
                        Console.WriteLine($"Колода закончилась. Конец партии. Проиграл игрок {players[0].Name}.");
                        Finished = true;
                        int score = 1;
                        players[0].IsFool = true;
                        fools[players[0].Name] = true;
                        scoreTable.AddFool(players[0].Name,score);
                        scoreTable.DisplayFools();
                    }
                }   
            }
        }

        //set start turn numbers
        private void SetStartTurnNumbers(ref List<IPlayer> players, int first){
            players[first].TurnNumber = 1;
            int counter = 0;
            for(int i=first+1; i<players.Count;i++){
                counter++;
                if(players[i].TurnNumber==0){
                    players[i].TurnNumber = players[first].TurnNumber+counter;
                }
            }
            for (int i=0; i<first;i++){
                counter++;
                if(players[i].TurnNumber==0){
                    players[i].TurnNumber = players[first].TurnNumber+counter;
                }
            }
            players = players.OrderBy(p=>p.TurnNumber).ToList();
        }
        
        //rerfesh turn numbers for nest turn
        private void RefreshTurnNumbers(ref List<IPlayer> players, int next){
            if(next==players.Count){
                next=next%players.Count;
            }
            players[next].TurnNumber = 1;
            int counter = 0;
            for(int i=next+1; i<players.Count;i++){
                counter++;
                if(players[i].TurnNumber!=0){
                    players[i].TurnNumber = players[next].TurnNumber+counter;
                }
            }
            for (int i=0; i<next;i++){
                counter++;
                if(players[i].TurnNumber!=0){
                    players[i].TurnNumber = players[next].TurnNumber+counter;
                }
            }
            players = players.OrderBy(p=>p.TurnNumber).ToList();
        }         
    }
}