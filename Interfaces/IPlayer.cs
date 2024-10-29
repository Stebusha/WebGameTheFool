namespace TheFool;

public interface IPlayer
{
    public string Name { get; set; }
    public int TurnNumber { get; set; }
    public bool Taken { get; set; }
    public bool IsFool { get; set; }
    //get cards from deck
    public void RefillHand(Deck deck);
    //return cards in hand
    public List<Card> GetCards();
    //return cards for attack
    public List<Card> GetCardsForAttack(Table gameTable);
    //attack
    public Card Attack(Table gameTable);
    //defend
    public void Defend(Card attackingCard, Table gameTable);
    //take all cards from game table
    public void TakeAllCards(Table gameTable);
}