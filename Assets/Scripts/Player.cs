using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Card selectedCard;
    private static CardHolder selectedCardHolder;
    private static CardHolder lastSelectedCardHolder;
    public static Card[] placedCards = new Card[3];
    private static int handSize = 3;
    private static List<string> deck;
    private static List<string> pickPile;
    private static List<string> discardPile;
    private static List<string> hand;

    [SerializeField] private Transform cardsLayout;
    [SerializeField] private Card cardPrefab;
    
    public static Card getSelectedCard() => selectedCard;
    public static void setSelectedCard(Card card)
    {
        selectedCard = card;
    }

    public static void placeCard()
    {
        if (getSelectedCardHolder() == null) return;
        placedCards[getSelectedCardHolder().index] = getSelectedCard();
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
            string key = card.getCardInfo().getKey();
            Debug.Log(key);
            keys.Add(key);
            hand.Remove(key);
            discardPile.Add(key);
        }

        Recipe recipe = RecipeManager.getRecipe(keys);
        foreach (var output in recipe.getOutput())
        {
            Debug.Log($"Emotion : {output.Key}, Value : {output.Value}");
        }

        foreach (var ennemy in Ennemy.ennemiesList)
        {
            ennemy.ApplyEffect(recipe);
        }

        foreach (var card in placedCards)
        {
            if (card == null) continue;
            Destroy(card.gameObject);
        }
        
        ResetSlots();
    }
    
    private void Start()
    {
       PrepareWave();
    }

    private void PickCards()
    {
        while (hand.Count < handSize)
        {
            string key = pickPile.popRandom();
            Debug.Log($"Picked {key}");
            hand.Add(key);
            Card newCard = Instantiate(cardPrefab, cardsLayout);
            newCard.setup(key);
        }
    }

    private void PrepareWave()
    {
        deck = new List<string>
        {
            "Talk",
            "Tell",
            "Talk",
        };
        ResetSlots();
        hand = new List<string>();
        pickPile = new List<string>();
        discardPile = new List<string>();
        foreach (var key in deck)
        {
            pickPile.Add(key);
        }
        pickPile.Shuffle();
        PickCards();
    }

    private void ResetSlots()
    {
        setSelectedCard(null);
        setSelectedCardHolder(null);
        placedCards = new Card[3]
        {
            null,
            null,
            null
        };
    }

}
