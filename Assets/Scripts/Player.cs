using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Card selectedCard;
    private static CardHolder selectedCardHolder;
    public static CardHandler[] placedCards = new CardHandler[3]; 
    public static Card getSelectedCard() => selectedCard;
    public static void setSelectedCard(Card card)
    {
        selectedCard = card;
    }

    public static void placeCard()
    {
        if (getSelectedCardHolder() == null) return;
        placedCards[getSelectedCardHolder().index] = getSelectedCard().getCardInfo();
    }

    public static CardHolder getSelectedCardHolder() => selectedCardHolder;
    public static void setSelectedCardHolder(CardHolder cardHolder) => selectedCardHolder = cardHolder;

    public void ValidateCombination()
    {
        foreach (var card in placedCards)
        {
            if (card == null) continue;
            Debug.Log(card.getKey());
        }
    }
    private void Awake()
    {
       setSelectedCard(null);
       setSelectedCardHolder(null);
       placedCards = new CardHandler[3]
       {
           null,
           null,
           null
       };
    }

}
