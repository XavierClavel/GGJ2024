using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Merchant : MonoBehaviour
{
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Sprite harpSprite;
    [SerializeField] private Sprite tambourinSprite;
    [SerializeField] private Sprite mandolinSprite;
    [SerializeField] private Consumable prefabConsumable;
    [SerializeField] private RectTransform emptyGameObject;
    [SerializeField] private RectTransform consumablesLayout;
    public RectTransform merchantTransform;
    public RectTransform buttonTransform;
    [HideInInspector] public List<Consumable> consumables;
    public static Merchant instance;
    [HideInInspector] public float posHidden = 1100f;
    [HideInInspector] public float posVisible = 650f;
    [HideInInspector] public bool isShopActive = false;

    private void Awake()
    {
        instance = this;
    }

    public void Show()
    {
        merchantTransform.DOAnchorPosX(posVisible, 1f).SetEase(Ease.InOutQuad);
    }

    public void Hide()
    {
        merchantTransform.DOAnchorPosX(posHidden, 1f).SetEase(Ease.InOutQuad);
    }

    public void SpawnShop()
    {
        consumables = new List<Consumable>();
        SetupHealConsumable();
        SetupHarpConsumable();
        SetupMandolinConsumable();
        SetupTambourinConsumable();
        
    }

    public void HideShop()
    {
        if (!isShopActive) return;
        isShopActive = false;
        Player.instance.HideShop();
    }

    public void DespawnShop()
    {
        consumablesLayout.KillAllChildren();
    }

    Consumable setupConsumable()
    {
        RectTransform go = Instantiate(emptyGameObject, consumablesLayout);
        Consumable consumable = Instantiate(prefabConsumable, go);
        consumable.setSlot(go);
        consumables.Add(consumable);
        return consumable;
    }

    void SetupHealConsumable()
    {
        setupConsumable()
            .setup(healSprite, Player.Heal)
            .setCost(5)
            .setText("Restore max health.");
    }

    void SetupHarpConsumable()
    {
        setupConsumable()
            .setup(harpSprite, Ennemy.IncreasePatience)
            .setCost(7)
            .setText("Increase patience of everyone by one.");
    }

    void SetupMandolinConsumable()
    {
        setupConsumable()
            .setup(mandolinSprite, Ennemy.ReduceEmotions)
            .setCost(6)
            .setText("Reduce emotions of everyone by one.");
    }

    void SetupTambourinConsumable()
    {
        setupConsumable()
            .setup(tambourinSprite, Ennemy.ApplyTambourin)
            .setCost(8)
            .setText("+2 emotion for closest people, -2 for farthest people.");
    }

    public void Buy(Consumable consumable)
    {
        consumables.Remove(consumable);
        consumables.ForEach(it => it.updateCanBeDragged());
    }
    
}
