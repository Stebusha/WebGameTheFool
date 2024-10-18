using System;
using System.Collections.Generic;

namespace TheFool
{
    public class PlayerHand{
        public List<Card> cards = new List<Card>();
        
        //return card by index
        public Card GetCard(int index) => cards.ElementAt(index);
        
        //remove card from hand
        public void RemoveCardFromHand(Card card){
            cards.Remove(card);
            Sort();
        }
        
        //sorted cards in hand by rank, trumps also sorted by rank in the end of hand
        public void Sort(){
            cards = cards.OrderBy(c=>c.Rank).ToList();
            List<Card> trumpCards = new List<Card>();
            foreach(var card in cards){
                if(card.Suit==Deck.trumpSuit){
                    trumpCards.Add(card);
                }
            }
            //Console.WriteLine();
            //Console.WriteLine("Trumps:");
            //foreach (var trump in trumpCards){
            //    Console.WriteLine(trump);
            //}
            if(trumpCards!=null){
                foreach(var trump in trumpCards){
                    cards.Remove(trump);
                }
            }
            //Console.WriteLine();
            //Console.WriteLine("Without Trumps:");
            //foreach (var card in cards){
            //    Console.WriteLine(card);   
            //}
            //Console.WriteLine();
            trumpCards = trumpCards.OrderBy(t=>t.Rank).ToList();
            foreach (var trump in trumpCards){
                cards.Add(trump);
            }
            //Console.WriteLine("Sorted:");
            //foreach (var card in cards){
            //    Console.WriteLine(card);
            //}  
        }
    }    

}
