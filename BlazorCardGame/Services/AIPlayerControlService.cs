using BlazorCardGame.Models;

namespace BlazorCardGame.Services;

public class AIPlayerControlService : IPlayerControl
{
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
    public void TakeCards(Table gameTable, Player AIplayer)
    {
        AIplayer.Taken = true;
        List<Card> onTableCards = gameTable.TakeCardsFromTable();
        AIplayer.inHand.AddRange(onTableCards);
        AIplayer.Sort();
    }
}
