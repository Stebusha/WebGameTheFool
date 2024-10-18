using System;
using System.Collections.Generic;

namespace TheFool
{
    public class Table{
        private List<Card> onTable = new List<Card>();
       
        //return card on table by index
        public Card GetCard(int index) => onTable[index];   
        
        //return length of card in game
        public int Length() => onTable.Count;
        
        //return cards from table
        public List<Card> TakeCardsFromTable() => onTable;
       
        //add card on table
        public void AddCardToTable(Card card) => onTable.Add(card);

        //remove card from table
        public void RemoveCardFromTable(Card card) => onTable.Remove(card);
        
        //clear table
        public void ClearTable() => onTable.Clear();

        //output for console
        public string ToString(List<Card> onTable){
            string onTableString = "";
            onTableString = "\n\n Карты в игре: \t";
            for(int i=0;i<onTable.Count;i++){
                Card tempCard = onTable[i];
                onTableString+=tempCard.ToString();
            }
            return onTableString;
        }
    }
}