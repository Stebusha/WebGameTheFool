using BlazorCardGame.Enums;

namespace BlazorCardGame.Models;
public class Player
{
    public List<Card> inHand { get; set; } = new List<Card>();
    public PlayerType playerType { get; set; } = PlayerType.Human;
    public string Name { get; set; } = string.Empty;
    public int TurnNumber { get; set; }
    public bool Taken { get; set; }
    public bool IsFool { get; set; }
    public bool IsAttack { get; set; }

    public Player() { }
    public Player(string _name, bool _fool)
    {
        Name = _name;
        IsFool = _fool;
    }

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
            foreach (var card in inHand)
            {
                if (card == selectedCard.First())
                {
                    card.IsPlayable = true;
                    continue;
                }

                card.IsPlayable = false;
            }
        }
    }
    public void RefreshPlayableForAttack(Table table)
    {
        if (table.Length() == 0)
        {
            RefreshPlayable();
        }
        else
        {
            List<RankType> ranks = new List<RankType>();

            for (int i = 0; i < table.Length(); i++)
            {
                ranks.Add(table.GetCard(i).Rank);
            }

            ranks = ranks.Distinct().ToList();

            if (ranks.Count != 0)
            {
                foreach (var card in inHand)
                {
                    card.IsPlayable = false;
                }

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
    public void RefreshPlayableForBeat(Card cardToBeat, Table table)
    {
        foreach (var card in inHand)
        {
            if (table.Length() % 2 != 0)
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
            for (int i = 0; i < gameTable.Length(); i += 2)
            {
                if (CanBeBeaten(gameTable.GetCard(i), gameTable))
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

        List<Card> trumpCards = [];
        List<Card> jokers = [];

        //remember trump cards
        foreach (var card in inHand)
        {
            if (card.Suit == Deck.s_trumpSuit)
            {
                trumpCards.Add(card);
            }

            if (card.Rank == RankType.Joker)
            {
                jokers.Add(card);
            }
        }

        if (trumpCards is not null)
        {
            //remove trump cards from hands
            foreach (var trump in trumpCards)
            {
                inHand.Remove(trump);
            }

            //sort trumps
            if (trumpCards.Count > 1)
            {
                trumpCards = trumpCards.OrderBy(t => t.Rank).ToList();
            }

            //add sorted trumps to hand
            inHand.AddRange(trumpCards);
        }

        if (jokers is not null)
        {
            //remove joker cards from hands
            foreach (var joker in jokers)
            {
                inHand.Remove(joker);
            }

            //sort jokers
            if (jokers.Count > 1)
            {

                jokers = jokers.OrderBy(t => t.Suit).ToList();
            }

            //add sorted jokers to hand
            inHand.AddRange(jokers);
        }
    }
    public void RefillHand(Deck deck)
    {
        if (inHand.Count == 0)
        {
            inHand = deck.DrawCards(GameConstants.REQUIRED_CARDS_COUNT);
            Sort();
        }
        else if (inHand.Count < GameConstants.REQUIRED_CARDS_COUNT)
        {
            inHand.AddRange(deck.DrawCards(GameConstants.REQUIRED_CARDS_COUNT - inHand.Count));
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
    public Card Attack(Table gameTable)
    {
        bool isAttacking = CanBeAttacking(inHand, gameTable);

        Card selectedCardForAttack = new Card();

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

                    selectedCardForAttack = attackingCard;

                    foreach (var card in inHand)
                    {
                        if (card != selectedCardForAttack)
                        {
                            card.IsSelected = false;
                            card.IsPlayable = false;
                        }
                    }

                    break;
                }

                if (selectedCardForAttack.ImageUrl != "")
                {
                    gameTable.AddCardToTable(selectedCardForAttack);

                    inHand.Remove(selectedCardForAttack);
                    Sort();

                    attackingCards.Remove(selectedCardForAttack);
                }
            }
        }

        return selectedCardForAttack;
    }

    //check attacking card can be beaten
    // private (bool, List<Card>) CanBeDefend(Card cardToDefend)
    // {
    //     List<Card> cardsToBeat = new List<Card>();

    //     foreach (var card in inHand)
    //     {
    //         if (card > cardToDefend)
    //         {
    //             cardsToBeat.Add(card);
    //         }
    //     }

    //     if (cardsToBeat.Count != 0)
    //     {
    //         return (true, cardsToBeat);
    //     }

    //     return (false, cardsToBeat);
    // }
    private bool CanBeBeaten(Card attackingCard, Table gameTable)
    {

        if (gameTable.Length() == 0)
        {
            return false;
        }

        if (attackingCard.ImageUrl == "")
        {
            return false;
        }

        foreach (var card in inHand)
        {

            if (card > attackingCard)
            {
                return true;
            }
        }

        return false;

    }

    //return cards for defense from attacking card
    private List<Card> GetCardsforDefense(Card attackingCard, Table gameTable)
    {

        List<Card> defenseCards = new List<Card>();

        if (gameTable.Length() != 0)
        {

            foreach (var card in inHand)
            {

                if (card > attackingCard)
                {
                    defenseCards.Add(card);
                    card.IsPlayable = true;
                }
                else
                {
                    card.IsPlayable = false;
                }


            }
        }

        RefreshPlayableForBeat(attackingCard, gameTable);

        return defenseCards;
    }

    //return chosen card to defend
    private Card GetCardToDefend(Card attackingCards, Table gameTable)
    {
        Card defendingCard = new Card();
        List<Card> defendingCards = GetCardsforDefense(attackingCards, gameTable);

        int selectedCardCount = 0;

        foreach (var card in defendingCards)
        {
            if (card.IsSelected)
            {
                selectedCardCount++;
                break;
            }
        }

        if (selectedCardCount != 0)
        {
            foreach (var card in defendingCards)
            {
                if (card.IsSelected)
                {
                    defendingCard = card;
                    break;
                }
            }

            foreach (var card in defendingCards)
            {
                if (card != defendingCard)
                {
                    card.IsSelected = false;
                }
            }
        }


        return defendingCard;
    }

    //defend
    public Card Defend(Card attackingCard, Table gameTable)
    {
        bool beaten = CanBeBeaten(attackingCard, gameTable);
        Card defendingCard = GetCardToDefend(attackingCard, gameTable);

        if (beaten)
        {
            if (defendingCard.ImageUrl != "")
            {
                gameTable.AddCardToTable(defendingCard);
                inHand.Remove(defendingCard);

                Sort();
            }
        }
        else
        {
            Taken = true;
        }

        return defendingCard;
    }

    //taken all cards from the game table, set property Taken
    public void TakeCards(Table gameTable)
    {
        Taken = true;

        List<Card> onTableCards = gameTable.TakeCardsFromTable();

        inHand.AddRange(onTableCards);
        inHand = inHand.Distinct().ToList();

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
