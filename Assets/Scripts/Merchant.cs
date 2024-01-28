using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Sprite harpSprite;
    [SerializeField] private Sprite tambourinSprite;
    [SerializeField] private Sprite mandolinSprite;
    [SerializeField] private Consumable prefabConsumable;
    [SerializeField] private RectTransform emptyGameObject;
    [SerializeField] private RectTransform consumablesLayout;
    [SerializeField] private RectTransform merchantTransform;
    private List<Consumable> consumables;
    public static Merchant instance;
    private float posHidden = 1100f;
    private float posVisible = 650f;

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
        Player.instance.HideShop();
    }

    public void DespawnShop()
    {
        Debug.Log("Despawn shop");
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
            .setCost(50);
    }

    void SetupHarpConsumable()
    {
        setupConsumable()
            .setup(harpSprite, Ennemy.IncreasePatience)
            .setCost(50);
    }

    void SetupMandolinConsumable()
    {
        setupConsumable()
            .setup(mandolinSprite, Ennemy.ReduceEmotions)
            .setCost(50);
    }

    void SetupTambourinConsumable()
    {
        setupConsumable()
            .setup(tambourinSprite, delegate {  })
            .setCost(50);
    }

    public void Buy(Consumable consumable)
    {
        consumables.Remove(consumable);
        consumables.ForEach(it => it.updateCanBeDragged());
    }
    
}
