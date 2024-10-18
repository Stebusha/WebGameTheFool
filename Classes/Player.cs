using System;
using System.Collections.Generic;

namespace TheFool;
public class Player:IPlayer {
    PlayerHand playerHand = new PlayerHand();
    public Player(){
        Console.WriteLine("Введите имя: ");
        Name = Console.ReadLine();

    }

    public Player(string _name, bool _fool) {
        Name = _name;
        IsFool = _fool;
    }
    public int TurnNumber{get;set;}
    public bool Taken{get; set;}
    public string Name{get;set;}
    public bool IsFool {get;set;}

    //return cards in hand
    public List<Card> GetCards()=> playerHand.cards;
    //draw cards from deck
    public void RefillHand(Deck deck){   
        if(playerHand.cards.Count==0){
            playerHand.cards = deck.DrawCards(6);
            playerHand.Sort();
        }
        else if(playerHand.cards.Count<6){
            playerHand.cards.AddRange(deck.DrawCards(6-playerHand.cards.Count));
            playerHand.Sort();
        } 
    }
    
    //check exist cards for attack
    private bool CanBeAttacking(List<Card> cards, Table gameTable){
        if (gameTable.Length() == 0){
            return true;
        }
        else{
            foreach(var card in cards){
                for(int i=0;i<gameTable.Length();i++){
                    if(card.Rank==gameTable.GetCard(i).Rank){
                        return true;
                    }
                }
            }
            return false;
        }
    }   
    
    //return cards for attack
    public List<Card> GetCardsForAttack(Table gameTable){
        List <Card> cardsForAttack = new List<Card>();
        if(CanBeAttacking(playerHand.cards,gameTable)){
            if(gameTable.Length()==0){
                return playerHand.cards;
            }
            else{
                foreach(var card in playerHand.cards){
                    for(int i=0;i<gameTable.Length();i++){
                        if(card.Rank==gameTable.GetCard(i).Rank){
                            cardsForAttack.Add(card);
                        }
                    }
                }
            }            
        }
        cardsForAttack = cardsForAttack.Distinct().ToList();
        return cardsForAttack;
    }
    
    //attack chosen card
    public Card Attack(Table gameTable){
        bool isAttacking = CanBeAttacking(playerHand.cards,gameTable);
        Card attackingCard = new Card();
        if(isAttacking){
            List<Card> attackingCards = GetCardsForAttack(gameTable);
            if(attackingCards.Count!=0){
                Console.WriteLine(ToString(attackingCards));
                Console.WriteLine("\nВыберите порядковый номер карты, которой хотите походить: ");
                bool settingNumber = false;
                while(!settingNumber){
                    string number = Console.ReadLine();
                    if(int.TryParse(number, out var index)){
                        if((index-1)>=0&&(index-1)<attackingCards.Count){
                            settingNumber=true;
                            attackingCard = attackingCards[index-1];
                            Console.WriteLine($"\nВы походили картой: {attackingCard}");
                            gameTable.AddCardToTable(attackingCard);
                            playerHand.RemoveCardFromHand(attackingCard);
                            attackingCards.Remove(attackingCard);
                            return attackingCard;
                        } 
                        else{
                            Console.WriteLine("Нет такого номера. Введите порядковый номер повторно: ");
                        }
                    }
                    else{
                        Console.WriteLine("Некорректный ввод .Введите порядковый номер повторно: ");
                    }
                }       
            }
            return attackingCard;
        }
        else{
            return attackingCard;
        }
    }
    
    //check attacking card can be beaten
    private bool CanBeBeaten(Card attackingCard,Table gameTable){
        if(gameTable.Length()==0){
            return false;
        }
        else{
            foreach(var card in playerHand.cards){
                if(card>attackingCard){
                    return true;  
                }
            }
            return false;
        }  
    }
    
    //return cards for defense from attacking card
    private List<Card> GetCardsforDefense(Card attackingCard, Table gameTable){
        List<Card> defenseCards = new List<Card>();
        if (gameTable.Length()!=0){
            foreach(var card in playerHand.cards){
                if(card>attackingCard){
                    defenseCards.Add(card);
                }   
            }
        }
        return defenseCards;    
    }
    
    //return chosen card to defend
    private Card GetCardToDefend(Card attackingCard,Table gameTable){
        Card cardToDefend = new Card();
        List<Card> defendingCards = GetCardsforDefense(attackingCard,gameTable);
        if(defendingCards.Count!=0){
            Console.WriteLine(ToString(defendingCards));
            Console.WriteLine("\nВыберите порядковый номер карты, которой хотите отбиться: ");
            bool settingNumber = false;
            while(!settingNumber){
                string number = Console.ReadLine();
                if(int.TryParse(number, out var index)){
                    if((index-1)>=0&&(index-1)<defendingCards.Count){
                        settingNumber=true;
                        cardToDefend = defendingCards[index-1];
                    } 
                    else{
                        Console.WriteLine("Нет такого номера. Введите порядковый номер повторно: ");
                    }
                }
                else{
                    Console.WriteLine("Некорректный ввод .Введите порядковый номер повторно: ");
                }
            }        
            
        }
        return cardToDefend;
    }
    
    //defend
    public void Defend(Card attackingCard, Table gameTable){
        bool beaten = CanBeBeaten(attackingCard,gameTable);
        Card defendingCard = GetCardToDefend(attackingCard,gameTable);
        if(beaten){
            Console.WriteLine($"\nВы отбились картой: {defendingCard}");
            gameTable.AddCardToTable(defendingCard);
            playerHand.RemoveCardFromHand(defendingCard);
        }
        else{
            Console.WriteLine("\nНечем отбиться");
            TakeAllCards(gameTable);
        }
    }
    
    //taken all cards from the game table, set property Taken
    public void TakeAllCards(Table gameTable){
        Taken = true;
        List<Card> onTableCards = gameTable.TakeCardsFromTable();
        playerHand.cards.AddRange(onTableCards);
        playerHand.Sort();
        Console.WriteLine("\nВы взяли карты :\n" +ToString(playerHand.cards));
    }

    //output cards for console
    public string ToString(List<Card> cards)
    {
        string cardDrawnString = "";
        cardDrawnString = $"\nКарты игрока {Name}: \n\n";
        if(cards.Count>6){
            for(int i = 0;i<cards.Count;i++){
                if(cards.Count%6>0){
                    if(i!=0&&i%6==0){
                        cardDrawnString+="\n\n";
                    }
                }
                Card tempCard = cards[i];
                cardDrawnString+=tempCard.ToString()+"\t\t";
            }
        }
        else{
            for(int i = 0;i<cards.Count;i++){
                Card tempCard = cards[i];
                cardDrawnString+=tempCard.ToString()+"\t\t";
            }
        } 
        return cardDrawnString;
    }
}