using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player instance;
    private static Card selectedCard;
    [SerializeField] private List<CardHolder> cardHolders;
    private static int health;
    private static int maxHealth;
    private static int gold;

    [SerializeField] private TextMeshProUGUI pickPileDisplay;
    [SerializeField] private TextMeshProUGUI discardPileDisplay;
    [SerializeField] private TextMeshProUGUI goldDisplay;
    [SerializeField] private TextMeshProUGUI healthDisplay;
    [SerializeField] private Transform cardsLayout;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private UpgradesManager upgradesPanel;
    [SerializeField] private RectTransform emptyGameObject;

    public static void setPickPileAmount(int amount)
    {
        instance.pickPileDisplay.SetText($"{amount}x");
    }

    public static void setDiscardPileAmount(int amount)
    {
        instance.discardPileDisplay.SetText($"{amount}x");
    }

    public static void IncreaseMaxHealth()
    {
        maxHealth += 2;
        TakeDamage(0);
    }

    public static Card getSelectedCard() => selectedCard;
    public static void setSelectedCard(Card card)
    {
        selectedCard = card;
    }

    public void ValidateCombination()
    {
        List<string> keys = new List<string>();
        foreach (var cardHolder in cardHolders)
        {
            Card card = cardHolder.selectedCard;
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

        foreach (var cardHolder in cardHolders)
        {
            cardHolder.UseCard();
        }

        if (Ennemy.isWaveOver())
        {
            WaveOver();
            return;
        }
        NewTurn();
    }

    private IEnumerator HideCards(List<Card> cards)
    {
        foreach (var card in cards)
        {
            card.Hide();
            yield return 0.2f;
        }
    }

    private void Awake()
    {
        instance = this;
        DeckManager.ResetDeck();
        WaveManager.ResetWave();
        selectedCard = null;
        cardHolders.ForEach(it => it.UseCard());
        health = 5;
        maxHealth = health;
        gold = 0;
        IncreaseGold(0);
        TakeDamage(0);
    }

    private void Start()
    {
       PrepareWave();
    }

    private void NewTurn()
    {
        ResetSlots();
        List<string> keys = DeckManager.PickCards();
        List<Card> cards = new List<Card>();
        foreach (var key in keys)
        {
            RectTransform go = Instantiate(emptyGameObject, cardsLayout);
            cards.Add(Instantiate(cardPrefab, go).setup(key, go));
        }

        StartCoroutine(nameof(DisplayCards), cards);

    }

    private IEnumerator DisplayCards(List<Card> cards)
    {
        foreach (var card in cards)
        {
            card.Show();
            yield return Helpers.getWait(0.2f);
        }   
    }

    

    public void PrepareWave()
    {
        StartCoroutine(nameof(NewWave));
    }

    private IEnumerator NewWave()
    {
        ResetSlots();
        WaveManager.IncreaseWave();
        DeckManager.ResetPiles();
        WaveDisplay.instance.DisplayWaveIndicator(true);
        yield return Helpers.getWait(1f);
        WaveDisplay.instance.MovePlayerIndicator();
        yield return Helpers.getWait(1f);
        WaveDisplay.instance.HideWaveIndicator();
        yield return Helpers.getWait(1f);
        EnnemyManager.SpawnEnnemies();
        NewTurn();
    }

    private void ResetSlots()
    {
        setSelectedCard(null);
        cardHolders.ForEach(it => it.UseCard());
    }
    

    public static void WaveOver()
    {
        Debug.Log("Wave Over");
        instance.StartCoroutine(nameof(onWaveOver));
    }

    private IEnumerator onWaveOver()
    {
        Card[] cards = cardsLayout.GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.Hide();
            yield return Helpers.getWait(0.2f);
        }

        yield return Helpers.getWait(1f);
        foreach (var card in cards)
        {
            Destroy(card);
        }
        instance.upgradesPanel.DisplayUpgrades();
    }

    public static void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0) health = 0;
        instance.healthDisplay.SetText($"{health}/{maxHealth}");
        if (health <= 0) Death();
    }

    private static void Death()
    {
        Helpers.ReloadScene();
    }

    public static void IncreaseGold(int amount)
    {
        gold += amount;
        instance.goldDisplay.SetText(gold.ToString());
    }

}
