using System;
using System.Collections;
using System.Collections.Generic;
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
    public static Merchant instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnShop()
    {
        RectTransform go = Instantiate(emptyGameObject, consumablesLayout);
        Consumable consumable = Instantiate(prefabConsumable, go);
        SetupHealConsumable(consumable);
        consumable.setSlot(go);
    }

    void SetupHealConsumable(Consumable consumable)
    {
        consumable.setup(healSprite, Player.Heal);
    }

    void SetupHarpConsumable(Consumable consumable)
    {
        consumable.setup(harpSprite, Ennemy.IncreasePatience);
    }

    void SetupMandolinConsumable(Consumable consumable)
    {
        consumable.setup(mandolinSprite, Ennemy.ReduceEmotions);
    }

    void SetupTambourinConsumable(Consumable consumable)
    {
        consumable.setup(tambourinSprite, delegate {  });
    }
    
}
