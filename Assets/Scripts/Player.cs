using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Card selectedCard;
    private static CardHolder selectedCardHolder;
    private static CardHolder lastSelectedCardHolder;
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

    public static void removeCard()
    {
        if (lastSelectedCardHolder == null) return;
        placedCards[lastSelectedCardHolder.index] = null;
    }

    public static CardHolder getSelectedCardHolder() => selectedCardHolder;
    public static void setSelectedCardHolder(CardHolder cardHolder)
    {
        selectedCardHolder = cardHolder;
        if (cardHolder != null) lastSelectedCardHolder = cardHolder;
    }

    public void ValidateCombination()
    {
        List<string> keys = new List<string>();
        foreach (var card in placedCards)
        {
            if (card == null) continue;
            Debug.Log(card.getKey());
            keys.Add(card.getKey());
        }

        Recipe recipe = RecipeManager.getRecipe(keys);
        foreach (var output in recipe.getOutput())
        {
            Debug.Log($"Emotion : {output.Key}, Value : {output.Value}");
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
