using BlazorCardGame.Models;

namespace BlazorCardGame.Services
{
    public interface IPlayerControl{
        public void RefillHand(Deck deck);
        public List<Card> GetCardsForAttack();
        public Card Attack();
        public void Defend(Card attackingCard, Table gametable);
        public void TakeCards(Table gameTable, ref Player player);
        public void EndTurn();
    }
    
}