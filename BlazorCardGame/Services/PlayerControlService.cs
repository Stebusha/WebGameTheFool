using BlazorCardGame.Models;

namespace BlazorCardGame.Services;
public class PlayerControlService : IPlayerControl
{

    public List<Card> GetCardsForAttack()
    {
        List<Card> cardsForAttack = new List<Card>();

        return cardsForAttack;
    }
    public Card Attack()
    {
        Card attackingCard = new Card();
        return attackingCard;
    }
    public void Defend(Card attackingCard, Table gametable)
    {

    }
    public void TakeCards(Table gameTable, Player player)
    {
        player.Taken = true;
        List<Card> onTableCards = gameTable.TakeCardsFromTable();
        player.inHand.AddRange(onTableCards);
        player.Sort();
    }
}