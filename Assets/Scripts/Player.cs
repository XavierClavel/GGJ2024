using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class Player : MonoBehaviour
{
    public static Player instance;
    private static Card selectedCard;
    private static Consumable selectedConsumable;
    [SerializeField] private List<CardHolder> cardHolders;
    private static int health;
    private static int maxHealth;
    private static int gold;
    public static float infoPanelPosVisible = 450f;
    public static float infoPanelPosHidden = 900f;

    public RectTransform infoPanel;
    public TextMeshProUGUI infoText;

    public RectTransform recipePanel;
    public RecipeDisplay recipeDisplay;

    [SerializeField] private TextMeshProUGUI pickPileDisplay;
    [SerializeField] private TextMeshProUGUI discardPileDisplay;
    [SerializeField] private TextMeshProUGUI goldDisplay;
    [SerializeField] private TextMeshProUGUI healthDisplay;
    [SerializeField] private Transform cardsLayout;
    [SerializeField] private Card cardPrefab;
    [SerializeField] private UpgradesManager upgradesPanel;
    [SerializeField] private RectTransform emptyGameObject;
    [SerializeField] private RectTransform pauseMenu;
    [SerializeField] private RectTransform winScreen;
    private bool isPauseMenuActive = false;

    public static void Heal()
    {
        AudioManager.PlaySfx("Apple");
        health = maxHealth;
        TakeDamage(0);
    }

    public static void ShowInfoPanel()
    {
        instance.infoText.SetText("");
        instance.infoPanel.DOAnchorPosY(infoPanelPosVisible, 1f).SetEase(Ease.InOutQuad);
    }
    
    public static void HideInfoPanel()
    {
        instance.infoPanel.DOAnchorPosY(infoPanelPosHidden, 1f).SetEase(Ease.InOutQuad);
    }
    
    public static void ShowRecipePanel()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(instance.recipePanel.DOAnchorPosY(infoPanelPosVisible, 1f).SetEase(Ease.InOutQuad));
        sequence.AppendInterval(2f);
        sequence.Append(instance.recipePanel.DOAnchorPosY(infoPanelPosHidden, 1f).SetEase(Ease.InOutQuad));
        sequence.Play();
    }

    
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
        health += 2;
        TakeDamage(0);
    }

    public static Card getSelectedCard() => selectedCard;
    public static void setSelectedCard(Card card)
    {
        selectedCard = card;
    }

    public static Consumable getSelectedConsumable() => selectedConsumable;

    public static void setSelectedConsumable(Consumable consumable)
    {
        selectedConsumable = consumable;
    }

    public void ValidateCombination()
    {
        List<string> keys = new List<string>();
        foreach (var cardHolder in cardHolders)
        {
            Card card = (Card)cardHolder.selectedDraggable;
            if (card == null) continue;
            string key = card.getCardInfo().getKey();
            keys.Add(key);
        }

        if (keys.isEmpty()) return;

        Recipe recipe = RecipeManager.getRecipe(keys);
        if (recipe == null) return;
        
        keys.ForEach(DeckManager.UseCard);

        Ennemy[] ennemies = new Ennemy[Ennemy.ennemiesList.Count];
        Ennemy.ennemiesList.CopyTo(ennemies);
        foreach (var ennemy in ennemies)
        {
            ennemy.ApplyEffect(recipe);
        }

        foreach (var cardHolder in cardHolders)
        {
            cardHolder.UseDraggable();
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
        selectedConsumable = null;
        cardHolders.ForEach(it => it.UseDraggable());
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if (isPauseMenuActive)
        {
            pauseMenu.DOAnchorPosY(UpgradesManager.hiddenPos, 1f)
                .SetEase(Ease.InOutQuad);
        }
        else
        {
            pauseMenu.DOAnchorPosY(UpgradesManager.visiblePos, 1f)
                .SetEase(Ease.InOutQuad);
        }

        isPauseMenuActive = !isPauseMenuActive;
    }

    private void ShowShop()
    {
        StartCoroutine(nameof(showShop));
    }

    public void HideShop()
    {
        StartCoroutine(nameof(hideShop));
    }

    private IEnumerator showShop()
    {
        
        WaveDisplay.instance.DisplayWaveIndicator(true);
        yield return Helpers.getWait(1f);
        WaveDisplay.instance.MovePlayerIndicator();
        yield return Helpers.getWait(1f);
        WaveDisplay.instance.HideWaveIndicator();
        yield return Helpers.getWait(1f);
        ShowInfoPanel();
        Merchant.instance.Show();
        yield return Helpers.getWait(1.5f);
        Merchant.instance.SpawnShop();
    }

    private IEnumerator hideShop()
    {
        Merchant.instance.DespawnShop();
        yield return Helpers.getWait(0.5f);
        Merchant.instance.Hide();
        yield return Helpers.getWait(0.5f);
        HideInfoPanel();
        yield return Helpers.getWait(1.5f);
        WaveOver();
    }

    private void NewTurn()
    {
        ResetSlots();
        List<string> keys = DeckManager.PickCards();
        List<Card> cards = new List<Card>();
        foreach (var key in keys)
        {
            RectTransform go = Instantiate(emptyGameObject, cardsLayout);
            //go.eulerAngles = Random.Range(-5f, 5f) * Vector3.forward;
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
        ResetSlots();
        DeckManager.ResetPiles();
        WaveManager.IncreaseWave();
        if (WaveManager.isWaveShop())
        {
            ShowShop();
        } else StartCoroutine(nameof(NewWave));
    }

    private IEnumerator NewWave()
    {
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
        cardHolders.ForEach(it => it.UseDraggable());
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
        Debug.Log(WaveManager.getCurrentWave());
        if (WaveManager.getCurrentWave() == 20) Win();
        else instance.upgradesPanel.DisplayUpgrades();
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
        TransitionManager.TransitionToScene("SampleScene");
    }

    public static void IncreaseGold(int amount)
    {
        gold += amount;
        instance.goldDisplay.SetText(gold.ToString());
    }

    public static int getGold() => gold;

    public static void SpendGold(int amount)
    {
        gold -= amount;
        instance.goldDisplay.SetText(gold.ToString());
    }

    public void Win()
    {
        winScreen.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuad);
    }

}
