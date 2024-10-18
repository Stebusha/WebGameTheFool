namespace BlazorWebApp.Models{
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
    }
}