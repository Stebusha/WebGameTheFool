using BlazorCardGame.Models;

namespace BlazorCardGame.Services
{
    public class AIPlayerControlService:IPlayerControl{
        public void RefillHand(Deck deck){

        }
        public List<Card> GetCardsForAttack(){
            List<Card> cardsForAttack = new List<Card>();
            return cardsForAttack;
        }
        public Card Attack(){
            Card attackingCard = new Card();
            return attackingCard;
        }
        public void Defend(Card attackingCard, Table gametable){

        }
        public void TakeCards(Table gameTable){

        }
        public void EndTurn(){

        }
    }
}