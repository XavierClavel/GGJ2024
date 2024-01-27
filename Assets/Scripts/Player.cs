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
        placedCards[getSelectedCardHolder().index] = card.getCardInfo();
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
