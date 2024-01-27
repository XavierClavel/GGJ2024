using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player instance;
    private static Card selectedCard;
    private static CardHolder selectedCardHolder;
    private static CardHolder lastSelectedCardHolder;
    public static Card[] placedCards = new Card[3];
    

    [SerializeField] private Transform cardsLayout;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private UpgradesManager upgradesPanel;
    [SerializeField] private RectTransform canvas;

    
    
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
            DeckManager.UseCard(key);
        }

        Recipe recipe = RecipeManager.getRecipe(keys);
        foreach (var output in recipe.getOutput())
        {
            Debug.Log($"Emotion : {output.Key}, Value : {output.Value}");
        }

        Ennemy[] ennemies = new Ennemy[Ennemy.ennemiesList.Count];
        Ennemy.ennemiesList.CopyTo(ennemies);
        foreach (var ennemy in ennemies)
        {
            ennemy.ApplyEffect(recipe);
        }

        foreach (var card in placedCards)
        {
            if (card == null) continue;
            Destroy(card.gameObject);
        }
        NewTurn();
    }

    private void Awake()
    {
        instance = this;
        DeckManager.ResetDeck();
    }

    private void Start()
    {
       PrepareWave();
    }

    private void NewTurn()
    {
        ResetSlots();
        List<string> newCards = DeckManager.PickCards();
        newCards.ForEach(key =>
            Instantiate(cardPrefab, cardsLayout).setup(key, cardsLayout));
    }

    

    public void PrepareWave()
    {
        ResetSlots();
        ResetHand();
        DeckManager.ResetPiles();
        NewTurn();
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

    private void ResetHand()
    {
        cardsLayout.KillAllChildren();
    }

    public static void WaveOver()
    {
        Debug.Log("Wave Over");
        instance.upgradesPanel.gameObject.SetActive(true);
        instance.upgradesPanel.DisplayUpgrades();
    }

}
