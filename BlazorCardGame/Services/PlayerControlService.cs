using BlazorCardGame.Models;

namespace BlazorCardGame.Services;
public class PlayerControlService : IPlayerControl
{
    List<Player>? players;
    public void RefillHand(Deck deck)
    {
        if (players != null)
        {
            foreach (var player in players)
            {
                if (player.inHand.Count >= 6)
                {
                    continue;
                }

                if (player.inHand.Count == 0)
                {
                    player.inHand = deck.DrawCards(6);
                    player.inHand.Sort();
                }
                else
                {
                    player.inHand.AddRange(deck.DrawCards(6 - player.inHand.Count));
                    player.inHand.Sort();
                }
            }
        }
    }
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
    public void TakeCards(Table gameTable, ref Player player)
    {
        player.Taken = true;
        List<Card> onTableCards = gameTable.TakeCardsFromTable();
        player.inHand.AddRange(onTableCards);
        player.inHand.Sort();
    }
    public void EndTurn()
    {

    }
}