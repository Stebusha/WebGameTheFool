using BlazorWebApp.Models;

namespace BlazorWebApp.Services
{
    public interface IPlayerControl{
        public void RefillHand(Deck deck);
        public List<Card> GetCardsForAttack();
        public Card Attack();
        public void Defend(Card attackingCard, Table gametable);
        public void TakeCards(Table gameTable);
        public void EndTurn();
    }
    
}