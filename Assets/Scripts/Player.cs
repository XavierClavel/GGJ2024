using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
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
    public static float infoPanelPosVisible = 360;
    public static float infoPanelPosHidden = 900f;

    public RectTransform infoPanel;
    public TextMeshProUGUI infoText;

    public RectTransform recipePanel;
    public RecipeDisplay recipeDisplay;
    public TextMeshProUGUI recipeText;

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
    [SerializeField] private Button upgradesDisplayButton;
    [SerializeField] private RectTransform upgradesDisplayTransform;
    private bool isPauseMenuActive = false;
    private Sequence sequenceDisplayRecipePanel;

    public static void Heal()
    {
        AudioManager.PlaySfx("Apple");
        health = maxHealth;
        TakeDamage(0);
    }

    public static void ShowInfoPanel()
    {
        instance.infoText.SetText("Welcome to the shop !");
        instance.infoPanel.DOAnchorPosY(infoPanelPosVisible, 1f).SetEase(Ease.InOutQuad);
    }
    
    public static void HideInfoPanel()
    {
        instance.infoPanel.DOAnchorPosY(infoPanelPosHidden, 1f).SetEase(Ease.InOutQuad);
    }
    
    
    public static void ShowRecipePanel(string text = "")
    {
        instance.recipeText.gameObject.SetActive(text != "");
        instance.recipeText.SetText(text);
        instance.sequenceDisplayRecipePanel?.Kill();
        instance.sequenceDisplayRecipePanel = DOTween.Sequence()
            .Append(instance.recipePanel.DOAnchorPosY(infoPanelPosVisible, 1f).SetEase(Ease.InOutQuad))
            .AppendCallback(delegate { Debug.Log("Moving down ok"); })
            .AppendInterval(3f)
            .AppendCallback(delegate { Debug.Log("Wait ok"); })
            .Append(instance.recipePanel.DOAnchorPosY(infoPanelPosHidden, 1f).SetEase(Ease.InOutQuad))
            .AppendCallback(delegate { Debug.Log("sequence completed"); })
            .Play();
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
        Merchant.instance.isShopActive = true;
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(delegate { WaveDisplay.instance.DisplayWaveIndicator(true); });
        sequence.AppendInterval(1f);
        sequence.AppendCallback(WaveDisplay.instance.MovePlayerIndicator);
        sequence.AppendInterval(1f);
        sequence.AppendCallback(WaveDisplay.instance.HideWaveIndicator);
        sequence.AppendCallback(Merchant.instance.Show);
        sequence.AppendInterval(1.5f);
        sequence.AppendCallback(Merchant.instance.SpawnShop);
        sequence.AppendCallback(delegate
        {
            Sequence objSequence = DOTween.Sequence();
            foreach (var consumable in Merchant.instance.consumables)
            {
                consumable.rectTransform.anchoredPosition += 800f * Vector2.up;
                objSequence.AppendCallback(delegate
                {
                    consumable.rectTransform.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuad);
                });
                objSequence.AppendInterval(0.2f);
            }

            objSequence.Play();
        });
        sequence.AppendInterval(1f);
        sequence.AppendCallback(ShowInfoPanel);
        sequence.AppendInterval(0.5f);
        sequence.Append(Merchant.instance.buttonTransform.DOAnchorPosX(Merchant.instance.posVisible, 1f)
            .SetEase(Ease.InOutQuad));
        sequence.Play();
    }

    public void HideShop()
    {
        Sequence s = DOTween.Sequence();
        s.Append(Merchant.instance.buttonTransform.DOAnchorPosX(Merchant.instance.posHidden, 1f)
            .SetEase(Ease.InOutQuad));
        AudioManager.PlaySfx("Merchant_Bye");
        s.AppendCallback(HideInfoPanel);
        s.AppendInterval(0.5f);
        foreach (var consumable in Merchant.instance.consumables)
        {
            s.AppendCallback(delegate
            {
                consumable.rectTransform.DOAnchorPosY(800f, 1f);
            });
            s.AppendInterval(0.2f);
        }
        s.AppendInterval(0.5f);
        s.AppendCallback(Merchant.instance.Hide);
        s.AppendInterval(0.5f);
        s.AppendCallback(Merchant.instance.DespawnShop);
        s.AppendCallback(WaveOver);
        s.Play();
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
        else DisplayButtonChoseUpgrades();
    }

    public void DisplayUpgrades()
    {
        upgradesDisplayButton.interactable = false;
        upgradesDisplayTransform.DOAnchorPosY(1000f, 1f);
        instance.upgradesPanel.DisplayUpgrades();
    }

    private void DisplayButtonChoseUpgrades()
    {
        upgradesDisplayButton.interactable = true;
        upgradesDisplayTransform.DOAnchorPosY(600f, 1f);
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
        AudioManager.PlaySfx("Loose");
        if (AudioManager.playingBossMusic)
        {
            AudioManager.playMainMusic();
        }
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
        AudioManager.playMainMusic();
        winScreen.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuad);
    }

}
