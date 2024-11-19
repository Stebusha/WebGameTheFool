using Microsoft.EntityFrameworkCore.Storage.Json;

namespace BlazorCardGame.Models;
public class Player
{
    private const int REQUIRED_CARDS_COUNT = 6;
    public List<Card> inHand { get; set; } = new List<Card>();
    public PlayerType playerType { get; set; } = PlayerType.Human;
    public string Name { get; set; } = string.Empty;
    public int TurnNumber { get; set; }
    public bool Taken { get; set; }
    public bool IsFool { get; set; }
    public bool IsAttack { get; set; } = true;
    public void RefreshPlayable()
    {
        List<Card> selectedCard = new List<Card>();

        foreach (var card in inHand)
        {
            if (card.IsSelected)
            {
                selectedCard.Add(card);
                break;
            }
        }

        if (selectedCard.Count == 0)
        {
            foreach (var card in inHand)
            {
                card.IsPlayable = true;
            }
        }
        else
        {
            List<RankType> ranks = new List<RankType>();

            foreach (var card in inHand)
            {
                if (card.IsSelected)
                {
                    ranks.Add(card.Rank);
                    continue;
                }

                card.IsPlayable = false;
            }

            if (ranks != null)
            {
                foreach (var card in inHand)
                {
                    foreach (var rank in ranks)
                    {
                        if (card.Rank == rank)
                        {
                            card.IsPlayable = true;
                        }
                    }
                }
            }
        }
    }

