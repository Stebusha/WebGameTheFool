using BlazorCardGame.Models;

namespace BlazorCardGame.Services;

public class AIPlayerControlService : IPlayerControl
{
    List<Player>? AIPlayers;
    private void SetNames(ref List<Player> players)
    {
        Random random = new Random();
        List<int> numbers = new List<int>();
        if (players != null)
        {
            foreach (var player in players)
            {
                int number = random.Next(0, 6);
                if (!numbers.Contains(number))
                {
                    player.Name = Enum.GetName(typeof(AINames), number) ?? "AI";
                    numbers.Add(number);
                }
                else
                {
                    while (numbers.Contains(number))
                    {
                        number = random.Next(0, 6);
                    }
                    player.Name = Enum.GetName(typeof(AINames), number) ?? "AI";
                    numbers.Add(number);
                }
            }
        }
    }
    public void RefillHand(Deck deck)
    {
        if (AIPlayers != null)
        {
            foreach (var ai in AIPlayers)
            {
                if (ai.inHand.Count >= 6)
                {
                    continue;
                }

                if (ai.inHand.Count == 0)
                {
                    ai.inHand = deck.DrawCards(6);
                    ai.inHand.Sort();
                }
                else
                {
                    ai.inHand.AddRange(deck.DrawCards(6 - ai.inHand.Count));
                    ai.inHand.Sort();
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
    public void TakeCards(Table gameTable, ref Player AIplayer)
    {
        AIplayer.Taken = true;
        List<Card> onTableCards = gameTable.TakeCardsFromTable();
        AIplayer.inHand.AddRange(onTableCards);
        AIplayer.inHand.Sort();
    }
    public void EndTurn()
    {

    }
}
