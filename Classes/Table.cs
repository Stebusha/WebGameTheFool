namespace TheFool;
public class Table
{
    private List<Card> _onTable = new List<Card>();

    //return card on table by index
    public Card GetCard(int index) => _onTable[index];

    //return length of card in game
    public int Length() => _onTable.Count;

    //return cards from table
    public List<Card> TakeCardsFromTable() => _onTable;

    //add card on table
    public void AddCardToTable(Card card) => _onTable.Add(card);

    //remove card from table
    public void RemoveCardFromTable(Card card) => _onTable.Remove(card);

    //clear table
    public void ClearTable() => _onTable.Clear();

    //output for console
    public override string ToString()
    {
        string onTableString = string.Empty;
        onTableString = "\nКарты в игре: \t";

        for (int i = 0; i < _onTable.Count; i++)
        {
            Card tempCard = _onTable[i];
            onTableString += tempCard.ToString() + "\t";
        }

        return onTableString;
    }
}
