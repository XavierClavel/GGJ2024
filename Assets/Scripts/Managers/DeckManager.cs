using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DeckManager
{
    private static int handSize = 3;
    private static List<string> deck;
    private static List<string> pickPile;
    private static List<string> discardPile;
    private static List<string> hand;
    
    public static void IncreaseHandSize()
    {
        handSize++;
    }

    public static void AddCardToDeck(string key)
    {
        deck.Add(key);
    }

    public static void UseCard(string key)
    {
        hand.Remove(key);
        discardPile.Add(key);
    }
    
    public static List<string> PickCards()
    {
        List<string> newCards = new List<string>();
        while (hand.Count < handSize)
        {
            if (pickPile.isEmpty())
            {
                foreach (var discardedCard in discardPile)
                {
                    pickPile.Add(discardedCard);
                }

                discardPile = new List<string>();
            }
            string key = pickPile.popRandom();
            Debug.Log($"Picked {key}");
            hand.Add(key);
            newCards.Add(key);
        }

        return newCards;
    }

    public static void ResetPiles()
    {
        hand = new List<string>();
        pickPile = new List<string>();
        discardPile = new List<string>();
        foreach (var key in deck)
        {
            pickPile.Add(key);
        }
        pickPile.Shuffle();
    }

    public static void ResetDeck()
    {
        deck = new List<string>
        {
            "Cuddle",
            "Tell",
            "Talk",
            "Sing",
            "Dance",
        };
    }
}