    public void RefreshPlayableForBeat(Card cardToBeat)
    {
        foreach (var card in inHand)
        {
            if (card > cardToBeat)
            {
                card.IsPlayable = true;
            }
            else
            {
                card.IsPlayable = false;
            }
        }
    }
    public bool CanBeSelected(Table gameTable)
    {
        if (IsAttack)
        {
            if (CanBeAttacking(inHand, gameTable))
            {
                List<Card> attackingCards = GetCardsForAttack(gameTable);

                foreach (var card in inHand)
                {
                    foreach (var attackingCard in attackingCards)
                    {
                        if (card == attackingCard)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        else
        {
            for (int i = 0; i < gameTable.Length(); i++)
            {
                if (CanBeDefend(gameTable.GetCard(i)).Item1)
                {
                    return true;
                }
            }

            return false;
        }

    }
    public void Sort()
    {
        //sort all cards in hand
        inHand = inHand.OrderBy(c => c.Rank).ToList();

        List<Card> trumpCards = new List<Card>();

        //remember trump cards
        foreach (var card in inHand)
        {
            if (card.Suit == Deck.s_trumpSuit)
            {
                trumpCards.Add(card);
            }
        }

        if (trumpCards != null)
        {
            //remove trump cards from hands
            foreach (var trump in trumpCards)
            {
                inHand.Remove(trump);
            }

            //sort trumps
            trumpCards = trumpCards.OrderBy(t => t.Rank).ToList();

            //add sorted trumps to hand
            foreach (var trump in trumpCards)
            {
                inHand.Add(trump);
            }
        }
    }
    public void RefillHand(Deck deck)
    {
        if (inHand.Count == 0)
        {
            inHand = deck.DrawCards(REQUIRED_CARDS_COUNT);
            Sort();
        }
        else if (inHand.Count < REQUIRED_CARDS_COUNT)
        {
            inHand.AddRange(deck.DrawCards(REQUIRED_CARDS_COUNT - inHand.Count));
            Sort();
        }

        foreach (var card in inHand)
        {
            if (!card.IsPlayable)
            {
                card.IsPlayable = true;
            }

            card.IsSelected = false;
        }
    }

    //check exist cards for attack
    private bool CanBeAttacking(List<Card> cards, Table gameTable)
    {
        if (gameTable.Length() == 0)
        {
            return true;
        }

        foreach (var card in cards)
        {
            for (int i = 0; i < gameTable.Length(); i++)
            {
                if (card.Rank == gameTable.GetCard(i).Rank)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //return cards for attack
    public List<Card> GetCardsForAttack(Table gameTable)
    {
        List<Card> cardsForAttack = new List<Card>();

        if (CanBeAttacking(inHand, gameTable))
        {
            IsAttack = true;

            if (gameTable.Length() == 0)
            {
                return inHand;
            }

            foreach (var card in inHand)
            {
                for (int i = 0; i < gameTable.Length(); i++)
                {
                    if (card.Rank != gameTable.GetCard(i).Rank)
                    {
                        card.IsPlayable = false;
                        continue;
                    }

                    cardsForAttack.Add(card);
                    card.IsPlayable = true;
                }
            }
        }

        cardsForAttack = cardsForAttack.Distinct().ToList();

        return cardsForAttack;
    }

    //attack chosen card
    public List<Card> Attack(Table gameTable)
    {
        bool isAttacking = CanBeAttacking(inHand, gameTable);

        List<Card> selectedCardForAttack = new List<Card>();

        if (isAttacking)
        {
            List<Card> attackingCards = GetCardsForAttack(gameTable);

            if (attackingCards.Count != 0)
            {
                foreach (var attackingCard in attackingCards)
                {
                    if (!attackingCard.IsSelected)
                    {
                        continue;
                    }

                    selectedCardForAttack.Add(attackingCard);

                }

                foreach (var selectedCard in selectedCardForAttack)
                {
                    gameTable.AddCardToTable(selectedCard);

                    inHand.Remove(selectedCard);
                    Sort();

                    attackingCards.Remove(selectedCard);
                }
            }
        }

        return selectedCardForAttack;
    }

    //check attacking card can be beaten
    private (bool, List<Card>) CanBeDefend(Card cardToDefend)
    {
        List<Card> cardsToBeat = new List<Card>();

        foreach (var card in inHand)
        {
            if (card > cardToDefend)
            {
                cardsToBeat.Add(card);
            }
        }

        if (cardsToBeat.Count != 0)
        {
            return (true, cardsToBeat);
        }

        return (false, cardsToBeat);
    }
    private bool CanBeBeaten(List<Card> attackingCards, Table gameTable)
    {

        if (gameTable.Length() == 0)
        {
            return false;
        }

        foreach (var card in inHand)
        {
            foreach (var attackingCard in attackingCards)
            {
                if (card > attackingCard)
                {
                    return true;
                }
            }

        }

        return false;

    }

    //return cards for defense from attacking card
    private List<Card> GetCardsforDefense(List<Card> attackingCards, Table gameTable)
    {

        List<Card> defenseCards = new List<Card>();

        if (gameTable.Length() != 0)
        {

            foreach (var card in inHand)
            {
                foreach (var attackingCard in attackingCards)
                {
                    if (card > attackingCard)
                    {
                        defenseCards.Add(card);
                        card.IsPlayable = true;
                        break;
                    }
                    else
                    {
                        card.IsPlayable = false;
                    }
                }

            }
        }

        return defenseCards;
    }

    //return chosen card to defend
    private List<Card> GetCardToDefend(List<Card> attackingCards, Table gameTable)
    {
        List<Card> defendingCards = GetCardsforDefense(attackingCards, gameTable);

        return defendingCards;
    }

    //defend
    public void Defend(List<Card> attackingCards, Table gameTable)
    {
        bool beaten = CanBeBeaten(attackingCards, gameTable);
        List<Card> defendingCards = GetCardToDefend(attackingCards, gameTable);

        if (beaten)
        {
            foreach (var defendingCard in defendingCards)
            {
                gameTable.AddCardToTable(defendingCard);
                inHand.Remove(defendingCard);
            }

            Sort();
        }
        else
        {
            TakeAllCards(gameTable);
        }
    }

    //taken all cards from the game table, set property Taken
    public void TakeAllCards(Table gameTable)
    {
        Taken = true;

        List<Card> onTableCards = gameTable.TakeCardsFromTable();

        inHand.AddRange(onTableCards);
        Sort();
    }

    //output cards for console
    public string ToString(List<Card> cards)
    {
        string cardDrawnString = string.Empty;
        cardDrawnString = $"\nКарты игрока {Name}: \n";

        if (cards.Count > 6)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards.Count % 6 > 0)
                {
                    if (i != 0 && i % 6 == 0)
                    {
                        cardDrawnString += "\n\n";
                    }
                }

                Card tempCard = cards[i];
                cardDrawnString += $"[{i + 1}] - {tempCard}\t";
            }
        }
        else
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card tempCard = cards[i];
                cardDrawnString += $"[{i + 1}] - {tempCard}\t";
            }
        }

        return cardDrawnString;
    }
}
